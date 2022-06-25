using Godot;
using System;

public class SideCam : Camera
{
    private Vector3 cameraPos;
    private Spatial spearhead;
    private Spatial sideCam;
    
    public override void _Ready()
    {
        base._Ready();
        cameraPos = new Vector3(0,200,200);
        spearhead = GetNode<Spatial>("/root/Environment/spearhead");
        sideCam = GetNode<Spatial>("/root/Environment/SideCam");
    }

    public override void _Process(float delta)
    {
        sideCam.Translation = spearhead.Translation + cameraPos;
    }
}