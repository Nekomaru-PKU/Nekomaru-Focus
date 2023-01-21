using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Common;

namespace Focus;
using static Focus.NativeExtentions;

public class UserWindowViewModel: INotifyPropertyChanged {
    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new(propertyName));

    internal UserWindowViewModel(UserWindowData o) {
        _data = o;
        ResizeCommand = new RelayCommand(ExecuteResize);
        CenterCommand = new RelayCommand(ExecuteCenter);
    }

    private UserWindowData _data;
    public string    Name => _data.Name;
    public Int32Rect Rect => _data.Rect;
    private void UpdateData() {
        _data = UserWindowDataProvider.FromHandle(_data.Handle);
        OnPropertyChanged(nameof(Name));
        OnPropertyChanged(nameof(Rect));
    }

    public ICommand ResizeCommand { get; }
    public ICommand CenterCommand { get; }

    private void ExecuteResize(object param) {
        var resolution = (WindowResolution)param;
        SetWindowSize(
            _data.Handle,
            resolution.Width,
            resolution.Height);
        UpdateData();
    }

    private void ExecuteCenter() {
        SetWindowPosition(
            _data.Handle,
            (SystemInfo.DisplaySize.Width  - _data.Rect.Width ) / 2,
            (SystemInfo.DisplaySize.Height - _data.Rect.Height) / 2);
        UpdateData();
    }
}
