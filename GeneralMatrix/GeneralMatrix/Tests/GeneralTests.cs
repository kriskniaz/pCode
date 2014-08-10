using System;
using System.Collections;
using DotNetMatrix;
using NUnit.Framework;

namespace DotNetMatrix.Tests
{
	/// <summary>
	/// Unit Tests for Accessors and constructors 
	/// and few of the Linear Algebra Methods.
	/// In addition it contains few static methods
	/// used in other unit tests
	/// </summary>
	/// 
	[TestFixture()]
	public class GeneralTests
	{

		GeneralMatrix A;
		double tmp;
		double[] columnwise = new double[]{1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0, 11.0, 12.0};
		double[] rowwise = new double[]{1.0, 4.0, 7.0, 10.0, 2.0, 5.0, 8.0, 11.0, 3.0, 6.0, 9.0, 12.0};
		double[][] avals = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
		double[][] rankdef;
		double[][] tvals = {new double[]{1.0, 2.0, 3.0}, new double[]{4.0, 5.0, 6.0}, new double[]{7.0, 8.0, 9.0}, new double[]{10.0, 11.0, 12.0}};
		double[][] subavals = {new double[]{5.0, 8.0, 11.0}, new double[]{6.0, 9.0, 12.0}};
		double[][] rvals = {new double[]{1.0, 4.0, 7.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
		double[][] pvals = {new double[]{1.0, 1.0, 1.0}, new double[]{1.0, 2.0, 3.0}, new double[]{1.0, 3.0, 6.0}};
		double[][] ivals = {new double[]{1.0, 0.0, 0.0, 0.0}, new double[]{0.0, 1.0, 0.0, 0.0}, new double[]{0.0, 0.0, 1.0, 0.0}};
		double[][] evals = {new double[]{0.0, 1.0, 0.0, 0.0}, new double[]{1.0, 0.0, 2e-7, 0.0}, new double[]{0.0, - 2e-7, 0.0, 1.0}, new double[]{0.0, 0.0, 1.0, 0.0}};
		double[][] square = {new double[]{166.0, 188.0, 210.0}, new double[]{188.0, 214.0, 240.0}, new double[]{210.0, 240.0, 270.0}};
		double[][] sqSolution = {new double[]{13.0}, new double[]{15.0}};
		double[][] condmat = {new double[]{1.0, 3.0}, new double[]{7.0, 9.0}};
		int[] rowindexset = new int[]{1, 2};
		int[] badrowindexset = new int[]{1, 3};
		int[] columnindexset = new int[]{1, 2, 3};
		int[] badcolumnindexset = new int[]{1, 2, 4};

		public GeneralTests()
		{
			rankdef = avals;
		}

		#region Private Helpers
		private double[][] PrepArray(int m, int n, double s)
		{
			double [][]A;
			int i,j;

			A = new double[m][];
			for (i = 0; i < m; i++)
			{
				A[i] = new double[n];
			}

			for (i=0; i<m; i++)
			{
				for (j=0; j<n; j++)
				{
					A[i][j]=s;
				}
			}

			return A;

		}
		#endregion

		#region Public Static Helpers


		/// <summary>
		/// Function checks if two double values are within the error
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns>true if they equal are within the error</returns>
		public static bool Check(double x, double y)
		{
			bool result=false;

			double eps = System.Math.Pow(2.0, - 52.0);
			
			if (x == 0 & System.Math.Abs(y) < 10 * eps)
				result=true ;

			else if (y == 0 & System.Math.Abs(x) < 10 * eps)
				result=true;

			else if (System.Math.Abs(x - y) > 10 * eps * System.Math.Max(System.Math.Abs(x), System.Math.Abs(y)))
			{
				throw new System.SystemException("The difference x-y is too large: x = " + x.ToString() + "  y = " + y.ToString());
			}
			else result=true;

			return result;
		}
		
	
		/// <summary>
		/// Check norm of difference of "vectors"
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns>true if difference for each dimension less than error</returns>
		public static bool Check(double[] x, double[] y)
		{
			bool result = false;
			if (x.Length == y.Length)
			{
				for (int i = 0; i < x.Length; i++)
				{
					result = Check(x[i], y[i]);
				}
			}
			else
			{
				throw new System.SystemException("Attempt to compare vectors of different lengths");
			}

			return result;
		}
		

		/// <summary>
		/// 
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static bool Check(double[][] x, double[][] y)
		{
			GeneralMatrix A = new GeneralMatrix(x);
			GeneralMatrix B = new GeneralMatrix(y);
			return Check(A, B);
		}
		
		/// <summary>Check norm of difference of Matrices.
		/// </summary>	
		public static bool Check(GeneralMatrix X, GeneralMatrix Y)
		{
			bool result=false;

			double eps = System.Math.Pow(2.0, - 52.0);
			if (X.Norm1() == 0.0 & Y.Norm1() < 10 * eps)
				result = true ;
			else if (Y.Norm1() == 0.0 & X.Norm1() < 10 * eps)
				result = true ;
			else if (X.Subtract(Y).Norm1() > 1000 * eps * System.Math.Max(X.Norm1(), Y.Norm1()))
			{
				throw new System.SystemException("The norm of (X-Y) is too large: " + X.Subtract(Y).Norm1().ToString());
			}
			else result = true;

			return result;
		}
		#endregion

		[Test()]
		public void TestConstructor_mns()
		{
			int m=2;
			int n=3;
			int i,j;
			double s=2.0;
			bool _equal = true;

			GeneralMatrix _gm = new GeneralMatrix(m,n,s);
			GeneralMatrix _ngm = new GeneralMatrix(PrepArray(m,n,s));
			double [][]A1 = _gm.ArrayCopy;
			double [][]A2 = _ngm.ArrayCopy;

			for (i=0; i<m; i++)
			{
				for (j=0; j<n; j++)
				{

					if (A1[i][j]!= A2[i][j])
					{
						_equal=false;
						break;
					}
				}
			}
			
			Assert.IsTrue(_equal);

		}

		[Test()]
		public void TestTranspose()
		{
			GeneralMatrix _gm = new GeneralMatrix(2,2);
			_gm.SetElement(0,0,1);
			_gm.SetElement(0,1,2);
			_gm.SetElement(1,0,3);
			_gm.SetElement(1,1,4);
			GeneralMatrix _ngm = _gm.Transpose();

			Assert.AreEqual(1,0,2);

		}

		/// <summary>
		/// Tests Norm1 - returns max sum of column
		/// </summary>
		[Test()]
		public void TestNorm1()
		{
			GeneralMatrix _gm = new GeneralMatrix(2,2);
			_gm.SetElement(0,0,1);
			_gm.SetElement(0,1,2);
			_gm.SetElement(1,0,3);
			_gm.SetElement(1,1,4);

			Assert.AreEqual(6,_gm.Norm1());


		}

		/// <summary>
		/// Tests solving linear equation.
		/// The solution vector is [1,-2]
		/// </summary>
		[Test()]
		public void TestSolve()
		{
			GeneralMatrix _ls = new GeneralMatrix(2,2);
			_ls.SetElement(0,0,1);
			_ls.SetElement(0,1,2);
			_ls.SetElement(1,0,3);
			_ls.SetElement(1,1,4);

			GeneralMatrix _rs = new GeneralMatrix(2,1);
			_rs.SetElement(0,0,-3);
			_rs.SetElement(1,0,-5);

			GeneralMatrix _solution = _ls.Solve(_rs);

			Assert.AreEqual(_solution.GetElement(0,0),1);
			Assert.AreEqual(_solution.GetElement(1,0),-2);

		}

		[Test()]
        [ExpectedException(typeof(System.ArgumentException), ExpectedMessage = "Array length must be a multiple of m.")]
		public void Negative_InvalidLenghtToConstructor()
		{
			double[] columnwise = new double[]{1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0, 11.0, 12.0};
			int invalidld = 5; /* should trigger bad shape for construction with val */
			A = new GeneralMatrix(columnwise, invalidld);
			
		}

		[Test()]
        [ExpectedException(typeof(System.ArgumentException), ExpectedMessage = "All rows must have the same length.")]
		public void Negative_InvalidRaggedArrayToConstructor()
		{
			double[][] rvals = {new double[]{1.0, 4.0, 7.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
			A = new GeneralMatrix(rvals);
		}

		/// <summary>
		/// Tests substration
		/// </summary>
		[Test()]
		public void Substraction()
		{
			double[] columnwise = new double[]{1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0, 11.0, 12.0};
			double[][] avals = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};

			int validld = 3; /* leading dimension of intended test Matrices */

			//one dimensional array of doubles packed by columns ala Fortran
			//is passed to the constructor 
			//the integer value tell the constructor how to
			//breakup one domensional value into mulidimensional column
			GeneralMatrix A = new GeneralMatrix(columnwise, validld);
			GeneralMatrix B = new GeneralMatrix(avals);
			double tmp = B.GetElement(0, 0);
			avals[0][0] = 0.0;
			GeneralMatrix C = B.Subtract(A);
			avals[0][0] = tmp;
			B = GeneralMatrix.Create(avals);
			tmp = B.GetElement(0, 0);
			avals[0][0] = 0.0;
			Assert.IsTrue(tmp==B.GetElement(0, 0));
		}

		[Test()]
		public void Identity()
		{
			double[][] ivals = {new double[]{1.0, 0.0, 0.0, 0.0}, new double[]{0.0, 1.0, 0.0, 0.0}, new double[]{0.0, 0.0, 1.0, 0.0}};
			GeneralMatrix I = new GeneralMatrix(ivals);

			GeneralMatrix K = GeneralMatrix.Identity(3,4);

			Assert.IsTrue(I.Norm1()==K.Norm1()&&I.Norm1()==1);

		}

		[Test()]
		public void RowAccessor()
		{
			double[][] avals = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
			GeneralMatrix B = new GeneralMatrix(avals);

			Assert.AreEqual(3,B.RowDimension);
		}

		[Test()]
		public void ColumnAccessor()
		{
			double[][] avals = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
			GeneralMatrix B = new GeneralMatrix(avals);

			Assert.AreEqual(4,B.ColumnDimension);
		}

		[Test()]
		public void ArrayAccessor()
		{
			double[][] avals = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
			GeneralMatrix B = new GeneralMatrix(avals);
			double[][] barray = B.Array;
			Assert.AreEqual(barray,avals);

		}

		[Test()]
		public void ArrayCopy()
		{
			double[][] avals = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
			GeneralMatrix B = new GeneralMatrix(avals);
			double[][] barray = B.ArrayCopy;
			Assert.IsTrue(Check(avals,barray));
		}

		[Test()]
		public void ColumnPackedCopy()
		{
			double[][] avals = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
			GeneralMatrix B = new GeneralMatrix(avals);
			double[] bpacked = B.ColumnPackedCopy;
			Assert.IsTrue(Check(bpacked,columnwise));
		}

		[Test()]
		public void RowPackedCopy()
		{
			double[][] avals = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
			GeneralMatrix B = new GeneralMatrix(avals);
			double[] bpacked = B.RowPackedCopy;
			Assert.IsTrue(Check(bpacked,rowwise));

		}

		[Test()]
        [ExpectedException(typeof(System.IndexOutOfRangeException), ExpectedMessage = "Index was outside the bounds of the array.")]
		public void Negative_GetElementRow()
		{
			double[][] avals = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
			GeneralMatrix B = new GeneralMatrix(avals);
			tmp = B.GetElement(B.RowDimension, B.ColumnDimension - 1);
		}

		[Test()]
        [ExpectedException(typeof(System.IndexOutOfRangeException), ExpectedMessage = "Index was outside the bounds of the array.")]
		public void Negative_GetElementColumn()
		{
			double[][] avals = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
			GeneralMatrix B = new GeneralMatrix(avals);
			tmp = B.GetElement(B.RowDimension-1, B.ColumnDimension);
		}

		[Test()]
		public void GetElement()
		{
			double[][] avals = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
			GeneralMatrix B = new GeneralMatrix(avals);
			Assert.AreEqual(B.GetElement(B.RowDimension-1,B.ColumnDimension-1),avals[B.RowDimension-1][B.ColumnDimension-1]);
		}

	}

}
