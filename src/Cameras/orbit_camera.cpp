#include "orbit_camera.h"
#include <godot_cpp/core/class_db.hpp>

using namespace godot;

void OrbitCamera::_bind_methods()
{
    ClassDB::bind_method(D_METHOD("get_movement_speed"), &OrbitCamera::get_movement_speed);
    ClassDB::bind_method(D_METHOD("set_movement_speed", "p_movement_speed"), &OrbitCamera::set_movement_speed);
    ADD_PROPERTY(PropertyInfo(Variant::FLOAT, "movement_speed"), "set_movement_speed", "get_movement_speed");

    // ClassDB::bind_method(D_METHOD("get_camera_sensitivity"), &OrbitCamera::get_camera_sensitivity);
    // ClassDB::bind_method(D_METHOD("set_camera_sensitivity", "p_camera_sensitivity"), &OrbitCamera::set_camera_sensitivity);
    // ADD_PROPERTY(PropertyInfo(Variant::FLOAT, "camera_sensitivity"), "set_camera_sensitivity", "get_camera_sensitivity");

    // ClassDB::bind_method(D_METHOD("get_camera_acceleration_speed"), &OrbitCamera::get_camera_acceleration_speed);
    // ClassDB::bind_method(D_METHOD("set_camera_acceleration_speed", "p_camera_acceleration_speed"), &OrbitCamera::set_camera_acceleration_speed);
    // ADD_PROPERTY(PropertyInfo(Variant::FLOAT, "camera_acceleration_speed"), "set_camera_acceleration_speed", "get_camera_acceleration_speed");
}

// void GDExample::_bind_methods() {
// 	ClassDB::bind_method(D_METHOD("get_amplitude"), &GDExample::get_amplitude);
// 	ClassDB::bind_method(D_METHOD("set_amplitude", "p_amplitude"), &GDExample::set_amplitude);
// 	ClassDB::add_property("GDExample", PropertyInfo(Variant::FLOAT, "amplitude"), "set_amplitude", "get_amplitude");
// }

OrbitCamera::OrbitCamera()
{
    // Init variables
    movement_speed = 15.0;
    // camera_sensitivity = 1.0;
    // camera_acceleration_speed = 0.5;
    // camera_acceleration_amount = 0.5;
    // base_camera_update_speed = 0.5;
    // fast_camera_update_speed = 0.75;
    // inverted = false;

    // internal_camera_sensitivity = 0;
    // max_internal_camera_sensitivity = 0;
}

OrbitCamera::~OrbitCamera()
{
    // Clean up
}

// Getters and setters
// Movement speed
void OrbitCamera::set_movement_speed(const float p_movement_speed)
{
    movement_speed = p_movement_speed;
}
double OrbitCamera::get_movement_speed() const
{
    return movement_speed;
}
// Camera sensitivity
// void OrbitCamera::set_camera_sensitivity(const float p_camera_sensitivity)
// {
//     camera_sensitivity = p_camera_sensitivity;
// }
// double OrbitCamera::get_camera_sensitivity() const
// {
//     return camera_sensitivity;
// }
// // Camera acceleration
// void OrbitCamera::set_camera_acceleration_speed(const float p_camera_acceleration_speed)
// {
//     camera_acceleration_speed = p_camera_acceleration_speed;
// }
// double OrbitCamera::get_camera_acceleration_speed() const
// {
//     return camera_acceleration_speed;
// }
// // Camera acceleration amount
// void OrbitCamera::set_camera_acceleration_amount(const float p_camera_acceleration_amount)
// {
//     camera_acceleration_amount = p_camera_acceleration_amount;
// }
// double OrbitCamera::get_camera_acceleration_amount() const
// {
//     return camera_acceleration_amount;
// }
// // Base camera update speed
// void OrbitCamera::set_base_camera_update_speed(const float p_base_camera_update_speed)
// {
//     base_camera_update_speed = p_base_camera_update_speed;
// }
// double OrbitCamera::get_base_camera_update_speed() const
// {
//     return base_camera_update_speed;
// }
// // Fast camera update speed
// void OrbitCamera::set_fast_camera_update_speed(const float p_fast_camera_update_speed)
// {
//     fast_camera_update_speed = p_fast_camera_update_speed;
// }
// double OrbitCamera::get_fast_camera_update_speed() const
// {
//     return fast_camera_update_speed;
// }
// // Inverted
// void OrbitCamera::set_inverted(const bool p_inverted)
// {
//     inverted = p_inverted;
// }
// bool OrbitCamera::get_inverted() const
// {
//     return inverted;
// }

void OrbitCamera::_process(double delta)
{
}