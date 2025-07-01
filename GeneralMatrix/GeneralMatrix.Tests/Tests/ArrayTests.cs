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
		//declaring and creting test variables, to be redefined in the init method
		GeneralMatrix A = new GeneralMatrix(1,1);
		GeneralMatrix R = new GeneralMatrix(1, 1);
		GeneralMatrix S = new GeneralMatrix(1, 1);
		GeneralMatrix Z = new GeneralMatrix(1, 1);
		GeneralMatrix O = new GeneralMatrix(1, 1);

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
		//ExpectedMessage="GeneralMatrix dimensions must agree
		public void Negative_Substract()
		{
			S = new GeneralMatrix(columnwise, nonconformld);
			Assert.That(()=>A.Subtract(S), Throws.ArgumentException);
		}

		[Test()]
		public void Substract()
		{
			A=R;
            Assert.That(A.Subtract(R).Norm1(), Is.EqualTo(0.0));
        }

		[Test()]
		public void SubstractEquals()
		{
			A = R.Copy();
            Assert.That(A.Norm1(), Is.Not.EqualTo(0.0));
			A.SubtractEquals(R);
            Assert.That(A.Subtract(Z).Norm1(), Is.Not.EqualTo(0.0));
        }

        [Test()]

        //ExpectedMessage="GeneralMatrix dimensions must agree"
        public void Negative_SubstractEquals()
        {
            Assert.That(() => A.SubtractEquals(S), Throws.ArgumentException);
        }

        [Test()]
        public void SubstractZeroMatrix()
        {
            Z = new GeneralMatrix(A.RowDimension, A.ColumnDimension);
            Assert.That(A.Subtract(Z).Norm1(), Is.Not.EqualTo(0.0));

		}

		[Test()]
		//GeneralMatrix dimensions must agree.
		public void Negative_AddMatrix()
		{
			Assert.That(()=> A.Add(S), Throws.ArgumentException);
		}

		[Test()]
		public void SubstractAndAdd()
		{
			GeneralMatrix B = GeneralMatrix.Random(A.RowDimension, A.ColumnDimension);
			GeneralMatrix C = A.Subtract(B);
            Assert.That(GeneralTests.Check(C.Add(B), A), Is.True);
		}

	
		[Test()]
		//"GeneralMatrix dimensions must agree."
		public void Negative_AddEquals()
		{

				Assert.That( () => A.AddEquals(S), Throws.ArgumentException);
		}

		[Test()]
		public void SubstractAndAddEquals()
		{
			GeneralMatrix B = GeneralMatrix.Random(A.RowDimension, A.ColumnDimension);
			GeneralMatrix C = A.Subtract(B);
			C.AddEquals(B);
			Assert.That(GeneralTests.Check(A, C), Is.True);

		}

		[Test()]
		public void UnaryMinus()
		{
			A = R.UnaryMinus();
			Z = new GeneralMatrix(A.RowDimension, A.ColumnDimension);
			Assert.That(GeneralTests.Check(A.Add(R), Z), Is.True);

		}

		[Test()]
		//"GeneralMatrix dimensions must agree."
		public void Negative_LeftDivide()
		{
			Assert.That(()=>A.ArrayLeftDivide(S), Throws.ArgumentException);
		}

		[Test()]
		public void LeftDivide()
		{
			A = R.Copy();
			GeneralMatrix C = A.ArrayLeftDivide(R);
			Assert.That(GeneralTests.Check(C, O), Is.True);

		}

		[Test()]
		//"GeneralMatrix dimensions must agree."
		public void Negative_LeftDivideEquals()
		{
			Assert.That(()=> A.ArrayLeftDivideEquals(S), Throws.ArgumentException);
		}

		[Test()]
		public void LeftDivideEquals()
		{
			A = R.Copy();
			A.ArrayLeftDivideEquals(R);
			Assert.That(GeneralTests.Check(A, O), Is.True);
		}
		
		[Test()]
		//"GeneralMatrix dimensions must agree."
		public void Negative_RightDivide()
		{
			Assert.That(()=>A.ArrayRightDivide(S), Throws.ArgumentException);
		}

		[Test()]
		public void RightDivide()
		{
			A = R.Copy();
			GeneralMatrix C = A.ArrayRightDivide(R);
			Assert.That(GeneralTests.Check(C, O), Is.True);
		}

		[Test()]
		//"GeneralMatrix dimensions must agree."
		public void Negative_RightDivideEquals()
		{
			Assert.That(()=>A.ArrayRightDivideEquals(S), Throws.ArgumentException);
		}

		[Test()]
		public void RightDivideEquals()
		{
			A = R.Copy();
			A.ArrayRightDivideEquals(R);
			Assert.That(GeneralTests.Check(A, O), Is.True);
		}

		[Test()]
		//ExpectedMessage = "GeneralMatrix dimensions must agree."
		public void Negative_ArrayMultiply()
		{
			Assert.That(()=>A.ArrayMultiply(S), Throws.ArgumentException);
		}

		[Test()]
		public void ArrayMultiply()
		{
			A = R.Copy();
			GeneralMatrix B = GeneralMatrix.Random(A.RowDimension, A.ColumnDimension);
			GeneralMatrix C = A.ArrayMultiply(B);
			Assert.That(GeneralTests.Check(C.ArrayRightDivideEquals(B), A), Is.True);
		}

		[Test()]
		//ExpectedMessage = "GeneralMatrix dimensions must agree."
		public void Negative_ArrayMultiplyEquals()
		{
			Assert.That(()=>A.ArrayMultiplyEquals(S), Throws.ArgumentException);
		}


		[Test()]
		public void ArrayMultiplyEquals()
		{
			A = R.Copy();
			GeneralMatrix B = GeneralMatrix.Random(A.RowDimension, A.ColumnDimension);
			A.ArrayMultiplyEquals(B);
			Assert.That(GeneralTests.Check(A.ArrayRightDivideEquals(B), R), Is.True);
		}

	}
}
