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

Imports DevExpress.Utils
Imports DevExpress.XtraReports.Design
Imports DynamicReportBusinessLayer

Namespace RepWizardCustomQuery
    Partial Public Class WizPageReportNameDetails
        Inherits DevExpress.Utils.InteriorWizardPage
        Private standardWizard As XtraReportWizardBase

        Public Sub New(ByVal runner As XRWizardRunnerBase)
            Me.standardWizard = runner.Wizard
            InitializeComponent()
        End Sub

        Protected Overrides Function OnSetActive() As Boolean
            Wizard.WizardButtons = WizardButton.Back Or WizardButton.[Next] Or WizardButton.DisabledFinish
            Return True
        End Function

        Protected Overrides Function OnWizardNext() As String
            If Len(TextBox1.Text) > 0 Then
                Dim strString As String = "Select colCustomReportDesignerName from tbl_Public_CustomReportDesignerDetails Where colCustomReportDesignerName = N'" & TextBox1.Text & "'"
                Dim dtTable As DataTable = BLCommon.GetData(strString)
                If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then
                    MsgBox("ReportName already exists", MsgBoxStyle.Information, "BTLIMS")
                Else
                    strReportName = TextBox1.Text
                    Return "WizPageDataOption"

                End If
            Else
                strReportName = String.Empty
            End If
            Return "WizPageReportNameDetails"
        End Function

    End Class
End Namespace