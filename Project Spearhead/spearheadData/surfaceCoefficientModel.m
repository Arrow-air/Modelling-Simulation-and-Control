clear;
clc;
close all;

%% Load Data
l_elevon = importdata('AerodynamicData/l_elevon.dat');
r_elevon = importdata('AerodynamicData/r_elevon.dat');
rudder = importdata('AerodynamicData/rudder.dat');

le_deflection = l_elevon.data(:,1);
re_deflection = r_elevon.data(:,1);
rudder_deflection = rudder.data(:,1);

le_fnm = l_elevon.data(:,2:end);
re_fnm = r_elevon.data(:,2:end);
rudder_fnm = rudder.data(:,2:end);

deflection_star = (-20:1:20)';

%% Compute Weights
d_order = 8;
r_order = 8;

le_w_hat = leastSquares(le_fnm,le_deflection,d_order);
re_w_hat = leastSquares(re_fnm,re_deflection,d_order);
rudder_w_hat = leastSquares(rudder_fnm,re_deflection,r_order);

%%
d_Psi_star = makepolymat(deflection_star,d_order); % out of sample Regressor Matrix
r_Psi_star = makepolymat(deflection_star,r_order); % out of sample Regressor Matrix

le_star_hat = d_Psi_star*le_w_hat; % predict output
re_star_hat = d_Psi_star*re_w_hat; % predict output
rudder_star_hat = r_Psi_star*rudder_w_hat; % predict output

%% Plot Data
figure(1);
plot(le_deflection,le_fnm,'*','linewidth',1); % plot raw data
hold on;
plot(deflection_star,le_star_hat,'linewidth',1); % plot function estimate
legend('Data','Estimate');

figure(2);
plot(re_deflection,re_fnm,'*','linewidth',1); % plot raw data
hold on;
plot(deflection_star,re_star_hat,'linewidth',1); % plot function estimate
legend('Data','Estimate');

figure(3);
plot(rudder_deflection,rudder_fnm,'*','linewidth',1); % plot raw data
hold on;
plot(deflection_star,rudder_star_hat,'linewidth',1); % plot function estimate
legend('Data','Estimate');

%%
dlmwrite("ConfigurationData/re_w_hat.txt", re_w_hat,' ', 0, 0);
dlmwrite("ConfigurationData/le_w_hat.txt", le_w_hat,' ', 0, 0);
dlmwrite("ConfigurationData/rudder_w_hat.txt", rudder_w_hat,' ', 0, 0);

dlmwrite("ConfigurationData/re_w_hatSim.txt", re_w_hat,',', 0, 0);
dlmwrite("ConfigurationData/le_w_hatSim.txt", le_w_hat,',', 0, 0);
dlmwrite("ConfigurationData/rudder_w_hatSim.txt", rudder_w_hat,',', 0, 0);
