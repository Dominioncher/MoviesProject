namespace MovieApplication.UserControls
{
    partial class MovieDetailsControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.editImage = new MovieApplication.Controls.ImagePickerControl();
            this.editDescription = new DevExpress.XtraEditors.MemoEdit();
            this.editReleaseDate = new DevExpress.XtraEditors.DateEdit();
            this.editTitle = new DevExpress.XtraEditors.MemoEdit();
            this.editGenre = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.editRaiting = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.tabbedControlGroup1 = new DevExpress.XtraLayout.TabbedControlGroup();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editDescription.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editReleaseDate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editReleaseDate.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editTitle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editGenre.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editRaiting.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.EmptyAreaImageOptions.ImagePadding = new System.Windows.Forms.Padding(50, 46, 50, 46);
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.ribbonControl1.OptionsMenuMinWidth = 545;
            this.ribbonControl1.Size = new System.Drawing.Size(1137, 260);
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.editImage);
            this.layoutControl1.Controls.Add(this.editDescription);
            this.layoutControl1.Controls.Add(this.editReleaseDate);
            this.layoutControl1.Controls.Add(this.editTitle);
            this.layoutControl1.Controls.Add(this.editGenre);
            this.layoutControl1.Controls.Add(this.editRaiting);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 260);
            this.layoutControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(954, 383, 812, 500);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(1137, 491);
            this.layoutControl1.TabIndex = 1;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // editImage
            // 
            this.editImage.EditValue = null;
            this.editImage.Location = new System.Drawing.Point(15, 203);
            this.editImage.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.editImage.Name = "editImage";
            this.editImage.Size = new System.Drawing.Size(374, 275);
            this.editImage.TabIndex = 10;
            // 
            // editDescription
            // 
            this.editDescription.Location = new System.Drawing.Point(537, 191);
            this.editDescription.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.editDescription.MenuManager = this.ribbonControl1;
            this.editDescription.Name = "editDescription";
            this.editDescription.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.editDescription.Size = new System.Drawing.Size(556, 261);
            this.editDescription.StyleController = this.layoutControl1;
            this.editDescription.TabIndex = 5;
            // 
            // editReleaseDate
            // 
            this.editReleaseDate.EditValue = null;
            this.editReleaseDate.Location = new System.Drawing.Point(537, 71);
            this.editReleaseDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.editReleaseDate.MenuManager = this.ribbonControl1;
            this.editReleaseDate.Name = "editReleaseDate";
            this.editReleaseDate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.editReleaseDate.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.editReleaseDate.Properties.DisplayFormat.FormatString = "";
            this.editReleaseDate.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.editReleaseDate.Properties.EditFormat.FormatString = "";
            this.editReleaseDate.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.editReleaseDate.Properties.MaskSettings.Set("mask", "d");
            this.editReleaseDate.Size = new System.Drawing.Size(205, 32);
            this.editReleaseDate.StyleController = this.layoutControl1;
            this.editReleaseDate.TabIndex = 7;
            // 
            // editTitle
            // 
            this.editTitle.Location = new System.Drawing.Point(28, 51);
            this.editTitle.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.editTitle.MenuManager = this.ribbonControl1;
            this.editTitle.Name = "editTitle";
            this.editTitle.Properties.LinesCount = 1;
            this.editTitle.Properties.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.editTitle.Size = new System.Drawing.Size(348, 136);
            this.editTitle.StyleController = this.layoutControl1;
            this.editTitle.TabIndex = 4;
            this.editTitle.EditValueChanged += new System.EventHandler(this.editTitle_EditValueChanged);
            // 
            // editGenre
            // 
            this.editGenre.Location = new System.Drawing.Point(537, 131);
            this.editGenre.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.editGenre.MenuManager = this.ribbonControl1;
            this.editGenre.Name = "editGenre";
            this.editGenre.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.editGenre.Size = new System.Drawing.Size(556, 32);
            this.editGenre.StyleController = this.layoutControl1;
            this.editGenre.TabIndex = 9;
            // 
            // editRaiting
            // 
            this.editRaiting.Location = new System.Drawing.Point(887, 71);
            this.editRaiting.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.editRaiting.MenuManager = this.ribbonControl1;
            this.editRaiting.Name = "editRaiting";
            this.editRaiting.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.editRaiting.Properties.PopupSizeable = true;
            this.editRaiting.Size = new System.Drawing.Size(206, 32);
            this.editRaiting.StyleController = this.layoutControl1;
            this.editRaiting.TabIndex = 8;
            this.editRaiting.EditValueChanged += new System.EventHandler(this.editRaiting_EditValueChanged);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.tabbedControlGroup1,
            this.layoutControlItem3});
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Size = new System.Drawing.Size(1137, 491);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.editTitle;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(129, 187);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(380, 190);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.Spacing = new DevExpress.XtraLayout.Utils.Padding(13, 13, 12, 12);
            this.layoutControlItem1.Text = "Title";
            this.layoutControlItem1.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(98, 20);
            // 
            // tabbedControlGroup1
            // 
            this.tabbedControlGroup1.Location = new System.Drawing.Point(380, 0);
            this.tabbedControlGroup1.Name = "tabbedControlGroup1";
            this.tabbedControlGroup1.SelectedTabPage = this.layoutControlGroup2;
            this.tabbedControlGroup1.Size = new System.Drawing.Size(733, 469);
            this.tabbedControlGroup1.TabPages.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup2,
            this.layoutControlGroup3});
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Size = new System.Drawing.Size(701, 409);
            this.layoutControlGroup2.Text = "Main";
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.editDescription;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 120);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(701, 289);
            this.layoutControlItem2.Spacing = new DevExpress.XtraLayout.Utils.Padding(13, 13, 12, 12);
            this.layoutControlItem2.Text = "Description";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(98, 20);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.editReleaseDate;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(350, 60);
            this.layoutControlItem4.Spacing = new DevExpress.XtraLayout.Utils.Padding(13, 13, 12, 12);
            this.layoutControlItem4.Text = "Release Date";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(98, 20);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.editRaiting;
            this.layoutControlItem5.Location = new System.Drawing.Point(350, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(351, 60);
            this.layoutControlItem5.Spacing = new DevExpress.XtraLayout.Utils.Padding(13, 13, 12, 12);
            this.layoutControlItem5.Text = "Raiting";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(98, 20);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.editGenre;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 60);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(701, 60);
            this.layoutControlItem6.Spacing = new DevExpress.XtraLayout.Utils.Padding(13, 13, 12, 12);
            this.layoutControlItem6.Text = "Genre";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(98, 20);
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup3.Name = "layoutControlGroup3";
            this.layoutControlGroup3.Size = new System.Drawing.Size(701, 409);
            this.layoutControlGroup3.Text = "Details";
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.editImage;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 190);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(380, 279);
            this.layoutControlItem3.TextVisible = false;
            // 
            // MovieDetailsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "MovieDetailsControl";
            this.Size = new System.Drawing.Size(1137, 751);
            this.Controls.SetChildIndex(this.ribbonControl1, 0);
            this.Controls.SetChildIndex(this.layoutControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editDescription.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editReleaseDate.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editReleaseDate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editTitle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editGenre.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editRaiting.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.MemoEdit editDescription;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraEditors.DateEdit editReleaseDate;
        private DevExpress.XtraLayout.TabbedControlGroup tabbedControlGroup1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup3;
        private Controls.ImagePickerControl editImage;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.MemoEdit editTitle;
        private DevExpress.XtraEditors.CheckedComboBoxEdit editGenre;
        private DevExpress.XtraEditors.ImageComboBoxEdit editRaiting;
    }
}
