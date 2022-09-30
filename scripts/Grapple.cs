using Godot;
using System;

public class Grapple : RigidBody2D {
    GameController game_controller;
    CollisionShape2D collider;
    Vector2 hooked_position;
    bool hooked = false;
    float grapple_timeout = 0;

    public override void _Ready() {
        game_controller = GetNode<GameController>("/root/GameNode");
        collider = GetNode<CollisionShape2D>("GrappleShape");
    }

    public override void _PhysicsProcess(float delta) {
        grapple_timeout += delta;
        if(grapple_timeout >= 2.0f && !hooked) {
            this.QueueFree();
        }
    }

    public override void _IntegrateForces(Physics2DDirectBodyState state) {
        if(hooked) {
            LinearVelocity = new Vector2(0, 0);
            Position = hooked_position;
        }
    }

    // 1. add collision checking
    // 2. hold grapple on pivot point and draw line from player to grapple
    // 3. apply angular momentum to player

    // make sure to delete the grapple after a certain amount of time or some other condition
    public void ShootGrapple(int force, Vector2 player_coordinates, Vector2 mouse_coordinates) {
        GlobalPosition = player_coordinates;
        double y_coordinate = mouse_coordinates.y - player_coordinates.y;
        double x_coordinate = mouse_coordinates.x - player_coordinates.x;
        double angle = Math.Atan(y_coordinate / x_coordinate);
        float y_velocity = (float) (force * Math.Sin(angle));
        float x_velocity = (float) (force * Math.Cos(angle));

        if(mouse_coordinates.x < player_coordinates.x) {
            x_velocity *= -1;
            y_velocity *= -1;
        }

        LinearVelocity = (new Vector2(x_velocity, y_velocity));
    }

    void GrappleEnteredObstacle(PhysicsBody2D entered_node) {
        if(entered_node is RigidBody2D) {
            hooked = true;
            hooked_position = entered_node.Position;
            collider.SetDeferred("disabled", true);
            
            // draw rope from player to hooked object
            Node2D player = GetNode<Node2D>("/root/GameNode/Player");
            Rope grapple_rope = new Rope(player, hooked_position);
            GetNode("/root/GameNode").AddChild(grapple_rope);
            game_controller.GrappleConnected();
        }
    }
}