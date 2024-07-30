<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmReportNameDetails
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
        Me.cmdOK = New DevExpress.XtraEditors.SimpleButton
        Me.txtReportName = New DevExpress.XtraEditors.TextEdit
        Me.LabelControl1 = New DevExpress.XtraEditors.LabelControl
        CType(Me.txtReportName.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.Location = New System.Drawing.Point(197, 41)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(75, 23)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Text = "OK"
        '
        'txtReportName
        '
        Me.txtReportName.Location = New System.Drawing.Point(78, 13)
        Me.txtReportName.Name = "txtReportName"
        Me.txtReportName.Size = New System.Drawing.Size(194, 20)
        Me.txtReportName.TabIndex = 1
        '
        'LabelControl1
        '
        Me.LabelControl1.Location = New System.Drawing.Point(12, 16)
        Me.LabelControl1.Name = "LabelControl1"
        Me.LabelControl1.Size = New System.Drawing.Size(60, 13)
        Me.LabelControl1.TabIndex = 2
        Me.LabelControl1.Text = "ReportName"
        '
        'frmReportNameDetails
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 76)
        Me.Controls.Add(Me.LabelControl1)
        Me.Controls.Add(Me.txtReportName)
        Me.Controls.Add(Me.cmdOK)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmReportNameDetails"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ReportNameDetails"
        CType(Me.txtReportName.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdOK As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents txtReportName As DevExpress.XtraEditors.TextEdit
    Friend WithEvents LabelControl1 As DevExpress.XtraEditors.LabelControl
End Class
