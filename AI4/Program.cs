//Krzysztof Desput
using System.Linq;

namespace AI4
{
    class Program
    {
        static void Main(string[] args)
        {
            TrainingSet trainingSet = new TrainingSet();
            TestingSet testingSet = new TestingSet();
            System.Console.WriteLine("Data loaded");
            System.Console.WriteLine("Training set: " + trainingSet.articles.Count);
            System.Console.WriteLine("Testing set: " + testingSet.articles.Count);

            SureClassifiers sureClassifiers = new SureClassifiers(trainingSet, testingSet);

            //looking for previous articles
            foreach (Article article in testingSet.articles.Values.OrderBy(key => key.id[0]))
            {
                if (article.specialCoverage == null) //check if article doesn't have specialCoverage
                {
                    int specialCoverage = sureClassifiers.PreviousArticle(article);
                    if (specialCoverage > 0) //check if specialCoverage was given by the method
                    {
                        article.specialCoverage = new int[1];
                        article.specialCoverage[0] = specialCoverage;
                    }
                }
            }

            //looking for next articles 
            foreach (Article article in testingSet.articles.Values.OrderByDescending(key => key.id[0]))
            {
                if (article.specialCoverage == null) //check if article doesn't have specialCoverage
                {
                    int specialCoverage = sureClassifiers.NextArticle(article);
                    if (specialCoverage > 0) //check if specialCoverage was given by the method
                    {
                        article.specialCoverage = new int[1];
                        article.specialCoverage[0] = specialCoverage;
                    }
                }
            }

            //looking for related articles
            foreach (Article article in testingSet.articles.Values.OrderBy(key => key.id[0]))
            {
                if (article.specialCoverage == null) //check if article doesn't have specialCoverage
                {
                    int specialCoverage = sureClassifiers.RelatedArticles(article);
                    if (specialCoverage > 0) //check if specialCoverage was given by the method
                    {
                        article.specialCoverage = new int[1];
                        article.specialCoverage[0] = specialCoverage;
                    }
                }
            }

            //finally getting special coverage from a SVM classifier
            SVMClassifier nnClassifier = new SVMClassifier(trainingSet, testingSet);

            foreach (Article article in testingSet.articles.Values)
            {
                if (article.specialCoverage == null) //check if specialCoverage was given by the method
                {
                    article.specialCoverage = new int[1];
                    article.specialCoverage[0] = nnClassifier.Classify(article);
                }
            }

            System.Console.WriteLine("Special coverages given");

            //write the results to .csv
            ResultsWriter resultsWriter = new ResultsWriter();
            resultsWriter.WriteResults(testingSet);
            System.Console.WriteLine("Results written");
            System.Console.ReadKey();
        }
    }
}
