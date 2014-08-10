using System;
using System.Diagnostics;
using NUnit.Framework;
using DotNetMatrix;
using Net.Kniaz.LMA;

namespace Net.Kniaz.LMA.Tests
{
	/// <summary>
	/// Implements Gauss Bell Shape function
	/// </summary>
	public class GaussianFunction : LMAFunction
	{

		/// <summary>
		/// Returns Gaussian values
		/// </summary>
		/// <param name="x">x value</param>
		/// <param name="a">parameters</param>
		/// <returns></returns>
		public override double GetY(double x, double[] a) 
		{
			
			if (a.Length%3!=0)
				throw new ArgumentException("Invalid number of parameters for Gaussian");

			int K = a.Length / 3;
			int i = 0;
			double y = 0.0;
			for (int j = 0; j < K; j++) 
			{
				double arg = (x - a[i + 1]) / a[i + 2];
				double ex = System.Math.Exp(-arg * arg);
				y += (a[i] * ex);
				i += 3;
			}
			return y;
		}

		/// <summary>
		/// Derivative value
		/// </summary>
		/// <param name="x">x value</param>
		/// <param name="a">vector of parameters</param>
		/// <param name="parameterIndex"></param>
		/// <returns></returns>
		public override double GetPartialDerivative(double x, double[] a, int parameterIndex) 
		{
			double result=0;

			// i - index one of the K Gaussians
			int i = 3 * (parameterIndex / 3);
			double arg = (x - a[i + 1]) / a[i + 2];
			double ex = System.Math.Exp(-arg * arg);
			double fac = a[i] * ex * 2.0 * arg;
			if (parameterIndex == i) 
				result = ex;
			else
				if (parameterIndex == (i + 1)) 
				{
					result = fac / a[i + 2];
				}
			else
				if (parameterIndex == (i + 2)) 
				{
					result = fac * arg / a[i + 2];
				}
			else 
			{
				Trace.WriteLine("Bad index value");
				result = 1.0;
			}


			return result;
		}


	}
	
	/// <summary>
	/// Tests Gaussian Fit
	/// </summary>
	/// 
	[TestFixture()]
	public class GaussianFit
	{
		[Test()]
		public void RunGaussian()
		{
			int npts=100;
			//produce test data - a is a vecor with exact solution
			double[] x = new double[npts];
			for (int i = 0; i < npts; i++) 
			{
				x[i] = 0.1 * (i + 1); // NR always counts from 1
			}

			double[] a = {5, 2, 3, 2, 5, 3};
			
			LMAFunction f = new GaussianFunction();

			double[][] dataPoints = f.GenerateData(a,x);

			LMA algorithm = new LMA(f,new double[] {4, 2, 2, 2, 5, 2},			
				dataPoints, null, new GeneralMatrix(6,6),1d-30,100);

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
