using System;
using System.Diagnostics;

namespace Net.Kniaz.Optimization.NonGradient
{
	/// <summary>
	/// Implements Nelder-Mead method of fiding minimum
	/// http://mathworld.wolfram.com/Nelder-MeadMethod.html
	/// </summary>
	/// 
	public class Nelder
	{
		double _epsilon;
		double _lambda;
		double _beta;
		double _gamma;
		int _itmax;
		int _currentIteration;
		int _dimension;
		double _step;
		double[] _parameters;
		IFunction _function;

		/// <summary>
		/// Parametrized constructor
		/// </summary>
		/// <param name="dim">Number of dimensions</param>
		/// <param name="initialPar">vector of parameters</param>
		/// <param name="f">Interface of function class to find minimum</param>
		/// <param name="step">lenght of the initial step</param>
		/// <param name="epsilon">sensitivity towards minimum</param>
		/// <param name="itMax">max numbr of iterations</param>
		public Nelder(int dim, double[] initialPar, IFunction f, double step, 
			double epsilon, int itMax)
		{
			string category = "Nelder constructor";

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

			_lambda = Constants.NelderLambda;
			_beta = Constants.NelderBeta;
			_gamma = Constants.NelderGamma;
			_dimension = dim;
			_step = step;
			_epsilon = epsilon;
			_itmax = itMax;
			_function = f;
			_parameters = new double[_dimension];
			for (int i=0; i<_dimension; i++)
				_parameters[i] = initialPar[i];

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
				_epsilon=value;
			}
		}

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

		public double[] Result
		{
			get{return _parameters;}
		}

		#endregion

		/// <summary>
		/// Finds minimum for a function and parameters given
		/// in the constructor
		/// </summary>
		public void FindMinimum()
		{

			string category = "FindMinimum()";

			bool al;
			double f1,f2,f3,c1,c2,c3;
			int i,j,k,lws,l1,l2;					
			lws=_dimension+1;

			Trace.WriteLine("In ",category);


			double [][]sym = Constants.GenerateMatrix(lws,lws);
			double []xo = new double[_dimension];
			double []xh = new double[_dimension];
			double []xc = new double[_dimension];
			double []xl = new double[_dimension];
			double []xsr= new double[_dimension];
			double []xek= new double[_dimension];
			double []xz = new double[_dimension];
			double []ww = new double[lws];
			double []x  = new double[_dimension];
			c3=_step;
			l1=l2=0;
			
			for (i=0; i<_dimension; i++) 
				xo[i]=_parameters[i];
			
			BeginProcess(sym,xo,_dimension,lws,c3);



			do
			{
				Trace.WriteLine("------------------------------------------------------------");
				Trace.WriteLine(category, String.Format("Iteration {0}.", _currentIteration));

				al=false;
				for (i=0; i<lws; i++)
				{
					for (j=0; j<_dimension; j++) 
					{
						x[j]=sym[i][j];
						Trace.WriteLine(category, String.Format("x [{0}] = {1}", _currentIteration,x[j]));
					}
					ww[i]=_function.GetVal(x);

				}
				c1=0.0;
				for (i=0; i<lws; i++)
				{
					if (i==0) k=lws-1; else k=i-1;
					c1+=System.Math.Abs( System.Math.Abs(ww[i])-System.Math.Abs(ww[k]) );
				}
				
				Sort(lws, ref l1, ref l2, ww);
				
				for (i=0; i<_dimension; i++)
				{
					c2=0.0;
					for (j=0; j<lws; j++)
						if (j!=l1) c2+=sym[j][i];
					xc[i]=c2/_dimension;
					xh[i]=sym[l1][i];
					xl[i]=sym[l2][i];
				}

				Reflection(_dimension,xh,xc,xsr);
				f1=_function.GetVal(xsr);
				if (f1<ww[l2]) 
				{
					Expansion(_dimension,xsr,xc,xek);
					f2=_function.GetVal(xek);
					if (f2<f1) 
					{
						for (i=0; i<_dimension; i++)
							sym[l1][i]=xek[i];
						ww[l1]=f2;
					}
					else
					{
						for (i=0; i<_dimension; i++)
							sym[l1][i]=xsr[i];
						ww[l1]=f1;
					}
				}
				else
				{
					if (f1>=ww[l1]) al=true;
					else
					{
						for (i=0; i<_dimension; i++)
						{
							sym[l1][i]=xsr[i];
							xh[i]=xsr[i];
						}
						ww[l1]=f1;
						al=true;
						for (i=0; i<lws; i++)
							if (i!=l1)
								if (f1<ww[i]) al=false;
					}
					if (al==true)
					{
						Contraction(_dimension,xh,xc,xz);
						f3=_function.GetVal(xz);
						if (f3>=ww[l1]) Reduction(_dimension,lws,sym,xl);
						else
						{
							for (i=0; i<_dimension; i++) sym[l1][i]=xz[i];
							ww[l1]=f3;
						}
					}
				}
				_currentIteration++;
			}
			while (c1>_epsilon);
			
			for (i=0; i<lws; i++)
			{
				for (j=0; j<_dimension; j++)
					_parameters[j]=sym[i][j];
				ww[i]=_function.GetVal(_parameters);
			}
			Sort(lws,ref l1, ref l2,ww);

			Trace.WriteLine("Out ",category);

		}
		
		/// <summary>
		/// Sets initial variables
		/// </summary>
		/// <param name="sym"></param>
		/// <param name="x"></param>
		/// <param name="n"></param>
		/// <param name="m"></param>
		/// <param name="alf"></param>
		private void BeginProcess(double [][]sym, double []x, int n, int m, double alf)
		{
			double[][]d = Constants.GenerateMatrix(m,m);
			double c1;
			int i,j,k;

			//create identity matrix
			for (i=0; i<n; i++)
				for (j=0; j<n; j++)
					if (i==j) d[i][j]=1.0;

			for (i=0; i<n; i++) 
				sym[0][i]=x[i];

			for (i=1; i<m; i++)
			{
				c1=0;
				j=i-1;
				for (k=0; k<n; k++) 
					c1+=d[j][k];
				x[j]+=alf*c1;
				for (k=0; k<n; k++) 
					sym[i][k]=x[k];
				x[j]+=-alf*c1;
			}
		}

		/// <summary>
		/// Reflects the simplex with respect to
		/// some line
		/// </summary>
		/// <param name="n"></param>
		/// <param name="xh"></param>
		/// <param name="xc"></param>
		/// <param name="xsr"></param>
		private void Reflection(int n, double []xh, double[] xc, double [] xsr)
		{
			int i;
			for (i=0; i<n; i++)
				xsr[i]=xc[i]+_lambda*(xc[i]-xh[i]);
		}

		/// <summary>
		/// Moves simples along the given dimension
		/// </summary>
		/// <param name="n"></param>
		/// <param name="xsr"></param>
		/// <param name="xc"></param>
		/// <param name="xek"></param>
		private void Expansion(int n, double []xsr, double []xc, double []xek)
		{
			int i;
			for (i=0; i<n; i++)
				xek[i]=xsr[i]+_beta*(xsr[i]-xc[i]);
		}

		/// <summary>
		/// Moves a dimension of the simplex into opposite direction
		/// </summary>
		/// <param name="n"></param>
		/// <param name="xh"></param>
		/// <param name="xc"></param>
		/// <param name="xz"></param>
		private void Contraction(int n, double []xh, double []xc, double []xz)
		{
			int i;
			for (i=0; i<n; i++)
				xz[i]=xc[i]+_gamma*(xh[i]-xc[i]);
		}

		/// <summary>
		/// Reduces distance between the current value
		/// and upper value for a given dimension
		/// </summary>
		/// <param name="n"></param>
		/// <param name="m"></param>
		/// <param name="sym"></param>
		/// <param name="xl"></param>
		private void Reduction(int n, int m, double [][]sym, double []xl)
		{ 
			int i,j;
			for (i=0; i<m; i++)
				for (j=0; j<n; j++)
					sym[i][j]=(xl[j]+sym[i][j])/2.0;
		}

		/// <summary>
		/// Selects min and max values of a vector
		/// </summary>
		/// <param name="n"></param>
		/// <param name="p"></param>
		/// <param name="q"></param>
		/// <param name="x"></param>
		private void Sort(int n, ref int p, ref int q, double []x)
		{ 
			int i;
			double max,min;
			max=x[1];
			p=1;
			for (i=0; i<n; i++)
				if (x[i]>max)
				{
					p=i;
					max=x[i];
				}
			min=x[1];
			q=1;
			for (i=0; i<n; i++)
				if (x[i]<min)
				{
					q=i;
					min=x[i];
				}
		}

	}//end of class
}//end of namespace
