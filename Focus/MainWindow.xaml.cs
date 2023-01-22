using System.Windows;
using System.Windows.Controls;

namespace Focus;

public partial class MainWindow: Window {
    public MainWindow() {
        InitializeComponent();
        Activated += (_, _) => App.MainWindowViewModel.ExecuteRefresh();
    }

    private void OnWindowResolutionDropdownClicked(object sender, RoutedEventArgs e) {
        var window = (NativeWindowViewModel)((Button)sender).DataContext;
        var menu = new ContextMenu();
        foreach (var resolution in Resolution.Presets) {
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
