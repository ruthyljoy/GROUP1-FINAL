using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

namespace M1PartialForms
{
    public partial class Form1 : Form
    {
        private readonly HttpClient _httpClient;

        public Form1()
        {
            InitializeComponent();
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7289/") };

            // Link the button click events
            button5.Click += async (sender, e) => await button1_ClickAsync(sender, e);
            button6.Click += async (sender, e) => await button2_ClickAsync(sender, e);
            button3.Click += async (sender, e) => await button3_ClickAsync(sender, e);
            button4.Click += async (sender, e) => await button4_ClickAsync(sender, e);
        }

        private async Task GetDataFromApiAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<TodoItem>>("api/TodoItems");

                if (response != null)
                {
                    dataGridView2.DataSource = response;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async Task PostDataToApiAsync(TodoItem newItem)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/TodoItems", newItem);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Product added successfully!");
                    await GetDataFromApiAsync(); // Refresh the DataGridView
                }
                else
                {
                    MessageBox.Show($"Error: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async Task PutDataToApiAsync(TodoItem updatedItem)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/TodoItems/{updatedItem.Id}", updatedItem);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Product updated successfully!");
                    await GetDataFromApiAsync(); // Refresh the DataGridView
                }
                else
                {
                    MessageBox.Show($"Error: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async Task DeleteDataFromApiAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/TodoItems/{id}");

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Product deleted successfully!");
                    await GetDataFromApiAsync(); // Refresh the DataGridView
                }
                else
                {
                    MessageBox.Show($"Error: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private async Task button1_ClickAsync(object sender, EventArgs e)
        {
            await GetDataFromApiAsync();
        }

        private async Task button2_ClickAsync(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView2.SelectedRows[0];
                var newItem = new TodoItem
                {
                    Id = 0, // The ID will be set by the server
                    Name = selectedRow.Cells["Name"].Value.ToString(),
                    Code = selectedRow.Cells["Code"].Value.ToString(),
                    Brand = selectedRow.Cells["Brand"].Value.ToString(),
                    UnitPrice = Convert.ToDecimal(selectedRow.Cells["UnitPrice"].Value),
                    IsComplete = Convert.ToBoolean(selectedRow.Cells["IsComplete"].Value)
                };

                await PostDataToApiAsync(newItem);
            }
            else
            {
                MessageBox.Show("Please select a row to add.");
            }
        }

        private async Task button3_ClickAsync(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView2.SelectedRows[0];
                var updatedItem = selectedRow.DataBoundItem as TodoItem;

                if (updatedItem != null)
                {
                    updatedItem.Name = selectedRow.Cells["Name"].Value.ToString();
                    updatedItem.Code = selectedRow.Cells["Code"].Value.ToString();
                    updatedItem.Brand = selectedRow.Cells["Brand"].Value.ToString();
                    updatedItem.UnitPrice = Convert.ToDecimal(selectedRow.Cells["UnitPrice"].Value);
                    updatedItem.IsComplete = Convert.ToBoolean(selectedRow.Cells["IsComplete"].Value);

                    await PutDataToApiAsync(updatedItem);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to update.");
            }
        }

        private async Task button4_ClickAsync(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridView2.SelectedRows[0];
                var itemToDelete = selectedRow.DataBoundItem as TodoItem;

                if (itemToDelete != null)
                {
                    await DeleteDataFromApiAsync(itemToDelete.Id);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }
    }

    public class TodoItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Brand { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsComplete { get; set; }
    }
}
