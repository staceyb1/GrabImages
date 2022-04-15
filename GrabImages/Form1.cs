using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using RestSharp;
using System.Net;

namespace GrabImages
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGetImage_Click(object sender, EventArgs e)
        {
            using (var db = new COBALTEntities())
            {
                DateTime? date = DateTime.Now.AddDays(-100);
                string imageURL = db.tbLoanImages.Where(x => x.StatusCode == "SU" && x.dt_photo >= date).Select(x => x.WDOCID).FirstOrDefault();
                if (string.IsNullOrWhiteSpace(imageURL))
                {
                    Console.WriteLine("Image path is Empty");
                    return;
                }

                Console.WriteLine(imageURL);
                if (File.Exists(imageURL))
                {
                    this.pbImage.Image = new Bitmap(imageURL);
                }
                else
                {
                    Console.WriteLine("Image path not exists.");
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "JPG files | *.jpg";
            sf.RestoreDirectory = true;
            sf.DefaultExt = "jpg";

            if (sf.ShowDialog() == DialogResult.OK)
            {
                string fileName = sf.FileName;
                this.pbImage.Image.Save(fileName);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            JsonClass jc = new JsonClass
            {
                Date = DateTime.Parse("1/1/2022"),
                Summary = "Hello"
            };
            JsonSerializerOptions jop = new JsonSerializerOptions();
            jop.WriteIndented = true;

            string jsonString = JsonSerializer.Serialize<JsonClass>(jc, jop);
            Console.WriteLine(jsonString);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var client = new RestClient("https://fbvision.cognitiveservices.azure.com/customvision/v3.0/Prediction/77820dee-6d41-40a2-b3fa-1cf114772d35/classify/iterations/Iteration1/image");
            //client..Timeout = -1;
            var request = new RestRequest();
            request.Method = Method.Post;

            var ba = File.ReadAllBytes("./Images/Sump2.jpg");

            var body = Parameter.CreateParameter("file", ba, ParameterType.RequestBody, false);
            
            request.Parameters.AddParameter(body);
            request.AddHeader("Prediction-Key", "8af04695434e478d9d0b109842442f67");
            request.AddHeader("Content-Type", "application/octet-stream");

            var rr = client.ExecutePostAsync(request);
            rr.Wait();
            Console.WriteLine(rr.Result.ResponseStatus.ToString());
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            //using (WebClient client = new WebClient())
            //{
            //    client.UploadFile(address, filePath);
            //}
        }
    }
}
