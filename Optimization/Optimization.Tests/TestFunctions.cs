using System;

namespace Net.Kniaz.Optimization
{

	/// <summary>
	/// 3 dimensional quadratic function with the minimum
	/// at (1,-2,3)
	/// </summary>
	public class TestFunction: RealFunction
	{

		public override int Dimension
		{
			get{return 3;}
		}

		public override double GetVal(double[] x)
		{
			double x1,x2,x3;
			x1=x[0];
			x2=x[1];
			x3=x[2];
			return 5*x1*x1+2*x2*x2+2*x3*x3+2*x1*x2+2*x2*x3-2*x1*x3-6*x3;
		}

	}

	/// <summary>
	/// Test function has a W like shape with 
	/// the first dip being global minimum at x =~-1.5
	/// and the second dip at x = 4
	/// </summary>
	public class TestFunction1: IOneDFunction
	{
		public double GetVal(double x)
		{
			return 3 * x * x + 40 * System.Math.Sin(x);
		}
	}

	/// <summary>
	/// A regular Parabola with a min of -0.75
	/// </summary>
	public class TestFunction2: IOneDFunction
	{
		public double GetVal(double x)
		{
			return 3 + 6*x  + 4*x*x;
		}
	}

	/// <summary>
	/// This function is "glued" from expotential and a parabola
	/// minimum is at 0.78152
	/// </summary>
	public class TestFunction3: IOneDFunction
	{
		public double GetVal(double x)
		{
			return System.Math.Exp(x)+5*(x-2)*x;
		}
	}
	/// <summary>
	///    Helper function that calculates square root of sum of squares
	///    Used by the test function to test on minimum
	/// </summary>
	public class NumericNet
	{

		public static double CalcDiff(int dim, double []x, double []y)
		{
			int count;
			double diff=0;

			for (count=0;count<dim;count++)
			{
				diff+=System.Math.Pow((x[count] - y[count]),2);
			}

			return System.Math.Sqrt(diff);
			
		}

	}
	/// <summary>
	/// Summary description for Functions.
	/// </summary>
	/// <summary>
	/// Implements multidimentsional parabola
	/// </summary>
	public class Parabola : RealFunction
	{
		private int _dim;
		public Parabola(int dimension)
		{
			_dim = dimension;
		}


		public override int Dimension
		{
			get{return _dim;}
		}

		public override double GetVal(double[] x)
		{
			double c;
			int i;
			c=0.0;
			for (i=0; i<_dim; i++)
				c+=x[i]*x[i];
			return(c);	
		}

	}

	/// <summary>
	/// 3 Dimensional banana - most gradient methods fail
	/// on this function. Rosenbrockbroock is very good at finding
	/// minimum for this
	/// </summary>
	public class Banana : RealFunction
	{
	
		public override int Dimension
		{
			get{return 2;}
		}

		private double sqr(double x)
		{
			return x*x;
		}
		

		public override double GetVal(double[] x)
		{
			double c;
			c=100*sqr(x[0]-sqr(x[1]))+(1-x[1])*(1-x[1]);
			return(c);
		}

	}
}
