using Godot;
using ThreeDLib;

namespace FPS
{
    [GlobalClass]
    public partial class FPSDamager : Damager
    {
        [Export]
        public float criticalAreaBonus = 1.05f;


        public void damageBehaviour(Enemy enemy, bool criticalArea) 
        {
            enemy.health -= criticalArea ? damage * criticalAreaBonus : damage;
            // GD.Print("enemy health:\t", enemy.health);
        }

        public FPSDamager() 
        {}
    }
}