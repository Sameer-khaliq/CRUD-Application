using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CRUD_Application
{
    public partial class Form1 : Form
    {
        MySqlConnection con;

        public Form1()
        {
            InitializeComponent();
            con = Database.GetConnection();
        }

        private void load_students()
        {
            con.Open();
            string query = "SELECT * FROM students";
            MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            con.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            load_students();
        }

        private void add_button_Click(object sender, EventArgs e)
        {
            // TextBoxes se input lein, Labels se nahi
            string user_name = name.Text.Trim();
            string user_email = email.Text.Trim();
            string user_phone = phone.Text.Trim();
            string user_department = department.Text.Trim();

            // Task 1: Validation - check if any field is empty
            if (string.IsNullOrWhiteSpace(user_name) ||
                string.IsNullOrWhiteSpace(user_email) ||
                string.IsNullOrWhiteSpace(user_phone) ||
                string.IsNullOrWhiteSpace(user_department))
            {
                MessageBox.Show("Please fill in all fields before adding a student.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            con.Open();
            string query = "INSERT INTO students (name, email, phone, department) VALUES (@name, @email, @phone, @department)";
            MySqlCommand cmd = new MySqlCommand(query, con);

            cmd.Parameters.AddWithValue("@name", user_name);
            cmd.Parameters.AddWithValue("@email", user_email);
            cmd.Parameters.AddWithValue("@phone", user_phone);
            cmd.Parameters.AddWithValue("@department", user_department);

            // Task 2: Capture affected rows
            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Student added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                name.Clear();
                email.Clear();
                phone.Clear();
                department.Clear();

                load_students();
            }
            else
            {
                MessageBox.Show("Failed to add student. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(user_id.Text))
            {
                MessageBox.Show("Please select a student from the table first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. User se confirmation lein
            DialogResult confirm = MessageBox.Show("Are you sure you want to delete this student?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
            {
                return;
            }

            int student_id = Convert.ToInt32(user_id.Text);

            con.Open();
            string query = "DELETE FROM students WHERE id = @id";
            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@id", student_id);

            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Record deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Fields clear karein
                user_id.Clear();
                name.Clear();
                email.Clear();
                phone.Clear();
                department.Clear();

                load_students();
            }
            else
            {
                MessageBox.Show("Record not found or already deleted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void update_button_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(user_id.Text))
            {
                MessageBox.Show("Please select a student from the table first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Validation: Empty fields check
            if (string.IsNullOrWhiteSpace(name.Text) ||
                string.IsNullOrWhiteSpace(email.Text) ||
                string.IsNullOrWhiteSpace(phone.Text) ||
                string.IsNullOrWhiteSpace(department.Text))
            {
                MessageBox.Show("All fields are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int student_id = Convert.ToInt32(user_id.Text);
            string user_name = name.Text;
            string user_email = email.Text;
            string user_phone = phone.Text;
            string user_department = department.Text;

            con.Open();
            string query = "UPDATE students SET name = @name, email = @email, phone = @phone, department = @department WHERE id = @id";

            MySqlCommand cmd = new MySqlCommand(query, con);
            cmd.Parameters.AddWithValue("@name", user_name);
            cmd.Parameters.AddWithValue("@email", user_email);
            cmd.Parameters.AddWithValue("@phone", user_phone);
            cmd.Parameters.AddWithValue("@department", user_department);
            cmd.Parameters.AddWithValue("@id", student_id);

            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close();

            if (rowsAffected > 0)
            {
                MessageBox.Show("Student details updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Fields clear karein
                user_id.Clear();
                name.Clear();
                email.Clear();
                phone.Clear();
                department.Clear();

                load_students();
            }
            else
            {
                MessageBox.Show("Failed to update record.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Values ko textboxes mein populate karein
                user_id.Text = row.Cells["id"].Value?.ToString() ?? "";
                name.Text = row.Cells["name"].Value?.ToString() ?? "";
                email.Text = row.Cells["email"].Value?.ToString() ?? "";
                phone.Text = row.Cells["phone"].Value?.ToString() ?? "";
                department.Text = row.Cells["department"].Value?.ToString() ?? "";
            }
        }

        private void user_id_TextChanged(object sender, EventArgs e)
        {

        }
    }
}