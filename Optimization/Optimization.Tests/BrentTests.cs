using System;
using NUnit.Framework;
using DotNetMatrix;
using Net.Kniaz.Optimization.QuasiNewton;

namespace Net.Kniaz.Optimization.QuasiNewton.Tests
{
	/// <summary>
	/// Testing linear minimum search using Brent method.
	/// </summary>
	/// 
	[TestFixture()]
	public class BrentTests
	{

		/// <summary>
		/// Testing using function from the interval test function.
		/// Global Min is at x = -1.364641. We are starting
		/// with finding the bracket at 10 with step 1.
		/// Bracket function returns -4.764, -0.529, 6.326.
		/// These are fed to the 1D min function
		/// </summary>
		[Test()]
		public void MinBrent1()
		{
			double xmin=0;
			int maxCount = 50;
			double realXmin = -1.364641;
			double eps = 1e-5;
			LineSearch ls = new LineSearch();
			TestFunction1 f = new TestFunction1();
			double[] res = new double[3];
			int counter = ls.FindMinInterval(f,-10,1,maxCount,ref res);
			counter = ls.FindMinimumViaBrent(f,res[0],res[1],res[2],maxCount,eps, ref xmin);
			Assert.IsTrue(counter<maxCount);
			Assert.IsTrue (System.Math.Abs(xmin-realXmin)<eps);
		}

		/// <summary>
		/// This time the start is around the local minimum
		/// so the search should converge around local minimum
		/// </summary>
		[Test()]
		public void MinBrent2()
		{
			double xmin = 0;
			//local minimum
			double realXmin = 4.057968;
			double eps = 1e-5;
			int maxCount = 50;
			LineSearch ls = new LineSearch();
			TestFunction1 f = new TestFunction1();
			int counter = ls.FindMinimumViaBrent(f,1,4,5,maxCount,eps,ref xmin);
			Assert.IsTrue(counter<maxCount);
			Assert.IsTrue (System.Math.Abs(xmin-realXmin)<eps);

		}

		/// <summary>
		/// Tests regular parabola with a min of -0.75.
		/// Start the test coming from negative direction
		/// </summary>
		[Test()]
		public void MinBrent3()
		{
			double xmin=0;
			double eps = 1e-5;
			double realXmin = -0.75;
			int maxCount = 50;
			LineSearch ls = new LineSearch();
			TestFunction2 f = new TestFunction2();
			double[] res = new double[3];
			int counter = ls.FindMinInterval(f,-10,1,30,ref res);
			counter = ls.FindMinimumViaBrent(f,res[0],res[1],res[2],maxCount,eps, ref xmin);
			Assert.IsTrue(counter<maxCount);
			Assert.IsTrue (System.Math.Abs(xmin-realXmin)<eps);
		}

		/// <summary>
		/// Testing the same parabola as in
		/// the previous function but
		/// coming from the far positive direction
		/// </summary>
		[Test()]
		public void MinBrent4()
		{
			double xmin=0;
			double eps = 1e-5;
			double realXmin = -0.75;
			int maxCount = 50;
			LineSearch ls = new LineSearch();
			TestFunction2 f = new TestFunction2();
			double[] res = new double[3];
			int counter = ls.FindMinInterval(f,1000,1,30, ref res);
			counter = ls.FindMinimumViaBrent(f,res[0],res[1],res[2],50,eps, ref xmin);
			Assert.IsTrue(counter<maxCount);
			Assert.IsTrue (System.Math.Abs(xmin-realXmin)<eps);
		}

		/// <summary>
		/// testing a parabola added to an exp
		/// </summary>
		[Test()]
		public void MinBrent5()
		{
			double eps = 1e-5;
			double xmin=0;
			double realXmin = 0.78152;
			int maxCount = 50;
			LineSearch ls = new LineSearch();
			TestFunction3 f = new TestFunction3();
			double[] res = new double[3];
			int counter = ls.FindMinInterval(f,1000,1,30, ref res);
			counter = ls.FindMinimumViaBrent(f,res[0],res[1],res[2],50,eps,ref xmin);
			Assert.IsTrue(counter<maxCount);
			Assert.IsTrue (System.Math.Abs(xmin-realXmin)<eps);

		}

	}
}
