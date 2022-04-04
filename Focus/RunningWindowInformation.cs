using System;

namespace Focus;

using WindowRect = System.Drawing.Rectangle;

public record RunningWindowInformation(
    IntPtr      Handle,
    string      Name,
    WindowRect  Rect);
