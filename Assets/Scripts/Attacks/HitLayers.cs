using System;
[Flags] public enum HitLayer {
    None = 0, All = ~0,
    Player = 1 << 0,
    Enemy = 1 << 1
}