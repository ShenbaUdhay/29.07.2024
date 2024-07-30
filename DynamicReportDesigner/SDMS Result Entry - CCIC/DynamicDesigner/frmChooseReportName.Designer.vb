<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChooseReportName
    Inherits DevExpress.XtraEditors.XtraForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.PanelControl1 = New DevExpress.XtraEditors.PanelControl
        Me.PanelControl2 = New DevExpress.XtraEditors.PanelControl
        Me.cmdOk = New DevExpress.XtraEditors.SimpleButton
        Me.GroupControl1 = New DevExpress.XtraEditors.GroupControl
        Me.chkReportName = New DevExpress.XtraEditors.CheckedListBoxControl
        Me.cmdDelete = New DevExpress.XtraEditors.SimpleButton
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl2.SuspendLayout()
        CType(Me.GroupControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupControl1.SuspendLayout()
        CType(Me.chkReportName, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelControl1
        '
        Me.PanelControl1.Controls.Add(Me.PanelControl2)
        Me.PanelControl1.Controls.Add(Me.GroupControl1)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(252, 440)
        Me.PanelControl1.TabIndex = 0
        '
        'PanelControl2
        '
        Me.PanelControl2.Controls.Add(Me.cmdDelete)
        Me.PanelControl2.Controls.Add(Me.cmdOk)
        Me.PanelControl2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControl2.Location = New System.Drawing.Point(2, 2)
        Me.PanelControl2.Name = "PanelControl2"
        Me.PanelControl2.Size = New System.Drawing.Size(248, 44)
        Me.PanelControl2.TabIndex = 2
        '
        'cmdOk
        '
        Me.cmdOk.Location = New System.Drawing.Point(163, 11)
        Me.cmdOk.Name = "cmdOk"
        Me.cmdOk.Size = New System.Drawing.Size(75, 23)
        Me.cmdOk.TabIndex = 0
        Me.cmdOk.Text = "OK"
        '
        'GroupControl1
        '
        Me.GroupControl1.Controls.Add(Me.chkReportName)
        Me.GroupControl1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.GroupControl1.Location = New System.Drawing.Point(2, 46)
        Me.GroupControl1.Name = "GroupControl1"
        Me.GroupControl1.Size = New System.Drawing.Size(248, 392)
        Me.GroupControl1.TabIndex = 1
        Me.GroupControl1.Text = "Available Reports"
        '
        'chkReportName
        '
        Me.chkReportName.CheckOnClick = True
        Me.chkReportName.Dock = System.Windows.Forms.DockStyle.Fill
        Me.chkReportName.Location = New System.Drawing.Point(2, 22)
        Me.chkReportName.Name = "chkReportName"
        Me.chkReportName.Size = New System.Drawing.Size(244, 368)
        Me.chkReportName.TabIndex = 0
        '
        'cmdDelete
        '
        Me.cmdDelete.Location = New System.Drawing.Point(10, 11)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.Size = New System.Drawing.Size(75, 23)
        Me.cmdDelete.TabIndex = 1
        Me.cmdDelete.Text = "Delete"
        '
        'frmChooseReportName
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(252, 440)
        Me.Controls.Add(Me.PanelControl1)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmChooseReportName"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ChooseReportName"
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl2.ResumeLayout(False)
        CType(Me.GroupControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupControl1.ResumeLayout(False)
        CType(Me.chkReportName, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents chkReportName As DevExpress.XtraEditors.CheckedListBoxControl
    Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents cmdOk As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents GroupControl1 As DevExpress.XtraEditors.GroupControl
    Friend WithEvents cmdDelete As DevExpress.XtraEditors.SimpleButton
End Class
