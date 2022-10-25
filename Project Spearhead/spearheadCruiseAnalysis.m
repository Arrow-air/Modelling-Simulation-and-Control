%% STATIC STABILITY ANALYSIS, COEFFICIENT OF PITCH MOMENT AGAINST ANGLE OF ATTACK
clear;
clc;
close all;
%%
adb = importdata('spearheadData/ConfigurationData/adb_w_hat.txt');
alpha = (-90:90);
beta = 0;
Cm = zeros(181,1);

for k = 1:181
    data = adbCoefficients(alpha(k),beta,adb);
    Cm(k) = data(5);
end

plot(alpha,Cm);