namespace Modules.ConnectionForm
{
    partial class frmConnection
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblError = new DevExpress.XtraEditors.LabelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.lblSQLConnection = new DevExpress.XtraEditors.LabelControl();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.lblPassword = new DevExpress.XtraEditors.LabelControl();
            this.lblUserName = new DevExpress.XtraEditors.LabelControl();
            this.lblDataBase = new DevExpress.XtraEditors.LabelControl();
            this.lblServerName = new DevExpress.XtraEditors.LabelControl();
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.txtUserName = new DevExpress.XtraEditors.TextEdit();
            this.txtDataBase = new DevExpress.XtraEditors.TextEdit();
            this.txtServerName = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataBase.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServerName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lblError
            // 
            this.lblError.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblError.Appearance.ForeColor = System.Drawing.Color.Red;
            this.lblError.Appearance.Options.UseFont = true;
            this.lblError.Appearance.Options.UseForeColor = true;
            this.lblError.Location = new System.Drawing.Point(-5, 202);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(26, 14);
            this.lblError.TabIndex = 23;
            this.lblError.Text = "Error";
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.Location = new System.Drawing.Point(170, 222);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "Cancel";
            // 
            // lblSQLConnection
            // 
            this.lblSQLConnection.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.lblSQLConnection.Appearance.Options.UseFont = true;
            this.lblSQLConnection.Location = new System.Drawing.Point(119, 17);
            this.lblSQLConnection.Name = "lblSQLConnection";
            this.lblSQLConnection.Size = new System.Drawing.Size(98, 14);
            this.lblSQLConnection.TabIndex = 21;
            this.lblSQLConnection.Text = "SQL Connection";
            // 
            // btnOK
            // 
            this.btnOK.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.btnOK.Appearance.Options.UseFont = true;
            this.btnOK.Location = new System.Drawing.Point(63, 222);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 20;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblPassword
            // 
            this.lblPassword.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblPassword.Appearance.Options.UseFont = true;
            this.lblPassword.Location = new System.Drawing.Point(19, 175);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(51, 14);
            this.lblPassword.TabIndex = 19;
            this.lblPassword.Text = "Password";
            // 
            // lblUserName
            // 
            this.lblUserName.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblUserName.Appearance.Options.UseFont = true;
            this.lblUserName.Location = new System.Drawing.Point(19, 135);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(58, 14);
            this.lblUserName.TabIndex = 18;
            this.lblUserName.Text = "User name";
            // 
            // lblDataBase
            // 
            this.lblDataBase.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblDataBase.Appearance.Options.UseFont = true;
            this.lblDataBase.Location = new System.Drawing.Point(19, 95);
            this.lblDataBase.Name = "lblDataBase";
            this.lblDataBase.Size = new System.Drawing.Size(50, 14);
            this.lblDataBase.TabIndex = 17;
            this.lblDataBase.Text = "Database";
            // 
            // lblServerName
            // 
            this.lblServerName.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblServerName.Appearance.Options.UseFont = true;
            this.lblServerName.Location = new System.Drawing.Point(19, 55);
            this.lblServerName.Name = "lblServerName";
            this.lblServerName.Size = new System.Drawing.Size(69, 14);
            this.lblServerName.TabIndex = 16;
            this.lblServerName.Text = "Server name";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(106, 169);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtPassword.Properties.Appearance.Options.UseFont = true;
            this.txtPassword.Properties.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(183, 20);
            this.txtPassword.TabIndex = 15;
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(106, 132);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtUserName.Properties.Appearance.Options.UseFont = true;
            this.txtUserName.Size = new System.Drawing.Size(183, 20);
            this.txtUserName.TabIndex = 14;
            // 
            // txtDataBase
            // 
            this.txtDataBase.Location = new System.Drawing.Point(106, 92);
            this.txtDataBase.Name = "txtDataBase";
            this.txtDataBase.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtDataBase.Properties.Appearance.Options.UseFont = true;
            this.txtDataBase.Size = new System.Drawing.Size(183, 20);
            this.txtDataBase.TabIndex = 13;
            // 
            // txtServerName
            // 
            this.txtServerName.Location = new System.Drawing.Point(106, 52);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.txtServerName.Properties.Appearance.Options.UseFont = true;
            this.txtServerName.Size = new System.Drawing.Size(183, 20);
            this.txtServerName.TabIndex = 12;
            // 
            // frmConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblSQLConnection);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.lblDataBase);
            this.Controls.Add(this.lblServerName);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.txtDataBase);
            this.Controls.Add(this.txtServerName);
            this.Name = "frmConnection";
            this.Text = "frmConnection";
            this.Load += new System.EventHandler(this.frmConnection_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDataBase.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServerName.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblError;
        internal DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.LabelControl lblSQLConnection;
        internal DevExpress.XtraEditors.SimpleButton btnOK;
        internal DevExpress.XtraEditors.LabelControl lblPassword;
        internal DevExpress.XtraEditors.LabelControl lblUserName;
        internal DevExpress.XtraEditors.LabelControl lblDataBase;
        internal DevExpress.XtraEditors.LabelControl lblServerName;
        private DevExpress.XtraEditors.TextEdit txtPassword;
        private DevExpress.XtraEditors.TextEdit txtUserName;
        private DevExpress.XtraEditors.TextEdit txtDataBase;
        private DevExpress.XtraEditors.TextEdit txtServerName;
    }
}