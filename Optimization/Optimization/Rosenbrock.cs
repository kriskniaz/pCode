using System;
using System.Diagnostics;

namespace Net.Kniaz.Optimization.NonGradient
{
	/// <summary>
	/// Implements the Rosenbrock optimization algorithm
    /// http://www.applied-mathematics.net/optimization/Rosenbrockbrock.html
	/// </summary>
	/// 
	public class Rosenbrock
	{
		double _epsilon;
		double _beta;
		double _alfa;
		int _itmax;
		int _currentIteration;
		int _dimension;
		double _initialStep;
		double[] x;
		IFunction _function;

        /// <summary>
        /// Initialize constant values required by the RosenbrockBrock algorithm
        /// </summary>
        private void Initialize()
        {
            _beta  = Constants.RosenbrockBeta;
            _alfa   = Constants.RosenbrockAlfa;
        }


		/// <summary>
		/// Parametrized constructor
		/// </summary>
		/// <param name="dim">Dimension of the function</param>
		/// <param name="initialPar">Initial parameters</param>
		/// <param name="f">function to be minimized</param>
		/// <param name="step">initial step</param>
		/// <param name="epsilon">tolerance or accuracy</param>
		/// <param name="itMax">max number of iterations</param>
		public Rosenbrock(int dim, double[] initialPar, IFunction f, double step, 
					double epsilon, int itMax)
		{
			string category = "Rosenbrock constructor";
			Trace.WriteLine("In ",category);
			if (dim<1)
			{
				Trace.WriteLine("Failed validation; Throwing Exception", category);
				throw new ArgumentException("Number of dimensions should be at least 1");
			}

			if (dim!=f.Dimension || initialPar.Length!=f.Dimension)
			{
				Trace.WriteLine("Failed validation; Throwing Exception", category);
				throw new ArgumentException("Number of dimensions between the function, initial step and value passed differ");
			}
			if (epsilon <=0)
			{
				Trace.WriteLine("Failed validation; Throwing exception", category);
				throw new ArgumentException("Epsilon should be >0, preferably a small number 1e-10");
			}

			if (itMax<1)
			{
				Trace.WriteLine("Failed validation; Throwing exception", category);
				throw new ArgumentException("number of iterations should be at least 1");
			}


			Initialize();
			_dimension = dim;
			_initialStep = step;
			_epsilon = epsilon;
			_itmax = itMax;
			_function = f;
			for (int i=0;i<dim;i++)
				x = initialPar;

			Trace.WriteLine("Out ",category);

		}
 
		/// <summary>
        /// Finds minimum of the function
        /// </summary>
		public void FindMinimum()
		{
			bool signal,theEnd;
			int i,j,k,dim;
			dim=_dimension;
			int []sign = new int[dim];
			double f0,f1,c5,c2;
			double [][]v = Constants.GenerateMatrix(dim,dim);
			double []c1 = new double[dim];
			double []step=new double[dim];
			double []s  = new double[dim];

			string category = "FindMinimum()";

			Trace.WriteLine("In ",category);
			 
			theEnd=false;

			for (i=0; i<dim; i++)
			{
				step[i]=_initialStep;
				s[i]=0;
				for (j=0; j<dim; j++)
					if (i==j) v[i][j]=1;
			}

			_currentIteration=0;

			do
			{
				Trace.WriteLine("------------------------------------------------------------");
				Trace.WriteLine(category, String.Format("Iteration {0}.", _currentIteration));

				for (j=0; j<dim; j++)
				{
					sign[j]=0;
					Trace.WriteLine(category, String.Format("x {0} = {1}", _currentIteration,x[j]));
				}

				_currentIteration++;

				
				f0=_function.GetVal(x);
				
				for (i=0; i<dim; i++)
				{
					c1[i]=0;
					for (j=0; j<dim; j++) x[j]+=step[i]*v[i][j];
					f1=_function.GetVal(x);
					if (f1>=f0) 
					{
						for (j=0; j<dim; j++) x[j]+=-step[i]*v[i][j];
						step[i]=-_beta*step[i];
						sign[i]=1;
					}
					else
					{
						s[i]+=step[i];
						step[i]=step[i]*_alfa;
						sign[i]=0;
						f0=f1;
					}
				}
				c5=0;
				signal=false;
				for (j=0; j<dim; j++)
					if (sign[j]==1) c5++;
				if (c5==dim)
				{
					for (j=0; j<dim; j++)
						if (s[j]==0) signal=true;
					if (signal==false) 
					{
						Minim(dim,s,v);
						Ortog(dim,v);
						for (k=0; k<dim; k++) 
							s[k]=0;
					}
				}
				c2=0;
				for (k=0;  k<dim; k++)
					c2+=System.Math.Abs(step[k]);
				if(c2<_epsilon) theEnd=true;
			}
			while (theEnd!=true);

			Trace.WriteLine("Out ",category);

		}


		#region Accessors
		public double Epsilon
		{
			get
			{
				return _epsilon;
			}

			set
			{
				this._epsilon=value;
			}
		}

        /// <summary>
        /// 
        /// </summary>
		public int Itmax
		{
			get
			{
				return _itmax;
			}
			set
			{
				this._itmax = value;
			}
		}

        /// <summary>
        /// 
        /// </summary>
		public int Iterations
		{
			get
			{
				return _currentIteration;
			}
			set
			{
				this._currentIteration = value;
			}
		}
		#endregion

		#region Private Functions
        /// <summary>
        /// Sqr Root of the sum column sqrs
        /// </summary>
        /// <param name="k"></param>
        /// <param name="l"></param>
        /// <param name="a"></param>
        /// <returns></returns>
		private double Norma(int k, int l, double [][]a)
		{
			int j;
			double c;
			c=0;
			for (j=0; j<k; j++)
			{
				c+=a[l][j]*a[l][j];
			}
			return(System.Math.Sqrt(c));
		}

        /// <summary>
        /// Scalar Product of two matrices
        /// </summary>
        /// <param name="k"></param>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <param name="a"></param>
        /// <returns></returns>
		private double scalarProduct(int k, int p, int q, double [][]a)
		{
			int i;
			double skal;
			skal=0;
			for (i=0; i<k; i++)
				skal+=a[p][i]*a[q][i];
			return(skal);
		}
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="k"></param>
        /// <param name="l"></param>
        /// <param name="a"></param>
		private void Wers(int k, int l, double [][]a)
		{
			int i;
			double c;
			c=Norma(k,l,a);
			for (i=0; i<k; i++)
				a[l][i]=a[l][i]/c;
		}
 
        /// <summary>
        /// Performs Orthogonalization of the vector
        /// </summary>
        /// <param name="wym"></param>
        /// <param name="v"></param>
		private void Ortog(int wym, double [][]v)
		{
			int i,j,k;
			double lask=0.0;
			j=0;
			
			double []w1 = new double[wym];

			Wers(wym,j,v);
			for (i=1; i<wym; i++)
			{  
				for (k=0; k<wym; k++) w1[k]=0;
				for (j=0; j<=i-1; j++)
				{
					lask=scalarProduct(wym,i,j,v);
					for (k=0; k<wym; k++) w1[k]+=v[j][k]*lask;
				}
				for (k=0; k<wym; k++) v[i][k]+=-w1[k];
				Wers(wym,i,v);
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="wym"></param>
		/// <param name="s1"></param>
		/// <param name="a"></param>
		private void Minim(int wym, double []s1, double [][]a)
		{
			double c1;
			double [][]b = Constants.GenerateMatrix(wym,wym);
			int i,j,k;
			for (i=0; i<wym; i++)
				for (j=0; j<wym; j++)
				{
					c1=0;
					for (k=i; k<wym; k++) c1+=s1[k]*a[k][j];
					b[i][j]=c1;
				}
			for (i=0; i<wym; i++)
				for (j=0; j<wym; j++) a[i][j]=b[i][j];
		}

		#endregion
	}

}
