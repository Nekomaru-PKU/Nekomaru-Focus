namespace Focus;

internal record Resolution(
    int AspectRatioX,
    int AspectRatioY,
    int Width,
    int Height) {
    public override string ToString() => string.Format(
        "{0} x {1} ({2}:{3})",
        Width, Height, AspectRatioX, AspectRatioY);

    public static readonly IEnumerable<Resolution> Presets =
        new Resolution[] {
            new(16,  9, 1920, 1080),
            new(16,  9, 1600,  900),
            new(16,  9, 1440,  810),
            new(16,  9, 1280,  720),

            new(16, 10, 1920, 1200),
            new(16, 10, 1600, 1000),
            new(16, 10, 1440,  900),
            new(16, 10, 1280,  800),
            
            new( 4,  3, 1920,  1440),
            new( 4,  3, 1600,  1200),
            new( 4,  3, 1440,  1080),
            new( 4,  3, 1280,  960),
        };
}
