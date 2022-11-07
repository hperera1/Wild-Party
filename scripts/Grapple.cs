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

    public void ShootGrapple(int force, Vector2 player_coordinates, Vector2 mouse_coordinates) {
        GlobalPosition = player_coordinates;
        double y_coordinate = mouse_coordinates.y - player_coordinates.y;
        double x_coordinate = mouse_coordinates.x - player_coordinates.x;
        double angle = Math.Atan(y_coordinate / x_coordinate);
        
        float y_velocity = (float) (force * Math.Sin(angle));
        float x_velocity = (float) (force * Math.Cos(angle));
        double y_displacement = Math.Sin(angle) * 40;
        double x_displacement = Math.Cos(angle) * 40;

        // flip velocity and position displacement if on the left side of player
        if(mouse_coordinates.x < player_coordinates.x) {
            x_velocity *= -1;
            y_velocity *= -1;
            x_displacement *= -1;
            y_displacement *= -1;
        }

        Vector2 grapple_pos = Position;
        grapple_pos.x += (float) x_displacement;
        grapple_pos.y += (float) y_displacement;
        Position = grapple_pos;
        LinearVelocity = (new Vector2(x_velocity, y_velocity));
    }

    void GrappleEnteredObstacle(PhysicsBody2D entered_node) {
        if(entered_node is RigidBody2D) {
            game_controller.GrappleConnected();
            hooked = true;
            hooked_position = entered_node.Position;
            collider.SetDeferred("disabled", true);
            
            // draw rope from player to hooked object
            Node2D player = GetNode<Node2D>("/root/GameNode/Player");
            Rope grapple_rope = new Rope(player, hooked_position);
            GetNode("/root/GameNode").AddChild(grapple_rope);
        }
    }
}