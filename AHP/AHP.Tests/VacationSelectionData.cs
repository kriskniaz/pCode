using DotNetMatrix;
using Net.Kniaz.AHP;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHP.Tests
{
    internal class VacationSelectionData
    {
        public VacationSelectionData() { }

        public GeneralMatrix VacationSpotSelectionUsingArrays()
        {
            double[][] criteria = new double[][]
                                {
                                    new double[] {1,5,0.33333333,1},
                                    new double[] {0,1,0.2,0.5},
                                    new double[] {0,0,1,3},
                                    new double[] {0,0,0,1}
                                };

            double[][] activitiesChoices = new double[][]
                {
                    new double[] {1,4,3},
                    new double[] {0,1,2},
                    new double[] {0,0,1}
                };

            double[][] nightlifeChoices = new double[][]
                {
                    new double[] {1,0.5,0.3333333},
                    new double[] {0,1,0.5},
                    new double[] {0,0,1}
                };

            double[][] siteseeingChoices = new double[][]
                {
                    new double[] {1,0.142857,0.2},
                    new double[] {0,1,2},
                    new double[] {0,0,1}
                };

            double[][] costChoices = new double[][]
                {
                    new double[] {1,3,5},
                    new double[] {0,1,2},
                    new double[] {0,0,1}
                };

            //4 criteria, 3 choices
            AHPModel model = new AHPModel(4, 3);
            model.AddCriteria(criteria);
            model.AddCriterionRatedChoices(0, activitiesChoices);
            model.AddCriterionRatedChoices(1, nightlifeChoices);
            model.AddCriterionRatedChoices(2, siteseeingChoices);
            model.AddCriterionRatedChoices(3, costChoices);

            model.CalculateModel();

            GeneralMatrix calcCriteria = model.CalculatedCriteria;
            GeneralMatrix results = model.ModelResult;
            return model.CalculatedChoices;
        }

        public GeneralMatrix VacationSpotSelectionUsingModel()
        {
            /// Criteria: Activities, Nightlife, Siteseeing, Cost
            /// Choices: Orlando, San Fran, New York;
            var ahpModelData = new AHPObjectModel("Select the best vacation spot");

            // Add criteria
            ahpModelData.Criteria.Add(new Criterion("V1", "Activities"));
            ahpModelData.Criteria.Add(new Criterion("V2", "Nighlife"));
            ahpModelData.Criteria.Add(new Criterion("V3", "Siteseeing"));
            ahpModelData.Criteria.Add(new Criterion("V4", "Cost"));

            // Add alternatives
            ahpModelData.Alternatives.Add(new Alternative("A1", "Orlando"));
            ahpModelData.Alternatives.Add(new Alternative("A2", "San Fran"));
            ahpModelData.Alternatives.Add(new Alternative("A3", "New York"));
            // Initialize comparison matrices
            ahpModelData.InitializeCriteriaComparisons();
            ahpModelData.InitializeAllAlternativeComparisons();

            ahpModelData.CriteriaComparisons.SetComparison("V1", "V2", 5);
            ahpModelData.CriteriaComparisons.SetComparison("V1", "V3", 1.0 / 3.0);
            ahpModelData.CriteriaComparisons.SetComparison("V1", "V4", 1);

            ahpModelData.CriteriaComparisons.SetComparison("V2", "V3", 0.2);
            ahpModelData.CriteriaComparisons.SetComparison("V2", "V4", 0.5);

            ahpModelData.CriteriaComparisons.SetComparison("V3", "V4", 3);

            ahpModelData.AlternativeComparisons["V1"].SetComparison("A1", "A2", 4);
            ahpModelData.AlternativeComparisons["V1"].SetComparison("A1", "A3", 3);
            ahpModelData.AlternativeComparisons["V1"].SetComparison("A2", "A3", 2);

            ahpModelData.AlternativeComparisons["V2"].SetComparison("A1", "A2", 0.5);
            ahpModelData.AlternativeComparisons["V2"].SetComparison("A1", "A3", 1.0 / 3.0);
            ahpModelData.AlternativeComparisons["V2"].SetComparison("A2", "A3", 0.5);

            ahpModelData.AlternativeComparisons["V3"].SetComparison("A1", "A2", 1.0 / 7.0);
            ahpModelData.AlternativeComparisons["V3"].SetComparison("A1", "A3", 0.2);
            ahpModelData.AlternativeComparisons["V3"].SetComparison("A2", "A3", 2);

            ahpModelData.AlternativeComparisons["V4"].SetComparison("A1", "A2", 3);
            ahpModelData.AlternativeComparisons["V4"].SetComparison("A1", "A3", 5);
            ahpModelData.AlternativeComparisons["V4"].SetComparison("A2", "A3", 2);

            AHPModel aHPModel = new AHPModel(4, 3); //3 criteria, 3 choices
            aHPModel.AddCriteria(ahpModelData.CriteriaComparisons.GetGeneralMatrix());
            aHPModel.AddCriterionRatedChoices(0, ahpModelData.AlternativeComparisons["V1"].GetGeneralMatrix());
            aHPModel.AddCriterionRatedChoices(1, ahpModelData.AlternativeComparisons["V2"].GetGeneralMatrix());
            aHPModel.AddCriterionRatedChoices(2, ahpModelData.AlternativeComparisons["V3"].GetGeneralMatrix());
            aHPModel.AddCriterionRatedChoices(3, ahpModelData.AlternativeComparisons["V4"].GetGeneralMatrix());

            aHPModel.CalculateModel();

            GeneralMatrix choices = aHPModel.CalculatedChoices;

            return choices;
        }
    }
}
