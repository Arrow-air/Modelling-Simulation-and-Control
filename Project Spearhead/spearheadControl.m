clear;
close all;
clc;

%pkg load control;

%% Import Aerodynamic data
adb = importdata('datafiles/adb_w_hat.txt');
l_elevon = importdata('datafiles/le_w_hat.txt');
r_elevon = importdata('datafiles/re_w_hat.txt');
rudder = importdata('datafiles/rudder_w_hat.txt');

%% Dynamic Simulation
g = 9.81;
T = 0.01;
Time = 100;
kT = round(Time/T);

X = zeros(20,kT);
Xest = zeros(20,kT);
Xe = zeros(6,kT);
Y = zeros(6,kT);
e = zeros(6,kT);
U = zeros(8,kT);

X(1,1) = 25;
U_e = [0;0;0;0;0;0;0;0];
PRef = [0;0;0];
VRef = [25;0;0];
ARef = [0;0;0];
RRef = [0;0;0];

Kplv = [200,10,100];
Kilv = [10,5,5];
Kdlv = [20,10,20];

Gpidlv1 = pid(Kplv(1),Kilv(1),Kdlv(1),1,T);
Gpidlv2 = pid(Kplv(2),Kilv(2),Kdlv(2),1,T);
Gpidlv3 = pid(Kplv(3),Kilv(3),Kdlv(3),1,T);

Kpar = [40,80,25];
Kiar = [5,5,5];
Kdar = [0,0,0];

Gpidar1 = pid(Kpar(1),Kiar(1),Kdar(1),1,T);
Gpidar2 = pid(Kpar(2),Kiar(2),Kdar(2),1,T);
Gpidar3 = pid(Kpar(3),Kiar(3),Kdar(3),1,T);

Gpidar = [Gpidar1,Gpidar2,Gpidar3];

Kplp = [100,100,10];
Kilp = [5,5,5];
Kdlp = [30,30,30];

Gpidlp1 = pid(Kplp(1),Kilp(1),Kdlp(1),1,T);
Gpidlp2 = pid(Kplp(2),Kilp(2),Kdlp(2),1,T);
Gpidlp3 = pid(Kplp(3),Kilp(3),Kdlp(3),1,T);

Gpidlp = [Gpidlp1,Gpidlp2,Gpidlp3];

Kpap = [70,70,70];
Kiap = [8,8,8];
Kdap = [50,50,50];

Gpidap1 = pid(Kpap(1),Kiap(1),Kdap(1),1,T);
Gpidap2 = pid(Kpap(2),Kiap(2),Kdap(2),1,T);
Gpidap3 = pid(Kpap(3),Kiap(3),Kdap(3),1,T);

Gpidap = [Gpidap1,Gpidap2,Gpidap3];

Elv = zeros(3,kT);
Ear = zeros(3,kT);
Elp = zeros(3,kT);
Eap = zeros(3,kT);

uvw = zeros(3,kT);
pqr = zeros(3,kT);
xyz = zeros(3,kT);
pts = zeros(3,kT);

ulv = zeros(3,1);
uar = zeros(3,1);
ulp = zeros(3,1);
uap = zeros(3,1);

t_span = [0,T];

for k = 1:kT-1
%%  Estimation
    Xest(:,k) = X(:,k);  % No KF Non Linear Prediction
%{
    Y(:,k) = X([1,3,5,11],k);
    Xest(:,k) = Adt*Xest(:,k-1) + Bdt*(U(:,k-1)-U_e);   % Linear Kalman Prediction
    e(:,k) = [Y(:,k) - Xest([1,3,5,11],k)];
    Xest(:,k) = Xest(:,k) + Ldt*e(:,k);

    Y(:,k) = X([1,3,5,11],k);
    K1 = Quad_Dynamics(k, Xest(:,k-1)         ,U(:,k-1)); # Runge-Kutta 4 Integration
    K2 = Quad_Dynamics(k, Xest(:,k-1) + K1*T/2,U(:,k-1));
    K3 = Quad_Dynamics(k, Xest(:,k-1) + K2*T/2,U(:,k-1));
    K4 = Quad_Dynamics(k, Xest(:,k-1) + K3*T  ,U(:,k-1));
    Xest(:,k) = Xest(:,k-1) + (1/6)*(K1 + 2*K2 + 2*K3 + K4)*T;  % Limited Nonlinear Kalman Prediction
    e(:,k) = [Y(:,k) - Xest([1,3,5,11],k)];
    Xest(:,k) = Xest(:,k) + Ldt*e(:,k);
%}

%%  Control

    uvw(:,k) = Xest(1:3,k);
    Elv(:,k) = (VRef - uvw(:,k))';

    pqr(:,k) = Xest(4:6,k);
    %Ear(:,k) = (RRef - pqr(:,k))';

    xyz(:,k) = Xest(7:9,k);
    Elp(:,k) = (PRef - xyz(:,k))';

    pts(:,k) = Xest(10:12,k);
    %Eap(:,k) = (ARef - pts(:,k))';

    ulv(1) = lsim(Gpidlv1,Elv(1,k));
    ulv(2) = lsim(Gpidlv2,Elv(2,k));
    ulv(3) = lsim(Gpidlv3,Elv(3,k));

    Ua = spearheadAttitudeFW(ARef,pts(:,k),Kpap,pqr(:,k),sqrt(uvw(1,k)^2 + uvw(2,k)^2 + uvw(3,k)^2));
    Ear(:,k) = Ua(1:3);
    pqrRef = Ua(4:6);

    uar(1) = lsim(Gpidar1,Ear(1,k));
    uar(2) = lsim(Gpidar2,Ear(2,k));
    uar(3) = lsim(Gpidar3,Ear(3,k));

    ulp(1) = lsim(Gpidlp1,Elp(1,k));
    ulp(2) = lsim(Gpidlp2,Elp(2,k));
    ulp(3) = lsim(Gpidlp3,Elp(3,k));

    %uap(1) = lsim(Gpidap1,Eap(1,k));
    %uap(2) = lsim(Gpidap2,Eap(2,k));
    %uap(3) = lsim(Gpidap3,Eap(3,k));

    U(:,k) =  min(1000,max(-1000,[0;0;0;0;ulv(1) + ulv(3) + ulp(3); (uar(1)+pqrRef(1))/2 + (uar(2)+pqrRef(2))/2 + ulp(3)/4 + ulp(2)/4; -(uar(1)+pqrRef(1))/2 + (uar(2)+pqrRef(2))/2 + ulp(3)/4 + ulp(2)/4; (uar(1)+pqrRef(3)) + ulv(2) + ulp(2)/2])); %} Constraint Saturation

%%  Simulation
%{
    K1 = spearheadModel(k, Xest(:,k)         ,U(:,k),adb,l_elevon,r_elevon,rudder); % Runge-Kutta4 Integration Nonlinear Dynamics
    K2 = spearheadModel(k, Xest(:,k) + K1*T/2,U(:,k),adb,l_elevon,r_elevon,rudder);
    K3 = spearheadModel(k, Xest(:,k) + K2*T/2,U(:,k),adb,l_elevon,r_elevon,rudder);
    K4 = spearheadModel(k, Xest(:,k) + K3*T  ,U(:,k),adb,l_elevon,r_elevon,rudder);
    X(:,k+1) = Xest(:,k) + (1/6)*(K1 + 2*K2 + 2*K3 + K4)*T;
%}

    t_span = [0,T];
    xode = ode45(@(t,X) spearheadModel(t,Xest(:,k),U(:,k),adb,l_elevon,r_elevon,rudder),t_span,Xest(:,k)); % Runge-Kutta45 Integration Nonlinear Dynamics
    X(:,k+1) = xode.y(:,end);

end

%PROT = profile("info");
%profile off;

Rad2Deg = [180/pi,180/pi,180/pi]';

%Plots
t = (0:kT-1)*T;
figure(1);
plot(t,X([1,2,3],:),'linewidth',2);
legend('u','v','w');
title('Linear Velocity');
xlabel('Time(s)');
ylabel('Meters/second (m/s)');

figure(2);
plot(t,X([4,5,6],:).*Rad2Deg,'linewidth',2);
legend('p','q','r');
title('Angular Velocity');
xlabel('Time(s)');
ylabel('Degrees/second (\deg /s)');

figure(3);
plot(t,X([7,8,9],:),'linewidth',2);
legend('X','Y','Z');
title('Position');
xlabel('Time(s)');
ylabel('Meters(m)');

figure(4);
plot(t,X([10,11,12],:).*Rad2Deg,'linewidth',2);
legend('\phi','\theta','\psi');
title('Attitude');
xlabel('Time(s)');
ylabel('Degrees(\deg)');

figure(5);
plot(t,U(1:4,:),'linewidth',2);
legend('Front Left','Front Right','Rear Left','Rear Right');
title('Vertical Motor PWM Signals');
xlabel('Time(s)');
ylabel('MicroSeconds(\mu s)');

figure(6);
plot(t,U(5,:),'linewidth',2);
legend('Throttle');
title('Throttle Input PWM Signal');
xlabel('Time(s)');
ylabel('MicroSeconds(\mu s)');

figure(7);
plot(t,U(6:8,:),'linewidth',2);
legend('Left Elevon','Right Elevon','Rudder');
title('Surface Deflection Input PWM Signals');
xlabel('Time(s)');
ylabel('MicroSeconds(\mu s)');