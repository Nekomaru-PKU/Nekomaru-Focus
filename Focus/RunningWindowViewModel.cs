using System;
using System.Windows;
using System.Windows.Input;
using WPFUI.Common;
using static PInvoke.User32;

namespace Focus {
    public class RunningWindowViewModel: DependencyObject {
        public ICommand ResizeCommand { get; }
        public ICommand CenterCommand { get; }

        public RunningWindowInformation Model { get; set; }

        internal RunningWindowViewModel(RunningWindowInformation o) {
            Model = o;

            ResizeCommand = new RelayCommand(ExecuteResize);
            CenterCommand = new RelayCommand(ExecuteCenter);
        }

        private void ExecuteResize(object param) {
            var resolution = (WindowResolution)param;
            _ = SetWindowPos(
                hWnd:            Model.Handle,
                hWndInsertAfter: IntPtr.Zero,
                X: 0,
                Y: 0,
                cx: resolution.Width,
                cy: resolution.Height,
                SetWindowPosFlags.SWP_NOACTIVATE |
                SetWindowPosFlags.SWP_NOMOVE |
                SetWindowPosFlags.SWP_NOOWNERZORDER |
                SetWindowPosFlags.SWP_NOZORDER);
        }

        private void ExecuteCenter() {
            _ = SetWindowPos(
                hWnd:            Model.Handle,
                hWndInsertAfter: IntPtr.Zero,
                X: (SystemInfo.DisplaySize.Width - Model.Rect.Width) / 2,
                Y: (SystemInfo.DisplaySize.Height - Model.Rect.Height) / 2,
                cx: 0,
                cy: 0,
                SetWindowPosFlags.SWP_NOACTIVATE |
                SetWindowPosFlags.SWP_NOOWNERZORDER |
                SetWindowPosFlags.SWP_NOSIZE |
                SetWindowPosFlags.SWP_NOZORDER);
        }
    }
}
