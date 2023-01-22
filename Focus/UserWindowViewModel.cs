using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

using static PInvoke.User32;

using Wpf.Ui.Common;
using PInvoke;

namespace Focus;
using static Focus.PInvokeHelpers;

public class UserWindowViewModel: INotifyPropertyChanged {
    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new(propertyName));

    internal UserWindowViewModel(nint handle) {
        _handle = handle;
        ResizeCommand = new RelayCommand(ExecuteResize);
        CenterCommand = new RelayCommand(ExecuteCenter);

        Name = string.Empty;
        Rect = default;
        UpdateProperties();
    }

    private nint _handle;

    public string    Name { get; private set; }
    public Int32Rect Rect { get; private set; }

    private void UpdateProperties() {
        Name = GetWindowText(_handle);
        GetClientRect(_handle, out var rect);
        Rect = new(
                rect.left,
                rect.top,
                rect.right  - rect.left,
                rect.bottom - rect.top);

        OnPropertyChanged(nameof(Name));
        OnPropertyChanged(nameof(Rect));
    }

    public ICommand ResizeCommand { get; }
    public ICommand CenterCommand { get; }

    private void ExecuteResize(object param) {
        var resolution = (WindowResolution)param;
        GetWindowRect(_handle, out var windowRect);
        GetClientRect(_handle, out var clientRect);
        SetWindowSize(
            _handle,
            (windowRect.right  - windowRect.left) -
            (clientRect.right  - clientRect.left) + resolution.Width,
            (windowRect.bottom - windowRect.top ) -
            (clientRect.bottom - clientRect.top ) + resolution.Height);
        UpdateProperties();
    }

    private void ExecuteCenter() {
        var hMonitor = MonitorFromWindow(
            _handle,
            MonitorOptions.MONITOR_DEFAULTTOPRIMARY);
        GetMonitorInfo(hMonitor, out var monitor);
        GetWindowRect (_handle , out var windowRect);
        SetWindowPosition(
            _handle,
            monitor.WorkArea.left
                + (monitor.WorkArea.right - monitor.WorkArea.left) / 2
                - (windowRect      .right - windowRect      .left) / 2,
            monitor.WorkArea.top
                + (monitor.WorkArea.bottom - monitor.WorkArea.top) / 2
                - (windowRect      .bottom - windowRect      .top) / 2);
        UpdateProperties();
    }
}
