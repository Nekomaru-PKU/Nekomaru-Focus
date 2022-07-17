using System.Windows;
using System.Windows.Controls;

namespace Focus;

public partial class MainWindow: Window {
    public MainWindow() {
        InitializeComponent();
        Activated += (_, _) => App.MainViewModel.ExecuteRefresh();
    }

    private void OnWindowResolutionDropdownClicked(object sender, RoutedEventArgs e) {
        var window = (UserWindowViewModel)((Button)sender).DataContext;
        var menu = new ContextMenu();
        foreach (var resolution in WindowResolution.Presets) {
            var item = new MenuItem {
                Header = resolution,
                Command = window.ResizeCommand,
                CommandParameter = resolution
            };
            menu.Items.Add(item);
        }
        menu.IsOpen = true;
    }
}
