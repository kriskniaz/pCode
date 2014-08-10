using System;
using System.Collections;
using DotNetMatrix;
using NUnit.Framework;
using DotNetMatrix.Tests;


namespace DotNetMatrix.Tests
{
	/// <summary>Linear Algebra methods:
	/// Transpose
	/// Multiply
	/// Condition
	/// Rank
	/// Determinant
	/// trace
	/// Norm1
	/// norm2
	/// normF
	/// normInf
	/// Solve
	/// solveTranspose
	/// Inverse
	/// chol
	/// Eigen
	/// lu
	/// qr
	/// svd 
	/// 
	/// </summary>
	/// 
	[TestFixture()]
	public class LinearAlgebraTests
	{
		GeneralMatrix A,T;
		double[] columnwise = new double[]{1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0, 11.0, 12.0};
		double[][] tvals = {new double[]{1.0, 2.0, 3.0}, new double[]{4.0, 5.0, 6.0}, new double[]{7.0, 8.0, 9.0}, new double[]{10.0, 11.0, 12.0}};
		int validld = 3; /* leading dimension of intended test Matrices */
		double columnsummax = 33.0;
		double rowsummax = 30.0;
		double sumofdiagonals = 15;
		double sumofsquares = 650;

		[SetUp()]
		public void InitData()
		{
			A = new GeneralMatrix(columnwise, validld);
			T = new GeneralMatrix(tvals);
		}

		[Test()]
		public void Transpose1()
		{
			T = A.Transpose();
			Assert.IsTrue(GeneralTests.Check(A.Transpose(), T));
		}

		[Test()]
		public void Transpose2()
		{
			A.Transpose();
			Assert.IsTrue(GeneralTests.Check(A.Norm1(), columnsummax));
		}

		[Test()]
		public void Transpose3()
		{
			A.Transpose();
			Assert.IsTrue(GeneralTests.Check(A.NormInf(), rowsummax));

		}

		[Test()]
		public void Transpose4()
		{
			A.Transpose();
			Assert.IsTrue(GeneralTests.Check(A.NormF(), System.Math.Sqrt(sumofsquares)));

		}

		[Test()]
		public void Transpose5()
		{
			A.Transpose();
			Assert.IsTrue(GeneralTests.Check(A.Trace(), sumofdiagonals));

		}

		[Test()]
		public void Transpose6()
		{
			A.Transpose();

			Assert.IsTrue(GeneralTests.Check(A.GetMatrix(0, A.RowDimension - 1, 0, A.RowDimension - 1).Determinant(), 0.0));
		}

		[Test()]
		public void MultiplyTranspose()
		{
			double[][] square = {new double[]{166.0, 188.0, 210.0}, new double[]{188.0, 214.0, 240.0}, new double[]{210.0, 240.0, 270.0}};
			GeneralMatrix sq  = new GeneralMatrix(square);
			Assert.IsTrue(GeneralTests.Check(A.Multiply(A.Transpose()), sq));
		}

		[Test()]
		public void MultiplyZero()
		{
			GeneralMatrix Z = new GeneralMatrix(A.RowDimension, A.ColumnDimension);
			Assert.IsTrue(GeneralTests.Check(A.Multiply(0.0),Z));
		}

		[Test()]
		public void QRDecomposition()
		{
			GeneralMatrix A = new GeneralMatrix(columnwise, 4);

			QRDecomposition QR = A.QRD();
			GeneralMatrix R = QR.R;
			Assert.IsTrue(GeneralTests.Check(A, QR.Q.Multiply(R)));
		}

		[Test()]
		public void SingularValueDecomposition()
		{
			GeneralMatrix A = new GeneralMatrix(columnwise, 4);
			SingularValueDecomposition SVD = A.SVD();
			Assert.IsTrue(GeneralTests.Check(A, SVD.GetU().Multiply(SVD.S.Multiply(SVD.GetV().Transpose()))));
		}

		[Test()]
		public void DEF()
		{
			double[][] rankdef = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
			GeneralMatrix def = new GeneralMatrix(rankdef);
			Assert.IsTrue(GeneralTests.Check(def.Rank(), System.Math.Min(def.RowDimension, def.ColumnDimension) - 1));
		}

		[Test()]
		public void SingularValues()
		{
			double[][] condmat = {new double[]{1.0, 3.0}, new double[]{7.0, 9.0}};
			GeneralMatrix B = new GeneralMatrix(condmat);
			SingularValueDecomposition SVD = B.SVD();
			double[] singularvalues = SVD.SingularValues;
			Assert.IsTrue(GeneralTests.Check(B.Condition(), singularvalues[0] / singularvalues[System.Math.Min(B.RowDimension, B.ColumnDimension) - 1]));
		}

		[Test()]
		public void LUDecomposition()
		{
			GeneralMatrix A = new GeneralMatrix(columnwise, 4);
			int n = A.ColumnDimension;
			A = A.GetMatrix(0, n - 1, 0, n - 1);
			A.SetElement(0, 0, 0.0);
			LUDecomposition LU = A.LUD();
			Assert.IsTrue(GeneralTests.Check(A.GetMatrix(LU.Pivot, 0, n - 1), LU.L.Multiply(LU.U)));
		}

		[Test()]
		public void Inverse()
		{
			GeneralMatrix r = GeneralMatrix.Random(4,4);
			GeneralMatrix iR = r.Inverse();
			Assert.IsTrue(GeneralTests.Check(r.Multiply(iR),GeneralMatrix.Identity(4,4)));

		}

		[Test()]
		public void SolveLinear()
		{
			double[][] subavals = {new double[]{5.0, 8.0, 11.0}, new double[]{6.0, 9.0, 12.0}};
			double[][] sqSolution = {new double[]{13.0}, new double[]{15.0}};
			GeneralMatrix sub = new GeneralMatrix(subavals);
			GeneralMatrix o = new GeneralMatrix(sub.RowDimension, 1, 1.0);
			GeneralMatrix sol = new GeneralMatrix(sqSolution);
			GeneralMatrix sq = sub.GetMatrix(0, sub.RowDimension - 1, 0, sub.RowDimension - 1);
			Assert.IsTrue(GeneralTests.Check(sq.Solve(sol), o));
		}

		[Test()]
		public void CholeskyDecomposition1()
		{
			double[][] pvals = {new double[]{1.0, 1.0, 1.0}, new double[]{1.0, 2.0, 3.0}, new double[]{1.0, 3.0, 6.0}};
			GeneralMatrix A = new GeneralMatrix(pvals);
			CholeskyDecomposition chol = A.chol();
			GeneralMatrix L = chol.GetL();
			Assert.IsTrue(GeneralTests.Check(A, L.Multiply(L.Transpose())));

		}

		[Test()]
		public void CholeskyDecomposition2()
		{
			double[][] pvals = {new double[]{1.0, 1.0, 1.0}, new double[]{1.0, 2.0, 3.0}, new double[]{1.0, 3.0, 6.0}};
			GeneralMatrix A = new GeneralMatrix(pvals);
			CholeskyDecomposition chol = A.chol();
			GeneralMatrix X = chol.Solve(GeneralMatrix.Identity(3, 3));
			Assert.IsTrue(GeneralTests.Check(A.Multiply(X), GeneralMatrix.Identity(3, 3)));

		}

		[Test()]
		public void EigenValueDecomposition1()
		{
			double[][] pvals = {new double[]{1.0, 1.0, 1.0}, new double[]{1.0, 2.0, 3.0}, new double[]{1.0, 3.0, 6.0}};
			GeneralMatrix A = new GeneralMatrix(pvals);
			EigenvalueDecomposition Eig = A.Eigen();
			GeneralMatrix D = Eig.D;
			GeneralMatrix V = Eig.GetV();
			Assert.IsTrue(GeneralTests.Check(A.Multiply(V), V.Multiply(D)));


		}

		[Test()]
		public void EigenValueDecomposition2()
		{
			double[][] evals = {new double[]{0.0, 1.0, 0.0, 0.0}, new double[]{1.0, 0.0, 2e-7, 0.0}, new double[]{0.0, - 2e-7, 0.0, 1.0}, new double[]{0.0, 0.0, 1.0, 0.0}};
			GeneralMatrix A = new GeneralMatrix(evals);
			EigenvalueDecomposition Eig = A.Eigen();
			GeneralMatrix D = Eig.D;
			GeneralMatrix V = Eig.GetV();
			Assert.IsTrue(GeneralTests.Check(A.Multiply(V), V.Multiply(D)));

		}

	}
}
