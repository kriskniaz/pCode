using System;

namespace Net.Kniaz.Optimization
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public interface IGradientFunction : IFunction
	{

		/// <summary>
		/// returns the value of the derivative 
		/// for the nth varaible at some point
		/// </summary>
		/// <param name="n"></param>
		/// <param name="x"></param>
		/// <returns></returns>
		double GetDerivativeVal(int n, double[]x);


		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
		double[] GetGradient(double[]x);


	}
}
