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
using MovieApplicationResources;

namespace MovieApplication.Modules
{
	public partial class RentalsModule: BaseModule
	{
        public override string ModuleName => "Rentals";

        public override string ModuleGroup => "Movie Rentals";

        public override Image ModuleImage => ImagesHelper.GetImage("Sale", ImagesHelper.ImageSize.Large);

        public RentalsModule()
		{
            InitializeComponent();
		}
	}
}
