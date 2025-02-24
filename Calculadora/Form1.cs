using System;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Calculadora
{
    public partial class Calculadora : Form
    {
        private bool nuevaOperacion = false;

        public Calculadora()
        {
            InitializeComponent();
            PersonaLizarInterfaz();
            txtDisplay.KeyPress += Calculadora_KeyPress; 
            txtDisplay.TextChanged += txtDisplay_TextChanged; 
        }

        private void AgregarNumero(string numero)
        {
            if (nuevaOperacion)
            {
                if (double.TryParse(txtDisplay.Text, out _))
                {
                    txtDisplay.Text = "";
                }
                nuevaOperacion = false;
            }
            if (!string.IsNullOrEmpty(txtDisplay.Text) && txtDisplay.Text[txtDisplay.Text.Length - 1] == ')')
            {
                txtDisplay.Text += " x ";
            }
            txtDisplay.Text += numero;
        }

        private void SeleccionarOperador(string operador)
        {
            if (nuevaOperacion)
            {
                nuevaOperacion = false;
            }

            if (!string.IsNullOrWhiteSpace(txtDisplay.Text))
            {
                char ultimoCaracter = txtDisplay.Text[txtDisplay.Text.Length - 1];

                // Permitir el uso de '-' para números negativos al inicio o después de '('
                if (operador == " - " && (ultimoCaracter == '(' || string.IsNullOrWhiteSpace(txtDisplay.Text)))
                {
                    txtDisplay.Text += "-";
                }
                // Permitir el uso de '-' después de otro operador para números negativos
                else if (operador == " - " && EsOperador(ultimoCaracter.ToString()) && ultimoCaracter != '-')
                {
                    txtDisplay.Text += "-";
                }
                // Evitar dos operadores seguidos (excepto para el signo negativo)
                else if (!EsOperador(ultimoCaracter.ToString()) || (operador == " - " && EsOperador(ultimoCaracter.ToString())))
                {
                    txtDisplay.Text += operador;
                }
            }
            // Permitir el primer operador si esta vacio el display
            else if (operador == " - ")
            {
                txtDisplay.Text = "-";
            }
        }

        private bool EsOperador(string caracter)
        {
            return caracter == "+" || caracter == "-" || caracter == "x" || caracter == "÷" || caracter == "^";
        }
        private void SeleccionarFuncionEspecial(string funcion)
        {
            if (!string.IsNullOrEmpty(txtDisplay.Text))
            {
                char ultimoCaracter = txtDisplay.Text[txtDisplay.Text.Length - 1];
                if (char.IsDigit(ultimoCaracter) || ultimoCaracter == ')')
                {
                    txtDisplay.Text += " x ";
                }
            }

            txtDisplay.Text += funcion + "(";
        }

        private void EvaluarExpresion()
        {
            try
            {
                string expresion = txtDisplay.Text
                    .Replace("÷", "/")
                    .Replace("√(", "sqrt(")
                    .Replace("x", "*");

                double resultado = EvaluarFuncionesMatematicas(expresion);
                txtDisplay.Text = resultado.ToString();
                nuevaOperacion = true;
            }
            catch (DivideByZeroException)
            {
                MessageBox.Show("Error: División por cero no permitida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error en la expresión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private double EvaluarFuncionesMatematicas(string expresion)
        {
            expresion = Regex.Replace(expresion, @"sin\(([^)]+)\)", match =>
                Math.Sin(EvaluarFuncionesMatematicas(match.Groups[1].Value) * Math.PI / 180).ToString());

            expresion = Regex.Replace(expresion, @"cos\(([^)]+)\)", match =>
                Math.Cos(EvaluarFuncionesMatematicas(match.Groups[1].Value) * Math.PI / 180).ToString());

            expresion = Regex.Replace(expresion, @"tan\(([^)]+)\)", match =>
                Math.Tan(EvaluarFuncionesMatematicas(match.Groups[1].Value) * Math.PI / 180).ToString());

            expresion = Regex.Replace(expresion, @"sqrt\(([^)]+)\)", match =>
                Math.Sqrt(EvaluarFuncionesMatematicas(match.Groups[1].Value)).ToString());

            expresion = Regex.Replace(expresion, @"log\(([^)]+)\)", match =>
                Math.Log10(EvaluarFuncionesMatematicas(match.Groups[1].Value)).ToString());

            expresion = EvaluarPotenciacion(expresion);

            var tabla = new DataTable();
            return Convert.ToDouble(tabla.Compute(expresion, null));
        }

        private string EvaluarPotenciacion(string expresion)
        {
            while (Regex.IsMatch(expresion, @"(\d+(\.\d+)?|\([^()]+\))\^(\d+(\.\d+)?|\([^()]+\))"))
            {
                expresion = Regex.Replace(expresion, @"(\d+(\.\d+)?|\([^()]+\))\^(\d+(\.\d+)?|\([^()]+\))", match =>
                {
                    string baseStr = match.Groups[1].Value;
                    string exponentStr = match.Groups[3].Value;

                    double baseNum = EvaluarFuncionesMatematicas(baseStr);
                    double exponentNum = EvaluarFuncionesMatematicas(exponentStr);

                    return Math.Pow(baseNum, exponentNum).ToString();
                });
            }
            return expresion;
        }

        private void Form1_Load(object sender, EventArgs e)
        { }

        private void button1_Click_1(object sender, EventArgs e)
        {
            EvaluarExpresion();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!txtDisplay.Text.Contains("."))
                txtDisplay.Text += ".";
        }

        private void button4_Click_1(object sender, EventArgs e) => SeleccionarOperador(" ÷ ");

        private void button3_Click_1(object sender, EventArgs e) => SeleccionarOperador(" - ");

        private void button8_Click(object sender, EventArgs e) => AgregarNumero("1");

        private void button9_Click(object sender, EventArgs e) => AgregarNumero("2");

        private void button23_Click(object sender, EventArgs e)
        {
            txtDisplay.Text += "(";
        }

        private void button7_Click(object sender, EventArgs e) => AgregarNumero("0");

        private void button11_Click(object sender, EventArgs e) => AgregarNumero("4");

        private void button14_Click(object sender, EventArgs e) => AgregarNumero("7");

        private void btn3_Click(object sender, EventArgs e) => AgregarNumero("3");

        private void btn5_Click(object sender, EventArgs e) => AgregarNumero("5");

        private void btn6_Click(object sender, EventArgs e) => AgregarNumero("6");

        private void btn8_Click(object sender, EventArgs e) => AgregarNumero("8");

        private void btn9_Click(object sender, EventArgs e) => AgregarNumero("9");

        private void btnMas_Click(object sender, EventArgs e) => SeleccionarOperador(" + ");

        private void button6_Click(object sender, EventArgs e) => SeleccionarOperador(" x ");

        private void button17_Click(object sender, EventArgs e) => SeleccionarOperador("^");

        private void button20_Click(object sender, EventArgs e) => SeleccionarFuncionEspecial("tan");

        private void button19_Click(object sender, EventArgs e) => SeleccionarFuncionEspecial("cos");

        private void button18_Click(object sender, EventArgs e) => SeleccionarFuncionEspecial("sin");

        private void button21_Click(object sender, EventArgs e)
        {
            SeleccionarFuncionEspecial("sqrt");
            string displayText = txtDisplay.Text;
            displayText = displayText.Replace("sqrt(", "√(");
            txtDisplay.Text = displayText;
        }

        private void button22_Click(object sender, EventArgs e) => SeleccionarFuncionEspecial("log");

        private void button25_Click_1(object sender, EventArgs e)
        {
            txtDisplay.Text = "";
            nuevaOperacion = false;
        }

        private void button24_Click_1(object sender, EventArgs e)
        {
            txtDisplay.Text += ")";
        }

        private void txtDisplay_TextChanged(object sender, EventArgs e)
        {
            int maxLength = 20; // Define la longitud máxima deseada
            if (txtDisplay.Text.Length > maxLength)
            {
                txtDisplay.Text = txtDisplay.Text.Substring(0, maxLength);
                txtDisplay.SelectionStart = maxLength; // Mueve el cursor al final
            }
        }

        private void Calculadora_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '0': btn0.PerformClick(); break;
                case '1': btn1.PerformClick(); break;
                case '2': btn2.PerformClick(); break;
                case '3': btn3.PerformClick(); break;
                case '4': btn4.PerformClick(); break;
                case '5': btn5.PerformClick(); break;
                case '6': btn6.PerformClick(); break;
                case '7': btn7.PerformClick(); break;
                case '8': btn8.PerformClick(); break;
                case '9': btn9.PerformClick(); break;
                case '+': btnMas.PerformClick(); break;
                case '-': btnMenos.PerformClick(); break;
                case '*': btnMultiplicar.PerformClick(); break;
                case '/': btnDividir.PerformClick(); break;
                case '.': btnP.PerformClick(); break;
                case '=':
                case (char)13: // Enter key
                    btnIgual.PerformClick();
                    break;
                case (char)8: // Backspace
                    btnDel.PerformClick();
                    break;
                case '(': button23.PerformClick(); break;
                case ')': button24.PerformClick(); break;
                case '^': button17.PerformClick(); break;
                default:
                    break;
            }
            e.Handled = true; // Evita que el carácter se muestre en el TextBox
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDisplay.Text))
            {
                string textoActual = txtDisplay.Text;
                char ultimoCaracter = textoActual[textoActual.Length - 1];

                if (ultimoCaracter == '(')
                {
                    if (textoActual.EndsWith("sin("))
                    {
                        txtDisplay.Text = textoActual.Substring(0, textoActual.Length - 4);
                    }
                    else if (textoActual.EndsWith("cos("))
                    {
                        txtDisplay.Text = textoActual.Substring(0, textoActual.Length - 4);
                    }
                    else if (textoActual.EndsWith("tan("))
                    {
                        txtDisplay.Text = textoActual.Substring(0, textoActual.Length - 4);
                    }
                    else if (textoActual.EndsWith("sqrt("))
                    {
                        txtDisplay.Text = textoActual.Substring(0, textoActual.Length - 5);
                    }
                    else if (textoActual.EndsWith("log("))
                    {
                        txtDisplay.Text = textoActual.Substring(0, textoActual.Length - 4);
                    }
                    else
                    {
                        txtDisplay.Text = textoActual.Substring(0, textoActual.Length - 1);
                    }
                }
                else
                {
                    txtDisplay.Text = textoActual.Substring(0, textoActual.Length - 1);
                }
            }
        }
    }
}