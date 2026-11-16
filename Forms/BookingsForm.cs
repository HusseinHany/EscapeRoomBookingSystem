
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using EscapeRoomMS.Database;

namespace EscapeRoomMS.Forms
{
    public partial class BookingsForm : Form
    {
        private int selectedBookingID = -1;
        private decimal selectedRoomPrice = 0;

        public BookingsForm()
        {
            InitializeComponent();
        }

        private void BookingsForm_Load(
            object sender,
            EventArgs e)
        {
            cmbPayment.SelectedIndex = 0;

            LoadRooms();

            LoadBookings();

            dgvBookings.SelectionMode =
                DataGridViewSelectionMode
                .FullRowSelect;

            dgvBookings.MultiSelect =
                false;
        }
        private void LoadRooms()
        {
            string query =
                @"SELECT RoomID,
                         RoomName,
                         PricePerSession
                  FROM Rooms";

            DataTable dt =
                DatabaseHelper.ExecuteQuery(
                    query);

            cmbRoom.DataSource = dt;
            cmbRoom.DisplayMember =
                "RoomName";

            cmbRoom.ValueMember =
                "RoomID";
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

            dgvBookings.DataSource =
                DatabaseHelper.ExecuteQuery(
                    query);
        }

        private void cmbRoom_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            try
            {
                if (cmbRoom.SelectedValue
                    == null)
                    return;

                string query =
                    @"SELECT PricePerSession
                      FROM Rooms
                      WHERE RoomID = "
                      + cmbRoom.SelectedValue;

                object price =
                    DatabaseHelper
                    .ExecuteScalar(query);

                selectedRoomPrice =
                    Convert.ToDecimal(price);

                lblTotalPrice.Text =
                    "EGP "
                    + selectedRoomPrice;
            }
            catch
            {

            }
        }

        private void btnAdd_Click(
            object sender,
            EventArgs e)
        {
            if (!ValidateInputs())
                return;

            try
            {
                string query =
                    @"INSERT INTO Bookings
                    (
                        RoomID,
                        CustomerName,
                        CustomerPhone,
                        BookingDate,
                        GroupSize,
                        PaymentStatus,
                        TotalPrice
                    )
                    VALUES
                    (
                        @RoomID,
                        @CustomerName,
                        @Phone,
                        @Date,
                        @GroupSize,
                        @Payment,
                        @Price
                    )";

                SqlParameter[] parameters =
                {
                    new SqlParameter(
                        "@RoomID",
                        cmbRoom.SelectedValue),

                    new SqlParameter(
                        "@CustomerName",
                        txtCustomerName.Text),

                    new SqlParameter(
                        "@Phone",
                        txtPhone.Text),

                    new SqlParameter(
                        "@Date",
                        dtpBookingDate.Value),

                    new SqlParameter(
                        "@GroupSize",
                        Convert.ToInt32(
                            txtGroupSize.Text)),

                    new SqlParameter(
                        "@Payment",
                        cmbPayment.Text),

                    new SqlParameter(
                        "@Price",
                        selectedRoomPrice)
                };

                int rows =
                    DatabaseHelper
                    .ExecuteNonQuery(
                        query,
                        parameters);

                if (rows > 0)
                {
                    MessageBox.Show(
                        "Booking Added!");

                    LoadBookings();

                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message);
            }
        }

        private void btnUpdate_Click(
            object sender,
            EventArgs e)
        {
            if (selectedBookingID == -1)
            {
                MessageBox.Show(
                    "Select booking first.");
                return;
            }

            string query =
                @"UPDATE Bookings
                  SET
                    RoomID=@RoomID,
                    CustomerName=@CustomerName,
                    CustomerPhone=@Phone,
                    BookingDate=@Date,
                    GroupSize=@GroupSize,
                    PaymentStatus=@Payment,
                    TotalPrice=@Price
                  WHERE BookingID=@BookingID";

            SqlParameter[] parameters =
            {
                new SqlParameter(
                    "@RoomID",
                    cmbRoom.SelectedValue),

                new SqlParameter(
                    "@CustomerName",
                    txtCustomerName.Text),

                new SqlParameter(
                    "@Phone",
                    txtPhone.Text),

                new SqlParameter(
                    "@Date",
                    dtpBookingDate.Value),

                new SqlParameter(
                    "@GroupSize",
                    Convert.ToInt32(
                        txtGroupSize.Text)),

                new SqlParameter(
                    "@Payment",
                    cmbPayment.Text),

                new SqlParameter(
                    "@Price",
                    selectedRoomPrice),

                new SqlParameter(
                    "@BookingID",
                    selectedBookingID)
            };

            DatabaseHelper.ExecuteNonQuery(
                query,
                parameters);

            MessageBox.Show(
                "Booking Updated!");

            LoadBookings();

            ClearFields();
        }
        private void btnDelete_Click(
            object sender,
            EventArgs e)
        {
            if (selectedBookingID == -1)
            {
                MessageBox.Show(
                    "Select booking first.");
                return;
            }

            DialogResult result =
                MessageBox.Show(
                    "Delete booking?",
                    "Confirm",
                    MessageBoxButtons.YesNo);

            if (result !=
                DialogResult.Yes)
                return;

            string query =
                @"DELETE FROM Bookings
                  WHERE BookingID =
                  @BookingID";

            SqlParameter[] parameters =
            {
                new SqlParameter(
                    "@BookingID",
                    selectedBookingID)
            };

            DatabaseHelper.ExecuteNonQuery(
                query,
                parameters);

            MessageBox.Show(
                "Booking Deleted!");

            LoadBookings();

            ClearFields();
        }

        private void dgvBookings_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row =
                dgvBookings.Rows[e.RowIndex];

            selectedBookingID =
                Convert.ToInt32(
                    row.Cells["BookingID"]
                    .Value);

            txtCustomerName.Text =
                row.Cells[
                    "CustomerName"]
                .Value.ToString();

            txtPhone.Text =
                row.Cells[
                    "CustomerPhone"]
                .Value.ToString();

            txtGroupSize.Text =
                row.Cells[
                    "GroupSize"]
                .Value.ToString();

            cmbPayment.Text =
                row.Cells[
                    "PaymentStatus"]
                .Value.ToString();

            dtpBookingDate.Value =
                Convert.ToDateTime(
                    row.Cells[
                        "BookingDate"]
                    .Value);
        }

        private void txtSearch_TextChanged(
            object sender,
            EventArgs e)
        {
            string query =
                @"SELECT *
                  FROM Bookings
                  WHERE CustomerName
                  LIKE @Search";

            SqlParameter[] parameters =
            {
                new SqlParameter(
                    "@Search",
                    "%" + txtSearch.Text + "%")
            };

            dgvBookings.DataSource =
                DatabaseHelper.ExecuteQuery(
                    query,
                    parameters);
        }

        private void btnClear_Click(
            object sender,
            EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtCustomerName.Clear();
            txtPhone.Clear();
            txtGroupSize.Clear();
            txtSearch.Clear();

            cmbPayment.SelectedIndex = 0;

            selectedBookingID = -1;
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(
                txtCustomerName.Text))
            {
                MessageBox.Show(
                    "Enter customer name.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(
                txtPhone.Text))
            {
                MessageBox.Show(
                    "Enter phone.");
                return false;
            }

            if (!int.TryParse(
                txtGroupSize.Text,
                out _))
            {
                MessageBox.Show(
                    "Invalid group size.");
                return false;
            }

            return true;
        }


        private void txtGroupSize_TextChanged(
    object sender,
    EventArgs e)
        {
            if (int.TryParse(
                txtGroupSize.Text,
                out int groupSize))
            {
                decimal total =
                    selectedRoomPrice
                    * groupSize;

                lblTotalPrice.Text =
                    "EGP " + total;
            }
            else
            {
                lblTotalPrice.Text =
                    "EGP 0";
            }
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

        private void txtCustomerName_TextChanged(object sender, EventArgs e)
        {

        }
        private void dgvBookings_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
