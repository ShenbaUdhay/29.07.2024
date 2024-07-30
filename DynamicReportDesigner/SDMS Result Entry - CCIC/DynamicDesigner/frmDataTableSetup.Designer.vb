<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDataTableSetup
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
        Me.PanelControl3 = New DevExpress.XtraEditors.PanelControl
        Me.btnExit = New DevExpress.XtraEditors.SimpleButton
        Me.btnCancel = New DevExpress.XtraEditors.SimpleButton
        Me.ListBoxControl1 = New DevExpress.XtraEditors.ListBoxControl
        Me.SimpleButton2 = New DevExpress.XtraEditors.SimpleButton
        Me.SimpleButton1 = New DevExpress.XtraEditors.SimpleButton
        Me.SplitterControl1 = New DevExpress.XtraEditors.SplitterControl
        Me.GridControl1 = New DevExpress.XtraGrid.GridControl
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl1.SuspendLayout()
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl2.SuspendLayout()
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelControl3.SuspendLayout()
        CType(Me.ListBoxControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelControl1
        '
        Me.PanelControl1.Controls.Add(Me.PanelControl2)
        Me.PanelControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControl1.Location = New System.Drawing.Point(0, 0)
        Me.PanelControl1.Name = "PanelControl1"
        Me.PanelControl1.Size = New System.Drawing.Size(764, 671)
        Me.PanelControl1.TabIndex = 0
        '
        'PanelControl2
        '
        Me.PanelControl2.Controls.Add(Me.PanelControl3)
        Me.PanelControl2.Controls.Add(Me.SplitterControl1)
        Me.PanelControl2.Controls.Add(Me.GridControl1)
        Me.PanelControl2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControl2.Location = New System.Drawing.Point(2, 2)
        Me.PanelControl2.Name = "PanelControl2"
        Me.PanelControl2.Size = New System.Drawing.Size(760, 667)
        Me.PanelControl2.TabIndex = 4
        '
        'PanelControl3
        '
        Me.PanelControl3.Controls.Add(Me.btnExit)
        Me.PanelControl3.Controls.Add(Me.btnCancel)
        Me.PanelControl3.Controls.Add(Me.ListBoxControl1)
        Me.PanelControl3.Controls.Add(Me.SimpleButton2)
        Me.PanelControl3.Controls.Add(Me.SimpleButton1)
        Me.PanelControl3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelControl3.Location = New System.Drawing.Point(418, 2)
        Me.PanelControl3.Name = "PanelControl3"
        Me.PanelControl3.Size = New System.Drawing.Size(340, 663)
        Me.PanelControl3.TabIndex = 5
        '
        'btnExit
        '
        Me.btnExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnExit.Location = New System.Drawing.Point(253, 632)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(75, 23)
        Me.btnExit.TabIndex = 5
        Me.btnExit.Text = "Exit"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(172, 632)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "Cancel"
        '
        'ListBoxControl1
        '
        Me.ListBoxControl1.Items.AddRange(New Object() {"PageIndex"})
        Me.ListBoxControl1.Location = New System.Drawing.Point(119, 25)
        Me.ListBoxControl1.Name = "ListBoxControl1"
        Me.ListBoxControl1.Size = New System.Drawing.Size(189, 589)
        Me.ListBoxControl1.TabIndex = 1
        '
        'SimpleButton2
        '
        Me.SimpleButton2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleButton2.Location = New System.Drawing.Point(92, 632)
        Me.SimpleButton2.Name = "SimpleButton2"
        Me.SimpleButton2.Size = New System.Drawing.Size(74, 23)
        Me.SimpleButton2.TabIndex = 3
        Me.SimpleButton2.Text = "OK"
        '
        'SimpleButton1
        '
        Me.SimpleButton1.Location = New System.Drawing.Point(16, 263)
        Me.SimpleButton1.Name = "SimpleButton1"
        Me.SimpleButton1.Size = New System.Drawing.Size(83, 23)
        Me.SimpleButton1.TabIndex = 2
        Me.SimpleButton1.Text = "Transfer"
        '
        'SplitterControl1
        '
        Me.SplitterControl1.Location = New System.Drawing.Point(413, 2)
        Me.SplitterControl1.Name = "SplitterControl1"
        Me.SplitterControl1.Size = New System.Drawing.Size(5, 663)
        Me.SplitterControl1.TabIndex = 2
        Me.SplitterControl1.TabStop = False
        '
        'GridControl1
        '
        Me.GridControl1.Dock = System.Windows.Forms.DockStyle.Left
        Me.GridControl1.EmbeddedNavigator.Buttons.Append.Visible = False
        Me.GridControl1.EmbeddedNavigator.Buttons.CancelEdit.Visible = False
        Me.GridControl1.EmbeddedNavigator.Buttons.Edit.Visible = False
        Me.GridControl1.EmbeddedNavigator.Buttons.EndEdit.Visible = False
        Me.GridControl1.EmbeddedNavigator.Buttons.Remove.Visible = False
        Me.GridControl1.Location = New System.Drawing.Point(2, 2)
        Me.GridControl1.MainView = Me.GridView1
        Me.GridControl1.Name = "GridControl1"
        Me.GridControl1.Size = New System.Drawing.Size(411, 663)
        Me.GridControl1.TabIndex = 1
        Me.GridControl1.UseEmbeddedNavigator = True
        Me.GridControl1.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.GridControl = Me.GridControl1
        Me.GridView1.Name = "GridView1"
        Me.GridView1.OptionsView.ShowGroupPanel = False
        '
        'frmDataTableSetup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(764, 671)
        Me.Controls.Add(Me.PanelControl1)
        Me.Name = "frmDataTableSetup"
        Me.Text = "frmDataTableSetup"
        CType(Me.PanelControl1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl1.ResumeLayout(False)
        CType(Me.PanelControl2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl2.ResumeLayout(False)
        CType(Me.PanelControl3, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelControl3.ResumeLayout(False)
        CType(Me.ListBoxControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridControl1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PanelControl1 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents ListBoxControl1 As DevExpress.XtraEditors.ListBoxControl
    Friend WithEvents SimpleButton1 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents SimpleButton2 As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents PanelControl2 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents SplitterControl1 As DevExpress.XtraEditors.SplitterControl
    Friend WithEvents GridControl1 As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents PanelControl3 As DevExpress.XtraEditors.PanelControl
    Friend WithEvents btnExit As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents btnCancel As DevExpress.XtraEditors.SimpleButton
End Class
