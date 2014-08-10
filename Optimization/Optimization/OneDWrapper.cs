using System;
using DotNetMatrix;


namespace Net.Kniaz.Optimization.QuasiNewton
{
	/// <summary>
	/// Summary description for OneDWrapper.
	/// </summary>
	public class OneDWrapper : IOneDFunction
	{
		IGradientFunction _function;
		GeneralMatrix _previousX;
		GeneralMatrix _alfaX;
		int _problemDimension;

		public OneDWrapper(IGradientFunction func, GeneralMatrix pX, GeneralMatrix aX)
		{
			if (pX==null || aX==null)
				throw new ArgumentException("Previous x and alfaX may not be null");

			_problemDimension = pX.RowDimension;

			_function = func;

			_previousX = new GeneralMatrix(pX.ArrayCopy);

			_alfaX = new GeneralMatrix(aX.ArrayCopy);
		}

		#region IOneDFunction Members

		public double GetVal(double x)
		{
			double [] localVector = new double[_problemDimension];

			for (int i=0; i<_problemDimension; i++)
				localVector[i] = _previousX.GetElement(i,0) - _alfaX.GetElement(i,0)*x;

			return _function.GetVal(localVector);
		}

		#endregion
	}
}
