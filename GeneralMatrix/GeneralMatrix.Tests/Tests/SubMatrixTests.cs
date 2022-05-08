using System;
using System.Collections;
using DotNetMatrix;
using NUnit.Framework;
using DotNetMatrix.Tests;

namespace DotNetMatrix.Tests
{
	/// <summary>
	/// Contains unit tests for the submatrix accessors
	/// </summary>
	/// 
	[TestFixture()]
	public class SubMatrixTests
	{
		GeneralMatrix M = new GeneralMatrix(1, 1);
		GeneralMatrix SUB = new GeneralMatrix(1,1);

		int[] rowindexset = new int[]{1, 2};
		int[] badrowindexset = new int[]{1, 3};
		int[] columnindexset = new int[]{1, 2, 3};
		int[] badcolumnindexset = new int[]{1, 2, 4};

		public SubMatrixTests()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		[Test()]
		//ExpectedMessage = "Submatrix indices"
		public void Negative_WrongIndecesForSubMatrix1()
		{
			int ib = 1, ie = 2, jb = 1, je = 3; /* index ranges for sub GeneralMatrix */
			double[][] avals = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
			GeneralMatrix B = new GeneralMatrix(avals);

			Assert.That(()=>B.GetMatrix(ib, ie + B.RowDimension + 1, jb, je), Throws.Exception);
		}

		[Test()]
		//ExpectedMessage = "Submatrix indices"
		public void Negative_WrongIndecesForSubMatrix2()
		{
			int ib = 1, ie = 2, jb = 1, je = 3; /* index ranges for sub GeneralMatrix */
			double[][] avals = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
			GeneralMatrix B = new GeneralMatrix(avals);

			Assert.That(()=> B.GetMatrix(ib, ie, jb, je + B.ColumnDimension + 1), Throws.Exception);
		}

		[Test()]
		public void SubMatrix()
		{
			int ib = 1, ie = 2, jb = 1, je = 3; /* index ranges for sub GeneralMatrix */
			double[][] avals = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
			double[][] subavals = {new double[]{5.0, 8.0, 11.0}, new double[]{6.0, 9.0, 12.0}};

			GeneralMatrix B = new GeneralMatrix(avals);
			M = B.GetMatrix(ib, ie, jb, je);
			SUB = new GeneralMatrix(subavals);

			Assert.IsTrue(GeneralTests.Check(SUB, M));

		}

		[Test()]
		//ExpectedMessage = "Submatrix indices"
		public void Negative_BadColumnIndexSet1()
		{
			int ib = 1, ie = 2;/* index ranges for sub GeneralMatrix */
			double[][] avals = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
			int[] badrowindexset = new int[]{1, 3};

			GeneralMatrix B = new GeneralMatrix(avals);

			Assert.That(()=>B.GetMatrix(ib, ie, badcolumnindexset), Throws.Exception);

		}

		[Test()]
		//ExpectedMessage = "Submatrix indices"
		public void Negative_BadColumnIndexSet2()
		{
			int ib = 1, ie = 2; /* index ranges for sub GeneralMatrix */
			double[][] avals = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
			int[] badrowindexset = new int[]{1, 3};

			GeneralMatrix B = new GeneralMatrix(avals);

			Assert.That (()=>B.GetMatrix(ib, ie + B.RowDimension + 1, columnindexset), Throws.Exception);

		}

		[Test()]
		public void SubMatrixWithColumnIndex()
		{
			int ib = 1, ie = 2; /* index ranges for sub GeneralMatrix */
			double[][] avals = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
			double[][] subavals = {new double[]{5.0, 8.0, 11.0}, new double[]{6.0, 9.0, 12.0}};

			GeneralMatrix B = new GeneralMatrix(avals);
			M = B.GetMatrix(ib, ie, columnindexset);
			SUB = new GeneralMatrix(subavals);

			Assert.IsTrue(GeneralTests.Check(SUB, M));
		}

		[Test()]
		//ExpectedMessage = "Submatrix indices"
		public void Negative_BadRowIndexSet()
		{
			int jb = 1, je = 3; /* index ranges for sub GeneralMatrix */
			double[][] avals = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
			int[] badrowindexset = new int[]{1, 3};

			GeneralMatrix B = new GeneralMatrix(avals);
			Assert.That(()=>B.GetMatrix(badrowindexset, jb, je), Throws.Exception);

		}

		[Test()]
		//ExpectedMessage = "Submatrix indices"
		public void Negative_BadSubIndecesData()
		{

			int jb = 1, je = 3; /* index ranges for sub GeneralMatrix */
			double[][] avals = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
			int[] rowindexset = new int[]{1, 2};

			GeneralMatrix B = new GeneralMatrix(avals);

			Assert.That(()=>B.GetMatrix(rowindexset, jb, je + B.ColumnDimension + 1), Throws.Exception);
		}

		[Test()]
		public void SubMatrixWithRowIndexSet()
		{
			int jb = 1, je = 3; /* index ranges for sub GeneralMatrix */
			double[][] avals = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
			double[][] subavals = {new double[]{5.0, 8.0, 11.0}, new double[]{6.0, 9.0, 12.0}};
			int[] rowindexset = new int[]{1, 2};

			GeneralMatrix B = new GeneralMatrix(avals);
			M = B.GetMatrix(rowindexset, jb, je);
			SUB = new GeneralMatrix(subavals);

			Assert.IsTrue(GeneralTests.Check(SUB, M));
		}

		[Test()]
		//ExpectedMessage = "Submatrix indices"
		public void Negative_BadRowIndexSetGoodColumnIndexSet()
		{
			double[][] avals = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
			int[] badrowindexset = new int[]{1, 3};
			int[] columnindexset = new int[]{1, 2, 3};

			GeneralMatrix B = new GeneralMatrix(avals);
			Assert.That(()=>B.GetMatrix(badrowindexset, columnindexset), Throws.Exception);
		}

		[Test()]
		//ExpectedMessage = "Submatrix indices"
		public void Negative_BadColumnIndexSetGoodRowIndexSet()
		{
			double[][] avals = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
			int[] rowindexset = new int[]{1, 2};
			int[] badcolumnindexset = new int[]{1, 2, 4};

			GeneralMatrix B = new GeneralMatrix(avals);
			Assert.That(()=>B.GetMatrix(badrowindexset, columnindexset), Throws.Exception);
		}

		[Test()]
		public void SubMatrixWithRowIndexSetColumnIndexSet()
		{
			double[][] avals = {new double[]{1.0, 4.0, 7.0, 10.0}, new double[]{2.0, 5.0, 8.0, 11.0}, new double[]{3.0, 6.0, 9.0, 12.0}};
			double[][] subavals = {new double[]{5.0, 8.0, 11.0}, new double[]{6.0, 9.0, 12.0}};
			int[] rowindexset = new int[]{1, 2};
			int[] columnindexset = new int[]{1, 2, 3};

			GeneralMatrix B = new GeneralMatrix(avals);
			M = B.GetMatrix(rowindexset, columnindexset);
			SUB = new GeneralMatrix(subavals);

			Assert.IsTrue(GeneralTests.Check(SUB, M));
		}
	}
}
