using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Viterbi
{
    public partial class Form1 : Form
    {
        const int num1 = 4, num2 = 5;
        public static string[] tags = { "VB", "TO", "NN", "PPSS" };
        public static string[] corpus = { "I", "want", "to", "race" };
        Dictionary<string, Dictionary<string, double>> transition = new Dictionary<string, Dictionary<string, double>>();
        Dictionary<string, Dictionary<string, double>> emission = new Dictionary<string, Dictionary<string, double>>();
        Dictionary<string, double> viterbi = new Dictionary<string, double>();

        public Form1()
        {

            InitializeComponent();

            populateDictFromStub();
            
            AutoCompleteStringCollection suggestions = new AutoCompleteStringCollection();
            suggestions.Add("I");
            suggestions.Add("want");
            suggestions.Add("race");
            editorTB.AutoCompleteCustomSource = suggestions;
            editorTB.AutoCompleteMode = AutoCompleteMode.Suggest;
            editorTB.AutoCompleteSource = AutoCompleteSource.CustomSource;

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
                        status.Text = probabilityLine;
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
                        status.Text = line;
                        String[] tagWordsPair = line.Split('\t');
                        String tag = tagWordsPair[0];

                        Dictionary<string, double> emmissionDict = new Dictionary<string, double>();
                        String words = tagWordsPair[1];
                        String[] wordProbabilities = words.Split(' ');
                        for(int i=0; i<wordProbabilities.Length; i += 2)
                        {
                            String wordForm = wordProbabilities[i];
                            double probability = double.Parse(wordProbabilities[i+1]);
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
        string word = "", state = "<s>";
        double prev_prob = 1, prob1, prob2;

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            editorTB.Text += listBox1.SelectedItem.ToString() + " ";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)   //backspacing doesnt work
        {
            int length = editorTB.Text.Length;
            if (length > 0 && editorTB.Text[length - 1] == ' ')
            {
                first_char = editorTB.Text.LastIndexOf(' ', length - 2) + 1;
                word = editorTB.Text.Substring(first_char, length - first_char).Trim();
                currentWordLabel.Text = "Current word: " + word;

                tagsLabel.Text += ' ';
                double max = 0;
                //foreach (KeyValuePair<string, double> entry in transition[state]) //1st val of entry=< "<s>", VB >
                for (int i = 0; i < transition[state].Count; i++)
                {
                    var entry = transition[state];
                    prob1 = prev_prob * entry[tags[i]];
                    try
                    {
                        prob2 = prob1 * emission[tags[i]][word];
                    }
                    catch (Exception exc)
                    {
                        prob2 = 0;
                    }
                    if (prob2 > max)
                    {
                        max = prob2;
                        state = tags[i];
                    }

                }
                prev_prob = max;
                tagsLabel.Text += "\t->\t" + state;
                string[] list_of_words = editorTB.Text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                string next_best_word = predictNextWord(list_of_words, transition, emission);
                listBox1.Items.Clear();
                listBox1.Items.Add(next_best_word);
            }
            else if (length == 0)
            {
                word = "";
                currentWordLabel.Text = "Current word" + word;
            }

        }




        public static string predictNextWord(string[] words, Dictionary<string, Dictionary<string, double>> transistion, Dictionary<string, Dictionary<string, double>> emission)
        {
            int MATRIX_ROWS = tags.Length + 2;
            int MATRIX_COLUMNS = words.Length + 1;
    
            //Viterbi Matrix
            double[,] matrix = new double[MATRIX_ROWS, MATRIX_COLUMNS];
            bool firstWord = true;
            int i = 0, j = 0;
            foreach (string word in words)
            {
                //Initialization for the first word
                if (firstWord)
                {
                    j = 1;
                    foreach (string tag in tags)
                    {
                        matrix[j, i] = transistion["<s>"][tag] * emission[tag][word];
                        j++;
                    }
                    firstWord = false;
                    i++;
                    continue;
                }

                //Viterbi algo
                j = 1;
                foreach (string tag in tags)
                {
                    double max = 0;
                    int k = 1;
                    foreach (string prevTag in tags)
                    {
                        double tagTransistion = matrix[k, i - 1] * transistion[prevTag][tag];
                        if (tagTransistion > max)
                        {
                            max = tagTransistion;
                        }
                        k++;
                    }
                    matrix[j, i] = max * emission[tag][word];
                    j++;
                }
                i++;

            }

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
                        double tagTransistion = matrix[k, i - 1] * transistion[prevtag][tag];
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
            return bestNextWord;
        }
    }
}
