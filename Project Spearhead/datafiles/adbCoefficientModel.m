clear;
clc;
close all;

%% Load Data
adb = importdata('mga_v2_adb.dat');

beta = adb.data(:,1);
alpha = adb.data(:,2);

adb_fnm = adb.data(:,3:8);


alpha_star = (-90:10:90)';
beta_star = (-180:1:180)';

%% Compute Weights
adb_order = 8;
adb_w_hat = zeros((adb_order+1)*56,6);
i=1;
for j = 1:19:1045
  adb_w_hat(i*(adb_order+1):i*(adb_order+1)+adb_order,:) = leastSquares(adb_fnm(0+j:18+j,:),alpha(0+j:18+j),adb_order);
  i=i+1;
end

adb_Psi_star = makepolymat(alpha_star,adb_order); % out of sample Regressor Matrix

adb_star_hat = adb_Psi_star *adb_w_hat((adb_order+1)*28:(adb_order+1)*28 + adb_order,:); % predict output

figure(1);
plot(alpha(516:516+19),adb_fnm(516:516+19,5),'*','linewidth',1); % plot raw data
hold on;
plot(alpha_star,adb_star_hat(:,5),'linewidth',1); % plot function estimate
legend('Data','Estimate');

dlmwrite("adb_w_hat.txt", adb_w_hat,' ', 0, 0);
