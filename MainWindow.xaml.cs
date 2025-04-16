using System;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Calc
{
    public partial class MainWindow : Window
    {
        private StringBuilder builder = new StringBuilder();
        private bool resultDisplayed = false;
        private bool error = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClearAll(object sender, RoutedEventArgs e)
        {
            builder.Clear();
            ExpressionDisplay.Text = "";
            ResultDisplay.Text = "0";
            resultDisplayed = false;
            error = false;
        }

        private void Clear(object sender, RoutedEventArgs e)
        {
            if (resultDisplayed)
            {
                ClearAll(sender, e);
            }
            else if (builder.Length > 0)
            {
                builder.Remove(builder.Length - 1, 1);
                UpdateExpressionDisplay();
            }
        }

        private void UpdateExpressionDisplay()
        {
            ExpressionDisplay.Text = builder.ToString();
        }


        private void UpdateResultDisplay(string value)
        {
            ResultDisplay.Text = value;
            resultDisplayed = true;
        }

        private void AddToExpression(string value)
        {
            if (resultDisplayed || error)
            {
                builder.Clear();
                resultDisplayed = false;
                error = false;
            }
            builder.Append(value);
            UpdateExpressionDisplay();
        }

        private void DigitButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            string digit = button.Content.ToString();
            AddToExpression(digit);
        }

        private void OperatorButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            string op = button.Content.ToString();
            AddToExpression(op);
        }


        private void FunctionButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            string function = button.Content.ToString();

            switch (function)
            {
                case "|x|":
                    AddToExpression("abs(");
                    break;
                case "1/x":
                    AddToExpression("1/");
                    break;
                case "√x":
                    AddToExpression("sqrt(");
                    break;
                case "n!":
                    AddToExpression("!");
                    break;
                case "x^2":
                    AddToExpression("^2");
                    break;
                case "x^y":
                    AddToExpression("^");
                    break;
                default:
                    AddToExpression(function);
                    break;
            }
        }


        private void ConstantButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            string constant = button.Content.ToString();
            if (constant == "π")
            {
                AddToExpression(Math.PI.ToString());
            }
            else if (constant == "e")
            {
                AddToExpression(Math.E.ToString());
            }
        }


        private void ParenthesisButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            string parenthesis = button.Content.ToString();
            AddToExpression(parenthesis);
        }

        private void CalculateResult(object sender, RoutedEventArgs e)
        {
            try
            {
                string expression = builder.ToString();
                double result = EvaluateExpression(expression);
                UpdateResultDisplay(result.ToString());
            }
            catch (Exception)
            {
                ResultDisplay.Text = "Ошибка";
                error = true;
            }
        }

        private double EvaluateExpression(string expression)
        {
            if (expression.StartsWith("sin("))
            {
                double angle = double.Parse(expression.Substring(4, expression.Length - 5));
                return Math.Sin(angle);
            }
            else if (expression.StartsWith("cos("))
            {
                double angle = double.Parse(expression.Substring(4, expression.Length - 5));
                return Math.Cos(angle);
            }
            else if (expression.StartsWith("tg("))
            {
                double angle = double.Parse(expression.Substring(3, expression.Length - 4));
                return Math.Tan(angle);
            }
            else if (expression.StartsWith("log("))
            {
                double value = double.Parse(expression.Substring(4, expression.Length - 5));
                return Math.Log10(value);
            }
            else if (expression.StartsWith("ln("))
            {
                double value = double.Parse(expression.Substring(3, expression.Length - 4));
                return Math.Log(value);
            }
            else if (expression.StartsWith("sqrt("))
            {
                double value = double.Parse(expression.Substring(5, expression.Length - 6));
                return Math.Sqrt(value);
            }
            else if (expression.StartsWith("abs("))
            {
                double value = double.Parse(expression.Substring(4, expression.Length - 5));
                return Math.Abs(value);
            }
            else if (expression.StartsWith("1/"))
            {
                double value = double.Parse(expression.Substring(2));
                return 1 / value;
            }
            else if (expression.Contains("!"))
            {
                int value = int.Parse(expression.Substring(0, expression.IndexOf('!')));
                return Factorial(value);
            }
            else if (expression.Contains("^"))
            {
                string[] parts = expression.Split('^');
                double baseValue = double.Parse(parts[0]);
                double exponent = double.Parse(parts[1]);
                return Math.Pow(baseValue, exponent);
            }
            else
            {
                var table = new DataTable();
                table.Columns.Add("expression", typeof(string), expression);
                DataRow row = table.NewRow();
                table.Rows.Add(row);
                return double.Parse((string)row["expression"]);
            }
        }


        private double Factorial(int number)
        {
            if (number == 0) return 1;
            double result = 1;
            for (int i = 1; i <= number; i++)
            {
                result *= i;
            }
            return result;
        }
    }
}
