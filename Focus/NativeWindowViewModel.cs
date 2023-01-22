using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

using Wpf.Ui.Common;

using static PInvoke.User32;

namespace Focus;

public class NativeWindowViewModel: INotifyPropertyChanged {
    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new(propertyName));

    internal NativeWindowViewModel(nint handle) {
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
        var resolution = (Resolution)param;
        NativeWindowMethods.ResizeClient(
            _handle,
            resolution.Width,
            resolution.Height);
        UpdateProperties();
    }

    private void ExecuteCenter() {
        NativeWindowMethods.CenterToScreen(_handle);
        UpdateProperties();
    }
}
