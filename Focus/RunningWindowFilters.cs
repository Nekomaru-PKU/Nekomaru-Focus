using System.Linq;
using static PInvoke.User32;

namespace Focus;

internal delegate bool RunningWindowFilterCallback(RunningWindowInformation window);

internal static class RunningWindowFilters {
    public static readonly RunningWindowFilterCallback HasName =
        window => window.Name != string.Empty;
    public static readonly RunningWindowFilterCallback NotHidden =
        window => IsWindowVisible(window.Handle);
    public static readonly RunningWindowFilterCallback NotTooSmall =
        window =>
            window.Rect.Width >= WindowResolution.Presets.Select(o => o.Width).Min();
    public static readonly RunningWindowFilterCallback NotFullscreen =
        window =>
            window.Rect.Height < SystemInfo.DisplaySize.Height;
    public static readonly RunningWindowFilterCallback Default =
        window =>
            HasName(window) &&
            NotHidden(window) &&
            NotTooSmall(window) &&
            NotFullscreen(window);
}
