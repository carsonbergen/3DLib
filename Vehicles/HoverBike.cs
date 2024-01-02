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
        public float baseSpeed = 400f;
        [Export]
        public float boostSpeed = 800f;
        [Export]
        public float brakingSpeed = 200f;
        [Export]
        public float maxBaseSpeed = 10f;
        [Export]
        public float maxBoostSpeed = 20f;

        private float speed = 0f;

        public override void _PhysicsProcess(double delta)
        {
            // Handle steering
            float steeringInput = Input.GetAxis("vehicle_right", "vehicle_left");
            Steering = steeringInput * 0.4f;

            // Adjust speed of vehicle
            if (Input.IsActionPressed("vehicle_boost"))
                speed = boostSpeed;
            else
                speed = baseSpeed;

            // Handle force application
            float engineForceInput = Input.GetAxis("vehicle_backward", "vehicle_forward");
            EngineForce = engineForceInput * speed;

            // If no input, begin braking
            if (engineForceInput == 0f)
                Brake = brakingSpeed * (float)delta;

            // Reset model roll
            if (steeringInput == 0f)
                model.RotationDegrees = model.RotationDegrees with
                {
                    X = Mathf.Lerp(model.RotationDegrees.X, 0f, (float)delta * resetSpeed)
                };
            // Rotate model
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

        public override void _IntegrateForces(PhysicsDirectBodyState3D state)
        {
            // Clamp velocity
            // Clamp for base speed
            if (speed == baseSpeed)
            {
                LinearVelocity = LinearVelocity with
                {
                    X = Mathf.Clamp(LinearVelocity.X, -maxBaseSpeed, maxBaseSpeed),
                    Z = Mathf.Clamp(LinearVelocity.Z, -maxBaseSpeed, maxBaseSpeed)
                };
            }
            // Clamp for boost speed
            else
            {
                LinearVelocity = LinearVelocity with
                {
                    X = Mathf.Clamp(LinearVelocity.X, -maxBoostSpeed, maxBoostSpeed),
                    Z = Mathf.Clamp(LinearVelocity.Z, -maxBoostSpeed, maxBoostSpeed)
                };
            }
        }
    }
}