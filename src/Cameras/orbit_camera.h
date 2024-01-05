#ifndef ORBIT_CAMERA_H
#define ORBIT_CAMERA_H

// Inherits from the node3d node
#include <godot_cpp/classes/node3d.hpp>
#include <godot_cpp/classes/camera3d.hpp>
#include <godot_cpp/classes/spring_arm3d.hpp>

namespace godot
{
    class OrbitCamera : public Node3D
    {
        GDCLASS(OrbitCamera, Node3D)
        // Properties
    private:
        // Nodes
        // Node3D target;
        // Camera3D camera;
        // SpringArm3D pivot;
        // Properties
        // Speed
        float movement_speed;
        // float camera_sensitivity;
        // float camera_acceleration_speed;
        // float camera_acceleration_amount;
        // float base_camera_update_speed;
        // float fast_camera_update_speed;
        // // Other settings
        // bool inverted;
        // // Private variables
        // float internal_camera_sensitivity = 0;
        // float max_internal_camera_sensitivity = 0;

    protected:
        static void _bind_methods();

    public:
        OrbitCamera();
        ~OrbitCamera();
        // Getters and setters
        // Movement speed
        void set_movement_speed(const float p_movement_speed);
        double get_movement_speed() const;
        // Camera sensitivity
        // void set_camera_sensitivity(const float p_camera_sensitivity);
        // double get_camera_sensitivity() const;
        // // Camera acceleration
        // void set_camera_acceleration_speed(const float p_camera_acceleration_speed);
        // double get_camera_acceleration_speed() const;
        // // Camera acceleration amount
        // void set_camera_acceleration_amount(const float p_camera_acceleration_amount);
        // double get_camera_acceleration_amount() const;
        // // Base camera update speed
        // void set_base_camera_update_speed(const float p_base_camera_update_speed);
        // double get_base_camera_update_speed() const;
        // // Fast camera update speed
        // void set_fast_camera_update_speed(const float p_fast_camera_update_speed);
        // double get_fast_camera_update_speed() const;
        // // Inverted
        // void set_inverted(const bool p_inverted);
        // bool get_inverted() const;

        void _process(double delta) override;
    };
}

#endif