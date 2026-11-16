using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using EscapeRoomMS.Database;

namespace EscapeRoomMS.Forms
{
    public partial class FilterForm : Form
    {
        public FilterForm()
        {
            InitializeComponent();
            EscapeRoomMS.Theme.Apply(this);
        }

        private void FilterForm_Load(object sender, EventArgs e)
        {
            LoadRooms();
            cmbStatus.SelectedIndex = 0;
            LoadBookings();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoadRooms()
        {
            string query =
                @"SELECT RoomID,
                         RoomName
                  FROM Rooms";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            DataRow allRow = dt.NewRow();

            allRow["RoomID"] = 0;
            allRow["RoomName"] = "All";

            dt.Rows.InsertAt(allRow, 0);

            cmbRoom.DataSource = dt;
            cmbRoom.DisplayMember = "RoomName";
            cmbRoom.ValueMember = "RoomID";
        }



        private void LoadBookings()
        {
            string query =
                @"SELECT
                    b.BookingID,
                    r.RoomName,
                    b.CustomerName,
                    b.CustomerPhone,
                    b.BookingDate,
                    b.GroupSize,
                    b.PaymentStatus,
                    b.TotalPrice
                  FROM Bookings b
                  INNER JOIN Rooms r
                  ON b.RoomID = r.RoomID";

            DataTable dt = DatabaseHelper.ExecuteQuery(query);

            dgvResults.DataSource = dt;

            lblCount.Text = dt.Rows.Count + " Results";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string query =
                    @"SELECT
                        b.BookingID,
                        r.RoomName,
                        b.CustomerName,
                        b.CustomerPhone,
                        b.BookingDate,
                        b.GroupSize,
                        b.PaymentStatus,
                        b.TotalPrice
                      FROM Bookings b
                      INNER JOIN Rooms r
                      ON b.RoomID = r.RoomID
                      WHERE 1=1";

                List<SqlParameter> parameters =
                    new List<SqlParameter>();

                if (!string.IsNullOrWhiteSpace(txtCustomer.Text))
                {
                    query += " AND b.CustomerName LIKE @Customer";
                    parameters.Add(new SqlParameter(
                        "@Customer",
                        "%" + txtCustomer.Text.Trim() + "%"));
                }

                if (cmbRoom.Text != "All")
                {
                    query += " AND r.RoomName = @Room";
                    parameters.Add(new SqlParameter(
                        "@Room",
                        cmbRoom.Text));
                }

                if (cmbStatus.Text != "All")
                {
                    query += " AND b.PaymentStatus = @Status";
                    parameters.Add(new SqlParameter(
                        "@Status",
                        cmbStatus.Text));
                }

                query += @" AND CAST(b.BookingDate AS DATE)
                            BETWEEN @From AND @To";
                parameters.Add(new SqlParameter(
                    "@From",
                    SqlDbType.Date) { Value = dtpFrom.Value.Date });
                parameters.Add(new SqlParameter(
                    "@To",
                    SqlDbType.Date) { Value = dtpTo.Value.Date });

                DataTable dt = DatabaseHelper.ExecuteQuery(
                    query,
                    parameters.ToArray());

                dgvResults.DataSource = dt;

                lblCount.Text = dt.Rows.Count + " Results";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Filter Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCustomer.Clear();

            cmbStatus.SelectedIndex = 0;
            cmbRoom.SelectedIndex = 0;

            dtpFrom.Value = DateTime.Today.AddMonths(-1);
            dtpTo.Value = DateTime.Today;

            LoadBookings();
        }

        private void dgvResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}