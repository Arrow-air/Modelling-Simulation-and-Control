using Godot;
using System;

public class RearCam : Camera
{
	private Vector3 cameraPos;
	private Spatial spearhead;
	private Spatial rearCam;
	
	public override void _Ready()
	{
		base._Ready();
		cameraPos = new Vector3(-700,450,0);
		spearhead = GetNode<Spatial>("/root/Environment/spearhead");
		rearCam = GetNode<Spatial>("/root/Environment/RearCam");
	}

	public override void _Process(float delta)
	{
		rearCam.Translation = spearhead.Translation + cameraPos;
	}
}
