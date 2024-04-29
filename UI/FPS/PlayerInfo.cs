using Godot;
using ThreeDLib;

namespace FPS
{
	public partial class PlayerInfo : Container
	{
		[Export]
		public FPSWeaponHolder weaponHolder;
		[Export]
		public RichTextLabel ammoCounter;
		[Export]
		public Label reloadWarning;

		public override void _Process(double delta)
		{
			int currentAmmo = weaponHolder.currentWeapon.getCurrentAmmoInMagazine();
			// Ammo counter logic
			ammoCounter.Text = currentAmmo.ToString();

			// Reload message
			if (currentAmmo == 0)
			{
				reloadWarning.Visible = true;
			}
			else
			{
				reloadWarning.Visible = false;
			}
		}
	}
}