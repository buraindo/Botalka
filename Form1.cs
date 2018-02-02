using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Botalka
{
    public partial class Form1 : Form
    {
        public readonly BotalkaProgram program = new BotalkaProgram();
        private int cur = 0;
        public Form1()
        {
            InitializeComponent();
            program.form = this;
            FormClosing += Form1_FormClosing;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var runTask = Task.Run(() => program.Run());
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!program.canClose)
            {
                e.Cancel = true;
                MessageBox.Show(BotalkaProgram.PhrasesList[cur]);
                cur += 1;
                cur %= BotalkaProgram.PhrasesList.Count;
            }
        }
    }
}
