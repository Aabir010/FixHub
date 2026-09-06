using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using FixHub.Helpers;

namespace FixHub.Forms
{
    public partial class frmManageCategories : Form
    {
        public frmManageCategories()
        {
            InitializeComponent();
            LoadCategories();
        }

        private void LoadCategories()
        {
            lblError.Text = "";
            grid.Rows.Clear();

            string sql = @"
                SELECT 
                    c.CategoryId, 
                    c.CategoryName,
                    (SELECT COUNT(*) FROM Admins a WHERE a.CategoryId = c.CategoryId) AS ProviderCount
                FROM ServiceCategories c
                ORDER BY c.CategoryName;";

            try
            {
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int categoryId = Convert.ToInt32(reader[0]);
                            string categoryName = reader[1]?.ToString() ?? "";
                            int providerCount = reader.IsDBNull(2) ? 0 : Convert.ToInt32(reader[2]);

                            string providerText = providerCount == 1 ? "1 provider" : $"{providerCount} providers";
                            grid.Rows.Add(categoryName, providerText, "Edit", "Delete", categoryId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Failed to load categories: " + ex.Message;
                System.Diagnostics.Debug.WriteLine($"LoadCategories error: {ex.Message}");
            }
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            string newCat = txtNewCategory.Text.Trim();

            if (string.IsNullOrWhiteSpace(newCat))
            {
                lblError.ForeColor = Color.Firebrick;
                lblError.Text = "Please enter a category name.";
                return;
            }

            try
            {
                // Check for existing duplicate
                string checkSql = "SELECT TOP 1 CategoryId FROM ServiceCategories WHERE LOWER(CategoryName) = LOWER(@name)";
                var existing = DbHelper.ExecuteScalar(checkSql, new SqlParameter("@name", newCat));
                if (existing != null)
                {
                    lblError.ForeColor = Color.Firebrick;
                    lblError.Text = $"Category '{newCat}' already exists.";
                    return;
                }

                string insertSql = "INSERT INTO ServiceCategories (CategoryName) VALUES (@name)";
                DbHelper.ExecuteNonQuery(insertSql, new SqlParameter("@name", newCat));

                txtNewCategory.InnerTextBox.Text = "";
                lblError.ForeColor = Color.SeaGreen;
                lblError.Text = $"Category '{newCat}' added successfully.";

                LoadCategories();
            }
            catch (Exception ex)
            {
                lblError.ForeColor = Color.Firebrick;
                lblError.Text = "Failed to add category: " + ex.Message;
            }
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int categoryId = Convert.ToInt32(grid.Rows[e.RowIndex].Cells["colCategoryId"].Value);
            string currentName = grid.Rows[e.RowIndex].Cells["colCategory"].Value?.ToString() ?? "";
            string colName = grid.Columns[e.ColumnIndex].Name;

            // Handle Edit
            if (colName == "colEdit")
            {
                string updatedName = InputBoxHelper.Show($"Enter new name for category '{currentName}':", "Rename Category", currentName);
                if (string.IsNullOrWhiteSpace(updatedName) || updatedName.Trim().Equals(currentName, StringComparison.OrdinalIgnoreCase))
                    return;

                updatedName = updatedName.Trim();

                try
                {
                    string checkSql = "SELECT TOP 1 CategoryId FROM ServiceCategories WHERE LOWER(CategoryName) = LOWER(@name) AND CategoryId != @id";
                    var duplicate = DbHelper.ExecuteScalar(checkSql,
                        new SqlParameter("@name", updatedName),
                        new SqlParameter("@id", categoryId));

                    if (duplicate != null)
                    {
                        MessageBox.Show($"A category named '{updatedName}' already exists.", "Duplicate Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string updateSql = "UPDATE ServiceCategories SET CategoryName = @name WHERE CategoryId = @id";
                    DbHelper.ExecuteNonQuery(updateSql,
                        new SqlParameter("@name", updatedName),
                        new SqlParameter("@id", categoryId));

                    MessageBox.Show($"Category renamed to '{updatedName}'.", "FixHub", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCategories();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to rename category: " + ex.Message, "FixHub", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            // Handle Delete
            else if (colName == "colDelete")
            {
                try
                {
                    // Check if any providers are assigned
                    string countSql = "SELECT COUNT(*) FROM Admins WHERE CategoryId = @id";
                    int count = Convert.ToInt32(DbHelper.ExecuteScalar(countSql, new SqlParameter("@id", categoryId)));

                    if (count > 0)
                    {
                        MessageBox.Show($"Cannot delete category '{currentName}' because {count} provider(s) are currently registered under it.\n\nPlease reassign or remove those providers first.",
                            "Category In Use", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var confirm = MessageBox.Show($"Are you sure you want to delete category '{currentName}'?",
                        "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (confirm != DialogResult.Yes) return;

                    string deleteSql = "DELETE FROM ServiceCategories WHERE CategoryId = @id";
                    DbHelper.ExecuteNonQuery(deleteSql, new SqlParameter("@id", categoryId));

                    MessageBox.Show($"Category '{currentName}' has been deleted.", "FixHub", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCategories();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to delete category: " + ex.Message, "FixHub", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void lblBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
