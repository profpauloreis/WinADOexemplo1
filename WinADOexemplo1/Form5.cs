using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinADOexemplo1
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {

            //verificar se os campos estão preenchidos
            if (TxtNome.Text == "" || TxtEmail.Text == "" || TxtTelefone.Text == "")
            {
                MessageBox.Show("Por favor, preencha pelo menos o nome, email e telefone.", "Campos obrigatórios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Exemplo de criação de objeto Socio (precisamos de criar esta classe)
            Socio NovoSocio = new Socio
            {
                Nome = TxtNome.Text,
                Email = TxtEmail.Text,
                Telefone = TxtTelefone.Text,
                DataNascimento = DtpDataNascimento.Value,
                DataEntrada = DtpDataEntrada.Value
            };


            // Criar a string de conexão
            string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={Application.StartupPath}\\ClubeDB.mdf;Integrated Security=True;Connect Timeout=30";

            // Criar a consulta SQL para inserir o novo sócio
            string query = "INSERT INTO Socios (Nome, Email, Telefone, DataNascimento, DataEntrada) VALUES (@Nome, @Email, @Telefone, @DataNascimento, @DataEntrada)";

            // Criar e abrir a conexão com a base de dados
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Criar o comando SQL com os parâmetros
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nome", NovoSocio.Nome);
                    command.Parameters.AddWithValue("@Email", NovoSocio.Email);
                    command.Parameters.AddWithValue("@Telefone", NovoSocio.Telefone);
                    command.Parameters.AddWithValue("@DataNascimento", NovoSocio.DataNascimento);
                    command.Parameters.AddWithValue("@DataEntrada", NovoSocio.DataEntrada);

                    // Executar o comando
                    int result = command.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Sócio adicionado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // Limpar os campos do formulário após salvar
                        LimparCampos();
                    }
                    else
                    {
                        MessageBox.Show("Erro ao adicionar sócio.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void LimparCampos()
        {
            TxtNome.Text = "";
            TxtEmail.Text = "";
            TxtTelefone.Text = "";
            DtpDataNascimento.Value = DateTime.Now;
            DtpDataEntrada.Value = DateTime.Now;
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    internal class Socio
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataEntrada { get; set; }
    }
}
