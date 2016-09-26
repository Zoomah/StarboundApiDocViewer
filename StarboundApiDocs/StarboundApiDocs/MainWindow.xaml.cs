using System;
using System.Windows;
using SWF = System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Windows.Input;
using System.Timers;

namespace StarboundApiDocViewer {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		/// <summary>
		/// Configuration
		/// </summary>
		private Config config;
		/// <summary>
		/// Timer for <see cref="searchBox"/>
		/// </summary>
		private Timer timer;
		/// <summary>
		/// Timer interval for <see cref="timer"/>
		/// </summary>
		private int interval = 500;

		/// <summary>
		/// MainWindow constructor
		/// </summary>
    public MainWindow() {
      InitializeComponent();
			// load config data
			config = Config.Load();
			// check if Starbound folder is set
      if (config.StarboundFolder != null) { // YES, folder is set
				// set window pos and size
				Left = config.WinPosX;
        Top = config.WinPosY;
        Height = config.WinHeight;
        Width = config.WinWidth;
				// set splitter pos, keeping it dynamic with Star-Unit
				leftCol.Width = new GridLength(config.SplitterPosition, GridUnitType.Star);
				rightCol.Width = new GridLength(1-config.SplitterPosition, GridUnitType.Star);
				// set maximized if needed
				if (config.WinMaximized)
          WindowState = WindowState.Maximized;
				// initialize MD file list
        InitMdFiles();
      } else { // NO, folder is not set
				// set default values to config
				config.WinPosX = Left;
        config.WinPosY = Top;
        config.WinHeight = Height;
        config.WinWidth = Width;
				// search for Starbound path
        FindStarboundPath();
      }
			// setup the timer
			timer = new Timer(interval);
			timer.AutoReset = false;
			timer.Elapsed += KeyDownTimer;
    }

		/// <summary>
		/// initializes the document list
		/// </summary>
		private void InitMdFiles() {
			try {
				// search for md files in '(starbound)/doc/lua' and sort them
				var files = Directory.GetFiles(Path.Combine(config.StarboundFolder, "doc", "lua"), "*.md");
				Array.Sort(files);
				// clear file list (just in case) and add the new entries for the files
				mdList.Items.Clear();
				foreach (var file in files)
					mdList.Items.Add(new { FullPath = file, DisplayName = Path.GetFileName(file) });

				// select the first file
				mdList.SelectedIndex = 0;
			} catch (Exception) {
				// okay... something above went horribly wrong, let's at least make a statement to the user
				Title = "Starbound API Documentation Viewer";
				mdCurrent.Content = "no file selected";
				mdView.NavigateToString(@"<html><body><h3 style=""font-family: 'Segoe UI', Arial; margin-top: 100px; text-align: center"">No files found.</h3></body></html>");

				MessageBox.Show("Sorry. Couldn't find any Help files.", "No files found", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}
		
		/// <summary>
		/// Try to find the Starbound path
		/// </summary>
		private void FindStarboundPath() {
			// try to get the steampath, if it fails, ask for a directory
			var steam = GetSteamPath();
			if (steam == null) {
				FolderDialog();
				return;
			}
			// try to find Starbound within steam, if it fails... you know...
			var sbdir = Path.Combine(steam, "SteamApps", "common", "Starbound");
			if (!Directory.Exists(sbdir) || !File.Exists(Path.Combine(sbdir, "win32", "starbound.exe"))) {
				FolderDialog();
				return;
			}
			// seems we found an installation, but just to make sure, ask the user, if (s)he says no... well... yeah... again...
			var r = MessageBox.Show("Is this your Starbound installation?\n\n" + sbdir, "Found a Starbound folder in Steam", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
			if (r == MessageBoxResult.No) {
				FolderDialog();
				return;
			}
			// set the folder to the config and (re)init the md file list 
			config.StarboundFolder = sbdir;
			InitMdFiles();
		}

		/// <summary>
		/// Find the Steam installation path
		/// </summary>
		/// <returns>the full path of your steam installation as string or null, if it wasn't found</returns>
		private string GetSteamPath() {
			try {
				return RegistryKey
					.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32)
					.OpenSubKey("SOFTWARE").OpenSubKey("Valve").OpenSubKey("Steam")
					.GetValue("InstallPath").ToString();
			} catch (Exception) {
				return null;
			}
		}

		/// <summary>
		/// Ask the user to browse to the Starbound installation
		/// </summary>
		private void FolderDialog() {
			// create dialog
			var dialog = new SWF.FolderBrowserDialog();
			// setup dialog
			dialog.Description = "Please select your Starbound folder.";
			if (config.StarboundFolder != null)
				dialog.SelectedPath = config.StarboundFolder;
			dialog.ShowNewFolderButton = false;
			// start dialog
			if (SWF.DialogResult.OK == dialog.ShowDialog()) {
				// set the folder to the config and (re)init the md file list 
				config.StarboundFolder = dialog.SelectedPath;
				InitMdFiles();
			}
		}

		/// <summary>
		/// Event handler:
		/// Selection in Listbox has changed.
		/// </summary>
		/// <param name="sender">a sender</param>
		/// <param name="e">event args</param>
		private void FileSelected(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
			// get current entry
			var entry = mdList.SelectedItem;
			if (entry != null) {
				// adjust titlebar and file label
				SetCurrent(getProperty(entry, "DisplayName"));
				// load the MD file
				LoadMdFile(getProperty(entry, "FullPath"));
			}
		}

		/// <summary>
		/// Sets the titlebar and the file label texts
		/// </summary>
		/// <param name="name">name of the current file without path</param>
		private void SetCurrent(string name) {
			Title = name + " - Starbound API Documentation Viewer";
			mdCurrent.Content = name;
		}

		/// <summary>
		/// Load the selected MD file into the viewer
		/// </summary>
		/// <param name="file">name of the current file with full path</param>
		private void LoadMdFile(string file) {
			// render the MD file into HTML and load it into the viewer
			mdView.NavigateToString(ViewTemplate.render(file));
		}

		/// <summary>
		/// Helper function:
		/// gets a property as string from a given object
		/// </summary>
		/// <param name="o">an object</param>
		/// <param name="property">a property name</param>
		/// <returns>value of the property as string</returns>
		private string getProperty(object o, string property) {
			return o?.GetType().GetProperty(property)?.GetValue(o, null).ToString();
		}

		/// <summary>
		/// Event handler:
		/// The directory select button was pressed.
		/// </summary>
		/// <param name="sender">a sender</param>
		/// <param name="e">event args</param>
		private void SelectSBFolder(object sender, RoutedEventArgs e) {
      FolderDialog();
    }

		/// <summary>
		/// Event handler:
		/// Keys are pressed with the webbrowser in focus.
		/// </summary>
		/// <param name="sender">a sender</param>
		/// <param name="e">event args</param>
		private void BeforeKeyDown(object sender, KeyEventArgs e) {
			// filter out Webbrowser hotkeys
			// any CTRL pressed?
			if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) {
				// CTRL + F --> to the searchbox
				if (e.Key == Key.F)
					GotoSearch(sender, null);
				// CTRL + C is okay, others not
				e.Handled = e.Key != Key.C;
			}
		}

		/// <summary>
		/// Event handler:
		/// Document is completely loaded.
		/// </summary>
		/// <param name="sender">a sender</param>
		/// <param name="e">event args</param>
		private void ViewLoaded(object sender, System.Windows.Navigation.NavigationEventArgs e) {
			// reset the zoom level
			zoomSlider.Value = 100;
		}

		/// <summary>
		/// Event handler:
		/// Search text has changed.
		/// </summary>
		/// <param name="sender">a sender</param>
		/// <param name="e">event args</param>
		private void SearchTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) {
			// show/hide clear button
			clearButton.Visibility = searchBox.Text.Length > 0 ? Visibility.Visible : Visibility.Hidden;
			// reset the timer
			timer.Stop();
			timer.Start();
		}

		/// <summary>
		/// Event handler:
		/// Timer has elapsed.
		/// </summary>
		/// <param name="sender">a sender</param>
		/// <param name="e">event args</param>
		private void KeyDownTimer(object sender, ElapsedEventArgs e) {
			// delegate the action into the correct thread
			App.Current.Dispatcher.Invoke((Action)delegate {
				// start searching
				Search(searchBox, null);
			});
		}

		/// <summary>
		/// Event handler:
		/// Zoom (-) button was pressed.
		/// </summary>
		/// <param name="sender">a sender</param>
		/// <param name="e">event args</param>
		private void ZoomOut(object sender, RoutedEventArgs e) {
			// decrease the soom a bit
			zoomSlider.Value -= zoomSlider.SmallChange;
		}

		/// <summary>
		/// Event handler:
		/// Zoomslider value has changed.
		/// </summary>
		/// <param name="sender">a sender</param>
		/// <param name="e">event args</param>
		private void zoomChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
			try {
				// call 'setZoom' in 'Template/script.js'
				mdView.InvokeScript("setZoom", e.NewValue);
			} catch (Exception) { }
		}

		/// <summary>
		/// Event handler:
		/// Zoom (+) button was pressed.
		/// </summary>
		/// <param name="sender">a sender</param>
		/// <param name="e">event args</param>
		private void ZoomIn(object sender, RoutedEventArgs e) {
			// increase the zoom a bit
			zoomSlider.Value += zoomSlider.SmallChange;
		}

		/// <summary>
		/// Event handler:
		/// Window wants to close.
		/// </summary>
		/// <param name="sender">a sender</param>
		/// <param name="e">event args</param>
		private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e) {
			// wait a sec...
			e.Cancel = true;
			// save window pos and size if not maximized
      if (WindowState != WindowState.Maximized) {
        config.WinPosX = Left;
        config.WinPosY = Top;
        config.WinHeight = Height;
        config.WinWidth = Width;
      }
			// save splitter position in %
      config.SplitterPosition = leftCol.ActualWidth/(leftCol.ActualWidth + rightCol.ActualWidth);
			// save the maximized state
      config.WinMaximized = (WindowState == WindowState.Maximized);
			// write config to file
      config.Save();
			// closing is okay now...
      e.Cancel = false;
    }

		/// <summary>
		///  Hotkey: Ctrl + F
		/// Context: <see cref="MainWindow"/>
		/// Jump to the searchbox.
		/// </summary>
		/// <param name="sender">a sender</param>
		/// <param name="e">event args</param>
		public void GotoSearch(object sender, ExecutedRoutedEventArgs e) {
			// focus and select the search box
			searchBox.Focus();
			searchBox.SelectAll();
		}

		/// <summary>
		///  Hotkey: Enter
		/// Context: <see cref="searchBox"/>
		/// Start a search.
		/// </summary>
		/// <param name="sender">a sender</param>
		/// <param name="e">event args</param>
		public void Search(object sender, ExecutedRoutedEventArgs e) {
			timer.Stop();
			try {
				// call 'search' in 'Template/script.js'
				mdView.InvokeScript("search", searchBox.Text);
			} catch (Exception) { }
		}

		/// <summary>
		///  Hotkey: F3
		/// Context: <see cref="MainWindow"/>
		/// Jump to the next finding.
		/// </summary>
		/// <param name="sender">a sender</param>
		/// <param name="e">event args</param>
		public void SearchNext(object sender, ExecutedRoutedEventArgs e) {
			try {
				// call 'setCurrent' in 'Template/script.js'
				mdView.InvokeScript("setCurrent", 1);
			} catch (Exception) { }
		}

		/// <summary>
		///  Hotkey: Shift + F3
		/// Context: <see cref="MainWindow"/>
		/// Jump to the previous finding.
		/// </summary>
		/// <param name="sender">a sender</param>
		/// <param name="e">event args</param>
		public void SearchPrev(object sender, ExecutedRoutedEventArgs e) {
			try {
				// call 'setCurrent' in 'Template/script.js'
				mdView.InvokeScript("setCurrent", -1);
			} catch (Exception) { }
		}

		/// <summary>
		///  Hotkey: Esc
		/// Context: <see cref="MainWindow"/>
		/// Clear the searchbox and all findings.
		/// </summary>
		/// <param name="sender">a sender</param>
		/// <param name="e">event args</param>
		private void Clear(object sender, ExecutedRoutedEventArgs e) {
			searchBox.Text = "";
			Search(sender, e);
		}
	}
}
