namespace Abacus.Reports.Data.Station
{
    partial class CostFeedingMonthly
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CostFeedingMonthly));
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
            this.textBox5 = new Telerik.Reporting.TextBox();
            this.textBox6 = new Telerik.Reporting.TextBox();
            this.�������_����GroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.�������_����CaptionTextBox = new Telerik.Reporting.TextBox();
            this.�������_����DataTextBox = new Telerik.Reporting.TextBox();
            this.�����GroupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.������SumFunctionTextBox2 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.shape4 = new Telerik.Reporting.Shape();
            this.�����GroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.�����CaptionTextBox = new Telerik.Reporting.TextBox();
            this.�����DataTextBox = new Telerik.Reporting.TextBox();
            this.shape1 = new Telerik.Reporting.Shape();
            this.labelsGroupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.labelsGroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.����������CaptionTextBox = new Telerik.Reporting.TextBox();
            this.���������CaptionTextBox = new Telerik.Reporting.TextBox();
            this.��������CaptionTextBox = new Telerik.Reporting.TextBox();
            this.����_������CaptionTextBox = new Telerik.Reporting.TextBox();
            this.������CaptionTextBox = new Telerik.Reporting.TextBox();
            this.������_������CaptionTextBox = new Telerik.Reporting.TextBox();
            this.shape2 = new Telerik.Reporting.Shape();
            this.sqlSchoolYears = new Telerik.Reporting.SqlDataSource();
            this.sqlMonths = new Telerik.Reporting.SqlDataSource();
            this.sqlStations = new Telerik.Reporting.SqlDataSource();
            this.sqlDataSource = new Telerik.Reporting.SqlDataSource();
            this.pageFooter = new Telerik.Reporting.PageFooterSection();
            this.pageInfoTextBox = new Telerik.Reporting.TextBox();
            this.textBox15 = new Telerik.Reporting.TextBox();
            this.pictureBox3 = new Telerik.Reporting.PictureBox();
            this.textBox16 = new Telerik.Reporting.TextBox();
            this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
            this.subReport1 = new Telerik.Reporting.SubReport();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.detail = new Telerik.Reporting.DetailSection();
            this.����������DataTextBox = new Telerik.Reporting.TextBox();
            this.���������DataTextBox = new Telerik.Reporting.TextBox();
            this.������_������DataTextBox = new Telerik.Reporting.TextBox();
            this.����_������DataTextBox = new Telerik.Reporting.TextBox();
            this.������DataTextBox = new Telerik.Reporting.TextBox();
            this.��������DataTextBox = new Telerik.Reporting.TextBox();
            this.shape3 = new Telerik.Reporting.Shape();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // �������_����GroupFooterSection
            // 
            this.�������_����GroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(1.0528163909912109D);
            this.�������_����GroupFooterSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox5,
            this.textBox6});
            this.�������_����GroupFooterSection.Name = "�������_����GroupFooterSection";
            // 
            // textBox5
            // 
            this.textBox5.CanGrow = true;
            this.textBox5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.25281643867492676D));
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(22.446882247924805D), Telerik.Reporting.Drawing.Unit.Cm(0.64688432216644287D));
            this.textBox5.Style.BackgroundColor = System.Drawing.Color.LightGray;
            this.textBox5.Style.Font.Bold = true;
            this.textBox5.Style.Font.Name = "Calibri";
            this.textBox5.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.textBox5.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(4D);
            this.textBox5.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox5.StyleName = "Caption";
            this.textBox5.Value = "=\"������ ��� �� ������� ���� : \" + Fields.�������_���� + \" - ������ ��������� : \"" +
    " + CStr(Count(Fields.������_������)) + \", ������ ������� = \"";
            // 
            // textBox6
            // 
            this.textBox6.CanGrow = true;
            this.textBox6.Format = "{0:C2}";
            this.textBox6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(22.5D), Telerik.Reporting.Drawing.Unit.Cm(0.25281643867492676D));
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.9883301258087158D), Telerik.Reporting.Drawing.Unit.Cm(0.64708298444747925D));
            this.textBox6.Style.BackgroundColor = System.Drawing.Color.LightGray;
            this.textBox6.Style.Font.Bold = true;
            this.textBox6.Style.Font.Name = "Calibri";
            this.textBox6.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.textBox6.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            this.textBox6.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox6.StyleName = "Data";
            this.textBox6.Value = "=Sum(Fields.������)";
            // 
            // �������_����GroupHeaderSection
            // 
            this.�������_����GroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.89999997615814209D);
            this.�������_����GroupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.�������_����CaptionTextBox,
            this.�������_����DataTextBox});
            this.�������_����GroupHeaderSection.Name = "�������_����GroupHeaderSection";
            this.�������_����GroupHeaderSection.PrintOnEveryPage = true;
            // 
            // �������_����CaptionTextBox
            // 
            this.�������_����CaptionTextBox.CanGrow = true;
            this.�������_����CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.�������_����CaptionTextBox.Name = "�������_����CaptionTextBox";
            this.�������_����CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3D), Telerik.Reporting.Drawing.Unit.Cm(0.747083306312561D));
            this.�������_����CaptionTextBox.Style.BackgroundColor = System.Drawing.Color.LightGray;
            this.�������_����CaptionTextBox.Style.Font.Bold = true;
            this.�������_����CaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.�������_����CaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.�������_����CaptionTextBox.StyleName = "Caption";
            this.�������_����CaptionTextBox.Value = "������� ����:";
            // 
            // �������_����DataTextBox
            // 
            this.�������_����DataTextBox.CanGrow = true;
            this.�������_����DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.053117036819458D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.�������_����DataTextBox.Name = "�������_����DataTextBox";
            this.�������_����DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(22.435214996337891D), Telerik.Reporting.Drawing.Unit.Cm(0.747083306312561D));
            this.�������_����DataTextBox.Style.BackgroundColor = System.Drawing.Color.LightGray;
            this.�������_����DataTextBox.Style.Font.Bold = true;
            this.�������_����DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.�������_����DataTextBox.StyleName = "Data";
            this.�������_����DataTextBox.Value = "=Fields.�������_����";
            // 
            // �����GroupFooterSection
            // 
            this.�����GroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.98311585187911987D);
            this.�����GroupFooterSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.������SumFunctionTextBox2,
            this.textBox2,
            this.shape4});
            this.�����GroupFooterSection.Name = "�����GroupFooterSection";
            // 
            // ������SumFunctionTextBox2
            // 
            this.������SumFunctionTextBox2.CanGrow = true;
            this.������SumFunctionTextBox2.Format = "{0:C2}";
            this.������SumFunctionTextBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(22.5D), Telerik.Reporting.Drawing.Unit.Cm(0.19999989867210388D));
            this.������SumFunctionTextBox2.Name = "������SumFunctionTextBox2";
            this.������SumFunctionTextBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.9883301258087158D), Telerik.Reporting.Drawing.Unit.Cm(0.64708298444747925D));
            this.������SumFunctionTextBox2.Style.Font.Bold = true;
            this.������SumFunctionTextBox2.Style.Font.Name = "Calibri";
            this.������SumFunctionTextBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.������SumFunctionTextBox2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            this.������SumFunctionTextBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.������SumFunctionTextBox2.StyleName = "Data";
            this.������SumFunctionTextBox2.Value = "=Sum(Fields.������)";
            // 
            // textBox2
            // 
            this.textBox2.CanGrow = true;
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.19999989867210388D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(22.446884155273438D), Telerik.Reporting.Drawing.Unit.Cm(0.64688432216644287D));
            this.textBox2.Style.Font.Bold = true;
            this.textBox2.Style.Font.Name = "Calibri";
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.textBox2.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(4D);
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox2.StyleName = "Caption";
            this.textBox2.Value = "=\"������ ��� �� ���� : \" + Fields.�����_���� + \" - ������ ��������� : \" + CStr(Co" +
    "unt(Fields.������_������)) + \", ������ ������� = \"";
            // 
            // shape4
            // 
            this.shape4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.84728270769119263D));
            this.shape4.Name = "shape4";
            this.shape4.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(25.435415267944336D), Telerik.Reporting.Drawing.Unit.Cm(0.13583311438560486D));
            // 
            // �����GroupHeaderSection
            // 
            this.�����GroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.90019941329956055D);
            this.�����GroupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.�����CaptionTextBox,
            this.�����DataTextBox,
            this.shape1});
            this.�����GroupHeaderSection.Name = "�����GroupHeaderSection";
            this.�����GroupHeaderSection.PrintOnEveryPage = true;
            // 
            // �����CaptionTextBox
            // 
            this.�����CaptionTextBox.CanGrow = true;
            this.�����CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.�����CaptionTextBox.Name = "�����CaptionTextBox";
            this.�����CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.7470834255218506D), Telerik.Reporting.Drawing.Unit.Cm(0.64708298444747925D));
            this.�����CaptionTextBox.Style.Font.Bold = true;
            this.�����CaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.�����CaptionTextBox.StyleName = "Caption";
            this.�����CaptionTextBox.Value = "�����:";
            // 
            // �����DataTextBox
            // 
            this.�����DataTextBox.CanGrow = true;
            this.�����DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.8001999855041504D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.�����DataTextBox.Name = "�����DataTextBox";
            this.�����DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(23.688135147094727D), Telerik.Reporting.Drawing.Unit.Cm(0.64708214998245239D));
            this.�����DataTextBox.Style.Font.Bold = true;
            this.�����DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.�����DataTextBox.StyleName = "Data";
            this.�����DataTextBox.Value = "=Fields.�����";
            // 
            // shape1
            // 
            this.shape1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.70019948482513428D));
            this.shape1.Name = "shape1";
            this.shape1.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(25.435415267944336D), Telerik.Reporting.Drawing.Unit.Cm(0.19999989867210388D));
            // 
            // labelsGroupFooterSection
            // 
            this.labelsGroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.26750868558883667D);
            this.labelsGroupFooterSection.Name = "labelsGroupFooterSection";
            this.labelsGroupFooterSection.Style.Visible = true;
            // 
            // labelsGroupHeaderSection
            // 
            this.labelsGroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.77937668561935425D);
            this.labelsGroupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.����������CaptionTextBox,
            this.���������CaptionTextBox,
            this.��������CaptionTextBox,
            this.����_������CaptionTextBox,
            this.������CaptionTextBox,
            this.������_������CaptionTextBox,
            this.shape2});
            this.labelsGroupHeaderSection.Name = "labelsGroupHeaderSection";
            this.labelsGroupHeaderSection.PrintOnEveryPage = true;
            // 
            // ����������CaptionTextBox
            // 
            this.����������CaptionTextBox.CanGrow = true;
            this.����������CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.����������CaptionTextBox.Name = "����������CaptionTextBox";
            this.����������CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.5470831394195557D), Telerik.Reporting.Drawing.Unit.Cm(0.64688432216644287D));
            this.����������CaptionTextBox.Style.Font.Bold = true;
            this.����������CaptionTextBox.Style.Font.Name = "Calibri";
            this.����������CaptionTextBox.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(4D);
            this.����������CaptionTextBox.StyleName = "Caption";
            this.����������CaptionTextBox.Value = "����������";
            // 
            // ���������CaptionTextBox
            // 
            this.���������CaptionTextBox.CanGrow = true;
            this.���������CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.6002001762390137D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.���������CaptionTextBox.Name = "���������CaptionTextBox";
            this.���������CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.9998002052307129D), Telerik.Reporting.Drawing.Unit.Cm(0.64688515663146973D));
            this.���������CaptionTextBox.Style.Font.Bold = true;
            this.���������CaptionTextBox.Style.Font.Name = "Calibri";
            this.���������CaptionTextBox.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(4D);
            this.���������CaptionTextBox.StyleName = "Caption";
            this.���������CaptionTextBox.Value = "���������";
            // 
            // ��������CaptionTextBox
            // 
            this.��������CaptionTextBox.CanGrow = true;
            this.��������CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(19D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.��������CaptionTextBox.Name = "��������CaptionTextBox";
            this.��������CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.8997999429702759D), Telerik.Reporting.Drawing.Unit.Cm(0.64688515663146973D));
            this.��������CaptionTextBox.Style.Font.Bold = true;
            this.��������CaptionTextBox.Style.Font.Name = "Calibri";
            this.��������CaptionTextBox.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(0D);
            this.��������CaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.��������CaptionTextBox.StyleName = "Caption";
            this.��������CaptionTextBox.Value = "��������";
            // 
            // ����_������CaptionTextBox
            // 
            this.����_������CaptionTextBox.CanGrow = true;
            this.����_������CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(20.899999618530273D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.����_������CaptionTextBox.Name = "����_������CaptionTextBox";
            this.����_������CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.5998008251190186D), Telerik.Reporting.Drawing.Unit.Cm(0.64688515663146973D));
            this.����_������CaptionTextBox.Style.Font.Bold = true;
            this.����_������CaptionTextBox.Style.Font.Name = "Calibri";
            this.����_������CaptionTextBox.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(0D);
            this.����_������CaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.����_������CaptionTextBox.StyleName = "Caption";
            this.����_������CaptionTextBox.Value = "����/������";
            // 
            // ������CaptionTextBox
            // 
            this.������CaptionTextBox.CanGrow = true;
            this.������CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(23.5D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.������CaptionTextBox.Name = "������CaptionTextBox";
            this.������CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.0412497520446777D), Telerik.Reporting.Drawing.Unit.Cm(0.64688515663146973D));
            this.������CaptionTextBox.Style.Font.Bold = true;
            this.������CaptionTextBox.Style.Font.Name = "Calibri";
            this.������CaptionTextBox.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(0D);
            this.������CaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.������CaptionTextBox.StyleName = "Caption";
            this.������CaptionTextBox.Value = "������";
            // 
            // ������_������CaptionTextBox
            // 
            this.������_������CaptionTextBox.CanGrow = true;
            this.������_������CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.60020112991333D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.������_������CaptionTextBox.Name = "������_������CaptionTextBox";
            this.������_������CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(11.399598121643066D), Telerik.Reporting.Drawing.Unit.Cm(0.64688515663146973D));
            this.������_������CaptionTextBox.Style.Font.Bold = true;
            this.������_������CaptionTextBox.Style.Font.Name = "Calibri";
            this.������_������CaptionTextBox.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(4D);
            this.������_������CaptionTextBox.StyleName = "Caption";
            this.������_������CaptionTextBox.Value = "������ & ������� ��������";
            // 
            // shape2
            // 
            this.shape2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.64708501100540161D));
            this.shape2.Name = "shape2";
            this.shape2.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(25.435415267944336D), Telerik.Reporting.Drawing.Unit.Cm(0.13229165971279144D));
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
            this.sqlDataSource.SelectCommand = resources.GetString("sqlDataSource.SelectCommand");
            // 
            // pageFooter
            // 
            this.pageFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.87239623069763184D);
            this.pageFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageInfoTextBox,
            this.textBox15,
            this.pictureBox3,
            this.textBox16});
            this.pageFooter.Name = "pageFooter";
            // 
            // pageInfoTextBox
            // 
            this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(19.400001525878906D), Telerik.Reporting.Drawing.Unit.Cm(0.20000070333480835D));
            this.pageInfoTextBox.Name = "pageInfoTextBox";
            this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.0883293151855469D), Telerik.Reporting.Drawing.Unit.Cm(0.67239552736282349D));
            this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.pageInfoTextBox.StyleName = "PageInfo";
            this.pageInfoTextBox.Value = "=\"���. \" + PageNumber + \"/\" + PageCount";
            // 
            // textBox15
            // 
            this.textBox15.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.66332471370697021D), Telerik.Reporting.Drawing.Unit.Cm(0.20000070333480835D));
            this.textBox15.Name = "textBox15";
            this.textBox15.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.7366752624511719D), Telerik.Reporting.Drawing.Unit.Cm(0.67239558696746826D));
            this.textBox15.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox15.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.textBox15.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.textBox15.StyleName = "PageInfo";
            this.textBox15.Value = "������������ ������� Abacus";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.20000070333480835D));
            this.pictureBox3.MimeType = "image/png";
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(0.66312485933303833D), Telerik.Reporting.Drawing.Unit.Cm(0.672295868396759D));
            this.pictureBox3.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.Stretch;
            this.pictureBox3.Value = ((object)(resources.GetObject("pictureBox3.Value")));
            // 
            // textBox16
            // 
            this.textBox16.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.4001998901367188D), Telerik.Reporting.Drawing.Unit.Cm(0.20000070333480835D));
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(13.999600410461426D), Telerik.Reporting.Drawing.Unit.Cm(0.67239558696746826D));
            this.textBox16.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox16.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.textBox16.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(0D);
            this.textBox16.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox16.StyleName = "PageInfo";
            this.textBox16.Value = "�/��� ������� �������������� ����������� & ����������";
            // 
            // reportHeader
            // 
            this.reportHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(3D);
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
            this.subReport1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.8000001907348633D), Telerik.Reporting.Drawing.Unit.Cm(1.9999003410339356D));
            // 
            // textBox1
            // 
            this.textBox1.CanGrow = true;
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916642278432846D), Telerik.Reporting.Drawing.Unit.Cm(1.9999997615814209D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(25.435415267944336D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.textBox1.Style.Font.Bold = true;
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox1.StyleName = "Caption";
            this.textBox1.Value = "��������� ��������� ������� �������� ������������� �������";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(0.65291553735733032D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.����������DataTextBox,
            this.���������DataTextBox,
            this.������_������DataTextBox,
            this.����_������DataTextBox,
            this.������DataTextBox,
            this.��������DataTextBox,
            this.shape3});
            this.detail.Name = "detail";
            // 
            // ����������DataTextBox
            // 
            this.����������DataTextBox.CanGrow = true;
            this.����������DataTextBox.Format = "{0:d}";
            this.����������DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.00020024616969749332D));
            this.����������DataTextBox.Name = "����������DataTextBox";
            this.����������DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.5470831394195557D), Telerik.Reporting.Drawing.Unit.Cm(0.52022302150726318D));
            this.����������DataTextBox.Style.Font.Name = "Calibri";
            this.����������DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.����������DataTextBox.StyleName = "Data";
            this.����������DataTextBox.Value = "=Fields.����������";
            // 
            // ���������DataTextBox
            // 
            this.���������DataTextBox.CanGrow = true;
            this.���������DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.6002001762390137D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.���������DataTextBox.Name = "���������DataTextBox";
            this.���������DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.9998011589050293D), Telerik.Reporting.Drawing.Unit.Cm(0.52042323350906372D));
            this.���������DataTextBox.Style.Font.Name = "Calibri";
            this.���������DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.���������DataTextBox.StyleName = "Data";
            this.���������DataTextBox.Value = "=Fields.���������";
            // 
            // ������_������DataTextBox
            // 
            this.������_������DataTextBox.CanGrow = true;
            this.������_������DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.60020112991333D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.������_������DataTextBox.Name = "������_������DataTextBox";
            this.������_������DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(11.399598121643066D), Telerik.Reporting.Drawing.Unit.Cm(0.52042323350906372D));
            this.������_������DataTextBox.Style.Font.Name = "Calibri";
            this.������_������DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.������_������DataTextBox.StyleName = "Data";
            this.������_������DataTextBox.Value = "=Fields.������_������";
            // 
            // ����_������DataTextBox
            // 
            this.����_������DataTextBox.CanGrow = true;
            this.����_������DataTextBox.Format = "{0:C2}";
            this.����_������DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(20.899999618530273D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.����_������DataTextBox.Name = "����_������DataTextBox";
            this.����_������DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.5998008251190186D), Telerik.Reporting.Drawing.Unit.Cm(0.52042323350906372D));
            this.����_������DataTextBox.Style.Font.Name = "Calibri";
            this.����_������DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.����_������DataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.����_������DataTextBox.StyleName = "Data";
            this.����_������DataTextBox.Value = "=Fields.����_������";
            // 
            // ������DataTextBox
            // 
            this.������DataTextBox.CanGrow = true;
            this.������DataTextBox.Format = "{0:C2}";
            this.������DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(23.5D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.������DataTextBox.Name = "������DataTextBox";
            this.������DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.9883298873901367D), Telerik.Reporting.Drawing.Unit.Cm(0.52042323350906372D));
            this.������DataTextBox.Style.Font.Name = "Calibri";
            this.������DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.������DataTextBox.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            this.������DataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.������DataTextBox.StyleName = "Data";
            this.������DataTextBox.Value = "=Fields.������";
            // 
            // ��������DataTextBox
            // 
            this.��������DataTextBox.CanGrow = true;
            this.��������DataTextBox.Format = "{0:N2}";
            this.��������DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(19D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.��������DataTextBox.Name = "��������DataTextBox";
            this.��������DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.8997999429702759D), Telerik.Reporting.Drawing.Unit.Cm(0.52042323350906372D));
            this.��������DataTextBox.Style.Font.Name = "Calibri";
            this.��������DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.��������DataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.��������DataTextBox.StyleName = "Data";
            this.��������DataTextBox.Value = "=Fields.��������";
            // 
            // shape3
            // 
            this.shape3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.52062386274337769D));
            this.shape3.Name = "shape3";
            this.shape3.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(25.435413360595703D), Telerik.Reporting.Drawing.Unit.Cm(0.13229165971279144D));
            // 
            // CostFeedingMonthly
            // 
            this.DataSource = this.sqlDataSource;
            this.Filters.Add(new Telerik.Reporting.Filter("=Fields.����_����", Telerik.Reporting.FilterOperator.Equal, "=Parameters.schoolyearID.Value"));
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
            group2.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.�����_�������", Telerik.Reporting.SortDirection.Asc));
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
            this.Name = "CostFeedingMonthly";
            this.PageSettings.Landscape = true;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(20D), Telerik.Reporting.Drawing.Unit.Mm(20D), Telerik.Reporting.Drawing.Unit.Mm(15D), Telerik.Reporting.Drawing.Unit.Mm(15D));
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
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(25.541252136230469D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.SqlDataSource sqlDataSource;
        private Telerik.Reporting.GroupHeaderSection �������_����GroupHeaderSection;
        private Telerik.Reporting.TextBox �������_����CaptionTextBox;
        private Telerik.Reporting.TextBox �������_����DataTextBox;
        private Telerik.Reporting.GroupFooterSection �������_����GroupFooterSection;
        private Telerik.Reporting.GroupHeaderSection �����GroupHeaderSection;
        private Telerik.Reporting.TextBox �����CaptionTextBox;
        private Telerik.Reporting.TextBox �����DataTextBox;
        private Telerik.Reporting.GroupFooterSection �����GroupFooterSection;
        private Telerik.Reporting.TextBox ������SumFunctionTextBox2;
        private Telerik.Reporting.GroupHeaderSection labelsGroupHeaderSection;
        private Telerik.Reporting.TextBox ����������CaptionTextBox;
        private Telerik.Reporting.TextBox ���������CaptionTextBox;
        private Telerik.Reporting.TextBox ������_������CaptionTextBox;
        private Telerik.Reporting.TextBox ��������CaptionTextBox;
        private Telerik.Reporting.TextBox ����_������CaptionTextBox;
        private Telerik.Reporting.TextBox ������CaptionTextBox;
        private Telerik.Reporting.GroupFooterSection labelsGroupFooterSection;
        private Telerik.Reporting.PageFooterSection pageFooter;
        private Telerik.Reporting.ReportHeaderSection reportHeader;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.TextBox ����������DataTextBox;
        private Telerik.Reporting.TextBox ���������DataTextBox;
        private Telerik.Reporting.TextBox ������_������DataTextBox;
        private Telerik.Reporting.TextBox ��������DataTextBox;
        private Telerik.Reporting.TextBox ����_������DataTextBox;
        private Telerik.Reporting.TextBox ������DataTextBox;
        private Telerik.Reporting.SubReport subReport1;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.Shape shape1;
        private Telerik.Reporting.Shape shape2;
        private Telerik.Reporting.Shape shape3;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox5;
        private Telerik.Reporting.TextBox textBox6;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private Telerik.Reporting.TextBox textBox16;
        private Telerik.Reporting.TextBox textBox15;
        private Telerik.Reporting.PictureBox pictureBox3;
        private Telerik.Reporting.SqlDataSource sqlStations;
        private Telerik.Reporting.SqlDataSource sqlMonths;
        private Telerik.Reporting.SqlDataSource sqlSchoolYears;
        private Telerik.Reporting.Shape shape4;

    }
}