using System;
using System.Collections.Generic;
using System.Linq;
using static PInvoke.User32;
using static PInvoke.User32.WindowLongIndexFlags;
using static PInvoke.User32.WindowStyles;
using static PInvoke.User32.WindowStylesEx;

namespace Focus;

internal static class UserWindowEnumerator {
    public static IEnumerable<nint> EnumerateUserWindows() {
        var windows = new List<IntPtr>();
        EnumWindows((hWnd, _) => {
            windows.Add(hWnd);
            return true;
        }, IntPtr.Zero);

        return from hWnd in windows
            where
                IsWindowVisible(hWnd) &&
                ! IsIconic(hWnd) &&
                ! IsZoomed(hWnd)

            // note:
            //     for modern apps and certain desktop apps (e.g. Telegram),
            //     (GetWindowLong(hWnd, GWL_STYLE) & (uint)WS_POPUP) != 0,
            //     so this condition is currently disabled.
            // let style = GetWindowLong(hWnd, GWL_STYLE)
            //     where (style & (uint)WS_POPUP) == 0

            let exStyle = GetWindowLong(hWnd, GWL_EXSTYLE)
                where
                    (exStyle & (uint)WS_EX_TOOLWINDOW) == 0 &&
                    (exStyle & (uint)WS_EX_NOACTIVATE) == 0 &&
                    (exStyle & (uint)WS_EX_TOOLWINDOW) == 0

            // note:
            //     here we temporarily exclude some windows by name,
            //     which are hard to filter out via trivial methods.
            let name = GetWindowText(hWnd)
                where ! _excludedWindowNames.Contains(name)
            select hWnd;
    }

    private static readonly IEnumerable<string> _excludedWindowNames = new string[] {
        // processName: exlorer.exe
        //   className: ApplicationFrameWindow
        "",

        // processName: TextInputHost.exe
        //   className: Windows.UI.Core.CoreWindow
        "Microsoft Text Input Application"
    };
}
