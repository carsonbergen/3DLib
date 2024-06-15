# FPS 
This part of the repo contains code for the FPS player controller and other related parts of the FPS system.

## FPSCharacterBody3D
This is the FPS player controller and extends the PlayerCharacterBody3D controller.

## FPSUpperBody
This is the first in the chain of crucial FPS components. It modifies the arm model, and camera.

## FPSWeaponHolder
This is the last in the chain of crucial FPS components. It handles all weapon related logic.

## PlayerSettings
Godot resource for player settings.
- `lookSensitivity`: how fast the camera moves when not ADSed.
- `adsSensitivity`: how fast the camera moves when ADSed.
- `fov`: saved FOV setup for the player.
- `jumpDistance`: how far the player will jump forward.
- `walkSpeed`: how fast the player walks by default.
- `sprintSpeed`: how fast the player sprints by default.
- `adsWalkSpeed`: how fast the player moves when ADSed.
- `crouchSpeed`: how fast the player moves when crouched.
- `jumpVelocity`: how high the player jumps.
- `speedChangeFactor`: for fast the speed of the player updates.
- `inAirMovementFactor`: how fluidly the player can move while in the air.
- Gravity: the amount of gravity applied to the player.