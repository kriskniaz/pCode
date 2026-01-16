using System;
using System.Collections.Generic;
using System.Linq;

namespace Net.Kniaz.AHP
{
    /// <summary>
    /// Represents a complete AHP (Analytical Hierarchy Process) decision model
    /// </summary>
    public class AHPObjectModel
    {
        public string Goal { get; set; }
        public List<Criterion> Criteria { get; set; }
        public List<Alternative> Alternatives { get; set; }

        // Pairwise comparison matrix for criteria (criteria vs criteria)
        public PairwiseComparisonMatrix CriteriaComparisons { get; set; }

        // Dictionary to store pairwise comparisons of alternatives with respect to each criterion
        // Key: Criterion ID, Value: Comparison matrix for alternatives under that criterion
        public Dictionary<string, PairwiseComparisonMatrix> AlternativeComparisons { get; set; }

        public AHPObjectModel(string goal)
        {
            Goal = goal;
            Criteria = new List<Criterion>();
            Alternatives = new List<Alternative>();
            AlternativeComparisons = new Dictionary<string, PairwiseComparisonMatrix>();
        }

        /// <summary>
        /// Initializes the criteria comparison matrix once all criteria are added
        /// </summary>
        public void InitializeCriteriaComparisons()
        {
            var criteriaIds = Criteria.Select(c => c.Id).ToList();
            CriteriaComparisons = new PairwiseComparisonMatrix(criteriaIds);
        }

        /// <summary>
        /// Initializes the alternative comparison matrix for a specific criterion
        /// </summary>
        public void InitializeAlternativeComparisons(string criterionId)
        {
            if (!Criteria.Any(c => c.Id == criterionId))
                throw new ArgumentException($"Criterion with ID {criterionId} not found");

            var alternativeIds = Alternatives.Select(a => a.Id).ToList();
            AlternativeComparisons[criterionId] = new PairwiseComparisonMatrix(alternativeIds);
        }

        /// <summary>
        /// Initializes alternative comparisons for all criteria
        /// </summary>
        public void InitializeAllAlternativeComparisons()
        {
            foreach (var criterion in Criteria)
            {
                InitializeAlternativeComparisons(criterion.Id);
            }
        }
    }

    /// <summary>
    /// Represents a decision criterion
    /// </summary>
    public class Criterion
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; } // Calculated from pairwise comparisons

        public Criterion(string id, string name, string description = "")
        {
            Id = id;
            Name = name;
            Description = description;
            Weight = 0.0;
        }
    }

    /// <summary>
    /// Represents a decision alternative
    /// </summary>
    public class Alternative
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double FinalScore { get; set; } // Calculated final priority score

        // Stores the priority scores with respect to each criterion
        public Dictionary<string, double> CriterionScores { get; set; }

        public Alternative(string id, string name, string description = "")
        {
            Id = id;
            Name = name;
            Description = description;
            FinalScore = 0.0;
            CriterionScores = new Dictionary<string, double>();
        }
    }

 
}