
namespace Modules.BusinessObjects.Report
{
    partial class FolderLabelReport
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
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrBarCode1 = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblJobID = new DevExpress.XtraReports.UI.XRLabel();
            this.lblDateReceived = new DevExpress.XtraReports.UI.XRLabel();
            this.lblProjectID = new DevExpress.XtraReports.UI.XRLabel();
            this.lblClient = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrBarCode1,
            this.xrLabel3,
            this.xrLabel1,
            this.lblJobID,
            this.lblDateReceived,
            this.lblProjectID,
            this.lblClient});
            this.Detail.HeightF = 112.3753F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.PageBreak = DevExpress.XtraReports.UI.PageBreak.BeforeBand;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrBarCode1
            // 
            this.xrBarCode1.Font = new System.Drawing.Font("Times New Roman", 7.5F);
            this.xrBarCode1.LocationFloat = new DevExpress.Utils.PointFloat(10F, 8F);
            this.xrBarCode1.Name = "xrBarCode1";
            this.xrBarCode1.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrBarCode1.SizeF = new System.Drawing.SizeF(85.46F, 64.17F);
            this.xrBarCode1.StylePriority.UseFont = false;
            this.xrBarCode1.Symbology = qrCodeGenerator1;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(108.42F, 72.17F);
            this.xrLabel3.Multiline = true;
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(91.21F, 14F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.Text = "Project ID:";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(108.42F, 58.17F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(91.21F, 14F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "Date Received:";
            // 
            // lblJobID
            // 
            this.lblJobID.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblJobID.LocationFloat = new DevExpress.Utils.PointFloat(108.42F, 13.92F);
            this.lblJobID.Multiline = true;
            this.lblJobID.Name = "lblJobID";
            this.lblJobID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblJobID.SizeF = new System.Drawing.SizeF(206.17F, 20.71F);
            this.lblJobID.StylePriority.UseFont = false;
            // 
            // lblDateReceived
            // 
            this.lblDateReceived.CanGrow = false;
            this.lblDateReceived.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblDateReceived.LocationFloat = new DevExpress.Utils.PointFloat(199.63F, 58.17F);
            this.lblDateReceived.Multiline = true;
            this.lblDateReceived.Name = "lblDateReceived";
            this.lblDateReceived.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblDateReceived.SizeF = new System.Drawing.SizeF(139.37F, 14F);
            this.lblDateReceived.StylePriority.UseFont = false;
            this.lblDateReceived.TextFormatString = "{0:MM/dd/yyyy}";
            // 
            // lblProjectID
            // 
            this.lblProjectID.CanGrow = false;
            this.lblProjectID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.lblProjectID.LocationFloat = new DevExpress.Utils.PointFloat(199.63F, 72.17001F);
            this.lblProjectID.Multiline = true;
            this.lblProjectID.Name = "lblProjectID";
            this.lblProjectID.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblProjectID.SizeF = new System.Drawing.SizeF(139.37F, 14F);
            this.lblProjectID.StylePriority.UseFont = false;
            this.lblProjectID.TextFormatString = "{0:MM/dd/yyyy}";
            // 
            // lblClient
            // 
            this.lblClient.CanGrow = false;
            this.lblClient.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F);
            this.lblClient.LocationFloat = new DevExpress.Utils.PointFloat(108.42F, 34.63F);
            this.lblClient.Multiline = true;
            this.lblClient.Name = "lblClient";
            this.lblClient.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblClient.SizeF = new System.Drawing.SizeF(237.58F, 23.54F);
            this.lblClient.StylePriority.UseFont = false;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // FolderLabelReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.PageHeight = 112;
            this.PageWidth = 349;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.Version = "20.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRLabel lblClient;
        private DevExpress.XtraReports.UI.XRLabel lblJobID;
        private DevExpress.XtraReports.UI.XRLabel lblDateReceived;
        private DevExpress.XtraReports.UI.XRLabel lblProjectID;
        private DevExpress.XtraReports.UI.XRBarCode xrBarCode1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
    }
}
