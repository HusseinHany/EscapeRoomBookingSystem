using System;
using System.Data;
using System.Windows.Forms;
using EscapeRoomMS.Database;

namespace EscapeRoomMS.Forms
{
    public partial class ReportsForm : Form
    {
        public ReportsForm()
        {
            InitializeComponent();
        }

        private void ReportsForm_Load(
            object sender,
            EventArgs e)
        {
            dtpFrom.Value =
                DateTime.Today
                .AddMonths(-1);

            dtpTo.Value =
                DateTime.Today;

            LoadReport();
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
            DashboardForm dashboard =
                new DashboardForm();

            dashboard.Show();

            this.Close();
        }

        private void LoadReport()
        {
            try
            {
                string fromDate =
                    dtpFrom.Value
                    .ToString("yyyy-MM-dd");

                string toDate =
                    dtpTo.Value
                    .ToString("yyyy-MM-dd");
                object revenue =
                    DatabaseHelper.ExecuteScalar(
                        @"SELECT ISNULL(
                            SUM(TotalPrice),0)
                          FROM Bookings
                          WHERE PaymentStatus='Paid'
                          AND CAST(
                              BookingDate AS DATE)
                          BETWEEN '"
                          + fromDate
                          + "' AND '"
                          + toDate
                          + "'");

                lblTotalRevenue.Text =
                    "EGP "
                    + revenue?.ToString();

                object bookings =
                    DatabaseHelper.ExecuteScalar(
                        @"SELECT COUNT(*)
                          FROM Bookings
                          WHERE CAST(
                              BookingDate AS DATE)
                          BETWEEN '"
                          + fromDate
                          + "' AND '"
                          + toDate
                          + "'");

                lblTotalBookings.Text =
                    bookings?.ToString();

                object paid =
                    DatabaseHelper.ExecuteScalar(
                        @"SELECT COUNT(*)
                          FROM Bookings
                          WHERE PaymentStatus='Paid'
                          AND CAST(
                              BookingDate AS DATE)
                          BETWEEN '"
                          + fromDate
                          + "' AND '"
                          + toDate
                          + "'");

                lblPaidBookings.Text =
                    paid?.ToString();

                object pending =
                    DatabaseHelper.ExecuteScalar(
                        @"SELECT COUNT(*)
                          FROM Bookings
                          WHERE PaymentStatus='Pending'
                          AND CAST(
                              BookingDate AS DATE)
                          BETWEEN '"
                          + fromDate
                          + "' AND '"
                          + toDate
                          + "'");

                lblPendingBookings.Text =
                    pending?.ToString();

                object cancelled =
                    DatabaseHelper.ExecuteScalar(
                        @"SELECT COUNT(*)
                          FROM Bookings
                          WHERE PaymentStatus='Cancelled'
                          AND CAST(
                              BookingDate AS DATE)
                          BETWEEN '"
                          + fromDate
                          + "' AND '"
                          + toDate
                          + "'");

                lblCancelledBookings.Text =
                    cancelled?.ToString();

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
                      WHERE CAST(
                          b.BookingDate AS DATE)
                      BETWEEN '"
                      + fromDate
                      + "' AND '"
                      + toDate
                      + @"'
                      ORDER BY
                      b.BookingDate DESC";

                dgvReport.DataSource =
                    DatabaseHelper
                    .ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Report Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnGenerate_Click(
            object sender,
            EventArgs e)
        {
            LoadReport();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void dgvReport_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}