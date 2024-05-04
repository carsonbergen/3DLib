using Godot;
using System;

public partial class Scope : Control
{
    public override void _Draw()
    {
		Vector2 pos = new Vector2(GetViewportRect().Size.X/2, GetViewportRect().Size.Y/2);
        DrawArc(pos, GetViewportRect().Size.Y/2 * 0.75f, 0f, 365f, 365, Colors.White, 2, true);
    }
}