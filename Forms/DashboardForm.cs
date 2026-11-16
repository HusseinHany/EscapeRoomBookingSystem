using EscapeRoomMS.Database;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace EscapeRoomMS.Forms
{
    public partial class DashboardForm : Form
    {
        private System.Windows.Forms.Timer clockTimer;

        public DashboardForm()
        {
            InitializeComponent();
        }



        private void LoadRecentBookings()
        {
            try
            {
                string query =
                    @"SELECT TOP 10
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
              ORDER BY
              b.BookingDate DESC";

                dgvRecentBookings.DataSource =
                    DatabaseHelper
                    .ExecuteQuery(query);

                dgvRecentBookings
                    .BackgroundColor =
                    Color.FromArgb(
                        28, 28, 55);

                dgvRecentBookings
                    .DefaultCellStyle
                    .BackColor =
                    Color.FromArgb(
                        28, 28, 55);

                dgvRecentBookings
                    .DefaultCellStyle
                    .ForeColor =
                    Color.White;

                dgvRecentBookings
                    .ColumnHeadersDefaultCellStyle
                    .BackColor =
                    Color.FromArgb(
                        46, 204, 113);

                dgvRecentBookings
                    .ColumnHeadersDefaultCellStyle
                    .ForeColor =
                    Color.White;

                dgvRecentBookings
                    .EnableHeadersVisualStyles =
                    false;

                dgvRecentBookings
                    .GridColor =
                    Color.Gray;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Dashboard Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void DashboardForm_Load(
            object sender,
            EventArgs e)
        {
            StartClock();
            LoadDashboardStats();
            LoadRecentBookings();
            
        }

        private void LoadDashboardStats()
        {
            try
            {
                object todayBookings =
                    DatabaseHelper.ExecuteScalar(
                        @"SELECT COUNT(*)
                          FROM Bookings
                          WHERE CAST(BookingDate AS DATE)
                          =
                          CAST(GETDATE() AS DATE)");

                lblTodayBookings.Text =
                    todayBookings?.ToString()
                    ?? "0";

                object revenue =
                    DatabaseHelper.ExecuteScalar(
                        @"SELECT ISNULL(
                            SUM(PricePerSession),0)
                          FROM Rooms");

                lblRevenue.Text =
                    "EGP "
                    + revenue?.ToString();

                object weeklyRevenue =
                    DatabaseHelper.ExecuteScalar(
                        @"SELECT ISNULL(
                            SUM(PricePerSession),0)
                          FROM Rooms
                          WHERE IsAvailable = 1");

                lblWeeklyRevenue.Text =
                    "EGP "
                    + weeklyRevenue?.ToString();

                object availableRooms =
                    DatabaseHelper.ExecuteScalar(
                        @"SELECT COUNT(*)
                          FROM Rooms
                          WHERE IsAvailable = 1");

                lblAvailableRooms.Text =
                    availableRooms?.ToString()
                    ?? "0";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Dashboard Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void StartClock()
        {
            clockTimer =
                new System.Windows.Forms.Timer();

            clockTimer.Interval =
                1000;

            clockTimer.Tick +=
                ClockTimer_Tick;

            clockTimer.Start();
        }

        private void ClockTimer_Tick(
            object sender,
            EventArgs e)
        {
            lblDateTime.Text =
                DateTime.Now
                .ToString(
                    "dddd, dd MMM yyyy | HH:mm:ss");
        }

        private void btnRooms_Click(
            object sender,
            EventArgs e)
        {
            RoomsForm rooms =
                new RoomsForm();
            rooms.ShowDialog();
            LoadDashboardStats();
        }

        private void btnBookings_Click(
            object sender,
                      EventArgs e)
        {
            BookingsForm bookings =
                new BookingsForm();

            bookings.ShowDialog();

            LoadDashboardStats();
        }

        private void btnFilter_Click(
            object sender,
                    EventArgs e)
        {
            FilterForm filter =
                new FilterForm();

            filter.ShowDialog();
        }
        private void btnLeaderboard_Click(
                 object sender,
                      EventArgs e)
        {
            LeaderboardForm leaderboard =
                new LeaderboardForm();

            leaderboard.ShowDialog();
        }

        private void btnReports_Click(
             object sender,
                 EventArgs e)
        {
            ReportsForm reports =
                new ReportsForm();

            reports.ShowDialog();

            LoadDashboardStats();
        }

        private void btnLogout_Click(
         object sender,
             EventArgs e)
        {
            Application.Exit();
        }

        private void lblAvailableRooms_Click(object sender, EventArgs e)
        {

        }

        private void dgvRecentBookings_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}