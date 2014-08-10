using NUnit.Framework;
using DotNetMatrix;

namespace Net.Kniaz.Optimization.QuasiNewton.Tests
{
	#region Functions
	
	public class TestFunction: RealFunction
	{

		public override int Dimension
		{
			get{return 3;}
		}
		public override double GetVal(double[] x)
		{
			double x1,x2,x3;
			x1=x[0];
			x2=x[1];
			x3=x[2];
			return 5*x1*x1+2*x2*x2+2*x3*x3+2*x1*x2+2*x2*x3-2*x1*x3-6*x3;
		}


	}

	public class Parabola: RealFunction
	{
		public override int Dimension
		{
			get{return 3;}
		}
		public override double GetVal(double[] x)
		{
			double x1,x2,x3;
			x1=x[0];
			x2=x[1];
			x3=x[2];
			return x1*x1 + x2*x2+ x3*x3;
		}

	}

	public class Banana : RealFunction
	{
		public override int Dimension
		{
			get{return 2;}
		}

	
		private double sqr(double x)
		{
			return x*x;
		}
		

		public override double GetVal(double[] x)
		{
			double c;
			c=100*sqr(x[0]-sqr(x[1]))+(1-x[1])*(1-x[1]);
			return(c);
		}

	}


		#endregion

	[TestFixture()]
	public class Derivatives
	{

		[Test()]
		public void PartialDerivative00()
		{
			double tVal=10;
			double eps=1e-5;
			double[] vector = {0,0,0};
			TestFunction f = new TestFunction();
			double der = f.GetPartialDerivativeVal(0,0,vector);
			Assert.IsTrue(System.Math.Abs(tVal-der)<=eps);
		}

		[Test()]
		public void PartialDerivative01()
		{
			double tVal=2;
			double eps=1e-5;
			double[] vector = {0,0,0};
			TestFunction f = new TestFunction();
			double der = f.GetPartialDerivativeVal(0,1,vector);
			Assert.IsTrue(System.Math.Abs(tVal-der)<=eps);
		}

		[Test()]
		public void PartialDerivative02()
		{
			double tVal=-2;
			double eps=1e-5;
			double[] vector = {0,0,0};
			TestFunction f = new TestFunction();
			double der = f.GetPartialDerivativeVal(0,2,vector);
			Assert.IsTrue(System.Math.Abs(tVal-der)<=eps);
		}

		[Test()]
		public void PartialDerivative10()
		{
			double tVal=2;
			double eps=1e-5;
			double[] vector = {0,0,0};
			TestFunction f = new TestFunction();
			double der = f.GetPartialDerivativeVal(1,0,vector);
			Assert.IsTrue(System.Math.Abs(tVal-der)<=eps);
		}

		[Test()]
		public void PartialDerivative11()
		{
			double tVal=4;
			double eps=1e-5;
			double[] vector = {0,0,0};
			TestFunction f = new TestFunction();
			double der = f.GetPartialDerivativeVal(1,1,vector);
			Assert.IsTrue(System.Math.Abs(tVal-der)<=eps);
		}

		[Test()]
		public void PartialDerivative12()
		{
			double tVal=2;
			double eps=1e-5;
			double[] vector = {0,0,0};
			TestFunction f = new TestFunction();
			double der = f.GetPartialDerivativeVal(1,2,vector);
			Assert.IsTrue(System.Math.Abs(tVal-der)<=eps);
		}

		[Test()]
		public void PartialDerivative20()
		{
			double tVal=-2;
			double eps=1e-5;
			double[] vector = {0,0,0};
			TestFunction f = new TestFunction();
			double der = f.GetPartialDerivativeVal(2,0,vector);
			Assert.IsTrue(System.Math.Abs(tVal-der)<=eps);
		}

		[Test()]
		public void PartialDerivative21()
		{
			double tVal=2;
			double eps=1e-5;
			double[] vector = {0,0,0};
			TestFunction f = new TestFunction();
			double der = f.GetPartialDerivativeVal(2,1,vector);
			Assert.IsTrue(System.Math.Abs(tVal-der)<=eps);
		}

		[Test()]
		public void PartialDerivative22()
		{
			double tVal=4;
			double eps=1e-5;
			double[] vector = {0,0,0};
			TestFunction f = new TestFunction();
			double der = f.GetPartialDerivativeVal(2,2,vector);
			Assert.IsTrue(System.Math.Abs(tVal-der)<=eps);
		}

		[Test()]
		public void Hessian()
		{
			double eps = 1e-5;
			double[] vector = {0,0,0};
			double[][] result = {new double[]{10.0, 2.0, -2.0}, 
									new double[]{ 2.0, 4.0,  2.0},
									new double[]{-2.0, 2.0,  4.0}};
			GeneralMatrix expectedMatrix = new GeneralMatrix(result);
			TestFunction f = new TestFunction();
			GeneralMatrix hessian = f.CalculateHessian(vector);
			for (int i=0;i<3; i++)
				for (int j=0;j<3; j++)
					Assert.IsTrue(System.Math.Abs(hessian.GetElement(i,j)-expectedMatrix.GetElement(i,j))<eps);
		}


	}
}
