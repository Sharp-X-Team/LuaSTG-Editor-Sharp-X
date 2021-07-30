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

namespace LuaSTGEditorSharp.Windows
{
    /// <summary>
    /// AboutNode.xaml 的交互逻辑
    /// </summary>
    public partial class AboutNodeWindow : Window
    {
        string Selection = "About";
        public AboutNodeWindow()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Selection = "About";
        }
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            Selection = "General";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
