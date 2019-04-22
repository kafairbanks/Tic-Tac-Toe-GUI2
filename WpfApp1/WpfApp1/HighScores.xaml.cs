using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for HighScores.xaml
    /// </summary>
    /// // Score class is store the scores and initials of each player
    public class Score
    {
        private string initials;
        private int score;
        public Score(string init, int sc)
        {
            initials = init;
            score = sc;
        }

        public string getInit()
        {
            return initials;
        }

        public int getScore()
        {
            return score;
        }

        public void setScore(int sc)
        {
            score = sc;
        }
    }
    // high score class will update/load scores
    public partial class HighScores : Page
    {
        public List<Score> scores = new List<Score>();

        // loads in scores from txt file if there is a file
        public List<Score> load()
        {
            string line;
            string tempInitials = " ";
            int tempScore;
            string[] tokens = new string[50];
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\HighScores.txt"; // the file path
            if (!System.IO.File.Exists(path))
            {
                System.IO.StreamWriter sw = System.IO.File.CreateText(path); // creates a file if there isnt already one
                sw.Close();
            }
            // Read the file and display it line by line. 
            System.IO.StreamReader file = new System.IO.StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\HighScores.txt");
            while ((line = file.ReadLine()) != null)
            {
                tokens = line.Split(' ');
                tempInitials = tokens.ElementAt(0);
                tempScore = Int32.Parse(tokens.ElementAt(1));
                scores.Add(new Score(tempInitials, tempScore));
            }
            file.Close();
            
            return scores;
            // Suspend the screen. 
        }
        // makes the score list in order from greatest to least
        public List<Score> rightOrder(List<Score> sco)
        {
            List<Score> sorted = new List<Score>();

            // Find the minimum element in unsorted array
            while (sco.Any())
            {
                int min_idx = 0;
                int n = sco.Count();
                for (int j = 1; j < n; j++)
                {
                    if (sco.ElementAt(j).getScore() > sco.ElementAt(min_idx).getScore())
                    {
                        min_idx = j;
                    }
                }

                // Swap the found minimum element with the first
                // element
                int tempSc = sco.ElementAt(min_idx).getScore();
                String tempIn = sco.ElementAt(min_idx).getInit();
                Score tempScore = new Score(tempIn, tempSc);
                sorted.Add(tempScore);
                sco.RemoveAt(min_idx);
            }

            return sorted;
        }
        public HighScores()
        {
            InitializeComponent();
            load();
            scores = rightOrder(scores);
            // prints out the top 5 scores
            if (scores.Any())
            {
                string first = scores.ElementAt(0).getInit() + "            " + scores.ElementAt(0).getScore();
                First.Content = first;
                if (scores.Count() >= 2)
                {
                    string second = scores.ElementAt(1).getInit() + "            " + scores.ElementAt(1).getScore();
                    Second.Content = second;
                }
                if (scores.Count() >= 3)
                {
                    string third = scores.ElementAt(2).getInit() + "            " + scores.ElementAt(2).getScore();
                    Third.Content = third;
                }
                if (scores.Count() >= 4)
                {
                    string fourth = scores.ElementAt(3).getInit() + "            " + scores.ElementAt(3).getScore();
                    Fourth.Content = fourth;
                }
                if (scores.Count() >= 5)
                {
                    string fifth = scores.ElementAt(4).getInit() + "            " + scores.ElementAt(4).getScore();
                    Fifth.Content = fifth;
                }
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Menu());
        }
    }
}
