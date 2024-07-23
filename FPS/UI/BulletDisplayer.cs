using Godot;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FPS
{
	public partial class BulletDisplayer : Label
	{
		[Export]
		public FPSWeaponHolder weaponHolder;

		private Thread textSettingThread;
		private string currentText = "";
		private bool continueThread = true;
		private int currentAmmo = 0;

		public override void _Ready()
		{
			StartATask();
		}

		public override void _Process(double delta)
		{
			currentAmmo = (int)weaponHolder.currentWeapon.getCurrentAmmoInMagazine();
			Text = currentText;
		}

		public async void StartATask()
		{
			await Task.Run(
				async () =>
				{
					while (continueThread)
					{
						currentText = await Repeat("|", this.currentAmmo);
					}
				}
			);
		}
		
		public static async Task<string> Repeat(string text, int n)
		{
			string ret = await Task.Run(
				() =>
				{
					if (n < 0) return "∞";
					var textAsSpan = text.AsSpan();
					var span = new Span<char>(new char[textAsSpan.Length * (int)n]);
					for (var i = 0; i < n; i++)
					{
						textAsSpan.CopyTo(span.Slice((int)i * textAsSpan.Length, textAsSpan.Length));
					}

					return span.ToString();
				}
			);
			return ret;
		}
	}
}