namespace Abacus.Reports.Data.Documents
{
    partial class MealsList
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.TypeReportSource typeReportSource1 = new Telerik.Reporting.TypeReportSource();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MealsList));
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.Group group2 = new Telerik.Reporting.Group();
            Telerik.Reporting.Group group3 = new Telerik.Reporting.Group();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            this.Â–ŸÕ’Ã…¡GroupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.Â–ŸÕ’Ã…¡GroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.subReport1 = new Telerik.Reporting.SubReport();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.Â…ƒœ”GroupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.Â…ƒœ”GroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.Â…ƒœ”CaptionTextBox = new Telerik.Reporting.TextBox();
            this.Â…ƒœ”DataTextBox = new Telerik.Reporting.TextBox();
            this.labelsGroupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.labelsGroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.„≈’Ã¡CaptionTextBox = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.shape1 = new Telerik.Reporting.Shape();
            this.sqlStations = new Telerik.Reporting.SqlDataSource();
            this.sqlMealTypes = new Telerik.Reporting.SqlDataSource();
            this.sqlDataSource = new Telerik.Reporting.SqlDataSource();
            this.pageFooter = new Telerik.Reporting.PageFooterSection();
            this.pictureBox3 = new Telerik.Reporting.PictureBox();
            this.textBox19 = new Telerik.Reporting.TextBox();
            this.pageInfoTextBox = new Telerik.Reporting.TextBox();
            this.detail = new Telerik.Reporting.DetailSection();
            this.„≈’Ã¡DataTextBox = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.shape2 = new Telerik.Reporting.Shape();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Â–ŸÕ’Ã…¡GroupFooterSection
            // 
            this.Â–ŸÕ’Ã…¡GroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.30000025033950806D);
            this.Â–ŸÕ’Ã…¡GroupFooterSection.Name = "Â–ŸÕ’Ã…¡GroupFooterSection";
            this.Â–ŸÕ’Ã…¡GroupFooterSection.Style.Visible = false;
            // 
            // Â–ŸÕ’Ã…¡GroupHeaderSection
            // 
            this.Â–ŸÕ’Ã…¡GroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(3.2470831871032715D);
            this.Â–ŸÕ’Ã…¡GroupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.subReport1,
            this.textBox1});
            this.Â–ŸÕ’Ã…¡GroupHeaderSection.Name = "Â–ŸÕ’Ã…¡GroupHeaderSection";
            this.Â–ŸÕ’Ã…¡GroupHeaderSection.PrintOnEveryPage = true;
            // 
            // subReport1
            // 
            this.subReport1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.subReport1.Name = "subReport1";
            typeReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("stationID", "=Fields.¬Õ”"));
            typeReportSource1.TypeName = "Abacus.Reports.Data.LogoStationShort, Abacus, Version=1.0.0.0, Culture=neutral, P" +
    "ublicKeyToken=null";
            this.subReport1.ReportSource = typeReportSource1;
            this.subReport1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8000001907348633D), Telerik.Reporting.Drawing.Unit.Cm(1.999900221824646D));
            // 
            // textBox1
            // 
            this.textBox1.CanGrow = true;
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(2.4341666698455811D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(16.735414505004883D), Telerik.Reporting.Drawing.Unit.Cm(0.712917149066925D));
            this.textBox1.Style.Font.Bold = true;
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox1.StyleName = "Caption";
            this.textBox1.Value = "= \" ¡‘¡Àœ√œ” √≈’Ã¡‘ŸÕ ¬—≈÷œÕ«–…¡ œ’ ”‘¡»Ãœ’ \" + Substr(Fields.≈–ŸÕ’Ã…¡, 4, len(Fi" +
    "elds.≈–ŸÕ’Ã…¡))";
            // 
            // Â…ƒœ”GroupFooterSection
            // 
            this.Â…ƒœ”GroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.39999979734420776D);
            this.Â…ƒœ”GroupFooterSection.Name = "Â…ƒœ”GroupFooterSection";
            this.Â…ƒœ”GroupFooterSection.Style.Visible = false;
            // 
            // Â…ƒœ”GroupHeaderSection
            // 
            this.Â…ƒœ”GroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.90000033378601074D);
            this.Â…ƒœ”GroupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.Â…ƒœ”CaptionTextBox,
            this.Â…ƒœ”DataTextBox});
            this.Â…ƒœ”GroupHeaderSection.Name = "Â…ƒœ”GroupHeaderSection";
            this.Â…ƒœ”GroupHeaderSection.PrintOnEveryPage = true;
            // 
            // Â…ƒœ”CaptionTextBox
            // 
            this.Â…ƒœ”CaptionTextBox.CanGrow = true;
            this.Â…ƒœ”CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.Â…ƒœ”CaptionTextBox.Name = "Â…ƒœ”CaptionTextBox";
            this.Â…ƒœ”CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.5470831394195557D), Telerik.Reporting.Drawing.Unit.Cm(0.747083306312561D));
            this.Â…ƒœ”CaptionTextBox.Style.BackgroundColor = System.Drawing.Color.LightGray;
            this.Â…ƒœ”CaptionTextBox.Style.Font.Bold = true;
            this.Â…ƒœ”CaptionTextBox.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            this.Â…ƒœ”CaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.Â…ƒœ”CaptionTextBox.StyleName = "Caption";
            this.Â…ƒœ”CaptionTextBox.Value = "≈…ƒœ” √≈’Ã¡‘œ” :";
            // 
            // Â…ƒœ”DataTextBox
            // 
            this.Â…ƒœ”DataTextBox.CanGrow = true;
            this.Â…ƒœ”DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.6001999378204346D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.Â…ƒœ”DataTextBox.Name = "Â…ƒœ”DataTextBox";
            this.Â…ƒœ”DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(13.188133239746094D), Telerik.Reporting.Drawing.Unit.Cm(0.747083306312561D));
            this.Â…ƒœ”DataTextBox.Style.BackgroundColor = System.Drawing.Color.LightGray;
            this.Â…ƒœ”DataTextBox.Style.Font.Bold = true;
            this.Â…ƒœ”DataTextBox.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            this.Â…ƒœ”DataTextBox.StyleName = "Data";
            this.Â…ƒœ”DataTextBox.Value = "=Fields.≈…ƒœ”";
            // 
            // labelsGroupFooterSection
            // 
            this.labelsGroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.36750823259353638D);
            this.labelsGroupFooterSection.Name = "labelsGroupFooterSection";
            this.labelsGroupFooterSection.Style.Visible = false;
            // 
            // labelsGroupHeaderSection
            // 
            this.labelsGroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.88540822267532349D);
            this.labelsGroupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.„≈’Ã¡CaptionTextBox,
            this.textBox2,
            this.shape1});
            this.labelsGroupHeaderSection.Name = "labelsGroupHeaderSection";
            this.labelsGroupHeaderSection.PrintOnEveryPage = true;
            // 
            // „≈’Ã¡CaptionTextBox
            // 
            this.„≈’Ã¡CaptionTextBox.CanGrow = true;
            this.„≈’Ã¡CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.253116250038147D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.„≈’Ã¡CaptionTextBox.Name = "„≈’Ã¡CaptionTextBox";
            this.„≈’Ã¡CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(15.588133811950684D), Telerik.Reporting.Drawing.Unit.Cm(0.69999963045120239D));
            this.„≈’Ã¡CaptionTextBox.Style.Font.Bold = true;
            this.„≈’Ã¡CaptionTextBox.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            this.„≈’Ã¡CaptionTextBox.StyleName = "Caption";
            this.„≈’Ã¡CaptionTextBox.Value = "–≈—…√—¡÷« √≈’Ã¡‘œ”";
            // 
            // textBox2
            // 
            this.textBox2.CanGrow = true;
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.1999998092651367D), Telerik.Reporting.Drawing.Unit.Cm(0.69999879598617554D));
            this.textBox2.Style.Font.Bold = true;
            this.textBox2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(0D);
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox2.StyleName = "Caption";
            this.textBox2.Value = "¡/¡";
            // 
            // shape1
            // 
            this.shape1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.75311654806137085D));
            this.shape1.Name = "shape1";
            this.shape1.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(16.735416412353516D), Telerik.Reporting.Drawing.Unit.Cm(0.13229165971279144D));
            // 
            // sqlStations
            // 
            this.sqlStations.ConnectionString = "Abacus.Properties.Settings.DBConnectionString";
            this.sqlStations.Name = "sqlStations";
            this.sqlStations.SelectCommand = "SELECT        ”‘¡»Ãœ”_ Ÿƒ, ≈–ŸÕ’Ã…¡\r\nFROM            ”’”_”‘¡»Ãœ…\r\nORDER BY ≈–ŸÕ’Ã" +
    "…¡";
            // 
            // sqlMealTypes
            // 
            this.sqlMealTypes.ConnectionString = "Abacus.Properties.Settings.DBConnectionString";
            this.sqlMealTypes.Name = "sqlMealTypes";
            this.sqlMealTypes.SelectCommand = "SELECT        √≈’Ã¡_≈…ƒœ”_ Ÿƒ, √≈’Ã¡_≈…ƒœ”\r\nFROM            √≈’Ã¡‘¡_≈…ƒ«";
            // 
            // sqlDataSource
            // 
            this.sqlDataSource.ConnectionString = "Abacus.Properties.Settings.DBConnectionString";
            this.sqlDataSource.Name = "sqlDataSource";
            this.sqlDataSource.SelectCommand = "SELECT        ¬Õ”, ≈–ŸÕ’Ã…¡, ≈…ƒœ”, √≈’Ã¡\r\nFROM            rep√≈’Ã¡‘¡_œÀ¡\r\nORDER " +
    "BY ≈…ƒœ” DESC, √≈’Ã¡";
            // 
            // pageFooter
            // 
            this.pageFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.5470849871635437D);
            this.pageFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pictureBox3,
            this.textBox19,
            this.pageInfoTextBox});
            this.pageFooter.Name = "pageFooter";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.pictureBox3.MimeType = "image/png";
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.54708343744277954D), Telerik.Reporting.Drawing.Unit.Cm(0.54698455333709717D));
            this.pictureBox3.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.Stretch;
            this.pictureBox3.Value = ((object)(resources.GetObject("pictureBox3.Value")));
            // 
            // textBox19
            // 
            this.textBox19.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.60019993782043457D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(13.3996000289917D), Telerik.Reporting.Drawing.Unit.Cm(0.5470842719078064D));
            this.textBox19.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox19.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.textBox19.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.textBox19.StyleName = "PageInfo";
            this.textBox19.Value = "–ÎÁÒÔˆÔÒÈ·Í¸ ”˝ÛÙÁÏ· Abacus - ƒÈÂ˝ËıÌÛÁ ¡Ò˜ÈÍﬁÚ ≈·„„ÂÎÏ·ÙÈÍﬁÚ ≈Í·ﬂ‰ÂıÛÁÚ &  ·Ù‹" +
    "ÒÙÈÛÁÚ";
            // 
            // pageInfoTextBox
            // 
            this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(13.999999046325684D), Telerik.Reporting.Drawing.Unit.Cm(0.00010093052696902305D));
            this.pageInfoTextBox.Name = "pageInfoTextBox";
            this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.7883329391479492D), Telerik.Reporting.Drawing.Unit.Cm(0.546984076499939D));
            this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.pageInfoTextBox.StyleName = "PageInfo";
            this.pageInfoTextBox.Value = "=\"”ÂÎ. \" + PageNumber + \"/\" + PageCount";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(0.65291637182235718D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.„≈’Ã¡DataTextBox,
            this.textBox3,
            this.shape2});
            this.detail.Name = "detail";
            // 
            // „≈’Ã¡DataTextBox
            // 
            this.„≈’Ã¡DataTextBox.CanGrow = true;
            this.„≈’Ã¡DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.253116250038147D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.„≈’Ã¡DataTextBox.Name = "„≈’Ã¡DataTextBox";
            this.„≈’Ã¡DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(15.535215377807617D), Telerik.Reporting.Drawing.Unit.Cm(0.52042406797409058D));
            this.„≈’Ã¡DataTextBox.Style.Font.Name = "Calibri";
            this.„≈’Ã¡DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.„≈’Ã¡DataTextBox.StyleName = "Data";
            this.„≈’Ã¡DataTextBox.Value = "=Fields.√≈’Ã¡";
            // 
            // textBox3
            // 
            this.textBox3.CanGrow = true;
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.00020024616969749332D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.1999998092651367D), Telerik.Reporting.Drawing.Unit.Cm(0.52022379636764526D));
            this.textBox3.Style.Font.Name = "Calibri";
            this.textBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox3.StyleName = "Data";
            this.textBox3.Value = "= RowNumber() + \".\"";
            // 
            // shape2
            // 
            this.shape2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.52062469720840454D));
            this.shape2.Name = "shape2";
            this.shape2.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(16.735416412353516D), Telerik.Reporting.Drawing.Unit.Cm(0.13229165971279144D));
            // 
            // MealsList
            // 
            this.DataSource = this.sqlDataSource;
            this.Filters.Add(new Telerik.Reporting.Filter("=Fields.¬Õ”", Telerik.Reporting.FilterOperator.Equal, "=Parameters.stationID.Value"));
            this.Filters.Add(new Telerik.Reporting.Filter("=Fields.≈…ƒœ”", Telerik.Reporting.FilterOperator.In, "=Parameters.meal_type.Value"));
            group1.GroupFooter = this.Â–ŸÕ’Ã…¡GroupFooterSection;
            group1.GroupHeader = this.Â–ŸÕ’Ã…¡GroupHeaderSection;
            group1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.≈–ŸÕ’Ã…¡"));
            group1.Name = "Â–ŸÕ’Ã…¡Group";
            group2.GroupFooter = this.Â…ƒœ”GroupFooterSection;
            group2.GroupHeader = this.Â…ƒœ”GroupHeaderSection;
            group2.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.≈…ƒœ”"));
            group2.Name = "Â…ƒœ”Group";
            group2.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.≈…ƒœ”", Telerik.Reporting.SortDirection.Desc));
            group3.GroupFooter = this.labelsGroupFooterSection;
            group3.GroupHeader = this.labelsGroupHeaderSection;
            group3.Name = "labelsGroup";
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1,
            group2,
            group3});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.Â–ŸÕ’Ã…¡GroupHeaderSection,
            this.Â–ŸÕ’Ã…¡GroupFooterSection,
            this.Â…ƒœ”GroupHeaderSection,
            this.Â…ƒœ”GroupFooterSection,
            this.labelsGroupHeaderSection,
            this.labelsGroupFooterSection,
            this.pageFooter,
            this.detail});
            this.Name = "MealsList";
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(20D), Telerik.Reporting.Drawing.Unit.Mm(20D), Telerik.Reporting.Drawing.Unit.Mm(20D), Telerik.Reporting.Drawing.Unit.Mm(20D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.AllowNull = true;
            reportParameter1.AutoRefresh = true;
            reportParameter1.AvailableValues.DataSource = this.sqlStations;
            reportParameter1.AvailableValues.DisplayMember = "= Fields.≈–ŸÕ’Ã…¡";
            reportParameter1.AvailableValues.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.≈–ŸÕ’Ã…¡", Telerik.Reporting.SortDirection.Asc));
            reportParameter1.AvailableValues.ValueMember = "= Fields.”‘¡»Ãœ”_ Ÿƒ";
            reportParameter1.Name = "stationID";
            reportParameter1.Text = "”Ù·ËÏ¸Ú";
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter2.AllowNull = true;
            reportParameter2.AutoRefresh = true;
            reportParameter2.AvailableValues.DataSource = this.sqlMealTypes;
            reportParameter2.AvailableValues.DisplayMember = "= Fields.√≈’Ã¡_≈…ƒœ”";
            reportParameter2.AvailableValues.ValueMember = "= Fields.√≈’Ã¡_≈…ƒœ”";
            reportParameter2.MultiValue = true;
            reportParameter2.Name = "meal_type";
            reportParameter2.Text = "≈ﬂ‰ÔÚ „Â˝Ï·ÙÔÚ";
            reportParameter2.Visible = true;
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
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
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(16.894166946411133D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.SqlDataSource sqlDataSource;
        private Telerik.Reporting.GroupHeaderSection Â–ŸÕ’Ã…¡GroupHeaderSection;
        private Telerik.Reporting.GroupFooterSection Â–ŸÕ’Ã…¡GroupFooterSection;
        private Telerik.Reporting.GroupHeaderSection Â…ƒœ”GroupHeaderSection;
        private Telerik.Reporting.TextBox Â…ƒœ”CaptionTextBox;
        private Telerik.Reporting.TextBox Â…ƒœ”DataTextBox;
        private Telerik.Reporting.GroupFooterSection Â…ƒœ”GroupFooterSection;
        private Telerik.Reporting.GroupHeaderSection labelsGroupHeaderSection;
        private Telerik.Reporting.TextBox „≈’Ã¡CaptionTextBox;
        private Telerik.Reporting.GroupFooterSection labelsGroupFooterSection;
        private Telerik.Reporting.PageFooterSection pageFooter;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.TextBox „≈’Ã¡DataTextBox;
        private Telerik.Reporting.SubReport subReport1;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.Shape shape1;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.Shape shape2;
        private Telerik.Reporting.PictureBox pictureBox3;
        private Telerik.Reporting.TextBox textBox19;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private Telerik.Reporting.SqlDataSource sqlStations;
        private Telerik.Reporting.SqlDataSource sqlMealTypes;

    }
}