using Godot;
using System;
using static Mathf;

public class coefficients
{
    private float alpha;
    private float adb_w_hat;
    private int adb_order;
    private int i;
    public static float[6] data;

    public float* adbCoefficients(alpha,beta,adb)
    {
        // data = {'CFX','CFY','CFZ','CMX','CMY','CMZ'}

        alpha = [1 alpha alpha^2 alpha^3 alpha^4 alpha^5 alpha^6 alpha^7 alpha^8];
        adb_order = length(alpha) - 1;
        adb_w_hat = adb;

        if beta >= -180 && beta < -175
        i = 1;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -175 && beta < -170
        i = 2;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -170 && beta < -160
        i = 3;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -160 && beta < -150
        i = 4;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -150 && beta < -140
        i = 5;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -140 && beta < -130
        i = 6;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -130 && beta < -120
        i = 7;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -120 && beta < -110
        i = 8;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -110 && beta < -100
        i = 9;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -100 && beta < -90
        i = 10;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -90 && beta < -80
        i = 11;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -80 && beta < -70
        i = 12;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -70 && beta < -60
        i = 13;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -60 && beta < -50
        i = 14;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -50 && beta < -40
        i = 15;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -40 && beta < -30
        i = 16;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -30 && beta < -25
        i = 17;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -25 && beta < -21
        i = 18;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -21 && beta < -18
        i = 19;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -18 && beta < -15
        i = 20;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -15 && beta < -12
        i = 21;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -12 && beta < -10
        i = 22;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -10 && beta < -8
        i = 23;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -8 && beta < -6
        i = 24;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -6 && beta < -4
        i = 25;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -4 && beta < -2
        i = 26;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= -2 && beta < 0
        i = 27;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 0 && beta < 2
        i = 28;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 2 && beta < 4
        i = 29;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 4 && beta < 6
        i = 30;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 6 && beta < 8
        i = 31;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 8 && beta < 10
        i = 32;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 10 && beta < 12
        i = 33;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 12 && beta < 15
        i = 34;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 15 && beta < 18
        i = 35;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 18 && beta < 21
        i = 36;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 21 && beta < 25
        i = 37;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 25 && beta < 30
        i = 38;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 30 && beta < 40
        i = 39;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 40 && beta < 50
        i = 40;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 50 && beta < 60
        i = 41;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 60 && beta < 70
        i = 42;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 70 && beta < 80
        i = 43;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 80 && beta < 90
        i = 44;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 90 && beta < 100
        i = 45;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 100 && beta < 110
        i = 46;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 110 && beta < 120
        i = 47;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 120 && beta < 130
        i = 48;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 130 && beta < 140
        i = 49;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 140 && beta < 150
        i = 50;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 150 && beta < 160
        i = 51;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 160 && beta < 170
        i = 52;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 170 && beta < 175
        i = 54;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta >= 175 && beta < 180
        i = 55;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        if beta == 180
        i = 56;
        data = alpha * adb_w_hat((adb_order+1)*i:(adb_order+1)*i + adb_order,:); % predict output
        end
        return(data);
    } 
}