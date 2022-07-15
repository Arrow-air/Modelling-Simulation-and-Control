using Godot;
using System;

using coefficients;

public class spearhead : RigidBody
{
	//private int rotationDir;
	// Aerodynamics
	public double[] adb_C;
	public double[] le_C;
	public double[] re_C;
	public double[] rudder_C;
	//adb = importdata('../../datafiles/adb_w_hat.txt');
	//l_elevon = importdata('../../datafiles/le_w_hat.txt');
	//r_elevon = importdata('../../datafiles/re_w_hat.txt');
	//rudder = importdata('../../datafiles/rudder_w_hat.txt');
	// VTOL Dimensions
	private double M;
	private double g;
	private double L1;
	private double L2;
	private double C;
	private double S;
	private double eS;
	private double rS;
    private double rho; // air desnsity at sea level

	// Mass Moment of Inertia about the COM As Taken From CAD
	private double Ixx;
	private double Iyy;
	private double Izz;

	// X = [u v w p q r x y z phi theta psi w1 w2 w3 w4 w5 dl dr drd]
	private Vector3 Vel;
	private Vector3 Accel;
	private Vector3 Pos;
	private Vector3 Angle;
	private double[] MotorV;
	private double dl;
	private double dr;
	private double drd;

	// U = [u1 u2 u3 u4 u5 u6 u7 u8]
	private double[] U;
	private double[] K;
	private double[] Mtau; 
	private double[] Ku;

	// Input Force and Torque Vectors
	private Vector3 Fthrust;
	private Vector3 Fgravity;
	private Vector3 Faerodynamics;
	private Vector3 Mthrust;
	private Vector3 Maerodynamics;
	public Vector3 resultantTorque;
	public Vector3 resultantForce;

	// Wind Frame Variables
	private double todeg;
	private double V;
	private double alpha;
	private double beta;

	private Spatial spearheadAtt;

	public override void _Ready()
    {
        base._Ready();
	
        //Vthrust = new Vector3(0,1000,0);
		//Hthrust = new Vector3(1000,0,0);
		//Vtorque = new Vector3(100000,100000,100000);
		//Htorque = new Vector3(100000,100000,100000);
		
		// Build In Ridig Body 6-DOF Equations of Motion
		//GODOT -> N E D = X Z -Y -> X Y Z
		M = 20;
		g = 9.81;
		rho = 1.225;
		L1 = 0.6;
		L2 = 0.6;
		C = 0.32;
		S = 2.24;
		eS = 0.28*0.58; // Elevon Planform Area chord x span
		rS = 0.32*0.30; // Rudder Planform Area chord x span
		
		Ixx = 5.061E+05;
		Iyy = 5.061E+05;
		Izz = 10.011E+05;

		K = new double[4]{Math.Pow(1.123*10,-6), Math.Pow(2.25*10,-6), 7.708e-10 * 1000, 7.708e-10 * 1500};
		Mtau = new double[3]{1/44.22, 1/44.22, 1/20};
		Ku = new double[3]{44.2205*8.18, 44.2205*7.02, 3.6};

		todeg = 180/Math.PI;
		V = 0;
		alpha = 0;
		beta = 0;

		Fthrust = new Vector3(0,0,0);
		Fgravity = new Vector3(0,0,0);
		Faerodynamics = new Vector3(0,0,0);
		Mthrust = new Vector3(0,0,0);
		Maerodynamics = new Vector3(0,0,0);
		resultantTorque = new Vector3(0,0,0);
		resultantForce = new Vector3(0,0,0);

		Vel = new Vector3(0,0,0);
		Accel = new Vector3(0,0,0);
		Pos = new Vector3(0,0,0);
		Angle = new Vector3(0,0,0);
		MotorV = new double[5]{0,0,0,0,0};
		dl = 0;
		dr = 0;
		drd = 0;

		spearheadAtt = GetNode<Spatial>("/root/Environment/spearhead");
    }

	public override void _IntegrateForces(PhysicsDirectBodyState state)
	{/*
		rotationDir = 0;
		resultantTorque = new Vector3(0,0,0);
        
        if (Input.IsActionPressed("ui_accept"))
        {
			AddCentralForce(Vthrust);
        }
		else if (Input.IsActionPressed("ui_up"))
        {
			AddCentralForce(Hthrust);
        }
		else if (Input.IsActionPressed("ui_right"))
		{
			rotationDir += 1;
			resultantTorque = new Vector3(rotationDir * Vtorque.x,0,0);
			AddTorque(resultantTorque);
		}
		else if (Input.IsActionPressed("ui_left"))
		{
			rotationDir -= 1;
			resultantTorque = new Vector3(rotationDir * Vtorque.x,0,0);
			AddTorque(resultantTorque);
		}
		else
        {
			AddCentralForce(new Vector3());
        }
*/
		var leftpeddle = Input.GetJoyAxis(0,0);
		var rightpeddle = Input.GetJoyAxis(0,1);
		var rudderin = Input.GetJoyAxis(0,2);

		var rollin = Input.GetJoyAxis(1,0);
		var pitchin = Input.GetJoyAxis(1,1);
		var throttle = Input.GetJoyAxis(1,2);
		var throttledailbottom = Input.GetJoyAxis(1,3);
		var throttledailtop = Input.GetJoyAxis(1,4);
		var yawin = Input.GetJoyAxis(1,5);
		var throttleslider = Input.GetJoyAxis(1,6);

		U =  new double[8]{0,0,0,0,throttle*1000 + 1000, pitchin*200 + rollin*200, pitchin*200 - rollin*200, rudderin*200};

		// States
		Vel = new Vector3(spearhead.LinearVelocity.x,spearhead.LinearVelocity.z,-spearhead.LinearVelocity.y);
		Accel = new Vector3(spearhead.AngularVelocity.x,spearhead.AngularVelocity.z,-spearhead.AngularVelocity.y);
		Angle = new Vector3(spearheadAtt.Rotation.x, spearheadAtt.Rotation.z, -spearheadAtt.Rotation.y);

		var phi = Angle.x;
		var theta = Angle.y;
		var psi = Angle.z;

		// Inputs
		MotorV = new double[5]{Ku[1]*U[1],Ku[1]*U[2],Ku[1]*U[3],Ku[1]*U[4],Ku[2]*U[5]};
		dl = Ku[3]*U[6];
		dr = Ku[3]*U[7];
		drd = Ku[3]*U[8];

		// Motors
		var F1 = K[0]*MotorV[0]*MotorV[0];
		var F2 = K[0]*MotorV[1]*MotorV[1];
		var F3 = K[0]*MotorV[2]*MotorV[2];
		var F4 = K[0]*MotorV[3]*MotorV[3];
		var F5 = K[1]*MotorV[4]*MotorV[4];

		var Tau1 =   K[2]*MotorV[0]*MotorV[0];
		var Tau2 = - K[2]*MotorV[1]*MotorV[1];
		var Tau3 = - K[2]*MotorV[2]*MotorV[2];
		var Tau4 =   K[2]*MotorV[3]*MotorV[3];
		var Tau5 =   K[3]*MotorV[4]*MotorV[4];

		var Fn = F1+F2+F3+F4;
		var Taun = Tau1+Tau2+Tau3+Tau4;

		// Wind Frame Variables
		V = Math.Sqrt(Vel.x*Vel.x + Vel.y*Vel.y + Vel.z*Vel.z);

		alpha = Math.Atan2(Vel.z,Vel.y)*todeg;

		beta = Math.Asin(Vel.y/V)*todeg;
		if (V == 0)
  			beta = 0;
		else
  			beta = Math.Asin(Vel.y/V)*todeg;

		// Aerodynamic Coefficients: ['CFX','CFY','CFZ','CMX','CMY','CMZ']
		adb_C = new coefficients.adbCoefficients(alpha,beta,adb);
		le_C = new double[9]{1, dl, Math.Pow(dl,2), Math.Pow(dl,3), Math.Pow(dl,4), Math.Pow(dl,5), Math.Pow(dl,6), Math.Pow(dl,7), Math.Pow(dl,8)}*l_elevon;
		re_C = new double[9]{1, dr, Math.Pow(dr,2), Math.Pow(dr,3), Math.Pow(dr,4), Math.Pow(dr,5), Math.Pow(dr,6), Math.Pow(dr,7), Math.Pow(dr,8)}*r_elevon;
		rudder_C = new double[9]{1, drd, Math.Pow(drd,2), Math.Pow(drd,3), Math.Pow(drd,4), Math.Pow(drd,5), Math.Pow(drd,6), Math.Pow(drd,7), Math.Pow(drd,8)}*rudder;

		// Forces and moments
		Fthrust = new Vector3((float)F5, (float)Fn, 0);
		Fgravity = new Vector3((float) (-M*g*Math.Sin(theta*todeg)),(float) (-M*g*Math.Cos(phi*todeg)*Math.Cos(theta*todeg)),(float) (M*g*Math.Sin(phi*todeg)*Math.Cos(theta*todeg)));
		Faerodynamics = new Vector3((float) (0.5 * rho * V*V * (S * adb_C[1]+ eS * le_C[1] + eS * re_C[1] + rS * rudder_C[1])),(float) (-0.5 * rho * V*V * (S * adb_C[3] + eS * le_C[3] + eS * re_C[3] + rS * rudder_C[3])),(float) (0.5 * rho * V*V * (S * adb_C[2] + eS * le_C[2] + eS * re_C[2] + rS * rudder_C[2])));
		
		Mthrust = new Vector3((float) (L1*((F1 + F3) - (F2 + F4)) + Tau5),(float) -Taun ,(float) (L2*((F1 + F2) - (F3 + F4))));
		Maerodynamics = new Vector3((float) (0.5 * rho * V*V * C * (S * adb_C[4] + eS * le_C[4] + eS * re_C[4] + rS * rudder_C[4])),(float) (-0.5 * rho * V*V * C * (S * adb_C[6] + eS * le_C[6] + eS * re_C[6] + rS * rudder_C[6])),(float) (0.5 * rho * V*V * C * (S * adb_C[5] + eS * le_C[5] + eS * re_C[5] + rS * rudder_C[5])));

		resultantTorque = Mthrust + Maerodynamics;
		resultantForce = Fgravity + Fthrust + Faerodynamics;

		AddTorque(resultantTorque);
		AddCentralForce(resultantForce);
	}
}