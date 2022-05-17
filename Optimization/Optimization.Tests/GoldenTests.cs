using System;
using NUnit.Framework;
using DotNetMatrix;
using Net.Kniaz.Optimization.QuasiNewton;

namespace Net.Kniaz.Optimization.QuasiNewton.Tests
{
	/// <summary>
	/// Testing one dimensional minimum using the Golden Section
	/// Algorithm
	/// </summary>
	/// 
	[TestFixture()]
	public class GoldenTests
	{
		/// <summary>
		/// Testing the algorithm using the test function
		/// test1 - a "w" shaped function
		/// Global Min is at x = -1.364641. We are starting
		/// with finding the bracket at 10 with step 1.
		/// Bracket function returns -4.764, -0.529, 6.326.
		/// These are fed to the 1D min function
		/// </summary>
		[Test()]
		public void MinGold1()
		{
			double xmin=0;
			double realXmin = -1.364641;
			double eps = 1e-5;
			int counterMax = 50;
			LineSearch ls = new LineSearch();
			TestFunction1 f = new TestFunction1();
			double [] res = new double[3];
			int counter1 = ls.FindMinInterval(f,-10,1,30,ref res);
			int counter2 = ls.FindMinimumViaGoldenSection(f,res[0],res[1],res[2],counterMax,eps,ref xmin);
			Assert.IsTrue(counter2<counterMax);
			Assert.IsTrue (System.Math.Abs(xmin-realXmin)<eps);
		}

		/// <summary>
		/// This time the start ofsearch is close to the local minimum
		/// so the search should converge to local minimum
		/// </summary>
		[Test()]
		public void MinGold2()
		{
			double xmin=0;
			//local minimum
			double realXmin = 4.057968;
			double eps = 1e-5;
			int counterMax = 50;
			LineSearch ls = new LineSearch();
			TestFunction1 f = new TestFunction1();
			int counter = ls.FindMinimumViaGoldenSection(f,1,4,5,counterMax,eps,ref xmin);
			Assert.IsTrue(counter<counterMax);
			Assert.IsTrue (System.Math.Abs(xmin-realXmin)<eps);

		}

		/// <summary>
		/// Tests regular parabola with a min of -0.75 coming from negative direction
		/// </summary>
		[Test()]
		public void MinGold3()
		{
			double xmin=0;
			double eps = 1e-5;
			double realXmin = -0.75;
			int counterMax = 50;
			LineSearch ls = new LineSearch();
			TestFunction2 f = new TestFunction2();
			double [] res = new double[3];
			int counter1 = ls.FindMinInterval(f,-10,1,30,ref res);
			int counter2 = ls.FindMinimumViaGoldenSection(f,res[0],res[1],res[2],counterMax,eps, ref xmin);
			Assert.IsTrue(counter2<counterMax);
			Assert.IsTrue (System.Math.Abs(xmin-realXmin)<eps);
		}

		/// <summary>
		/// testing a parabola coming from the far positive direction
		/// </summary>

		[Test()]
		public void MinGold4()
		{
			double xmin=0;
			double eps = 1e-5;
			double realXmin = -0.75;
			int counterMax = 50;
			LineSearch ls = new LineSearch();
			TestFunction2 f = new TestFunction2();
			double[] res = new double[3];
			int counter1 = ls.FindMinInterval(f,1000,1,30,ref res);
			int counter2 = ls.FindMinimumViaGoldenSection(f,res[0],res[1],res[2],counterMax,eps, ref xmin);
			Assert.IsTrue(counter2<counterMax);
			Assert.IsTrue (System.Math.Abs(xmin-realXmin)<eps);
		}

	}
}
