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

namespace Interface
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Dictionary<string, string> var_dict = new Dictionary<string, string>();

        public bool IsValidVar(string var)
        {
            if (var == null || var == "")
                return false;
            if (var_dict != null)
            {
                foreach (string key in var_dict.Keys)
                    if (var == key)
                        return false;
            }
            if (var.Contains(" ") || var.Length > 3)
                return false;
            return true;
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }


        //кнопки для работы со списком переменных
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string var = new_var.Text;
            if(!IsValidVar(var))
            {
                MessageBox.Show("Not valid variable name!");
                return;
            }
            var_dict.Add(var, "");
            variables.Items.Add(var); 
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            string elem = (string)variables.SelectedItem;
            if (elem != null)
            {
                var_dict.Remove(elem);
                variables.Items.Remove(elem);
            }
            else
            {
                elem = new_var.Text;
                if (var_dict.ContainsKey(elem))
                {
                    var_dict.Remove(elem);
                    variables.Items.Remove(elem);
                }

            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
