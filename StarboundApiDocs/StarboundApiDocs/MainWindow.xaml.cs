using System.Windows;
using SWF = System.Windows.Forms;

namespace StarboundApiDocs {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();
      mdView.NavigateToString(Renderer.fromFile(@"C:\Games\Steam\SteamApps\common\Starbound\doc\lua\widget.md"));
    }

    private void SelectSBFolder(object sender, RoutedEventArgs e) {
      FolderDialog();
    }

    private static void FolderDialog() {
      var dialog = new SWF.FolderBrowserDialog();
      dialog.SelectedPath = @"C:\Games\Steam\SteamApps\common\Starbound";
      dialog.Description = "Please select your Starbound folder.";
      dialog.ShowNewFolderButton = false;
      if (SWF.DialogResult.OK == dialog.ShowDialog()) {
        // set new directory
        // load md-files
      }
    }
  }
}
