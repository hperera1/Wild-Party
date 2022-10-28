using Godot;
using System;

public class GameController : Node2D {
    PlayerController player;

    public override void _Ready() {
        player = GetNode<PlayerController>("Player");
    }

    public void GrappleConnected() {
        player.SetStateGrappling();
        player.SetStartingValues();
    }
}
