' Developer Express Code Central Example:
' How to customize a Report Wizard
' 
' This example demonstrates how to create a Custom Report Wizard with the
' capability to define the SQL query, based on which the resulting report's data
' source will be generated (see the corresponding suggestion:
' http://www.devexpress.com/scid=AS4685). To accomplish this task, override the
' ReportCommand.NewReportWizard, ReportCommand.AddNewDataSource, and
' ReportCommand.VerbReportWizard commands as described in this documentation
' article: How to: Override Commands in the End-User Designer (Custom Saving)
' (ms-help://DevExpress.NETv9.1/DevExpress.XtraReports/CustomDocument2211.htm), in
' order to run you custom wizard in the corresponding handler. A Custom Report
' Wizard inherits from the XRWizardRunnerBase class, custom wizard pages are
' inherited from the InteriorWizardPage class. Note that for most custom wizard
' pages, you should override the InteriorWizardPage.OnWizardBack() and
' InteriorWizardPage.OnWizardNext() virtual methods, to provide proper wizard
' navigation logic.
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=E1538

Namespace RepWizardCustomQuery
    Partial Class WizPageQuery
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.IContainer = Nothing

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso (components IsNot Nothing) Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

#Region "Windows Form Designer generated code"

        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.Query_memoEdit = New DevExpress.XtraEditors.MemoEdit
            Me.Test_simpleButton = New DevExpress.XtraEditors.SimpleButton
            Me.dataGridView1 = New System.Windows.Forms.DataGridView
            CType(Me.headerPicture, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Query_memoEdit.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.dataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'titleLabel
            '
            Me.titleLabel.Location = New System.Drawing.Point(15, 8)
            Me.titleLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
            Me.titleLabel.Size = New System.Drawing.Size(308, 11)
            Me.titleLabel.Text = "Query Setup"
            '
            'subtitleLabel
            '
            Me.subtitleLabel.Location = New System.Drawing.Point(31, 20)
            Me.subtitleLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
            Me.subtitleLabel.Size = New System.Drawing.Size(292, 21)
            Me.subtitleLabel.Text = "Enter your database query"
            '
            'headerPanel
            '
            Me.headerPanel.Margin = New System.Windows.Forms.Padding(2)
            Me.headerPanel.Size = New System.Drawing.Size(497, 47)
            '
            'headerPicture
            '
            Me.headerPicture.Location = New System.Drawing.Point(456, 4)
            Me.headerPicture.Margin = New System.Windows.Forms.Padding(2)
            Me.headerPicture.Size = New System.Drawing.Size(37, 40)
            '
            'headerSeparator
            '
            Me.headerSeparator.Location = New System.Drawing.Point(0, 47)
            Me.headerSeparator.Margin = New System.Windows.Forms.Padding(2)
            Me.headerSeparator.Padding = New System.Windows.Forms.Padding(2)
            Me.headerSeparator.Size = New System.Drawing.Size(492, 2)
            '
            'Query_memoEdit
            '
            Me.Query_memoEdit.EditValue = "select * from Select_Public_Denormalize_View"
            Me.Query_memoEdit.Location = New System.Drawing.Point(12, 54)
            Me.Query_memoEdit.Margin = New System.Windows.Forms.Padding(2)
            Me.Query_memoEdit.Name = "Query_memoEdit"
            Me.Query_memoEdit.Size = New System.Drawing.Size(471, 50)
            Me.Query_memoEdit.TabIndex = 5
            '
            'Test_simpleButton
            '
            Me.Test_simpleButton.Location = New System.Drawing.Point(408, 108)
            Me.Test_simpleButton.Margin = New System.Windows.Forms.Padding(2)
            Me.Test_simpleButton.Name = "Test_simpleButton"
            Me.Test_simpleButton.Size = New System.Drawing.Size(75, 25)
            Me.Test_simpleButton.TabIndex = 6
            Me.Test_simpleButton.Text = "&Apply"
            '
            'dataGridView1
            '
            Me.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dataGridView1.Location = New System.Drawing.Point(3, 138)
            Me.dataGridView1.Name = "dataGridView1"
            Me.dataGridView1.Size = New System.Drawing.Size(492, 162)
            Me.dataGridView1.TabIndex = 7
            '
            'WizPageQuery
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.Query_memoEdit)
            Me.Controls.Add(Me.dataGridView1)
            Me.Controls.Add(Me.Test_simpleButton)
            Me.Margin = New System.Windows.Forms.Padding(2)
            Me.Name = "WizPageQuery"
            Me.Size = New System.Drawing.Size(497, 303)
            Me.Controls.SetChildIndex(Me.Test_simpleButton, 0)
            Me.Controls.SetChildIndex(Me.dataGridView1, 0)
            Me.Controls.SetChildIndex(Me.Query_memoEdit, 0)
            Me.Controls.SetChildIndex(Me.headerPanel, 0)
            Me.Controls.SetChildIndex(Me.headerSeparator, 0)
            Me.Controls.SetChildIndex(Me.titleLabel, 0)
            Me.Controls.SetChildIndex(Me.subtitleLabel, 0)
            Me.Controls.SetChildIndex(Me.headerPicture, 0)
            CType(Me.headerPicture, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Query_memoEdit.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.dataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private Query_memoEdit As DevExpress.XtraEditors.MemoEdit
        Private WithEvents Test_simpleButton As DevExpress.XtraEditors.SimpleButton
        Private dataGridView1 As System.Windows.Forms.DataGridView
    End Class
End Namespace