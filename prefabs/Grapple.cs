using Godot;
using System;

public class Grapple : RigidBody2D {
    public override void _Ready() {
        
    }

    public void ShootGrapple(int speed, Vector2 player_coordinates, Vector2 mouse_coordinates) {
        GlobalPosition = player_coordinates;
        AddForce(new Vector2(0, 0), new Vector2(100, 0));
    }
}
