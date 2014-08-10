using System;
using DotNetMatrix;

namespace Net.Kniaz.Optimization.QuasiNewton
{
	/// <summary>
	/// Summary description for QuasiNewtonOptimization.
	/// </summary>
	public class BFGS : Optimizer
	{
		public BFGS(int dim, double[]initialPar, IGradientFunction f,  double step, double epsilon, int itMax)
			:base(dim, initialPar, f,  step, epsilon, itMax)
		{}

		protected override GeneralMatrix CalculateNextHessianApproximation(GeneralMatrix pH, 
			double[]prevX, double[]curX, double[]prevGrad, double[]curGrad)
		{
			GeneralMatrix cH = new GeneralMatrix(_nDim,_nDim);
			GeneralMatrix cX = new GeneralMatrix(curX,_nDim);
			GeneralMatrix pX = new GeneralMatrix(prevX,_nDim);
			GeneralMatrix cG = new GeneralMatrix(curGrad,_nDim);
			GeneralMatrix pG = new GeneralMatrix(prevGrad,_nDim);

			GeneralMatrix sigma = cX.Subtract(pX);
			GeneralMatrix gamma = cG.Subtract(pG);
			
			double sigmaTGamma = sigma.Transpose().Multiply(gamma).GetElement(0,0);

			GeneralMatrix hGammaSigmaT = pH.Multiply(gamma.Multiply(sigma.Transpose()));
			GeneralMatrix sigmaGammaTH = sigma.Multiply(gamma.Transpose().Multiply(pH));
			double gammaTHGamma = (gamma.Transpose().Multiply(pH.Multiply(gamma))).GetElement(0,0);
			GeneralMatrix sigmaSigmaT  = sigma.Multiply(sigma.Transpose());

			GeneralMatrix term1 = (hGammaSigmaT.Add(sigmaGammaTH)).Multiply(1/sigmaTGamma);
			GeneralMatrix term2 = (sigmaSigmaT.Multiply(1/sigmaTGamma)).Multiply(1+gammaTHGamma/sigmaTGamma);

			return pH.Subtract(term1).Add(term2);
		}
	}
}
