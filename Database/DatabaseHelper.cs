using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace EscapeRoomMS.Database
{
    public static class DatabaseHelper
    {
        // "." is the default SQL Server instance on this computer.
        // Use @"Data Source=.\SQLEXPRESS;" for SQL Server Express, or
        // @"Data Source=(localdb)\MSSQLLocalDB;" for LocalDB.
        private static readonly string connectionString =
            @"Data Source=.;
              Initial Catalog=EscapeRoomDB;
              Integrated Security=True;
              TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection conn =
                    GetConnection())
                {
                    conn.Open();

                    MessageBox.Show(
                        "Database Connected Successfully!",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "SQL Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return false;
            }
        }

        public static DataTable ExecuteQuery(
            string query,
            SqlParameter[] parameters = null)
        {
            DataTable dt =
                new DataTable();

            try
            {
                using (SqlConnection conn =
                    GetConnection())
                {
                    conn.Open();

                    using (SqlCommand cmd =
                        new SqlCommand(
                            query,
                            conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        using (SqlDataAdapter adapter =
                            new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Database Error");
            }

            return dt;
        }

        // CRUD
        public static int ExecuteNonQuery(
            string query,
            SqlParameter[] parameters = null)
        {
            try
            {
                using (SqlConnection conn =
                    GetConnection())
                {
                    conn.Open();

                    using (SqlCommand cmd =
                        new SqlCommand(
                            query,
                            conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }

                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "SQL Error");

                return 0;
            }
        }

        public static object ExecuteScalar(
            string query)
        {
            try
            {
                using (SqlConnection conn =
                    GetConnection())
                {
                    conn.Open();

                    using (SqlCommand cmd =
                        new SqlCommand(
                            query,
                            conn))
                    {
                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}