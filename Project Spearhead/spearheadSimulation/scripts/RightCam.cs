using Godot;
using System;

public class RightCam : Camera
{
	private Vector3 cameraPos;
	private Spatial spearhead;
	private Spatial rightCam;
	
	public override void _Ready()
	{
		base._Ready();
		cameraPos = new Vector3(-180,450,650);
		spearhead = GetNode<Spatial>("/root/Environment/spearhead");
		rightCam = GetNode<Spatial>("/root/Environment/RightCam");
	}

	public override void _Process(float delta)
	{
		rightCam.Translation = spearhead.Translation + cameraPos;
	}
}
