using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Common;

namespace Focus;

public class MainWindowViewModel: DependencyObject {
    public ICommand RefreshCommand { get; }

    public static readonly DependencyProperty WindowsFilterProperty =
        DependencyProperty.Register(
            nameof(WindowsFilter),
            typeof(string),
            typeof(MainWindow),
            new PropertyMetadata(
                string.Empty,
                (d, _) => ((MainWindowViewModel)d).UpdateWindowsFiltered()));
    public string WindowsFilter {
        get => (string)GetValue(WindowsFilterProperty);
        set => SetValue(WindowsFilterProperty, value);
    }

    public ObservableCollection<NativeWindowViewModel> WindowsFiltered { get; private set; } = new();

    private List<NativeWindowViewModel> Windows { get; } = new();

    public MainWindowViewModel() {
        RefreshCommand = new RelayCommand(ExecuteRefresh);
    }

    public void ExecuteRefresh() {
        var windows = NativeWindowMethods.EnumerateForegroundWindows();
        Windows.Clear();
        Windows.AddRange(from window in windows select new NativeWindowViewModel(window));
        UpdateWindowsFiltered();
    }

    private void UpdateWindowsFiltered() {
        WindowsFiltered.Clear();
        var filtered = Windows.Where(
            o => o.Name.ToLower().Contains(WindowsFilter.ToLower()));
        foreach (var o in filtered)
            WindowsFiltered.Add(o);
    }
}
