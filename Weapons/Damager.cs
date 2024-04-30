using Godot;

namespace ThreeDLib
{
    [GlobalClass]
    public partial class Damager : Resource
    {
        [Export]
        public float damage = 0f;

        public void damageBehaviour(Enemy enemy) 
        {
            enemy.health -= damage;
            GD.Print("enemy health:\t", enemy.health);
        }

        public Damager() 
        {}
    }
}