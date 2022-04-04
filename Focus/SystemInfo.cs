using System.Drawing;
using static PInvoke.User32;

namespace Focus;

internal static class SystemInfo {
    public static Size DisplaySize { get; } = new(
        GetSystemMetrics(SystemMetric.SM_CXFULLSCREEN),
        GetSystemMetrics(SystemMetric.SM_CYFULLSCREEN));
}
