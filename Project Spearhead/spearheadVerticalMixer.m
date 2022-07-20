M = 20;
L1 = 1.65;
L2 = 1.425;
Ixx = 5.061E+05;
Iyy = 5.061E+05;
Izz = 10.011E+05;
K = 1.123*10^-6;
K2 = 7.708*10^-7;
Ku = 44.2205*8.18;

u = [0.5;0.5;0.5;0.5];

Usqr = [L1*K*(Ku^2)/Ixx,-L1*K*(Ku^2)/Ixx,L1*K*(Ku^2)/Ixx,-L1*K*(Ku^2)/Ixx;
        L2*K*(Ku^2)/Iyy,L2*K*(Ku^2)/Iyy,-L2*K*(Ku^2)/Iyy,-L2*K*(Ku^2)/Iyy;
        K2*(Ku^2)/Izz,   -K2*(Ku^2)/Izz,  -K2*(Ku^2)/Izz,   K2*(Ku^2)/Izz;
        -K*(Ku^2)/M,     -K*(Ku^2)/M,     -K*(Ku^2)/M,     -K*(Ku^2)/M]\u;

y = sqrt(abs(Usqr))