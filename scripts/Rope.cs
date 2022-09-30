using Godot;
using System;

public class Rope : Line2D {
    Node2D player;
    Vector2 player_position;
    Vector2 hooked_position;

    public Rope(Node2D _player, Vector2 _hooked_pos) {
        player = _player;
        player_position = player.Position;
        hooked_position = _hooked_pos;
        AddPoint(player_position, 0);
        AddPoint(hooked_position, 1);
        Width = 1;
    }

    public override void _PhysicsProcess(float delta) {
        SetPointPosition(0, player.Position);
    }
}