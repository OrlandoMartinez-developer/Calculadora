using System;
using System.Data;
using System.Drawing;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Threading;
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
        }

        private void PersonaLizarInterfaz()
        {

            this.BackColor = Color.FromArgb(30,30,30);
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            txtDisplay.BackColor = Color.FromArgb(50, 50, 50);
            txtDisplay.ForeColor = Color.White;
            txtDisplay.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            txtDisplay.BorderStyle = BorderStyle.None;
            txtDisplay.TextAlign = HorizontalAlignment.Right;


            foreach (Control control in this.Controls) {

                if (control is Button) { 
                Button btn = (Button)control;
                    btn.BackColor = Color.FromArgb(50, 50, 50);
                    btn.ForeColor = Color.White;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderColor = Color.FromArgb(70, 70, 70);
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(70, 70, 70);
                    btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(90, 90, 90);
                    btn.Font = new Font("Seogi UI", 7,FontStyle.Bold); 
                    btn.Cursor = Cursors.Hand;
                }
            }

            btnIgual.BackColor = Color.FromArgb(0, 120, 215);
            btnIgual.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 150, 255);
            btnIgual.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 90, 175);

            button25.BackColor = Color.FromArgb(215, 0, 0);
            button25.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 50, 50);
            button25.FlatAppearance.MouseDownBackColor = Color.FromArgb(175, 0, 0);

            btnDel.BackColor = Color.FromArgb(215, 0, 0); // Rojo oscuro
            btnDel.ForeColor = Color.White;
            btnDel.Font = new Font("Segoe UI", 8, FontStyle.Bold); // Fuente más grande
            btnDel.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 50, 50); // Rojo más claro al pasar el mouse
            btnDel.FlatAppearance.MouseDownBackColor = Color.FromArgb(175, 0, 0); // Rojo más oscuro al hacer clic
        }

        
        private void AgregarNumero(string numero)
        {
            if (nuevaOperacion)
            {
                // Si se comienza con un número tras una operación, reinicia o continúa la operación
                if (double.TryParse(txtDisplay.Text, out _))
                {
                    txtDisplay.Text = "";
                }
                nuevaOperacion = false;
            }
            if (!string.IsNullOrEmpty(txtDisplay.Text) && txtDisplay.Text[txtDisplay.Text.Length - 1] == ')')
            {
                // Agrega un operador de multiplicación antes del número
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
                txtDisplay.Text += operador;

            }
        }
        private void SeleccionarFuncionEspecial(string funcion)
        {
            // Verifica si el último carácter es un número o un paréntesis de cierre
            if (!string.IsNullOrEmpty(txtDisplay.Text))
            {
                char ultimoCaracter = txtDisplay.Text[txtDisplay.Text.Length - 1];
                if (char.IsDigit(ultimoCaracter) || ultimoCaracter == ')')
                {
                    // Agrega un operador de multiplicación antes de la función
                    txtDisplay.Text += " x ";
                }
            }

            // Agrega la función especial con paréntesis
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

            expresion = EvaluarPotenciacion(expresion);
            // Ahora evalúa la expresión restante con DataTable
            var tabla = new DataTable();
            return Convert.ToDouble(tabla.Compute(expresion, null));
        }

        private string EvaluarPotenciacion(string expresion)
        {
            return Regex.Replace(expresion, @"(\d+(\.\d+)?)\^(\d+(\.\d+)?)", match =>
            {
                double baseNum = Convert.ToDouble(match.Groups[1].Value);
                double exponent = Convert.ToDouble(match.Groups[3].Value);
                return Math.Pow(baseNum, exponent).ToString();
            });
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

        private void button20_Click(object sender, EventArgs e)
        {
           
            SeleccionarFuncionEspecial("tan");
        }

        private void button19_Click(object sender, EventArgs e)
        {
           
            SeleccionarFuncionEspecial("cos");
        }

        private void button18_Click(object sender, EventArgs e)
        {
            SeleccionarFuncionEspecial("sin");
        }

        private void button21_Click(object sender, EventArgs e)
        {
            SeleccionarFuncionEspecial("sqrt");
            string displayText = txtDisplay.Text;
            displayText = displayText.Replace("sqrt(", "√(");
            txtDisplay.Text = displayText;

        }

        private void button22_Click(object sender, EventArgs e)
        {
            SeleccionarFuncionEspecial("log");
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

        private void txtDisplay_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtDisplay.Text))
            {
                // Obtén el último carácter
                string textoActual = txtDisplay.Text;
                char ultimoCaracter = textoActual[textoActual.Length - 1];

                // Si el último carácter es "(", verifica si es parte de una función especial
                if (ultimoCaracter == '(')
                {
                    // Verifica si es una función especial (sin, cos, tan, sqrt, log)
                    if (textoActual.EndsWith("sin("))
                    {
                        txtDisplay.Text = textoActual.Substring(0, textoActual.Length - 4); // Elimina "sin("
                    }
                    else if (textoActual.EndsWith("cos("))
                    {
                        txtDisplay.Text = textoActual.Substring(0, textoActual.Length - 4); // Elimina "cos("
                    }
                    else if (textoActual.EndsWith("tan("))
                    {
                        txtDisplay.Text = textoActual.Substring(0, textoActual.Length - 4); // Elimina "tan("
                    }
                    else if (textoActual.EndsWith("sqrt("))
                    {
                        txtDisplay.Text = textoActual.Substring(0, textoActual.Length - 5); // Elimina "sqrt("
                    }
                    else if (textoActual.EndsWith("log("))
                    {
                        txtDisplay.Text = textoActual.Substring(0, textoActual.Length - 4); // Elimina "log("
                    }
                    else
                    {
                        // Si no es una función especial, solo elimina "("
                        txtDisplay.Text = textoActual.Substring(0, textoActual.Length - 1);
                    }
                }
                else
                {
                    // Si no es "(", simplemente elimina el último carácter
                    txtDisplay.Text = textoActual.Substring(0, textoActual.Length - 1);
                }
            }
        }
    }
}