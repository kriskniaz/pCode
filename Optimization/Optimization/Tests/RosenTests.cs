using System;
using NUnit.Framework;

namespace Net.Kniaz.Optimization.NonGradient.Tests
{

	/// <summary>
	/// Summary description for TestRosenbrock.
	/// </summary>
	/// 
	[TestFixture()]
	public class RosenTests
	{
		/// <summary>
		/// Calculates the minimum of the 5 dimensional parabola
		/// using the rosenbrock Method
		/// </summary>
		[Test()]
		public void CalculateParabolaMinimum()
		{
			int _dim=5;
			double []_data = {5,34,-12,55,6};
			double []_reference = {0,0,0,0,0};
			double _epsilon=1e-7;
			double _realEpsilon = System.Math.Sqrt(_epsilon);
			double _step=1000;
			int _itmax=1000;
			double diff;

			Parabola func = new Parabola(_dim);
			Rosenbrock objRosenbrock = new Rosenbrock(_dim, _data, func, _step, _epsilon, _itmax);
			objRosenbrock.FindMinimum();
			diff = NumericNet.CalcDiff(_dim,_reference,_data);
			Assert.IsTrue(diff<_realEpsilon);
		}

		/// <summary>
		/// Calculate minimum of the rosebrock "Banana" function
		/// using Rosenbrock method
		/// </summary>
		[Test()]
		public void CalculateBananaMinimum()
		{
			int _dim=2;
			double []_data = {5,34};
			double []_reference = {1,1};
			double _epsilon=1e-7;
			double _realEpsilon = System.Math.Sqrt(_epsilon);
			double _step=1000;
			int _itmax=1000;
			double diff;

			Banana func = new Banana();
			Rosenbrock objRosenbrock = new Rosenbrock(_dim, _data, func, _step, _epsilon, _itmax);
			objRosenbrock.FindMinimum();
			diff = NumericNet.CalcDiff(_dim,_reference,_data);
			Assert.IsTrue(diff<_realEpsilon);
		}
	}
}
