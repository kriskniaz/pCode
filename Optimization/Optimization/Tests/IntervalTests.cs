using System;
using NUnit.Framework;
using DotNetMatrix;
using Net.Kniaz.Optimization.QuasiNewton;



namespace Net.Kniaz.Optimization.QuasiNewton.Tests
{


	/// <summary>
	/// Testing of the interval barcketing algorithm
	/// </summary>
	/// 
	[TestFixture()]
	public class IntervalTests
	{

		/// <summary>
		/// Try finding a bracket starting from negative end
		/// </summary>
		[Test()]
		public void MinInterval1()
		{
			int maxCount=30;
			LineSearch ls = new LineSearch();
			TestFunction1 f = new TestFunction1();
			double[] res = new double[3];
			int counter = ls.FindMinInterval(f,-500,1,maxCount, ref res);
			double aVal = f.GetVal(res[0]);
			double bVal = f.GetVal(res[1]);
			double cVal = f.GetVal(res[2]);

			Assert.IsTrue(counter<maxCount);
			Assert.IsTrue((aVal>bVal)&&(cVal>bVal));

		}

		/// <summary>
		/// Start from the positive end
		/// </summary>
		[Test()]
		public void MinInterval2()
		{
			int maxCount=30;
			LineSearch ls = new LineSearch();
			TestFunction1 f = new TestFunction1();
			double[] res = new double [3];
			int counter = ls.FindMinInterval(f,500,1,maxCount, ref res);
			double aVal = f.GetVal(res[0]);
			double bVal = f.GetVal(res[1]);
			double cVal = f.GetVal(res[2]);

			Assert.IsTrue(counter<maxCount);
			Assert.IsTrue((aVal>bVal)&&(cVal>bVal));

		}

		[Test()]
		public void MinInterval3()
		{
			int maxCount=30;
			LineSearch ls = new LineSearch();
			TestFunction1 f = new TestFunction1();
			double[] res = new double[3];
			int counter = ls.FindMinInterval(f,4,2,maxCount, ref res);
			double aVal = f.GetVal(res[0]);
			double bVal = f.GetVal(res[1]);
			double cVal = f.GetVal(res[2]);
			Assert.IsTrue(counter<maxCount);
			Assert.IsTrue((aVal>bVal)&&(cVal>bVal));

		}

	}
}
