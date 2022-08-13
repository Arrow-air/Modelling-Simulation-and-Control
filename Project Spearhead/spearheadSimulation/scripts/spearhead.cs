using Godot;
using System;

public class spearhead : RigidBody
{
	private Spatial spearheadPos;
	private RigidBody spearheadVel;
	
	private Coefficients coeffClass;
	
	private ControlSystem[] lPosControlClass = new ControlSystem[3];
	private ControlSystem[] lVelControlClass = new ControlSystem[3];
	private ControlSystem[] aPosControlClass = new ControlSystem[3];
	private ControlSystem[] aVelControlClass = new ControlSystem[3];
	
	// Aerodynamics
	public double[] adb_C;
	public double[] le_C;
	public double[] re_C;
	public double[] rudder_C;
	
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
	private Vector3 aVel;
	private Vector3 Pos;
	private Vector3 aPos;
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
	private Vector3 resultantTorque;
	private Vector3 resultantForce;

	// Wind Frame Variables
	private double todeg;
	private double V;
	private double alpha;
	private double beta;
	private float ground;
	
	public override void _Ready()
	{
		base._Ready();
		
		spearheadPos = GetNode<Spatial>("/root/Environment/spearhead");
		spearheadVel = GetNode<RigidBody>("/root/Environment/spearhead");
		
		coeffClass = new Coefficients();
		
		lPosControlClass[0] = new ControlSystem();
		lPosControlClass[1] = new ControlSystem();
		lPosControlClass[2] = new ControlSystem();
		
		lVelControlClass[0] = new ControlSystem();
		lVelControlClass[1] = new ControlSystem();
		lVelControlClass[2] = new ControlSystem();
		
		aPosControlClass[0] = new ControlSystem();
		aPosControlClass[1] = new ControlSystem();
		aPosControlClass[2] = new ControlSystem();
		
		aVelControlClass[0] = new ControlSystem();
		aVelControlClass[1] = new ControlSystem();
		aVelControlClass[2] = new ControlSystem();
		
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
		
		U = new double[8]{0,0,0,0,0,0,0,0};
		K = new double[4]{1.123*Math.Pow(10,-6), 2.25*Math.Pow(10,-6), 7.708e-10 * 10, 7.708e-10 * 15};
		Mtau = new double[3]{1/44.22, 1/44.22, 1/20};
		Ku = new double[3]{8.18, 7.02, 3.6};//44.2205*

		todeg = 180/Math.PI;
		V = 0;
		alpha = 0;
		beta = 0;
		ground = 143.68f;

		Fthrust = new Vector3(0,0,0);
		Fgravity = new Vector3(0,0,0);
		Faerodynamics = new Vector3(0,0,0);
		Mthrust = new Vector3(0,0,0);
		Maerodynamics = new Vector3(0,0,0);
		resultantTorque = new Vector3(0,0,0);
		resultantForce = new Vector3(0,0,0);

		Vel = new Vector3(0,0,0);
		aVel = new Vector3(0,0,0);
		Pos = new Vector3(0,0,0);
		aPos = new Vector3(0,0,0);
		MotorV = new double[5]{0,0,0,0,0};
		dl = 0;
		dr = 0;
		drd = 0;
		
		lPosControlClass[0].PIDSetup(1f,0f,0f,0.0005f);
		lPosControlClass[1].PIDSetup(1f,0f,0f,0.0005f);
		lPosControlClass[2].PIDSetup(40f,2f,70f,0.001f);
		
		lVelControlClass[0].PIDSetup(1f,0f,0f,0.0005f);
		lVelControlClass[1].PIDSetup(1f,0f,0f,0.0005f);
		lVelControlClass[2].PIDSetup(1f,0f,0f,0.0005f);
		
		aPosControlClass[0].PIDSetup(1f,0f,0f,0.0005f);
		aPosControlClass[1].PIDSetup(1f,0f,0f,0.0005f);
		aPosControlClass[2].PIDSetup(1f,0f,0f,0.0005f);
		
		aVelControlClass[0].PIDSetup(1f,0f,0f,0.0005f);
		aVelControlClass[1].PIDSetup(1f,0f,0f,0.0005f);
		aVelControlClass[2].PIDSetup(1f,0f,0f,0.0005f);
	}

	public override void _IntegrateForces(PhysicsDirectBodyState state)
	{	
		//state.InverseInertia = new Vector3(1/Ixx,1/Izz,1/Iyy);
		
		lPosControlClass[0].PIDRun(state.Step);
		lPosControlClass[1].PIDRun(state.Step);
		lPosControlClass[2].PIDRun(state.Step);
		
		lVelControlClass[0].PIDRun(state.Step);
		lVelControlClass[1].PIDRun(state.Step);
		lVelControlClass[2].PIDRun(state.Step);
		
		aPosControlClass[0].PIDRun(state.Step);
		aPosControlClass[1].PIDRun(state.Step);
		aPosControlClass[2].PIDRun(state.Step);
		
		aVelControlClass[0].PIDRun(state.Step);
		aVelControlClass[1].PIDRun(state.Step);
		aVelControlClass[2].PIDRun(state.Step);
		
		var rollin = float.Parse(Input.GetJoyAxis(0,0).ToString("0.00"));
		var pitchin = float.Parse(Input.GetJoyAxis(0,1).ToString("0.00"));
		var throttle = float.Parse(Input.GetJoyAxis(0,2).ToString("0.00"));
		
		var throttledailbottom = float.Parse(Input.GetJoyAxis(0,3).ToString("0.00"));
		var throttledailtop = float.Parse(Input.GetJoyAxis(0,4).ToString("0.00"));
		var yawin = float.Parse(Input.GetJoyAxis(0,5).ToString("0.00"));
		var throttleslider = float.Parse(Input.GetJoyAxis(0,6).ToString("0.00"));
		
		var leftpeddle = float.Parse(Input.GetJoyAxis(1,0).ToString("0.00"));
		var rightpeddle = float.Parse(Input.GetJoyAxis(1,1).ToString("0.00"));
		var rudderin = float.Parse(Input.GetJoyAxis(1,2).ToString("0.00"));
		//GD.Print("Roll" + rollin + "Pitch" + pitchin + "Throttle" + throttle + "ThrotDailB" + throttledailbottom + "ThrotDailT" + throttledailtop + "Yaw" + yawin + "ThrotSilde" + throttleslider + "Lp" + leftpeddle + "Rp" + rightpeddle + "Rudder" + rudderin);
		
		// States
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
		
		//GD.Print("XPos "+(x).ToString("0.00")+" YPos "+(y).ToString("0.00")+" ZPos "+(z).ToString("0.00")+" Xvel "+(u).ToString("0.00")+" Yvel "+(v).ToString("0.00")+" Zvel "+(w).ToString("0.00")+" Phi "+(phi*todeg).ToString("0.00")+" Theta "+(theta*todeg).ToString("0.00")+" Psi "+(psi*todeg).ToString("0.00"));
		
		// Inputs
		float outp = lPosControlClass[2].PID(-(z+ground),100);
		//GD.Print(-(z+ground)+ " " +outp); (-1000*throttle + 1000)/2
		U =  new double[8]{outp,outp,outp,outp,0, pitchin*200 + rollin*200, pitchin*200 - rollin*200, yawin*200};
		//U =  new double[8]{0,0,0,0,0,0,0,0};
		//GD.Print(U[4].ToString() + U[5].ToString() + U[6].ToString() + U[7].ToString());
		
		MotorV = new double[5]{Ku[0]*U[0],Ku[0]*U[1],Ku[0]*U[2],Ku[0]*U[3],Ku[1]*U[4]};
		dl = Ku[2]*U[5]/8;
		dr = Ku[2]*U[6]/8;
		drd = Ku[2]*U[7]/8;
		
		// Motors
		double F1 = K[0]*MotorV[0]*MotorV[0];
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
		V = Math.Sqrt(u*u + v*v + w*w);
		alpha = Math.Atan2(w,u)*todeg;
		beta = Math.Asin(v/V)*todeg;
		if (V == 0)
  			beta = 0;
		else
  			beta = Math.Asin(v/V)*todeg;

		// Aerodynamic Coefficients: ['CFX','CFY','CFZ','CMX','CMY','CMZ']
		adb_C = coeffClass.adbCoefficients(alpha,beta);
		le_C = coeffClass.dlCoefficients(dl);
		re_C = coeffClass.drCoefficients(dr);
		rudder_C = coeffClass.drdCoefficients(drd);
		
		// Forces and moments
		//GODOT -> N E D = X Z -Y -> X Y Z
		Fthrust = new Vector3((float)F5, (float)Fn, 0);
		//Fgravity = new Vector3((float) (-M*g*Math.Sin(theta*todeg)),(float) (-M*g*Math.Cos(phi*todeg)*Math.Cos(theta*todeg)),(float) (M*g*Math.Sin(phi*todeg)*Math.Cos(theta*todeg)));
		Faerodynamics = new Vector3((float) (0.5* rho * V*V * (S * adb_C[0]+ eS * le_C[0] + eS * re_C[0] + rS * rudder_C[0])),(float) (-0.5 * rho * V*V * (S * adb_C[2] + eS * le_C[2] + eS * re_C[2] + rS * rudder_C[2])),(float) (0.5 * rho * V*V * (S * adb_C[1] + eS * le_C[1] + eS * re_C[1] + rS * rudder_C[1])));
		//GD.Print(Faerodynamics.y);
		Mthrust = new Vector3((float) (L1*((F1 + F3) - (F2 + F4)) /*+ Tau5*/),(float) -Taun ,(float) (L2*((F1 + F2) - (F3 + F4))));
		Maerodynamics = new Vector3((float) (0.5* rho * V*V * C * (S * adb_C[3] + eS * le_C[3] + eS * re_C[3] + rS * rudder_C[3])),(float) (0.5 * rho * V*V * C * (S * adb_C[5] + eS * le_C[5] + eS * re_C[5] + rS * rudder_C[5])),(float) (0.5* rho * V*V * C * (S * adb_C[4] + eS * le_C[4] + eS * re_C[4] + rS * rudder_C[4])));

		resultantTorque = Mthrust + Maerodynamics;
		resultantForce = Fgravity + Fthrust + Faerodynamics;

		AddTorque(resultantTorque);
		AddCentralForce(resultantForce);
		
		//AddTorque(Mthrust);
		//AddCentralForce(Fthrust);
	}
}
