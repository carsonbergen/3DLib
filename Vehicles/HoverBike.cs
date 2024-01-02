using Godot;

namespace ThreeDLib
{
    public partial class HoverBike : Vehicle
    {
        [Export]
        public float leanSpeed = 10f;
        [Export]
        public float resetSpeed = 10f;

        [Export]
        public float baseSpeed = 50f;
        [Export]
        public float boostSpeed = 100f;
        [Export]
        public float brakingSpeed = 200f;

        private float speed = 0f;
        private float previousVelocity = 0f;

        public override void _PhysicsProcess(double delta)
        {
            GD.Print("Engine force: ", EngineForce);
            GD.Print("Braking force: ", Brake);
            GD.Print("Steering: ", Steering);
            GD.Print("Linear velocity: ", LinearVelocity);

            float steeringInput = Input.GetAxis("vehicle_right", "vehicle_left");

            if (Input.IsActionJustPressed("vehicle_boost"))
            {
                previousVelocity = LinearVelocity.X;
                speed = boostSpeed;
            }
            else
            {
                Brake = brakingSpeed * (float) delta;
                speed = baseSpeed;
            }

            Steering = steeringInput * 0.4f;
            float engineForceInput = Input.GetAxis("vehicle_backward", "vehicle_forward");
            EngineForce = engineForceInput * speed;

            if (engineForceInput == 0f)
            {
                Brake = brakingSpeed * (float) delta;
            }

            if (steeringInput == 0f)
            {
                model.RotationDegrees = model.RotationDegrees with
                {
                    X = Mathf.Lerp(model.RotationDegrees.X, 0f, (float) delta * resetSpeed)
                };
            }
            else
            {
                model.RotationDegrees = model.RotationDegrees with
                {
                    X = Mathf.Lerp(
                                    model.RotationDegrees.X,
                                    model.RotationDegrees.X + (45 * Steering),
                                    (float)delta * leanSpeed
                                )
                };
                model.RotationDegrees = model.RotationDegrees with
                {
                    X = Mathf.Clamp(model.RotationDegrees.X, -15, 15)
                };
            }
        }
    }
}