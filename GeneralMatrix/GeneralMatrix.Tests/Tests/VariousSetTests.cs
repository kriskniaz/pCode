using System;
using System.Collections;
using DotNetMatrix;
using NUnit.Framework;
using DotNetMatrix.Tests;

namespace DotNetMatrix.Tests
{
	/// <summary>
	/// Summary description for VariousSetTests.
	/// </summary>
	/// 
	[TestFixture()]
	public class VariousSetTests
	{
		GeneralMatrix B, M, SUB;
		int ib = 1, ie = 2, jb = 1, je = 3; /* index ranges for sub GeneralMatrix */
		double[][] avals = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
		double[][] subavals = {new double[]{5.0, 8.0, 11.0}, new double[]{6.0, 9.0, 12.0}};
		int[] rowindexset = new int[]{1, 2};
		int[] badrowindexset = new int[]{1, 3};
		int[] columnindexset = new int[]{1, 2, 3};
		int[] badcolumnindexset = new int[]{1, 2, 4};


		public VariousSetTests()
		{
			B = new GeneralMatrix(avals);
			SUB = new GeneralMatrix(subavals);
			M = new GeneralMatrix(2, 3, 0.0);
		}


		[Test()]
		//ExpectedMessage = "Index was outside the bounds of the array."
		public void BadSetValue1()
		{

			Assert.That( ()=> B.SetElement(B.RowDimension, B.ColumnDimension - 1, 0.0), Throws.Exception);
		}

		[Test()]
		//ExpectedMessage = "Index was outside the bounds of the array."
		public void BadSetValue2()
		{
			Assert.That( ()=> B.SetElement(B.RowDimension - 1, B.ColumnDimension, 0.0), Throws.Exception);
		}

		[Test()]
		public void SetElement()
		{
			B.SetElement(ib, jb, 0.0);
			double tmp = B.GetElement(ib, jb);
			Assert.IsTrue(GeneralTests.Check(0.0,tmp));
		}

		[Test()]
		//ExpectedMessage = "Submatrix indices"
		public void Negative_BadSetMatrix1()
		{
			Assert.That( ()=> B.SetMatrix(ib, ie + B.RowDimension + 1, jb, je, M), Throws.Exception);
		}
		
		[Test()]
		public void SetMatrix1()
		{
			B.SetMatrix(ib, ie, jb, je, M);
			Assert.IsTrue(GeneralTests.Check(M.Subtract(B.GetMatrix(ib, ie, jb, je)), M));
		}

		[Test()]
		public void SetSubMatrix()
		{
			B.SetMatrix(ib, ie, jb, je, SUB);
			GeneralMatrix NewSub = B.GetMatrix(ib, ie, jb, je);
			Assert.IsTrue(GeneralTests.Check(SUB,NewSub));
		}

		[Test()]
		//ExpectedMessage = "Submatrix indices"
		public void Negative_BadSetMatrix2()
		{
			Assert.That(()=> B.SetMatrix(ib, ie + B.RowDimension + 1, columnindexset, M), Throws.Exception);
		}

		[Test()]
		//ExpectedMessage = "Submatrix indices"
		public void Negative_BadSetMatrix3()
		{
			Assert.That(()=>B.SetMatrix(ib, ie, badcolumnindexset, M), Throws.Exception);
		}

		[Test()]
		public void SetMatrix2()
		{
			B.SetMatrix(ib, ie, columnindexset, M);
			Assert.IsTrue(GeneralTests.Check(M.Subtract(B.GetMatrix(ib, ie, columnindexset)), M));

		}

		[Test()]
		public void SetMatrix3()
		{
			B.SetMatrix(ib, ie, jb, je, SUB);

			GeneralMatrix C = B.GetMatrix(ib, ie, jb, je);

			Assert.IsTrue(GeneralTests.Check(C,SUB));

		}

		[Test()]
		//ExpectedMessage = "Submatrix indices"
		public void Negative_BadSetMatrix4()
		{
			Assert.That(()=> B.SetMatrix(rowindexset, jb, je + B.ColumnDimension + 1, M), Throws.Exception);
		}

		[Test()]
		//ExpectedMessage = "Submatrix indices")]
		public void Negative_BadSetMatrix5()
		{
			Assert.That(()=> B.SetMatrix(badrowindexset, jb, je, M), Throws.Exception);
		}


		[Test()]
		public void SetMatrix4()
		{
			B.SetMatrix(rowindexset, jb, je, M);
			Assert.IsTrue(GeneralTests.Check(M.Subtract(B.GetMatrix(rowindexset, jb, je)), M));
		}

		[Test()]
		//ExpectedMessage = "Submatrix indices"
		public void Negative_BadSetMatrix6()
		{
			Assert.That( ()=> B.SetMatrix(rowindexset, badcolumnindexset, M), Throws.Exception);
		}

		[Test()]
		//ExpectedMessage = "Submatrix indices"
		public void Negative_BadSetMatrix7()
		{
			Assert.That(()=> B.SetMatrix(badrowindexset, columnindexset, M), Throws.Exception);
		}

		[Test()]
		public void SetMatrix5()
		{
			B.SetMatrix(rowindexset, columnindexset, M);
		}

	}
}
