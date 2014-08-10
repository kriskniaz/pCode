using System;
using DotNetMatrix;

namespace Net.Kniaz.AHP
{
	/// <summary>
	/// This class implements the Saaty's method for estimating
	/// eigenvalues of the priorities matrix.
	/// </summary>
	public class PrioritiesSelector
	{
		//Priority matrix of the size n by 1
		private GeneralMatrix _matrix=null;

		//consistency of the choices
		private double _consistency=0.0;

		//ratio of the consistency of the selection to the random index
		//should be less than 10% for the choice to be consistent
		//and useful
		private double _consistencyRatio=0.0;

		//estimate of the max eigen value of the prioriry matrix
		private double _lambdaMax=0.0;

		#region Accessors
		/// <summary>
		/// Final matrix of n by 1 containing results of the
		/// selection
		/// </summary>
		public GeneralMatrix CalculatedMatrix
		{
			get{return _matrix;}
		}

		/// <summary>
		/// Consistency of the selection
		/// </summary>
		public double Consistency
		{
			get{return _consistency;}
		}

		/// <summary>
		/// Consistency ratio of CI/RI
		/// </summary>
		public double ConsistencyRatio
		{
			get{return _consistencyRatio;}
		}

		/// <summary>
		/// Estimate of the max eigenvalue
		/// </summary>
		public double LambdaMax
		{
			get{return _lambdaMax;}
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Calculates priorities using the Saaty method
		/// </summary>
		/// <param name="argMatrix"> matrix of choices</param>
		/// <returns></returns>
		public void ComputePriorities(GeneralMatrix argMatrix)
		{
			double lMax=0.0;
			
			int n = argMatrix.ColumnDimension;
			int m = argMatrix.RowDimension;
			if (n!= m)
				throw new ArgumentException("Matrix must be symmetrical");

			_matrix = new GeneralMatrix(n,1);

			PCalc(argMatrix,_matrix);

			GeneralMatrix product = FCalc(argMatrix,_matrix);

			for (int i=0;i<n; i++)
				lMax+=product.GetElement(i,0)/n;

			_lambdaMax = lMax;


			_consistency = (_lambdaMax-n)/(n-1);
			_consistencyRatio = _consistency/Constants.randomIndices[n];
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Average values of the priority matrix over sum of columns.
		/// set values of sum of averaged rows into a new matrix
		/// </summary>
		/// <param name="argMatrix"></param>
		/// <param name="selection"></param>
		public void PCalc(GeneralMatrix argMatrix, GeneralMatrix selection)
		{
			int n = argMatrix.ColumnDimension;
			GeneralMatrix sMatrix = new GeneralMatrix(argMatrix.ArrayCopy);
			
			double c=0.0;
			int i,j;

			for (i=0;i<sMatrix.ColumnDimension; i++)
			{
				c=0.0;
				for (j=0; j<sMatrix.RowDimension; j++)
					c+=sMatrix.GetElement(j,i);
				selection.SetElement(i,0,c);
			}

			for (i=0;i<sMatrix.ColumnDimension; i++)
			{
				for (j=0; j<sMatrix.RowDimension; j++)
					sMatrix.SetElement(j,i,sMatrix.GetElement(j,i)/selection.GetElement(i,0));

			}

			for (i=0;i<sMatrix.RowDimension; i++)
			{
				c=0.0;
				for (j=0; j<sMatrix.ColumnDimension; j++)
					c+=sMatrix.GetElement(i,j);
				selection.SetElement(i,0,c/n);
			}
			
		}

		/// <summary>
		/// multiply normalized priority matrix by sum of average rows 
		/// </summary>
		/// <param name="argMatrix"></param>
		/// <param name="selection"></param>
		/// <returns></returns>
		private GeneralMatrix FCalc(GeneralMatrix argMatrix, GeneralMatrix selection)
		{
			GeneralMatrix matrix = argMatrix.Multiply(selection);
			return (matrix.ArrayRightDivide(selection));

		}

		#endregion

	}
}
