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
            EscapeRoomMS.Theme.Apply(this);
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

                // Grid colours come from Theme.Apply.
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

                // Money actually taken today: paid bookings dated today.
                object revenue =
                    DatabaseHelper.ExecuteScalar(
                        @"SELECT ISNULL(SUM(TotalPrice), 0)
                          FROM Bookings
                          WHERE PaymentStatus = 'Paid'
                            AND CAST(BookingDate AS DATE)
                                = CAST(GETDATE() AS DATE)");

                lblRevenue.Text =
                    "EGP "
                    + Convert.ToDecimal(revenue).ToString("N0");

                // Paid bookings over the last 7 days, today included.
                object weeklyRevenue =
                    DatabaseHelper.ExecuteScalar(
                        @"SELECT ISNULL(SUM(TotalPrice), 0)
                          FROM Bookings
                          WHERE PaymentStatus = 'Paid'
                            AND CAST(BookingDate AS DATE)
                                BETWEEN CAST(DATEADD(DAY, -6, GETDATE()) AS DATE)
                                    AND CAST(GETDATE() AS DATE)");

                lblWeeklyRevenue.Text =
                    "EGP "
                    + Convert.ToDecimal(weeklyRevenue).ToString("N0");

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