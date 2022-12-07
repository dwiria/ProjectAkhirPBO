using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Npgsql;
using TODOLISTSMART_V1;

namespace TODOLISTSMART
{
    public partial class HalamanUtama : Form
    {

        private string id = "";
        private int intRow = 0;

        public HalamanUtama()
        {
            InitializeComponent();
            resetMe();
        }

        private void resetMe()
        {
            this.id = string.Empty;

            nametaskTextBox.Text = "";
            deadlineTextBox.Text = "";
            descTextBox.Text = "";

            if (statusComBox.Items.Count > 0)
            {
                statusComBox.SelectedIndex = 0;
            }

            updateButton.Text = "Edit";
            deleteButton.Text = "Delete";

            keywordtextBox.Clear();

            if (keywordtextBox.CanSelect)
            {
                keywordtextBox.Select();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadData("");
        }

        private void loadData(string keyword)
        {
            NpgsqlConnection connection = new NpgsqlConnection();
            string constr = "Server=localhost;Port=5432;User Id=postgres;Password=111222;Database=todolist";
            connection.ConnectionString = constr;
            connection.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select * from todo";
            NpgsqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(dr);
                dgv11.DataSource = dt;
                //dgv.AutoResizeColumns();
                foreach (DataGridViewColumn column in dgv11.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

            }
            cmd.Dispose();
            connection.Close();
            toolStripStatusLabel1.Text = "Number of row(s): " + intRow.ToString();

        }

        private void execute(string mySQL, string param)
        {
            CRUD.cmd = new NpgsqlCommand(mySQL, CRUD.con);
            addParameters(param);
            CRUD.PerformCRUD(CRUD.cmd);
        }

        private void addParameters(string str)
        {
            CRUD.cmd.Parameters.Clear();
            CRUD.cmd.Parameters.AddWithValue("name_task", nametaskTextBox.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("deadline", deadlineTextBox.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("description", descTextBox.Text.Trim());
            CRUD.cmd.Parameters.AddWithValue("status", statusComBox.SelectedItem.ToString());

            if (str == "Edit" || str == "Delete" || str == "Search" && !string.IsNullOrEmpty(this.id))
            {
                CRUD.cmd.Parameters.AddWithValue("id", this.id);
            }
        }

        private void insertButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nametaskTextBox.Text.Trim()) || string.IsNullOrEmpty(deadlineTextBox.Text.Trim()) ||
               string.IsNullOrEmpty(descTextBox.Text.Trim()))
            {
                MessageBox.Show("Harap masukkan data.", "Tambah Data", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            CRUD.sql = "INSERT INTO todo(name_task, deadline, description, status) VALUES(@name_task, @deadline, @description, @status)";

            execute(CRUD.sql, "Add");

            MessageBox.Show("Data berhasil dimasukkan", "Insert Data",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            loadData("");


            resetMe();

        }

        private void dgv1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex != -1)
            {
                DataGridView dgv1 = dgv11;

                this.id = Convert.ToString(dgv1.CurrentRow.Cells[0].Value);
                updateButton.Text = "Edit (" + this.id + ")";
                deleteButton.Text = "Delete (" + this.id + ")";

                nametaskTextBox.Text = Convert.ToString(dgv1.CurrentRow.Cells[1].Value);
                deadlineTextBox.Text = Convert.ToString(dgv1.CurrentRow.Cells[2].Value);
                descTextBox.Text = Convert.ToString(dgv1.CurrentRow.Cells[3].Value);
                statusComBox.SelectedItem = Convert.ToString(dgv1.CurrentRow.Cells[4].Value);
            }

        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (dgv11.Rows.Count == 0)
            {
                return;
            }

            if (string.IsNullOrEmpty(this.id))
            {
                MessageBox.Show("Harap pilih data", "Update Data",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            CRUD.sql = "UPDATE todo SET name_task = @name_task, deadline = @deadline, description = @description, status = @status WHERE id_todo = @id::integer";

            execute(CRUD.sql, "Edit");

            MessageBox.Show("Data berhasil diubah", "Update Data",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            loadData("");


            resetMe();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (dgv11.Rows.Count == 0)
            {
                return;
            }

            if (string.IsNullOrEmpty(this.id))
            {
                MessageBox.Show("Harap pilih data dari tabel", "Delete Data",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (MessageBox.Show("Apakah ingin menghapus data yang dipilih secara permanen?", "Delete Data",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                CRUD.sql = "DELETE FROM todo WHERE id_todo = @id::integer";

                execute(CRUD.sql, "Delete");

                MessageBox.Show("Data berhasil dihapus", "Delete Data",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                loadData("");


                resetMe();
            }
            loadData("");

            resetMe();

        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            NpgsqlConnection connection = new NpgsqlConnection();
            string constr = "Server=localhost;Port=5432;User Id=postgres;Password=111222;Database=todolist";
            connection.ConnectionString = constr;
            connection.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM todo WHERE name_task ='" + keywordtextBox.Text + "'";
            NpgsqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(dr);
                dgv11.DataSource = dt;
                foreach (DataGridViewColumn column in dgv11.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

            }
            cmd.Dispose();
            connection.Close();
        }
    }
}