using Godot;
using System;

public class Coefficients
{
	private double[] alphaArray;
	private double[] dlArray;
	private double[] drArray;
	private double[] drdArray;	
	
	private double[,] adb_w_hat = new double[18,6]{{0.009119553225565362, 0.004995850954688388, -0.2760071730412326, 0.006699300229956985, 0.03593140070946013, 0.02052984855877083},
													{ 0.003650543179820484, 0.0001827479123096937, -0.04866017760850735, 0.002203879890319755, 0.03723649381780122, -0.000251168148096217},
													{ 8.746805413254943e-05, 1.245369693838539e-05, 0.0001432184269485602, 1.822802676279311e-05, -0.0001400858945126904, 1.922421997834026e-05},
													{-1.450570447841088e-06, -2.205690256476296e-07, 1.564969862432394e-05, -1.369875720961186e-08,-1.840908046125351e-05, 1.467686248615808e-07},
													{-5.303907443203684e-08, -6.677273147043835e-09, -2.808162446263611e-08, -2.713576775246426e-09, 7.4062881264218e-08, -1.405858217164826e-08},
													{ 1.947266418700993e-10, 4.055474531492885e-11, -2.788673013149891e-09, 9.711085724471957e-12, 3.210198333980598e-09, -3.800021061976131e-11},
													{ 1.090533166041664e-11, 1.129118714868207e-12, 2.137122431805666e-12, -5.47344415776133e-13, -1.351604056770952e-11, 2.596077244117142e-12},
													{-9.450673500154524e-15, -2.128037941188634e-15, 1.6878896248296e-13, -8.286449529075843e-16, -1.863907398008277e-13, 3.615594954722588e-15},
													{-6.71098662264076e-16, -6.348797385959032e-17, -5.402419227111743e-17, 6.490399666898436e-17, 7.73379429396301e-16, -1.395094406074403e-16},
													{ 0.01286306401122509, 0, -0.2754224886651332, 0, 0.03034332448349663, 0},
													{ 0.0037408887509731, 0, -0.04887977436827944, 0, 0.03716435480821142, 0},
													{ 8.744888950138656e-05, 0, 0.0001505353394124565, 0, -0.0001400701326659127, 0},
													{-1.478493171741349e-06, 0, 1.582177883097878e-05, 0, -1.838453179808803e-05, 0},
													{-5.312331377292513e-08, 0, -3.283355493508576e-08, 0, 7.643349390938447e-08, 0},
													{ 1.983529439071624e-10, 0, -2.820344653752215e-09, 0, 3.203525635011834e-09, 0},
													{ 1.091420880838035e-11, 0, 3.059784758813486e-12, 0, -1.407567247937975e-11, 0},
													{-9.618018441792591e-15, 0, 1.704954066165244e-13, 0, -1.857182585572237e-13, 0},
													{-6.713410546278374e-16, 0, -1.090611733105077e-16, 0, 8.078278933247812e-16, 0}};
													
	private double[,] le_w_hat = new double[9,6]{{0.0006510017815873437,0,0.006381781712733069,-0.004076963051623877,-0.007117421685707228,0.0004158887558707988},
												  {7.863296505126788e-05,0,-0.07016538445560977,0.04482473487806317,0.0788663862261566,5.023420750113587e-05},
												  {-0.0001180324498315086,0,0.0001668578094591256,-0.0001065961115016417,-0.0001899173976957342,-7.540432999122084e-05},
												  {-2.954008576853876e-05,0,0.0001798770303701278,-0.0001149133622152969,-0.0002236669681785092,-1.887150697345827e-05},
												  {-6.19313814778497e-07,0,2.705498056016579e-05,-1.728391198748014e-05,-3.23675027053254e-05,-3.956448696242418e-07},
												  {1.12200361936052e-07,0,-4.473021687624406e-07,2.857563102574246e-07,5.767944315507948e-07,7.167853103545108e-08},
												  {2.252368524549504e-09,0,-1.347035114615781e-07,8.605453005042453e-08,1.593971942521813e-07,1.438912093450588e-09},
												  {-1.209612703588721e-10,0,4.978164362252092e-10,-3.180270483252228e-10,-6.415476634953981e-10,-7.727538706894165e-11},
												  {-2.641535090877278e-12,0,1.725174809676976e-10,-1.102117576123195e-10,-2.033261042142992e-10,-1.687528959879402e-12}};
													
	private double[,] re_w_hat = new double[9,6]{{0.0006510017815873437,0,0.006381781712733069,0.004076963051623877,-0.007117421685707228,-0.0004158887558707988},
													{7.863296505126788e-05,0,-0.07016538445560977,-0.04482473487806317,0.0788663862261566,-5.023420750113587e-05},
													{-0.0001180324498315086,0,0.0001668578094591256,0.0001065961115016417,-0.0001899173976957342,7.540432999122084e-05},
													{-2.954008576853876e-05,0,0.0001798770303701278,0.0001149133622152969,-0.0002236669681785092,1.887150697345827e-05},
													{-6.19313814778497e-07,0,2.705498056016579e-05,1.728391198748014e-05,-3.23675027053254e-05,3.956448696242418e-07},
													{1.12200361936052e-07,0,-4.473021687624406e-07,-2.857563102574246e-07,5.767944315507948e-07,-7.167853103545108e-08},
													{2.252368524549504e-09,0,-1.347035114615781e-07,-8.605453005042453e-08,1.593971942521813e-07,-1.438912093450588e-09},
													{-1.209612703588721e-10,0,4.978164362252092e-10,3.180270483252228e-10,-6.415476634953981e-10,7.727538706894165e-11},
													{-2.641535090877278e-12,0,1.725174809676976e-10,1.102117576123195e-10,-2.033261042142992e-10,1.687528959879402e-12}};
													
	private double[,] rudder_w_hat = new double[9,6]{{0.0001506266180366136,0.0005156123486492869,0,0,0,0.000366709255325344},
													{1.206728164231292e-05,0.07533223368524236,0,0,0,0.07357136331031788},
													{-5.850052997694259e-05,-0.0001852063105678568,0,0,0,-0.0001770167139753852},
													{-7.529249130075776e-07,8.468130895628189e-05,0,0,0,7.012325080057056e-05},
													{7.258742191538528e-07,1.642729966349701e-06,0,0,0,1.56839171440818e-06},
													{6.304463919983978e-09,-1.143101205592446e-06,0,0,0,-1.033195465277796e-06},
													{-5.84351101548221e-09,-3.723728750322941e-09,0,0,0,-3.608931353442554e-09},
													{-1.142087458970595e-11,1.856597640284777e-09,0,0,0,1.693907414282954e-09},
													{9.082869684543368e-12,1.863739373869068e-12,0,0,0,1.92208956102291e-12}};
																									
	private int adb_order;
	private int i;
	
	public double[] adb_C;
	public double[] le_C;
	public double[] re_C;
	public double[] rudder_C;
	//adb = importdata('../../datafiles/adb_w_hatSim.txt');
	//l_elevon = importdata('../../datafiles/le_w_hatSim.txt');
	//r_elevon = importdata('../../datafiles/re_w_hatSim.txt');
	//rudder = importdata('../../datafiles/rudder_w_hatSim.txt');
	
	public double[] adbCoefficients(double alpha, double beta)
	{
		adb_C = new double[6]{0,0,0,0,0,0};
		//var filePath = @"~/Desktop/Git/Modelling-Simulation-and-Control/Project Spearhead/datafiles/adb_w_hatSim.txt";
		//var data = System.IO.File.ReadLines(filePath).Select(x => x.Split(',')).ToArray();
		//string[][] data = System.IO.File.ReadLines(filePath).Where(line => line != "").Select(x => x.Split(',')).ToArray();
		alphaArray = new double[9]{1, alpha, Math.Pow(alpha,2), Math.Pow(alpha,3), Math.Pow(alpha,4), Math.Pow(alpha,5), Math.Pow(alpha,6), Math.Pow(alpha,7), Math.Pow(alpha,8)};
		
		if(beta < 0)
		{
			for(int j = 0; j < 6;j++)
			{
				for(int k=0;k<9;k++)
				{
					adb_C[j] += alphaArray[k] * adb_w_hat[k,j]; // predict output
				}
			}
		}
		if( beta >= 0)
		{
			for(int j = 0; j < 6;j++)
			{
				for(int k=9;k<18;k++)
				{
					adb_C[j] += alphaArray[k-9] * adb_w_hat[k,j]; // predict output
				}
			}
		}
		return(adb_C);
	}
	
	public double[] dlCoefficients(double dl)
	{
		le_C = new double[6]{0,0,0,0,0,0};
		dlArray = new double[9]{1, dl, Math.Pow(dl,2), Math.Pow(dl,3), Math.Pow(dl,4), Math.Pow(dl,5), Math.Pow(dl,6), Math.Pow(dl,7), Math.Pow(dl,8)};
		
		for(int j = 0; j < 6;j++)
		{
			for(int k=0;k<9;k++)
			{
				le_C[j] += dlArray[k] * le_w_hat[k,j];
			}
		}
		return(le_C);	
	}
	
	public double[] drCoefficients(double dr)
	{
		re_C = new double[6]{0,0,0,0,0,0};
		drArray = new double[9]{1, dr, Math.Pow(dr,2), Math.Pow(dr,3), Math.Pow(dr,4), Math.Pow(dr,5), Math.Pow(dr,6), Math.Pow(dr,7), Math.Pow(dr,8)};
		
		for(int j = 0; j < 6;j++)
		{
			for(int k=0;k<9;k++)
			{
				re_C[j] += drArray[k] * re_w_hat[k,j];
			}
		}
		return(re_C);	
	}
	
	public double[] drdCoefficients(double drd)
	{
		rudder_C = new double[6]{0,0,0,0,0,0};
		drdArray = new double[9]{1, drd, Math.Pow(drd,2), Math.Pow(drd,3), Math.Pow(drd,4), Math.Pow(drd,5), Math.Pow(drd,6), Math.Pow(drd,7), Math.Pow(drd,8)};
		
		for(int j = 0; j < 6;j++)
		{
			for(int k=0;k<9;k++)
			{
				rudder_C[j] += drdArray[k] * rudder_w_hat[k,j];
			}
		}
		return(rudder_C);
	}
	
	/*
		// data = {'CFX','CFY','CFZ','CMX','CMY','CMZ'}
		double[] data = new double[6];
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
	*/
}
