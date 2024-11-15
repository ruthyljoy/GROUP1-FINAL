using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace M1PartialForms
{
    public partial class Form3 : Form
    {
        private readonly HttpClient _httpClient;

        public Form3()
        {
            InitializeComponent();
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7289/") };

            button2.Click += async (sender, e) => await button2_ClickAsync(sender, e);
        }

        private async Task GetDataFromApiAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<TodoItem>>("api/TodoItems");

                if (response != null)
                {
                    dataGridView1.DataSource = response;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private async Task button2_ClickAsync(object sender, EventArgs e)
        {
            await GetDataFromApiAsync();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Call the GetDataFromApiAsync method
            _ = GetDataFromApiAsync();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }


}
