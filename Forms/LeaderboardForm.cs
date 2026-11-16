using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using EscapeRoomMS.Database;

namespace EscapeRoomMS.Forms
{
    public partial class LeaderboardForm : Form
    {
        public LeaderboardForm()
        {
            InitializeComponent();
            EscapeRoomMS.Theme.Apply(this);
        }

        private void LeaderboardForm_Load(
            object sender,
            EventArgs e)
        {
            LoadLeaderboard();
        }
        private void LoadLeaderboard()
        {
            try
            {
                string query =
                    @"SELECT *

                      FROM
                      (
                          SELECT
                              ROW_NUMBER()
                              OVER(
                              ORDER BY
                              SUM(TotalPrice)
                              DESC) AS Rank,

                              CustomerName,

                              COUNT(*) AS
                              TotalBookings,

                              SUM(TotalPrice)
                              AS TotalSpent

                          FROM Bookings

                          WHERE PaymentStatus
                          = 'Paid'

                          GROUP BY
                          CustomerName
                      ) AS Leaderboard

                      ORDER BY Rank";

                DataTable dt =
                    DatabaseHelper
                    .ExecuteQuery(query);

                dgvLeaderboard.DataSource =
                    dt;

                dgvLeaderboard
                    .AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode
                    .Fill;

                lblCount.Text =
                    dt.Rows.Count
                    + " Customers";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Leaderboard Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                string query =
                    @"SELECT *

                      FROM
                      (
                          SELECT
                              ROW_NUMBER()
                              OVER(
                              ORDER BY
                              SUM(TotalPrice)
                              DESC) AS Rank,

                              CustomerName,

                              COUNT(*) AS
                              TotalBookings,

                              SUM(TotalPrice)
                              AS TotalSpent

                          FROM Bookings

                          WHERE PaymentStatus
                          = 'Paid'

                          GROUP BY
                          CustomerName
                      ) AS Leaderboard";

                // Ranks are worked out over every customer first, so a
                // search keeps each customer's real position.
                SqlParameter[] parameters = null;

                if (!string.IsNullOrWhiteSpace(
                    txtSearch.Text))
                {
                    query +=
                        " WHERE CustomerName LIKE @Search";
                    parameters = new SqlParameter[]
                    {
                        new SqlParameter(
                            "@Search",
                            "%" + txtSearch.Text.Trim() + "%")
                    };
                }

                query += " ORDER BY Rank";

                DataTable dt =
                    DatabaseHelper
                    .ExecuteQuery(query, parameters);

                dgvLeaderboard.DataSource =
                    dt;

                dgvLeaderboard
                    .AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode
                    .Fill;

                lblCount.Text =
                    dt.Rows.Count
                    + " Customers";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Search Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void txtSearch_TextChanged(
            object sender,
            EventArgs e)
        {
            btnSearch_Click(
                sender,
                e);
        }
        private void btnRefresh_Click(
            object sender,
            EventArgs e)
        {
            txtSearch.Clear();

            LoadLeaderboard();
        }

        private void btnBack_Click(
            object sender,
            EventArgs e)
        {
            this.Close();
        }      
        private void btnLogout_Click(
         object sender,
                        EventArgs e)
        {
            Application.Exit();
        }

        private void dgvLeaderboard_CellContentClick(
            object sender,
            DataGridViewCellEventArgs e)
        {

        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
    }
}