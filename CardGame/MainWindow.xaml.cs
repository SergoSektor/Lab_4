using Newtonsoft.Json;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CardGame
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Controller load_game;
        public MainWindow()
        {
            InitializeComponent();
            LoadButton.Visibility = LoadLabel.Visibility = Visibility.Hidden;


            if (File.Exists("GameSave.json"))
            {

                JsonSerializerSettings settings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                };
                DataContractSerializer jsonF = new DataContractSerializer(typeof(Controller));
                using (FileStream fileStream = new FileStream("GameSave.json", FileMode.Open))
                    load_game = (Controller)jsonF.ReadObject(fileStream);

                LoadButton.Visibility = LoadLabel.Visibility = Visibility.Visible;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GameScence gameScence = new GameScence();

            gameScence.Show();
            this.Hide();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            GameScence gameScence = new GameScence(load_game);

            gameScence.Show();
            this.Hide();
        }
    }
}
