namespace Abacus.Reports.Data.Station
{
    partial class SumCostCleaningMonth
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SumCostCleaningMonth));
            Telerik.Reporting.TypeReportSource typeReportSource1 = new Telerik.Reporting.TypeReportSource();
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
            this.�������_����GroupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.�������_����GroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.�������_����DataTextBox = new Telerik.Reporting.TextBox();
            this.�������_����CaptionTextBox = new Telerik.Reporting.TextBox();
            this.�����GroupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.tOTAL_DAYSumFunctionTextBox2 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.shape4 = new Telerik.Reporting.Shape();
            this.�����GroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.�����DataTextBox = new Telerik.Reporting.TextBox();
            this.�����CaptionTextBox = new Telerik.Reporting.TextBox();
            this.shape1 = new Telerik.Reporting.Shape();
            this.labelsGroupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.labelsGroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.����������CaptionTextBox = new Telerik.Reporting.TextBox();
            this.tOTAL_DAYCaptionTextBox = new Telerik.Reporting.TextBox();
            this.shape2 = new Telerik.Reporting.Shape();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.sqlSchoolYears = new Telerik.Reporting.SqlDataSource();
            this.sqlMonths = new Telerik.Reporting.SqlDataSource();
            this.sqlStations = new Telerik.Reporting.SqlDataSource();
            this.sqlDataSource = new Telerik.Reporting.SqlDataSource();
            this.pageFooter = new Telerik.Reporting.PageFooterSection();
            this.pictureBox3 = new Telerik.Reporting.PictureBox();
            this.textBox15 = new Telerik.Reporting.TextBox();
            this.pageInfoTextBox = new Telerik.Reporting.TextBox();
            this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
            this.subReport1 = new Telerik.Reporting.SubReport();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.detail = new Telerik.Reporting.DetailSection();
            this.����������DataTextBox = new Telerik.Reporting.TextBox();
            this.tOTAL_DAYDataTextBox = new Telerik.Reporting.TextBox();
            this.shape3 = new Telerik.Reporting.Shape();
            this.textBox3 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // �������_����GroupFooterSection
            // 
            this.�������_����GroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.800000011920929D);
            this.�������_����GroupFooterSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox7,
            this.textBox8});
            this.�������_����GroupFooterSection.Name = "�������_����GroupFooterSection";
            // 
            // textBox7
            // 
            this.textBox7.CanGrow = true;
            this.textBox7.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(13.408235549926758D), Telerik.Reporting.Drawing.Unit.Cm(0.69989955425262451D));
            this.textBox7.Style.BackgroundColor = System.Drawing.Color.LightGray;
            this.textBox7.Style.Font.Bold = true;
            this.textBox7.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(0D);
            this.textBox7.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox7.StyleName = "Caption";
            this.textBox7.Value = "=\"������ ��� �� ������� ���� : \" + Fields.�������_���� + \",  ������ ������� : \"";
            // 
            // textBox8
            // 
            this.textBox8.CanGrow = true;
            this.textBox8.Format = "{0:C2}";
            this.textBox8.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(13.408438682556152D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.5387461185455322D), Telerik.Reporting.Drawing.Unit.Cm(0.69999885559082031D));
            this.textBox8.Style.BackgroundColor = System.Drawing.Color.LightGray;
            this.textBox8.Style.Font.Bold = true;
            this.textBox8.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(4D);
            this.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox8.StyleName = "Data";
            this.textBox8.Value = "=Sum(Fields.TOTAL_DAY)";
            // 
            // �������_����GroupHeaderSection
            // 
            this.�������_����GroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.89999997615814209D);
            this.�������_����GroupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.�������_����DataTextBox,
            this.�������_����CaptionTextBox});
            this.�������_����GroupHeaderSection.Name = "�������_����GroupHeaderSection";
            // 
            // �������_����DataTextBox
            // 
            this.�������_����DataTextBox.CanGrow = true;
            this.�������_����DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.053117036819458D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.�������_����DataTextBox.Name = "�������_����DataTextBox";
            this.�������_����DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(13.946884155273438D), Telerik.Reporting.Drawing.Unit.Cm(0.747083306312561D));
            this.�������_����DataTextBox.Style.BackgroundColor = System.Drawing.Color.LightGray;
            this.�������_����DataTextBox.Style.Font.Bold = true;
            this.�������_����DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.�������_����DataTextBox.StyleName = "Data";
            this.�������_����DataTextBox.Value = "=Fields.�������_����";
            // 
            // �������_����CaptionTextBox
            // 
            this.�������_����CaptionTextBox.CanGrow = true;
            this.�������_����CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.�������_����CaptionTextBox.Name = "�������_����CaptionTextBox";
            this.�������_����CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.0529167652130127D), Telerik.Reporting.Drawing.Unit.Cm(0.747083306312561D));
            this.�������_����CaptionTextBox.Style.BackgroundColor = System.Drawing.Color.LightGray;
            this.�������_����CaptionTextBox.Style.Font.Bold = true;
            this.�������_����CaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.�������_����CaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.�������_����CaptionTextBox.StyleName = "Caption";
            this.�������_����CaptionTextBox.Value = "������� ����:";
            // 
            // �����GroupFooterSection
            // 
            this.�����GroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.89999955892562866D);
            this.�����GroupFooterSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.tOTAL_DAYSumFunctionTextBox2,
            this.textBox4,
            this.shape4});
            this.�����GroupFooterSection.Name = "�����GroupFooterSection";
            // 
            // tOTAL_DAYSumFunctionTextBox2
            // 
            this.tOTAL_DAYSumFunctionTextBox2.CanGrow = true;
            this.tOTAL_DAYSumFunctionTextBox2.Format = "{0:C2}";
            this.tOTAL_DAYSumFunctionTextBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(13.408438682556152D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.tOTAL_DAYSumFunctionTextBox2.Name = "tOTAL_DAYSumFunctionTextBox2";
            this.tOTAL_DAYSumFunctionTextBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.5387461185455322D), Telerik.Reporting.Drawing.Unit.Cm(0.69999885559082031D));
            this.tOTAL_DAYSumFunctionTextBox2.Style.Font.Bold = true;
            this.tOTAL_DAYSumFunctionTextBox2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(4D);
            this.tOTAL_DAYSumFunctionTextBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.tOTAL_DAYSumFunctionTextBox2.StyleName = "Data";
            this.tOTAL_DAYSumFunctionTextBox2.Value = "=Sum(Fields.TOTAL_DAY)";
            // 
            // textBox4
            // 
            this.textBox4.CanGrow = true;
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(13.408233642578125D), Telerik.Reporting.Drawing.Unit.Cm(0.69989955425262451D));
            this.textBox4.Style.Font.Bold = true;
            this.textBox4.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(0D);
            this.textBox4.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            this.textBox4.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox4.StyleName = "Caption";
            this.textBox4.Value = "=\"������ ��� �� ���� : \" + Fields.����� + \",  ������ ������� : \"";
            // 
            // shape4
            // 
            this.shape4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.70019948482513428D));
            this.shape4.Name = "shape4";
            this.shape4.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(16.999902725219727D), Telerik.Reporting.Drawing.Unit.Cm(0.13229165971279144D));
            // 
            // �����GroupHeaderSection
            // 
            this.�����GroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.85281693935394287D);
            this.�����GroupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.�����DataTextBox,
            this.�����CaptionTextBox,
            this.shape1});
            this.�����GroupHeaderSection.Name = "�����GroupHeaderSection";
            // 
            // �����DataTextBox
            // 
            this.�����DataTextBox.CanGrow = true;
            this.�����DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.7472834587097168D), Telerik.Reporting.Drawing.Unit.Cm(0.00020024616969749332D));
            this.�����DataTextBox.Name = "�����DataTextBox";
            this.�����DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(15.252717971801758D), Telerik.Reporting.Drawing.Unit.Cm(0.64708214998245239D));
            this.�����DataTextBox.Style.Font.Bold = true;
            this.�����DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.�����DataTextBox.StyleName = "Data";
            this.�����DataTextBox.Value = "=Fields.�����";
            // 
            // �����CaptionTextBox
            // 
            this.�����CaptionTextBox.CanGrow = true;
            this.�����CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.00020024616969749332D));
            this.�����CaptionTextBox.Name = "�����CaptionTextBox";
            this.�����CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.7470834255218506D), Telerik.Reporting.Drawing.Unit.Cm(0.64708298444747925D));
            this.�����CaptionTextBox.Style.Font.Bold = true;
            this.�����CaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.�����CaptionTextBox.StyleName = "Caption";
            this.�����CaptionTextBox.Value = "�����:";
            // 
            // shape1
            // 
            this.shape1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.64748388528823853D));
            this.shape1.Name = "shape1";
            this.shape1.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(16.999902725219727D), Telerik.Reporting.Drawing.Unit.Cm(0.19999989867210388D));
            // 
            // labelsGroupFooterSection
            // 
            this.labelsGroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.32002407312393188D);
            this.labelsGroupFooterSection.Name = "labelsGroupFooterSection";
            this.labelsGroupFooterSection.Style.Visible = true;
            // 
            // labelsGroupHeaderSection
            // 
            this.labelsGroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.73249161243438721D);
            this.labelsGroupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.����������CaptionTextBox,
            this.tOTAL_DAYCaptionTextBox,
            this.shape2,
            this.textBox2});
            this.labelsGroupHeaderSection.Name = "labelsGroupHeaderSection";
            this.labelsGroupHeaderSection.PrintOnEveryPage = true;
            this.labelsGroupHeaderSection.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(4D);
            this.labelsGroupHeaderSection.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            // 
            // ����������CaptionTextBox
            // 
            this.����������CaptionTextBox.CanGrow = true;
            this.����������CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.7472834587097168D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.����������CaptionTextBox.Name = "����������CaptionTextBox";
            this.����������CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.6733417510986328D), Telerik.Reporting.Drawing.Unit.Cm(0.54708343744277954D));
            this.����������CaptionTextBox.Style.Font.Bold = true;
            this.����������CaptionTextBox.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(0D);
            this.����������CaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.����������CaptionTextBox.StyleName = "Caption";
            this.����������CaptionTextBox.Value = "����������";
            // 
            // tOTAL_DAYCaptionTextBox
            // 
            this.tOTAL_DAYCaptionTextBox.CanGrow = true;
            this.tOTAL_DAYCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.4208250045776367D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.tOTAL_DAYCaptionTextBox.Name = "tOTAL_DAYCaptionTextBox";
            this.tOTAL_DAYCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.57907485961914D), Telerik.Reporting.Drawing.Unit.Cm(0.54708343744277954D));
            this.tOTAL_DAYCaptionTextBox.Style.Font.Bold = true;
            this.tOTAL_DAYCaptionTextBox.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(0D);
            this.tOTAL_DAYCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.tOTAL_DAYCaptionTextBox.StyleName = "Caption";
            this.tOTAL_DAYCaptionTextBox.Value = "������ ������";
            // 
            // shape2
            // 
            this.shape2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0.60019993782043457D));
            this.shape2.Name = "shape2";
            this.shape2.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(16.999902725219727D), Telerik.Reporting.Drawing.Unit.Cm(0.13229165971279144D));
            // 
            // textBox2
            // 
            this.textBox2.CanGrow = true;
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.7470836639404297D), Telerik.Reporting.Drawing.Unit.Cm(0.54708343744277954D));
            this.textBox2.Style.Font.Bold = true;
            this.textBox2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(0D);
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox2.StyleName = "Caption";
            this.textBox2.Value = "�/�";
            // 
            // sqlSchoolYears
            // 
            this.sqlSchoolYears.ConnectionString = "Abacus.Properties.Settings.DBConnectionString";
            this.sqlSchoolYears.Name = "sqlSchoolYears";
            this.sqlSchoolYears.SelectCommand = "SELECT        SCHOOLYEAR_ID, �������_����\r\nFROM            ���_�������_���";
            // 
            // sqlMonths
            // 
            this.sqlMonths.ConnectionString = "Abacus.Properties.Settings.DBConnectionString";
            this.sqlMonths.Name = "sqlMonths";
            this.sqlMonths.SelectCommand = "SELECT        �����_���, �����\r\nFROM            ���_�����";
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
            this.sqlDataSource.SelectCommand = "SELECT        ���, ��������, SCHOOLYEAR_ID, �������_����, ����������, �����_�����" +
    "��, �����, TOTAL_DAY\r\nFROM            rep������_�����������_�����\r\nORDER BY ����" +
    "����, ����������, �����_�������";
            // 
            // pageFooter
            // 
            this.pageFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.54728531837463379D);
            this.pageFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pictureBox3,
            this.textBox15,
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
            // textBox15
            // 
            this.textBox15.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.6002001166343689D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(12.808036804199219D), Telerik.Reporting.Drawing.Unit.Cm(0.5470842719078064D));
            this.textBox15.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox15.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.textBox15.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.textBox15.StyleName = "PageInfo";
            this.textBox15.Value = "������������ ������� Abacus - ��������� ������� �������������� ����������� & ����" +
    "������";
            // 
            // pageInfoTextBox
            // 
            this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(13.408437728881836D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.pageInfoTextBox.Name = "pageInfoTextBox";
            this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.5387458801269531D), Telerik.Reporting.Drawing.Unit.Cm(0.546984076499939D));
            this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.pageInfoTextBox.StyleName = "PageInfo";
            this.pageInfoTextBox.Value = "=\"���. \" + PageNumber + \"/\" + PageCount";
            // 
            // reportHeader
            // 
            this.reportHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(2.8000001907348633D);
            this.reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.subReport1,
            this.textBox1});
            this.reportHeader.Name = "reportHeader";
            // 
            // subReport1
            // 
            this.subReport1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(9.9921220680698752E-05D));
            this.subReport1.Name = "subReport1";
            typeReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("stationID", "=Fields.���"));
            typeReportSource1.TypeName = "Abacus.Reports.Data.LogoStationShort, Abacus, Version=1.0.0.0, Culture=neutral, P" +
    "ublicKeyToken=null";
            this.subReport1.ReportSource = typeReportSource1;
            this.subReport1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8000001907348633D), Telerik.Reporting.Drawing.Unit.Cm(1.8999001979827881D));
            // 
            // textBox1
            // 
            this.textBox1.CanGrow = true;
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(1.9002001285552979D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(16.999900817871094D), Telerik.Reporting.Drawing.Unit.Cm(0.89980012178421021D));
            this.textBox1.Style.Font.Bold = true;
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox1.StyleName = "Caption";
            this.textBox1.Value = "������������� ��������� ������� ������������ ��� �.�.�. ";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(0.80019986629486084D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.����������DataTextBox,
            this.tOTAL_DAYDataTextBox,
            this.shape3,
            this.textBox3});
            this.detail.Name = "detail";
            // 
            // ����������DataTextBox
            // 
            this.����������DataTextBox.CanGrow = true;
            this.����������DataTextBox.Format = "{0:d}";
            this.����������DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.7472834587097168D), Telerik.Reporting.Drawing.Unit.Cm(0.00020024616969749332D));
            this.����������DataTextBox.Name = "����������DataTextBox";
            this.����������DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.6733417510986328D), Telerik.Reporting.Drawing.Unit.Cm(0.667508065700531D));
            this.����������DataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.����������DataTextBox.StyleName = "Data";
            this.����������DataTextBox.Value = "=Fields.����������";
            // 
            // tOTAL_DAYDataTextBox
            // 
            this.tOTAL_DAYDataTextBox.CanGrow = true;
            this.tOTAL_DAYDataTextBox.Format = "{0:C2}";
            this.tOTAL_DAYDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.4208250045776367D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.tOTAL_DAYDataTextBox.Name = "tOTAL_DAYDataTextBox";
            this.tOTAL_DAYDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.5263586044311523D), Telerik.Reporting.Drawing.Unit.Cm(0.667508065700531D));
            this.tOTAL_DAYDataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.tOTAL_DAYDataTextBox.StyleName = "Data";
            this.tOTAL_DAYDataTextBox.Value = "=Fields.TOTAL_DAY";
            // 
            // shape3
            // 
            this.shape3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.6679081916809082D));
            this.shape3.Name = "shape3";
            this.shape3.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(16.999902725219727D), Telerik.Reporting.Drawing.Unit.Cm(0.13229165971279144D));
            // 
            // textBox3
            // 
            this.textBox3.CanGrow = true;
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.00020024616969749332D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.7470836639404297D), Telerik.Reporting.Drawing.Unit.Cm(0.667508065700531D));
            this.textBox3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox3.StyleName = "Data";
            this.textBox3.Value = "=RowNumber() + \".\"";
            // 
            // SumCostCleaningMonth
            // 
            this.DataSource = this.sqlDataSource;
            this.Filters.Add(new Telerik.Reporting.Filter("=Fields.SCHOOLYEAR_ID", Telerik.Reporting.FilterOperator.Equal, "=Parameters.schoolyearID.Value"));
            this.Filters.Add(new Telerik.Reporting.Filter("=Fields.�����_�������", Telerik.Reporting.FilterOperator.In, "=Parameters.monthID.Value"));
            this.Filters.Add(new Telerik.Reporting.Filter("=Fields.���", Telerik.Reporting.FilterOperator.In, "=Parameters.stationID.Value"));
            group1.GroupFooter = this.�������_����GroupFooterSection;
            group1.GroupHeader = this.�������_����GroupHeaderSection;
            group1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.�������_����"));
            group1.Name = "�������_����Group";
            group2.GroupFooter = this.�����GroupFooterSection;
            group2.GroupHeader = this.�����GroupHeaderSection;
            group2.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.�����"));
            group2.Name = "�����Group";
            group3.GroupFooter = this.labelsGroupFooterSection;
            group3.GroupHeader = this.labelsGroupHeaderSection;
            group3.Name = "labelsGroup";
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1,
            group2,
            group3});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.�������_����GroupHeaderSection,
            this.�������_����GroupFooterSection,
            this.�����GroupHeaderSection,
            this.�����GroupFooterSection,
            this.labelsGroupHeaderSection,
            this.labelsGroupFooterSection,
            this.pageFooter,
            this.reportHeader,
            this.detail});
            this.Name = "SumCostCleaningMonth";
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(20D), Telerik.Reporting.Drawing.Unit.Mm(20D), Telerik.Reporting.Drawing.Unit.Mm(20D), Telerik.Reporting.Drawing.Unit.Mm(20D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.AllowNull = true;
            reportParameter1.AutoRefresh = true;
            reportParameter1.AvailableValues.DataSource = this.sqlSchoolYears;
            reportParameter1.AvailableValues.DisplayMember = "= Fields.�������_����";
            reportParameter1.AvailableValues.ValueMember = "= Fields.SCHOOLYEAR_ID";
            reportParameter1.Name = "schoolyearID";
            reportParameter1.Text = "������� ����";
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter1.Visible = true;
            reportParameter2.AllowNull = true;
            reportParameter2.AutoRefresh = true;
            reportParameter2.AvailableValues.DataSource = this.sqlMonths;
            reportParameter2.AvailableValues.DisplayMember = "= Fields.�����";
            reportParameter2.AvailableValues.ValueMember = "= Fields.�����_���";
            reportParameter2.MultiValue = true;
            reportParameter2.Name = "monthID";
            reportParameter2.Text = "�����";
            reportParameter2.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter2.Visible = true;
            reportParameter3.AllowNull = true;
            reportParameter3.AutoRefresh = true;
            reportParameter3.AvailableValues.DataSource = this.sqlStations;
            reportParameter3.AvailableValues.DisplayMember = "= Fields.��������";
            reportParameter3.AvailableValues.ValueMember = "= Fields.�������_���";
            reportParameter3.MultiValue = true;
            reportParameter3.Name = "stationID";
            reportParameter3.Text = "���";
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
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(16.999900817871094D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.SqlDataSource sqlDataSource;
        private Telerik.Reporting.GroupHeaderSection �������_����GroupHeaderSection;
        private Telerik.Reporting.GroupFooterSection �������_����GroupFooterSection;
        private Telerik.Reporting.GroupHeaderSection �����GroupHeaderSection;
        private Telerik.Reporting.GroupFooterSection �����GroupFooterSection;
        private Telerik.Reporting.TextBox tOTAL_DAYSumFunctionTextBox2;
        private Telerik.Reporting.GroupHeaderSection labelsGroupHeaderSection;
        private Telerik.Reporting.TextBox ����������CaptionTextBox;
        private Telerik.Reporting.TextBox tOTAL_DAYCaptionTextBox;
        private Telerik.Reporting.GroupFooterSection labelsGroupFooterSection;
        private Telerik.Reporting.PageFooterSection pageFooter;
        private Telerik.Reporting.ReportHeaderSection reportHeader;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.TextBox ����������DataTextBox;
        private Telerik.Reporting.TextBox tOTAL_DAYDataTextBox;
        private Telerik.Reporting.SubReport subReport1;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox �������_����DataTextBox;
        private Telerik.Reporting.TextBox �������_����CaptionTextBox;
        private Telerik.Reporting.TextBox �����DataTextBox;
        private Telerik.Reporting.TextBox �����CaptionTextBox;
        private Telerik.Reporting.Shape shape1;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.Shape shape2;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.Shape shape3;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.Shape shape4;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.PictureBox pictureBox3;
        private Telerik.Reporting.TextBox textBox15;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private Telerik.Reporting.SqlDataSource sqlStations;
        private Telerik.Reporting.SqlDataSource sqlMonths;
        private Telerik.Reporting.SqlDataSource sqlSchoolYears;

    }
}