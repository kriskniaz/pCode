using System;
using NUnit.Framework;
using DotNetMatrix;


namespace Net.Kniaz.Optimization.QuasiNewton.Tests
{

		/// <summary>
		/// TestDFP.
		/// </summary>
		/// 
		[TestFixture()]
		public class DFPTests
		{
			/// <summary>
			/// Tests finding minimum of a onedimensional
			/// test function that has been obtained via reduction to one dimension
			/// of the multidimensional function i.e
			/// f(p) = f(x1,x2,...xn) where x1 = f1(p), x2 = f2(p), xn = fn(p)
			/// In this case the test function is a tridimensional 
			/// and x1=0, x2 = 0, x3 = 6p
			/// </summary>

			private double Diff(double[]x1, double[]x2, int dim)
			{
				double d=0;
				for (int i=0;i<dim; i++)
					d+=(x1[i]-x2[i])*(x1[i]-x2[i]);
				return System.Math.Sqrt(d);
			}

				
			[Test()]
			public void FitDFP1()
			{
				double eps = 1e-5;
				double rEps = eps*10;
				double step = 1;
				int itMax=50;
				double[] vector = {0,0,0};
				double[] expVector = {1,-2,3};
				TestFunction f = new TestFunction();
				int dim = f.Dimension;
				DFP alg = new DFP(dim, vector, f, step, eps, itMax);
				alg.FindMinimum();
				Assert.IsTrue(Diff(expVector,alg.Minimum,dim)<rEps);
			}

			[Test()]
			public void FitBFGS1()
			{
				double eps = 1e-5;
				double rEps = eps*10;
				double step = 2;
				double[] vector = {0,0,0};
				double[] expVector = {1,-2,3};
				int itMax = 50;
				TestFunction f = new TestFunction();
				int dim = f.Dimension;
				BFGS alg = new BFGS(dim, vector, f, step, eps, itMax);
				alg.FindMinimum();
				Assert.IsTrue(Diff(expVector,alg.Minimum,dim)<rEps);
			}


			[Test()]
			public void FitDFP2()
			{
				double eps = 1e-5;
				double rEps = eps*10;
				double step = 3;
				double[] vector = {6,45,22};
				double[] expVector = {0,0,0};
				int itMax = 50;
				Parabola f = new Parabola();
				int dim = f.Dimension;
				DFP alg = new DFP(dim, vector, f, step, eps, itMax);
				alg.FindMinimum();
				Assert.IsTrue(Diff(expVector,alg.Minimum,dim)<rEps);
			}

			[Test()]
			public void FitBFGS2()
			{
				double eps = 1e-5;
				double rEps = eps*10;
				int itMax = 50;
				double step=1;
				double[] vector = {6,45,22};
				double[] expVector = {0,0,0};
				Parabola f = new Parabola();
				int dim = f.Dimension;
				BFGS alg = new BFGS(dim,vector,f,step,eps,itMax);
				alg.FindMinimum();
				Assert.IsTrue(Diff(expVector,alg.Minimum,dim)<rEps);
			}


			[Test()]
			public void FitDFP3()
			{
				double eps = 1e-5;
				double rEps = eps;
				int itMax = 200;
				double step=1;
				double[] vector = {6,-5};
				double[] expVector = {1,1};
				Banana f = new Banana();
				int dim = f.Dimension;
				DFP alg = new DFP(dim,vector,f,step,eps,itMax);
				alg.FindMinimum();
				Assert.IsTrue(Diff(expVector,alg.Minimum,dim)<rEps);
			}

			[Test()]
			public void FitBFGS3()
			{
				double eps = 1e-5;
				double rEps = eps;
				int itMax = 200;
				double step=1;
				double[] vector = {6,-5};
				double[] expVector = {1,1};
				Banana f = new Banana();
				int dim = f.Dimension;
				BFGS alg = new BFGS(dim,vector,f,step,eps,itMax);
				alg.FindMinimum();
				Assert.IsTrue(Diff(expVector,alg.Minimum,dim)<rEps);
			}

		}
	}
