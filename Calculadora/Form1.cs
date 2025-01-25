using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Calculadora
{
    public partial class Form1 : Form
    {
        private bool nuevaOperacion = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void AgregarNumero(string numero)
        {
            if (nuevaOperacion)
            {
                txtDisplay.Text = "";
                nuevaOperacion = false;
            }
            txtDisplay.Text += numero;
        }

        private void SeleccionarOperador(string operador)
        {
            if (!string.IsNullOrWhiteSpace(txtDisplay.Text) && !nuevaOperacion)
            {
                txtDisplay.Text += operador;
                nuevaOperacion = false;
            }
        }

        private void EvaluarExpresion()
        {
            try
            {
                string expresion = txtDisplay.Text;
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
            // Evaluar funciones matemáticas avanzadas
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

            // Ahora evalúa la expresión restante con DataTable
            var tabla = new DataTable();
            return Convert.ToDouble(tabla.Compute(expresion, null));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Inicializaciones o código que deseas ejecutar al cargar el formulario
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            EvaluarExpresion();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!txtDisplay.Text.Contains("."))
                txtDisplay.Text += ".";
        }

        private void button4_Click_1(object sender, EventArgs e) => SeleccionarOperador("/");

        private void button3_Click_1(object sender, EventArgs e) => SeleccionarOperador("-");

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

        private void btnMas_Click(object sender, EventArgs e) => SeleccionarOperador("+");

        private void button6_Click(object sender, EventArgs e) => SeleccionarOperador("*");

        private void button17_Click(object sender, EventArgs e) => SeleccionarOperador("^");

        private void button20_Click(object sender, EventArgs e)
        {
            txtDisplay.Text += "tan(";
        }

        private void button19_Click(object sender, EventArgs e)
        {
            txtDisplay.Text += "cos(";
        }

        private void button18_Click(object sender, EventArgs e)
        {
            txtDisplay.Text += "sin(";
        }

        private void button21_Click(object sender, EventArgs e)
        {
            txtDisplay.Text += "sqrt(";
        }

        private void button22_Click(object sender, EventArgs e)
        {
            txtDisplay.Text += "log(";
        }

        private void button25_Click_1(object sender, EventArgs e)
        {
            txtDisplay.Text = "";
            nuevaOperacion = false;
        }

        private void button24_Click_1(object sender, EventArgs e)
        {
            txtDisplay.Text += ")";
        }
    }
}