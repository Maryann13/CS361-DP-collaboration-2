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
using Core;

namespace Interface
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal Dictionary<string, string> var_dict = new Dictionary<string, string>();

        private Calc calculate_window = new Calc();

        private Variables var_window = new Variables();

        internal Formula formula;
      
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
        // основные кнопки
        //
        //

        private void calculate_Click(object sender, RoutedEventArgs e)
        {
            calculate_window.ShowDialog();
        }

        private void del_last_Click(object sender, RoutedEventArgs e)
        {
            if (formula == null)
                return;

            if (formula is Const || formula is Var)
            {
                formula = null;
                operations.SelectedItem = init;
                calculate.IsEnabled = false;
                del_last.IsEnabled = false;
                clear_formula.IsEnabled = false;
                formula_text.Content = "";
            }
            else
            {
                string f_string = formula_text.Content as string;

                if (formula is RemoveSpacesDecorator)
                    formula_text.Content = f_string.Substring(3, f_string.Length - 4);
                else if (formula is ConcatDecorator)
                    formula_text.Content = f_string.Substring(0, f_string.LastIndexOf("&&"));
                else if (formula is ReplaceSpaceDecorator)
                    formula_text.Content = f_string.Substring(0, f_string.LastIndexOf('#'));
                else if (formula is ReplaceSubstringDecorator)
                    formula_text.Content = f_string.Substring(0, f_string.LastIndexOf('@'));
                else if (formula is CharsFreqRemoveAdapter)
                    formula_text.Content = f_string.Substring(0, f_string.LastIndexOf('!'));
                else
                    formula_text.Content = f_string.Substring(1, f_string.Length - 2);

                formula = (formula as FormulaDecorator).Subformula;
            }

        }

        private void clear_formula_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in operations.Items)
            {
                if (item is TabItem)
                {
                    if ((item as TabItem).Name != "init")
                    {
                        (item as TabItem).IsEnabled = false;
                    }
                    else
                        (item as TabItem).IsEnabled = true;
                }
            }
            operations.SelectedItem = init;
            calculate.IsEnabled = false;
            del_last.IsEnabled = false;
            clear_formula.IsEnabled = false;

            formula = null;
            formula_text.Content = "";
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
            foreach (var item in operations.Items)
            {
                if (item is TabItem)
                {
                    if ((item as TabItem).Name == "init")
                    {
                        (item as TabItem).IsEnabled = false;
                        operations.SelectedItem = removesp;
                    }
                    else
                        (item as TabItem).IsEnabled = true;
                }
            }
            calculate.IsEnabled = true;
            del_last.IsEnabled = true;
            clear_formula.IsEnabled = true;

            formula = new Const(init_const.Text);
            formula_text.Content = init_const.Text;
            init_const.Text = "";
        }

        private void init_with_var_Click(object sender, RoutedEventArgs e)
        {
            if (init_var.Text.Length == 0 || init_var.Text.Contains(" "))
            {
                MessageBox.Show("Not valid variable name!");
                return;
            }

            foreach (var item in operations.Items)
            {
                if (item is TabItem)
                {
                    if ((item as TabItem).Name == "init")
                    {
                        (item as TabItem).IsEnabled = false;
                        operations.SelectedItem = removesp;
                    }
                    else
                        (item as TabItem).IsEnabled = true;
                }
            }
            calculate.IsEnabled = true;
            del_last.IsEnabled = true;
            clear_formula.IsEnabled = true;

            formula = new Var(init_var.Text);
            formula_text.Content = init_var.Text;
            init_var.Text = "";
        }

        // remove spaces
        private void remove_spaces_Click(object sender, RoutedEventArgs e)
        {
            var f = new RemoveSpacesDecorator();
            f.Subformula = formula;
            formula = f;

            formula_text.Content = string.Format("(^ {0})", formula_text.Content);
        }

        // concatination
        private void concat_with_const_Click(object sender, RoutedEventArgs e)
        {
            var f = new ConcatDecorator(new Const(concat_const.Text));
            f.Subformula = formula;
            formula = f;

            formula_text.Content = string.Format("{0} && {1}",
                formula_text.Content, concat_const.Text);
            concat_const.Text = "";
        }

        private void concat_with_var_Click(object sender, RoutedEventArgs e)
        {
            if (concat_var.Text.Length == 0 || concat_var.Text.Contains(" "))
            {
                MessageBox.Show("Not valid variable name!");
                return;
            }

            var f = new ConcatDecorator(new Var(concat_var.Text));
            f.Subformula = formula;
            formula = f;

            formula_text.Content = string.Format("{0} && {1}",
                formula_text.Content, concat_var.Text);
            concat_var.Text = "";
        }

        // replace symbol
        private void replace_symbol_Click(object sender, RoutedEventArgs e)
        {
            if (replace_symb.Text.Length == 1)
            {
                var f = new ReplaceSpaceDecorator(replace_symb.Text[0]);
                f.Subformula = formula;
                formula = f;

                formula_text.Content = string.Format("{0} # {1}",
                    formula_text.Content, replace_symb.Text);
                replace_symb.Text = "";
            }
        }

        // replace substring
        private void replace_substr_const_const_Click(object sender, RoutedEventArgs e)
        {
            var f = new ReplaceSubstringDecorator
                    (new Const(cc_1.Text), new Const(cc_2.Text));
            f.Subformula = formula;
            formula = f;

            formula_text.Content = string.Format("{0} @ ({1}, {2})",
                formula_text.Content, cc_1.Text, cc_2.Text);
            cc_1.Text = "";
            cc_2.Text = "";
        }

        private void replace_substr_const_var_Click(object sender, RoutedEventArgs e)
        {
            if (cv_2.Text.Length == 0 || cv_2.Text.Contains(" "))
            {
                MessageBox.Show("Not valid variable name!");
                return;
            }

            var f = new ReplaceSubstringDecorator
                (new Const(cv_1.Text), new Var(cv_2.Text));
            f.Subformula = formula;
            formula = f;

            formula_text.Content = string.Format("{0} @ ({1}, {2})",
                formula_text.Content, cv_1.Text, cv_2.Text);
            cv_1.Text = "";
            cv_2.Text = "";
        }

        private void replace_substr_var_const_Click(object sender, RoutedEventArgs e)
        {
            if (vc_1.Text.Length == 0 || vc_1.Text.Contains(" "))
            {
                MessageBox.Show("Not valid variable name!");
                return;
            }

            var f = new ReplaceSubstringDecorator
                (new Var(vc_1.Text), new Const(vc_2.Text));
            f.Subformula = formula;
            formula = f;

            formula_text.Content = string.Format("{0} @ ({1}, {2})",
                formula_text.Content, vc_1.Text, vc_2.Text);
            vc_1.Text = "";
            vc_2.Text = "";
        }

        private void replace_substr_var_var_Click(object sender, RoutedEventArgs e)
        {
            if (vv_1.Text.Length == 0 || vv_1.Text.Contains(" ") ||
                vv_2.Text.Length == 0 || vv_2.Text.Contains(" "))
            {
                MessageBox.Show("Not valid variable name!");
                return;
            }

            var f = new ReplaceSubstringDecorator
                (new Var(vv_1.Text), new Var(vv_2.Text));
            f.Subformula = formula;
            formula = f;

            formula_text.Content = string.Format("{0} @ ({1}, {2})",
                formula_text.Content, vv_1.Text, vv_2.Text);
            vv_1.Text = "";
            vv_2.Text = "";
        }

        // remove symbol
        private void remove_symbol_Click(object sender, RoutedEventArgs e)
        {
            var f = new CharsFreqRemoveAdapter(new Const(remove_symb.Text));
            f.Subformula = formula;
            formula = f;

            formula_text.Content = string.Format("{0} ! {1}",
                formula_text.Content, remove_symb.Text);
            remove_symb.Text = "";
        }

        // add brackets
        private void add_brackets_Click(object sender, RoutedEventArgs e)
        {
            var f = new ParenthesesDecorator();
            f.Subformula = formula;
            formula = f;

            formula_text.Content = string.Format("({0})", formula_text.Content);
        }

        private void replace_symb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            replace_symb.Text = "";
        }
    }
}
