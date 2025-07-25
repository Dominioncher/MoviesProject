using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MovieApplicationDataBase;

namespace MovieApplication.Controls
{
	public partial class ImagePickerControl: DevExpress.XtraEditors.XtraUserControl
	{
        public object EditValue
        {
            get => pictureEdit1.Image;
            set => pictureEdit1.Image = value as Image;
        }

        public ImagePickerControl()
		{
            InitializeComponent();
		}

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Images |*.png;*.bmp;*.dib;*.jpg;*.jpeg;*.jpe;*.jfif;*.gif;*.tif;*.tiff;*.ico";
                var result = openFileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    var image = Image.FromFile(openFileDialog.FileName);
                    pictureEdit1.Image = image.Resize(new Size(400, 400));
                    image.Dispose();
                    GC.Collect();
                }
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            pictureEdit1.Image = null;
        }
    }
}
