﻿namespace BTLIMS.Module {
	partial class BTLIMSModule {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            // 
            // BTLIMSModule
            // 
            this.AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.BaseObject));
            this.AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.FileData));
            this.AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.FileAttachmentBase));
            this.AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.HCategory));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.SystemModule.SystemModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.CloneObject.CloneObjectModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ReportsV2.ReportsModuleV2));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.TreeListEditors.TreeListEditorsModuleBase));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Validation.ValidationModule));
            this.RequiredModuleTypes.Add(typeof(Modules.ModulesModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Chart.ChartModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Kpi.KpiModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.PivotGrid.PivotGridModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Dashboards.DashboardsModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Security.SecurityModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Notifications.NotificationsModule));

		}

		#endregion
	}
}
