using System;
using System.Diagnostics;
using NUnit.Framework;
using DotNetMatrix;
using Net.Kniaz.LMA;

namespace Net.Kniaz.LMA.Tests
{

	public class LineFunction: LMAFunction
	{
		public override double GetY(double x, double[] a) 
		{
			return a[0] * x + a[1];
		}

		public override double GetPartialDerivative(double x, double[] a, int parameterIndex) 
		{
			double result=0;
			switch (parameterIndex) 
			{
				case 0: 
					result = x;
					break;
				case 1: 
					result = 1;	
					break;
				default:
					throw new ArgumentException("No such parameter index: " + parameterIndex);
			}
			return result;
		}
	}
	
	/// <summary>
	/// Summary description for LineFit.
	/// </summary>
	/// 
	[TestFixture()]
	public class LineFit
	{
		[Test()]
		public void RunLine()
		{
			double[] x = {0, 2, 6, 8, 9};
			double[] a = {2, 0.51};

			LMAFunction f = new LineFunction();

			double[][] dataPoints = f.GenerateData(a,x);

			LMA algorithm = new LMA(f,new double[] {100, -100},			
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
