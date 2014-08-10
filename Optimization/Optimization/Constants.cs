using System;
using System.Text;

namespace Net.Kniaz.Optimization
{
	class Constants
	{

        //Rosenbrockbrock constants
	    public const double RosenbrockBeta = 0.5;
	    public const double RosenbrockAlfa  = 3.0;
        //Nelder-Mead constants
        public const double NelderLambda = 1.0;
        public const double NelderBeta = 2.0;
        public const double NelderGamma = 0.5;

		//one dimensional search constants
		public static int DIV = 8;
		public static double TINY = 1e-10;
		public static int BRACKET_POINTS = 3;
	
		public static double GOLDEN_SECTION_VAL 
		{
			get {return (System.Math.Sqrt(5)+1)/2;}
		}

		public static double RGOLDEN_SECTION_VAL
		{
			get {return (GOLDEN_SECTION_VAL-1.0);}
		}

		public static double CGOLDEN_SECTION_VAL
		{
			get {return (1.0 - RGOLDEN_SECTION_VAL);}
		}


		/// <summary>
		/// Generates n x m jagged array of doubles
		/// </summary>
		/// <param name="m"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		public static double[][] GenerateMatrix(int m, int n)
		{
			double[][] A = new double[m][];
			for (int i = 0; i < m; i++)
			{
				A[i] = new double[n];
			}

			return A;
		}


	}
}

