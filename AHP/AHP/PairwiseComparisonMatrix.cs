using DotNetMatrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Kniaz.AHP
{
    /// <summary>
    /// Represents a pairwise comparison matrix
    /// </summary>
    public class PairwiseComparisonMatrix
    {
        private Dictionary<string, int> _indexMap;
        private GeneralMatrix _matrix;
        private List<string> _elementIds;

        public List<string> ElementIds
        {
            get
            {
                return this._elementIds;
            }
        }

        public int Size
        {
            get
            {
                return this._elementIds.Count;
            }
        }

        public PairwiseComparisonMatrix(List<string> elementIds)
        {
            _elementIds = new List<string>(elementIds);
            _indexMap = new Dictionary<string, int>();

            for (int i = 0; i < elementIds.Count; i++)
            {
                _indexMap[elementIds[i]] = i;
            }

            _matrix = new GeneralMatrix(elementIds.Count, elementIds.Count);

            // Initialize diagonal to 1 (element compared to itself)
            for (int i = 0; i < elementIds.Count; i++)
            {
                _matrix.SetElement(i, i,1.0);
            }
        }

        /// <summary>
        /// Sets a pairwise comparison value
        /// </summary>
        /// <param name="elementId1">First element ID</param>
        /// <param name="elementId2">Second element ID</param>
        /// <param name="value">Comparison value (1-9 scale)</param>
        public void SetComparison(string elementId1, string elementId2, double value)
        {
            if (!_indexMap.ContainsKey(elementId1))
                throw new ArgumentException($"Element {elementId1} not found");
            if (!_indexMap.ContainsKey(elementId2))
                throw new ArgumentException($"Element {elementId2} not found");
            if (value < 1.0 / 9.0 || value > 9.0)
                throw new ArgumentException("Comparison value must be between 1/9 and 9");

            int i = _indexMap[elementId1];
            int j = _indexMap[elementId2];

            _matrix.SetElement(i, j,value);
            _matrix.SetElement(j, i,1.0 / value); // Set reciprocal
        }

        /// <summary>
        /// Gets a comparison value
        /// </summary>
        public double GetComparison(string elementId1, string elementId2)
        {
            if (!_indexMap.ContainsKey(elementId1) || !_indexMap.ContainsKey(elementId2))
                throw new ArgumentException("Element not found");

            int i = _indexMap[elementId1];
            int j = _indexMap[elementId2];
            return _matrix.GetElement(i, j);
        }

        /// <summary>
        /// Gets the raw matrix
        /// </summary>
        public double[,] GetMatrix()
        {
            return (double[,])_matrix.Clone();
        }

        /// <summary>
        /// Gets the GeneralMatrix object
        /// </summary>
        /// <returns></returns>
        public GeneralMatrix GetGeneralMatrix()
        {
            return _matrix;
        }

        /// <summary>
        /// Calculates priority vector using the normalized column average method
        /// </summary>
        public Dictionary<string, double> CalculatePriorityVector()
        {
            var priorities = new Dictionary<string, double>();
            int n = _elementIds.Count;

            // Normalize each column
            double[,] normalized = new double[n, n];
            for (int j = 0; j < n; j++)
            {
                double columnSum = 0;
                for (int i = 0; i < n; i++)
                {
                    columnSum += _matrix.GetElement(i, j);
                }

                for (int i = 0; i < n; i++)
                {
                    normalized[i, j] = _matrix.GetElement(i, j) / columnSum;
                }
            }

            // Calculate row averages
            for (int i = 0; i < n; i++)
            {
                double rowSum = 0;
                for (int j = 0; j < n; j++)
                {
                    rowSum += normalized[i, j];
                }
                priorities[_elementIds[i]] = rowSum / n;
            }

            return priorities;
        }

        /// <summary>
        /// Calculates the consistency ratio to check for inconsistencies
        /// </summary>
        public double CalculateConsistencyRatio()
        {
            int n = _elementIds.Count;
            if (n <= 2) return 0; // Consistency ratio not applicable for 2x2 or smaller

            // Random Consistency Index values
            double[] RI = { 0, 0, 0.58, 0.90, 1.12, 1.24, 1.32, 1.41, 1.45, 1.49 };
            if (n > RI.Length)
                throw new InvalidOperationException("Matrix too large for consistency check");

            var priorities = CalculatePriorityVector();

            // Calculate lambda max (principal eigenvalue approximation)
            double lambdaMax = 0;
            for (int j = 0; j < n; j++)
            {
                double columnSum = 0;
                for (int i = 0; i < n; i++)
                {
                    columnSum += _matrix.GetElement(i, j) * priorities[_elementIds[i]];
                }
                lambdaMax += columnSum / priorities[_elementIds[j]];
            }
            lambdaMax /= n;

            // Calculate Consistency Index
            double CI = (lambdaMax - n) / (n - 1);

            // Calculate Consistency Ratio
            double CR = CI / RI[n - 1];

            return CR;
        }
    }
}
