using Godot;
using System;
using static Mathf;

public class coefficients
{
    private double[] alphaArray;
    private double[] adb_w_hat;
    private int adb_order;
    private int i;
    public static double[] data = new double[6];

    public float adbCoefficients(double alpha, double beta,double[] adb)
    {
        // data = {'CFX','CFY','CFZ','CMX','CMY','CMZ'}

        alphaArray = new double[9]{1, alpha, Math.Pow(alpha,2), Math.Pow(alpha,3), Math.Pow(alpha,4), Math.Pow(alpha,5), Math.Pow(alpha,6), Math.Pow(alpha,7), Math.Pow(alpha,8)};
        adb_order = length(alpha) - 1;
        adb_w_hat = adb;

        if( beta >= -180 && beta < -175)
        {
        i = 1;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -175 && beta < -170)
        {
        i = 2;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -170 && beta < -160)
        {
        i = 3;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -160 && beta < -150)
        {
        i = 4;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -150 && beta < -140)
        {
        i = 5;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -140 && beta < -130)
        {
        i = 6;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -130 && beta < -120)
        {
        i = 7;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -120 && beta < -110)
        {
        i = 8;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -110 && beta < -100)
        {
        i = 9;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -100 && beta < -90)
        {
        i = 10;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -90 && beta < -80)
        {
        i = 11;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -80 && beta < -70)
        {
        i = 12;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -70 && beta < -60)
        {
        i = 13;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -60 && beta < -50)
        {
        i = 14;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -50 && beta < -40)
        {
        i = 15;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -40 && beta < -30)
        {
        i = 16;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -30 && beta < -25)
        {
        i = 17;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -25 && beta < -21)
        {
        i = 18;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -21 && beta < -18)
        {
        i = 19;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -18 && beta < -15)
        {
        i = 20;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -15 && beta < -12)
        {
        i = 21;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -12 && beta < -10)
        {
        i = 22;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -10 && beta < -8)
        {
        i = 23;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -8 && beta < -6)
        {
        i = 24;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -6 && beta < -4)
        {
        i = 25;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -4 && beta < -2)
        {
        i = 26;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= -2 && beta < 0)
        {
        i = 27;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 0 && beta < 2)
        {
        i = 28;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 2 && beta < 4)
        {
        i = 29;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 4 && beta < 6)
        {
        i = 30;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 6 && beta < 8)
        {
        i = 31;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 8 && beta < 10)
        {
        i = 32;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 10 && beta < 12)
        {
        i = 33;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 12 && beta < 15)
        {
        i = 34;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 15 && beta < 18)
        {
        i = 35;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 18 && beta < 21)
        {
        i = 36;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 21 && beta < 25)
        {
        i = 37;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 25 && beta < 30)
        {
        i = 38;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 30 && beta < 40)
        {
        i = 39;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 40 && beta < 50)
        {
        i = 40;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 50 && beta < 60)
        {
        i = 41;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 60 && beta < 70)
        {
        i = 42;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 70 && beta < 80)
        {
        i = 43;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 80 && beta < 90)
        {
        i = 44;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 90 && beta < 100)
        {
        i = 45;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 100 && beta < 110)
        {
        i = 46;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 110 && beta < 120)
        {
        i = 47;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 120 && beta < 130)
        {
        i = 48;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 130 && beta < 140)
        {
        i = 49;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 140 && beta < 150)
        {
        i = 50;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 150 && beta < 160)
        {
        i = 51;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 160 && beta < 170)
        {
        i = 52;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 170 && beta < 175)
        {
        i = 54;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta >= 175 && beta < 180)
        {
        i = 55;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        if( beta == 180)
        {
        i = 56;
        data = alphaArray * adb_w_hat((adb_order+1)*i-1:(adb_order+1)*i-1 + adb_order,:); // predict output
        }
        return(data);
    } 
}