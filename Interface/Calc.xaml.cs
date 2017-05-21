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
    /// Логика взаимодействия для Calc.xaml
    /// </summary>
    public partial class Calc : Window
    {
        public Calc()
        {
            InitializeComponent();
        }

        private void calculate_Click(object sender, RoutedEventArgs e)
        {
            /*var vars = (Owner as MainWindow).variables.Items;
            var listbox = new ListBox(); 
            foreach (var v in vars)
                listbox.Items.Add(v);
            variables.Children.Add(listbox);*/
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            variables.Children.Clear();
            var var_dict = (Owner as MainWindow).var_dict;
            //ListBox lb = new ListBox();
            foreach (var v in var_dict.Keys)
            {
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                sp.HorizontalAlignment = HorizontalAlignment.Right;
                Label l = new Label();
                l.Content = v;
                l.Margin = new Thickness(5);
                TextBox tb = new TextBox();
                tb.Name = "textbox_" + v;
                tb.Width = 70;
                tb.Margin = new Thickness(5);
                sp.Children.Add(l);
                sp.Children.Add(tb);
                variables.Children.Add(sp);
                //variables.Children.Add();
            }
            //variables.Children.Add(lb);
            //variables.Children.Add(lb);
            //variables.ScrollOwner = scroll;
            //variables.Children.Add(lb);
        }
    }
}
