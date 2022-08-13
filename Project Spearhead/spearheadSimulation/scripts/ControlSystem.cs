using Godot;
using System;

public class ControlSystem
{
	float[] Gains; 
	float tau;
	float dt;
	float Error;
	float sumError;
	float prevError;
	float prevY;
	float prevU;
	
	// Setup PID
	public void PIDSetup(float Kp, float Ki, float Kd, float Tau)
	{
		Gains =  new float[3]{Kp, Ki, Kd};
		tau = Tau;
		dt = 0f;
		Error = 0f;
		sumError = 0f;
		prevError = 0f;
		prevY = 0f;
		prevU = 0f;
	}
	
	// Simple PID With Derivative Filter
	public float PID(float Val,float Sp)
	{
		Error = Sp - Val;
		
		float P = Gains[0] * Error;
		
		float I = Gains[1] * sumError;
		sumError += dt*Error;
		
		float D = Gains[2] * filter(Error - prevError)/dt;
		
		prevError = Error;
		
		float sumPID = saturation(P + I + D);
		return(sumPID);
	}
	
	float saturation(float data)
	{
		if (data > 1000)
		{
			data = 1000;
		}
		if (data < 0)
		{
			data = 0;
		}
		return(data);
	}
	
	// Simple Bilinear First Order Filter
	float filter(float U)
	{
		float coeff = 2 * tau / dt;
		float num = 1 - coeff;
		float dnum = 1 + coeff;
		float Y = (U + prevU - prevY*num) / dnum;
		
		prevU = U;
		prevY = Y;
		return(Y);
	}
	
	// 'delta' is Elapsed Time Since Previous Frame.
	public void PIDRun(float delta)
	{    
		dt = delta;
	}
}
