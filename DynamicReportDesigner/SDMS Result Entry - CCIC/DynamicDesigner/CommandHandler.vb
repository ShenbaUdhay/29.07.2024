Imports System.IO
Imports System.Text
Imports DevExpress.XtraReports.Design
Imports DevExpress.XtraReports.UI
Imports DevExpress.XtraReports.UserDesigner
Imports DynamicReportBusinessLayer

Public Class CommandHandler
    Implements DevExpress.XtraReports.UserDesigner.ICommandHandler

    Private panel As XRDesignPanel
    Private _canHandleCommand As Boolean

    Public Sub New(ByVal panel1 As XRDesignPanel)
        Me.panel = panel1
    End Sub

    Public Function CanHandleCommand(ByVal command As ReportCommand, ByRef useNextHandler As Boolean) As Boolean Implements ICommandHandler.CanHandleCommand
        'Return command = ReportCommand.NewReport OrElse command = ReportCommand.NewReportWizard OrElse command = ReportCommand.SaveFile OrElse command = ReportCommand.OpenFile OrElse command = ReportCommand.AddNewDataSource OrElse command = ReportCommand.Closing OrElse command = ReportCommand.Close OrElse command = ReportCommand.Exit OrElse command = ReportCommand.SaveFileAs ' this code is not worked in the new upgraded version for Devexpress 14.1.6. so that only i commanded this one and add a new one.
        useNextHandler = Not (command = ReportCommand.NewReport OrElse command = ReportCommand.NewReportWizard OrElse command = ReportCommand.SaveFile OrElse command = ReportCommand.OpenFile OrElse command = ReportCommand.AddNewDataSource OrElse command = ReportCommand.Closing OrElse command = ReportCommand.Close OrElse command = ReportCommand.Exit OrElse command = ReportCommand.SaveFileAs)
        Return Not useNextHandler ' in this new upgrade version for Devexpress 14.1.6 we faced the problem to call this function,by refered the below link i took this new code and fixed the bug.
        ''https://www.devexpress.com/Support/Center/Question/Details/Q448949
    End Function
    'Public Function CanHandleCommand(ByVal command As DevExpress.XtraReports.UserDesigner.ReportCommand) As Boolean Implements DevExpress.XtraReports.UserDesigner.ICommandHandler.CanHandleCommand
    '    Return command = ReportCommand.NewReport OrElse command = ReportCommand.NewReportWizard OrElse command = ReportCommand.SaveFile OrElse command = ReportCommand.OpenFile OrElse command = ReportCommand.AddNewDataSource OrElse command = ReportCommand.Closing OrElse command = ReportCommand.Close OrElse command = ReportCommand.Exit OrElse command = ReportCommand.SaveFileAs
    'End Function

    '  Public Sub HandleCommand(ByVal command As ReportCommand, _
    'ByVal args() As Object) Implements ICommandHandler.HandleCommand
    '      MessageBox.Show("Cannot delete label!")
    '  End Sub
    Public Sub HandleCommand(ByVal command As DevExpress.XtraReports.UserDesigner.ReportCommand, ByVal args() As Object) Implements DevExpress.XtraReports.UserDesigner.ICommandHandler.HandleCommand

        If command = ReportCommand.SaveFile OrElse command = ReportCommand.Closing OrElse command = ReportCommand.Close OrElse command = ReportCommand.Exit Then
            If Not panel.Report Is Nothing Then
                If Not strReportName Is Nothing AndAlso Len(strReportName) > 0 Then
                    Dim strString As String = "Select colCustomReportDesignerLayOut from tbl_Public_CustomReportDesignerDetails Where colCustomReportDesignerName = N'" & strReportName & "'"
                    Dim dtTable As DataTable = BLCommon.GetData(strString)
                    If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 AndAlso Not IsDBNull(dtTable.Rows(0)("colCustomReportDesignerLayOut")) AndAlso Len(dtTable.Rows(0)("colCustomReportDesignerLayOut")) > 0 Then
                        Dim myAdapter As System.Data.SqlClient.SqlDataAdapter = panel.Report.DataAdapter
                        If Not myAdapter Is Nothing Then
                            myAdapter.SelectCommand.Connection.ConnectionString = ""
                        End If
                        'sqladap.SelectCommand.Connection.ConnectionString = GetSQLConnection()
                        Dim strUpdateString As String = String.Empty
                        If Not strTableColumnSchema Is Nothing AndAlso Len(strTableColumnSchema) > 0 Then
                            strUpdateString = "Update tbl_Public_CustomReportDesignerDetails set colCustomReportDesignerLayOut = N'" & Replace(Replace(Replace(GetReportLayout(panel.Report), "'", "''"), GetSQLConnectionReportUPdate, "SQLCONNECTIONSTRING"), GetSQLConnectionWithoutPasswordReportUpdate, "SQLCONNECTIONSTRING") & "',colTableSchema = N'" & Replace(strTableColumnSchema, "''", "''''") & "',colNormalColumn = N'" & strNormalColumn & "',colRowColumn = N'" & strRowColumn & "',colUniqueColumn = N'" & strUniqueColumn & "',colVerticalColumnCount=N'" & strColumnCount & " ' Where colCustomReportDesignerName = N'" & strReportName & "'"
                            '   strUpdateString = "Update tbl_Public_CustomReportDesignerDetails set colCustomReportDesignerLayOut = N'" & Replace(Replace(Replace(GetReportLayout(panel.Report), "'", "''"), GetSQLConnection, "SQLCONNECTIONSTRING"), GetSQLConnectionWithOutPassword, "SQLCONNECTIONSTRING") & "',colTableSchema = N'" & Replace(strTableColumnSchema, "''", "''''") & "',colNormalColumn = N'" & strNormalColumn & "',colRowColumn = N'" & strRowColumn & "',colUniqueColumn = N'" & strUniqueColumn & "' Where colCustomReportDesignerName = N'" & strReportName & "'"
                        Else
                            strUpdateString = "Update tbl_Public_CustomReportDesignerDetails set colCustomReportDesignerLayOut = N'" & Replace(Replace(Replace(GetReportLayout(panel.Report), "'", "''"), GetSQLConnectionReportUPdate, "SQLCONNECTIONSTRING"), GetSQLConnectionWithoutPasswordReportUpdate, "SQLCONNECTIONSTRING") & "' Where colCustomReportDesignerName = N'" & strReportName & "'"
                            '  strUpdateString = "Update tbl_Public_CustomReportDesignerDetails set colCustomReportDesignerLayOut = N'" & Replace(Replace(Replace(GetReportLayout(panel.Report), "'", "''"), GetSQLConnection, "SQLCONNECTIONSTRING"), GetSQLConnectionWithOutPassword, "SQLCONNECTIONSTRING") & "' Where colCustomReportDesignerName = N'" & strReportName & "'"
                        End If
                        ExecuteNonQuery(strUpdateString)
                    Else
                        Dim myAdapter As System.Data.SqlClient.SqlDataAdapter = panel.Report.DataAdapter
                        If Not myAdapter Is Nothing Then
                            myAdapter.SelectCommand.Connection.ConnectionString = ""
                        End If
                        'sqladap.SelectCommand.Connection.ConnectionString = GetSQLConnection()
                        Dim strSaveString As String = String.Empty
                        If Not strTableColumnSchema Is Nothing AndAlso Len(strTableColumnSchema) > 0 Then
                            strSaveString = "Insert into tbl_Public_CustomReportDesignerDetails (colCustomReportDesignerName,colCustomReportDesignerLayOut,colTableSchema,colNormalColumn,colRowColumn,colUniqueColumn,colVerticalColumnCount) Values (N'" & strReportName & "',N'" & Replace(Replace(Replace(GetReportLayout(panel.Report), "'", "''"), GetSQLConnection, "SQLCONNECTIONSTRING"), GetSQLConnectionWithOutPassword, "SQLCONNECTIONSTRING") & "',N'" & Replace(strTableColumnSchema, "''", "''''") & "',N'" & strNormalColumn & "',N'" & strRowColumn & "',N'" & strUniqueColumn & "',N'" & strColumnCount & "')"
                        Else
                            strSaveString = "Insert into tbl_Public_CustomReportDesignerDetails (colCustomReportDesignerName,colCustomReportDesignerLayOut) Values (N'" & strReportName & "',N'" & Replace(Replace(Replace(GetReportLayout(panel.Report), "'", "''"), GetSQLConnection, "SQLCONNECTIONSTRING"), GetSQLConnectionWithOutPassword, "SQLCONNECTIONSTRING") & "')"
                        End If
                        ExecuteNonQuery(strSaveString)
                    End If
                Else

                    Dim objForm As New frmReportNameDetails
                    objForm.ShowDialog()
                    If Not strReportName Is Nothing AndAlso Len(strReportName) > 0 Then
                        Dim myAdapter As System.Data.SqlClient.SqlDataAdapter = panel.Report.DataAdapter
                        If Not myAdapter Is Nothing Then
                            myAdapter.SelectCommand.Connection.ConnectionString = ""
                        End If
                        Dim strSaveString As String = String.Empty
                        If Not strTableColumnSchema Is Nothing AndAlso Len(strTableColumnSchema) > 0 Then
                            strSaveString = "Insert into tbl_Public_CustomReportDesignerDetails (colCustomReportDesignerName,colCustomReportDesignerLayOut,colTableSchema,colNormalColumn,colRowColumn,colUniqueColumn,colVerticalColumnCount) Values (N'" & strReportName & "',N'" & Replace(Replace(Replace(GetReportLayout(panel.Report), "'", "''"), GetSQLConnection, "SQLCONNECTIONSTRING"), GetSQLConnectionWithOutPassword, "SQLCONNECTIONSTRING") & "',N'" & Replace(strTableColumnSchema, "''", "''''") & "',N'" & strNormalColumn & "',N'" & strRowColumn & "',N'" & strUniqueColumn & "',N'" & strColumnCount & "')"
                        Else
                            strSaveString = "Insert into tbl_Public_CustomReportDesignerDetails (colCustomReportDesignerName,colCustomReportDesignerLayOut) Values (N'" & strReportName & "',N'" & Replace(Replace(Replace(GetReportLayout(panel.Report), "'", "''"), GetSQLConnection, "SQLCONNECTIONSTRING"), GetSQLConnectionWithOutPassword, "SQLCONNECTIONSTRING") & "')"
                        End If
                        ExecuteNonQuery(strSaveString)
                    End If
                End If
            End If
        End If

        If command = ReportCommand.SaveFileAs Then
            Dim objForm As New frmReportNameDetails
            objForm.ShowDialog()
            If Not strReportName Is Nothing AndAlso Len(strReportName) > 0 Then
                Dim myAdapter As System.Data.SqlClient.SqlDataAdapter = panel.Report.DataAdapter
                If Not myAdapter Is Nothing Then
                    myAdapter.SelectCommand.Connection.ConnectionString = ""
                End If
                Dim strSaveString As String = String.Empty
                If Not strTableColumnSchema Is Nothing AndAlso Len(strTableColumnSchema) > 0 Then
                    strSaveString = "Insert into tbl_Public_CustomReportDesignerDetails (colCustomReportDesignerName,colCustomReportDesignerLayOut,colTableSchema,colNormalColumn,colRowColumn,colUniqueColumn,colVerticalColumnCount) Values (N'" & strReportName & "',N'" & Replace(Replace(Replace(GetReportLayout(panel.Report), "'", "''"), GetSQLConnection, "SQLCONNECTIONSTRING"), GetSQLConnectionWithOutPassword, "SQLCONNECTIONSTRING") & "',N'" & Replace(strTableColumnSchema, "''", "''''") & "',N'" & strNormalColumn & "',N'" & strRowColumn & "',N'" & strUniqueColumn & "',N'" & strColumnCount & "' )"
                Else
                    strSaveString = "Insert into tbl_Public_CustomReportDesignerDetails (colCustomReportDesignerName,colCustomReportDesignerLayOut) Values (N'" & strReportName & "',N'" & Replace(Replace(Replace(GetReportLayout(panel.Report), "'", "''"), GetSQLConnection, "SQLCONNECTIONSTRING"), GetSQLConnectionWithOutPassword, "SQLCONNECTIONSTRING") & "')"
                End If
                ExecuteNonQuery(strSaveString)
            End If
        End If

        If command = ReportCommand.OpenFile Then
            Dim objForm As New frmChooseReportName
            objForm.ShowDialog()
            If Not strReportName Is Nothing AndAlso Len(strReportName) > 0 Then
                'Dim strString As String = "Select colCustomReportDesignerLayOut,colTableSchema,colNormalColumn,colRowColumn,colUniqueColumn from tbl_Public_CustomReportDesignerDetails Where colCustomReportDesignerName = N'" & strReportName & "'"
                'Dim dtTable As DataTable = BLCommon.GetData(strString)
                'If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 AndAlso Not IsDBNull(dtTable.Rows(0)("colCustomReportDesignerLayOut")) AndAlso Len(dtTable.Rows(0)("colCustomReportDesignerLayOut")) > 0 Then
                '    'If Not IsDBNull(dtTable.Rows(0)("colTableSchema")) AndAlso Len(dtTable.Rows(0)("colTableSchema")) > 0 Then
                '    '    strTableColumnSchema = dtTable.Rows(0)("colTableSchema")
                '    '    dtTableColumn = BLCommon.GetData(strTableColumnSchema)
                '    'Else
                '    '    strTableColumnSchema = String.Empty
                '    'End If
                '    'If Not IsDBNull(dtTable.Rows(0)("colNormalColumn")) AndAlso Len(dtTable.Rows(0)("colNormalColumn")) > 0 Then
                '    '    strNormalColumn = dtTable.Rows(0)("colNormalColumn")
                '    'Else
                '    '    strNormalColumn = String.Empty
                '    'End If
                '    'If Not IsDBNull(dtTable.Rows(0)("colRowColumn")) AndAlso Len(dtTable.Rows(0)("colRowColumn")) > 0 Then
                '    '    strRowColumn = dtTable.Rows(0)("colRowColumn")
                '    'Else
                '    '    strRowColumn = String.Empty
                '    'End If
                '    'If Not IsDBNull(dtTable.Rows(0)("colUniqueColumn")) AndAlso Len(dtTable.Rows(0)("colUniqueColumn")) > 0 Then
                '    '    strUniqueColumn = dtTable.Rows(0)("colUniqueColumn")
                '    'Else
                '    '    strUniqueColumn = String.Empty
                '    'End If
                '    'panel.CloseReport()
                '    'Dim xtraReport As XtraReport = GetReport(Replace(dtTable.Rows(0)("colCustomReportDesignerLayOut"), "SQLCONNECTIONSTRING", GetSQLConnection))
                '    ''xtraReport.Parameters(0).Value = "H1009010"
                '    ''xtraReport.RequestParameters = False
                '    'panel.OpenReport(GetReportFromLayOut(strReportName))
                'End If
                Dim xtraReport As XtraReport = GetReportFromLayOut(strReportName)
                panel.OpenReport(xtraReport)
            End If
        End If

        If command = ReportCommand.AddNewDataSource OrElse (Len(strReportName) = 0 AndAlso command = ReportCommand.NewReport) OrElse (Len(strReportName) = 0 AndAlso command = ReportCommand.NewReportWizard) Then
            Dim wizard As New CustomWizard(panel.Report, panel)
            wizard.Run(command)
        End If

        If command = ReportCommand.ShowPreviewTab Then

            'Dim xtrReport As XtraReport = panel.Report

            'Dim obj As New ReportSettingsClass1
            'xtrReport = obj.GenerateRowsasColumn(xtrReport)

        End If

        'handled = True
    End Sub


    'Public Sub HandleCommand(ByVal command As DevExpress.XtraReports.UserDesigner.ReportCommand, ByVal args() As Object, ByRef handled As Boolean) Implements ICommandHandler.HandleCommand

    '    If command = ReportCommand.SaveFile OrElse command = ReportCommand.Closing OrElse command = ReportCommand.Close OrElse command = ReportCommand.Exit Then
    '        If Not panel.Report Is Nothing Then
    '            If Not strReportName Is Nothing AndAlso Len(strReportName) > 0 Then
    '                Dim strString As String = "Select colCustomReportDesignerLayOut from tbl_Public_CustomReportDesignerDetails Where colCustomReportDesignerName = N'" & strReportName & "'"
    '                Dim dtTable As DataTable = BLCommon.GetData(strString)
    '                If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 AndAlso Not IsDBNull(dtTable.Rows(0)("colCustomReportDesignerLayOut")) AndAlso Len(dtTable.Rows(0)("colCustomReportDesignerLayOut")) > 0 Then
    '                    Dim strUpdateString As String = String.Empty
    '                    If Not strTableColumnSchema Is Nothing AndAlso Len(strTableColumnSchema) > 0 Then
    '                        strUpdateString = "Update tbl_Public_CustomReportDesignerDetails set colCustomReportDesignerLayOut = N'" & Replace(Replace(Replace(GetReportLayout(panel.Report), "'", "''"), GetSQLConnectionReportUPdate, "SQLCONNECTIONSTRING"), GetSQLConnectionWithoutPasswordReportUpdate, "SQLCONNECTIONSTRING") & "',colTableSchema = N'" & Replace(strTableColumnSchema, "''", "''''") & "',colNormalColumn = N'" & strNormalColumn & "',colRowColumn = N'" & strRowColumn & "',colUniqueColumn = N'" & strUniqueColumn & "',colVerticalColumnCount=N'" & strColumnCount & " ' Where colCustomReportDesignerName = N'" & strReportName & "'"
    '                        '   strUpdateString = "Update tbl_Public_CustomReportDesignerDetails set colCustomReportDesignerLayOut = N'" & Replace(Replace(Replace(GetReportLayout(panel.Report), "'", "''"), GetSQLConnection, "SQLCONNECTIONSTRING"), GetSQLConnectionWithOutPassword, "SQLCONNECTIONSTRING") & "',colTableSchema = N'" & Replace(strTableColumnSchema, "''", "''''") & "',colNormalColumn = N'" & strNormalColumn & "',colRowColumn = N'" & strRowColumn & "',colUniqueColumn = N'" & strUniqueColumn & "' Where colCustomReportDesignerName = N'" & strReportName & "'"
    '                    Else
    '                        strUpdateString = "Update tbl_Public_CustomReportDesignerDetails set colCustomReportDesignerLayOut = N'" & Replace(Replace(Replace(GetReportLayout(panel.Report), "'", "''"), GetSQLConnectionReportUPdate, "SQLCONNECTIONSTRING"), GetSQLConnectionWithoutPasswordReportUpdate, "SQLCONNECTIONSTRING") & "' Where colCustomReportDesignerName = N'" & strReportName & "'"
    '                        '  strUpdateString = "Update tbl_Public_CustomReportDesignerDetails set colCustomReportDesignerLayOut = N'" & Replace(Replace(Replace(GetReportLayout(panel.Report), "'", "''"), GetSQLConnection, "SQLCONNECTIONSTRING"), GetSQLConnectionWithOutPassword, "SQLCONNECTIONSTRING") & "' Where colCustomReportDesignerName = N'" & strReportName & "'"
    '                    End If
    '                    ExecuteNonQuery(strUpdateString)
    '                Else
    '                    Dim strSaveString As String = String.Empty
    '                    If Not strTableColumnSchema Is Nothing AndAlso Len(strTableColumnSchema) > 0 Then
    '                        strSaveString = "Insert into tbl_Public_CustomReportDesignerDetails (colCustomReportDesignerName,colCustomReportDesignerLayOut,colTableSchema,colNormalColumn,colRowColumn,colUniqueColumn,colVerticalColumnCount) Values (N'" & strReportName & "',N'" & Replace(Replace(Replace(GetReportLayout(panel.Report), "'", "''"), GetSQLConnection, "SQLCONNECTIONSTRING"), GetSQLConnectionWithOutPassword, "SQLCONNECTIONSTRING") & "',N'" & Replace(strTableColumnSchema, "''", "''''") & "',N'" & strNormalColumn & "',N'" & strRowColumn & "',N'" & strUniqueColumn & "',N'" & strColumnCount & "')"
    '                    Else
    '                        strSaveString = "Insert into tbl_Public_CustomReportDesignerDetails (colCustomReportDesignerName,colCustomReportDesignerLayOut) Values (N'" & strReportName & "',N'" & Replace(Replace(Replace(GetReportLayout(panel.Report), "'", "''"), GetSQLConnection, "SQLCONNECTIONSTRING"), GetSQLConnectionWithOutPassword, "SQLCONNECTIONSTRING") & "')"
    '                    End If
    '                    ExecuteNonQuery(strSaveString)
    '                End If
    '            Else
    '                Dim objForm As New frmReportNameDetails
    '                objForm.ShowDialog()
    '                If Not strReportName Is Nothing AndAlso Len(strReportName) > 0 Then
    '                    Dim strSaveString As String = String.Empty
    '                    If Not strTableColumnSchema Is Nothing AndAlso Len(strTableColumnSchema) > 0 Then
    '                        strSaveString = "Insert into tbl_Public_CustomReportDesignerDetails (colCustomReportDesignerName,colCustomReportDesignerLayOut,colTableSchema,colNormalColumn,colRowColumn,colUniqueColumn,colVerticalColumnCount) Values (N'" & strReportName & "',N'" & Replace(Replace(Replace(GetReportLayout(panel.Report), "'", "''"), GetSQLConnection, "SQLCONNECTIONSTRING"), GetSQLConnectionWithOutPassword, "SQLCONNECTIONSTRING") & "',N'" & Replace(strTableColumnSchema, "''", "''''") & "',N'" & strNormalColumn & "',N'" & strRowColumn & "',N'" & strUniqueColumn & "',N'" & strColumnCount & "')"
    '                    Else
    '                        strSaveString = "Insert into tbl_Public_CustomReportDesignerDetails (colCustomReportDesignerName,colCustomReportDesignerLayOut) Values (N'" & strReportName & "',N'" & Replace(Replace(Replace(GetReportLayout(panel.Report), "'", "''"), GetSQLConnection, "SQLCONNECTIONSTRING"), GetSQLConnectionWithOutPassword, "SQLCONNECTIONSTRING") & "')"
    '                    End If
    '                    ExecuteNonQuery(strSaveString)
    '                End If
    '            End If
    '        End If
    '    End If

    '    If command = ReportCommand.SaveFileAs Then
    '        Dim objForm As New frmReportNameDetails
    '        objForm.ShowDialog()
    '        If Not strReportName Is Nothing AndAlso Len(strReportName) > 0 Then
    '            Dim strSaveString As String = String.Empty
    '            If Not strTableColumnSchema Is Nothing AndAlso Len(strTableColumnSchema) > 0 Then
    '                strSaveString = "Insert into tbl_Public_CustomReportDesignerDetails (colCustomReportDesignerName,colCustomReportDesignerLayOut,colTableSchema,colNormalColumn,colRowColumn,colUniqueColumn,colVerticalColumnCount) Values (N'" & strReportName & "',N'" & Replace(Replace(Replace(GetReportLayout(panel.Report), "'", "''"), GetSQLConnection, "SQLCONNECTIONSTRING"), GetSQLConnectionWithOutPassword, "SQLCONNECTIONSTRING") & "',N'" & Replace(strTableColumnSchema, "''", "''''") & "',N'" & strNormalColumn & "',N'" & strRowColumn & "',N'" & strUniqueColumn & "',N'" & strColumnCount & "' )"
    '            Else
    '                strSaveString = "Insert into tbl_Public_CustomReportDesignerDetails (colCustomReportDesignerName,colCustomReportDesignerLayOut) Values (N'" & strReportName & "',N'" & Replace(Replace(Replace(GetReportLayout(panel.Report), "'", "''"), GetSQLConnection, "SQLCONNECTIONSTRING"), GetSQLConnectionWithOutPassword, "SQLCONNECTIONSTRING") & "')"
    '            End If
    '            ExecuteNonQuery(strSaveString)
    '        End If
    '    End If

    '    If command = ReportCommand.OpenFile Then
    '        Dim objForm As New frmChooseReportName
    '        objForm.ShowDialog()
    '        If Not strReportName Is Nothing AndAlso Len(strReportName) > 0 Then
    '            'Dim strString As String = "Select colCustomReportDesignerLayOut,colTableSchema,colNormalColumn,colRowColumn,colUniqueColumn from tbl_Public_CustomReportDesignerDetails Where colCustomReportDesignerName = N'" & strReportName & "'"
    '            'Dim dtTable As DataTable = BLCommon.GetData(strString)
    '            'If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 AndAlso Not IsDBNull(dtTable.Rows(0)("colCustomReportDesignerLayOut")) AndAlso Len(dtTable.Rows(0)("colCustomReportDesignerLayOut")) > 0 Then
    '            '    'If Not IsDBNull(dtTable.Rows(0)("colTableSchema")) AndAlso Len(dtTable.Rows(0)("colTableSchema")) > 0 Then
    '            '    '    strTableColumnSchema = dtTable.Rows(0)("colTableSchema")
    '            '    '    dtTableColumn = BLCommon.GetData(strTableColumnSchema)
    '            '    'Else
    '            '    '    strTableColumnSchema = String.Empty
    '            '    'End If
    '            '    'If Not IsDBNull(dtTable.Rows(0)("colNormalColumn")) AndAlso Len(dtTable.Rows(0)("colNormalColumn")) > 0 Then
    '            '    '    strNormalColumn = dtTable.Rows(0)("colNormalColumn")
    '            '    'Else
    '            '    '    strNormalColumn = String.Empty
    '            '    'End If
    '            '    'If Not IsDBNull(dtTable.Rows(0)("colRowColumn")) AndAlso Len(dtTable.Rows(0)("colRowColumn")) > 0 Then
    '            '    '    strRowColumn = dtTable.Rows(0)("colRowColumn")
    '            '    'Else
    '            '    '    strRowColumn = String.Empty
    '            '    'End If
    '            '    'If Not IsDBNull(dtTable.Rows(0)("colUniqueColumn")) AndAlso Len(dtTable.Rows(0)("colUniqueColumn")) > 0 Then
    '            '    '    strUniqueColumn = dtTable.Rows(0)("colUniqueColumn")
    '            '    'Else
    '            '    '    strUniqueColumn = String.Empty
    '            '    'End If
    '            '    'panel.CloseReport()
    '            '    'Dim xtraReport As XtraReport = GetReport(Replace(dtTable.Rows(0)("colCustomReportDesignerLayOut"), "SQLCONNECTIONSTRING", GetSQLConnection))
    '            '    ''xtraReport.Parameters(0).Value = "H1009010"
    '            '    ''xtraReport.RequestParameters = False
    '            '    'panel.OpenReport(GetReportFromLayOut(strReportName))
    '            'End If
    '            panel.OpenReport(GetReportFromLayOut(strReportName))
    '        End If
    '    End If

    '    If command = ReportCommand.AddNewDataSource OrElse (Len(strReportName) = 0 AndAlso command = ReportCommand.NewReport) OrElse (Len(strReportName) = 0 AndAlso command = ReportCommand.NewReportWizard) Then
    '        Dim wizard As New CustomWizard(panel.Report, panel)
    '        wizard.Run(command)
    '    End If

    '    If command = ReportCommand.ShowPreviewTab Then

    '        'Dim xtrReport As XtraReport = panel.Report

    '        'Dim obj As New ReportSettingsClass1
    '        'xtrReport = obj.GenerateRowsasColumn(xtrReport)

    '    End If

    '    handled = True
    'End Sub

    Private Function GetReportLayout(ByVal xtraReport As XtraReport) As String
        Dim ms As New MemoryStream()

        xtraReport.SaveLayout(ms)
        ms.Seek(0, SeekOrigin.Begin)

        Return New StreamReader(ms, Encoding.UTF8).ReadToEnd()
    End Function

    Private Function GetReport(ByVal layout As String) As XtraReport
        Dim ms As New MemoryStream()
        Dim sw As New StreamWriter(ms, Encoding.UTF8)

        sw.Write(layout.ToCharArray())
        sw.Flush()
        ms.Seek(0, SeekOrigin.Begin)

        Return XtraReport.FromStream(ms, True)
    End Function
End Class

Public Class CustomWizard
    Inherits XRWizardRunnerBase
    Private m_wizardCommand As ReportCommand
    Private XrDesignPanel1 As DevExpress.XtraReports.UserDesigner.XRDesignPanel

    Public Property WizardCommand() As ReportCommand
        Get
            Return m_wizardCommand
        End Get
        Set(ByVal value As ReportCommand)
            m_wizardCommand = value
        End Set
    End Property

    Public Sub New(ByVal report As XtraReport, ByVal Panel1 As DevExpress.XtraReports.UserDesigner.XRDesignPanel)
        MyBase.New(report)
        XrDesignPanel1 = Panel1
        'Me.Wizard = New NewXtraReportStandardBuilderWizard(report)
        Me.Wizard = New NewStandardReportWizard(report)
    End Sub

    Public Function Run(ByVal wizardCommand As ReportCommand) As DialogResult
        If Me.Report Is Nothing Then
            Return DialogResult.Cancel
        End If
        Try
            Dim sqlAd As SqlClient.SqlDataAdapter = New SqlClient.SqlDataAdapter
            sqlAd = Me.Report.DataAdapter
            strSqlCommandText = sqlAd.SelectCommand.CommandText
        Catch ex As Exception

        End Try
        Dim form As New XtraReportWizardForm(Wizard.DesignerHost)

        Me.WizardCommand = wizardCommand

        'If wizardCommand <> ReportCommand.AddNewDataSource Then
        '    form.Controls.Add(New WizPageWelcome(Me))
        'End If
        'new WizPageConnectionCustom(this),          // Custom
        ', and so on...

        'DirectCast(Wizard, NewXtraReportStandardBuilderWizard).DatasetName = "AAA"
        'DirectCast(Wizard, NewXtraReportStandardBuilderWizard).ConnectionString = "Provider=SQLOLEDB;" & GetSQLConnection()
        DirectCast(Wizard, NewStandardReportWizard).DatasetName = "AAA"
        DirectCast(Wizard, NewStandardReportWizard).ConnectionString = "Provider=SQLOLEDB;" & GetSQLConnection()
        If wizardCommand = ReportCommand.AddNewDataSource Then
            form.Controls.AddRange(New Control() {New RepWizardCustomQuery.WizPageDataOption(Me), New WizPageTablesCustom(Me), New RepWizardCustomQuery.WizPageQuery(Me), New WizPageChooseFields(Me), New WizPageGrouping(Me), New WizPageSummary(Me), New WizPageGroupedLayout(Me), _
                     New WizPageUngroupedLayout(Me), New WizPageStyle(Me), New WizPageReportTitle(Me), New WizPageLabelType(Me), New WizPageLabelOptions(Me)})
        Else
            form.Controls.AddRange(New Control() {New RepWizardCustomQuery.WizPageReportNameDetails(Me), New RepWizardCustomQuery.WizPageDataOption(Me), New WizPageTablesCustom(Me), New RepWizardCustomQuery.WizPageQuery(Me), New WizPageChooseFields(Me), New WizPageGrouping(Me), New WizPageSummary(Me), New WizPageGroupedLayout(Me), _
                     New WizPageUngroupedLayout(Me), New WizPageStyle(Me), New WizPageReportTitle(Me), New WizPageLabelType(Me), New WizPageLabelOptions(Me)})

            'DirectCast(Wizard, NewXtraReportStandardBuilderWizard).DatasetName = "AAA"
            'DirectCast(Wizard, NewXtraReportStandardBuilderWizard).ConnectionString = "Provider=SQLOLEDB;" & GetSQLConnection()
        End If

        Dim result As DialogResult = form.ShowDialog()

        If result = DialogResult.OK Then
            If wizardCommand = ReportCommand.AddNewDataSource Then

                Dim host As System.ComponentModel.Design.IDesignerHost = DirectCast(XrDesignPanel1.GetService(GetType(System.ComponentModel.Design.IDesignerHost)), System.ComponentModel.Design.IDesignerHost)
                Dim ds As System.ComponentModel.IComponent = Nothing
                For Each comp As System.ComponentModel.IComponent In host.Container.Components
                    If TypeOf comp Is DataSet Then
                        ds = comp
                    End If
                Next
                If Not ds Is Nothing Then
                    host.Container.Remove(ds)
                End If

                'Dim reportWizard As NewXtraReportStandardBuilderWizard = DirectCast(Wizard, NewXtraReportStandardBuilderWizard)
                Dim reportWizard As NewStandardReportWizard = DirectCast(Wizard, NewStandardReportWizard)
                Dim name As String = reportWizard.Dataset.DataSetName

                Dim pd As System.ComponentModel.PropertyDescriptor = System.ComponentModel.TypeDescriptor.GetProperties(Me.Report)("DataSource")
                pd.SetValue(Me.Report, reportWizard.Dataset)

                Me.Report.DataAdapter = reportWizard.DataAdapters.Item(reportWizard.DataAdapters.Count - 1)

                ds = XrDesignPanel1.Report.DataSource
                If Not ds Is Nothing Then
                    host.Container.Add(ds)
                End If

                Dim iiCommandHandler As New CommandHandler(XrDesignPanel1)
                'iiCommandHandler.HandleCommand(DevExpress.XtraReports.UserDesigner.ReportCommand.SaveFile, Nothing, False)
                'XrDesignPanel1.OpenReport(GetReportFromLayOut(strReportName))
                iiCommandHandler.HandleCommand(DevExpress.XtraReports.UserDesigner.ReportCommand.SaveFile, Nothing)
                'XrDesignPanel1.OpenReport(GetReportFromLayOut(strReportName))
            Else
                Wizard.BuildReport()
            End If
        End If

        Return result
    End Function


End Class

Public Class WizPageDatasetCustom
    Inherits WizPageDataset

    Private standardWizard As XtraReportWizardBase
    Private runner As CustomWizard


    Public Sub New(ByVal runner As XRWizardRunnerBase)
        MyBase.New(runner)
        Me.standardWizard = runner.Wizard
        Me.runner = TryCast(runner, CustomWizard)
    End Sub

    Protected Overrides Sub UpdateWizardButtons()
        MyBase.UpdateWizardButtons()

        If runner.WizardCommand = ReportCommand.AddNewDataSource Then
            MyBase.Wizard.WizardButtons = MyBase.Wizard.WizardButtons And (Not DevExpress.Utils.WizardButton.Back)
        End If
    End Sub

    Protected Overrides Function OnWizardNext() As String
        MyBase.OnWizardNext()

        'DirectCast(Me.standardWizard, NewXtraReportStandardBuilderWizard).DatasetName = "AAA"
        'DirectCast(Me.standardWizard, NewXtraReportStandardBuilderWizard).ConnectionString = "Provider=SQLOLEDB;" & GetSQLConnection()
        DirectCast(Me.standardWizard, NewStandardReportWizard).ConnectionString = "Provider=SQLOLEDB;" & GetSQLConnection()

        If runner.WizardCommand = ReportCommand.AddNewDataSource Then
            Return "WizPageDataOption"
        Else
            Return "WizPageReportNameDetails"
        End If
    End Function

End Class

Public Class WizPageTablesCustom
    Inherits WizPageTables

    Private standardWizard As XtraReportWizardBase

    Public Sub New(ByVal runner As XRWizardRunnerBase)
        MyBase.New(runner)
        standardWizard = DirectCast(runner.Wizard, NewStandardReportWizard)
        'standardWizard = DirectCast(runner.Wizard, NewXtraReportStandardBuilderWizard)
    End Sub

    Protected Overrides Function OnSetActive() As Boolean
        WizardHelper.LastDataSourceWizardType = "WizPageTables"

        Return MyBase.OnSetActive()
    End Function

    Protected Overrides Function OnWizardNext() As String
        MyBase.OnWizardNext()

        Return "WizPageChooseFields"
    End Function

End Class

Public Class WizardHelper
    Private Shared _lastDataSourceWizardType As String

    Public Shared Property LastDataSourceWizardType() As String
        Get
            Return WizardHelper._lastDataSourceWizardType
        End Get
        Set(ByVal value As String)
            WizardHelper._lastDataSourceWizardType = value
        End Set
    End Property

End Class

Public Class WizPageChooseFieldsCustom
    Inherits WizPageChooseFields

    Public Sub New(ByVal runner As XRWizardRunnerBase)

        MyBase.New(runner)
    End Sub

    Protected Overrides Function OnWizardBack() As String
        MyBase.OnWizardBack()

        Return WizardHelper.LastDataSourceWizardType
    End Function

End Class

