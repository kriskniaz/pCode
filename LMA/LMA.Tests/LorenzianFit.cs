using System;
using System.Diagnostics;
using NUnit.Framework;
using DotNetMatrix;
using Net.Kniaz.LMA;

namespace Net.Kniaz.LMA.Tests
{

	/// <summary>
	/// Represents Lorenzian function. Derivative is calculated
	/// using a default f(x+dx)/dx method in the base class
	/// </summary>
	public class LorenzianFunction: LMAFunction
	{
		/// <summary>
		/// Returns value ofthe Lorenzian
		/// </summary>
		/// <param name="x"></param>
		/// <param name="a"></param>
		/// <returns></returns>
		public override double GetY(double x, double[] a) 
		{
			double result = a[0]*0.5/(System.Math.PI*((x-a[1])*(x-a[1])+0.25*a[0]*a[0]));
			return result;
		}

	}


	[TestFixture()]
	public class LorenzianFit
	{
		[Test()]
		public void RunLorenzian()
		{
			int npts=100;
			//produce test data - a is a vecor with exact solution
			double[] x = new double[npts];
			for (int i = 0; i < npts; i++) 
			{
				x[i] = -5 + 0.1 *i; // NR always counts from 1
			}

			double[] a = {1,0};
			
			LMAFunction f = new LorenzianFunction();

			double[][] dataPoints = f.GenerateData(a,x);

			//initial guess should be close enough
			LMA algorithm = new LMA(f,new double[] {5, 5},			
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
