using System;

namespace Net.Kniaz.AHP
{
	/// <summary>
	/// Random Indices values (Consistency Indices for randomly selected values
	/// in the priority matrix) taken from Saaty.
	/// </summary>
	public class Constants
	{
		//values are for the following dimensions of the requirements matrix
		//0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15
		public static double[]randomIndices = {0.00, 0.00, 0.00, 0.58, 0.90, 1.12,
										1.24, 1.32, 1.41, 1.45,	1.49, 
										1.51, 1.48, 1.56, 1.57, 1.59, 1.605, 1.61, 1.615, 1.62, 1.625};
	}
}
