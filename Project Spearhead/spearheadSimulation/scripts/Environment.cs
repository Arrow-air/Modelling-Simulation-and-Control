using Godot;
using System;

public class Environment : Spatial
{
	public Camera frontCam;
	public Camera rearCam;
	public Camera rightCam;
	public Camera leftCam;
	public Label3D state;
	private Spatial spearheadPos;
	private RigidBody spearheadVel;
	
	private Vector3 Vel;
	private Vector3 aVel;
	private Vector3 Pos;
	private Vector3 aPos;
	
	private int viewState;
	private double todeg;
	private float ground;
	
	public override void _Ready()
	{
		base._Ready();
		frontCam = GetNode<Camera>("/root/Environment/FrontCam");
		rightCam = GetNode<Camera>("/root/Environment/RightCam");
		leftCam = GetNode<Camera>("/root/Environment/LeftCam");
		rearCam = GetNode<Camera>("/root/Environment/RearCam");
		state = GetNode<Label3D>("/root/Environment/spearhead/State");
		spearheadPos = GetNode<Spatial>("/root/Environment/spearhead");
		spearheadVel = GetNode<RigidBody>("/root/Environment/spearhead");
		
		viewState = 0;
		todeg = 180/Math.PI;
		ground = 143.68f;
	}
	public override void _Process(float delta)
	{
		Vel = new Vector3(spearheadVel.LinearVelocity.x,spearheadVel.LinearVelocity.z,-spearheadVel.LinearVelocity.y);
		aVel = new Vector3(spearheadVel.AngularVelocity.x,spearheadVel.AngularVelocity.z,-spearheadVel.AngularVelocity.y);
		Pos = new Vector3(spearheadPos.GlobalTranslation.x, spearheadPos.GlobalTranslation.z, -spearheadPos.GlobalTranslation.y);
		aPos = new Vector3(spearheadPos.Rotation.x, spearheadPos.Rotation.z, spearheadPos.Rotation.y);
		
		float u = Vel.x;
		float v = Vel.y;
		float w = Vel.z;
		
		float p = aVel.x;
		float q = aVel.y;
		float r = aVel.z;
		
		float x = Pos.x;
		float y = Pos.y;
		float z = Pos.z;
		
		float phi = aPos.x;
		float theta = aPos.y;
		float psi = aPos.z;
		
		state.Text = "XPos "+(x).ToString("0.00")+" YPos "+(y).ToString("0.00")+" ZPos "+(-(z+ground)).ToString("0.00")+" Xvel "+(u).ToString("0.00")+" Yvel "+(v).ToString("0.00")+" Zvel "+(w).ToString("0.00")+" Phi "+(phi*todeg).ToString("0.00")+" Theta "+(theta*todeg).ToString("0.00")+" Psi "+(psi*todeg).ToString("0.00");
		
		if (Input.IsActionPressed("ui_down"))
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
		else if (Input.IsActionPressed("ui_right"))
		{
			viewState = 3;
			if(viewState == 3)
			{
				rightCam.MakeCurrent();
			}
		}
		else if (Input.IsActionPressed("ui_left"))
		{
			viewState = 4;
			if(viewState == 4)
			{
				leftCam.MakeCurrent();
			}
		}
	}
}
