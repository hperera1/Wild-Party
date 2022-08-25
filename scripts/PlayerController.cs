using Godot;
using Godot.Collections;

public class PlayerController : KinematicBody2D {
    public enum PlayerState { Idle, Walking, Grappling };
	const int walk_speed = 200;
    const int grapple_speed = 400;
	Vector2 velocity;
    Grapple grapple;

	public override void _Ready() {
		grapple = GetNode<Grapple>("Grapple");
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
            GD.Print("shoot grapple");
            var world_space = GetWorld2d().DirectSpaceState;
		    Vector2 mouse_position = GetGlobalMousePosition();
            grapple.ShootGrapple(grapple_speed, GlobalPosition, mouse_position);
        }

		MoveAndSlide(velocity, new Vector2(0, -1));
	}
}
