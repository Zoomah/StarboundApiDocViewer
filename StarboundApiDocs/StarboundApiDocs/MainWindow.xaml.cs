using System;
using System.Windows;
using SWF = System.Windows.Forms;
using Microsoft.Win32;
using System.IO;

namespace StarboundApiDocs {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
    private Config config;

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

        FillFileList();
      } else {
        config.WinPosX = Left;
        config.WinPosY = Top;
        config.WinHeight = Height;
        config.WinWidth = Width;

        FindStarboundPath();
      }
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
      FillFileList();
    }

    private void FillFileList() {
      try {
        var path = Path.Combine(config.StarboundFolder, "doc", "lua");
        var files = Directory.GetFiles(path,"*.md");
        Array.Sort(files);
        foreach (var file in files)
          mdList.Items.Add(new { FullPath = file, DisplayName = Path.GetFileName(file)});
        if (files.Length > 0) {
          mdList.SelectedIndex = 0;
        } else {
          throw new Exception();
        }
      } catch (Exception) {
        MessageBox.Show("Sorry. Couldn't find any Help files.", "No files found", MessageBoxButton.OK, MessageBoxImage.Warning);
      }
    }

    private void SetCurrent(string name) {
			Title = name + " - Starbound API Documentation Viewer";
			mdCurrent.Content = name;
    }

    private void LoadMdFile(string file) {
      mdView.NavigateToString(Renderer.fromFile(file));
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
      dialog.ShowNewFolderButton = false;
      if (SWF.DialogResult.OK == dialog.ShowDialog()) {
        config.StarboundFolder = dialog.SelectedPath;
        FillFileList();
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
  }
}
