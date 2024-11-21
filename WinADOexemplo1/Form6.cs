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
    public partial class Form6 : Form
    {
        private SqlConnection connection;
        private SqlDataAdapter dataAdapter;
        private DataTable dataTable;
        private int posCurrente = 0;
        private bool emEditMode = false;

        public int Id { get; set; }    //adicionamos para obter o ID
        public Form6()
        {
            InitializeComponent();
            InicializarDatabase();
        }

        //adicionamos o novo contrutor
        public Form6(int id)
        {
            Id = id;
            InitializeComponent();
            InicializarDatabase();
        }

        private void InicializarDatabase()
        {
            //string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={Application.StartupPath}\\ClubeDB.mdf;Integrated Security=True;Connect Timeout=30";
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB; Initial Catalog=ClubeDB; Integrated Security=True;";
            connection = new SqlConnection(connectionString);
            dataAdapter = new SqlDataAdapter("SELECT * FROM Socios", connection);
            dataTable = new DataTable();
            dataAdapter.Fill(dataTable);

            if (Id > 0) {
                // Aqui vamos usar o ID para obter o registo na tabela de dados
                int position = ObterPosicaoPorId(Id);
                if (position != -1)
                {
                    ExibirDados(position);
                }
                else
                {
                    MessageBox.Show("Registo não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else

            if (dataTable.Rows.Count > 0)
            {
                ExibirDados(0);
            }
        }

        private int ObterPosicaoPorId(int id)
        {
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                if (Convert.ToInt32(dataTable.Rows[i]["ID"]) == id)
                {
                    return i; // Retorna a posição
                }
            }
            return -1; // Retorna -1 se não encontrado
        }

        private void ExibirDados(int position)
        {
            if (position >= 0 && position < dataTable.Rows.Count)
            {
                DataRow row = dataTable.Rows[position];
                TxtID.Text = row["ID"].ToString();
                TxtNome.Text = row["Nome"].ToString();
                TxtEmail.Text = row["Email"].ToString();
                TxtTelefone.Text = row["Telefone"].ToString();
                DtpDataNascimento.Value = Convert.ToDateTime(row["DataNascimento"]);
                DtpDataEntrada.Value = Convert.ToDateTime(row["DataEntrada"]);
                posCurrente = position;
                AtivarControlos(false);
            }
        }

        private void AtivarControlos(bool enable)
        {
            TxtID.Enabled = enable;
            TxtNome.Enabled = enable;
            TxtEmail.Enabled = enable;
            TxtTelefone.Enabled = enable;
            DtpDataNascimento.Enabled = enable;
            DtpDataEntrada.Enabled = enable;
        }

        private void BtnInicio_Click(object sender, EventArgs e)
        {
            ExibirDados(0);
        }

        private void BtnAnterior_Click(object sender, EventArgs e)
        {
            if (posCurrente > 0)
            {
                ExibirDados(posCurrente - 1);
            }

        }

        private void BtnProximo_Click(object sender, EventArgs e)
        {
            if (posCurrente < dataTable.Rows.Count - 1)
            {
                ExibirDados(posCurrente + 1);
            }

        }

        private void BtnUltimo_Click(object sender, EventArgs e)
        {
            ExibirDados(dataTable.Rows.Count - 1);

        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (emEditMode)
            {
                AtualizarRegisto();
            }
            else
            {
                InserirRegisto();
            }
            emEditMode = false;
            AtivarControlos(false);

        }

        private bool ValidarInput()
        {
            if (string.IsNullOrWhiteSpace(TxtNome.Text) || string.IsNullOrWhiteSpace(TxtEmail.Text) || string.IsNullOrWhiteSpace(TxtTelefone.Text))
            {
                MessageBox.Show("Por favor, preencha todos os campos obrigatórios.", "Campos obrigatórios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void AtualizarRegisto()
        {
            if (ValidarInput())
            {
                DataRow row = dataTable.Rows[posCurrente];
                row["Nome"] = TxtNome.Text;
                row["Email"] = TxtEmail.Text;
                row["Telefone"] = TxtTelefone.Text;
                row["DataNascimento"] = DtpDataNascimento.Value;
                row["DataEntrada"] = DtpDataEntrada.Value;

                SqlCommandBuilder builder = new SqlCommandBuilder(dataAdapter);
                dataAdapter.Update(dataTable);

                MessageBox.Show("Sócio atualizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void InserirRegisto()
        {
            if (ValidarInput())
            {
                DataRow newRow = dataTable.NewRow();
                newRow["Nome"] = TxtNome.Text;
                newRow["Email"] = TxtEmail.Text;
                newRow["Telefone"] = TxtTelefone.Text;
                newRow["DataNascimento"] = DtpDataNascimento.Value;
                newRow["DataEntrada"] = DtpDataEntrada.Value;

                dataTable.Rows.Add(newRow);
                SqlCommandBuilder builder = new SqlCommandBuilder(dataAdapter);
                dataAdapter.Update(dataTable);

                MessageBox.Show("Novo sócio adicionado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (dataTable.Rows.Count == 0)
            {
                MessageBox.Show("Não há registos para eliminar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show("Tem certeza que deseja eliminar este sócio?", "Confirmar Eliminação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                EliminarRegisto();
            }
        }
    

        private void EliminarRegisto()
        {
            try
            {
                dataTable.Rows[posCurrente].Delete();
                SqlCommandBuilder builder = new SqlCommandBuilder(dataAdapter);
                dataAdapter.Update(dataTable);

                MessageBox.Show("Sócio eliminado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Atualizar a exibição após a eliminação
                if (dataTable.Rows.Count > 0)
                {
                    if (posCurrente >= dataTable.Rows.Count)
                    {
                        posCurrente = dataTable.Rows.Count - 1;
                    }
                    ExibirDados(posCurrente);
                }
                else
                {
                    LimparCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao eliminar o sócio: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            emEditMode = false;
            AtivarControlos(false);
            ExibirDados(posCurrente);
        }



        private void BtnEditar_Click(object sender, EventArgs e)
        {
            emEditMode = true;
            AtivarControlos(true);
        }

        private void BtnNovo_Click(object sender, EventArgs e)
        {
            // Ativar o modo de edição
            emEditMode = true;

            // Limpar os campos
            LimparCampos();

            // Habilitar os controles para edição
            AtivarControlos(true);

            // Desabilitar os botões de navegação
            AlternarBotoesNavegacao(false);

            // Focar no primeiro campo
            TxtNome.Focus();
        }

        private void AlternarBotoesNavegacao(bool enabled)
        {
            BtnInicio.Enabled = enabled;
            BtnAnterior.Enabled = enabled;
            BtnProximo.Enabled = enabled;
            BtnUltimo.Enabled = enabled;
        }

    }
   
}
