using System;

namespace Net.Kniaz.Optimization
{
	/// <summary>
	/// Interface for an arbitrary function of n dimensions
	/// </summary>
	public interface IFunction
	{

		/// <summary>
		/// Returns dimension of the function
		/// </summary>
		int Dimension
		{
			get;
		}

		/// <summary>
		/// Return Value of the function for a value vector x
		/// </summary>
		/// <param name="x">vector of arguments</param>
		/// <returns></returns>
		double GetVal(double []x);
	}
}
