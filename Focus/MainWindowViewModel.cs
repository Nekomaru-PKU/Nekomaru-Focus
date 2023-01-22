using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using static PInvoke.User32;

namespace Focus;

public class MainWindowViewModel: DependencyObject {
    public ICommand RefreshCommand { get; }

    public static readonly DependencyProperty WindowsFilterProperty =
        DependencyProperty.Register(
            nameof(WindowsFilter),
            typeof(string),
            typeof(MainWindowViewModel),
            new PropertyMetadata(string.Empty, OnWindowsFilterChanged));
    public string WindowsFilter {
        get => (string)GetValue(WindowsFilterProperty);
        set => SetValue(WindowsFilterProperty, value);
    }
    private static void OnWindowsFilterChanged(
        DependencyObject d,
        DependencyPropertyChangedEventArgs e) {
        ((MainWindowViewModel)d).UpdateWindowsFiltered();
    }

    public ObservableCollection<UserWindowViewModel> WindowsFiltered { get; private set; } = new();

    private List<UserWindowViewModel> Windows { get; } = new();

    public MainWindowViewModel() {
        RefreshCommand = new RelayCommand(ExecuteRefresh);
    }

    public void ExecuteRefresh() {
        var windows = UserWindowEnumerator.EnumerateUserWindows();
        Windows.Clear();
        Windows.AddRange(from window in windows select new UserWindowViewModel(window));
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
