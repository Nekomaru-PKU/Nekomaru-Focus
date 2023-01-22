using static PInvoke.User32;
using static PInvoke.User32.SetWindowPosFlags;
using static PInvoke.User32.WindowLongIndexFlags;
using static PInvoke.User32.WindowStyles;
using static PInvoke.User32.WindowStylesEx;

namespace Focus;

internal static class NativeWindowMethods {
    internal static void ResizeClient(nint hWnd, int width, int height) {
        GetWindowRect(hWnd, out var windowRect);
        GetClientRect(hWnd, out var clientRect);

        var oldWindowWidth  = windowRect.right  - windowRect.left;
        var oldWindowHeight = windowRect.bottom - windowRect.top;
        var oldClientWidth  = clientRect.right  - clientRect.left;
        var oldClientHeight = clientRect.bottom - clientRect.top;

        var newWindowWidth  = oldWindowWidth  - oldClientWidth  + width;
        var newWindowHeight = oldWindowHeight - oldClientHeight + height;
        SetWindowPos(
            hWnd,
            hWndInsertAfter: IntPtr.Zero,
            windowRect.left + oldWindowWidth  / 2 - newWindowWidth  / 2,
            windowRect.top  + oldWindowHeight / 2 - newWindowHeight / 2,
            newWindowWidth,
            newWindowHeight,
            SWP_NOACTIVATE |
            SWP_NOOWNERZORDER |
            SWP_NOZORDER);
    }

    internal static void CenterToScreen(nint hWnd) {
        var hMonitor = MonitorFromWindow(
            hWnd,
            MonitorOptions.MONITOR_DEFAULTTOPRIMARY);
        GetMonitorInfo(hMonitor, out var monitor);
        GetWindowRect (hWnd    , out var windowRect);
        SetWindowPos  (
            hWnd,
            hWndInsertAfter: IntPtr.Zero,
            monitor.WorkArea.left
                + (monitor.WorkArea.right - monitor.WorkArea.left) / 2
                - (windowRect      .right - windowRect      .left) / 2,
            monitor.WorkArea.top
                + (monitor.WorkArea.bottom - monitor.WorkArea.top) / 2
                - (windowRect      .bottom - windowRect      .top) / 2,
            0,
            0, 
            SWP_NOACTIVATE |
            SWP_NOOWNERZORDER |
            SWP_NOSIZE |
            SWP_NOZORDER);
    }

    internal static IEnumerable<nint> EnumerateForegroundWindows() {
        var windows = new List<nint>();
        EnumWindows((Window, _) => {
            windows.Add(Window);
            return true;
        }, (nint)0);

        return from Window in windows
            where
                IsWindowVisible(Window) &&
                ! IsIconic(Window) &&
                ! IsZoomed(Window)

            // note:
            //     for modern apps and certain desktop apps (e.g. Telegram),
            //     (GetWindowLong(Window, GWL_STYLE) & (uint)WS_POPUP) != 0,
            //     so this condition is currently disabled.
            // let style = GetWindowLong(Window, GWL_STYLE)
            //     where (style & (uint)WS_POPUP) == 0

            let exStyle = GetWindowLong(Window, GWL_EXSTYLE)
                where
                    (exStyle & (uint)WS_EX_TOOLWINDOW) == 0 &&
                    (exStyle & (uint)WS_EX_NOACTIVATE) == 0 &&
                    (exStyle & (uint)WS_EX_TOOLWINDOW) == 0

            // note:
            //     here we temporarily exclude some windows by name,
            //     which are hard to filter out via trivial methods.
            let name = GetWindowText(Window)
                where
                    // processName: exlorer.exe
                    //   className: ApplicationFrameWindow
                    name != "" &&

                    // processName: TextInputHost.exe
                    //   className: Windows.UI.Core.CoreWindow
                    name != "Microsoft Text Input Application"
            select Window;
    }
}
