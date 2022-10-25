function [dX] = spearheadVerticalModel(k,X,U)
%% Xdot = f(X,U)

%% Mass of the VTOL Craft as Taken From CAD
M = 20;
g = 9.81;

%% VTOL Dimensions
% CG at [0.6, 0, 0] m.
L1 = 1.65;
L2 = 1.425;
%% Mass Moment of Inertia about the COM As Taken From CAD
Ixx = 8.734;
Iyy = 5.592;
Izz = 13.623;

%% X = [u v w p q r x y z phi theta psi w1 w2 w3 w4]
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
% Control Surface Defelection Angles(degrees)

%% U = [u1 u2 u3 u4] K = Motor Gain Mau = Time Constant Ku = Input Gain
K = [1.123*10^-6; 2.25*10^-6; 7.708*10^-7; 18.708*10^-7];
Mtau = [1/44.22; 1/44.22; 1/20];
Ku = [44.2205*8.18; 44.2205*7.02; 3.6];

F1 = K(1)*w1*w1;
F2 = K(1)*w2*w2;
F3 = K(1)*w3*w3;
F4 = K(1)*w4*w4;

Tau1 = K(3)*w1*w1;
Tau2 = -K(3)*w2*w2;
Tau3 = -K(3)*w3*w3;
Tau4 = K(3)*w4*w4;

Fn = F1+F2+F3+F4;
Taun = Tau1+Tau2+Tau3+Tau4;

%% Forces: Thrust
FTx = 0;
FTy = 0;
FTz = -Fn;

%% Forces: Gravity
FGx = -g*sin(theta);
FGy = g*sin(phi)*cos(theta);
FGz = g*cos(phi)*cos(theta);

%% Moments: Thrust
LT = L1*((F1 + F3) - (F2 + F4));
MT = L2*((F1 + F2) - (F3 + F4));
NT = Taun;

%% 6-DOF Dynamics: dX = [udot vdot wdot pdot qdot rdot xdot ydot zdot phidot thetadot psidot]
dX1 = FGx + FTx/M - q*w + r*v;%# + g*sin(theta);
dX2 = FGy + FTy/M - r*u + p*w;
dX3 = FGz + FTz/M - p*v + q*u;%# - g*cos(theta);
dX4 = LT/Ixx - q*r*(Izz - Iyy)/Ixx;
dX5 = MT/Iyy - p*r*(Ixx - Izz)/Iyy;
dX6 = NT/Izz - p*q*(Iyy - Ixx)/Izz;

dX7 = u*(cos(theta)*cos(psi)) + v*(sin(phi)*sin(theta)*cos(psi) - cos(phi)*sin(psi)) + w*(cos(phi)*sin(theta)*cos(psi) + sin(phi)*sin(psi));
dX8 = u*(cos(theta)*sin(psi)) + v*(sin(phi)*sin(theta)*sin(psi) + cos(phi)*cos(psi)) + w*(cos(phi)*sin(theta)*sin(psi) - sin(phi)*cos(psi));
dX9 = -u*(sin(theta)) + v*(sin(phi)*cos(theta)) + w*(cos(phi)*cos(theta));
dX10 = p + q*(sin(phi)*tan(theta)) + r*(cos(phi)*tan(theta));
dX11 = q*cos(phi) - r*sin(phi);
dX12 = q*(sin(phi)/cos(theta)) + r*(cos(phi)/cos(theta));

%% Motor Dynamics: dX = [w1dot w2dot w3dot w4dot], U = Pulse Width of the pwm signal 0-1000
dX13 = -(1/Mtau(1))*w1 + Ku(1)*U(1);
dX14 = -(1/Mtau(1))*w2 + Ku(1)*U(2);
dX15 = -(1/Mtau(1))*w3 + Ku(1)*U(3);
dX16 = -(1/Mtau(1))*w4 + Ku(1)*U(4);

dX = [dX1;dX2;dX3;dX4;dX5;dX6;dX7;dX8;dX9;dX10;dX11;dX12;dX13;dX14;dX15;dX16];
end
