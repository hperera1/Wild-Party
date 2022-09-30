using Godot;
using Godot.Collections;
using System;

public class PlayerController : KinematicBody2D {
    [Export] public PackedScene grapple; // instancing grapple
	Grapple current_grapple;
    public enum PlayerState { Idle, Walking, Grappling };
	PlayerState current_state = PlayerState.Walking;
	const int walking_speed = 200;
	const int grappling_speed = 200;
    const int grapple_force = 600;
	int grapple_length;
	Vector2 velocity;

	public override void _Ready() {
		
	}

	// TODO: set walking and grappling inputs to do different things
	public override void _PhysicsProcess(float delta) {      
		if(current_state == PlayerState.Walking)  {
			if(Input.IsActionPressed("ui_left")) {
				velocity.x = -walking_speed;
			}
			else if(Input.IsActionPressed("ui_right")) {
				velocity.x = walking_speed;
			}
			else {
				velocity.x = 0;
			}

			if(Input.IsActionPressed("ui_up")) {
				velocity.y = -walking_speed;
			}
			else if(Input.IsActionPressed("ui_down")) {
				velocity.y = walking_speed;
			}
			else {
				velocity.y = 0;
			}
		}

		// when grappling, e & q should handle clockwise and counter clockwise movement respectively
		if(current_state == PlayerState.Grappling) {
			if(Input.IsActionPressed("grapple_clockwise")) {
				velocity = CalculateRotation() * 2;
			}
			else if(Input.IsActionPressed("grapple_counter_clockwise")) {
				
			}
			else {
				velocity = Vector2.Zero;
			}
		}

        if(Input.IsActionJustPressed("grapple_shoot")) {
            Grapple new_grapple = (Grapple) grapple.Instance();
			current_grapple = new_grapple;
			GetNode("/root/GameNode").AddChild(new_grapple);

            var world_space = GetWorld2d().DirectSpaceState;
		    Vector2 mouse_position = GetGlobalMousePosition();
            new_grapple.ShootGrapple(grapple_force, GlobalPosition, mouse_position);
        }

		MoveAndSlide(velocity, new Vector2(0, -1));
	}

	// (-y, x)
	Vector2 CalculateRotation() {
		Vector2 rotation_vector = new Vector2();
		double y_coordinate = Position.y - current_grapple.Position.y;
        double x_coordinate = Position.x - current_grapple.Position.x;
		rotation_vector.x = (float) (-y_coordinate);
		rotation_vector.y = (float) (x_coordinate);

		return rotation_vector;
	}

	public void SetStateGrappling() {
		current_state = PlayerState.Grappling;
	}
}
