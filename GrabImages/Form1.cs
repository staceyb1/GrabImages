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
    }
}
