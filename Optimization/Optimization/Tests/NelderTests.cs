using System;
using NUnit.Framework;

namespace Net.Kniaz.Optimization.NonGradient.Tests
{

	

	/// <summary>
	/// Tests Nelder Mead with 5 dimensional parabola 
	/// and 3d Banana
	/// </summary>
	///
	[TestFixture()]
	public class NelderTests
	{

		//Calculates minimum of the 5 dimensional simple parabola
		//using Nelder Mead algorithm
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
			Nelder objNelder = new Nelder(_dim,_data,func,_step,_epsilon,_itmax);
			objNelder.FindMinimum();
			diff = NumericNet.CalcDiff(_dim,_reference,objNelder.Result);
			Assert.IsTrue(diff<_realEpsilon);
		}


		//Calculates minimum of the Rosenbrock function ("two D banana")
		//using the Nelder Mead function
		[Test()]
		public void CalculateBananaMinimum()
		{
			int _dim=2;
			double []_data = {5,34};
			double []_reference = {1,1};
			double _epsilon=1e-7;
			double _realEpsilon = System.Math.Sqrt(_epsilon);
			//Nelder needs larger initial step
			double _step=2000;
			int _itmax=1000;
			double diff;

			Banana func = new Banana();
			Nelder objNelder = new Nelder(_dim,_data,func,_step,_epsilon,_itmax);;
			objNelder.FindMinimum();
			diff = NumericNet.CalcDiff(_dim,_reference,objNelder.Result);
			Assert.IsTrue(diff<_realEpsilon);
		}
	}
}
