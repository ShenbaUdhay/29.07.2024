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

Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports DevExpress.Utils
Imports DevExpress.XtraReports.Design
Imports DevExpress.XtraReports.Native

Namespace RepWizardCustomQuery
    Partial Public Class WizPageQuery
        Inherits DevExpress.Utils.InteriorWizardPage
        Private standardWizard As XtraReportWizardBase
        Private da As IDbDataAdapter
        Private ds As DataSet

        Private Shared Function CreateDataAdapter(ByVal connectionString As String, ByVal selectQuery As String) As IDbDataAdapter
            Dim result As IDbDataAdapter

            If ConnectionStringHelper.GetConnectionType(connectionString) = ConnectionType.Sql Then
                connectionString = ConnectionStringHelper.RemoveProviderFromConnectionString(connectionString)
                Dim sqlSelectCommand As New SqlCommand(selectQuery, New SqlConnection(connectionString))
                result = New SqlDataAdapter(sqlSelectCommand)
            Else
                Dim selectCommand As New OleDbCommand(selectQuery, New OleDbConnection(connectionString))
                result = New OleDbDataAdapter(selectCommand)
            End If

            result.TableMappings.Add("Table", "Table")

            Return result
        End Function

        Public Sub New(ByVal runner As XRWizardRunnerBase)
            ' standardWizard = DirectCast(runner.Wizard, NewXtraReportStandardBuilderWizard)
            standardWizard = DirectCast(runner.Wizard, NewStandardReportWizard)
            ds = New DataSet()


            InitializeComponent()

            If Not strDataSourceQuery Is Nothing AndAlso Len(strDataSourceQuery) > 0 Then
                Me.Query_memoEdit.Text = strDataSourceQuery
            End If
        End Sub

        Private Sub Test_simpleButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Test_simpleButton.Click
            ' da = CreateDataAdapter(DirectCast(Me.standardWizard, NewXtraReportStandardBuilderWizard).ConnectionString, Me.Query_memoEdit.Text)
            da = CreateDataAdapter(DirectCast(Me.standardWizard, NewStandardReportWizard).ConnectionString, Me.Query_memoEdit.Text)
            ds.Reset()

            Try
                da.Fill(ds)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
                Return
            End Try

            Me.dataGridView1.Columns.Clear()
            Me.dataGridView1.DataSource = ds.Tables(0)
        End Sub

        Protected Overrides Function OnSetActive() As Boolean
            WizardHelper.LastDataSourceWizardType = "WizPageQuery"

            Wizard.WizardButtons = WizardButton.Back Or WizardButton.[Next] Or WizardButton.DisabledFinish

            Return True
        End Function

        Protected Overrides Function OnWizardBack() As String
            Return "WizPageDataOption"
        End Function

        Protected Overrides Function OnWizardNext() As String
            'Dim reportWizard As NewXtraReportStandardBuilderWizard = DirectCast(Me.standardWizard, NewXtraReportStandardBuilderWizard)
            Dim reportWizard As NewStandardReportWizard = DirectCast(Me.standardWizard, NewStandardReportWizard)
            ds.DataSetName = reportWizard.DatasetName
            reportWizard.Dataset = ds
            reportWizard.DataAdapters.Clear()
            reportWizard.DataAdapters.Add(da)

            strDataSourceQuery = Me.Query_memoEdit.Text

            Return "WizPageChooseFields"
        End Function

        Private Sub WizPageQuery_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                If strSqlCommandText.Length > 0 Then
                    Me.Query_memoEdit.EditValue = strSqlCommandText
                End If
            Catch ex As Exception

            End Try
        End Sub
    End Class
End Namespace