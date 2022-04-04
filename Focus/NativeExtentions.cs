using System;
using System.Collections.Generic;
using static PInvoke.User32;

namespace Focus;

internal static class NativeExtentions {
    public static void SetWindowPosition(IntPtr hWnd, int x, int y) => 
        _ = SetWindowPos(
            hWnd, hWndInsertAfter: IntPtr.Zero, x, y, 0, 0, 
            SetWindowPosFlags.SWP_NOACTIVATE |
            SetWindowPosFlags.SWP_NOOWNERZORDER |
            SetWindowPosFlags.SWP_NOSIZE |
            SetWindowPosFlags.SWP_NOZORDER);

    public static void SetWindowSize(IntPtr hWnd, int cx, int cy) =>
        _ = SetWindowPos(
            hWnd, hWndInsertAfter: IntPtr.Zero, 0, 0, cx, cy,
            SetWindowPosFlags.SWP_NOACTIVATE |
            SetWindowPosFlags.SWP_NOMOVE |
            SetWindowPosFlags.SWP_NOOWNERZORDER |
            SetWindowPosFlags.SWP_NOZORDER);

    public static IEnumerable<IntPtr> EnumerateWindows() {
        var list = new List<IntPtr>();
        EnumWindows((hWnd, _) => {
            list.Add(hWnd);
            return true;
        }, IntPtr.Zero);
        return list;
    }
}
