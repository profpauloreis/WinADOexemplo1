using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinADOexemplo1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void ConfigurarDataGridView()
        {
            // Configurações básicas
            dataGridViewSocios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewSocios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewSocios.AllowUserToAddRows = false;
            dataGridViewSocios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewSocios.MultiSelect = false;

            // Estilo
            dataGridViewSocios.BackgroundColor = Color.LightGray;
            dataGridViewSocios.DefaultCellStyle.BackColor = Color.White;
            dataGridViewSocios.DefaultCellStyle.ForeColor = Color.Black;
            dataGridViewSocios.DefaultCellStyle.Font = new Font("Arial", 10);
            dataGridViewSocios.AlternatingRowsDefaultCellStyle.BackColor = Color.LightBlue;

            // Cabeçalho
            dataGridViewSocios.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dataGridViewSocios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewSocios.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 12, FontStyle.Bold);
        }




        private void btnConsultar_Click(object sender, EventArgs e)
        {
            //string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\ClubeDB.mdf;Integrated Security=True;Connect Timeout=30";
            string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={Application.StartupPath}\\ClubeDB.mdf;Integrated Security=True;Connect Timeout=30";
            using (SqlConnection connection = new SqlConnection(connectionString))
                
            {
                try
                {
                    connection.Open();
                    string query = "SELECT ID, Nome, DataNascimento, Email, Telefone, DataEntrada FROM Socios";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable sociosTable = new DataTable();
                    adapter.Fill(sociosTable);
                    dataGridViewSocios.DataSource = sociosTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro: {ex.Message}");
                }

            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            ConfigurarDataGridView();
        }

        private void TsBtnNrAsc_Click(object sender, EventArgs e)
        {
            //ordenar a datagridview por id ascendente
            dataGridViewSocios.Sort(dataGridViewSocios.Columns["ID"], ListSortDirection.Ascending);


        }

        private void TsBtnNrDesc_Click(object sender, EventArgs e)
        {
            //ordenar a datagridview por id descendente
            dataGridViewSocios.Sort(dataGridViewSocios.Columns["ID"], ListSortDirection.Descending);
        }

        private void TsNtnNomeAsc_Click(object sender, EventArgs e)
        {
            dataGridViewSocios.Sort(dataGridViewSocios.Columns["Nome"], ListSortDirection.Ascending);
        }

        private void TsBtnNomeDesc_Click(object sender, EventArgs e)
        {
            dataGridViewSocios.Sort(dataGridViewSocios.Columns["Nome"], ListSortDirection.Descending);
        }

        //pesquisa por nome
        

        private void TsBtnPesquisar_Click_1(object sender, EventArgs e)
        {
            // Obtém o texto digitado na caixa de pesquisa
            string nome = TsPesquisar.Text;
            MessageBox.Show($"Texto pesquisado: {nome}"); // Para verificar o que está sendo pesquisado

            // Define a string de conexão com a base de dados
            string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={Application.StartupPath}\\ClubeDB.mdf;Integrated Security=True;Connect Timeout=30";

            // Cria uma nova conexão SQL usando a string de conexão
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    // Tenta abrir a conexão com a base de dados
                    connection.Open();

                    //// Define a query SQL para buscar sócios cujo nome contenha o texto pesquisado
                    //string query = $"SELECT ID, Nome, DataNascimento, Email, Telefone, DataEntrada FROM Socios WHERE Nome LIKE '%{nome}%'";

                    //// Cria um adaptador SQL com a query e a conexão
                    //SqlDataAdapter adapter = new SqlDataAdapter(query, connection);

                    string query = "SELECT ID, Nome, DataNascimento, Email, Telefone, DataEntrada FROM Socios WHERE Nome LIKE @nome";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@nome", $"%{nome}%");
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    // Cria uma nova DataTable para armazenar os resultados
                    DataTable sociosTable = new DataTable();

                    // Preenche a DataTable com os resultados da query
                    adapter.Fill(sociosTable);

                    // Define a fonte de dados da DataGridView como a DataTable
                    dataGridViewSocios.DataSource = sociosTable;

                    // Exibe uma mensagem com o número de registos encontrados (opcional)
                    MessageBox.Show($"Número de registos encontrados: {sociosTable.Rows.Count}"); // Para verificar se retornou dados
                }
                catch (Exception ex)
                {
                    // Exibe uma mensagem de erro caso ocorra uma exceção
                    MessageBox.Show($"Erro: {ex.Message}");
                }
            }
            // O bloco 'using' garante que a conexão será fechada automaticamente após o uso
        }

        private void TsBtnNovo_Click(object sender, EventArgs e)
        {
            // Cria uma nova instância do Form5 (formulário de registo de sócio)
            Form5 form5 = new Form5();
            form5.ShowDialog();

        }

        private void TsBtnGerir_Click(object sender, EventArgs e)
        {
            // Cria uma nova instância do Form6 (formulário de crud de sócios)
            Form6 form6 = new Form6();
            form6.ShowDialog();
        }
    }
}
