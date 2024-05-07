using Godot;
using ThreeDLib;

namespace FPS
{
    [GlobalClass]
    public partial class FPSDamager : Damager
    {
        [Export]
        public float criticalAreaBonus = 1.05f;
        [Export]
        public float passThroughDamageReduction = 0.05f;


        public void damageBehaviour(Enemy enemy, bool criticalArea, int passThroughAmount) 
        {
            var damageToBeDone = damage - (float)(passThroughDamageReduction * passThroughAmount);
            GD.Print("damage:", criticalArea ? (damageToBeDone * criticalAreaBonus) : damageToBeDone, "\t", passThroughAmount, "\t", criticalArea);
            enemy.health -= criticalArea ? (damageToBeDone * criticalAreaBonus) : damageToBeDone; 
            // GD.Print("enemy health:\t", enemy.health);
        }

        public FPSDamager() 
        {}
    }
}