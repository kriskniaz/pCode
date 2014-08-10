using System;
using DotNetMatrix;

namespace Net.Kniaz.AHP
{
	/// <summary>
	/// This class implements a decision model of the
	/// Analytic Hierarchy Process by Saaty.
	/// (T.L. Saaty "The Analytic Hierarchy Process", McGraw-Hill New York, 1980)
	/// The client of the class must provide
	/// decision making criteria that are scored against each other
	/// via a pairwise comparison (using the 1-9 scale proposed by Saaty) and
	/// the choices tht are scored for every criteria also using binary
	/// comparison process. 
	/// Client should use the AddCriteria method for the criteria and AddCriterionRatedChoices for
	/// choices scored agains the criteria. Order in which choices are added to the model
	/// must match the ordering of criteria in the matrix.
	/// It is assumed that all matrixes submittd to the model are symmetrical, inverse.
	/// for example: say we are trying to decide between Orlando, SF and NY as vacation
	/// places (see the unit test for a full example). We have 4 criteria to based
	/// our selection on: Activities, Nightlife, Siteseeing, Cost
	/// We start build our crteria matrix by comparing activities with remaining 3 criteria
	/// and using the scale from 1 (equally preferable to 9: extremely more preferable).
	/// Then we score nightlife against siteseeing and Cost and finally we compart Siteseeing ang cost.
	/// There is no reason to compare Cost to Activities again because it will be just a reverse
	/// from the comparison made in the first choice. 
	/// 
	/// As a result we get the following matrix:
	///				Activities	NightLife	SiteSeeing	Cost
	/// Activites		1			3			4		5
	/// Nigthlife					1			0.5		0.33333
	/// SiteSeeing								1		3
	/// Cost											1
	/// 
	/// The left lower triangle of the matrix should be left to 0
	/// because the class will transpose and take inverse of the 
	/// upper right corner.
	/// 
	/// After setting up criteria and choices the client should call the calculate
	/// model method.
	/// final result (choices scored against each criteria) are contained in
	/// the model result matrix. Each column of the result matrix represents 
	/// scored of choices against each criteria
	/// This assembly uses the GeneralMatrix package for the matrix algebra.
	/// </summary>
	public class AHPModel
	{
		//number of criteria
		private int _nCriteria;
		//number of choices
		private int _mChoices;
		//criteria times choices
		private int _superDim;
		
		//scored criteria
		private GeneralMatrix _criteria;
		
		//choices scored for all criteria such that
		//if there are n n criteria and m choices
		//the choice matrix will have a dimention of n by n*m
		private GeneralMatrix _choiceMatrix;
		
		//calculated criteria
		private GeneralMatrix _orderedCriteria;
		
		//calculated choices
		private GeneralMatrix _calculatedChoices;
		
		//final result of the model - matrix of m (choices) by 1
		//calculated as a product of criteria selection times
		//choice selection for each criteria = choices weighted 
		//by criteria
		private GeneralMatrix _modelResult;
		
		//consistency ratio matrix for each comparison. Each should be equal or
		//less to 10% for consistent model. First row represents
		//Consistency ratio for the criteria of the model
		private GeneralMatrix _lambdas;


		/// <summary>
		/// Parametrized Constructor
		/// </summary>
		/// <param name="n">number of selection criteria in the model</param>
		/// <param name="m">number of choices in the model</param>
		public AHPModel(int n, int m)
		{
			if ((n>20)||(m>20))
				throw new ArgumentException("models with over 20 criteria and /or choices are not supported");
			_superDim = n*m;
			_nCriteria = n;
			_mChoices = m;
			_criteria = new GeneralMatrix(n,n);
			_choiceMatrix = new GeneralMatrix(m,_superDim);
			_orderedCriteria = new GeneralMatrix(n,1);
			_modelResult = new GeneralMatrix(m,n);
			_calculatedChoices = new GeneralMatrix(m,1);
			_lambdas = new GeneralMatrix(n+1,1);
		}

		#region Accessors

		/// <summary>
		/// Criteria priorities as calculated by the model.
		/// A matrix of n by 1
		/// </summary>
		public GeneralMatrix CalculatedCriteria
		{
			get{return _orderedCriteria;}

		}

		/// <summary>
		/// Raw criteria scored after the pairwise comparison
		/// A matrix of n by n
		/// </summary>
		public GeneralMatrix RatedCriteria
		{
			get{return _criteria;}
			set
			{
				_criteria = ExpandUtility(value);
				_criteria=value;
			}
		}

		/// <summary>
		/// Model choices scored in pairwise comparison for each criteria
		/// A matrix of n by n*m
		/// </summary>
		public GeneralMatrix ChoiceMatrix
		{
			get{return _choiceMatrix;}
		}

		/// <summary>
		/// Result
		/// </summary>
		public GeneralMatrix ModelResult
		{
			get{return _modelResult;}
		}

		/// <summary>
		/// 
		/// </summary>
		public GeneralMatrix CalculatedChoices
		{
			get{return _calculatedChoices;}
		}

		/// <summary>
		/// Matrix of consistency ratios for the model
		/// size of n+1 where n is number of criteria
		/// First row represents consistency ratio for the 
		/// 
		/// </summary>
		public GeneralMatrix ConsistencyRatio
		{
			get{return _lambdas;}
		}

		#endregion

		#region Public Functions
	
		public void AddCriteria(GeneralMatrix matrix)
		{
			_criteria = ExpandUtility(matrix);
		}

		public void AddCriteria(double[][] matrix)
		{
			GeneralMatrix newMatrix = new GeneralMatrix(matrix);
			AddCriteria(newMatrix);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="matrix"></param>
		/// <param name="criterionId"></param>
		public void AddCriterionRatedChoices(int criterionId, GeneralMatrix matrix)
		{
			if (criterionId>_nCriteria)
				throw new ArgumentException("Passed criterion Id greater than numberof criteria");


			int col0 = criterionId*_mChoices;
			int colMax = col0+_mChoices-1;
			GeneralMatrix newMatrix = (GeneralMatrix)ExpandUtility(matrix).Clone();
			_choiceMatrix.SetMatrix(0,_mChoices-1,col0,colMax,newMatrix);
		}

		public void AddCriterionRatedChoices(int criterionId, double[][] matrix)
		{
			GeneralMatrix gMatrix = new GeneralMatrix(matrix);
			AddCriterionRatedChoices(criterionId,gMatrix);
		}

		/// <summary>
		/// (
		/// </summary>
		/// <param name="matrix"></param>
		/// <returns></returns>
		public static GeneralMatrix ExpandUtility(GeneralMatrix matrix)
		{
			double val=0.0;
			int n = matrix.RowDimension;
			int m = matrix.ColumnDimension;

			if (n!=m) 
				throw new ArgumentException("Criteria matrix must be symmetrical");

			GeneralMatrix newMatrix = matrix.Transpose();

			//for all transposed elements calculate their inverse values
			//set diagonal elements to 0
			for (int i=0; i<n; i++)
				for (int j=0; j<=i; j++)
				{
					val = newMatrix.GetElement(i,j);
					if (val==0.0)
						throw new ArgumentException("Criteria comparison values van't be 0");
						newMatrix.SetElement(i,j,1/val);
					if (i==j)
						newMatrix.SetElement(i,j,0);
				}
			//add transposed, inverse matrix to the original one
			//create fully expanded matrix 
			return newMatrix.Add(matrix);
		}

		/// <summary>
		/// 
		/// </summary>
		public void CalculateModel()
		{
			CalculatePriorities();
			CalculateChoices();
			CalculateFinalResult();

		}

		#endregion

		#region Private functions
		/// <summary>
		/// 
		/// </summary>
		private void CalculatePriorities()
		{
			PrioritiesSelector selector = new PrioritiesSelector();
			selector.ComputePriorities(_criteria);
			
			//first (zero-th) element of the lambda matrix is the
			//consistency ratio factor for the selection matrix
			_lambdas.SetElement(0,0,selector.ConsistencyRatio);

			_orderedCriteria = selector.CalculatedMatrix;
		}

		/// <summary>
		/// 
		/// </summary>
		private void CalculateChoices()
		{
			GeneralMatrix tempMatrix=null;
			int i, col0, colMax;

			PrioritiesSelector selector = new PrioritiesSelector();
			
			for (i=0; i<this._nCriteria; i++)
			{
				col0 = i*_mChoices;
				colMax = col0+_mChoices-1;
				tempMatrix = _choiceMatrix.GetMatrix(0,_mChoices-1,col0,colMax);
				selector.ComputePriorities(tempMatrix);
				//first element of the matrix is consistency ratio
				//for the criteria
				_lambdas.SetElement(i+1,0,selector.ConsistencyRatio);

				_modelResult.SetMatrix(0,_mChoices-1,i,i,selector.CalculatedMatrix);

			}

		}

		/// <summary>
		/// Calculates final model results as a sum of product of choices rated
		/// for each criteria times weighing (preference) of each criteria
		/// </summary>
		private void CalculateFinalResult()
		{
			double sum;
			for (int i=0; i<_mChoices; i++)
			{
				sum=0;
				for (int j=0; j<_nCriteria; j++)
				{
					sum+=_modelResult.GetElement(i,j)*this._orderedCriteria.GetElement(j,0);
				}
				_calculatedChoices.SetElement(i,0,sum);
			}

		}

		#endregion

	}
	
}
