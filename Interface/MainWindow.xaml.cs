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

        private Calc calculate_window = new Calc();

        private Variables var_window = new Variables();

      
        //
        //
        // конструкторы и загрузчики
        //
        //

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            calculate_window.Owner = this;
            var_window.Owner = this;
        }

        //
        //
        // меню
        //
        //

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

       

        //
        //
        // основные кнопки
        //
        //

        private void calculate_Click(object sender, RoutedEventArgs e)
        {
            calculate_window.ShowDialog();
        }

        private void del_last_Click(object sender, RoutedEventArgs e)
        {
            //todo
        }

        private void clear_formula_Click(object sender, RoutedEventArgs e)
        {
            //todo
        }

        private void change_variables_Click(object sender, RoutedEventArgs e)
        {
            var_window.ShowDialog();
        }

        //
        //
        // кнопки добавления операций
        //
        //

        // init
        private void init_with_const_Click(object sender, RoutedEventArgs e)
        {

        }

        private void init_with_var_Click(object sender, RoutedEventArgs e)
        {

        }

        // remove spaces
        private void remove_spaces_Click(object sender, RoutedEventArgs e)
        {

        }

        // concatination
        private void concat_with_const_Click(object sender, RoutedEventArgs e)
        {

        }

        private void concat_with_var_Click(object sender, RoutedEventArgs e)
        {

        }

        // replace symbol
        private void replace_symbol_Click(object sender, RoutedEventArgs e)
        {

        }

        // replace substring
        private void replace_substr_const_const_Click(object sender, RoutedEventArgs e)
        {

        }

        private void replace_substr_const_var_Click(object sender, RoutedEventArgs e)
        {

        }

        private void replace_substr_var_const_Click(object sender, RoutedEventArgs e)
        {

        }

        private void replace_substr_var_var_Click(object sender, RoutedEventArgs e)
        {

        }

        // remove symbol
        private void remove_symbol_Click(object sender, RoutedEventArgs e)
        {

        }

        // add brackets
        private void add_brackets_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
