using System;
using DotNetMatrix;

namespace Net.Kniaz.Optimization
{
	/// <summary>
	/// Summary description for RealFunction.
	/// </summary>
	public abstract class RealFunction : IGradientFunction
	{

		#region IGradientFunction Members

		public abstract int Dimension
		{
			get;
		}

		public virtual double GetVal(double[] x)
		{
			// TODO:  Add RealFunction.GetVal implementation
			return 0;
		}

		public double GetDerivativeVal(int n, double[] x)
		{
			double delta = 1e-10;
			double [] nX = new double[x.Length];

			for ( int i=0; i<x.Length; i++)
				nX[i] = x[i];
			
			nX[n] = nX[n]+delta;
			double dplusResult = GetVal(nX);

			nX[n] = nX[n]-2*delta;
			double dminusResult = GetVal(nX);

			double result = (dplusResult-dminusResult)/(2*delta);

			return result;		
		}

		public double[] GetGradient(double []x)
		{
			double[] grad = new double[Dimension];
			for (int i = 0; i<Dimension; i++)
				grad[i] = GetDerivativeVal(i,x);

			return grad;								  
		}


		#endregion

		#region public helpers
		public double GetPartialDerivativeVal(int row, int col, double[]x)
		{
			double delta = 1e-10;
			double [] nX = new double[x.Length];

			for ( int i=0; i<x.Length; i++)
				nX[i] = x[i];
			
			nX[col] = nX[col]+delta;
			double dplusResult = GetDerivativeVal(row,nX);

			nX[col] = nX[col]-2*delta;
			double dminusResult = GetDerivativeVal(row,nX);

			double result = (dplusResult-dminusResult)/(2*delta);

			return result;		
		}

		public double[][] GetHessian(double[]x)
		{
			GeneralMatrix mat = CalculateHessian(x);
			return mat.ArrayCopy;
		}


		
		public GeneralMatrix CalculateHessian(double[]x)
		{
			GeneralMatrix hessian = new GeneralMatrix(Dimension,Dimension);
			for (int i=0; i<Dimension; i++)
				for (int j=0; j<Dimension; j++)
					hessian.SetElement(i,j,GetPartialDerivativeVal(i,j,x));
			return hessian;
		}
		#endregion


	}
}
