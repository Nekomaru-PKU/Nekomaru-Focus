using System.Windows;

namespace Focus;

public partial class App : Application {
    public static MainWindowViewModel MainViewModel => ((App)Current)._mainViewModel;
    private readonly MainWindowViewModel _mainViewModel = new();
}
