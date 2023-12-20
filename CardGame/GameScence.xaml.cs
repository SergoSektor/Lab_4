using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CardGame
{
    /// <summary>
    /// Логика взаимодействия для GameScence.xaml
    /// </summary>
    public partial class GameScence : Window
    {
        Controller game;
        Canvas[] PlayerHandCardsCanvas;
        Button[] PlayerHandCards;
        Canvas[] border;
        Button[] choiceLines;
        Image[] stage_images;

        int selected_number;
        public GameScence()
        {
            InitializeComponent();
            Initialize();


            game = new Controller(RoundNumber, RoundStatus, EnemyHP, EnemyMP, PlayerHP, PlayerMP, PlayerHandCards, PlayerHandCardsCanvas, Footnote, border, stage_images);

            game.GameOutPut();

            AskPlayer(false);
        }

        public GameScence(Controller load_game) {
            InitializeComponent();
            Initialize();

            game = load_game;

            game.Load(RoundNumber, RoundStatus, EnemyHP, EnemyMP, PlayerHP, PlayerMP, PlayerHandCards, PlayerHandCardsCanvas, Footnote, border, stage_images);

            game.GameOutPut();
            game.RoundStage();

            AskPlayer(false);
            game.RoundStage();
        }

        private void Initialize() {
            PlayerHandCardsCanvas = new Canvas[] {
                HandCard1Canvas,
                HandCard2Canvas,
                HandCard3Canvas,
                HandCard4Canvas,
            };
            PlayerHandCards = new Button[] {
                HandCard1,
                HandCard2,
                HandCard3,
                HandCard4,
            };
            border = new Canvas[] {
                Card_line1_e,
                Card_line1_f,
                Card_line2_e,
                Card_line2_f,
                Card_line3_e,
                Card_line3_f,
                Card_line4_e,
                Card_line4_f,
                Card_line5_e,
                Card_line5_f,
            };
            choiceLines = new Button[] {
                ChoiceLine1,
                ChoiceLine2,
                ChoiceLine3,
                ChoiceLine4,
                ChoiceLine5,
            };
            stage_images = new Image[] { 
                Stage1, 
                Stage2,
                Stage3,
            };

        }

        private void DebugButton_Click(object sender, RoutedEventArgs e)
        { 
            Footnote.Content = "";
            DebugOut debug = new DebugOut(false);
            debug.ShowDialog();
        }

        private void SkipButton_Click(object sender, RoutedEventArgs e)
        {
            Footnote.Content = "";
            game.Skip();
            game.RoundStage();
        }

        private void HandCard1_Click(object sender, RoutedEventArgs e)
        {
            PlayCard(1);
        }
        private void HandCard2_Click(object sender, RoutedEventArgs e)
        {
            PlayCard(2);
        }
        private void HandCard3_Click(object sender, RoutedEventArgs e)
        {
            PlayCard(3);
        }
        private void HandCard4_Click(object sender, RoutedEventArgs e)
        {
            PlayCard(4);
        }

        private void AskPlayer(bool value) {
            Footnote.Content = "";

            for (int i = 0; i < choiceLines.Length;i++)
                choiceLines[i].Visibility = (value) ? (Visibility.Visible):(Visibility.Hidden);

            for (int i = 0; i < PlayerHandCards.Length; i++)
                PlayerHandCards[i].IsEnabled = !value;

            SkipButton.IsEnabled = !value;
            DebugButton.IsEnabled = !value;
        }

        private void ChoiceLine1_Click(object sender, RoutedEventArgs e)
        {
            ChoiceLine(1);
        }

        private void ChoiceLine2_Click(object sender, RoutedEventArgs e)
        {
            ChoiceLine(2);
        }

        private void ChoiceLine3_Click(object sender, RoutedEventArgs e)
        {
            ChoiceLine(3);
        }

        private void ChoiceLine4_Click(object sender, RoutedEventArgs e)
        {
            ChoiceLine(4);
        }

        private void ChoiceLine5_Click(object sender, RoutedEventArgs e)
        {
            ChoiceLine(5);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            DataContractSerializer jsonF = new DataContractSerializer(typeof(Controller));
            game.Save();
            using (FileStream fileStream = new FileStream("GameSave.json", FileMode.Create))
                jsonF.WriteObject(fileStream, game);

            Environment.Exit(0);
        }

        private void PlayCard(int card_number)
        {
            Footnote.Content = "";
            try
            {
                if (game.PlayCard(card_number) != null)
                {
                    selected_number = card_number;
                    AskPlayer(true);
                }
            }
            catch (Exception ex)
            {
                Footnote.Content = ex.Message;
            }
            game.GameOutPut();
        }

        private void ChoiceLine(int line_number) {
            Footnote.Content = "";
            game.PlayCard(selected_number, line_number);
            game.GameOutPut();
            AskPlayer(false);
        }
    }
}
