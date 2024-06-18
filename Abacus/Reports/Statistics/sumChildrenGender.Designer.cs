namespace Abacus.Reports.Statistics
{
    partial class sumChildrenGender
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.GraphGroup graphGroup1 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.GraphTitle graphTitle1 = new Telerik.Reporting.GraphTitle();
            Telerik.Reporting.NumericalScale numericalScale1 = new Telerik.Reporting.NumericalScale();
            Telerik.Reporting.CategoryScale categoryScale1 = new Telerik.Reporting.CategoryScale();
            Telerik.Reporting.GraphGroup graphGroup2 = new Telerik.Reporting.GraphGroup();
            Telerik.Reporting.TypeReportSource typeReportSource1 = new Telerik.Reporting.TypeReportSource();
            Telerik.Reporting.Group group1 = new Telerik.Reporting.Group();
            Telerik.Reporting.Group group2 = new Telerik.Reporting.Group();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            this.�������_����GroupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.cATEGORY_TEXTCountFunctionTextBox = new Telerik.Reporting.TextBox();
            this.graph1 = new Telerik.Reporting.Graph();
            this.polarCoordinateSystem1 = new Telerik.Reporting.PolarCoordinateSystem();
            this.graphAxis1 = new Telerik.Reporting.GraphAxis();
            this.graphAxis2 = new Telerik.Reporting.GraphAxis();
            this.sqlDataSource = new Telerik.Reporting.SqlDataSource();
            this.barSeries2 = new Telerik.Reporting.BarSeries();
            this.�������_����GroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.����_����DataTextBox = new Telerik.Reporting.TextBox();
            this.����_����CaptionTextBox = new Telerik.Reporting.TextBox();
            this.labelsGroupFooterSection = new Telerik.Reporting.GroupFooterSection();
            this.labelsGroupHeaderSection = new Telerik.Reporting.GroupHeaderSection();
            this.cATEGORY_TEXTCaptionTextBox = new Telerik.Reporting.TextBox();
            this.������CaptionTextBox = new Telerik.Reporting.TextBox();
            this.������CaptionTextBox = new Telerik.Reporting.TextBox();
            this.pOSOSTOCaptionTextBox = new Telerik.Reporting.TextBox();
            this.sqlSchoolYears = new Telerik.Reporting.SqlDataSource();
            this.pageFooter = new Telerik.Reporting.PageFooterSection();
            this.textBox16 = new Telerik.Reporting.TextBox();
            this.textBox20 = new Telerik.Reporting.TextBox();
            this.pageInfoTextBox = new Telerik.Reporting.TextBox();
            this.currentTimeTextBox = new Telerik.Reporting.TextBox();
            this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
            this.subReport1 = new Telerik.Reporting.SubReport();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.detail = new Telerik.Reporting.DetailSection();
            this.cATEGORY_TEXTDataTextBox = new Telerik.Reporting.TextBox();
            this.������DataTextBox = new Telerik.Reporting.TextBox();
            this.������DataTextBox = new Telerik.Reporting.TextBox();
            this.pOSOSTODataTextBox = new Telerik.Reporting.TextBox();
            this.shape1 = new Telerik.Reporting.Shape();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // �������_����GroupFooterSection
            // 
            this.�������_����GroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(13.953134536743164D);
            this.�������_����GroupFooterSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.cATEGORY_TEXTCountFunctionTextBox,
            this.graph1});
            this.�������_����GroupFooterSection.Name = "�������_����GroupFooterSection";
            this.�������_����GroupFooterSection.Style.BackgroundColor = System.Drawing.Color.LightGray;
            // 
            // cATEGORY_TEXTCountFunctionTextBox
            // 
            this.cATEGORY_TEXTCountFunctionTextBox.CanGrow = true;
            this.cATEGORY_TEXTCountFunctionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010002215276472271D), Telerik.Reporting.Drawing.Unit.Cm(0.10573320835828781D));
            this.cATEGORY_TEXTCountFunctionTextBox.Name = "cATEGORY_TEXTCountFunctionTextBox";
            this.cATEGORY_TEXTCountFunctionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(17.718547821044922D), Telerik.Reporting.Drawing.Unit.Cm(0.64739948511123657D));
            this.cATEGORY_TEXTCountFunctionTextBox.Style.Font.Bold = true;
            this.cATEGORY_TEXTCountFunctionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.cATEGORY_TEXTCountFunctionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.cATEGORY_TEXTCountFunctionTextBox.StyleName = "Data";
            this.cATEGORY_TEXTCountFunctionTextBox.Value = "= \"������ ��� �� ������� ���� : \" + Fields.�������_����";
            // 
            // graph1
            // 
            graphGroup1.Label = "����";
            graphGroup1.Name = "categoryGroup";
            this.graph1.CategoryGroups.Add(graphGroup1);
            this.graph1.CoordinateSystems.Add(this.polarCoordinateSystem1);
            this.graph1.DataSource = this.sqlDataSource;
            this.graph1.Filters.Add(new Telerik.Reporting.Filter("=Fields.SCHOOLYEAR_ID", Telerik.Reporting.FilterOperator.Equal, "=Parameters.school_year.Value"));
            this.graph1.Legend.IsInsidePlotArea = false;
            this.graph1.Legend.Style.LineColor = System.Drawing.Color.LightGray;
            this.graph1.Legend.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            this.graph1.Legend.Style.Visible = true;
            this.graph1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.47624999284744263D), Telerik.Reporting.Drawing.Unit.Cm(0.89958328008651733D));
            this.graph1.Name = "graph1";
            this.graph1.NoDataMessage = "";
            this.graph1.PlotAreaStyle.LineColor = System.Drawing.Color.LightGray;
            this.graph1.PlotAreaStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            this.graph1.Series.Add(this.barSeries2);
            this.graph1.SeriesGroups.Add(graphGroup2);
            this.graph1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(16.893966674804688D), Telerik.Reporting.Drawing.Unit.Cm(12.453550338745117D));
            this.graph1.Style.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            graphTitle1.Style.Font.Bold = true;
            graphTitle1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            graphTitle1.Style.LineColor = System.Drawing.Color.LightGray;
            graphTitle1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            graphTitle1.Text = "=\"�������� ����������� ��� ����\" + \" - ����. ����: \" + Fields.�������_����";
            this.graph1.Titles.Add(graphTitle1);
            // 
            // polarCoordinateSystem1
            // 
            this.polarCoordinateSystem1.AngularAxis = this.graphAxis1;
            this.polarCoordinateSystem1.Name = "polarCoordinateSystem2";
            this.polarCoordinateSystem1.RadialAxis = this.graphAxis2;
            // 
            // graphAxis1
            // 
            this.graphAxis1.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis1.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis1.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis1.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis1.MinorGridLineStyle.Visible = false;
            this.graphAxis1.Name = "GraphAxis2";
            this.graphAxis1.Scale = numericalScale1;
            this.graphAxis1.Style.Visible = false;
            // 
            // graphAxis2
            // 
            this.graphAxis2.MajorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis2.MajorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis2.MinorGridLineStyle.LineColor = System.Drawing.Color.LightGray;
            this.graphAxis2.MinorGridLineStyle.LineWidth = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.graphAxis2.MinorGridLineStyle.Visible = false;
            this.graphAxis2.Name = "GraphAxis1";
            categoryScale1.SpacingSlotCount = 0D;
            this.graphAxis2.Scale = categoryScale1;
            this.graphAxis2.Style.Visible = false;
            // 
            // sqlDataSource
            // 
            this.sqlDataSource.ConnectionString = "Abacus.Properties.Settings.DBConnectionString";
            this.sqlDataSource.Name = "sqlDataSource";
            this.sqlDataSource.SelectCommand = "SELECT        SCHOOLYEAR_ID, �������_����, ����, ������, TOTAL, �������\r\nFROM    " +
    "        sum������_����_������";
            // 
            // barSeries2
            // 
            this.barSeries2.ArrangeMode = Telerik.Reporting.GraphSeriesArrangeMode.Stacked100;
            this.barSeries2.CategoryGroup = graphGroup1;
            this.barSeries2.CoordinateSystem = this.polarCoordinateSystem1;
            this.barSeries2.DataPointLabel = "= Fields.�������";
            this.barSeries2.DataPointLabelFormat = "{0:P2}";
            this.barSeries2.DataPointLabelStyle.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.barSeries2.LegendItem.Style.BackgroundColor = System.Drawing.Color.Transparent;
            this.barSeries2.LegendItem.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.barSeries2.LegendItem.Style.LineColor = System.Drawing.Color.Transparent;
            this.barSeries2.LegendItem.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Cm(0D);
            this.barSeries2.LegendItem.Value = "= Fields.����";
            this.barSeries2.Name = "barSeries1";
            graphGroup2.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.����"));
            graphGroup2.Name = "seriesGroup";
            graphGroup2.Sortings.Add(new Telerik.Reporting.Sorting("=Fields.�������", Telerik.Reporting.SortDirection.Desc));
            this.barSeries2.SeriesGroup = graphGroup2;
            this.barSeries2.X = "= Fields.������";
            // 
            // �������_����GroupHeaderSection
            // 
            this.�������_����GroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.70000004768371582D);
            this.�������_����GroupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.����_����DataTextBox,
            this.����_����CaptionTextBox});
            this.�������_����GroupHeaderSection.Name = "�������_����GroupHeaderSection";
            // 
            // ����_����DataTextBox
            // 
            this.����_����DataTextBox.CanGrow = true;
            this.����_����DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.5888519287109375D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.����_����DataTextBox.Name = "����_����DataTextBox";
            this.����_����DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(15.252397537231445D), Telerik.Reporting.Drawing.Unit.Cm(0.64708298444747925D));
            this.����_����DataTextBox.Style.BackgroundColor = System.Drawing.Color.LightGray;
            this.����_����DataTextBox.Style.Font.Bold = true;
            this.����_����DataTextBox.Style.Font.Name = "Calibri";
            this.����_����DataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.����_����DataTextBox.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(4D);
            this.����_����DataTextBox.StyleName = "Data";
            this.����_����DataTextBox.Value = "= Fields.�������_����";
            // 
            // ����_����CaptionTextBox
            // 
            this.����_����CaptionTextBox.CanGrow = true;
            this.����_����CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.����_����CaptionTextBox.Name = "����_����CaptionTextBox";
            this.����_����CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.5886514186859131D), Telerik.Reporting.Drawing.Unit.Cm(0.64708298444747925D));
            this.����_����CaptionTextBox.Style.BackgroundColor = System.Drawing.Color.LightGray;
            this.����_����CaptionTextBox.Style.Font.Bold = true;
            this.����_����CaptionTextBox.Style.Font.Name = "Calibri";
            this.����_����CaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.����_����CaptionTextBox.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(4D);
            this.����_����CaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.����_����CaptionTextBox.StyleName = "Caption";
            this.����_����CaptionTextBox.Value = "������� ����:";
            // 
            // labelsGroupFooterSection
            // 
            this.labelsGroupFooterSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
            this.labelsGroupFooterSection.Name = "labelsGroupFooterSection";
            this.labelsGroupFooterSection.Style.Visible = false;
            // 
            // labelsGroupHeaderSection
            // 
            this.labelsGroupHeaderSection.Height = Telerik.Reporting.Drawing.Unit.Cm(0.69999963045120239D);
            this.labelsGroupHeaderSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.cATEGORY_TEXTCaptionTextBox,
            this.������CaptionTextBox,
            this.������CaptionTextBox,
            this.pOSOSTOCaptionTextBox});
            this.labelsGroupHeaderSection.Name = "labelsGroupHeaderSection";
            this.labelsGroupHeaderSection.PrintOnEveryPage = true;
            // 
            // cATEGORY_TEXTCaptionTextBox
            // 
            this.cATEGORY_TEXTCaptionTextBox.CanGrow = true;
            this.cATEGORY_TEXTCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.9995224445592612E-05D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.cATEGORY_TEXTCaptionTextBox.Name = "cATEGORY_TEXTCaptionTextBox";
            this.cATEGORY_TEXTCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.387293815612793D), Telerik.Reporting.Drawing.Unit.Cm(0.64708214998245239D));
            this.cATEGORY_TEXTCaptionTextBox.Style.Font.Bold = true;
            this.cATEGORY_TEXTCaptionTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.cATEGORY_TEXTCaptionTextBox.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            this.cATEGORY_TEXTCaptionTextBox.StyleName = "Caption";
            this.cATEGORY_TEXTCaptionTextBox.Value = "����";
            // 
            // ������CaptionTextBox
            // 
            this.������CaptionTextBox.CanGrow = true;
            this.������CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.3875961303710938D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.������CaptionTextBox.Name = "������CaptionTextBox";
            this.������CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.5122027397155762D), Telerik.Reporting.Drawing.Unit.Cm(0.64708214998245239D));
            this.������CaptionTextBox.Style.Font.Bold = true;
            this.������CaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.������CaptionTextBox.StyleName = "Caption";
            this.������CaptionTextBox.Value = "������";
            // 
            // ������CaptionTextBox
            // 
            this.������CaptionTextBox.CanGrow = true;
            this.������CaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.899999618530273D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.������CaptionTextBox.Name = "������CaptionTextBox";
            this.������CaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.599799633026123D), Telerik.Reporting.Drawing.Unit.Cm(0.64708214998245239D));
            this.������CaptionTextBox.Style.Font.Bold = true;
            this.������CaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.������CaptionTextBox.StyleName = "Caption";
            this.������CaptionTextBox.Value = "������ �������";
            // 
            // pOSOSTOCaptionTextBox
            // 
            this.pOSOSTOCaptionTextBox.CanGrow = true;
            this.pOSOSTOCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.499999046325684D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.pOSOSTOCaptionTextBox.Name = "pOSOSTOCaptionTextBox";
            this.pOSOSTOCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.3411498069763184D), Telerik.Reporting.Drawing.Unit.Cm(0.64708298444747925D));
            this.pOSOSTOCaptionTextBox.Style.Font.Bold = true;
            this.pOSOSTOCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.pOSOSTOCaptionTextBox.StyleName = "Caption";
            this.pOSOSTOCaptionTextBox.Value = "�������";
            // 
            // sqlSchoolYears
            // 
            this.sqlSchoolYears.ConnectionString = "Abacus.Properties.Settings.DBConnectionString";
            this.sqlSchoolYears.Name = "sqlSchoolYears";
            this.sqlSchoolYears.SelectCommand = "SELECT        SCHOOLYEAR_ID, �������_����\r\nFROM            ���_�������_���";
            // 
            // pageFooter
            // 
            this.pageFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(1.0357164144515991D);
            this.pageFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox16,
            this.textBox20,
            this.pageInfoTextBox,
            this.currentTimeTextBox});
            this.pageFooter.Name = "pageFooter";
            // 
            // textBox16
            // 
            this.textBox16.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.8999996185302734D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.8285512924194336D), Telerik.Reporting.Drawing.Unit.Cm(0.45978257060050964D));
            this.textBox16.Style.Font.Name = "Calibri";
            this.textBox16.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox16.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox16.StyleName = "PageInfo";
            this.textBox16.Value = "��������� ����� ������������� �������";
            // 
            // textBox20
            // 
            this.textBox20.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox20.Name = "textBox20";
            this.textBox20.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.8412480354309082D), Telerik.Reporting.Drawing.Unit.Cm(0.45978257060050964D));
            this.textBox20.Style.Font.Name = "Calibri";
            this.textBox20.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
            this.textBox20.StyleName = "PageInfo";
            this.textBox20.Value = "�������� ABACUS";
            // 
            // pageInfoTextBox
            // 
            this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.8999996185302734D), Telerik.Reporting.Drawing.Unit.Cm(0.4763500988483429D));
            this.pageInfoTextBox.Name = "pageInfoTextBox";
            this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.8186502456665039D), Telerik.Reporting.Drawing.Unit.Cm(0.55936628580093384D));
            this.pageInfoTextBox.Style.Font.Name = "Calibri";
            this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.pageInfoTextBox.StyleName = "PageInfo";
            this.pageInfoTextBox.Value = "=\"���. \" + PageNumber + \"/\" + PageCount";
            // 
            // currentTimeTextBox
            // 
            this.currentTimeTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0.4763500988483429D));
            this.currentTimeTextBox.Name = "currentTimeTextBox";
            this.currentTimeTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.8412480354309082D), Telerik.Reporting.Drawing.Unit.Cm(0.55936628580093384D));
            this.currentTimeTextBox.Style.Font.Name = "Calibri";
            this.currentTimeTextBox.StyleName = "PageInfo";
            this.currentTimeTextBox.Value = "=NOW()";
            // 
            // reportHeader
            // 
            this.reportHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(4D);
            this.reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.subReport1,
            this.textBox1});
            this.reportHeader.Name = "reportHeader";
            // 
            // subReport1
            // 
            this.subReport1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.subReport1.Name = "subReport1";
            typeReportSource1.TypeName = "Abacus.Reports.LogoA2, Abacus, Version=1.0.0.0, Culture=neutral, PublicKeyToken=n" +
    "ull";
            this.subReport1.ReportSource = typeReportSource1;
            this.subReport1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(10.100000381469727D), Telerik.Reporting.Drawing.Unit.Cm(2.9999997615814209D));
            // 
            // textBox1
            // 
            this.textBox1.CanGrow = true;
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(3.2000000476837158D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(17.841251373291016D), Telerik.Reporting.Drawing.Unit.Cm(0.70000004768371582D));
            this.textBox1.Style.Font.Bold = true;
            this.textBox1.Style.Font.Name = "Calibri";
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox1.StyleName = "Caption";
            this.textBox1.Value = "������������� ��������� ������� ����������� ��� ����";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(0.73249238729476929D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.cATEGORY_TEXTDataTextBox,
            this.������DataTextBox,
            this.������DataTextBox,
            this.pOSOSTODataTextBox,
            this.shape1});
            this.detail.Name = "detail";
            // 
            // cATEGORY_TEXTDataTextBox
            // 
            this.cATEGORY_TEXTDataTextBox.CanGrow = true;
            this.cATEGORY_TEXTDataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.9995224445592612E-05D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.cATEGORY_TEXTDataTextBox.Name = "cATEGORY_TEXTDataTextBox";
            this.cATEGORY_TEXTDataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.387293815612793D), Telerik.Reporting.Drawing.Unit.Cm(0.54708343744277954D));
            this.cATEGORY_TEXTDataTextBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.cATEGORY_TEXTDataTextBox.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Pixel(5D);
            this.cATEGORY_TEXTDataTextBox.StyleName = "Data";
            this.cATEGORY_TEXTDataTextBox.Value = "= Fields.����";
            // 
            // ������DataTextBox
            // 
            this.������DataTextBox.CanGrow = true;
            this.������DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.3875942230224609D), Telerik.Reporting.Drawing.Unit.Cm(0.00020024616969749332D));
            this.������DataTextBox.Name = "������DataTextBox";
            this.������DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.51220440864563D), Telerik.Reporting.Drawing.Unit.Cm(0.59979987144470215D));
            this.������DataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.������DataTextBox.StyleName = "Data";
            this.������DataTextBox.Value = "=Fields.������";
            // 
            // ������DataTextBox
            // 
            this.������DataTextBox.CanGrow = true;
            this.������DataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.899999618530273D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.������DataTextBox.Name = "������DataTextBox";
            this.������DataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.5998010635375977D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.������DataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.������DataTextBox.StyleName = "Data";
            this.������DataTextBox.Value = "= Fields.TOTAL";
            // 
            // pOSOSTODataTextBox
            // 
            this.pOSOSTODataTextBox.CanGrow = true;
            this.pOSOSTODataTextBox.Format = "{0:P1}";
            this.pOSOSTODataTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.5D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.pOSOSTODataTextBox.Name = "pOSOSTODataTextBox";
            this.pOSOSTODataTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.3411514759063721D), Telerik.Reporting.Drawing.Unit.Cm(0.60000014305114746D));
            this.pOSOSTODataTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.pOSOSTODataTextBox.StyleName = "Data";
            this.pOSOSTODataTextBox.Value = "= Fields.�������";
            // 
            // shape1
            // 
            this.shape1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.9995224445592612E-05D), Telerik.Reporting.Drawing.Unit.Cm(0.60020077228546143D));
            this.shape1.Name = "shape1";
            this.shape1.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(17.841047286987305D), Telerik.Reporting.Drawing.Unit.Cm(0.13229165971279144D));
            // 
            // sumChildrenGender
            // 
            this.DataSource = this.sqlDataSource;
            this.Filters.Add(new Telerik.Reporting.Filter("=Fields.SCHOOLYEAR_ID", Telerik.Reporting.FilterOperator.Equal, "=Parameters.school_year.Value"));
            group1.GroupFooter = this.�������_����GroupFooterSection;
            group1.GroupHeader = this.�������_����GroupHeaderSection;
            group1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.�������_����"));
            group1.Name = "�������_����Group";
            group2.GroupFooter = this.labelsGroupFooterSection;
            group2.GroupHeader = this.labelsGroupHeaderSection;
            group2.Name = "labelsGroup";
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            group1,
            group2});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.�������_����GroupHeaderSection,
            this.�������_����GroupFooterSection,
            this.labelsGroupHeaderSection,
            this.labelsGroupFooterSection,
            this.pageFooter,
            this.reportHeader,
            this.detail});
            this.Name = "sumChildrenGender";
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(20D), Telerik.Reporting.Drawing.Unit.Mm(10D), Telerik.Reporting.Drawing.Unit.Mm(20D), Telerik.Reporting.Drawing.Unit.Mm(20D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.AllowNull = true;
            reportParameter1.AutoRefresh = true;
            reportParameter1.AvailableValues.DataSource = this.sqlSchoolYears;
            reportParameter1.AvailableValues.DisplayMember = "= Fields.�������_����";
            reportParameter1.AvailableValues.ValueMember = "= Fields.SCHOOLYEAR_ID";
            reportParameter1.Name = "school_year";
            reportParameter1.Text = "������� ����";
            reportParameter1.Type = Telerik.Reporting.ReportParameterType.Integer;
            reportParameter1.Visible = true;
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
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(17.841251373291016D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.SqlDataSource sqlDataSource;
        private Telerik.Reporting.GroupHeaderSection �������_����GroupHeaderSection;
        private Telerik.Reporting.GroupFooterSection �������_����GroupFooterSection;
        private Telerik.Reporting.TextBox cATEGORY_TEXTCountFunctionTextBox;
        private Telerik.Reporting.GroupHeaderSection labelsGroupHeaderSection;
        private Telerik.Reporting.TextBox cATEGORY_TEXTCaptionTextBox;
        private Telerik.Reporting.TextBox ������CaptionTextBox;
        private Telerik.Reporting.TextBox ������CaptionTextBox;
        private Telerik.Reporting.TextBox pOSOSTOCaptionTextBox;
        private Telerik.Reporting.GroupFooterSection labelsGroupFooterSection;
        private Telerik.Reporting.PageFooterSection pageFooter;
        private Telerik.Reporting.ReportHeaderSection reportHeader;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.TextBox cATEGORY_TEXTDataTextBox;
        private Telerik.Reporting.TextBox ������DataTextBox;
        private Telerik.Reporting.TextBox ������DataTextBox;
        private Telerik.Reporting.TextBox pOSOSTODataTextBox;
        private Telerik.Reporting.TextBox ����_����DataTextBox;
        private Telerik.Reporting.TextBox ����_����CaptionTextBox;
        private Telerik.Reporting.SubReport subReport1;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.Shape shape1;
        private Telerik.Reporting.Graph graph1;
        private Telerik.Reporting.PolarCoordinateSystem polarCoordinateSystem1;
        private Telerik.Reporting.GraphAxis graphAxis1;
        private Telerik.Reporting.GraphAxis graphAxis2;
        private Telerik.Reporting.BarSeries barSeries2;
        private Telerik.Reporting.TextBox textBox16;
        private Telerik.Reporting.TextBox textBox20;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private Telerik.Reporting.TextBox currentTimeTextBox;
        private Telerik.Reporting.SqlDataSource sqlSchoolYears;

    }
}