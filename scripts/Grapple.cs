using Godot;
using System;

public class Grapple : RigidBody2D {
    public override void _Ready() {
        
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
}