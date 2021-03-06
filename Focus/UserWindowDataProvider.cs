using System;
using static PInvoke.User32;

namespace Focus;

internal static class UserWindowDataProvider {
    public static UserWindowData FromHandle(IntPtr handle) {
        GetWindowRect(handle, out var rect);
        return new() {
            Handle = handle,
            Name   = GetWindowText(handle),
            Rect   = new(
                rect.left,
                rect.top,
                rect.right  - rect.left,
                rect.bottom - rect.top),
        };
    }
}
