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
    /// Логика взаимодействия для DebugOut.xaml
    /// </summary>
    public partial class DebugOut : Window
    {
        private bool finale_debug;
        public DebugOut(bool debug_status)
        {
            InitializeComponent();
            LogText.IsReadOnly = true;

            LogText.AppendText(Debug.GetLogs());

            finale_debug = debug_status;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (finale_debug)
                Environment.Exit(0);
        }
    }
}
