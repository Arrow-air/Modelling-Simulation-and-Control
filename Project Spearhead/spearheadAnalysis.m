close all; % close all figures
clear;     % clear workspace variables
clc;       % clear command window
format short;

%% Octave Packeges
pkg load control;
pkg load symbolic;

warning ("off");

%% Import Aerodynamic data
adb = importdata('datafiles/adb_w_hat.txt');
l_elevon = importdata('datafiles/le_w_hat.txt');
r_elevon = importdata('datafiles/re_w_hat.txt');
rudder = importdata('datafiles/rudder_w_hat.txt');

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

%% U = [u1 u2 u3 u4 u5 u6 u7 u8] K = Motor Gain Mau = Time Constant Ku = Input Gain
K = [1.123*10^-6; 2.25*10^-6; 7.708e-10 * 1000; 7.708e-10 * 1500];
Mtau = [1/44.22; 1/44.22; 1/20];
Ku = [8.18; 7.02; 0.09];

%% Jacobian Linearisation
syms x1 x2 x3 x4 x5 x6 x7 x8 x9 x10 x11 x12 x13 x14 x15 x16 x17 x18 x19 x20
syms u1 u2 u3 u4 u5 u6 u7 u8

Xs = [x1 x2 x3 x4 x5 x6 x7 x8 x9 x10 x11 x12 x13 x14 x15 x16 x17 x18 x19 x20].';
Us = [u1 u2 u3 u4 u5 u6 u7 u8].';

dX = spearheadModel(0,Xs,Us,adb,l_elevon,r_elevon,rudder);

%% Equilibrium Points

#W_e = ((-4*K(2)) + sqrt((4*K(2))^2 - (4*(-M*g)*(4*K(1)))))/(2*(4*K(1)))*ones(4,1);
W_e = sqrt(M*g/(4*K(1)))*ones(4,1);
U_e = [(W_e/(Ku(1)*Mtau(1)));u5;0;0;0];
X_e = [x1;x2;x3;0;0;0;x7;x8;x9;0;0;0;W_e;x17;0;0;0];

%% Jacobian Matrices

JA = jacobian(dX,Xs.');
JB = jacobian(dX,Us.');

%% Define Discrete-Time BeagleBone Dynamics

T = 0.01; % Sample period (s)- 100Hz
ADC = 3.3/((2^12)-1); % 12-bit ADC Quantization
DAC =  3.3/((2^12)-1); % 12-bit DAC Quantization

%% Define Linear Continuous-Time Multirotor Dynamics: x_dot = Ax + Bu, y = Cx + Du

JA1 = subs(JA,Xs,X_e);
A = subs(JA1,Us,U_e);
A = eval(A);

JB1 = subs(JB,Xs,X_e);
B = subs(JB1,Us,U_e);
B = eval(B);

% C = 8x26 matrix
C = [1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0;
     0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0;
     0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0;
     0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0;
     0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0;
     0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0;
     0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0;
     0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1];

% D = 8x8 matrix
D = [0, 0, 0, 0, 0, 0, 0, 0;
     0, 0, 0, 0, 0, 0, 0, 0;
     0, 0, 0, 0, 0, 0, 0, 0;
     0, 0, 0, 0, 0, 0, 0, 0;
     0, 0, 0, 0, 0, 0, 0, 0;
     0, 0, 0, 0, 0, 0, 0, 0;
     0, 0, 0, 0, 0, 0, 0, 0;
     0, 0, 0, 0, 0, 0, 0, 0];

%% Discrete-Time System

#sysdt = c2d(ss(A,B,C,D),T,'zoh');  % Generate Discrete-Time System

#Adt = sysdt.a;
#Bdt = sysdt.b;
#Cdt = sysdt.c;
#Ddt = sysdt.d;

%% System Characteristics

poles = eig(A);

figure(1);
plot(poles,'*');
grid on;
title('Discrete System Eigenvalues')

cntr = rank(ctrb(A,B))

obs = rank(obsv(A,C))
