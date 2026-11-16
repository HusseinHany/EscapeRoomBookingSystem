using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using EscapeRoomMS.Database;

namespace EscapeRoomMS.Forms
{
    public partial class RoomsForm : Form
    {
        private int selectedRoomID = -1;

        public RoomsForm()
        {
            InitializeComponent();
        }

        private void RoomsForm_Load(
            object sender,
            EventArgs e)
        {
            cmbDifficulty.SelectedIndex = 0;


            LoadRooms();

            dgvRooms.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvRooms.MultiSelect = false;
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
        private void LoadRooms()
        {
            try
            {
                string query =
                    @"SELECT
                        RoomID,
                        RoomName,
                        Theme,
                        Difficulty,
                        Capacity,
                        PricePerSession,
                        IsAvailable
                      FROM Rooms";

                dgvRooms.DataSource =
                    DatabaseHelper
                    .ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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
                    @"INSERT INTO Rooms
                    (
                        RoomName,
                        Theme,
                        Difficulty,
                        Capacity,
                        PricePerSession,
                        IsAvailable
                    )
                    VALUES
                    (
                        @RoomName,
                        @Theme,
                        @Difficulty,
                        @Capacity,
                        @Price,
                        @Available
                    )";

                SqlParameter[] parameters =
                {
                    new SqlParameter(
                        "@RoomName",
                        txtRoomName.Text),

                    new SqlParameter(
                        "@Theme",
                        txtTheme.Text),

                    new SqlParameter(
                        "@Difficulty",
                        cmbDifficulty.Text),

                    new SqlParameter(
                        "@Capacity",
                        Convert.ToInt32(
                            txtCapacity.Text)),

                    new SqlParameter(
                        "@Price",
                        Convert.ToDecimal(
                            txtPrice.Text)),

                    new SqlParameter(
                        "@Available",
                        chkAvailable.Checked)
                };

                int rows =
                    DatabaseHelper
                    .ExecuteNonQuery(
                        query,
                        parameters);

                if (rows > 0)
                {
                    MessageBox.Show(
                        "Room Added Successfully!");

                    LoadRooms();

                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "SQL Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void btnUpdate_Click(
            object sender,
            EventArgs e)
        {
            if (selectedRoomID == -1)
            {
                MessageBox.Show(
                    "Please select a room first.");

                return;
            }

            if (!ValidateInputs())
                return;

            try
            {
                string query =
                    @"UPDATE Rooms
                      SET
                        RoomName = @RoomName,
                        Theme = @Theme,
                        Difficulty = @Difficulty,
                        Capacity = @Capacity,
                        PricePerSession = @Price,
                        IsAvailable = @Available
                      WHERE RoomID = @RoomID";

                SqlParameter[] parameters =
                {
                    new SqlParameter(
                        "@RoomName",
                        txtRoomName.Text),

                    new SqlParameter(
                        "@Theme",
                        txtTheme.Text),

                    new SqlParameter(
                        "@Difficulty",
                        cmbDifficulty.Text),

                    new SqlParameter(
                        "@Capacity",
                        Convert.ToInt32(
                            txtCapacity.Text)),

                    new SqlParameter(
                        "@Price",
                        Convert.ToDecimal(
                            txtPrice.Text)),

                    new SqlParameter(
                        "@Available",
                        chkAvailable.Checked),

                    new SqlParameter(
                        "@RoomID",
                        selectedRoomID)
                };

                int rows =
                    DatabaseHelper
                    .ExecuteNonQuery(
                        query,
                        parameters);

                if (rows > 0)
                {
                    MessageBox.Show(
                        "Room Updated Successfully!");

                    LoadRooms();

                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "SQL Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void btnDelete_Click(
    object sender,
    EventArgs e)
        {
            if (selectedRoomID == -1)
            {
                MessageBox.Show(
                    "Please select a room first.");

                return;
            }

            DialogResult result =
                MessageBox.Show(
                    "Mark this room as unavailable?",
                    "Confirm",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                string query =
                    @"UPDATE Rooms
              SET IsAvailable = 0
              WHERE RoomID = @RoomID";

                SqlParameter[] parameters =
                {
            new SqlParameter(
                "@RoomID",
                selectedRoomID)
        };

                int rows =
                    DatabaseHelper
                    .ExecuteNonQuery(
                        query,
                        parameters);

                if (rows > 0)
                {
                    MessageBox.Show(
                        "Room marked as unavailable.");

                    LoadRooms();

                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "SQL Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void txtSearch_TextChanged(
            object sender,
            EventArgs e)
        {
            try
            {
                string query =
                    @"SELECT *
                      FROM Rooms
                      WHERE RoomName
                      LIKE @Search";

                SqlParameter[] parameters =
                {
                    new SqlParameter(
                        "@Search",
                        "%" + txtSearch.Text + "%")
                };

                dgvRooms.DataSource =
                    DatabaseHelper
                    .ExecuteQuery(query, parameters);
            }
            catch
            {

            }
        }

        private void dgvRooms_CellClick(
            object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row =
                dgvRooms.Rows[e.RowIndex];

            selectedRoomID =
                Convert.ToInt32(
                    row.Cells["RoomID"].Value);

            txtRoomName.Text =
                row.Cells["RoomName"]
                .Value
                .ToString();

            txtTheme.Text =
                row.Cells["Theme"]
                .Value
                .ToString();

            cmbDifficulty.Text =
                row.Cells["Difficulty"]
                .Value
                .ToString();

            txtCapacity.Text =
                row.Cells["Capacity"]
                .Value
                .ToString();

            txtPrice.Text =
                row.Cells["PricePerSession"]
                .Value
                .ToString();

            chkAvailable.Checked =
                Convert.ToBoolean(
                    row.Cells["IsAvailable"]
                    .Value);
        }

        private void btnClear_Click(
            object sender,
            EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtRoomName.Clear();
            txtTheme.Clear();
            txtCapacity.Clear();
            txtPrice.Clear();
            txtSearch.Clear();

            cmbDifficulty.SelectedIndex = 0;

            chkAvailable.Checked = true;

            selectedRoomID = -1;
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(
                txtRoomName.Text))
            {
                MessageBox.Show(
                    "Enter room name.");

                return false;
            }

            if (string.IsNullOrWhiteSpace(
                txtTheme.Text))
            {
                MessageBox.Show(
                    "Enter room theme.");

                return false;
            }

            if (!int.TryParse(
                txtCapacity.Text,
                out _))
            {
                MessageBox.Show(
                    "Capacity must be a number.");

                return false;
            }

            if (!decimal.TryParse(
                txtPrice.Text,
                out _))
            {
                MessageBox.Show(
                    "Price must be a number.");

                return false;
            }

            return true;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}