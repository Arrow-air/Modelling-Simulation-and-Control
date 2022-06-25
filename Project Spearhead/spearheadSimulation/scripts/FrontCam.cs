using Godot;
using System;

public class FrontCam : Camera
{
    private Vector3 cameraPos;
    private Spatial spearhead;
    private Spatial frontCam;
    
    public override void _Ready()
    {
        base._Ready();
        cameraPos = new Vector3(100,200,0);
        spearhead = GetNode<Spatial>("/root/Environment/spearhead");
        frontCam = GetNode<Spatial>("/root/Environment/FrontCam");
    }

    public override void _Process(float delta)
    {
        frontCam.Translation = spearhead.Translation + cameraPos;
    }
}