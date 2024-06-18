namespace Abacus.Reports
{
    partial class LogoStationEthnos
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogoStationEthnos));
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            this.sqlDataSource1 = new Telerik.Reporting.SqlDataSource();
            this.sCHOOL_PHONECaptionTextBox = new Telerik.Reporting.TextBox();
            this.detail = new Telerik.Reporting.DetailSection();
            this.sCHOOL_EMAILDataTextBox = new Telerik.Reporting.TextBox();
            this.sCHOOL_FAXDataTextBox = new Telerik.Reporting.TextBox();
            this.sCHOOL_INFODataTextBox = new Telerik.Reporting.TextBox();
            this.sCHOOL_NAMEDataTextBox = new Telerik.Reporting.TextBox();
            this.sCHOOL_PHONEDataTextBox = new Telerik.Reporting.TextBox();
            this.pictureBox1 = new Telerik.Reporting.PictureBox();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionString = "Abacus.Properties.Settings.DBConnectionString";
            this.sqlDataSource1.Name = "sqlDataSource1";
            this.sqlDataSource1.SelectCommand = "SELECT        repSTATION_LOGO.*\r\nFROM            repSTATION_LOGO";
            // 
            // sCHOOL_PHONECaptionTextBox
            // 
            this.sCHOOL_PHONECaptionTextBox.CanGrow = true;
            this.sCHOOL_PHONECaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.0744422348288936E-07D), Telerik.Reporting.Drawing.Unit.Cm(5.7060341835021973D));
            this.sCHOOL_PHONECaptionTextBox.Name = "sCHOOL_PHONECaptionTextBox";
            this.sCHOOL_PHONECaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.0470825433731079D), Telerik.Reporting.Drawing.Unit.Cm(0.50563287734985352D));
            this.sCHOOL_PHONECaptionTextBox.Style.Font.Bold = true;
            this.sCHOOL_PHONECaptionTextBox.Style.Font.Name = "Calibri";
            this.sCHOOL_PHONECaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.sCHOOL_PHONECaptionTextBox.StyleName = "Caption";
            this.sCHOOL_PHONECaptionTextBox.Value = "тГК.:";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(7.2235331535339355D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.sCHOOL_EMAILDataTextBox,
            this.sCHOOL_FAXDataTextBox,
            this.sCHOOL_INFODataTextBox,
            this.sCHOOL_NAMEDataTextBox,
            this.sCHOOL_PHONEDataTextBox,
            this.pictureBox1,
            this.textBox1,
            this.sCHOOL_PHONECaptionTextBox,
            this.textBox2,
            this.textBox3,
            this.textBox4,
            this.textBox5,
            this.textBox6,
            this.textBox7,
            this.textBox8,
            this.textBox9});
            this.detail.Name = "detail";
            // 
            // sCHOOL_EMAILDataTextBox
            // 
            this.sCHOOL_EMAILDataTextBox.CanGrow = true;
            this.sCHOOL_EMAILDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.047283411026001D), Telerik.Reporting.Drawing.Unit.Cm(6.71790075302124D));
            this.sCHOOL_EMAILDataTextBox.Name = "sCHOOL_EMAILDataTextBox";
            this.sCHOOL_EMAILDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.7527170181274414D), Telerik.Reporting.Drawing.Unit.Cm(0.50563246011734009D));
            this.sCHOOL_EMAILDataTextBox.Style.Font.Name = "Calibri";
            this.sCHOOL_EMAILDataTextBox.StyleName = "Data";
            this.sCHOOL_EMAILDataTextBox.Value = "= Fields.EMAIL";
            // 
            // sCHOOL_FAXDataTextBox
            // 
            this.sCHOOL_FAXDataTextBox.CanGrow = true;
            this.sCHOOL_FAXDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.047283411026001D), Telerik.Reporting.Drawing.Unit.Cm(6.2118673324584961D));
            this.sCHOOL_FAXDataTextBox.Name = "sCHOOL_FAXDataTextBox";
            this.sCHOOL_FAXDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.7527179718017578D), Telerik.Reporting.Drawing.Unit.Cm(0.50583350658416748D));
            this.sCHOOL_FAXDataTextBox.Style.Font.Name = "Calibri";
            this.sCHOOL_FAXDataTextBox.StyleName = "Data";
            this.sCHOOL_FAXDataTextBox.Value = "= Fields.жан";
            // 
            // sCHOOL_INFODataTextBox
            // 
            this.sCHOOL_INFODataTextBox.CanGrow = true;
            this.sCHOOL_INFODataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.3002002239227295D), Telerik.Reporting.Drawing.Unit.Cm(5.20020055770874D));
            this.sCHOOL_INFODataTextBox.Name = "sCHOOL_INFODataTextBox";
            this.sCHOOL_INFODataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.4997997283935547D), Telerik.Reporting.Drawing.Unit.Cm(0.50563287734985352D));
            this.sCHOOL_INFODataTextBox.Style.Font.Name = "Calibri";
            this.sCHOOL_INFODataTextBox.StyleName = "Data";
            this.sCHOOL_INFODataTextBox.Value = "= Fields.диавеияистгс";
            // 
            // sCHOOL_NAMEDataTextBox
            // 
            this.sCHOOL_NAMEDataTextBox.CanGrow = true;
            this.sCHOOL_NAMEDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(4.1999998092651367D));
            this.sCHOOL_NAMEDataTextBox.Name = "sCHOOL_NAMEDataTextBox";
            this.sCHOOL_NAMEDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.8000001907348633D), Telerik.Reporting.Drawing.Unit.Cm(0.494166761636734D));
            this.sCHOOL_NAMEDataTextBox.Style.Font.Bold = true;
            this.sCHOOL_NAMEDataTextBox.Style.Font.Name = "Calibri";
            this.sCHOOL_NAMEDataTextBox.StyleName = "Data";
            this.sCHOOL_NAMEDataTextBox.Value = "= \"бяежомгпиайос стахлос \" + Substr(Fields.епымулиа, 4, Len(Fields.епымулиа))";
            // 
            // sCHOOL_PHONEDataTextBox
            // 
            this.sCHOOL_PHONEDataTextBox.CanGrow = true;
            this.sCHOOL_PHONEDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.047283411026001D), Telerik.Reporting.Drawing.Unit.Cm(5.7060341835021973D));
            this.sCHOOL_PHONEDataTextBox.Name = "sCHOOL_PHONEDataTextBox";
            this.sCHOOL_PHONEDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.7527179718017578D), Telerik.Reporting.Drawing.Unit.Cm(0.50563287734985352D));
            this.sCHOOL_PHONEDataTextBox.Style.Font.Name = "Calibri";
            this.sCHOOL_PHONEDataTextBox.StyleName = "Data";
            this.sCHOOL_PHONEDataTextBox.Value = "= Fields.цяаллатеиа";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.04728364944458D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.pictureBox1.MimeType = "image/gif";
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.7470835447311401D), Telerik.Reporting.Drawing.Unit.Cm(1.4941666126251221D));
            this.pictureBox1.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.Stretch;
            this.pictureBox1.Value = ((object)(resources.GetObject("pictureBox1.Value")));
            // 
            // textBox1
            // 
            this.textBox1.CanGrow = true;
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(4.6943674087524414D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.8000001907348633D), Telerik.Reporting.Drawing.Unit.Cm(0.50563281774520874D));
            this.textBox1.Style.Font.Name = "Calibri";
            this.textBox1.StyleName = "Data";
            this.textBox1.Value = "= Fields.тав_диеухумсг";
            // 
            // textBox2
            // 
            this.textBox2.CanGrow = true;
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00020024616969749332D), Telerik.Reporting.Drawing.Unit.Cm(5.20020055770874D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.299799919128418D), Telerik.Reporting.Drawing.Unit.Cm(0.50563287734985352D));
            this.textBox2.Style.Font.Bold = true;
            this.textBox2.Style.Font.Name = "Calibri";
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox2.StyleName = "Caption";
            this.textBox2.Value = "пКГЯОЖОЯъЕР:";
            // 
            // textBox3
            // 
            this.textBox3.CanGrow = true;
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.0744422348288936E-07D), Telerik.Reporting.Drawing.Unit.Cm(6.2118673324584961D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.0470825433731079D), Telerik.Reporting.Drawing.Unit.Cm(0.50583314895629883D));
            this.textBox3.Style.Font.Bold = true;
            this.textBox3.Style.Font.Name = "Calibri";
            this.textBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox3.StyleName = "Caption";
            this.textBox3.Value = "Fax.:";
            // 
            // textBox4
            // 
            this.textBox4.CanGrow = true;
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(6.71790075302124D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.0470833778381348D), Telerik.Reporting.Drawing.Unit.Cm(0.50563287734985352D));
            this.textBox4.Style.Font.Bold = true;
            this.textBox4.Style.Font.Name = "Calibri";
            this.textBox4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox4.StyleName = "Caption";
            this.textBox4.Value = "E-mail:";
            // 
            // textBox5
            // 
            this.textBox5.CanGrow = true;
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(2.9943666458129883D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.2470831871032715D), Telerik.Reporting.Drawing.Unit.Cm(0.499799907207489D));
            this.textBox5.Style.Font.Bold = true;
            this.textBox5.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(4D);
            this.textBox5.StyleName = "Data";
            this.textBox5.Value = "ояцамислос апасвокгсгс";
            // 
            // textBox6
            // 
            this.textBox6.CanGrow = true;
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(3.4943664073944092D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.2470831871032715D), Telerik.Reporting.Drawing.Unit.Cm(0.499799907207489D));
            this.textBox6.Style.Font.Bold = true;
            this.textBox6.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(4D);
            this.textBox6.StyleName = "Data";
            this.textBox6.Value = "еяцатийоу думалийоу";
            // 
            // textBox7
            // 
            this.textBox7.CanGrow = true;
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(1.4943667650222778D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.2470831871032715D), Telerik.Reporting.Drawing.Unit.Cm(0.499799907207489D));
            this.textBox7.Style.Font.Bold = true;
            this.textBox7.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(4D);
            this.textBox7.StyleName = "Data";
            this.textBox7.Value = "еккгмийг дглойяатиа";
            // 
            // textBox8
            // 
            this.textBox8.CanGrow = true;
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(1.9943662881851196D));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.2470831871032715D), Telerik.Reporting.Drawing.Unit.Cm(0.499799907207489D));
            this.textBox8.Style.Font.Bold = true;
            this.textBox8.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(4D);
            this.textBox8.StyleName = "Data";
            this.textBox8.Value = "упоуяцеио еяцасиас";
            // 
            // textBox9
            // 
            this.textBox9.CanGrow = true;
            this.textBox9.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(2.4943664073944092D));
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.2470831871032715D), Telerik.Reporting.Drawing.Unit.Cm(0.499799907207489D));
            this.textBox9.Style.Font.Bold = true;
            this.textBox9.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(4D);
            this.textBox9.StyleName = "Data";
            this.textBox9.Value = "йаи йоимымийым упохесеым";
            // 
            // LogoStationEthnos
            // 
            this.DataSource = this.sqlDataSource1;
            this.Filters.Add(new Telerik.Reporting.Filter("=Fields.стахлос_йыд", Telerik.Reporting.FilterOperator.Equal, "=Parameters.stationID.Value"));
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detail});
            this.Name = "LogoIek";
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(3D), Telerik.Reporting.Drawing.Unit.Mm(3D), Telerik.Reporting.Drawing.Unit.Mm(3D), Telerik.Reporting.Drawing.Unit.Mm(3D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.AllowNull = true;
            reportParameter1.AutoRefresh = true;
            reportParameter1.AvailableValues.DataSource = this.sqlDataSource1;
            reportParameter1.AvailableValues.ValueMember = "= Fields.стахлос_йыд";
            reportParameter1.Name = "stationID";
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.Integer;
            this.ReportParameters.Add(reportParameter1);
            this.Style.BackgroundColor = System.Drawing.Color.White;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Title")});
            styleRule1.Style.Color = System.Drawing.Color.Black;
            styleRule1.Style.Font.Bold = true;
            styleRule1.Style.Font.Italic = false;
            styleRule1.Style.Font.Name = "Tahoma";
            styleRule1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(18D);
            styleRule1.Style.Font.Strikeout = false;
            styleRule1.Style.Font.Underline = false;
            styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Caption")});
            styleRule2.Style.Color = System.Drawing.Color.Black;
            styleRule2.Style.Font.Name = "Tahoma";
            styleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            styleRule2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Data")});
            styleRule3.Style.Font.Name = "Tahoma";
            styleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            styleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("PageInfo")});
            styleRule4.Style.Font.Name = "Tahoma";
            styleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            styleRule4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4});
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(8.80000114440918D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.SqlDataSource sqlDataSource1;
        private Telerik.Reporting.TextBox sCHOOL_PHONECaptionTextBox;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.TextBox sCHOOL_EMAILDataTextBox;
        private Telerik.Reporting.TextBox sCHOOL_FAXDataTextBox;
        private Telerik.Reporting.TextBox sCHOOL_INFODataTextBox;
        private Telerik.Reporting.TextBox sCHOOL_NAMEDataTextBox;
        private Telerik.Reporting.TextBox sCHOOL_PHONEDataTextBox;
        private Telerik.Reporting.PictureBox pictureBox1;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.TextBox textBox9;

    }
}