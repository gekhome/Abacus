namespace Abacus.Reports.Data.Documents
{
    partial class MealsPlan
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.TypeReportSource typeReportSource1 = new Telerik.Reporting.TypeReportSource();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MealsPlan));
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.Group group2 = new Telerik.Reporting.Group();
            Telerik.Reporting.Group group3 = new Telerik.Reporting.Group();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter3 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            this.��������GroupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.��������GroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.subReport1 = new Telerik.Reporting.SubReport();
            this.����������GroupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.����������GroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.����������DataTextBox = new Telerik.Reporting.TextBox();
            this.����������CaptionTextBox = new Telerik.Reporting.TextBox();
            this.labelsGroupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.labelsGroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.������CaptionTextBox = new Telerik.Reporting.TextBox();
            this.�����������CaptionTextBox = new Telerik.Reporting.TextBox();
            this.�������CaptionTextBox = new Telerik.Reporting.TextBox();
            this.shape2 = new Telerik.Reporting.Shape();
            this.sqlStations = new Telerik.Reporting.SqlDataSource();
            this.sqlDataSource = new Telerik.Reporting.SqlDataSource();
            this.pageFooter = new Telerik.Reporting.PageFooterSection();
            this.pictureBox3 = new Telerik.Reporting.PictureBox();
            this.textBox19 = new Telerik.Reporting.TextBox();
            this.pageInfoTextBox = new Telerik.Reporting.TextBox();
            this.detail = new Telerik.Reporting.DetailSection();
            this.������DataTextBox = new Telerik.Reporting.TextBox();
            this.�����������DataTextBox = new Telerik.Reporting.TextBox();
            this.�������DataTextBox = new Telerik.Reporting.TextBox();
            this.shape3 = new Telerik.Reporting.Shape();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // ��������GroupFooterSection
            // 
            this.��������GroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.39999979734420776D);
            this.��������GroupFooterSection.Name = "��������GroupFooterSection";
            this.��������GroupFooterSection.Style.Visible = false;
            // 
            // ��������GroupHeaderSection
            // 
            this.��������GroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(2.9000000953674316D);
            this.��������GroupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox1,
            this.subReport1});
            this.��������GroupHeaderSection.Name = "��������GroupHeaderSection";
            // 
            // textBox1
            // 
            this.textBox1.CanGrow = true;
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(2.0002007484436035D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(16.894067764282227D), Telerik.Reporting.Drawing.Unit.Cm(0.79979974031448364D));
            this.textBox1.Style.Font.Bold = true;
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox1.StyleName = "Caption";
            this.textBox1.Value = "=\"��������� ������������ ��� \" + Fields.��������";
            // 
            // subReport1
            // 
            this.subReport1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.subReport1.Name = "subReport1";
            typeReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("stationID", "=Fields.�������_���"));
            typeReportSource1.TypeName = "Abacus.Reports.Data.LogoStationShort, Abacus, Version=1.0.0.0, Culture=neutral, P" +
    "ublicKeyToken=null";
            this.subReport1.ReportSource = typeReportSource1;
            this.subReport1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.3528199195861816D), Telerik.Reporting.Drawing.Unit.Cm(1.999900221824646D));
            // 
            // ����������GroupFooterSection
            // 
            this.����������GroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.40000060200691223D);
            this.����������GroupFooterSection.Name = "����������GroupFooterSection";
            this.����������GroupFooterSection.Style.Visible = false;
            // 
            // ����������GroupHeaderSection
            // 
            this.����������GroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(1.399999737739563D);
            this.����������GroupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox2,
            this.����������DataTextBox,
            this.����������CaptionTextBox});
            this.����������GroupHeaderSection.Name = "����������GroupHeaderSection";
            // 
            // textBox2
            // 
            this.textBox2.CanGrow = true;
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(6.0764741897583008D), Telerik.Reporting.Drawing.Unit.Cm(0.28954383730888367D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.2233254909515381D), Telerik.Reporting.Drawing.Unit.Cm(0.70000004768371582D));
            this.textBox2.Style.BackgroundColor = System.Drawing.Color.LightGray;
            this.textBox2.Style.Font.Bold = true;
            this.textBox2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(0D);
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox2.StyleName = "Data";
            this.textBox2.Value = "= Fields.DAY_GREEK";
            // 
            // ����������DataTextBox
            // 
            this.����������DataTextBox.CanGrow = true;
            this.����������DataTextBox.Format = "{0:d}";
            this.����������DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.3000001907348633D), Telerik.Reporting.Drawing.Unit.Cm(0.299999862909317D));
            this.����������DataTextBox.Name = "����������DataTextBox";
            this.����������DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0998001098632812D), Telerik.Reporting.Drawing.Unit.Cm(0.70000004768371582D));
            this.����������DataTextBox.Style.BackgroundColor = System.Drawing.Color.LightGray;
            this.����������DataTextBox.Style.Font.Bold = true;
            this.����������DataTextBox.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(0D);
            this.����������DataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.����������DataTextBox.StyleName = "Data";
            this.����������DataTextBox.Value = "=Fields.����������";
            // 
            // ����������CaptionTextBox
            // 
            this.����������CaptionTextBox.CanGrow = true;
            this.����������CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(4.0235576629638672D), Telerik.Reporting.Drawing.Unit.Cm(0.28954383730888367D));
            this.����������CaptionTextBox.Name = "����������CaptionTextBox";
            this.����������CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0527164936065674D), Telerik.Reporting.Drawing.Unit.Cm(0.70000004768371582D));
            this.����������CaptionTextBox.Style.BackgroundColor = System.Drawing.Color.LightGray;
            this.����������CaptionTextBox.Style.Font.Bold = true;
            this.����������CaptionTextBox.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(4D);
            this.����������CaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.����������CaptionTextBox.StyleName = "Caption";
            this.����������CaptionTextBox.Value = "����� : ";
            // 
            // labelsGroupFooterSection
            // 
            this.labelsGroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.38793334364891052D);
            this.labelsGroupFooterSection.Name = "labelsGroupFooterSection";
            this.labelsGroupFooterSection.Style.Visible = false;
            // 
            // labelsGroupHeaderSection
            // 
            this.labelsGroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.832491934299469D);
            this.labelsGroupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.������CaptionTextBox,
            this.�����������CaptionTextBox,
            this.�������CaptionTextBox,
            this.shape2});
            this.labelsGroupHeaderSection.Name = "labelsGroupHeaderSection";
            this.labelsGroupHeaderSection.PrintOnEveryPage = true;
            // 
            // ������CaptionTextBox
            // 
            this.������CaptionTextBox.CanGrow = true;
            this.������CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.������CaptionTextBox.Name = "������CaptionTextBox";
            this.������CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.1999001502990723D), Telerik.Reporting.Drawing.Unit.Cm(0.64708340167999268D));
            this.������CaptionTextBox.Style.Font.Bold = true;
            this.������CaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.������CaptionTextBox.StyleName = "Caption";
            this.������CaptionTextBox.Value = "������";
            // 
            // �����������CaptionTextBox
            // 
            this.�����������CaptionTextBox.CanGrow = true;
            this.�����������CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.20020055770874D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.�����������CaptionTextBox.Name = "�����������CaptionTextBox";
            this.�����������CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.6995997428894043D), Telerik.Reporting.Drawing.Unit.Cm(0.64708340167999268D));
            this.�����������CaptionTextBox.Style.Font.Bold = true;
            this.�����������CaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.�����������CaptionTextBox.StyleName = "Caption";
            this.�����������CaptionTextBox.Value = "�����������";
            // 
            // �������CaptionTextBox
            // 
            this.�������CaptionTextBox.CanGrow = true;
            this.�������CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.90000057220459D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.�������CaptionTextBox.Name = "�������CaptionTextBox";
            this.�������CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.9412498474121094D), Telerik.Reporting.Drawing.Unit.Cm(0.64708340167999268D));
            this.�������CaptionTextBox.Style.Font.Bold = true;
            this.�������CaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.�������CaptionTextBox.StyleName = "Caption";
            this.�������CaptionTextBox.Value = "�������";
            // 
            // shape2
            // 
            this.shape2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0.70020031929016113D));
            this.shape2.Name = "shape2";
            this.shape2.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(16.841150283813477D), Telerik.Reporting.Drawing.Unit.Cm(0.13229165971279144D));
            // 
            // sqlStations
            // 
            this.sqlStations.ConnectionString = "Abacus.Properties.Settings.DBConnectionString";
            this.sqlStations.Name = "sqlStations";
            this.sqlStations.SelectCommand = "SELECT        �������_���, ��������\r\nFROM            ���_�������\r\nORDER BY ������" +
    "��";
            // 
            // sqlDataSource
            // 
            this.sqlDataSource.ConnectionString = "Abacus.Properties.Settings.DBConnectionString";
            this.sqlDataSource.Name = "sqlDataSource";
            this.sqlDataSource.SelectCommand = "SELECT        �������_���, ��������, ����������, DAY_GREEK, ������, �����������, " +
    "�������\r\nFROM            rep�����������_������\r\nORDER BY ����������, ������, ���" +
    "��������, �������";
            // 
            // pageFooter
            // 
            this.pageFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.54718518257141113D);
            this.pageFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pictureBox3,
            this.textBox19,
            this.pageInfoTextBox});
            this.pageFooter.Name = "pageFooter";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.pictureBox3.MimeType = "image/png";
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.54708343744277954D), Telerik.Reporting.Drawing.Unit.Cm(0.54698455333709717D));
            this.pictureBox3.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.Stretch;
            this.pictureBox3.Value = ((object)(resources.GetObject("pictureBox3.Value")));
            // 
            // textBox19
            // 
            this.textBox19.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.54728347063064575D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox19.Name = "textBox19";
            this.textBox19.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(12.860754013061523D), Telerik.Reporting.Drawing.Unit.Cm(0.5470842719078064D));
            this.textBox19.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox19.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.textBox19.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.textBox19.StyleName = "PageInfo";
            this.textBox19.Value = "������������ ������� Abacus - ��������� ������� �������������� ����������� & ����" +
    "������";
            // 
            // pageInfoTextBox
            // 
            this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(13.408238410949707D), Telerik.Reporting.Drawing.Unit.Cm(0.00020105361181776971D));
            this.pageInfoTextBox.Name = "pageInfoTextBox";
            this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.5387458801269531D), Telerik.Reporting.Drawing.Unit.Cm(0.546984076499939D));
            this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.pageInfoTextBox.StyleName = "PageInfo";
            this.pageInfoTextBox.Value = "=\"���. \" + PageNumber + \"/\" + PageCount";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(0.77957451343536377D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.������DataTextBox,
            this.�����������DataTextBox,
            this.�������DataTextBox,
            this.shape3});
            this.detail.Name = "detail";
            // 
            // ������DataTextBox
            // 
            this.������DataTextBox.CanGrow = true;
            this.������DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.������DataTextBox.Name = "������DataTextBox";
            this.������DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.1999001502990723D), Telerik.Reporting.Drawing.Unit.Cm(0.64708298444747925D));
            this.������DataTextBox.Style.Font.Name = "Calibri";
            this.������DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.������DataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.������DataTextBox.StyleName = "Data";
            this.������DataTextBox.Value = "=Fields.������";
            // 
            // �����������DataTextBox
            // 
            this.�����������DataTextBox.CanGrow = true;
            this.�����������DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.2002010345458984D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.�����������DataTextBox.Name = "�����������DataTextBox";
            this.�����������DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.6995992660522461D), Telerik.Reporting.Drawing.Unit.Cm(0.64708214998245239D));
            this.�����������DataTextBox.Style.Font.Name = "Calibri";
            this.�����������DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.�����������DataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.�����������DataTextBox.StyleName = "Data";
            this.�����������DataTextBox.Value = "=Fields.�����������";
            // 
            // �������DataTextBox
            // 
            this.�������DataTextBox.CanGrow = true;
            this.�������DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.90000057220459D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.�������DataTextBox.Name = "�������DataTextBox";
            this.�������DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.9412498474121094D), Telerik.Reporting.Drawing.Unit.Cm(0.64708214998245239D));
            this.�������DataTextBox.Style.Font.Name = "Calibri";
            this.�������DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.�������DataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.�������DataTextBox.StyleName = "Data";
            this.�������DataTextBox.Value = "=Fields.�������";
            // 
            // shape3
            // 
            this.shape3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.64728283882141113D));
            this.shape3.Name = "shape3";
            this.shape3.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(16.841150283813477D), Telerik.Reporting.Drawing.Unit.Cm(0.13229165971279144D));
            // 
            // MealsPlan
            // 
            this.DataSource = this.sqlDataSource;
            this.Filters.Add(new Telerik.Reporting.Filter("=Fields.����������", Telerik.Reporting.FilterOperator.GreaterOrEqual, "=Parameters.date_start.Value"));
            this.Filters.Add(new Telerik.Reporting.Filter("=Fields.����������", Telerik.Reporting.FilterOperator.LessOrEqual, "=Parameters.date_end.Value"));
            this.Filters.Add(new Telerik.Reporting.Filter("=Fields.�������_���", Telerik.Reporting.FilterOperator.Equal, "=Parameters.stationID.Value"));
            group1.GroupFooter = this.��������GroupFooterSection;
            group1.GroupHeader = this.��������GroupHeaderSection;
            group1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.��������"));
            group1.Name = "��������Group";
            group2.GroupFooter = this.����������GroupFooterSection;
            group2.GroupHeader = this.����������GroupHeaderSection;
            group2.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.����������"));
            group2.Name = "����������Group";
            group3.GroupFooter = this.labelsGroupFooterSection;
            group3.GroupHeader = this.labelsGroupHeaderSection;
            group3.Name = "labelsGroup";
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1,
            group2,
            group3});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.��������GroupHeaderSection,
            this.��������GroupFooterSection,
            this.����������GroupHeaderSection,
            this.����������GroupFooterSection,
            this.labelsGroupHeaderSection,
            this.labelsGroupFooterSection,
            this.pageFooter,
            this.detail});
            this.Name = "MealsPlan";
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(20D), Telerik.Reporting.Drawing.Unit.Mm(20D), Telerik.Reporting.Drawing.Unit.Mm(20D), Telerik.Reporting.Drawing.Unit.Mm(20D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.AllowNull = true;
            reportParameter1.AutoRefresh = true;
            reportParameter1.Name = "date_start";
            reportParameter1.Text = "��� ��/���";
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter2.AllowNull = true;
            reportParameter2.AutoRefresh = true;
            reportParameter2.Name = "date_end";
            reportParameter2.Text = "��� ��/���";
            reportParameter2.Type = Telerik.Reporting.ReportParameterType.DateTime;
            reportParameter3.AllowNull = true;
            reportParameter3.AutoRefresh = true;
            reportParameter3.AvailableValues.DataSource = this.sqlStations;
            reportParameter3.AvailableValues.DisplayMember = "= Fields.��������";
            reportParameter3.AvailableValues.ValueMember = "= Fields.�������_���";
            reportParameter3.Name = "stationID";
            reportParameter3.Text = "�������";
            reportParameter3.Type = Telerik.Reporting.ReportParameterType.Integer;
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            this.ReportParameters.Add(reportParameter3);
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
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(16.999799728393555D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.SqlDataSource sqlDataSource;
        private Telerik.Reporting.GroupHeaderSection ��������GroupHeaderSection;
        private Telerik.Reporting.GroupFooterSection ��������GroupFooterSection;
        private Telerik.Reporting.GroupHeaderSection ����������GroupHeaderSection;
        private Telerik.Reporting.GroupFooterSection ����������GroupFooterSection;
        private Telerik.Reporting.GroupHeaderSection labelsGroupHeaderSection;
        private Telerik.Reporting.TextBox ������CaptionTextBox;
        private Telerik.Reporting.TextBox �����������CaptionTextBox;
        private Telerik.Reporting.TextBox �������CaptionTextBox;
        private Telerik.Reporting.GroupFooterSection labelsGroupFooterSection;
        private Telerik.Reporting.PageFooterSection pageFooter;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.TextBox ������DataTextBox;
        private Telerik.Reporting.TextBox �����������DataTextBox;
        private Telerik.Reporting.TextBox �������DataTextBox;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.SubReport subReport1;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox ����������DataTextBox;
        private Telerik.Reporting.TextBox ����������CaptionTextBox;
        private Telerik.Reporting.Shape shape2;
        private Telerik.Reporting.Shape shape3;
        private Telerik.Reporting.PictureBox pictureBox3;
        private Telerik.Reporting.TextBox textBox19;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private Telerik.Reporting.SqlDataSource sqlStations;

    }
}