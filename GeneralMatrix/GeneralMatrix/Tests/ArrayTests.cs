using System;
using System.Collections;
using DotNetMatrix;
using NUnit.Framework;
using DotNetMatrix.Tests;

namespace DotNetMatrix.Tests
{
	/// <summary>Array-like methods:
	/// Subtract
	/// SubtractEquals
	/// Add
	/// AddEquals
	/// ArrayLeftDivide
	/// ArrayLeftDivideEquals
	/// ArrayRightDivide
	/// ArrayRightDivideEquals
	/// arrayTimes
	/// ArrayMultiplyEquals
	/// uminus
	/// </summary>
	[TestFixture()]
	public class ArrayTests
	{
		GeneralMatrix A,R,S,Z,O;
		double[] columnwise = new double[]{1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0, 11.0, 12.0};
		int nonconformld = 4; /* leading dimension which is valid, but nonconforming */
		int validld = 3; /* leading dimension of intended test Matrices */


		[SetUp()]
		public void InitData()
		{
			A = new GeneralMatrix(columnwise, validld);
			R = GeneralMatrix.Random(A.RowDimension, A.ColumnDimension);
			S = new GeneralMatrix(columnwise, nonconformld);
			O = new GeneralMatrix(A.RowDimension, A.ColumnDimension, 1.0);
		}

		/// <summary>
		/// Tests illegal substraction - matrices have different dimensions
		/// so the substraction must fail
		/// </summary>
		[Test()]
		[ExpectedException(typeof(System.ArgumentException),ExpectedMessage="GeneralMatrix dimensions must agree.")]
		public void Negative_Substract()
		{
			S = new GeneralMatrix(columnwise, nonconformld);
			S = A.Subtract(S);
		}

		[Test()]
		public void Substract()
		{
			A=R;
			Assert.AreEqual(0.0,A.Subtract(R).Norm1());
		}

		[Test()]
		public void SubstractEquals()
		{
			A = R.Copy();
			Assert.IsFalse(A.Norm1()==0.0);
			A.SubtractEquals(R);
			Assert.IsTrue(A.Norm1() == 0.0);
		}

		[Test()]
		[ExpectedException(typeof(System.ArgumentException),ExpectedMessage="GeneralMatrix dimensions must agree.")]
		public void Negative_SubstractEquals()
		{
			A.SubtractEquals(S);
		}

		[Test()]
		public void SubstractZeroMatrix()
		{
			Z = new GeneralMatrix(A.RowDimension, A.ColumnDimension);
			Assert.IsTrue(A.Subtract(Z).Norm1()!=0.0);

		}

		[Test()]
        [ExpectedException(typeof(System.ArgumentException), ExpectedMessage = "GeneralMatrix dimensions must agree.")]
		public void Negative_AddMatrix()
		{
			A.Add(S);
		}

		[Test()]
		public void SubstractAndAdd()
		{
			GeneralMatrix B = GeneralMatrix.Random(A.RowDimension, A.ColumnDimension);
			GeneralMatrix C = A.Subtract(B);
			Assert.IsTrue(GeneralTests.Check(C.Add(B), A));
		}

		[Test()]
        [ExpectedException(typeof(System.ArgumentException), ExpectedMessage = "GeneralMatrix dimensions must agree.")]
		public void Negative_AddEquals()
		{
			A.AddEquals(S);
		}

		[Test()]
		public void SubstractAndAddEquals()
		{
			GeneralMatrix B = GeneralMatrix.Random(A.RowDimension, A.ColumnDimension);
			GeneralMatrix C = A.Subtract(B);
			C.AddEquals(B);
			Assert.IsTrue(GeneralTests.Check(A,C));

		}

		[Test()]
		public void UnaryMinus()
		{
			A = R.UnaryMinus();
			Z = new GeneralMatrix(A.RowDimension, A.ColumnDimension);
			Assert.IsTrue(GeneralTests.Check(A.Add(R), Z));

		}

		[Test()]
        [ExpectedException(typeof(System.ArgumentException), ExpectedMessage = "GeneralMatrix dimensions must agree.")]
		public void Negative_LeftDivide()
		{
			S = A.ArrayLeftDivide(S);
		}

		[Test()]
		public void LeftDivide()
		{
			A = R.Copy();
			GeneralMatrix C = A.ArrayLeftDivide(R);
			Assert.IsTrue(GeneralTests.Check(C,O));

		}

		[Test()]
        [ExpectedException(typeof(System.ArgumentException), ExpectedMessage = "GeneralMatrix dimensions must agree.")]
		public void Negative_LeftDivideEquals()
		{
			A.ArrayLeftDivideEquals(S);
		}

		[Test()]
		public void LeftDivideEquals()
		{
			A = R.Copy();
			A.ArrayLeftDivideEquals(R);
			Assert.IsTrue(GeneralTests.Check(A,O));
		}

		[Test()]
        [ExpectedException(typeof(System.ArgumentException), ExpectedMessage = "GeneralMatrix dimensions must agree.")]
		public void Negative_RightDivide()
		{
			A.ArrayRightDivide(S);
		}

		[Test()]
		public void RightDivide()
		{
			A = R.Copy();
			GeneralMatrix C = A.ArrayRightDivide(R);
			Assert.IsTrue(GeneralTests.Check(C,O));
		}

		[Test()]
        [ExpectedException(typeof(System.ArgumentException), ExpectedMessage = "GeneralMatrix dimensions must agree.")]
		public void Negative_RightDivideEquals()
		{
			A.ArrayRightDivideEquals(S);
		}

		[Test()]
		public void RightDivideEquals()
		{
			A = R.Copy();
			A.ArrayRightDivideEquals(R);
			Assert.IsTrue(GeneralTests.Check(A,O));
		}

		[Test()]
        [ExpectedException(typeof(System.ArgumentException), ExpectedMessage = "GeneralMatrix dimensions must agree.")]
		public void Negative_ArrayMultiply()
		{
			S = A.ArrayMultiply(S);
		}

		[Test()]
		public void ArrayMultiply()
		{
			A = R.Copy();
			GeneralMatrix B = GeneralMatrix.Random(A.RowDimension, A.ColumnDimension);
			GeneralMatrix C = A.ArrayMultiply(B);
			Assert.IsTrue(GeneralTests.Check(C.ArrayRightDivideEquals(B), A));
		}

		[Test()]
        [ExpectedException(typeof(System.ArgumentException), ExpectedMessage = "GeneralMatrix dimensions must agree.")]
		public void Negative_ArrayMultiplyEquals()
		{
			A.ArrayMultiplyEquals(S);
		}


		[Test()]
		public void ArrayMultiplyEquals()
		{
			A = R.Copy();
			GeneralMatrix B = GeneralMatrix.Random(A.RowDimension, A.ColumnDimension);
			A.ArrayMultiplyEquals(B);
			Assert.IsTrue(GeneralTests.Check(A.ArrayRightDivideEquals(B), R));
		}

	}
}
