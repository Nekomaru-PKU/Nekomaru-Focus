using System;
using System.Collections.Generic;
using static PInvoke.User32;

namespace Focus;

internal static class RunningWindowEnumerator {
    public static IEnumerable<RunningWindowInformation> Enumerate() {
        var windows = new List<RunningWindowInformation>();
        EnumWindows((handle, _) => {
            windows.Add(RunningWindowInformationFactory.FromHandle(handle));
            return true;
        }, IntPtr.Zero);
        return windows;
    }
}
