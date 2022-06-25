using Godot;
using System;
using static Mathf;
using coefficients;

public class spearhead : RigidBody
{
	//private int rotationDir;
	// Aerodynamics
	public float adb_C[6];
	public float le_C[6];
	public float re_C[6];
	public float rudder_C[6];
	//adb = importdata('../../datafiles/adb_w_hat.txt');
	//l_elevon = importdata('../../datafiles/le_w_hat.txt');
	//r_elevon = importdata('../../datafiles/re_w_hat.txt');
	//rudder = importdata('../../datafiles/rudder_w_hat.txt');
	// VTOL Dimensions
	private float M;
	private float g;
	private float L1;
	private float L2;
	private float C;
	private float S;
	private float eS;
	private float rS;
    private float rho; // air desnsity at sea level

	// Mass Moment of Inertia about the COM As Taken From CAD
	private float Ixx;
	private float Iyy;
	private float Izz;
	private float Ixz;

	// X = [u v w p q r x y z phi theta psi w1 w2 w3 w4 w5 dl dr drd]
	private Vector3 Vel;
	private Vector3 Accel;
	private Vector3 Pos;
	private Vector3 Angle;
	private float MotorV[5];
	private float dl;
	private float dr;
	private float drd;


	// U = [u1 u2 u3 u4 u5 u6 u7 u8]
	private float U[8];
	private float K[6];
	private float Mtau[3]; 
	private float Ku[3];

	// Input Force and Torque Vectors
	private Vector3 Fthrust;
	private Vector3 Fgravity;
	private Vector3 Faerodynamics;
	private Vector3 Mthrust;
	private Vector3 Maerodynamics;
	public Vector3 resultantTorque;
	public Vector3 resultantForce;

	// Wind Frame Variables
	private float todeg;
	private float V;
	private float alpha;
	private float beta;

	public override void _Ready()
    {
        base._Ready();
		/*
        Vthrust = new Vector3(0,1000,0);
		Hthrust = new Vector3(1000,0,0);
		Vtorque = new Vector3(100000,100000,100000);
		Htorque = new Vector3(100000,100000,100000);
		*/
		// Build In Ridig Body 6-DOF Equations of Motion
		// N E D -> X Z -Y -> X Y Z
		M = 20;
		g = 9.81;
		L1 = 0.6;
		L2 = 0.6;
		C = 0.32;
		S = 2.24;
		eS = 0.28*0.58; // Elevon Planform Area chord x span
		rS = 0.32*0.30; // Rudder Planform Area chord x span

		rho = 1.225;

		Ixx = 5.061E+05;
		Iyy = 5.061E+05;
		Izz = 10.011E+05;

		K = {1.812e-07 * 10, 0.0007326 * 10, 1.812e-07 * 20, 0.0007326 * 20, 7.708e-10 * 80, 7.708e-10 * 15};
		Mtau = {1/44.22, 1/44.22, 1/20};
		Ku = {515.5/2, 515.5/4, 0.09};

		todeg = 180/3.142;
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
		MotorV = {0};
		dl = 0;
		dr = 0;
		drd = 0;
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

		/*
		%% U = [u1 u2 u3 u4 u5 u6 u7 u8] K = Motor Gain Mau = Time Constant Ku = Input Gain
		K = [1.812e-07 * 10; 0.0007326 * 10; 1.812e-07 * 20; 0.0007326 * 20; 7.708e-10 * 80; 7.708e-10 * 15];
		Mtau = [1/44.22; 1/44.22; 1/20];
		Ku = [515.5/2; 515.5/4; 0.09];

		F1 = K(1)*w1*w1 + K(2)*w1;
		F2 = K(1)*w2*w2 + K(2)*w2;
		F3 = K(1)*w3*w3 + K(2)*w3;
		F4 = K(1)*w4*w4 + K(2)*w4;
		F5 = K(3)*w5*w5 + K(4)*w5;

		Tau1 = K(5)*w1*w1;
		Tau2 = -K(5)*w2*w2;
		Tau3 = -K(5)*w3*w3;
		Tau4 = K(5)*w4*w5;
		Tau5 = K(6)*w5*w5;

		Fn = F1+F2+F3+F4;
		Taun = Tau1+Tau2+Tau3+Tau4;

		%% Wind Frame Variables: V = Airspeed alpha = AngleofAttack beta = AngleofSideslip
		todeg = 180/pi;
		V = sqrt(u*u + v*v + w*w);
		alpha = atan2(w,u)*todeg;

		if V == 0
		beta = 0;
		else
		beta = asin(v/V)*todeg;
		end

		%% Aerodynamic Coefficients: ['CFX','CFY','CFZ','CMX','CMY','CMZ']
		adb_C = adbCoefficients(alpha,beta,adb);
		le_C = [1 dl dl^2 dl^3 dl^4 dl^5 dl^6 dl^7 dl^8]*l_elevon;
		re_C = [1 dr dr^2 dr^3 dr^4 dr^5 dr^6 dr^7 dr^8]*r_elevon;
		rudder_C = [1 drd drd^2 drd^3 drd^4 drd^5 drd^6 drd^7 drd^8]*rudder;

		%% Forces: Thrust
		FTx = F5;
		FTy = 0;
		FTz = -Fn;

		%% Forces: Gravity
		FGx = -g*sin(theta);
		FGy = g*sin(phi)*cos(theta);
		FGz = g*cos(phi)*cos(theta);

		%% Forces: Aerodynamic [Drag,Sideforce,Lift]
		Drag = 0.5 * rho * V*V * (S * adb_C(1)*alpha + eS * le_C(1)*dl + eS * re_C(1)*dr + rS * rudder_C(1)*drd);
		sideforce = 0.5 * rho * V*V * (S * adb_C(2)*beta + eS * le_C(2)*dl + eS * re_C(2)*dr + rS * rudder_C(2)*drd);
		Lift = 0.5 * rho * V*V * (S * adb_C(3)*alpha + eS * le_C(3)*dl + eS * re_C(3)*dr + rS * rudder_C(3)*drd);

		FAx = Drag;
		FAy = sideforce;
		FAz = Lift;

		%% Moments: Thrust
		LT = L1*((F1 + F3) - (F2 + F4)) + Tau5;
		MT = L2*((F1 + F2) - (F3 + F4));
		NT = Taun;

		%% Moments: Aerodynamic
		LA = 0.5 * rho * V*V * C * (S * adb_C(4) + eS * le_C(4)*dl + eS * re_C(4)*dr + rS * rudder_C(4)*drd);
		MA = 0.5 * rho * V*V * C * (S * adb_C(5)*alpha + eS * le_C(5)*dl + eS * re_C(5)*dr + rS * rudder_C(5)*drd);
		NA = 0.5 * rho * V*V * C * (S * adb_C(6)*beta + eS * le_C(6)*dl + eS * re_C(6)*dr + rS * rudder_C(6)*drd);
		*/

		// States
		Vel = new Vector3(spearhead.linear_velocity.x,spearhead.linear_velocity.z,-spearhead.linear_velocity.y);
		Accel = new Vector3(spearhead.angular_velocity.x,spearhead.angular_velocity.z,-spearhead.angular_velocity.y);

		// Inputs
		MotorV = {Ku[1]*U[1],Ku[1]*U[2],Ku[1]*U[3],Ku[1]*U[4],Ku[2]*U[5]};
		dl = Ku[3]*U[6];
		dr = Ku[3]*U[7];
		drd = Ku[3]*U[8];

		// Motors
		F1 = K[0]*MotorV[0]*MotorV[0] + K[1]*MotorV[0];
		F2 = K[0]*MotorV[1]*MotorV[1] + K[1]*MotorV[1];
		F3 = K[0]*MotorV[2]*MotorV[2] + K[1]*MotorV[2];
		F4 = K[0]*MotorV[3]*MotorV[3] + K[1]*MotorV[3];
		F5 = K[2]*MotorV[4]*MotorV[4] + K[3]*MotorV[4];

		Tau1 =   K[4]*MotorV[0]*MotorV[0];
		Tau2 = - K[4]*MotorV[1]*MotorV[1];
		Tau3 = - K[4]*MotorV[2]*MotorV[2];
		Tau4 =   K[4]*MotorV[3]*MotorV[3];
		Tau5 =   K[5]*MotorV[4]*MotorV[4];

		Fn = F1+F2+F3+F4;
		Taun = Tau1+Tau2+Tau3+Tau4;

		// Wind Frame Variables
		V = sqrt(Vel.x*Vel.x + Vel.y*Vel.y + Vel.z*Vel.z);

		alpha = atan2(Vel.z,Vel.y)*todeg;

		beta = asin(Vel.y/V)*todeg;
		if (V == 0)
  			beta = 0;
		else
  			beta = asin(Vel.y/V)*todeg;

		// Aerodynamic Coefficients: ['CFX','CFY','CFZ','CMX','CMY','CMZ']
		adb_C = new coefficients.adbCoefficients(alpha,beta,adb);
		le_C = {1 dl dl^2 dl^3 dl^4 dl^5 dl^6 dl^7 dl^8}*l_elevon;
		re_C = {1 dr dr^2 dr^3 dr^4 dr^5 dr^6 dr^7 dr^8}*r_elevon;
		rudder_C = {1 drd drd^2 drd^3 drd^4 drd^5 drd^6 drd^7 drd^8}*rudder;

		// Forces and moments
		Fthrust = new Vector3(F5, Fn, 0);
		Fgravity = new Vector3(-m*g*sin(theta), -M*g*cos(phi)*cos(theta), M*g*sin(phi)*cos(theta));
		Faerodynamics = new Vector3(0.5 * rho * V*V * (S * adb_C[1]*alpha + eS * le_C[1]*dl + eS * re_C[1]*dr + rS * rudder_C[1]*drd), -0.5 * rho * V*V * (S * adb_C[3]*alpha + eS * le_C[3]*dl + eS * re_C[3]*dr + rS * rudder_C[3]*drd), 0.5 * rho * V*V * (S * adb_C[2]*beta + eS * le_C[2]*dl + eS * re_C[2]*dr + rS * rudder_C[2]*drd));
		
		Mthrust = new Vector3(L1*((F1 + F3) - (F2 + F4)) + Tau5, -Taun , L2*((F1 + F2) - (F3 + F4)));
		Maerodynamics = new Vector3(0.5 * rho * V*V * C * (S * adb_C[4] + eS * le_C[4]*dl + eS * re_C[4]*dr + rS * rudder_C[4]*drd), -0.5 * rho * V*V * C * (S * adb_C[6]*beta + eS * le_C[6]*dl + eS * re_C[6]*dr + rS * rudder_C[6]*drd), 0.5 * rho * V*V * C * (S * adb_C[5]*alpha + eS * le_C[5]*dl + eS * re_C[5]*dr + rS * rudder_C[5]*drd));

		resultantTorque = Mthrust + Maerodynamics;
		resultantForce = Fgravity + Fthrust + Faerodynamics;

		AddTorque(resultantTorque);
		AddCentralForce(resultantForce);
	}
}