using System.Windows;

namespace Focus;

public partial class App : Application {
    public static MainWindowViewModel MainWindowViewModel => ((App)Current)._mainWindowViewModel;
    private readonly MainWindowViewModel _mainWindowViewModel = new();
}
