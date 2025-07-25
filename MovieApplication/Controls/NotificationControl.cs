using DevExpress.XtraBars.Alerter;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieApplication.Controls
{
    public class NotificationControl : AlertControl
    {
        public Padding Padding { get; set; } = new Padding(0);

        private XtraForm _parent;

        public void Initialize(XtraForm parent)
        {
            _parent = parent;
            parent.Resize += Parent_Resize;
            parent.FormClosing += Parent_FormClosing;
            parent.Move += Parent_Move;
            BeforeFormShow += NotificationControl_BeforeFormShow;
            FormLocation = AlertFormLocation.BottomRight;
            ShowPinButton = false;
        }

        private void NotificationControl_BeforeFormShow(object sender, AlertFormEventArgs e)
        {
            e.AlertForm.OpacityLevel = 1;
            var x = _parent.Location.X + _parent.Size.Width - e.AlertForm.Width - Padding.Right;
            var y = _parent.Location.Y + _parent.Size.Height - e.AlertForm.Height - Padding.Bottom;
            e.Location = new Point(x, y);
        }

        private void Parent_Move(object sender, EventArgs e)
        {
            UpdateFormsLocation();
        }

        private void Parent_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            foreach (var form in AlertFormList)
            {
                form.Close();
            }
        }

        private void Parent_Resize(object sender, EventArgs e)
        {
            UpdateFormsLocation();
        }

        private void UpdateFormsLocation()
        {
            foreach (var form in AlertFormList)
            {
                var x = _parent.Location.X + _parent.Size.Width - form.Width - Padding.Right;
                var y = _parent.Location.Y + _parent.Size.Height - form.Height - Padding.Bottom;
                form.Location = new Point(x, y);
            }
        }
    }
}
