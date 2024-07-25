using Godot;
using System;
using System.Text;

namespace FPS
{
	public partial class BulletDisplayer : Label
	{
		[Export]
		public FPSWeaponHolder weaponHolder;

		public override void _Process(double delta)
		{
			int currentAmmo = weaponHolder.currentWeapon.getCurrentAmmoInMagazine();
			if (currentAmmo >= 0) Text = repeat("|", currentAmmo);
			else Text = "∞";
		}

		public static string repeat(string text, int n)
		{
			var textAsSpan = text.AsSpan();
			var span = new Span<char>(new char[textAsSpan.Length * (int)n]);
			for (var i = 0; i < n; i++)
			{
				textAsSpan.CopyTo(span.Slice((int)i * textAsSpan.Length, textAsSpan.Length));
			}

			return span.ToString();
		}
	}
}