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
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Page
    {
        // private member variable to store which places on the board are taken 
        char[] boardState;
        int[] colorState;
        string playerName;
        Button[] buttonsOnBoard;
        Boolean won = false;
        Boolean lost = false;

        cpp_file cpp;
        int[] intCompAnswer;

        public Game(string name)
        {
            InitializeComponent();
            boardState = new char[64];
            colorState = new int[64];

            playerName = name;
            buttonsOnBoard = new Button[] {cell1, cell2, cell3, cell4, cell5, cell6, cell7, cell8,
                                            cell9, cell10, cell11, cell12, cell13, cell14, cell15, cell16,
                                            cell17, cell18, cell19, cell20, cell21, cell22, cell23, cell24,
                                            cell25, cell26, cell27, cell28, cell29, cell30, cell31, cell32,
                                            cell33, cell34, cell35, cell36, cell37, cell38, cell39, cell40,
                                            cell41, cell42, cell43, cell44, cell45, cell46, cell47, cell48,
                                            cell49, cell50, cell51, cell52, cell53, cell54, cell55, cell56,
                                            cell57, cell58, cell59, cell60, cell61, cell62, cell63, cell64};

            displayName.Text = "Player: " + playerName;

            //new instance of the cpp code that is loaded from cpp.dll
            cpp = new cpp_file();
            //array that is passed to the dll code and oerwritten with the ai's answers
            intCompAnswer = new int[6];
        }

        private void colorSpaces(int[] spacesToColor)
        {
            for (int i = 0; i < 64; i++)
            {
                if (spacesToColor[i] == 1)
                {
                    BrushConverter bc = new BrushConverter();
                    buttonsOnBoard[i].Background = (Brush)bc.ConvertFrom("#EA8F8F");
                }
                else
                {
                    buttonsOnBoard[i].Background = System.Windows.Media.Brushes.Transparent;
                }
            }
        }
        // sets win string to visible on game board
        private void winString_Loaded(object sender, EventArgs e)
        {
            if (won)
            {
                WON.Visibility = Visibility.Visible;
            }
        }
        // sets lose string to visible on game board
        private void loseString_Loaded(object sender, EventArgs e)
        {
            if (lost)
            {
                LOST.Visibility = Visibility.Visible;
            }
        }
        // sets all strings and buttons needed for play again to visible on game board
        private void playAgain_loaded(object sender, EventArgs e)
        {
            if (lost || won)
            {
                displayName.Visibility = Visibility.Collapsed;
                endButton.Visibility = Visibility.Collapsed;
                PlayAgain.Visibility = Visibility.Visible;
                YES.Visibility = Visibility.Visible;
                NO.Visibility = Visibility.Visible;
                HIGHSCORE.Visibility = Visibility.Visible;
                updateScore();
            }
        }
        // creates a new game when user clicks yes
        private void YES_Click(object sender, RoutedEventArgs e)
        {
            if (!(endButton.Background == System.Windows.Media.Brushes.Transparent))
            {
                NavigationService.Navigate(new Game(playerName));
            }

        }
        // takes user to menu when user clicks no
        private void NO_Click(object sender, RoutedEventArgs e)
        {
            if (!(endButton.Background == System.Windows.Media.Brushes.Transparent))
            {
                NavigationService.Navigate(new Menu());
            }
        }
        // takes user to high score screen
        private void HIGHSCORE_Click(object sender, RoutedEventArgs e)
        {
            if (!(endButton.Background == System.Windows.Media.Brushes.Transparent))
            {
                NavigationService.Navigate(new HighScores());
            }
        }

        public void updateScore()
        {
            HighScores high = new HighScores(); // creates a new high score object 
            int found = 0;
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\HighScores.txt"; // sets the path where high scores txt file is
            if (!System.IO.File.Exists(path))
            {
                System.IO.StreamWriter sw = System.IO.File.CreateText(path);  // creates a new txt file if one hasnt been created
            }
            // if the user has played, will update their score if they won
            for (int i = 0; i < high.scores.Count(); ++i)
            {
                if (playerName.Equals(high.scores.ElementAt(i).getInit()))
                {
                    found++;
                    if (won)
                        high.scores.ElementAt(i).setScore((high.scores.ElementAt(i).getScore() + 1));
                    using (System.IO.StreamWriter file =
           new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"\HighScores.txt"))
                    {
                        foreach (Score line in high.scores)
                        {
                            file.WriteLine(line.getInit() + " " + (line.getScore()).ToString());
                        }
                    }
                }
            }
            // if the user hasn't played
            if (found == 0)
            {
                int sc = 0;
                if (won)
                    ++sc;
                Score newPlayer = new Score(playerName, sc); // creates new score object for new user 
                high.scores.Add(newPlayer);
                high.scores = high.rightOrder(high.scores); // orders list

                using (System.IO.StreamWriter file =
           new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"\HighScores.txt")) 
                {
                    foreach (Score line in high.scores)
                    {
                        file.WriteLine(line.getInit() + " " + (line.getScore()).ToString()); // writes new file
                    }
                }
            }
        }

        private void handleEvent(int index, object sender, EventArgs e)
        {
            //check to make sure the space is not occupied 
            if (boardState[index] == 'X' || boardState[index] == 'O')
            {
                return;
            }

            buttonsOnBoard[index].Content = "X";
            boardState[index] = 'X';
            colorState[index] = 1;
            colorSpaces(colorState);
            colorState[index] = 0;

            //writes to intCompAnswer[], intCompAnswer[0] is the computer's move (-1 if the player has already won with their move)
            //intCompAnswer[1-4] returns what moves make up the winning four in a row, if any
            //intCompAnswer[5] return 1 if the game is won, 0 otherwise 
            cpp.aiCall(index, intCompAnswer);

            //someone has won
            if (intCompAnswer[5] == 1)
            {
                //player has won!
                if (intCompAnswer[0] == -1)
                {
                    won = true;
                    winString_Loaded(sender, e);
                    playAgain_loaded(sender, e);
                }
                //computer has won
                else
                {
                    AIhandleEvent(intCompAnswer[0]);
                    lost = true;
                    loseString_Loaded(sender, e);
                    playAgain_loaded(sender, e);
                }

                //color the winning moves
                for (int i = 1; i < 5; ++i)
                {
                    colorState[intCompAnswer[i]] = 1;
                }
                colorSpaces(colorState);

                cpp.Dispose();
            }
            //color computer move as normal
            else
            {
                AIhandleEvent(intCompAnswer[0]);
            }
        }
  

        private void AIhandleEvent(int index)
        {
            buttonsOnBoard[index].Content = "O";
            boardState[index] = '0';
            colorState[index] = 1;
            colorSpaces(colorState);
            colorState[index] = 0;
        }

        private void endButton_Click(object sender, RoutedEventArgs e)
        {
            if ( !(endButton.Background == System.Windows.Media.Brushes.Transparent) )
            {
                NavigationService.Navigate(new Menu());
            }
            
        }

        /****************************************** ALL 64 BUTTONS *************************************/

        private void cell1_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            handleEvent(index, sender, e);
        }

        private void cell2_Click(object sender, RoutedEventArgs e)
        {
            int index = 1;
            handleEvent(index, sender, e);
        }

        private void cell3_Click(object sender, RoutedEventArgs e)
        {
            int index = 2;
            handleEvent(index, sender, e);
        }

        private void cell4_Click(object sender, RoutedEventArgs e)
        {
            int index = 3;
            handleEvent(index, sender, e);
        }

        private void cell5_Click(object sender, RoutedEventArgs e)
        {
            int index = 4;
            handleEvent(index, sender, e);
        }

        private void cell6_Click(object sender, RoutedEventArgs e)
        {
            int index = 5;
            handleEvent(index, sender, e);
        }

        private void cell7_Click(object sender, RoutedEventArgs e)
        {
            int index = 6;
            handleEvent(index, sender, e);
        }

        private void cell8_Click(object sender, RoutedEventArgs e)
        {
            int index = 7;
            handleEvent(index, sender, e);
        }

        private void cell9_Click(object sender, RoutedEventArgs e)
        {
            int index = 8;
            handleEvent(index, sender, e);
        }

        private void cell10_Click(object sender, RoutedEventArgs e)
        {
            int index = 9;
            handleEvent(index, sender, e);
        }

        private void cell11_Click(object sender, RoutedEventArgs e)
        {
            int index = 10;
            handleEvent(index, sender, e);
        }

        private void cell12_Click(object sender, RoutedEventArgs e)
        {
            int index = 11;
            handleEvent(index, sender, e);
        }

        private void cell13_Click(object sender, RoutedEventArgs e)
        {
            int index = 12;
            handleEvent(index, sender, e);
        }

        private void cell14_Click(object sender, RoutedEventArgs e)
        {
            int index = 13;
            handleEvent(index, sender, e);
        }

        private void cell15_Click(object sender, RoutedEventArgs e)
        {
            int index = 14;
            handleEvent(index, sender, e);
        }

        private void cell16_Click(object sender, RoutedEventArgs e)
        {
            int index = 15;
            handleEvent(index, sender, e);
        }

        private void cell17_Click(object sender, RoutedEventArgs e)
        {
            int index = 16;
            handleEvent(index, sender, e);
        }

        private void cell18_Click(object sender, RoutedEventArgs e)
        {
            int index = 17;
            handleEvent(index, sender, e);
        }

        private void cell19_Click(object sender, RoutedEventArgs e)
        {
            int index = 18;
            handleEvent(index, sender, e);
        }

        private void cell20_Click(object sender, RoutedEventArgs e)
        {
            int index = 19;
            handleEvent(index, sender, e);
        }

        private void cell21_Click(object sender, RoutedEventArgs e)
        {
            int index = 20;
            handleEvent(index, sender, e);
        }

        private void cell22_Click(object sender, RoutedEventArgs e)
        {
            int index = 21;
            handleEvent(index, sender, e);
        }

        private void cell23_Click(object sender, RoutedEventArgs e)
        {
            int index = 22;
            handleEvent(index, sender, e);
        }

        private void cell24_Click(object sender, RoutedEventArgs e)
        {
            int index = 23;
            handleEvent(index, sender, e);
        }

        private void cell25_Click(object sender, RoutedEventArgs e)
        {
            int index = 24;
            handleEvent(index, sender, e);
        }

        private void cell26_Click(object sender, RoutedEventArgs e)
        {
            int index = 25;
            handleEvent(index, sender, e);
        }
            private void cell27_Click(object sender, RoutedEventArgs e)
        {
            int index = 26;
            handleEvent(index, sender, e);
        }

        private void cell28_Click(object sender, RoutedEventArgs e)
        {
            int index = 27;
            handleEvent(index, sender, e);
        }

        private void cell29_Click(object sender, RoutedEventArgs e)
        {
            int index = 28;
            handleEvent(index, sender, e);
        }

        private void cell30_Click(object sender, RoutedEventArgs e)
        {
            int index = 29;
            handleEvent(index, sender, e);
        }

        private void cell31_Click(object sender, RoutedEventArgs e)
        {
            int index = 30;
            handleEvent(index, sender, e);
        }

        private void cell32_Click(object sender, RoutedEventArgs e)
        {
            int index = 31;
            handleEvent(index, sender, e);
        }

        private void cell33_Click(object sender, RoutedEventArgs e)
        {
            int index = 32;
            handleEvent(index, sender, e);
        }

        private void cell34_Click(object sender, RoutedEventArgs e)
        {
            int index = 33;
            handleEvent(index, sender, e);
        }

        private void cell35_Click(object sender, RoutedEventArgs e)
        {
            int index = 34;
            handleEvent(index, sender, e);
        }

        private void cell36_Click(object sender, RoutedEventArgs e)
        {
            int index = 35;
            handleEvent(index, sender, e);
        }

        private void cell37_Click(object sender, RoutedEventArgs e)
        {
            int index = 36;
            handleEvent(index, sender, e);
        }

        private void cell38_Click(object sender, RoutedEventArgs e)
        {
            int index = 37;
            handleEvent(index, sender, e);
        }

        private void cell39_Click(object sender, RoutedEventArgs e)
        {
            int index = 38;
            handleEvent(index, sender, e);
        }

        private void cell40_Click(object sender, RoutedEventArgs e)
        {
            int index = 39;
            handleEvent(index, sender, e);
        }

        private void cell41_Click(object sender, RoutedEventArgs e)
        {
            int index = 40;
            handleEvent(index, sender, e);
        }

        private void cell42_Click(object sender, RoutedEventArgs e)
        {
            int index = 41;
            handleEvent(index, sender, e);
        }

        private void cell43_Click(object sender, RoutedEventArgs e)
        {
            int index = 42;
            handleEvent(index, sender, e);
        }

        private void cell44_Click(object sender, RoutedEventArgs e)
        {
            int index = 43;
            handleEvent(index, sender, e);
        }

        private void cell45_Click(object sender, RoutedEventArgs e)
        {
            int index = 44;
            handleEvent(index, sender, e);
        }

        private void cell46_Click(object sender, RoutedEventArgs e)
        {
            int index = 45;
            handleEvent(index, sender, e);
        }

        private void cell47_Click(object sender, RoutedEventArgs e)
        {
            int index = 46;
            handleEvent(index, sender, e);
        }

        private void cell48_Click(object sender, RoutedEventArgs e)
        {
            int index = 47;
            handleEvent(index, sender, e);
        }

        private void cell49_Click(object sender, RoutedEventArgs e)
        {
            int index = 48;
            handleEvent(index, sender, e);
        }

        private void cell50_Click(object sender, RoutedEventArgs e)
        {
            int index = 49;
            handleEvent(index, sender, e);
        }

        private void cell51_Click(object sender, RoutedEventArgs e)
        {
            int index = 50;
            handleEvent(index, sender, e);
        }

        private void cell52_Click(object sender, RoutedEventArgs e)
        {
            int index = 51;
            handleEvent(index, sender, e);
        }

        private void cell53_Click(object sender, RoutedEventArgs e)
        {
            int index = 52;
            handleEvent(index, sender, e);
        }

        private void cell54_Click(object sender, RoutedEventArgs e)
        {
            int index = 53;
            handleEvent(index, sender, e);
        }

        private void cell55_Click(object sender, RoutedEventArgs e)
        {
            int index = 54;
            handleEvent(index, sender, e);
        }

        private void cell56_Click(object sender, RoutedEventArgs e)
        {
            int index = 55;
            handleEvent(index, sender, e);
        }

        private void cell57_Click(object sender, RoutedEventArgs e)
        {
            int index = 56;
            handleEvent(index, sender, e);
        }

        private void cell58_Click(object sender, RoutedEventArgs e)
        {
            int index = 57;
            handleEvent(index, sender, e);
        }

        private void cell59_Click(object sender, RoutedEventArgs e)
        {
            int index = 58;
            handleEvent(index, sender, e);
        }

        private void cell60_Click(object sender, RoutedEventArgs e)
        {
            int index = 59;
            handleEvent(index, sender, e);
        }

        private void cell61_Click(object sender, RoutedEventArgs e)
        {
            int index = 60;
            handleEvent(index, sender, e);
        }

        private void cell62_Click(object sender, RoutedEventArgs e)
        {
            int index = 61;
            handleEvent(index, sender, e);
        }

        private void cell63_Click(object sender, RoutedEventArgs e)
        {
            int index = 62;
            handleEvent(index, sender, e);
        }

        private void cell64_Click(object sender, RoutedEventArgs e)
        {
            int index = 63;
            handleEvent(index, sender, e);
        }
    }
}
