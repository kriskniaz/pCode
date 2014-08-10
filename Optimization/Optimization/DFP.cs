using System;
using DotNetMatrix;

namespace Net.Kniaz.Optimization.QuasiNewton
{
	/// <summary>
	/// Summary description for DFP.
	/// </summary>
	public class DFP : Optimizer
	{

		public DFP(int dim, double[]initialPar, IGradientFunction f, double step, double epsilon, int itMax)
			:base(dim, initialPar, f,  step, epsilon, itMax)
		{
		}



		protected override GeneralMatrix CalculateNextHessianApproximation(GeneralMatrix previousH, 
			double[]prevX, double[]curX, double[]prevGrad, double[]curGrad)
		{
			GeneralMatrix currentH = new GeneralMatrix(_nDim,_nDim);
			GeneralMatrix cX = new GeneralMatrix(curX,_nDim);
			GeneralMatrix pX = new GeneralMatrix(prevX,_nDim);
			GeneralMatrix cG = new GeneralMatrix(curGrad,_nDim);
			GeneralMatrix pG = new GeneralMatrix(prevGrad,_nDim);

			GeneralMatrix dX = cX.Subtract(pX);
			GeneralMatrix dG = cG.Subtract(pG);
			
			double aK1 = 1/(dX.Transpose().Multiply(dG).GetElement(0,0));
			GeneralMatrix aK2 = dX.Multiply(dX.Transpose());
			
			GeneralMatrix aK = aK2.Multiply(aK1);

			double bK1 = -1/(dG.Transpose().Multiply(previousH).Multiply(dG).GetElement(0,0));
			GeneralMatrix bK2 = previousH.Multiply(dG).Multiply(dG.Transpose()).Multiply(previousH.Transpose());
			
			GeneralMatrix bK =bK2.Multiply(bK1);

			currentH = previousH.Add(aK).Add(bK);
			
			return currentH;
		}


	}
}
