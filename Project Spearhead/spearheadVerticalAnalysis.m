%% QUADCOPTER STABILITY ANALYSIS AT EQUILIBRIUM

close all; % close all figures
clear;     % clear workspace variables
clc;       % clear command window
format short;

%% Octave Packeges
%pkg load control;
%pkg load symbolic;

warning ("off");
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

%% U = [u1 u2 u3 u4 u5 u6 u7 u8] K = Motor Gain Mau = Time Constant Ku = Input Gain
K = [1.123*10^-6; 2.25*10^-6; 7.708e-10 * 1000; 7.708e-10 * 1500];
Mtau = [1/44.22; 1/44.22; 1/20];
Ku = [8.18; 7.02; 0.09];

%% Jacobian Linearisation
% X = [u v w p q r x y z phi theta psi w1 w2 w3 w4]
syms x1 x2 x3 x4 x5 x6 x7 x8 x9 x10 x11 x12 x13 x14 x15 x16
syms u1 u2 u3 u4

Xs = [x1 x2 x3 x4 x5 x6 x7 x8 x9 x10 x11 x12 x13 x14 x15 x16].';
Us = [u1 u2 u3 u4].';

dX = spearheadVerticalModel(0,Xs,Us);

%% Equilibrium Points

%W_e = ((-4*K(2)) + sqrt((4*K(2))^2 - (4*(-M*g)*(4*K(1)))))/(2*(4*K(1)))*ones(4,1);
W_e = sqrt(M*g/(4*K(1)))*ones(4,1);
U_e = (W_e/(Ku(1)*Mtau(1)));
X_e = [0;0;0;0;0;0;x7;x8;x9;0;0;0;W_e];

%% Jacobian Matrices

JA = jacobian(dX,Xs.');
JB = jacobian(dX,Us.');

%% Define Discrete-Time MCU Dynamics

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

% C = 16x16 matrix
C = [1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0;
     0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0;
     0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0;
     0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0;
     0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0;
     0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0;
     0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0;
     0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0;
     0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0;
     0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0;
     0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0;
     0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0;
     0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0;
     0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0;
     0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0;
     0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1];


% D = 16x4 matrix
D = [0, 0, 0, 0;
     0, 0, 0, 0;
     0, 0, 0, 0;
     0, 0, 0, 0;
     0, 0, 0, 0;
     0, 0, 0, 0;
     0, 0, 0, 0;
     0, 0, 0, 0;
     0, 0, 0, 0;
     0, 0, 0, 0;
     0, 0, 0, 0;
     0, 0, 0, 0;
     0, 0, 0, 0;
     0, 0, 0, 0;
     0, 0, 0, 0;
     0, 0, 0, 0];

%% Discrete-Time System

sysdt = c2d(ss(A,B,C,D),T,'zoh');  % Generate Discrete-Time System

Adt = sysdt.a;
Bdt = sysdt.b;
Cdt = sysdt.c;
Ddt = sysdt.d;

%% System Characteristics

poles = eig(Adt);

figure(1);
plot(poles,'*');
grid on;
title('Discrete System Eigenvalues')

cntr = rank(ctrb(Adt,Bdt));

obs = rank(obsv(Adt,Cdt));
