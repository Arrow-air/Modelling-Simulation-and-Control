function [dX] = spearheadModel(k,X,U,adb,l_elevon,r_elevon,rudder)
%% Xdot = f(X,U)

%% Mass of the VTOL Craft as Taken From CAD
M = 20;
g = 9.81;
rho = 1.225; % sea level air density

%% VTOL Dimensions
% CG at [0.6, 0, 0] m.
L1 = 1.65;
L2 = 1.425;
C = 0.32; % Wing Mean Cord Length
S = 2.24; % Wing Planform Area
eS = 0.28*0.58;% Elevon Planform Area chord x span
rS = 0.32*0.30;% Rudder Planform Area chord x span

%% Mass Moment of Inertia about the COM As Taken From CAD
Ixx = 5.061E+05;
Iyy = 5.061E+05;
Izz = 10.011E+05;

%% X = [u v w p q r x y z phi theta psi w1 w2 w3 w4 w5 dl dr drd]
% Linear and Angular Velocity in Fb (m/s and rads/s)
u = X(1);
v = X(2);
w = X(3);
p = X(4);
q = X(5);
r = X(6);
% Linear and Angular position in Fe (meters and rads)
x = X(7);
y = X(8);
z = X(9);
phi = X(10);
theta = X(11);
psi = X(12);
% Motor angular velocity (RPM)
w1 = X(13);
w2 = X(14);
w3 = X(15);
w4 = X(16);
w5 = X(17);
% Control Surface Defelection Angles(degrees)
dl = X(18);
dr = X(19);
drd = X(20);

%% U = [u1 u2 u3 u4 u5 u6 u7 u8] K = Motor Gain Mau = Time Constant Ku = Input Gain
K = [1.123*10^-6; 2.25*10^-6; 7.708*10^-7; 18.708*10^-7];
Mtau = [1/44.22; 1/44.22; 1/20];
Ku = [44.2205*8.18; 44.2205*7.02; 3.6];

F1 = K(1)*w1*w1;
F2 = K(1)*w2*w2;    
F3 = K(1)*w3*w3;
F4 = K(1)*w4*w4;

F5 = K(2)*w5*w5;

Tau1 = K(3)*w1*w1;
Tau2 = -K(3)*w2*w2;
Tau3 = -K(3)*w3*w3;
Tau4 = K(3)*w4*w4;

Tau5 = K(4)*w5*w5;

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
FAx = 0.5 * rho * V*V * (S * adb_C(1) + eS * le_C(1) + eS * re_C(1) + rS * rudder_C(1));
FAy = 0.5 * rho * V*V * (S * adb_C(2) + eS * le_C(2) + eS * re_C(2) + rS * rudder_C(2));
FAz = 0.5 * rho * V*V * (S * adb_C(3) + eS * le_C(3) + eS * re_C(3) + rS * rudder_C(3));

%% Moments: Thrust
LT = L1*((F1 + F3) - (F2 + F4)) + Tau5;
MT = L2*((F1 + F2) - (F3 + F4));
NT = Taun;

%% Moments: Aerodynamic [Roll,Pitch,Yaw]
LA = 0.5 * rho * V*V * C * (S * adb_C(4) + eS * le_C(4) + eS * re_C(4) + rS * rudder_C(4));
MA = 0.5 * rho * V*V * C * (S * adb_C(5) + eS * le_C(5) + eS * re_C(5) + rS * rudder_C(5));
NA = 0.5 * rho * V*V * C * (S * adb_C(6) + eS * le_C(6) + eS * re_C(6) + rS * rudder_C(6));

%% 6-DOF Dynamics: dX = [udot vdot wdot pdot qdot rdot xdot ydot zdot phidot thetadot psidot]
dX1 = FGx + FAx/M + FTx/M - q*w + r*v;%# + g*sin(theta);
dX2 = FGy + FAy/M + FTy/M - r*u + p*w;
dX3 = FGz + FAz/M + FTz/M - p*v + q*u;%# - g*cos(theta);
dX4 = LA/Ixx + LT/Ixx - q*r*(Izz - Iyy)/Ixx;
dX5 = MA/Iyy + MT/Iyy - p*r*(Ixx - Izz)/Iyy;
dX6 = NA/Izz + NT/Izz - p*q*(Iyy - Ixx)/Izz;

dX7 = u*(cos(theta)*cos(psi)) + v*(sin(phi)*sin(theta)*cos(psi) - cos(phi)*sin(psi)) + w*(cos(phi)*sin(theta)*cos(psi) + sin(phi)*sin(psi));
dX8 = u*(cos(theta)*sin(psi)) + v*(sin(phi)*sin(theta)*sin(psi) + cos(phi)*cos(psi)) + w*(cos(phi)*sin(theta)*sin(psi) - sin(phi)*cos(psi));
dX9 = -u*(sin(theta)) + v*(sin(phi)*cos(theta)) + w*(cos(phi)*cos(theta));
dX10 = p + q*(sin(phi)*tan(theta)) + r*(cos(phi)*tan(theta));
dX11 = q*cos(phi) - r*sin(phi);
dX12 = q*(sin(phi)/cos(theta)) + r*(cos(phi)/cos(theta));

%% Motor Dynamics: dX = [w1dot w2dot w3dot w4dot w5dot], U = Pulse Width of the pwm signal 0-1000
dX13 = -(1/Mtau(1))*w1 + Ku(1)*U(1);
dX14 = -(1/Mtau(1))*w2 + Ku(1)*U(2);
dX15 = -(1/Mtau(1))*w3 + Ku(1)*U(3);
dX16 = -(1/Mtau(1))*w4 + Ku(1)*U(4);
dX17 = -(1/Mtau(2))*w5 + Ku(2)*U(5);

%% Servo Dynamics: dX = [w6dot w7dot w8dot], U = Pulse Width of the pwm signal 0-1000
dX18 = -(1/Mtau(3))*dl + Ku(3)*U(6);
dX19 = -(1/Mtau(3))*dr + Ku(3)*U(7);
dX20 = -(1/Mtau(3))*drd + Ku(3)*U(8);

dX = [dX1;dX2;dX3;dX4;dX5;dX6;dX7;dX8;dX9;dX10;dX11;dX12;dX13;dX14;dX15;dX16;dX17;dX18;dX19;dX20];
end
