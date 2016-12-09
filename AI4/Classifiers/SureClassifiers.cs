//Krzysztof Desput
namespace AI4
{
    class SureClassifiers //classifiers that are 100% sure
    {
        private TrainingSet trainingSet;
        private TestingSet testingSet;
        public SureClassifiers(TrainingSet trainingSet, TestingSet testingSet)
        {
            this.trainingSet = trainingSet;
            this.testingSet = testingSet;
        }

        public int PreviousArticle(Article article) //get special coverage from the previous article
        {
            int specialCoverage = 0; //if specialCoverage == 0 - no previous articles found
            if (article.previous != null) //check if article has got previous article in chain
            {
                int previous = article.previous[0]; //get id of previous article
                if (trainingSet.articles.ContainsKey(previous) && trainingSet.articles[previous].specialCoverage != null) //look for the previous article in trainingSet
                {
                    specialCoverage = trainingSet.articles[previous].specialCoverage[0]; //get specialCoverage from the previous article
                }
                else if (testingSet.articles.ContainsKey(previous) && testingSet.articles[previous].specialCoverage != null) //look for the previous article in testingSet
                {
                    specialCoverage = testingSet.articles[previous].specialCoverage[0]; //get specialCoverage from the previous article
                }
            }
            return specialCoverage;
        }

        public int NextArticle(Article article) //get special coverage from the next article
        {
            int specialCoverage = 0; //if specialCoverage == 0 - no next articles found
            if (article.next != null) //check if article has got next article in chain
            {
                int next = article.next[0];
                if (trainingSet.articles.ContainsKey(next) && trainingSet.articles[next].specialCoverage != null) //look for the next article in testingSet
                {
                    specialCoverage = trainingSet.articles[next].specialCoverage[0]; //get specialCoverage from the next article
                }
                else if (testingSet.articles.ContainsKey(next) && testingSet.articles[next].specialCoverage != null) //look for the next article in testingSet
                {
                    specialCoverage = testingSet.articles[next].specialCoverage[0]; //get specialCoverage from the next article
                }
            }
            return specialCoverage;
        }

        public int RelatedArticles(Article article) //get special coverage from related articles
        {
            int specialCoverage = 0; //if specialCoverage == 0 - no related articles found, if -1 then related articles have different special coverages
            if (article.related != null)
            {
                foreach (int related in article.related)
                {
                    if (trainingSet.articles.ContainsKey(related) && trainingSet.articles[related].specialCoverage != null) //look for the related article in testingSet
                    {
                        if (specialCoverage == 0 || specialCoverage == trainingSet.articles[related].specialCoverage[0]) //check if it is the only special coverage
                            specialCoverage = trainingSet.articles[related].specialCoverage[0];
                        else specialCoverage = -1;
                    }
                    else if (testingSet.articles.ContainsKey(related) && testingSet.articles[related].specialCoverage != null) //look for the related article in testingSet
                    {
                        if (specialCoverage == 0 || specialCoverage == testingSet.articles[related].specialCoverage[0]) //check if it is the only special coverage
                            specialCoverage = testingSet.articles[related].specialCoverage[0];
                        else specialCoverage = -1;
                    }
                }
            }
            return specialCoverage;
        }
    }
}
