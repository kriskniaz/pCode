using System;
using System.Diagnostics;
//public domain JAMA matrix library ported to .NET
//will be used her for matrix operations
using DotNetMatrix; 
namespace Net.Kniaz.LMA
{
	
	/// <summary>
    /// A class which implements the <i>Levenberg-Marquardt Algorithm</i> (LMA) fit for non-linear,
    /// multidimensional parameter space. The algorithm is described in
    /// Numerical Recipes in FORTRAN, 2nd edition, p. 676-679, ISBN 0-521-43064X, 1992.
    /// 
    /// The matrix <code>(LMAMatrix)</code> class used in the fit is an interface, so you can use your
    /// favourite implementation. This package uses Matrix from JAMA-math libraries,
    /// but feel free to use anything you want. Note that you have to implement
    /// the actual model function and its partial derivates as <code>LMAFunction</code> before making the fit
    /// 
    /// author Janne Holopainen (jaolho@utu.fi, tojotamies@gmail.com)
    /// java version 1.1, 24.03.2006
    /// .......................................................................
    /// Ported to .NET from the above listed version by Kris Kniaz
    /// LMAMatrix interface was removed in this implementation. Instead we are
    /// using explicit GeneralMatrix class - a .NET port of the JAMA
    /// matrix package
    /// .NET version 1.0 28.10.2006
    /// </summary>
	public class LMA
	{
		//interface of the function to be fitted
		private ILMAFunction function;

		 //The array of fit parameters (a.k.a, the a-vector).
		private double[] parameters;

		//Parameters incremented by value of lambda
		private double[] incrementedParameters;
		 
		 //Measured data points for which the model function is to be fitted.
		 //double[0 = x, 1 = y][data point index] = data value
		private double[][] dataPoints;

		//Weights for each data point. The merit function is: chi2 = sum[ (y_i - y(x_i;a))^2 * w_i ].
		//For gaussian errors in datapoints, set w_i = 1 / sigma_i.
		private double[] weights;
	
		//Hessian
		private GeneralMatrix alpha;

		//gradient
		private double[] beta;

		private double[] da;
		
		private double chi2;
		private double lambda;

		private double incrementedChi2;
		private int iterationCount;
	
		// default end conditions
		private double minDeltaChi2 = 1e-30;
		private int maxIterations = 100;
	
		/// <summary>
		///Ctor. In the LMA fit N is the number of data points, M is the number of fit parameters.
		///Call <code>fit()</code> to start the actual fitting.
		/// </summary>
		/// <param name="function">The model function to be fitted. Must be able to take M input parameters.</param>
		/// <param name="parameters">The initial guess for the fit parameters, length M.</param>
		/// <param name="dataPoints">The data points in an array, <code>double[0 = x, 1 = y][point index]</code>. Size must be <code>double[2][N]</code>.</param>
		/// <param name="weights">The weights, normally given as: <code>weights[i] = 1 / sigma_i^2</code>. 
		/// If you have a bad data point, set its weight to zero. If the given array is null,
		/// a new array is created with all elements set to 1.</param>
		/// <param name="alpha">A Matrix instance. Must be initiated to (M x M) size. 
		/// In this case we are using the GeneralMatrix type from the open source JAMA library</param>
        /// <param name="argDeltaChi2">delta chi square</param>
        /// <param name="argMaxIter">maximum number of iterations</param>
		public LMA(LMAFunction function, double[] parameters, 
			double[][] dataPoints, double[] weights, 
			GeneralMatrix alpha,
			double argDeltaChi2, int argMaxIter) 
		{
			if (dataPoints[0].Length != dataPoints[1].Length) 
				throw new ArgumentException("Data must have the same number of x and y points.");
			
			if (dataPoints.Length != 2) 
				throw new ArgumentException("Data point array must be 2 x N");
			
			this.function = function;
			this.parameters = parameters;
			this.dataPoints = dataPoints;
			this.weights = CheckWeights(dataPoints[0].Length, weights);
			this.incrementedParameters = new double[parameters.Length]; 
			this.alpha = alpha;
			this.beta = new double[parameters.Length];
			this.da = new double[parameters.Length];
			minDeltaChi2 = argDeltaChi2;
			maxIterations = argMaxIter;
			lambda = Constants.lambda;

		}

		#region Accessors
		public double[] Parameters
		{
			get{return this.parameters;}
		}

		public int Iterations
		{
			get{return this.iterationCount;}
		}

		public double Chi2
		{
			get{return this.chi2;}
		}
		#endregion

		/// <summary>
		/// The default fit. If used after calling fit(lambda, minDeltaChi2, maxIterations),
		/// uses those values. The stop condition is fetched from <code>this.stop()</code>.
		/// Override <code>this.stop()</code> if you want to use another stop condition.
		/// </summary>
		public void Fit() 
		{
			iterationCount = 0;
		
			do 
			{
				chi2 = CalculateChi2();
				UpdateAlpha();
				UpdateBeta();
			
				
				SolveIncrements();
				incrementedChi2 = CalculateIncrementedChi2();
				// The guess results to worse chi2 - make the step smaller
				if (incrementedChi2 >= chi2) 
				{
					lambda *= 10;
				}
					// The guess results to better chi2 - move and make the step larger
				else 
				{
					lambda /= 10;
					UpdateParameters();
				}
				iterationCount++;
			} while (!this.Stop());
		}
	
		/// <summary>
		/// Initializes and starts the fit. The stop condition is fetched from <code>this.stop()</code>.
		/// Override <code>this.stop()</code> if you want to use another stop condition.
		/// </summary>
		/// <param name="lambda"></param>
		/// <param name="minDeltaChi2"></param>
		/// <param name="maxIterations"></param>
		public void Fit(double lambda, double minDeltaChi2, int maxIterations) 
		{
			this.lambda = lambda;
			this.minDeltaChi2 = minDeltaChi2;
			this.maxIterations = maxIterations;
			Fit();
		}

		/// <summary>
		///The stop condition for the fit.
		///Override this if you want to use another stop condition.
		/// </summary>
		/// <returns></returns>
		public bool Stop() 
		{
			return System.Math.Abs(chi2 - incrementedChi2) < minDeltaChi2 || iterationCount > maxIterations;
		}

		/// <summary>
		/// Updates parameters from incrementedParameters.
		/// </summary>
		protected void UpdateParameters() 
		{
			System.Array.Copy(incrementedParameters, 0, parameters, 0, parameters.Length);
		}

		/// <summary>
		/// Solves the increments array (<code>this.da</code>) using alpha and beta.
		/// Then updates the <code>this.incrementedParameters</code> array.
		/// NOTE: Inverts alpha. Call at least <code>updateAlpha()</code> before calling this.
		/// </summary>
		protected void SolveIncrements() 
		{
			try 
			{
				//use the GeneralMatrix package to invert alpha
				//one could also use 
				//double[] da = DoubleMatrix.solve(alpha, beta);

				GeneralMatrix m = alpha.Inverse();
				//set alpha with inverted matrix
				alpha.SetMatrix(0,alpha.RowDimension-1,0,alpha.ColumnDimension-1,m);
			}
			catch (Exception e) 
			{
				Trace.WriteLine(e.StackTrace);
			}

			for (int i = 0; i < alpha.RowDimension; i++) 
			{
				da[i] = 0;
				for (int j = 0; j < alpha.ColumnDimension; j++) 
				{
					da[i] += alpha.GetElement(i, j) * beta[j];
				}
			}
			
			for (int i = 0; i < parameters.Length; i++) 
			{
				//if (!Double.isNaN(da[i]) && !Double.isInfinite(da[i]))
				incrementedParameters[i] = parameters[i] + da[i];
			}
		}

				
		/// <summary>
		/// Calculates value of the function for given parameter array
		/// </summary>
		/// <param name="a">input parameters</param>
		/// <returns>value of the function</returns>
		protected double CalculateChi2(double[] a) 
		{
			double result = 0;
			for (int i = 0; i < dataPoints[0].Length; i++) 
			{
				double dy = dataPoints[1][i] - function.GetY(dataPoints[0][i], a);	
				result += weights[i] * dy * dy; 
			}
			return result;
		}
	
		/// <summary>
		/// Calculates function value for the current fit parameters
		/// Does not change the value of chi2
		/// </summary>
		/// <returns>value of the function</returns>
		protected double CalculateChi2() 
		{
			return CalculateChi2(parameters);
		}
			 
		/// <summary>
		/// Calculates function value for the incremented parameters (da + a).
		/// Does not change the value of chi2.
		/// </summary>
		/// <returns></returns>
		protected double CalculateIncrementedChi2() 
		{
			return CalculateChi2(incrementedParameters);
		}

		// 
		/// <summary>
		/// Calculates all elements for <code>this.alpha</code>.
		/// </summary>
		protected void UpdateAlpha() 
		{
			for (int i = 0; i < parameters.Length; i++) 
			{
				for (int j = 0; j < parameters.Length; j++) 
				{
					alpha.SetElement(i, j, CalculateAlphaElement(i, j));
				}
			}
		}

		/// <summary>
		/// Calculates lambda weighted element for the alpha-matrix.
		/// NOTE: Does not change the value of alpha-matrix.
		/// </summary>
		/// <param name="row">row of the Hessian</param>
		/// <param name="col">column of the Hessian</param>
		/// <returns></returns>
		protected double CalculateAlphaElement(int row, int col) 
		{
			double result = 0;
			for (int i = 0; i < dataPoints[0].Length; i++) 
			{
				result += 
					weights[i] * 
					function.GetPartialDerivative(dataPoints[0][i], parameters, row) *
					function.GetPartialDerivative(dataPoints[0][i], parameters, col);
			}
			if (row == col) result *= (1 + lambda);
			return result;
		}

		/// <summary>
		/// Calculates all elements for <code>this.beta</code>.
		/// </summary>
		protected void UpdateBeta() 
		{
			for (int i = 0; i < parameters.Length; i++) 
			{
				beta[i] = CalculateBetaElement(i);
			}
		}
	

		/// <summary>
		/// Calculates element of the beta (gradient) matrix
		/// NOTE: Does not change the value of beta-matrix.
		/// </summary>
		/// <param name="row"></param>
		/// <returns>Value of the gradient point</returns>
		protected double CalculateBetaElement(int row) 
		{
			double result = 0;
			for (int i = 0; i < dataPoints[0].Length; i++) 
			{
				result += 
					weights[i] * 
					(dataPoints[1][i] - function.GetY(dataPoints[0][i], parameters)) *
					function.GetPartialDerivative(dataPoints[0][i], parameters, row);
			}
			return result;
		}

		/// <summary>
		/// Checks if the matrix of weights for each point is a 
		/// matrix of positive elements. Otherwise it initializes
		/// a new matrix and sets each value to 1
		/// </summary>
		/// <param name="length"></param>
		/// <param name="weights"></param>
		/// <returns></returns>
		protected double[] CheckWeights(int length, double[] weights) 
		{
			bool damaged = false;
			// check for null
			if (weights == null) 
			{
				Trace.WriteLine("weights matrix was null");
				damaged = true;
				weights = new double[length];
			}
				// check if all elements are zeros or if there are negative, NaN or Infinite elements
			else 
			{
				bool allZero = true;
				bool illegalElement = false;
				for (int i = 0; i < weights.Length && !illegalElement; i++) 
				{
					if (weights[i] < 0 || Double.IsNaN(weights[i]) || Double.IsInfinity(weights[i])) illegalElement = true;
					allZero = (weights[i] == 0) && allZero;
				}
				damaged = allZero || illegalElement;
			}

			if (damaged)
			{
				Trace.WriteLine("Weights were not well defined. All elements set to 1.");
				for (int i = 0; i < weights.Length; i++)
				{
					weights[i]=1;
				}
			}

			return weights;
		}// end of weights
	}
}
