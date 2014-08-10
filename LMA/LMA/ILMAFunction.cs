using System;

namespace Net.Kniaz.LMA
{
	/// <summary>
	/// Defines Function of a[] parameters that will be fitted 
	/// </summary>
	interface ILMAFunction
	{
		/// <summary>
		/// Returns the y value of the function for
		/// the given x and vector of parameters
		/// </summary>
		/// <param name="x">The <i>x</i>-value for which the <i>y</i>-value is calculated.</param>
		/// <param name="a">The fitting parameters. </param>
		/// <returns></returns>
		double GetY(double x, double[] a);

		/// <summary>
		/// The method which gives the partial derivates used in the LMA fit.
		/// If you can't calculate the derivate, use a small <code>a</code>-step (e.g., <i>da</i> = 1e-20)
		/// and return <i>dy/da</i> at the given <i>x</i> for each fit parameter.
		/// </summary>
		/// <param name="x">The <i>x</i>-value for which the partial derivate is calculated.</param>
		/// <param name="a">The fitting parameters.</param>
		/// <param name="parameterIndex">The parameter index for which the partial derivate is calculated.</param>
		/// <returns>The partial derivate of the function with respect to parameter <code>parameterIndex</code> at <i>x</i>.</returns>
		double GetPartialDerivative(double x, double[] a, int parameterIndex);

	}
}
