using System;
using DotNetMatrix;

namespace Net.Kniaz.Optimization.QuasiNewton
{
	/// <summary>
	/// Summary description for Optimizer.
	/// </summary>
	public abstract class Optimizer
	{
		protected GeneralMatrix _hessian;
		protected IGradientFunction _f;
		protected double _epsilon;
		protected int _itMax,_nDim,_curIter;
		protected double[] _xCurrent;
		protected double[] _initial;
		protected double _step;
		protected double _alpha;

		public Optimizer(int dim, double[]initialPar, IGradientFunction f, double step, double epsilon, int itMax)
		{
			_f = f;
			_epsilon = epsilon;
			_itMax = itMax;
			_nDim = dim;
			_xCurrent = new double[_nDim];
			_initial = new double[_nDim];
			_step = step;
			//initiate the first value of alpha to some random value less than 100;
			Random rnd = new Random();
			_alpha = rnd.NextDouble()*100;
			_hessian = new GeneralMatrix(_nDim,_nDim);
			for (int i=0; i<_nDim; i++)
				_initial[i] = initialPar[i];

		}

		public double[] Minimum
		{
			get{return _xCurrent;}
		}

		public int Iteration
		{
			get{return _curIter;}
		}

		public void FindMinimum()
		{
			int i;
			_xCurrent = FirstStep();
			double [] xPrev = new double[_nDim];
			for (i=0;i<_nDim;i++)
				xPrev[i] = _initial[i];

			double [] xNext;
			GeneralMatrix hPrev = GeneralMatrix.Identity(_nDim,_nDim);
			double currEps=1e5;
			_curIter=0;
			while (currEps>_epsilon && _curIter<_itMax)
			{
				_hessian = CalculateNextHessianApproximation(hPrev,xPrev,_xCurrent,_f.GetGradient(xPrev),_f.GetGradient(_xCurrent));
				xNext = CalculateNextPoint(_xCurrent,_f.GetGradient(_xCurrent),_hessian);
				for (i=0;i<_nDim; i++)
				{
					xPrev[i] = _xCurrent[i];
					_xCurrent[i] = xNext[i];
				}
				currEps = Diff(_xCurrent,xPrev);
				_curIter++;
			}

		}

		#region non public Helpers
		/// <summary>
		/// Returns first point toward the minimum.
		/// An auxiliiary method made public only
		/// for NUnit testing
		/// </summary>
		/// <returns></returns>
		private double[] FirstStep()
		{
			//first approximation of the Hessian is always the
			//identity matrix
			GeneralMatrix iMat = GeneralMatrix.Identity(_nDim,_nDim);

			return CalculateNextPoint(_initial,_f.GetGradient(_initial),iMat);
		}

		private double[] CalculateNextPoint(double[] pX, double[] pGrad, GeneralMatrix hessian)
		{
			int i=0;
			double xmin=0;
			double step = _step;
			GeneralMatrix alfaX = new GeneralMatrix(_nDim,1);
			GeneralMatrix prevX = new GeneralMatrix(pX,_nDim);
			GeneralMatrix prevGrad = new GeneralMatrix(pGrad,_nDim);
			double[] intermediate = new double[_nDim];;

			alfaX = hessian.Multiply(prevGrad);


			//doing a line search to minimize alpha
			OneDWrapper wrapper = new OneDWrapper(_f,prevX,alfaX);
			LineSearch search = new LineSearch();
			double[] interval = new double[Constants.BRACKET_POINTS];
			int it1 = search.FindMinInterval(wrapper,_alpha,step,50,ref interval);
			int it2 = search.FindMinimumViaBrent(wrapper,interval[0],interval[1],interval[2],50,_epsilon, ref xmin);
			
			for (i=0;i<_nDim; i++)
				intermediate[i] = prevX.GetElement(i,0) - xmin*alfaX.GetElement(i,0);

			_alpha = xmin;

			return intermediate;
		}

		protected virtual GeneralMatrix CalculateNextHessianApproximation(GeneralMatrix previousH, 
			double[]prevX, double[]curX, double[]prevGrad, double[]curGrad)
		{
			return previousH;

		}

		protected double Diff(double[]x1, double[]x2)
		{
			double d=0;
			for (int i=0;i<_nDim; i++)
				d+=(x1[i]-x2[i])*(x1[i]-x2[i]);
			return System.Math.Sqrt(d);
		}
		#endregion
	}
}
