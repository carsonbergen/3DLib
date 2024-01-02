using Godot;

namespace ThreeDLib
{
    public partial class HoverBike : Vehicle
    {
        [ExportCategory("Wheels")]
        [Export]
        public VehicleWheel3D frontLeftWheel;
        [Export]
        public VehicleWheel3D frontRightWheel;
        [Export]
        public VehicleWheel3D backLeftWheel;
        [Export]
        public VehicleWheel3D backRightWheel;

        [ExportCategory("Vehicle & engine properties")]
        [ExportGroup("Rotation speeds")]
        [Export]
        public float leanSpeed = 10f;
        [Export]
        public float baseTurnSpeed = 0.5f;
        [Export]
        public float boostTurnSpeed = 0.25f;
        [Export]
        public float resetSpeed = 10f;
        [ExportGroup("Movement speeds")]
        [Export]
        public float baseSpeed = 400f;
        [Export]
        public float boostSpeed = 800f;
        [Export]
        public float steeringSpeed = 1f;
        [Export]
        public float brakingSpeed = 200f;
        [Export]
        public float maxBaseSpeed = 10f;
        [Export]
        public float maxBoostSpeed = 20f;
        [ExportCategory("Physics")]
        [Export]
        public RayCast3D groundCast;

        private float speed = 0f;
        private float turnSpeed = 0f;

        public override void _PhysicsProcess(double delta)
        {
            // Handle steering
            float steeringInput = Input.GetAxis("vehicle_right", "vehicle_left");
            Steering = steeringInput * turnSpeed;
            frontLeftWheel.Steering = steeringInput * turnSpeed;
            frontRightWheel.Steering = steeringInput * turnSpeed;

            backLeftWheel.Steering = -(steeringInput * turnSpeed);
            backRightWheel.Steering = -(steeringInput * turnSpeed);


            // Adjust speed of vehicle
            if (Input.IsActionPressed("vehicle_boost"))
            {
                speed = boostSpeed;
                turnSpeed = boostTurnSpeed;
            }
            else
            {
                speed = baseSpeed;
                turnSpeed = baseTurnSpeed;
            }
            if (groundCast.IsColliding())
            {
                // Handle force application
                float engineForceInput = Input.GetAxis("vehicle_backward", "vehicle_forward");
                EngineForce = engineForceInput * speed;


                // If no input, begin braking or rotating
                if (engineForceInput == 0f)
                {
                    if (steeringInput != 0f)
                    {
                        AngularVelocity = AngularVelocity with { Y = steeringInput * steeringSpeed };
                        EngineForce = steeringSpeed * turnSpeed;
                    }
                    else
                        Brake = brakingSpeed * (float)delta;
                }
            }
            // Reset model roll
            if (steeringInput == 0f || !groundCast.IsColliding())
                model.RotationDegrees = model.RotationDegrees with
                {
                    X = Mathf.Lerp(model.RotationDegrees.X, 0f, (float)delta * resetSpeed)
                };
            // Rotate model
            else
            {
                GD.Print(model.RotationDegrees);
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

        // Clamp velocity
        public override void _IntegrateForces(PhysicsDirectBodyState3D state)
        {
            // Clamp velocity
            // Clamp for base speed
            if (groundCast.IsColliding())
            {
                GD.Print("Ground cast is colliding");
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
}