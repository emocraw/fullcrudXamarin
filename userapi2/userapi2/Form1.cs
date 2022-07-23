using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace userapi2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string idWhileClick;
        private async void addbtn_Click(object sender, EventArgs e)
        {
            using (var client = new HttpClient())
            {
                
                var data = "{" +
                           " \"username\" : \"" + usertxt.Text + "\" " +
                           ", \"password\" : \"" +passwordtxt.Text + "\"" +
                           " }";
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("http://localhost/usermanagement/api/postuser.php", content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();

                    dynamic res = JsonConvert.DeserializeObject(result);
                    if (res["status"] == "ok")
                    {
                        statusaddlbl.Text = "บันทึกข้อมูลสำเร็จ";
                    }
                    else
                    {
                        statusaddlbl.Text = "ข้อมูลซ้ำ";
                    }
                }
                else
                {
                    statusaddlbl.Text = "ไม่สามารถเชื่อมต่อฐานข้อมูล";
                }
                selectall();
            }
        }

        public class Root
        {
            public int id { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public string type { get; set; }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                 usertxt.Text = dataGridView1.Rows[e.RowIndex].Cells["username"].Value.ToString();
                 passwordtxt.Text = dataGridView1.Rows[e.RowIndex].Cells["password"].Value.ToString();
                idWhileClick = dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString();


            }
        }

        private async void selectall()
        {
            
            using (var client = new HttpClient())
            {
                HttpResponseMessage res = await client.GetAsync("http://localhost/usermanagement/api/getuser.php");
                if (res.IsSuccessStatusCode)
                {
                    string result = await res.Content.ReadAsStringAsync();

                    dynamic resObj = JsonConvert.DeserializeObject(result);
              
                    dataGridView1.DataSource = resObj;
                }
            }      
        }

        private async void searchbtn_Click(object sender, EventArgs e)
        {
            selectall();
        }

        private async void edit_Click(object sender, EventArgs e)
        {
            using (var client = new HttpClient())
            {

                var data = "{" +
                           " \"username\" : \"" + usertxt.Text + "\" " +
                           ", \"password\" : \"" + passwordtxt.Text + "\" " +
                           ", \"idWhileClick\" : \"" + idWhileClick + "\" }";
                var content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync("http://localhost/usermanagement/api/putuser.php", content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();

                    dynamic res = JsonConvert.DeserializeObject(result);
                    if (res["status"] == "ok")
                    {
                        statusaddlbl.Text = "บันทึกข้อมูลสำเร็จ";
                    }
                    else
                    {
                        statusaddlbl.Text = "ข้อมูลซ้ำ";
                    }
                }
                else
                {
                    statusaddlbl.Text = "ไม่สามารถเชื่อมต่อฐานข้อมูล";
                }
                selectall();
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.DeleteAsync("http://localhost/usermanagement/api/delete.php?id="+idWhileClick);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();

                    dynamic res = JsonConvert.DeserializeObject(result);
                    if (res["status"] == "ok")
                    {
                        statusaddlbl.Text = "ลบข้อมูลสำเร็จ";
                    }
                }
                else
                {
                    statusaddlbl.Text = "ไม่สามารถเชื่อมต่อฐานข้อมูล";
                }
                selectall();
            }
        }

       
    }
}
