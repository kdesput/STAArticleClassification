//Krzysztof Desput
using System;
using System.Collections.Generic;
using System.Linq;
using libsvm;

namespace AI4
{
    public class ProblemBuilder
    {
        public svm_problem CreateProblem(List<string> x, double[] y, List<string> vocabulary) //create new svm problem
        {
            return new svm_problem
            {
                y = y,
                x = x.Select(xVector => CreateNode(xVector, vocabulary)).ToArray(),
                l = y.Length
            };
        }

        public static svm_node[] CreateNode(string x, List<string> vocabulary) //create new svm node
        {
            var node = new List<svm_node>(vocabulary.Count);

            string[] words = x.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries); //get features from x
            words = words.ToArray();
            for (int i = 0; i < vocabulary.Count; i++)
            {
                int occurence = words.Count(s => String.Equals(s, vocabulary[i], StringComparison.Ordinal)); //how many times does the word from vocabulary occur in x (features)
                if (occurence != 0) //if there was at least one common word 
                {
                    node.Add(new svm_node
                    {
                        index = i + 1,
                        value = occurence
                    });
                }
            }
            return node.ToArray();
        }
    }
}
