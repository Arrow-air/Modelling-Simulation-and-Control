using Godot;
using System;

public class Environment : Spatial
{
    public Camera frontCam;
    public Camera sideCam;
    public Camera rearCam;

    private int viewState;

    public override void _Ready()
    {
        base._Ready();
        frontCam = GetNode<Camera>("/root/Environment/FrontCam");
        sideCam = GetNode<Camera>("/root/Environment/SideCam");
        rearCam = GetNode<Camera>("/root/Environment/RearCam");

        viewState = 0;
    }
    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("ui_accept"))
        {
            viewState = 1;
            if(viewState == 1)
            {
                frontCam.MakeCurrent();
            }
        }
		else if (Input.IsActionPressed("ui_up"))
        {
            viewState = 2;
            if(viewState == 2)
            {
                rearCam.MakeCurrent();
            }
        }
        else if (Input.IsActionPressed("ui_right") || Input.IsActionPressed("ui_left"))
		{
            viewState = 3;
            if(viewState == 3)
            {
                sideCam.MakeCurrent();
            }
		}
    }
}