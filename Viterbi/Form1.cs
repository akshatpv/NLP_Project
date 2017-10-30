using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Viterbi
{
    public partial class Form1 : Form
    {
        const int num1 = 4, num2 = 5;
        //public static string[] tags = { "VB", "TO", "NN", "PPSS" };
        //public static string[] corpus = { "I", "want", "to", "race" };

        public static string[] tags;
        public static string[] corpus;

        Dictionary<string, Dictionary<string, double>> transition = new Dictionary<string, Dictionary<string, double>>();
        Dictionary<string, Dictionary<string, double>> emission = new Dictionary<string, Dictionary<string, double>>();
        Dictionary<string, double> viterbi = new Dictionary<string, double>();

        public Form1()
        {

            InitializeComponent();

            populateDictFromFiles();

            int index = 0;
            corpus = new string[(emission[state]).Count];
            foreach (KeyValuePair<string, double> corpus_entry in emission[state])
            {
                corpus[index++] = corpus_entry.Key;
            }

        }

        private void populateDictFromFiles()
        {
            emission.Clear();
            transition.Clear();
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(@"../../App_Data/processed_brown/transition.txt", System.Text.Encoding.Default))
                {
                    // Read the stream to a string, and write the string to the console.
                    String line = sr.ReadLine();
                    String[] tags = line.Split(' ');
                    foreach (String prior in tags)
                    {
                        String probabilityLine = sr.ReadLine();
                        String[] probabilities = probabilityLine.Split(' ');
                        Dictionary<string, double> priorTransition = new Dictionary<string, double>();
                        for (int i = 0; i < tags.Length; i++)
                        {
                            priorTransition.Add(tags[i], float.Parse(probabilities[i]));
                        }
                        transition.Add(prior, priorTransition);
                    }
                }
            }
            catch (Exception e)
            {
                status.Text = "The file could not be read:" + e.Message;
            }
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(@"../../App_Data/processed_brown/emmission.txt", System.Text.Encoding.Default))
                {
                    // Read the stream to a string, and write the string to the console.
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        String[] tagWordsPair = line.Split('\t');
                        String tag = tagWordsPair[0];

                        Dictionary<string, double> emmissionDict = new Dictionary<string, double>();
                        String words = tagWordsPair[1];
                        String[] wordProbabilities = words.Split(' ');
                        for (int i = 0; i < wordProbabilities.Length; i += 2)
                        {
                            String wordForm = wordProbabilities[i];
                            double probability = double.Parse(wordProbabilities[i + 1]);
                            emmissionDict.Add(wordForm, probability);
                        }
                        emission.Add(tag, emmissionDict);
                    }
                }
            }
            catch (Exception e)
            {
                status.Text = "The file could not be read:" + e.Message;
            }

            tags = new string[transition.Count];

            int index = 0;
            foreach (KeyValuePair<string, Dictionary<string, double>> tag_entry in transition)
            {
                tags[index++] = tag_entry.Key;
            }


        }

        private void populateDictFromStub()
        {
            emission.Clear();
            Dictionary<string, double> tempEmissionArr = new Dictionary<string, double>();
            tempEmissionArr.Add("I", 0);
            tempEmissionArr.Add("want", 0.0093);
            tempEmissionArr.Add("to", 0);
            tempEmissionArr.Add("race", 0.00012);
            emission.Add("VB", tempEmissionArr);

            tempEmissionArr = new Dictionary<string, double>();
            tempEmissionArr.Add("I", 0);
            tempEmissionArr.Add("want", 0);
            tempEmissionArr.Add("to", 0.99);
            tempEmissionArr.Add("race", 0);
            emission.Add("TO", tempEmissionArr);

            tempEmissionArr = new Dictionary<string, double>();
            tempEmissionArr.Add("I", 0);
            tempEmissionArr.Add("want", 0.000054);
            tempEmissionArr.Add("to", 0);
            tempEmissionArr.Add("race", 0.00057);
            emission.Add("NN", tempEmissionArr);

            tempEmissionArr = new Dictionary<string, double>();
            tempEmissionArr.Add("I", 0.37);
            tempEmissionArr.Add("want", 0);
            tempEmissionArr.Add("to", 0);
            tempEmissionArr.Add("race", 0);
            emission.Add("PPSS", tempEmissionArr);

            transition.Clear();
            Dictionary<string, double> tempTransitionArr = new Dictionary<string, double>();
            tempTransitionArr.Add("VB", 0.019);
            tempTransitionArr.Add("TO", 0.0043);
            tempTransitionArr.Add("NN", 0.041);
            tempTransitionArr.Add("PPSS", 0.067);
            transition.Add("<s>", tempTransitionArr);

            tempTransitionArr = new Dictionary<string, double>();
            tempTransitionArr.Add("VB", 0.0038);
            tempTransitionArr.Add("TO", 0.035);
            tempTransitionArr.Add("NN", 0.047);
            tempTransitionArr.Add("PPSS", 0.007);
            transition.Add("VB", tempTransitionArr);

            tempTransitionArr = new Dictionary<string, double>();
            tempTransitionArr.Add("VB", 0.83);
            tempTransitionArr.Add("TO", 0);
            tempTransitionArr.Add("NN", 0.00047);
            tempTransitionArr.Add("PPSS", 0);
            transition.Add("TO", tempTransitionArr);

            tempTransitionArr = new Dictionary<string, double>();
            tempTransitionArr.Add("VB", 0.004);
            tempTransitionArr.Add("TO", 0.16);
            tempTransitionArr.Add("NN", 0.087);
            tempTransitionArr.Add("PPSS", 0.0045);
            transition.Add("NN", tempTransitionArr);

            tempTransitionArr = new Dictionary<string, double>();
            tempTransitionArr.Add("VB", 0.23);
            tempTransitionArr.Add("TO", 0.00079);
            tempTransitionArr.Add("NN", 0.0012);
            tempTransitionArr.Add("PPSS", 0.00014);
            transition.Add("PPSS", tempTransitionArr);
        }

        int first_char = 0;
        string word = "", state = "\\t";
        double prev_prob = 1, prob1, prob2;

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            editorTB.Text += listBox1.SelectedItem.ToString();
        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        private void editorTB_TextChanged(object sender, EventArgs e)   //backspacing doesnt work
        {

            int length = editorTB.Text.Length;
            if (length > 0 && editorTB.Text[length - 1] == ' ')
            {
                first_char = editorTB.Text.LastIndexOf(' ', length - 2) + 1;
                word = editorTB.Text.Substring(first_char, length - first_char).Trim();
                label2.Text = "Current word: " + word;

                tagsLabel.Text += ' ';
                double max = 0;
                string sentence = editorTB.Text.ToLower();
                string[] list_of_words = sentence.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                status.Text = "Loading...";
                double[,] matrix = new double[tags.Length + 2, list_of_words.Length];
                int[,] backPointers = new int[tags.Length + 2, list_of_words.Length];
                int lastBackPointer = fillViterbi(matrix,backPointers, list_of_words, transition, emission);
                tagsLabel.Text = "";
                if(lastBackPointer != -1)
                {
                    if(matrix[lastBackPointer, list_of_words.Length - 1] > 0.000001)
                    {
                        editorTB.BackColor = System.Drawing.Color.Green;
                    }else
                    {
                        editorTB.BackColor = System.Drawing.Color.Red;
                    }
                    tagsLabel.Text += "<" + tags[lastBackPointer] + ">";
                    for (int i = list_of_words.Length - 1; i >= 0; i--)
                    {
                        int backPointer = backPointers[lastBackPointer, i];
                        if(backPointer != -1)
                        {
                            tagsLabel.Text += "<"+tags[backPointer]+">";
                        }else
                        {
                            tagsLabel.Text += "<!>";
                        }
                    }
                }
                Reverse(tagsLabel.Text);
                string next_best_word = predictNextWord(matrix, list_of_words, transition, emission);
                listBox1.Items.Clear();
                if(next_best_word != null)
                {
                    listBox1.Items.Add(next_best_word);
                }
                status.Text = "";
            }
            else if (length == 0)
            {
                word = "";
                label2.Text = "Current word" + word;
            }
        }

        public static int fillViterbi(double[,] matrix, int[,] backPointers, string[] words, Dictionary<string, Dictionary<string, double>> transition, Dictionary<string, Dictionary<string, double>> emission)
        {
            for (int tagI = 0; tagI < tags.Length; tagI++)
            {
                double trans_prob = transition["\\t"][tags[tagI]];
                matrix[tagI, 0] = trans_prob * (emission[tags[tagI]].ContainsKey(words[0]) ? emission[tags[tagI]][words[0]] : 0);
                backPointers[tagI, 0] = 23;
            }
            for (int wordI = 1; wordI < words.Length; wordI++)
            {
                for (int tagI = 0; tagI < tags.Length; tagI++)
                {
                    double max = 0;
                    int maxTagJ = -1;
                    string maxTag = null;
                    for (int tagJ = 0; tagJ < tags.Length; tagJ++)
                    {
                        double tagTransistion = matrix[tagJ, wordI - 1] * transition[tags[tagJ]][tags[tagI]];
                        if (tagTransistion > max)
                        {
                            max = tagTransistion;
                            maxTagJ = tagJ;
                            maxTag = tags[tagJ];
                        }
                    }
                    matrix[tagI, wordI] = max * (emission[tags[tagI]].ContainsKey(words[wordI]) ? emission[tags[tagI]][words[wordI]] : 0);
                    backPointers[tagI, wordI] = maxTagJ;
                }
            }
            double maxBP = 0;
            int maxI = -1;
            for(int i = 0; i < tags.Length; i++)
            {
                if(matrix[i, words.Length - 1] > maxBP)
                {
                    maxBP = matrix[i, words.Length - 1];
                    maxI = i;
                }
            }
            return maxI;
        }

        public static string predictNextWord(double[,] matrix, string[] words, Dictionary<string, Dictionary<string, double>> transition, Dictionary<string, Dictionary<string, double>> emission)
        {
            double max = 0;
            String maxTag = null, maxWord = null;
            for(int tagI = 0; tagI < tags.Length; tagI++)
            {
                for (int prevTagI = 0; prevTagI < tags.Length; prevTagI++)
                {
                    foreach (KeyValuePair<string, double> corpus_entry in emission[tags[tagI]])
                    {
                        double a = matrix[prevTagI, words.Length - 1];
                        double b = transition[tags[prevTagI]][tags[tagI]];
                        double c = corpus_entry.Value;
                        double prob =  a * b * c;
                        if(a>0 && b>0 && c>0)
                        {
                            int h = 6;
                        }
                        if(prob > max)
                        {
                            max = prob;
                            maxTag = tags[tagI];
                            maxWord = corpus_entry.Key;
                        }
                    }
                }
            }

            /*
            //trying all corpora and returning best match according to matrix found above
            double highestProbability = 0;
            string bestNextWord = "";
            foreach (string corpora in corpus)
            {
                double highestProbabilityForCurrentCorpora = 0;
                foreach (string tag in tags)
                {
                    int k = 1;
                    double max = 0;
                    foreach (string prevtag in tags)
                    {
                        double tagTransistion = matrix[k, i - 1] * transition[prevtag][tag];
                        if (tagTransistion > max)
                        {
                            max = tagTransistion;
                        }
                        k++;
                    }
                    double corporaProbabilityForCurrrentTag = max * emission[tag][corpora];
                    if (corporaProbabilityForCurrrentTag > highestProbabilityForCurrentCorpora)
                    {
                        highestProbabilityForCurrentCorpora = corporaProbabilityForCurrrentTag;
                    }
                }

                //If you want to return best three or whatever i just need to store this shit and sort
                if (highestProbabilityForCurrentCorpora > highestProbability)
                {
                    highestProbability = highestProbabilityForCurrentCorpora;
                    bestNextWord = corpora;
                }
            }
            */
            return maxWord;
        }
    }
}
