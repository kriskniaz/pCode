using System;

namespace Net.Kniaz.LMA
{
	/// <summary>
	/// Abstract class implementing the LMAFunction interface
	/// </summary>
	public abstract class LMAFunction : ILMAFunction
	{
		/// <summary>
		/// Returns the y value of the function for
		/// the given x and vector of parameters
		/// </summary>
		/// <param name="x">The <i>x</i>-value for which the <i>y</i>-value is calculated.</param>
		/// <param name="a">The fitting parameters. </param>
		/// <returns></returns>
		public abstract double GetY(double x, double[] a);

		/// <summary>
		/// The method which gives the partial derivates used in the LMA fit.
		/// If you can't provide the functional derivative, use a small <code>a</code>-step (e.g., <i>da</i> = 1e-20)
		/// and return <i>dy/da</i> at the given <i>x</i> for each fit parameter.
		/// This is provided in the method below as a default implementation
		/// </summary>
		/// <param name="x">The <i>x</i>-value for which the partial derivate is calculated.</param>
		/// <param name="a">The fitting parameters.</param>
		/// <param name="parameterIndex">The parameter index for which the partial derivate is calculated.</param>
		/// <returns>The partial derivative of the function with respect to parameter <code>parameterIndex</code> at <i>x</i>.</returns>
		public virtual double GetPartialDerivative(double x, double[] a, int parameterIndex) 
		{
            //kk 25 Jun 2010
            //this value has been changed to 1*10-9 from 1*10-14 after a hint by a user
            //who was having issues with convergence on some gaussian function

			double delta = 0.000000001;
			double[] newParam = new double[a.Length];
			for (int i=0; i<a.Length; i++)
				newParam[i] = a[i];
			
			newParam[parameterIndex] = a[parameterIndex]+delta;
			double dplusResult = GetY(x,newParam);

			newParam[parameterIndex] = a[parameterIndex]-delta;
			double dminusResult = GetY(x,newParam);

			double result = (dplusResult-dminusResult)/(2*delta);

			return result;
		}


		/// <summary>
		/// Returns array of x,y values, given x and fitting parameters
		/// used by all tests to generate test data for exact fits
		/// </summary>
		/// <param name="xValues">x values</param>
		/// <param name="a">fitting parameters</param>
		/// <returns>point values</returns>
		public double[][] GenerateData(double[] a, double[] xValues) 
		{
			double[] yValues = new double[xValues.Length];

			for (int i = 0; i < xValues.Length; i++) 
			{
				yValues[i] = GetY(xValues[i], a);
			}

			return new double[][] {xValues, yValues};
		}

	}
}
