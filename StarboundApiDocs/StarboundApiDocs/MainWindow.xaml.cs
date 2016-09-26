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
    private Config config;

		private Timer timer;

		private int interval = 500;

    public MainWindow() {
      InitializeComponent();
			config = Config.Load();

      if (config.StarboundFolder != null) {
        Left = config.WinPosX;
        Top = config.WinPosY;
        Height = config.WinHeight;
        Width = config.WinWidth;
				
				leftCol.Width = new GridLength(config.SplitterPosition, GridUnitType.Star);
				rightCol.Width = new GridLength(1-config.SplitterPosition, GridUnitType.Star);

				if (config.WinMaximized)
          WindowState = WindowState.Maximized;

        InitMdFiles();
      } else {
        config.WinPosX = Left;
        config.WinPosY = Top;
        config.WinHeight = Height;
        config.WinWidth = Width;

        FindStarboundPath();
      }

			timer = new Timer(interval);
			timer.AutoReset = false;
			timer.Elapsed += KeyDownTimer;
    }

		private void KeyDownTimer(object sender, ElapsedEventArgs e) {
			App.Current.Dispatcher.Invoke((Action)delegate {
				Search(searchBox, null);
			});
		}

		public void GotoSearch(object sender, ExecutedRoutedEventArgs e) {
			searchBox.Focus();
		}

		public void Search(object sender, ExecutedRoutedEventArgs e) {
			timer.Stop();
			try {
				mdView.InvokeScript("search", searchBox.Text);
			} catch (Exception) { }
		}

		public void SearchNext(object sender, ExecutedRoutedEventArgs e) {
			try {
				mdView.InvokeScript("setCurrent", 1);
			} catch (Exception) { }
		}

		public void SearchPrev(object sender, ExecutedRoutedEventArgs e) {
			try {
				mdView.InvokeScript("setCurrent", -1);
			} catch (Exception) { }
		}

		private void FindStarboundPath() {
      var steam = GetSteamPath();
      if (steam == null) {
        FolderDialog();
        return;
      }

      var sbdir = Path.Combine(steam, "SteamApps", "common", "Starbound");
      if (!Directory.Exists(sbdir) || !File.Exists(Path.Combine(sbdir, "win32", "starbound.exe"))) {
        FolderDialog();
        return;
      }

      var r = MessageBox.Show("Is this your Starbound installation?\n\n" + sbdir, "Found a Starbound folder in Steam", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
      if (r == MessageBoxResult.No) {
        FolderDialog();
        return;
      }

      config.StarboundFolder = sbdir;
      InitMdFiles();
    }

		private void InitMdFiles() {
      try {
        var files = Directory.GetFiles(Path.Combine(config.StarboundFolder, "doc", "lua"), "*.md");
				Array.Sort(files);

				mdList.Items.Clear();
				foreach (var file in files)
          mdList.Items.Add(new { FullPath = file, DisplayName = Path.GetFileName(file)});

				if (mdList.HasItems) {
          mdList.SelectedIndex = 0;
        } else {
          throw new Exception();
        }
      } catch (Exception) {
				Title = "Starbound API Documentation Viewer";
				mdCurrent.Content = "no file selected";
				mdView.NavigateToString(@"<html><body><h3 style=""font-family: 'Segoe UI', Arial; margin-top: 100px; text-align: center"">No files found.</h3></body></html>");

				MessageBox.Show("Sorry. Couldn't find any Help files.", "No files found", MessageBoxButton.OK, MessageBoxImage.Warning);
      }
    }

    private void SetCurrent(string name) {
			Title = name + " - Starbound API Documentation Viewer";
			mdCurrent.Content = name;
    }

    private void LoadMdFile(string file) {
			mdView.NavigateToString(ViewTemplate.render(file));
    }

    private void SelectSBFolder(object sender, RoutedEventArgs e) {
      FolderDialog();
    }

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

    private void FolderDialog() {
      var dialog = new SWF.FolderBrowserDialog();
      dialog.Description = "Please select your Starbound folder.";
			if (config.StarboundFolder != null)
				dialog.SelectedPath = config.StarboundFolder;
      dialog.ShowNewFolderButton = false;
      if (SWF.DialogResult.OK == dialog.ShowDialog()) {
        config.StarboundFolder = dialog.SelectedPath;
        InitMdFiles();
      }
    }

    private void FileSelected(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
      var entry = mdList.SelectedItem;
      if (entry != null) {
        SetCurrent(getProperty(entry,"DisplayName"));
        LoadMdFile(getProperty(entry,"FullPath"));
      }
    }

    private string getProperty(object o, string property) {
      return o?.GetType().GetProperty(property)?.GetValue(o, null).ToString();
    }

    private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e) {
      if (WindowState != WindowState.Maximized) {
        config.WinPosX = Left;
        config.WinPosY = Top;
        config.WinHeight = Height;
        config.WinWidth = Width;
      }

      config.SplitterPosition = leftCol.ActualWidth/(leftCol.ActualWidth + rightCol.ActualWidth);

      config.WinMaximized = (WindowState == WindowState.Maximized);
      config.Save();

      e.Cancel = false;
    }

		private void ZoomDown(object sender, RoutedEventArgs e) {
			zoomSlider.Value -= zoomSlider.SmallChange;
		}

		private void ZoomUp(object sender, RoutedEventArgs e) {
			zoomSlider.Value += zoomSlider.SmallChange;
		}

		private void zoomChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
			try {
				mdView.InvokeScript("setZoom",e.NewValue);
			} catch (Exception) { }
		}

		private void SetZoom(double zoom) {
			zoomSlider.Value = zoom;
		}

		private void ViewLoaded(object sender, System.Windows.Navigation.NavigationEventArgs e) {
			SetZoom(100);
		}

		private void BeforeKeyDown(object sender, KeyEventArgs e) {
			if(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) {
				if (e.Key == Key.F) {
					searchBox.Focus();
					searchBox.SelectAll();
				}
				e.Handled = e.Key != Key.C;
			} else {
				e.Handled = true;
			}
		}

		private void SearchTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) {
			timer.Stop();
			timer.Start();
		}

		private void Clear(object sender, ExecutedRoutedEventArgs e) {
			searchBox.Text = "";
			Search(sender, e);
		}
	}
}
