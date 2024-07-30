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
    Partial Class WizPageDataOption
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
            Me.radioGroup1 = New DevExpress.XtraEditors.RadioGroup
            CType(Me.headerPicture, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.radioGroup1.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'titleLabel
            '
            Me.titleLabel.Location = New System.Drawing.Point(15, 8)
            Me.titleLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
            Me.titleLabel.Size = New System.Drawing.Size(308, 18)
            Me.titleLabel.Text = "Choose Data Source"
            '
            'subtitleLabel
            '
            Me.subtitleLabel.Location = New System.Drawing.Point(114, 120)
            Me.subtitleLabel.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
            Me.subtitleLabel.Size = New System.Drawing.Size(284, 15)
            Me.subtitleLabel.Text = "Select Query or Table/View"
            '
            'headerPanel
            '
            Me.headerPanel.Margin = New System.Windows.Forms.Padding(2)
            '
            'headerPicture
            '
            Me.headerPicture.Location = New System.Drawing.Point(443, 4)
            Me.headerPicture.Margin = New System.Windows.Forms.Padding(2)
            Me.headerPicture.Size = New System.Drawing.Size(37, 40)
            '
            'headerSeparator
            '
            Me.headerSeparator.Location = New System.Drawing.Point(0, 54)
            Me.headerSeparator.Margin = New System.Windows.Forms.Padding(2)
            Me.headerSeparator.Padding = New System.Windows.Forms.Padding(2)
            Me.headerSeparator.Size = New System.Drawing.Size(490, 2)
            '
            'radioGroup1
            '
            Me.radioGroup1.EditValue = "Query"
            Me.radioGroup1.Location = New System.Drawing.Point(156, 146)
            Me.radioGroup1.Margin = New System.Windows.Forms.Padding(2)
            Me.radioGroup1.Name = "radioGroup1"
            Me.radioGroup1.Properties.Items.AddRange(New DevExpress.XtraEditors.Controls.RadioGroupItem() {New DevExpress.XtraEditors.Controls.RadioGroupItem("Query", "Query"), New DevExpress.XtraEditors.Controls.RadioGroupItem("Table", "Table/View")})
            Me.radioGroup1.Size = New System.Drawing.Size(242, 27)
            Me.radioGroup1.TabIndex = 5
            '
            'WizPageDataOption
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.radioGroup1)
            Me.Margin = New System.Windows.Forms.Padding(2)
            Me.Name = "WizPageDataOption"
            Me.Size = New System.Drawing.Size(497, 303)
            Me.Controls.SetChildIndex(Me.radioGroup1, 0)
            Me.Controls.SetChildIndex(Me.subtitleLabel, 0)
            Me.Controls.SetChildIndex(Me.headerPanel, 0)
            Me.Controls.SetChildIndex(Me.headerSeparator, 0)
            Me.Controls.SetChildIndex(Me.titleLabel, 0)
            Me.Controls.SetChildIndex(Me.headerPicture, 0)
            CType(Me.headerPicture, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.radioGroup1.Properties, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

#End Region

        Private radioGroup1 As DevExpress.XtraEditors.RadioGroup
    End Class
End Namespace