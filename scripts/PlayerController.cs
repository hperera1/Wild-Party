using Godot;
using Godot.Collections;

public class PlayerController : KinematicBody2D {
    [Export] public PackedScene grapple; // instancing grapple
	Grapple current_grapple;
    public enum PlayerState { Idle, Walking, Grappling };
	const int walk_speed = 200;
    const int grapple_force = 600;
	Vector2 velocity;

	public override void _Ready() {
		
	}

	public override void _PhysicsProcess(float delta) {        
		if(Input.IsActionPressed("ui_left")) {
			velocity.x = -walk_speed;
		}
		else if(Input.IsActionPressed("ui_right")) {
			velocity.x = walk_speed;
		}
		else {
			velocity.x = 0;
		}

		if(Input.IsActionPressed("ui_up")) {
			velocity.y = -walk_speed;
		}
		else if(Input.IsActionPressed("ui_down")) {
			velocity.y = walk_speed;
		}
		else {
			velocity.y = 0;
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
}
