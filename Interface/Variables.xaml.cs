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

namespace Interface
{
    /// <summary>
    /// Логика взаимодействия для Variables.xaml
    /// </summary>
    public partial class Variables : Window
    {
        public Variables()
        {
            InitializeComponent();
            scroll.ScrollToBottom();
        }

        //
        // вспомогательные функции
        //
        //

        public bool IsValidVar(string var, Dictionary<string, string> var_dict)
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


        //
        //
        // кнопки для работы со списком переменных
        //
        //

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var var_dict = (Owner as MainWindow).var_dict;
            string var = new_var.Text;
            if (!IsValidVar(var, var_dict))
            {
                MessageBox.Show("Not valid variable name!");
                return;
            }
            var_dict.Add(var, "");
            variables.Items.Add(var);
            (Owner as MainWindow).var_dict = var_dict;
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            var var_dict = (Owner as MainWindow).var_dict;
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
            (Owner as MainWindow).var_dict = var_dict;
            //todo: удалить, только если эта переменная не задействована в формуле
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            variables.Items.Clear();
            (Owner as MainWindow).var_dict.Clear();
            //todo: очистить, только если ни одна переменная не задействована в формуле
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            e.Cancel = true;
            Hide();
        }
    }  
}
