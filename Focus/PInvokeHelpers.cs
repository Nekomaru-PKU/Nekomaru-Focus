using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;

using PInvoke;
using static PInvoke.User32;
using static PInvoke.User32.SetWindowPosFlags;

namespace Focus;

internal static class PInvokeHelpers {
    public static void SetWindowPosition(IntPtr hWnd, int x, int y) => 
        _ = SetWindowPos(
            hWnd,
            hWndInsertAfter: IntPtr.Zero,
            x, y, 0, 0, 
            SWP_NOACTIVATE |
            SWP_NOOWNERZORDER |
            SWP_NOSIZE |
            SWP_NOZORDER);

    public static void SetWindowSize(IntPtr hWnd, int cx, int cy) =>
        _ = SetWindowPos(
            hWnd,
            hWndInsertAfter: IntPtr.Zero,
            0, 0, cx, cy,
            SWP_NOACTIVATE |
            SWP_NOMOVE |
            SWP_NOOWNERZORDER |
            SWP_NOZORDER);
}
