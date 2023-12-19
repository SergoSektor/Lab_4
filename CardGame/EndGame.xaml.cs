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
using System.Windows.Shapes;

namespace CardGame
{
    /// <summary>
    /// Логика взаимодействия для EndGame.xaml
    /// </summary>
    public partial class EndGame : Window
    {
        public EndGame(bool PlayerWin)
        {
            InitializeComponent();
            LoseText.Content = (PlayerWin) ? "Вы выиграли" : "Вы проиграли";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DebugOut debug = new DebugOut(true);
            debug.ShowDialog();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
