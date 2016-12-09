//Krzysztof Desput
using System;
using System.Collections.Generic;
using System.Linq;
using libsvm;

namespace AI4
{
    public class SVMClassifier
    {
        private TrainingSet trainingSet; //training set
        private HashSet<string> vocabulary; //list of all features that exist in articles
        private List<string> x; //list of features of each article
        private List<double> y; //list of special coverages of each article
        private C_SVC model; //model used for predictions
        public SVMClassifier(TrainingSet trainingSet, TestingSet testingSet)
        {
            this.trainingSet = trainingSet;
            vocabulary = new HashSet<string>();
            x = new List<string>();
            y = new List<double>();

            foreach (Article article in trainingSet.articles.Values) //load data from the training set
            {
                string features = ArticleFeatures(article);
                AddFeaturesToVocabulary(features);
                //add features and special coverages to lists
                x.Add(features);
                y.Add(article.specialCoverage[0]);
            }

            foreach (Article article in testingSet.articles.Values) //load articles with given specialCoverage from the testing set
            {
                if(article.specialCoverage != null)
                {
                    string features = ArticleFeatures(article);
                    AddFeaturesToVocabulary(features);
                    //add features and special coverages to lists
                    x.Add(features);
                    y.Add(article.specialCoverage[0]);
                }
            }

            //create new problem
            ProblemBuilder problemBuilder = new ProblemBuilder();
            var problem = problemBuilder.CreateProblem(x, y.ToArray(), vocabulary.ToList());

            //create new model
            const int C = 1; //C parameter for C_SVC
            model = new C_SVC(problem, KernelHelper.LinearKernel(), C);
        }

        public int Classify(Article article)
        {
            var newX = ProblemBuilder.CreateNode(ArticleFeatures(article), vocabulary.ToList()); //create node
            var predictedY = model.Predict(newX); //get special coverage
            return (int)predictedY;
        }


        private string DateFeature(long versioncreated) //get the date feature from article's date of creation (e.g. "September1" for the first half of september)
        {
            DateTime datetime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            datetime = datetime.AddMilliseconds(versioncreated).ToLocalTime();
            string ret = "";
            string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            ret = months[datetime.Month - 1]; //get the article's month 
            if (datetime.Day <= 15) //get the half of the month 
                ret += "1";
            else ret += "2";
            return ret;
        }

        private string ArticleFeatures(Article article)
        {
            string features = ""; //article's features (bag-of-words)
            foreach (string keyword in article.keywords) //add keywords to article's features
            {
                features = features + "," + keyword;
            }
            foreach (string category in article.categories) //add categories to article's features
            {
                features = features + "," + category;
            }
            foreach (Place place in article.places) //add places to article's features
            {
                features = features + "," + place.country + "," + place.city;
            }
            features = features + "," + article.priority[0]; //add priority to article's features

            //add month and half of the month to article's features
            features = features + "," + DateFeature(article.versioncreated[0]);

            return features;
        }

        public void AddFeaturesToVocabulary(string features) //add features to vocabulary
        {
            string[] featuresArray = features.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach(string feature in featuresArray)
            {
                vocabulary.Add(feature);
            }
        }
    }
}
