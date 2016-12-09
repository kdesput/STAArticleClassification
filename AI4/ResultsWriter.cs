//Krzysztof Desput
using System.Text;
using System.IO;

namespace AI4
{
    class ResultsWriter
    {
        private string path = "C:\\Results\\sta-special-articles-2015-submission.csv";
        public void WriteResults(TestingSet testingSet) //write the results to .csv file
        {
            StringBuilder csv = new StringBuilder(); //String builder for csv file with results
            csv.AppendLine("id;specialCoverage"); //header line
            foreach (Article article in testingSet.articles.Values)
            {
                csv.AppendLine(string.Format("{0};{1}", article.id[0], article.specialCoverage[0]));
            }
            File.WriteAllText(@path, csv.ToString()); 
        }
    }
}
