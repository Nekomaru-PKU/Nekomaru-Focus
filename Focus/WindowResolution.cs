using System.Collections.Generic;

namespace Focus;

internal record WindowResolution(
    int AspectRatioX,
    int AspectRatioY,
    int Width,
    int Height) {
    public override string ToString() => string.Format(
        "{0} x {1} ({2}:{3})",
        Width, Height, AspectRatioX, AspectRatioY);

    public static readonly IEnumerable<WindowResolution> Presets =
        new WindowResolution[] {
            new(16, 10, 1920, 1200),
            new(16, 10, 1440,  900),
            new(16, 10, 1280,  800),

            new(16,  9, 1920, 1080),
            new(16,  9, 1600,  900),
            new(16,  9, 1280,  720),
            new(16,  9,  640,  360),

            new( 4,  3, 1024,  768),
            new( 4,  3,  800,  600),
        };
}
