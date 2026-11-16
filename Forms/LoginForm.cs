
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using EscapeRoomMS.Database;

namespace EscapeRoomMS.Forms
{
    public partial class LoginForm : Form
    {
        private int failedAttempts = 0;
        private const int MaxAttempts = 3;

        public LoginForm()
        {
            InitializeComponent();
        }

        // Load
        private void LoginForm_Load(
            object sender,
            EventArgs e)
        {
            lblError.Text = "";
            txtPassword.PasswordChar = '\0';
            txtPassword.UseSystemPasswordChar = true;
            btnShowPassword.Text = "👁";

            txtUsername.Focus();
        }

            // LOGIN
            private void btnLogin_Click(
            object sender,
            EventArgs e)
        {
            lblError.Text = "";

            if (string.IsNullOrWhiteSpace(
                txtUsername.Text))
            {
                lblError.Text =
                    "Please enter username.";

                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(
                txtPassword.Text))
            {
                lblError.Text =
                    "Please enter password.";

                txtPassword.Focus();
                return;
            }

            if (failedAttempts >= MaxAttempts)
            {
                lblError.Text =
                    "Too many failed attempts.";

                btnLogin.Enabled = false;
                return;
            }

            try
            {
                string query =
                    @"SELECT *
                      FROM Staff
                      WHERE Username = @Username
                      AND Password = @Password
                      AND IsActive = 1";

                using (SqlConnection conn =
                    DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    using (SqlCommand cmd =
                        new SqlCommand(
                            query,
                            conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@Username",
                            txtUsername.Text.Trim());

                        cmd.Parameters.AddWithValue(
                            "@Password",
                            txtPassword.Text.Trim());

                        SqlDataAdapter adapter =
                            new SqlDataAdapter(cmd);

                        DataTable dt =
                            new DataTable();

                        adapter.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            DashboardForm dashboard =
                                new DashboardForm();

                            dashboard.Show();

                            this.Hide();
                        }
                        else
                        {
                            failedAttempts++;

                            int remaining =
                                MaxAttempts
                                - failedAttempts;

                            lblError.Text =
                                remaining > 0
                                ? $"Invalid credentials. {remaining} attempt(s) left."
                                : "Account locked.";

                            txtPassword.Clear();
                            txtPassword.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Login Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // SHOW / HIDE PASSWORD
        private void btnShowPassword_Click(
            object sender,
            EventArgs e)
        {
            if (txtPassword.UseSystemPasswordChar)
            {
                txtPassword.UseSystemPasswordChar =
                    false;

                txtPassword.PasswordChar =
                    '\0';

                btnShowPassword.Text =
                    "🙈";
            }
            else
            {
                txtPassword.UseSystemPasswordChar =
                    true;

                btnShowPassword.Text =
                    "👁";
            }
        }

        private void btnLogout_Click(
            object sender,
                EventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
    }
}

