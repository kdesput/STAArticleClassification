//Krzysztof Desput
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace AI4
{
    public class TestingSet
    { 
        public Dictionary<int, Article> articles;
        private string path = "C:\\Data\\sta-special-articles-2015-testing.json";
        public TestingSet() //get testing set from .json file
        {
            string readText = File.ReadAllText(@path);
            JArray trainingSet = JArray.Parse(readText);
            List<JToken> results = trainingSet.Children().ToList();
            articles = new Dictionary<int, Article>();
            foreach (JToken result in results)
            {
                Article article = JsonConvert.DeserializeObject<Article>(result.ToString());
                articles.Add(article.id[0], article);
            }
        }
    }
}
