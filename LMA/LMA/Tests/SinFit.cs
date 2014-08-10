using System;
using System.Diagnostics;
using NUnit.Framework;
using DotNetMatrix;
using Net.Kniaz.LMA;

namespace Net.Kniaz.LMA.Tests
{
	/// <summary>
	/// function represents sinusoidal aplitude
	/// </summary>
	public class SinFunction : LMAFunction
	{
	
		/// <summary>
		/// Returns value of the function
		/// </summary>
		/// <param name="x"></param>
		/// <param name="a"></param>
		/// <returns></returns>
		public override double GetY(double x, double[] a) 
		{
			return a[0] * System.Math.Sin(x / a[1]);
		}

		/// <summary>
		/// Returns derivative
		/// </summary>
		/// <param name="x"></param>
		/// <param name="a"></param>
		/// <param name="parameterIndex"></param>
		/// <returns></returns>
		public override double GetPartialDerivative(double x, double[] a, int parameterIndex) 
		{
			double result=0;
			switch (parameterIndex) 
			{
				case 0: 
					result = System.Math.Sin(x / a[1]);
					break;
				case 1: 
					result = a[0] * System.Math.Cos(x / a[1]) * (-x / (a[1] * a[1]));
					break;
				default:
					throw new ArgumentException("No such parameter index: " + parameterIndex);
			}

			return result;
		}
	}

	
	/// <summary>
	/// NUnit test function.
	/// </summary>
	/// 
	[TestFixture()]
	public class SinFit
	{

		/// <summary>
		/// Test method
		/// </summary>
		[Test()]
		public void RunSin()
		{
			//produce test data - a is a vecor with exact solution
			double[] x = {0.0, 0.1, 0.2, 0.3, 0.5, 0.7};
			double[] a = {2.2, 0.4};
			LMAFunction f = new SinFunction();

			double[][] dataPoints = f.GenerateData(a,x);

			LMA algorithm = new LMA(f,new double[] {0.1, 10},			
				dataPoints, null, new GeneralMatrix(2,2),1d-20,100);
			
			algorithm.Fit();

			for (int i=0; i<a.Length; i++)
			{
				Assert.IsTrue(System.Math.Abs(algorithm.Parameters[i]-a[i])<0.0001);
				Trace.WriteLine("Parameter" + i.ToString() + " " + algorithm.Parameters[i].ToString());
			}

			Trace.WriteLine("# of iterations =" + algorithm.Iterations.ToString());


		}
	}
}
