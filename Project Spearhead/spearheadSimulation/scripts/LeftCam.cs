using Godot;
using System;

public class LeftCam : Camera
{
	private Vector3 cameraPos;
	private Spatial spearhead;
	private Spatial leftCam;
	
	public override void _Ready()
	{
		base._Ready();
		cameraPos = new Vector3(-180,450,-650);
		spearhead = GetNode<Spatial>("/root/Environment/spearhead");
		leftCam = GetNode<Spatial>("/root/Environment/LeftCam");
	}

	public override void _Process(float delta)
	{
		leftCam.Translation = spearhead.Translation + cameraPos;
	}
}
