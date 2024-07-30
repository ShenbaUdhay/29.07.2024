
namespace Modules.BusinessObjects.Report
{
    partial class SampleReport
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

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraPrinting.BarCode.QRCodeGenerator qrCodeGenerator1 = new DevExpress.XtraPrinting.BarCode.QRCodeGenerator();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrBarCode1 = new DevExpress.XtraReports.UI.XRBarCode();
            this.lblTest = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblDueDate = new DevExpress.XtraReports.UI.XRLabel();
            this.lblRecievedDate = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCollectedDate = new DevExpress.XtraReports.UI.XRLabel();
            this.lblSampleMatrix = new DevExpress.XtraReports.UI.XRLabel();
            this.lblSampleName = new DevExpress.XtraReports.UI.XRLabel();
            this.lblSampleID = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrBarCode1,
            this.lblTest,
            this.xrLabel6,
            this.xrLabel5,
            this.xrLabel4,
            this.xrLabel3,
            this.xrLabel2,
            this.lblDueDate,
            this.lblRecievedDate,
            this.lblCollectedDate,
            this.lblSampleMatrix,
            this.lblSampleName,
            this.lblSampleID});
            this.Detail.HeightF = 76.00001F;
            this.Detail.Name = "Detail";
            // 
            // xrBarCode1
            // 
            this.xrBarCode1.Alignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrBarCode1.Font = new System.Drawing.Font("Times New Roman", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrBarCode1.LocationFloat = new DevExpress.Utils.PointFloat(180.48F, 5.999998F);
            this.xrBarCode1.Name = "xrBarCode1";
            this.xrBarCode1.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrBarCode1.ShowText = false;
            this.xrBarCode1.SizeF = new System.Drawing.SizeF(85F, 48F);
            this.xrBarCode1.StylePriority.UseFont = false;
            this.xrBarCode1.Symbology = qrCodeGenerator1;
            // 
            // lblTest
            // 
            this.lblTest.CanGrow = false;
            this.lblTest.Font = new System.Drawing.Font("Times New Roman", 7.5F);
            this.lblTest.LocationFloat = new DevExpress.Utils.PointFloat(180.48F, 57F);
            this.lblTest.Multiline = true;
            this.lblTest.Name = "lblTest";
            this.lblTest.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTest.SizeF = new System.Drawing.SizeF(84.99998F, 19.00001F);
            this.lblTest.StylePriority.UseFont = false;
            this.lblTest.StylePriority.UseTextAlignment = false;
            this.lblTest.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel6
            // 
            this.xrLabel6.Font = new System.Drawing.Font("Times New Roman", 7.5F);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(10.54F, 65F);
            this.xrLabel6.Multiline = true;
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(73.47F, 11F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.Text = "Due Date:";
            this.xrLabel6.WordWrap = false;
            // 
            // xrLabel5
            // 
            this.xrLabel5.Font = new System.Drawing.Font("Times New Roman", 7.5F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 54F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(73.47F, 11F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.Text = "Date Received:";
            this.xrLabel5.WordWrap = false;
            // 
            // xrLabel4
            // 
            this.xrLabel4.CanGrow = false;
            this.xrLabel4.Font = new System.Drawing.Font("Times New Roman", 7.5F);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(10.54F, 43F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(73.47F, 11F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "Date Collected:";
            this.xrLabel4.WordWrap = false;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Times New Roman", 7.5F);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(10.54F, 32F);
            this.xrLabel3.Multiline = true;
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(73.47F, 11F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.Text = "Sample Matrix:";
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new System.Drawing.Font("Times New Roman", 7.5F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(10.54F, 20F);
            this.xrLabel2.Multiline = true;
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(73.47F, 11F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "Sample Name:";
            // 
            // lblDueDate
            // 
            this.lblDueDate.Font = new System.Drawing.Font("Times New Roman", 7.5F);
            this.lblDueDate.LocationFloat = new DevExpress.Utils.PointFloat(84.53F, 65F);
            this.lblDueDate.Name = "lblDueDate";
            this.lblDueDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblDueDate.SizeF = new System.Drawing.SizeF(93.2F, 11F);
            this.lblDueDate.StylePriority.UseFont = false;
            this.lblDueDate.TextFormatString = "{0:MM/dd/yy HH:mm}";
            this.lblDueDate.WordWrap = false;
            // 
            // lblRecievedDate
            // 
            this.lblRecievedDate.Font = new System.Drawing.Font("Times New Roman", 7.5F);
            this.lblRecievedDate.LocationFloat = new DevExpress.Utils.PointFloat(84.53F, 54F);
            this.lblRecievedDate.Name = "lblRecievedDate";
            this.lblRecievedDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblRecievedDate.SizeF = new System.Drawing.SizeF(92.67F, 11F);
            this.lblRecievedDate.StylePriority.UseFont = false;
            this.lblRecievedDate.TextFormatString = "{0:MM/dd/yy HH:mm}";
            this.lblRecievedDate.WordWrap = false;
            // 
            // lblCollectedDate
            // 
            this.lblCollectedDate.Font = new System.Drawing.Font("Times New Roman", 7.5F);
            this.lblCollectedDate.LocationFloat = new DevExpress.Utils.PointFloat(84.00999F, 43F);
            this.lblCollectedDate.Name = "lblCollectedDate";
            this.lblCollectedDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblCollectedDate.SizeF = new System.Drawing.SizeF(93.2F, 11F);
            this.lblCollectedDate.StylePriority.UseFont = false;
            this.lblCollectedDate.TextFormatString = "{0:MM/dd/yy HH:mm}";
            this.lblCollectedDate.WordWrap = false;
            // 
            // lblSampleMatrix
            // 
            this.lblSampleMatrix.Font = new System.Drawing.Font("Times New Roman", 7.5F);
            this.lblSampleMatrix.LocationFloat = new DevExpress.Utils.PointFloat(84.00999F, 32.00001F);
            this.lblSampleMatrix.Name = "lblSampleMatrix";
            this.lblSampleMatrix.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblSampleMatrix.SizeF = new System.Drawing.SizeF(93.18999F, 11F);
            this.lblSampleMatrix.StylePriority.UseFont = false;
            this.lblSampleMatrix.WordWrap = false;
            // 
            // lblSampleName
            // 
            this.lblSampleName.Font = new System.Drawing.Font("Times New Roman", 7.5F);
            this.lblSampleName.LocationFloat = new DevExpress.Utils.PointFloat(84.00999F, 20.00001F);
            this.lblSampleName.Name = "lblSampleName";
            this.lblSampleName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblSampleName.SizeF = new System.Drawing.SizeF(93.19F, 11F);
            this.lblSampleName.StylePriority.UseFont = false;
            this.lblSampleName.WordWrap = false;
            // 
            // lblSampleID
            // 
            this.lblSampleID.Font = new System.Drawing.Font("Times New Roman", 7.5F, System.Drawing.FontStyle.Bold);
            this.lblSampleID.LocationFloat = new DevExpress.Utils.PointFloat(10F, 5.000003F);
            this.lblSampleID.Multiline = true;
            this.lblSampleID.Name = "lblSampleID";
            this.lblSampleID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblSampleID.SizeF = new System.Drawing.SizeF(166.67F, 14F);
            this.lblSampleID.StylePriority.UseFont = false;
            // 
            // SampleReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.Detail});
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.PageHeight = 100;
            this.PageWidth = 271;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.Version = "20.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.XRLabel lblSampleID;
        private DevExpress.XtraReports.UI.XRLabel lblSampleName;
        private DevExpress.XtraReports.UI.XRLabel lblSampleMatrix;
        private DevExpress.XtraReports.UI.XRLabel lblCollectedDate;
        private DevExpress.XtraReports.UI.XRLabel lblDueDate;
        private DevExpress.XtraReports.UI.XRLabel lblRecievedDate;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel lblTest;
        private DevExpress.XtraReports.UI.XRBarCode xrBarCode1;
    }
}
