using DotNetMatrix;
using Net.Kniaz.AHP;
using NUnit.Framework;
using System;
using System.Reflection;

namespace AHP.Tests
{
    [TestFixture()]
    public class TestAHPModel
    {
        [Test()]
        public void TestAHPModelForPhoneSelection()
        {
            var ahpModelData = new AHPObjectModel("Select the best smartphone");
            // Add criteria
            ahpModelData.Criteria.Add(new Criterion("C1", "Price"));
            ahpModelData.Criteria.Add(new Criterion("C2", "Camera Quality"));
            ahpModelData.Criteria.Add(new Criterion("C3", "Battery Life"));
            // Add alternatives
            ahpModelData.Alternatives.Add(new Alternative("A1", "Phone A"));
            ahpModelData.Alternatives.Add(new Alternative("A2", "Phone B"));
            ahpModelData.Alternatives.Add(new Alternative("A3", "Phone C"));
            // Initialize comparison matrices
            ahpModelData.InitializeCriteriaComparisons();
            ahpModelData.InitializeAllAlternativeComparisons();
            // Verify that the model is set up correctly

            ahpModelData.CriteriaComparisons.SetComparison("C1", "C2", 3); // Price is moderately more important than Camera Quality
            ahpModelData.CriteriaComparisons.SetComparison("C1", "C3", 5); // Price is strongly more important than Battery Life
            ahpModelData.CriteriaComparisons.SetComparison("C2", "C3", 3); // Camera Quality is slightly more important than Battery Life

            ahpModelData.AlternativeComparisons["C1"].SetComparison("A1", "A2", 5); // For Price, A1 is strongly preferred over A2
            ahpModelData.AlternativeComparisons["C1"].SetComparison("A1", "A3", 7); // For Price, A1 is very strongly preferred over A3
            ahpModelData.AlternativeComparisons["C1"].SetComparison("A2", "A3", 3); // For Price, A2 is moderately preferred over A3

            ahpModelData.AlternativeComparisons["C2"].SetComparison("A1", "A2", 1/3.0); // For Camera Quality, A2 is moderately preferred over A1
            ahpModelData.AlternativeComparisons["C2"].SetComparison("A1", "A3", 5); // For Camera Quality, A1 is strongly preferred over A3
            ahpModelData.AlternativeComparisons["C2"].SetComparison("A2", "A3", 5); // For Camera Quality, A2 is strongly preferred over A3
            
            ahpModelData.AlternativeComparisons["C3"].SetComparison("A1", "A2", 1/5.0); // For Battery Life, A2 is strongly preferred over A1
            ahpModelData.AlternativeComparisons["C3"].SetComparison("A1", "A3", 1/7.0); // For Battery Life, A3 is very strongly preferred over A1
            ahpModelData.AlternativeComparisons["C3"].SetComparison("A2", "A3", 3); // For Battery Life, A2 is moderately preferred over A3

            AHPModel aHPModel = new AHPModel(3,3); //3 criteria, 3 choices
            aHPModel.AddCriteria(ahpModelData.CriteriaComparisons.GetGeneralMatrix());
            aHPModel.AddCriterionRatedChoices(0, ahpModelData.AlternativeComparisons["C1"].GetGeneralMatrix());
            aHPModel.AddCriterionRatedChoices(1, ahpModelData.AlternativeComparisons["C2"].GetGeneralMatrix());
            aHPModel.AddCriterionRatedChoices(2, ahpModelData.AlternativeComparisons["C3"].GetGeneralMatrix());
            aHPModel.CalculateModel();

            GeneralMatrix choices = aHPModel.CalculatedChoices;
            //choices: SF 42%, Orlando31%, NY 27%
            Assert.That(System.Math.Round(choices.GetElement(0, 0) * 100, 0), Is.EqualTo(31));
            Assert.That(System.Math.Round(choices.GetElement(1, 0) * 100, 0), Is.EqualTo(42));
            Assert.That(System.Math.Round(choices.GetElement(2, 0) * 100, 0), Is.EqualTo(27));

        }

        [Test()]
        public void TestAHPModelforHolidaySpotSelection()
        {
            VacationSelectionData data = new VacationSelectionData();

            GeneralMatrix choices = data.VacationSpotSelectionUsingModel();

            //choices: SF 42%, Orlando31%, NY 27%
            Assert.That(System.Math.Round(choices.GetElement(0, 0) * 100, 0), Is.EqualTo(31));
            Assert.That(System.Math.Round(choices.GetElement(1, 0) * 100, 0), Is.EqualTo(42));
            Assert.That(System.Math.Round(choices.GetElement(2, 0) * 100, 0), Is.EqualTo(27));

        }

        [Test()]
        public void TestCompareSpotSelections()
        {
            VacationSelectionData data = new VacationSelectionData();
            GeneralMatrix modelChoice = data.VacationSpotSelectionUsingModel();
            GeneralMatrix arrayChoice = data.VacationSpotSelectionUsingArrays();

            var modelChoice1 = modelChoice.GetElement(0, 0);
            var modelChoice2 = modelChoice.GetElement(1, 0);
            var modelChoice3 = modelChoice.GetElement(2,0 );

            var arrayChoice1 = arrayChoice.GetElement(0, 0);
            var arrayChoice2 = arrayChoice.GetElement(1, 0);
            var arrayChoice3 = arrayChoice.GetElement(2, 0);

            Assert.That(System.Math.Round(modelChoice1 * 100, 0), Is.EqualTo(System.Math.Round(arrayChoice1 * 100, 0)));
        }

        [Test()]
        public void TestCarSelection()
        {
            var ahpModelData = new AHPObjectModel("Select the best car");

            //criteria
            ahpModelData.Criteria.Add(new Criterion("C1", "Price"));
            ahpModelData.Criteria.Add(new Criterion("C2", "Looks"));
            ahpModelData.Criteria.Add(new Criterion("C3", "Performance"));
            
            //alternatices
            ahpModelData.Alternatives.Add(new Alternative("A1", "Alfa Romeo"));
            ahpModelData.Alternatives.Add(new Alternative("A2", "Volvo"));
            ahpModelData.Alternatives.Add(new Alternative("A3", "Toyota"));

            // Initialize comparison matrices
            ahpModelData.InitializeCriteriaComparisons();
            ahpModelData.InitializeAllAlternativeComparisons();

            //criteria comparisons
            ahpModelData.CriteriaComparisons.SetComparison("C1", "C2", 3);
            ahpModelData.CriteriaComparisons.SetComparison("C1", "C3", 5.0);
            ahpModelData.CriteriaComparisons.SetComparison("C2", "C3", 1);

            //alternatives - price
            ahpModelData.AlternativeComparisons["C1"].SetComparison("A1", "A2", 3.0);
            ahpModelData.AlternativeComparisons["C1"].SetComparison("A1", "A3", 1.0/5.0);
            ahpModelData.AlternativeComparisons["C1"].SetComparison("A2", "A3", 1.0/7.0);

            //alternatives - looks
            ahpModelData.AlternativeComparisons["C2"].SetComparison("A1", "A2", 3.0);
            ahpModelData.AlternativeComparisons["C2"].SetComparison("A1", "A3", 9.0);
            ahpModelData.AlternativeComparisons["C2"].SetComparison("A2", "A3", 3.0);

            //alternatives - performance
            ahpModelData.AlternativeComparisons["C3"].SetComparison("A1", "A2", 3.0);
            ahpModelData.AlternativeComparisons["C3"].SetComparison("A1", "A3", 7.0);
            ahpModelData.AlternativeComparisons["C3"].SetComparison("A2", "A3", 3.0);

            AHPModel aHPModel = new AHPModel(3, 3); //3 criteria, 3 choices
            aHPModel.AddCriteria(ahpModelData.CriteriaComparisons.GetGeneralMatrix());
            aHPModel.AddCriterionRatedChoices(0, ahpModelData.AlternativeComparisons["C1"].GetGeneralMatrix());
            aHPModel.AddCriterionRatedChoices(1, ahpModelData.AlternativeComparisons["C2"].GetGeneralMatrix());
            aHPModel.AddCriterionRatedChoices(2, ahpModelData.AlternativeComparisons["C3"].GetGeneralMatrix());

            aHPModel.CalculateModel();

            GeneralMatrix choices = aHPModel.CalculatedChoices;

            Assert.That(System.Math.Round(choices.GetElement(0, 0) * 100, 0), Is.EqualTo(50));
            Assert.That(System.Math.Round(choices.GetElement(1, 0) * 100, 0), Is.EqualTo(36));
            Assert.That(System.Math.Round(choices.GetElement(2, 0) * 100, 0), Is.EqualTo(14));

            string t;

        }

    }


}
