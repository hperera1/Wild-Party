using Godot;
using Godot.Collections;
using System;

public class PlayerController : KinematicBody2D {
    [Export] public PackedScene grapple; // instancing grapple
	Grapple current_grapple;
    public enum PlayerState { Idle, Walking, Grappling };
	PlayerState current_state = PlayerState.Walking;
	const int walking_speed = 200;
	const int grappling_speed = 3;
    const int grapple_force = 600;
	float delta_sum = 0.0f;
	Vector2 velocity;

	// initial constant grapple variables
	double grapple_radius = 0;
	double starting_angle = 999999;

	public override void _Ready() {
		
	}

	// TODO: set walking and grappling inputs to do different things
	public override void _PhysicsProcess(float delta) {     
		if(current_state == PlayerState.Walking)  {
			velocity = Vector2.Zero;

			if(Input.IsActionPressed("ui_left")) {
				velocity.x = -walking_speed;
			}
			else if(Input.IsActionPressed("ui_right")) {
				velocity.x = walking_speed;
			}

			if(Input.IsActionPressed("ui_up")) {
				velocity.y = -walking_speed;
			}
			else if(Input.IsActionPressed("ui_down")) {
				velocity.y = walking_speed;
			}

			MoveAndSlide(velocity, new Vector2(0, -1));
		}

		// when grappling, e & q should handle clockwise and counter clockwise movement respectively
		if(current_state == PlayerState.Grappling) {
			if(Input.IsActionPressed("grapple_clockwise")) {
				CalculatePosition(delta);
			}
			else if(Input.IsActionPressed("grapple_counter_clockwise")) {
				
			}
			else {
				
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
	}

	void CalculatePosition(float delta) {
		delta_sum += delta; 
		if(starting_angle == 999999) {
			CalculateStartingAngle();
		}

		Position = new Vector2(
			(float) Math.Sin(grappling_speed * delta_sum - starting_angle) * (float) grapple_radius, 
			(float) Math.Cos(grappling_speed * delta_sum - starting_angle) * (float) grapple_radius
		) + current_grapple.Position;
	}

	void CalculateStartingAngle() {
		double radius_squared_doubled = 2 * Math.Pow(grapple_radius, 2);
		Vector2 player_pos = Position;
		Vector2 rotation_pos = new Vector2(
			(float) (Math.Sin(0) * grapple_radius), 
			(float) (Math.Cos(0) * grapple_radius)
		) + current_grapple.Position;
		
		float pos_distance = player_pos.DistanceSquaredTo(rotation_pos);
		double distance_calculation = (radius_squared_doubled - pos_distance) / radius_squared_doubled;
		if(distance_calculation < -1) {
			distance_calculation = -2 - distance_calculation;
		}

		starting_angle = Math.Acos(distance_calculation);
		if(player_pos.x > rotation_pos.x) {
			starting_angle *= -1;
		}
	}

	public void SetStartingValues() {
		grapple_radius = Position.DistanceTo(current_grapple.Position);
	}

	public void SetStateGrappling() {
		current_state = PlayerState.Grappling;
	}
}
