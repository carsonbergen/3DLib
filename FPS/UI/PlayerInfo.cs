using Godot;
using ThreeDLib;

namespace FPS
{
	public partial class PlayerInfo : Container
	{
		[Export]
		public FPSCharacterBody3D player;

		[Export]
		public FPSWeaponHolder weaponHolder;
		[Export]
		public Label ammoInMagazineLabel;
		[Export]
		public Label ammoLeftLabel;
		[Export]
		public Label weaponLabel;

		[Export]
		public Label reloadWarning;

		[Export]
		public Label healthValueLabel;


		public override void _Process(double delta)
		{
			int currentAmmo = weaponHolder.currentWeapon.getCurrentAmmoInMagazine();
			int currentAmmoLeft = weaponHolder.currentWeapon.getTotalAmmoLeft() - (
				weaponHolder.currentWeapon.getMagazineSize() - currentAmmo
			);
			// Ammo counter logic
			// ammoInMagazineLabel.Text = currentAmmo.ToString();
			ammoLeftLabel.Text = currentAmmoLeft.ToString();
			weaponLabel.Text = weaponHolder.currentWeapon.name;

			// Reload message
			if (currentAmmo == 0)
			{
				reloadWarning.Visible = true;
			}
			else
			{
				reloadWarning.Visible = false;
			}

			healthValueLabel.Text = player.playerAttributes.health.ToString();
		}
	}
}