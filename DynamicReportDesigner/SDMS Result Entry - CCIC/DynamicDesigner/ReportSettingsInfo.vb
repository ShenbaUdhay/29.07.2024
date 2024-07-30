Imports System.Collections
Imports System.IO
Imports System.Text
Imports DevExpress.XtraReports.UI
Imports DynamicReportBusinessLayer
Imports ReportMultilanguage
Imports System.Math
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Security
Imports Modules.BusinessObjects.Hr



Public Class ReportSettingsInfo

    Public Function TableColumnSchema() As String
        Return strTableColumnSchema
    End Function

    Public Function RowColumn() As String
        Return strRowColumn
    End Function

    Public Function NormalColumn() As String
        Return strNormalColumn
    End Function

    Public Function UniqueColumn() As String
        Return strUniqueColumn
    End Function

    Public Function TableColumn() As DataTable
        Return dtTableColumn
    End Function

    Public Function SetDatasourceAsLocalTable(ByVal Report As XtraReport) As XtraReport
        Try
            Report.DataSource = Nothing
            Report.DataMember = Nothing
            Report.DataSource = dtReportDataSource
            Return Report
        Catch ex As Exception
            Return Report
        End Try
    End Function
    Public Sub CheckValue(ByVal obj As Object)
        Dim objTemp As Object = obj
    End Sub
    Public Function ExecuteDataSet(ByVal Query As String) As DataSet
        Dim ds As DataSet = DynamicReportBusinessLayer.BLCommon.ExecuteDataSet(Query)
        Return ds
    End Function
    Public Sub GetAccreditationLogo(ByVal sender As Object)
        sender.Image = BT_AESI_ACCREDIATIONLogo
    End Sub
    Public Sub GetAddressLine1(ByVal sender As Object)
        sender.text = BT_Address1
    End Sub
    Public Sub GetAddressLine2(ByVal sender As Object)
        sender.text = BT_Address3
    End Sub
    Public Sub GetAddressLine3(ByVal sender As Object)
        sender.text = BT_Address3
    End Sub
    Public Sub GetFax(ByVal sender As Object)
        sender.text = BT_Fax
    End Sub
    Public Sub GetPhone(ByVal sender As Object)
        sender.text = BT_Phone
    End Sub
    Public Function GetReleasedByUser() As String
        Try
            Dim StrReleasedByUser As String
            StrReleasedByUser = SecuritySystem.CurrentUserName
            Return StrReleasedByUser
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function dsReportDataSource() As DataSet
        Return dsDataSource
    End Function
    Public Function dsQCReportDataSource() As DataSet
        Return dsQCDataSource()
    End Function
    Public Function GetGridFilterPanelText() As String()
        Return strGridFilterPanelText
    End Function
    Public Function GetAccreditation() As String
        Return strAccreditation
    End Function

    Public Function GetSampleID() As String
        Return strSampleID
    End Function

    Public Function getInvoiceComment() As String
        If Not strInvoiceComment Is Nothing AndAlso Len(strInvoiceComment) > 0 Then
            Return strInvoiceComment
        End If
        Return strInvoiceComment
    End Function

    Public Function UpdateFinalResult() As Boolean
        Return bolUpdateFinal
    End Function

    Public Function TotalNoOfPage()
        If Len(TotalNumberPage) > 0 Then
            Return TotalNumberPage
        Else
            Return ""
        End If
    End Function
    Public Function GetstrpackageName() As String
        Return strpackageName
    End Function

    Public Function RepComment()
        Try
            Return strCPageComment
        Catch ex As Exception
        End Try
    End Function

    Public Function NoOfSamples() As String
        Return CPNoOfSamples
    End Function

    Public Function ReportedDate() As String
        Return CPReportedDate
    End Function

    Public Function SuboutStatus() As Boolean
        Return bolSuboutStatus
    End Function
    Public Function ReportStatus() As String
        Return CPReportStatus
    End Function
    Public Function GetAnalystBy() As ArrayList
        Return strAnalystBy
    End Function
    Public Function dtUnitConversion() As DataTable
        Return _dtUnitConversion
    End Function

    'Public Sub GetNELAPLogo(ByVal sender As Object)
    '    sender.Image = Multilanguage.BT_NELAPLogo
    'End Sub
    '' Copied this function from SDHJ
    '' On 13/06/2017 
    Public Function GenerateRowsasColumn(ByVal Report As XtraReport, ByVal dtTable As DataTable, Optional ByVal intNumberofColumns As Integer = 8, Optional ByVal strMainColumn As String = "Parameter", Optional ByVal bolParameterSortorder As Boolean = False) As XtraReport
        'Dim ds As DataSet = Report.DataSource
        'Dim dtTable As DataTable = New DataTable
        'If Report.DataSource.GetType.BaseType.Name = "DataTable" Then
        '    dtTable = Report.DataSource
        'Else
        '    Dim ds As DataSet = Report.DataSource
        '    dtTable = ds.Tables(0)
        'End If
        Try
            If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then
                dtTable.AcceptChanges()
                dtTableColumn.Rows.Clear()
                Try
                    Dim strSplit() As String = Split(strNormalColumn, ", ")
                    If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
                        For intC As Integer = 0 To strSplit.Length - 1
                            If dtTableColumn.Columns.Contains(strSplit(intC)) AndAlso dtTable.Columns.Contains(strSplit(intC)) Then
                                dtTableColumn.Columns(strSplit(intC)).DataType = dtTable.Columns(strSplit(intC)).DataType
                            End If
                        Next
                    End If
                    strSplit = Split(strUniqueColumn, ", ")
                    If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
                        For intC As Integer = 0 To strSplit.Length - 1
                            If dtTableColumn.Columns.Contains(strSplit(intC)) AndAlso dtTable.Columns.Contains(strSplit(intC)) Then
                                dtTableColumn.Columns(strSplit(intC)).DataType = dtTable.Columns(strSplit(intC)).DataType
                            End If
                        Next
                    End If
                    strSplit = Split(strRowColumn, ", ")
                    If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
                        For intC As Integer = 0 To strSplit.Length - 1
                            For ic As Integer = 1 To intNumberofColumns
                                If dtTableColumn.Columns.Contains(strSplit(intC) & ic.ToString) AndAlso dtTable.Columns.Contains(strSplit(intC)) Then
                                    dtTableColumn.Columns(strSplit(intC) & ic.ToString).DataType = dtTable.Columns(strSplit(intC)).DataType
                                End If
                            Next
                        Next
                    End If
                Catch ex As Exception

                End Try
                Dim dtParameterDetails As DataTable = New DataTable
                If bolParameterSortorder = True Then
                    If dtTable.Columns.IndexOf("ParamSortOrder") > -1 Then
                        dtParameterDetails = dtTable.DefaultView.ToTable(True, strMainColumn, "ParamSortOrder")
                        Dim dvParam As DataView = New DataView(dtParameterDetails, "", "ParamSortOrder asc", DataViewRowState.CurrentRows)
                        dtParameterDetails = dvParam.ToTable(True)
                    Else
                        dtParameterDetails = dtTable.DefaultView.ToTable(True, strMainColumn)
                    End If
                Else
                    dtParameterDetails = dtTable.DefaultView.ToTable(True, strMainColumn)
                End If

                Dim dtSampleDetails As DataTable = New DataTable
                'dtTable.DefaultView.ToTable(True, Split(strNormalColumn, ", "))
                If Not dtParameterDetails Is Nothing AndAlso dtParameterDetails.Rows.Count > 0 Then
                    Dim intParam As Integer = 0
                    Dim intPageCount As Integer = 0
                    For Each drParamRow As DataRow In dtParameterDetails.Rows
                        intParam = intParam + 1
                        Dim drParamAsString As String = ""
                        drParamAsString &= String.Format("{0}, ", drParamRow(strMainColumn))
                        If drParamAsString.EndsWith(", ") Then
                            drParamAsString = drParamAsString.Remove(drParamAsString.Length - 2, 2)
                        End If
                        Dim dvSampleDetails As DataView = New DataView(dtTable, strMainColumn & "='" & drParamAsString & "'", "", DataViewRowState.CurrentRows)
                        ' Dim dvSampleDetails As DataView = New DataView(dtTable, strMainColumn & "='" & drParamRow(strMainColumn) & "'", "", DataViewRowState.CurrentRows)
                        'dtSampleDetails = dvSampleDetails.ToTable(True, Split(strNormalColumn, ", "))
                        ' Dim dvSampleDetails As DataView = New DataView(dtTable, strMainColumn & "='" & drParamRow(strMainColumn) & "'", "", DataViewRowState.CurrentRows)
                        'dtSampleDetails = dvSampleDetails.ToTable(True, Split(strNormalColumn, ", "))
                        dtSampleDetails = dvSampleDetails.ToTable(True)
                        For Each drSampleRows As DataRow In dtSampleDetails.Rows
                            Dim strFilter As String = setFilter(drSampleRows, intPageCount)
                            If intParam = 1 Then
                                Dim drrRowExists() As DataRow = dtTableColumn.Select(strFilter)
                                If Not drrRowExists Is Nothing AndAlso drrRowExists.Length > 0 Then
                                Else
                                    Dim drNewRow As DataRow = dtTableColumn.NewRow
                                    drNewRow("PageIndex") = intPageCount
                                    If Not strNormalColumn Is Nothing AndAlso Len(strNormalColumn) > 0 Then
                                        Dim strSplit() As String = Split(strNormalColumn, ", ")
                                        If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
                                            For intC As Integer = 0 To strSplit.Length - 1
                                                drNewRow(strSplit(intC)) = drSampleRows(strSplit(intC))
                                            Next
                                        End If
                                    End If
                                    Dim bolFlag As Boolean = False
                                    '----------------------------------------
                                    If Not IsDBNull(drParamRow(strMainColumn)) AndAlso Not IsDBNull(drSampleRows(strMainColumn)) Then
                                        'If drParamRow(strMainColumn) = drParamRow1(strMainColumn) Then
                                        If Not strRowColumn Is Nothing AndAlso Len(strRowColumn) > 0 Then
                                            Dim strSplit() As String = Split(strRowColumn, ", ")
                                            If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
                                                Dim drrResult() As DataRow = dtTable.Select(GetResult(drSampleRows, drSampleRows))
                                                If Not drrResult Is Nothing AndAlso drrResult.Length > 0 Then
                                                    For intC As Integer = 0 To strSplit.Length - 1
                                                        drNewRow(strSplit(intC) & intParam) = drrResult(0)(strSplit(intC))
                                                        bolFlag = True
                                                    Next
                                                    'Exit For
                                                Else
                                                    'For intC As Integer = 0 To strSplit.Length - 1
                                                    '    If strSplit(intC) = strMainColumn Then
                                                    '        drNewRow(strSplit(intC) & intParam) = drParamRow(strSplit(intC))
                                                    '    End If
                                                    'Next
                                                End If
                                            End If
                                        End If
                                        'End If
                                    End If
                                    '----------------------------------------
                                    '-------------------------------------------
                                    'Dim dvParameterDetails1 As DataView = New DataView(dtTable, strMainColumn & "='" & drParamRow(strMainColumn) & "'", "", DataViewRowState.CurrentRows)
                                    'Dim dtParameterDetails1 As DataTable = dvParameterDetails1.ToTable(True, Split(strRowColumn, ", "))
                                    'For Each drParamRow1 As DataRow In dtParameterDetails1.Rows
                                    '    If Not IsDBNull(drParamRow(strMainColumn)) AndAlso Not IsDBNull(drParamRow1(strMainColumn)) Then
                                    '        'If drParamRow(strMainColumn) = drParamRow1(strMainColumn) Then
                                    '        If Not strRowColumn Is Nothing AndAlso Len(strRowColumn) > 0 Then
                                    '            Dim strSplit() As String = Split(strRowColumn, ", ")
                                    '            If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
                                    '                Dim drrResult() As DataRow = dtTable.Select(GetResult(drParamRow1, drSampleRows))
                                    '                If Not drrResult Is Nothing AndAlso drrResult.Length > 0 Then
                                    '                    For intC As Integer = 0 To strSplit.Length - 1
                                    '                        drNewRow(strSplit(intC) & intParam) = drrResult(0)(strSplit(intC))
                                    '                        bolFlag = True
                                    '                    Next
                                    '                    Exit For
                                    '                Else
                                    '                    'For intC As Integer = 0 To strSplit.Length - 1
                                    '                    '    If strSplit(intC) = strMainColumn Then
                                    '                    '        drNewRow(strSplit(intC) & intParam) = drParamRow(strSplit(intC))
                                    '                    '    End If
                                    '                    'Next
                                    '                End If
                                    '            End If
                                    '        End If
                                    '        'End If
                                    '    End If
                                    'Next
                                    '---------------------------------------
                                    'Dim drrResult1() As DataRow = dtTable.Select(GetResult(drParamRow, drSampleRows))
                                    'If Not drrResult1 Is Nothing AndAlso drrResult1.Length > 0 Then
                                    '    drNewRow("Result" & intParam) = drrResult1(0)("Result")
                                    'End If
                                    If bolFlag Then
                                        dtTableColumn.Rows.Add(drNewRow)
                                    End If
                                End If
                            Else
                                Dim drrExistingRows() As DataRow = dtTableColumn.Select(IIf(Len(strFilter) > 0, strFilter & " and ", "") & " PageIndex = '" & intPageCount & "'")
                                If Not drrExistingRows Is Nothing AndAlso drrExistingRows.Length > 0 Then
                                    '----------------------------------------
                                    If Not IsDBNull(drParamRow(strMainColumn)) AndAlso Not IsDBNull(drSampleRows(strMainColumn)) Then
                                        'If drParamRow(strMainColumn) = drParamRow1(strMainColumn) Then
                                        If Not strRowColumn Is Nothing AndAlso Len(strRowColumn) > 0 Then
                                            Dim strSplit() As String = Split(strRowColumn, ", ")
                                            If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
                                                Dim drrResult() As DataRow = dtTable.Select(GetResult(drSampleRows, drSampleRows))
                                                If Not drrResult Is Nothing AndAlso drrResult.Length > 0 Then
                                                    For intC As Integer = 0 To strSplit.Length - 1
                                                        drrExistingRows(0)(strSplit(intC) & intParam) = drrResult(0)(strSplit(intC))
                                                    Next
                                                Else
                                                    For intC As Integer = 0 To strSplit.Length - 1
                                                        If strSplit(intC) = strMainColumn Then
                                                            drrExistingRows(0)(strSplit(intC) & intParam) = drParamRow(strSplit(intC))
                                                        End If
                                                    Next
                                                End If
                                            End If
                                        End If
                                        'End If
                                    End If
                                    '----------------------------------------
                                    '-----------------------------------------
                                    'Dim dvParameterDetails1 As DataView = New DataView(dtTable, strMainColumn & "='" & drParamRow(strMainColumn) & "'", "", DataViewRowState.CurrentRows)
                                    'Dim dtParameterDetails1 As DataTable = dvParameterDetails1.ToTable(True, Split(strRowColumn, ", "))
                                    'For Each drParamRow1 As DataRow In dtParameterDetails1.Rows
                                    '    If Not IsDBNull(drParamRow(strMainColumn)) AndAlso Not IsDBNull(drParamRow1(strMainColumn)) Then
                                    '        'If drParamRow(strMainColumn) = drParamRow1(strMainColumn) Then
                                    '        If Not strRowColumn Is Nothing AndAlso Len(strRowColumn) > 0 Then
                                    '            Dim strSplit() As String = Split(strRowColumn, ", ")
                                    '            If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
                                    '                Dim drrResult() As DataRow = dtTable.Select(GetResult(drParamRow1, drSampleRows))
                                    '                If Not drrResult Is Nothing AndAlso drrResult.Length > 0 Then
                                    '                    For intC As Integer = 0 To strSplit.Length - 1
                                    '                        drrExistingRows(0)(strSplit(intC) & intParam) = drrResult(0)(strSplit(intC))
                                    '                    Next
                                    '                Else
                                    '                    For intC As Integer = 0 To strSplit.Length - 1
                                    '                        If strSplit(intC) = strMainColumn Then
                                    '                            drrExistingRows(0)(strSplit(intC) & intParam) = drParamRow(strSplit(intC))
                                    '                        End If
                                    '                    Next
                                    '                End If
                                    '            End If
                                    '        End If
                                    '        'End If
                                    '    End If
                                    'Next
                                    '---------------------------------------------------
                                    drrExistingRows(0).AcceptChanges()
                                Else
                                    Dim drNewRow As DataRow = dtTableColumn.NewRow
                                    drNewRow("PageIndex") = intPageCount
                                    If Not strNormalColumn Is Nothing AndAlso Len(strNormalColumn) > 0 Then
                                        Dim strSplit() As String = Split(strNormalColumn, ", ")
                                        If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
                                            For intC As Integer = 0 To strSplit.Length - 1
                                                drNewRow(strSplit(intC)) = drSampleRows(strSplit(intC))
                                            Next
                                        End If
                                    End If
                                    Dim bolFlag As Boolean = False
                                    '-----------------------------------------
                                    If Not strRowColumn Is Nothing AndAlso Len(strRowColumn) > 0 Then
                                        Dim strSplit() As String = Split(strRowColumn, ", ")
                                        If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
                                            Dim drrResult() As DataRow = dtTable.Select(GetResult(drSampleRows, drSampleRows))
                                            If Not drrResult Is Nothing AndAlso drrResult.Length > 0 Then
                                                For intC As Integer = 0 To strSplit.Length - 1
                                                    drNewRow(strSplit(intC) & intParam) = drrResult(0)(strSplit(intC))
                                                    bolFlag = True
                                                Next
                                                'Exit For
                                            Else
                                            End If
                                        End If
                                    End If
                                    '-----------------------------------------
                                    '-------------------------------------------
                                    'Dim dvParameterDetails1 As DataView = New DataView(dtTable, strMainColumn & "='" & drParamRow(strMainColumn) & "'", "", DataViewRowState.CurrentRows)
                                    'Dim dtParameterDetails1 As DataTable = dvParameterDetails1.ToTable(True, Split(strRowColumn, ", "))
                                    'For Each drParamRow1 As DataRow In dtParameterDetails1.Rows
                                    '    'If drParamRow(strMainColumn) = drParamRow1(strMainColumn) Then
                                    '    If Not strRowColumn Is Nothing AndAlso Len(strRowColumn) > 0 Then
                                    '        Dim strSplit() As String = Split(strRowColumn, ", ")
                                    '        If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
                                    '            Dim drrResult() As DataRow = dtTable.Select(GetResult(drParamRow1, drSampleRows))
                                    '            If Not drrResult Is Nothing AndAlso drrResult.Length > 0 Then
                                    '                For intC As Integer = 0 To strSplit.Length - 1
                                    '                    drNewRow(strSplit(intC) & intParam) = drrResult(0)(strSplit(intC))
                                    '                    bolFlag = True
                                    '                Next
                                    '                Exit For
                                    '            Else
                                    '                'For intC As Integer = 0 To strSplit.Length - 1
                                    '                '    If strSplit(intC) = strMainColumn Then
                                    '                '        drNewRow(strSplit(intC) & intParam) = drParamRow(strSplit(intC))
                                    '                '    End If
                                    '                'Next
                                    '            End If
                                    '        End If
                                    '    End If
                                    '    'End If
                                    'Next
                                    '-----------------------------------------------------
                                    If bolFlag Then
                                        dtTableColumn.Rows.Add(drNewRow)
                                    End If
                                End If
                            End If
                        Next
                        'Dim dvAssignTest As DataView = New DataView(dtTableColumn, " PageIndex = " & intPageCount & " and (" & strMainColumn & intParam & " is null or ( " & strMainColumn & intParam & " is not null and  " & strMainColumn & intParam & "=''))", "", DataViewRowState.CurrentRows)
                        Dim dvAssignTest As DataView = New DataView(dtTableColumn, " PageIndex = " & intPageCount & " and (" & strMainColumn & intParam & " is null)", "", DataViewRowState.CurrentRows)
                        If dvAssignTest.Count > 0 Then
                            For Each drvA As DataRowView In dvAssignTest
                                drvA(strMainColumn & intParam) = drParamRow(strMainColumn)
                            Next
                        End If
                        If (intParam Mod intNumberofColumns) = 0 Then
                            intParam = 0
                            intPageCount = intPageCount + 1
                        End If
                    Next
                End If
            End If
            Report.DataSource = Nothing
            Report.DataMember = Nothing
            Report.DataSource = dtTableColumn
            Report.DataMember = "DataTable"
            Return Report
        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
    End Function

    Public Function GenerateRowsasColumn(ByVal Report As XtraReport, Optional ByVal intNumberofColumns As Integer = 8, Optional ByVal strMainColumn As String = "Parameter", Optional ByVal bolParameterSortorder As Boolean = False) As XtraReport
        'Dim ds As DataSet = Report.DataSource
        Dim dtTable As DataTable = New DataTable
        If Report.DataSource.GetType.BaseType.Name = "DataTable" Then
            dtTable = Report.DataSource
        Else
            Dim ds As DataSet = Report.DataSource
            dtTable = ds.Tables(0)
        End If

        If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then
            dtTable.AcceptChanges()
            dtTableColumn.Rows.Clear()
            Try
                Dim strSplit() As String = Split(strNormalColumn, ", ")
                If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
                    For intC As Integer = 0 To strSplit.Length - 1
                        If dtTableColumn.Columns.Contains(strSplit(intC)) AndAlso dtTable.Columns.Contains(strSplit(intC)) Then
                            dtTableColumn.Columns(strSplit(intC)).DataType = dtTable.Columns(strSplit(intC)).DataType
                        End If
                    Next
                End If
                strSplit = Split(strUniqueColumn, ", ")
                If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
                    For intC As Integer = 0 To strSplit.Length - 1
                        If dtTableColumn.Columns.Contains(strSplit(intC)) AndAlso dtTable.Columns.Contains(strSplit(intC)) Then
                            dtTableColumn.Columns(strSplit(intC)).DataType = dtTable.Columns(strSplit(intC)).DataType
                        End If
                    Next
                End If
                strSplit = Split(strRowColumn, ", ")
                If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
                    For intC As Integer = 0 To strSplit.Length - 1
                        For ic As Integer = 1 To intNumberofColumns
                            If dtTableColumn.Columns.Contains(strSplit(intC) & ic.ToString) AndAlso dtTable.Columns.Contains(strSplit(intC)) Then
                                dtTableColumn.Columns(strSplit(intC) & ic.ToString).DataType = dtTable.Columns(strSplit(intC)).DataType
                            End If
                        Next
                    Next
                End If
            Catch ex As Exception

            End Try
            Dim dtParameterDetails As DataTable = New DataTable
            If bolParameterSortorder = True Then
                If dtTable.Columns.IndexOf("ParamSortOrder") > -1 Then
                    dtParameterDetails = dtTable.DefaultView.ToTable(True, strMainColumn, "ParamSortOrder")
                    Dim dvParam As DataView = New DataView(dtParameterDetails, "", "ParamSortOrder asc", DataViewRowState.CurrentRows)
                    dtParameterDetails = dvParam.ToTable(True)
                Else
                    dtParameterDetails = dtTable.DefaultView.ToTable(True, strMainColumn)
                End If
            Else
                dtParameterDetails = dtTable.DefaultView.ToTable(True, strMainColumn)
            End If

            Dim dtSampleDetails As DataTable = New DataTable
            'dtTable.DefaultView.ToTable(True, Split(strNormalColumn, ", "))
            If Not dtParameterDetails Is Nothing AndAlso dtParameterDetails.Rows.Count > 0 Then
                Dim intParam As Integer = 0
                Dim intPageCount As Integer = 0
                For Each drParamRow As DataRow In dtParameterDetails.Rows
                    intParam = intParam + 1
                    Dim dvSampleDetails As DataView = New DataView(dtTable, strMainColumn & "='" & drParamRow(strMainColumn) & "'", "", DataViewRowState.CurrentRows)
                    'dtSampleDetails = dvSampleDetails.ToTable(True, Split(strNormalColumn, ", "))
                    dtSampleDetails = dvSampleDetails.ToTable(True)
                    For Each drSampleRows As DataRow In dtSampleDetails.Rows
                        Dim strFilter As String = setFilter(drSampleRows, intPageCount)
                        If intParam = 1 Then
                            Dim drrRowExists() As DataRow = dtTableColumn.Select(strFilter)
                            If Not drrRowExists Is Nothing AndAlso drrRowExists.Length > 0 Then
                            Else
                                Dim drNewRow As DataRow = dtTableColumn.NewRow
                                drNewRow("PageIndex") = intPageCount
                                If Not strNormalColumn Is Nothing AndAlso Len(strNormalColumn) > 0 Then
                                    Dim strSplit() As String = Split(strNormalColumn, ", ")
                                    If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
                                        For intC As Integer = 0 To strSplit.Length - 1
                                            drNewRow(strSplit(intC)) = drSampleRows(strSplit(intC))
                                        Next
                                    End If
                                End If
                                Dim bolFlag As Boolean = False
                                '----------------------------------------
                                If Not IsDBNull(drParamRow(strMainColumn)) AndAlso Not IsDBNull(drSampleRows(strMainColumn)) Then
                                    'If drParamRow(strMainColumn) = drParamRow1(strMainColumn) Then
                                    If Not strRowColumn Is Nothing AndAlso Len(strRowColumn) > 0 Then
                                        Dim strSplit() As String = Split(strRowColumn, ", ")
                                        If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
                                            Dim drrResult() As DataRow = dtTable.Select(GetResult(drSampleRows, drSampleRows))
                                            If Not drrResult Is Nothing AndAlso drrResult.Length > 0 Then
                                                For intC As Integer = 0 To strSplit.Length - 1
                                                    drNewRow(strSplit(intC) & intParam) = drrResult(0)(strSplit(intC))
                                                    bolFlag = True
                                                Next
                                                'Exit For
                                            Else
                                                'For intC As Integer = 0 To strSplit.Length - 1
                                                '    If strSplit(intC) = strMainColumn Then
                                                '        drNewRow(strSplit(intC) & intParam) = drParamRow(strSplit(intC))
                                                '    End If
                                                'Next
                                            End If
                                        End If
                                    End If
                                    'End If
                                End If
                                '----------------------------------------
                                '-------------------------------------------
                                'Dim dvParameterDetails1 As DataView = New DataView(dtTable, strMainColumn & "='" & drParamRow(strMainColumn) & "'", "", DataViewRowState.CurrentRows)
                                'Dim dtParameterDetails1 As DataTable = dvParameterDetails1.ToTable(True, Split(strRowColumn, ", "))
                                'For Each drParamRow1 As DataRow In dtParameterDetails1.Rows
                                '    If Not IsDBNull(drParamRow(strMainColumn)) AndAlso Not IsDBNull(drParamRow1(strMainColumn)) Then
                                '        'If drParamRow(strMainColumn) = drParamRow1(strMainColumn) Then
                                '        If Not strRowColumn Is Nothing AndAlso Len(strRowColumn) > 0 Then
                                '            Dim strSplit() As String = Split(strRowColumn, ", ")
                                '            If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
                                '                Dim drrResult() As DataRow = dtTable.Select(GetResult(drParamRow1, drSampleRows))
                                '                If Not drrResult Is Nothing AndAlso drrResult.Length > 0 Then
                                '                    For intC As Integer = 0 To strSplit.Length - 1
                                '                        drNewRow(strSplit(intC) & intParam) = drrResult(0)(strSplit(intC))
                                '                        bolFlag = True
                                '                    Next
                                '                    Exit For
                                '                Else
                                '                    'For intC As Integer = 0 To strSplit.Length - 1
                                '                    '    If strSplit(intC) = strMainColumn Then
                                '                    '        drNewRow(strSplit(intC) & intParam) = drParamRow(strSplit(intC))
                                '                    '    End If
                                '                    'Next
                                '                End If
                                '            End If
                                '        End If
                                '        'End If
                                '    End If
                                'Next
                                '---------------------------------------
                                'Dim drrResult1() As DataRow = dtTable.Select(GetResult(drParamRow, drSampleRows))
                                'If Not drrResult1 Is Nothing AndAlso drrResult1.Length > 0 Then
                                '    drNewRow("Result" & intParam) = drrResult1(0)("Result")
                                'End If
                                If bolFlag Then
                                    dtTableColumn.Rows.Add(drNewRow)
                                End If
                            End If
                        Else
                            Dim drrExistingRows() As DataRow = dtTableColumn.Select(IIf(Len(strFilter) > 0, strFilter & " and ", "") & " PageIndex = " & intPageCount & "")
                            If Not drrExistingRows Is Nothing AndAlso drrExistingRows.Length > 0 Then
                                '----------------------------------------
                                If Not IsDBNull(drParamRow(strMainColumn)) AndAlso Not IsDBNull(drSampleRows(strMainColumn)) Then
                                    'If drParamRow(strMainColumn) = drParamRow1(strMainColumn) Then
                                    If Not strRowColumn Is Nothing AndAlso Len(strRowColumn) > 0 Then
                                        Dim strSplit() As String = Split(strRowColumn, ", ")
                                        If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
                                            Dim drrResult() As DataRow = dtTable.Select(GetResult(drSampleRows, drSampleRows))
                                            If Not drrResult Is Nothing AndAlso drrResult.Length > 0 Then
                                                For intC As Integer = 0 To strSplit.Length - 1
                                                    drrExistingRows(0)(strSplit(intC) & intParam) = drrResult(0)(strSplit(intC))
                                                Next
                                            Else
                                                For intC As Integer = 0 To strSplit.Length - 1
                                                    If strSplit(intC) = strMainColumn Then
                                                        drrExistingRows(0)(strSplit(intC) & intParam) = drParamRow(strSplit(intC))
                                                    End If
                                                Next
                                            End If
                                        End If
                                    End If
                                    'End If
                                End If
                                '----------------------------------------
                                '-----------------------------------------
                                'Dim dvParameterDetails1 As DataView = New DataView(dtTable, strMainColumn & "='" & drParamRow(strMainColumn) & "'", "", DataViewRowState.CurrentRows)
                                'Dim dtParameterDetails1 As DataTable = dvParameterDetails1.ToTable(True, Split(strRowColumn, ", "))
                                'For Each drParamRow1 As DataRow In dtParameterDetails1.Rows
                                '    If Not IsDBNull(drParamRow(strMainColumn)) AndAlso Not IsDBNull(drParamRow1(strMainColumn)) Then
                                '        'If drParamRow(strMainColumn) = drParamRow1(strMainColumn) Then
                                '        If Not strRowColumn Is Nothing AndAlso Len(strRowColumn) > 0 Then
                                '            Dim strSplit() As String = Split(strRowColumn, ", ")
                                '            If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
                                '                Dim drrResult() As DataRow = dtTable.Select(GetResult(drParamRow1, drSampleRows))
                                '                If Not drrResult Is Nothing AndAlso drrResult.Length > 0 Then
                                '                    For intC As Integer = 0 To strSplit.Length - 1
                                '                        drrExistingRows(0)(strSplit(intC) & intParam) = drrResult(0)(strSplit(intC))
                                '                    Next
                                '                Else
                                '                    For intC As Integer = 0 To strSplit.Length - 1
                                '                        If strSplit(intC) = strMainColumn Then
                                '                            drrExistingRows(0)(strSplit(intC) & intParam) = drParamRow(strSplit(intC))
                                '                        End If
                                '                    Next
                                '                End If
                                '            End If
                                '        End If
                                '        'End If
                                '    End If
                                'Next
                                '---------------------------------------------------
                                drrExistingRows(0).AcceptChanges()
                            Else
                                Dim drNewRow As DataRow = dtTableColumn.NewRow
                                drNewRow("PageIndex") = intPageCount
                                If Not strNormalColumn Is Nothing AndAlso Len(strNormalColumn) > 0 Then
                                    Dim strSplit() As String = Split(strNormalColumn, ", ")
                                    If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
                                        For intC As Integer = 0 To strSplit.Length - 1
                                            drNewRow(strSplit(intC)) = drSampleRows(strSplit(intC))
                                        Next
                                    End If
                                End If
                                Dim bolFlag As Boolean = False
                                '-----------------------------------------
                                If Not strRowColumn Is Nothing AndAlso Len(strRowColumn) > 0 Then
                                    Dim strSplit() As String = Split(strRowColumn, ", ")
                                    If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
                                        Dim drrResult() As DataRow = dtTable.Select(GetResult(drSampleRows, drSampleRows))
                                        If Not drrResult Is Nothing AndAlso drrResult.Length > 0 Then
                                            For intC As Integer = 0 To strSplit.Length - 1
                                                drNewRow(strSplit(intC) & intParam) = drrResult(0)(strSplit(intC))
                                                bolFlag = True
                                            Next
                                            'Exit For
                                        Else
                                        End If
                                    End If
                                End If
                                '-----------------------------------------
                                '-------------------------------------------
                                'Dim dvParameterDetails1 As DataView = New DataView(dtTable, strMainColumn & "='" & drParamRow(strMainColumn) & "'", "", DataViewRowState.CurrentRows)
                                'Dim dtParameterDetails1 As DataTable = dvParameterDetails1.ToTable(True, Split(strRowColumn, ", "))
                                'For Each drParamRow1 As DataRow In dtParameterDetails1.Rows
                                '    'If drParamRow(strMainColumn) = drParamRow1(strMainColumn) Then
                                '    If Not strRowColumn Is Nothing AndAlso Len(strRowColumn) > 0 Then
                                '        Dim strSplit() As String = Split(strRowColumn, ", ")
                                '        If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
                                '            Dim drrResult() As DataRow = dtTable.Select(GetResult(drParamRow1, drSampleRows))
                                '            If Not drrResult Is Nothing AndAlso drrResult.Length > 0 Then
                                '                For intC As Integer = 0 To strSplit.Length - 1
                                '                    drNewRow(strSplit(intC) & intParam) = drrResult(0)(strSplit(intC))
                                '                    bolFlag = True
                                '                Next
                                '                Exit For
                                '            Else
                                '                'For intC As Integer = 0 To strSplit.Length - 1
                                '                '    If strSplit(intC) = strMainColumn Then
                                '                '        drNewRow(strSplit(intC) & intParam) = drParamRow(strSplit(intC))
                                '                '    End If
                                '                'Next
                                '            End If
                                '        End If
                                '    End If
                                '    'End If
                                'Next
                                '-----------------------------------------------------
                                If bolFlag Then
                                    dtTableColumn.Rows.Add(drNewRow)
                                End If
                            End If
                        End If
                    Next
                    Dim dvAssignTest As DataView = New DataView(dtTableColumn, " PageIndex = " & intPageCount & " and (" & strMainColumn & intParam & " is null or len(" & strMainColumn & intParam & ")=0)", "", DataViewRowState.CurrentRows)
                    If dvAssignTest.Count > 0 Then
                        For Each drvA As DataRowView In dvAssignTest
                            drvA(strMainColumn & intParam) = drParamRow(strMainColumn)
                        Next
                    End If
                    If (intParam Mod intNumberofColumns) = 0 Then
                        intParam = 0
                        intPageCount = intPageCount + 1
                    End If
                Next
            End If
        End If
        Report.DataSource = Nothing
        Report.DataMember = Nothing
        Report.DataSource = dtTableColumn
        Return Report
    End Function

    Public Function FormatSF(ByVal dblInput As Double, ByVal intSF As Integer, ByVal bolIsNotation As Boolean) As String
        Try
            If dblInput = 0 Then
                Return dblInput.ToString
            End If
            Dim strResult As String = String.Empty
            Dim strCal As String = String.Empty
            Dim intCorrPower As Integer         'Exponent used in rounding calculation
            Dim intSign As Integer              'Holds sign of dblInput since logs are used in 
            Dim intInputDigitCount As Integer
            '-- Store sign of dblInput --
            intSign = Sign(dblInput)

            Dim strInput As String = String.Empty
            strInput = CStr(dblInput)
            If Not strInput Is Nothing AndAlso Len(strInput) > 0 Then
                Dim strInputSplit() As String = Split(strInput, ".")
                If Not strInputSplit Is Nothing AndAlso strInputSplit.Length > 0 Then
                    If strInputSplit(0).Contains("-") Then
                        intInputDigitCount = Len(strInputSplit(0)) - 1
                    Else
                        intInputDigitCount = Len(strInputSplit(0))
                    End If
                End If
            End If

            '-- Calculate exponent of dblInput --
            intCorrPower = Int(Log10(Abs(dblInput)))
            '--START: Modified by Mohan, on 17th December 2014 for http://ablabs.net/btlimstracker/edit_bug.aspx?id=2334
            strCal = dblInput
            If intCorrPower >= 0 AndAlso Mid(strCal, 1, intSF).Contains(".") Then 'strCal.Contains(".") Then
                Try
                    Dim chArrCal As Char() = strInput.ToCharArray()
                    Dim strTempstrCal As String = String.Empty
                    strTempstrCal = Replace(Replace(strCal, "-", ""), ".", "")
                    'When length of input value is more than significant number and 
                    'next char after significant number is greater than 4
                    If strTempstrCal.Length > intSF AndAlso Mid(strTempstrCal, intSF + 1, 1) > 4 Then
                        Dim intExtraDigitsToCare As Integer = 0
                        If New String(chArrCal, 0, intSF).Contains("-") = True AndAlso New String(chArrCal, 0, intSF).Contains(".") = True Then
                            intExtraDigitsToCare = 2
                        ElseIf New String(chArrCal, 0, intSF).Contains("-") = True OrElse New String(chArrCal, 0, intSF).Contains(".") = True Then
                            intExtraDigitsToCare = 1
                        End If
                        '- Task ID 3302, if number is 4.251 and sigfig 2, here 
                        ' 1st condition checks last significant digit [2] is even or odd, ==> 4.251 = 4.2 (2 is even) => 4.25
                        ' 2nd condition checks the following significant digit [5] is 5 or not, ==>(after sigfig is 5) => 4.251
                        ' 3rd condition checks is there any number after significant digit [5] except 0 or empty, ==>(number after 5 is 1, so we need to add 1 to last significant digit i.e 2+1 = 3) => 4.3
                        ' 1,2 and 3 conditions satisfies then result is 4.3
                        '  
                        'If strTempstrCal.Length > intSF + 2 AndAlso Mid(strTempstrCal, intSF, 1) Mod 2 = 0 AndAlso Mid(strTempstrCal, intSF + 1, 1) = 5 AndAlso _
                        '  (Mid(strTempstrCal, intSF + 2, 1) = 0 OrElse Mid(strTempstrCal, intSF + 2, 1) = "") Then '-- TASK ID : 2925

                        'AndAlso Mid(strTempstrCal, intSF + 1, 1) = 5
                        If Mid(strTempstrCal, intSF, 1) Mod 2 = 0 AndAlso Mid(strTempstrCal, intSF + 1, 1) = 5 Then
                            If strTempstrCal.Length = intSF + 1 Then
                                'When last significant digit is Even number
                                strCal = New String(chArrCal, 0, intSF + intExtraDigitsToCare)
                            End If
                        Else
                            'When last significant digit is Odd number
                            strCal = New String(chArrCal, 0, intSF + intExtraDigitsToCare)
                            If Val(Microsoft.VisualBasic.Right(strCal, 1)) > 8 AndAlso strCal.Contains(".") Then
                                'strCal = New String(chArrCal, 0, intSF + intExtraDigitsToCare - 1) & (Microsoft.VisualBasic.Right(strCal, 1) + 1)
                                Dim intNoofDecimals As Integer = strCal.Length - InStr(strCal, ".")
                                Dim strDecimal As String = "0."
                                Do Until strDecimal.Length >= intNoofDecimals + 1
                                    strDecimal = strDecimal & "0"
                                Loop
                                strCal = CDbl(strCal) + CDbl((strDecimal & "1"))
                            ElseIf Val(Microsoft.VisualBasic.Right(strCal, 1)) > 8 Then
                                strCal = Val(strCal) + 1
                            Else
                                strCal = New String(chArrCal, 0, intSF + intExtraDigitsToCare - 1) & (Microsoft.VisualBasic.Right(strCal, 1) + 1)
                            End If
                        End If
                    End If

                Catch ex As Exception
                    'If any error, assign the original input value
                    strCal = strInput
                Finally
                    dblInput = CDbl(strCal)
                End Try
            ElseIf Mid(strCal, 1, intSF).Contains(".") Then 'strCal.Contains(".") Then
                Try
                    Dim chArrCal As Char() = strInput.ToCharArray()
                    Dim strTempstrCal As String = String.Empty
                    strTempstrCal = Replace(Replace(strCal, "-", ""), ".", "")
                    Dim strTempstrCal_1 As String = strTempstrCal
                    Dim intZeroCount As Integer = Len(strTempstrCal) - Len(strTempstrCal_1.TrimStart("0"c))
                    'Dim strSplit() As String = Split(strCal, ".")
                    'If Len(Replace(strSplit(0), "-", "")) > 0 Then
                    '    intZeroCount = intZeroCount - Len(Replace(strSplit(0), "-", ""))
                    'End If
                    If (intZeroCount + intSF + 1) >= 0 AndAlso (intZeroCount + intSF + 1) <= strTempstrCal.Length Then
                        'If strTempstrCal.Length > intSF AndAlso Mid(strTempstrCal, strTempstrCal.Length - (intSF - (intZeroCount + 1)), 1) > 4 Then
                        If strTempstrCal.Length > intSF AndAlso Mid(strTempstrCal, (intZeroCount + intSF + 1), 1) > 4 Then
                            Dim intExtraDigitsToCare As Integer = 0
                            If New String(chArrCal, 0, intSF).Contains("-") = True AndAlso New String(chArrCal, 0, intSF).Contains(".") = True Then
                                intExtraDigitsToCare = 2 + intZeroCount
                            ElseIf New String(chArrCal, 0, intSF).Contains("-") = True OrElse New String(chArrCal, 0, intSF).Contains(".") = True Then
                                intExtraDigitsToCare = 1 + intZeroCount
                            End If
                            If Mid(strTempstrCal, intSF + intZeroCount, 1) Mod 2 = 0 AndAlso Mid(strTempstrCal, intSF + 1 + intZeroCount, 1) = 5 AndAlso
                                  (Mid(strTempstrCal, intSF + 2 + intZeroCount, 1) = "" OrElse Mid(strTempstrCal, intSF + 2 + intZeroCount, 1) = 0) Then
                                strCal = New String(chArrCal, 0, intSF + intExtraDigitsToCare)
                            Else
                                strCal = New String(chArrCal, 0, intSF + intExtraDigitsToCare)
                                If Val(Microsoft.VisualBasic.Right(strCal, 1)) > 8 AndAlso strCal.Contains(".") Then
                                    Dim intNoofDecimals As Integer = strCal.Length - InStr(strCal, ".")
                                    Dim strDecimal As String = "0."
                                    Do Until strDecimal.Length >= intNoofDecimals + 1
                                        strDecimal = strDecimal & "0"
                                    Loop
                                    strCal = CDbl(strCal) + CDbl((strDecimal & "1"))
                                ElseIf Val(Microsoft.VisualBasic.Right(strCal, 1)) > 8 Then
                                    strCal = Val(strCal) + 1
                                Else
                                    strCal = New String(chArrCal, 0, intSF + intExtraDigitsToCare - 1) & (Microsoft.VisualBasic.Right(strCal, 1) + 1)
                                End If
                            End If
                        End If
                    End If
                Catch ex As Exception
                    strCal = strInput
                Finally
                    dblInput = CDbl(strCal)
                End Try
            End If
            '--End: Modified by Mohan, on 17th December 2014 for http://ablabs.net/btlimstracker/edit_bug.aspx?id=2334
            strCal = Round(dblInput * 10 ^ ((intSF - 1) - intCorrPower))   'integer value with no sig fig
            'If bolIsNotation = False Or (bolIsNotation = True AndAlso intCorrPower > 0) Then
            '    strCal = strCal * 10 ^ (intCorrPower - (intSF - 1))         'raise to original power
            'End If

            If bolIsNotation = False Then
                Dim strDot As String = String.Empty
                If (intSF - (intInputDigitCount + 1)) >= 0 AndAlso (intSF - (intInputDigitCount + 1)) <= strCal.Length Then
                    If CDbl(Mid(strCal, strCal.Length - (intSF - (intInputDigitCount + 1)), strCal.Length)) = 0 Then
                        strDot = Mid(strCal, strCal.Length - (intSF - (intInputDigitCount + 1)), strCal.Length)
                    End If
                End If
                strCal = strCal * 10 ^ (intCorrPower - (intSF - 1))        'raise to original power
                Dim intNumberofFraction As Integer
                intNumberofFraction = GetNoOfFractionDigits(CDbl(strCal))
                strCal = [String].Format("{0:F" & intNumberofFraction & "}", (CDbl(strCal)))
                If Len(strDot) > 0 Then
                    If strCal.Contains(".") Then
                        strCal = strCal & strDot
                    Else
                        strCal = strCal & "." & strDot
                    End If
                End If
                'If flagcal = False Then
                '    If (intCorrPower - (intSF - 1)) > 0 AndAlso intCorrPower > 1 Then
                '        flagcal = True
                '        Return FormatSF(CDbl(strCal), intSF, True)
                '    End If
                'End If
            ElseIf (bolIsNotation = True AndAlso intCorrPower > 0) Then
                If InStr(dblInput, ".") = 0 AndAlso Len(strCal.Replace("-", "")) = intSF Then
                Else
                    Dim strDot As String = String.Empty
                    If (intSF - (intInputDigitCount + 1)) >= 0 AndAlso (intSF - (intInputDigitCount + 1)) <= strCal.Length Then
                        If CDbl(Mid(strCal, strCal.Length - (intSF - (intInputDigitCount + 1)), strCal.Length)) = 0 Then
                            strDot = Mid(strCal, strCal.Length - (intSF - (intInputDigitCount + 1)), strCal.Length)
                        End If
                    End If
                    If intCorrPower <> intSF Then
                        strCal = strCal * 10 ^ (intCorrPower - (intSF - 1))        'raise to original power
                    Else

                    End If
                    Dim intNumberofFraction As Integer
                    intNumberofFraction = GetNoOfFractionDigits(CDbl(strCal))
                    strCal = [String].Format("{0:F" & intNumberofFraction & "}", (CDbl(strCal)))
                    If Len(strDot) > 0 Then
                        strCal = strCal & "." & strDot
                    End If
                End If
                'If (intCorrPower - (intSF - 1)) > 0 AndAlso intCorrPower > 1 Then
                '    Return FormatSF(CDbl(strCal), intSF, True)
                'End If
            End If
            ''-- Answer sometimes needs padding with 0s --
            'If InStr(strCal, ".") = 0 Then
            '    If Len(strCal) < intSF Then
            '        strCal = Format(strCal, "##0." & CStr(intSF - Len(strCal)))
            '    End If
            'End If
            If intSF > 1 And Abs(CDbl(strCal)) < 1 Then
                Do Until Microsoft.VisualBasic.Left(Microsoft.VisualBasic.Right(strCal, intSF), 1) <> "0" And Microsoft.VisualBasic.Left(Microsoft.VisualBasic.Right(strCal, intSF), 1) <> "."
                    strCal = strCal & "0"
                Loop
            End If

            If bolIsNotation = False Then
                If Val(strCal) > 0 Then
                    Dim strTem As String = Replace(strCal, ".", "")
                    Dim Icount As Integer = intSF - Len(strTem)
                    For iCheck As Integer = 1 To Icount
                        strCal = strCal & "0"
                    Next
                End If
                strResult = strCal
                Return strResult
            ElseIf bolIsNotation = True Then
                If intCorrPower <> 0 Then

                    Dim strFisrt As String = Microsoft.VisualBasic.Left(strCal, 1)
                    If strFisrt = "-" Then
                        strFisrt = Microsoft.VisualBasic.Left(strCal, 2)
                    End If
                    Dim strSecont As String = Microsoft.VisualBasic.Right(strCal, strCal.Length - 1)
                    If Len(strSecont) > 0 Then
                        strResult = strFisrt & "." & Replace(strSecont, ".", "") & " X 10 " & SplitChar(intCorrPower)
                    Else
                        strResult = strFisrt & " X 10 " & SplitChar(intCorrPower)
                    End If

                    'strResult = strFisrt & "." & Replace(strSecont, ".", "") & " * 10 ^ " & intCorrPower
                    Return strResult
                Else
                    Dim strDot As String = String.Empty
                    If (intSF - (intInputDigitCount + 1)) >= 0 AndAlso (intSF - (intInputDigitCount + 1)) <= strCal.Length Then
                        If CDbl(Mid(strCal, strCal.Length - (intSF - (intInputDigitCount + 1)), strCal.Length)) = 0 Then
                            strDot = Mid(strCal, strCal.Length - (intSF - (intInputDigitCount + 1)), strCal.Length)
                        End If
                    End If
                    strCal = strCal * 10 ^ (intCorrPower - (intSF - 1))        'raise to original power
                    If Len(strDot) > 0 Then
                        strCal = strCal & "." & strDot
                    End If
                    strResult = strCal
                    Return strResult
                End If
            End If
            Return strResult
        Catch ex As Exception
            Return dblInput.ToString
            'Finally
            '    flagcal = False
        End Try
    End Function
    Public Function GetNoOfFractionDigits(ByVal dbl As Double) As Integer
        Try

            Dim iFCount As Integer
            Dim strNumber As String = CStr(dbl)
            If Not strNumber Is Nothing AndAlso Len(strNumber) > 0 AndAlso IsNumeric(strNumber) Then

                Dim decDbNumber As Decimal
                decDbNumber = Decimal.Parse(strNumber, System.Globalization.NumberStyles.Any)
                Dim strNumberSplit() As String = decDbNumber.ToString.Split(CChar(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
                If strNumberSplit.Length > 1 Then
                    iFCount = strNumberSplit(1).Length
                End If

            End If
            Return iFCount
        Catch ex As Exception
            Return 0
        End Try
    End Function
    Public Function Split(ByVal strText As String, ByVal Separator() As Char) As String()
        Try
            Dim strSeparator As String = New String(Separator)
            Dim strArray() As String = Nothing
            If Len(Trim(strText)) > 0 Then
                strArray = strText.Split(strSeparator)
                Return strArray
            End If
            Return strArray
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Private Function SplitChar(ByVal strVal As String) As String
        Dim RetChar As String = String.Empty
        Dim singleChar As Char
        For Each singleChar In strVal
            If Len(RetChar) > 0 Then
                RetChar = RetChar & SuperScriptChar(singleChar)
            Else
                RetChar = SuperScriptChar(singleChar)
            End If
        Next
        Return RetChar
    End Function

    Private Function SuperScriptChar(ByVal strVal As String) As String
        If strVal = "0" Then
            Return "⁰"
        End If
        If strVal = "1" Then
            Return "¹"
        End If
        If strVal = "2" Then
            Return "²"
        End If
        If strVal = "3" Then
            Return "³"
        End If
        If strVal = "4" Then
            Return "⁴"
        End If
        If strVal = "5" Then
            Return "⁵"
        End If
        If strVal = "6" Then
            Return "⁶"
        End If
        If strVal = "7" Then
            Return "⁷"
        End If
        If strVal = "8" Then
            Return "⁸"
        End If
        If strVal = "9" Then
            Return "⁹"
        End If
        If strVal = "+" Then
            Return "⁺"
        End If
        If strVal = "-" Then
            Return "⁻"
        End If
        Return ""
    End Function
    'Public Function GenerateRowsasColumn(ByVal Report As XtraReport, Optional ByVal intNumberofColumns As Integer = 8) As XtraReport
    '    Try
    '        Dim ds As DataSet = Report.DataSource
    '        Dim dtTable As DataTable = ds.Tables(0)
    '        If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then

    '            Dim dtParameterDetails As DataTable = dtTable.DefaultView.ToTable(True, Split(strRowColumn, ", "))
    '            Dim dtSampleDetails As DataTable = dtTable.DefaultView.ToTable(True, Split(strNormalColumn, ", "))
    '            If Not dtParameterDetails Is Nothing AndAlso dtParameterDetails.Rows.Count > 0 Then
    '                Dim intParam As Integer = 0
    '                Dim intPageCount As Integer = 0
    '                For Each drParamRow As DataRow In dtParameterDetails.Rows
    '                    intParam = intParam + 1
    '                    For Each drSampleRows As DataRow In dtSampleDetails.Rows
    '                        Dim strFilter As String = setFilter(drSampleRows, intPageCount)
    '                        If intParam = 1 Then
    '                            Dim drrRowExists() As DataRow = dtTableColumn.Select(strFilter)
    '                            If Not drrRowExists Is Nothing AndAlso drrRowExists.Length > 0 Then
    '                            Else
    '                                Dim drNewRow As DataRow = dtTableColumn.NewRow
    '                                drNewRow("PageIndex") = intPageCount
    '                                If Not strNormalColumn Is Nothing AndAlso Len(strNormalColumn) > 0 Then
    '                                    Dim strSplit() As String = Split(strNormalColumn, ", ")
    '                                    If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
    '                                        For intC As Integer = 0 To strSplit.Length - 1
    '                                            drNewRow(strSplit(intC)) = drSampleRows(strSplit(intC))
    '                                        Next
    '                                    End If
    '                                End If
    '                                If Not strRowColumn Is Nothing AndAlso Len(strRowColumn) > 0 Then
    '                                    Dim strSplit() As String = Split(strRowColumn, ", ")
    '                                    If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
    '                                        Dim drrResult() As DataRow = dtTable.Select(GetResult(drParamRow, drSampleRows))
    '                                        If Not drrResult Is Nothing AndAlso drrResult.Length > 0 Then
    '                                            For intC As Integer = 0 To strSplit.Length - 1
    '                                                drNewRow(strSplit(intC) & intParam) = drrResult(0)(strSplit(intC))
    '                                            Next
    '                                        Else 'To fill the parameter name which is not in this Samples. Except the result, we are filling other columns to show the caption
    '                                            For intC As Integer = 0 To strSplit.Length - 1
    '                                                drNewRow(strSplit(intC) & intParam) = drParamRow(strSplit(intC))
    '                                            Next
    '                                        End If
    '                                    End If
    '                                End If
    '                                Dim drrResult1() As DataRow = dtTable.Select(GetResult(drParamRow, drSampleRows))
    '                                If Not drrResult1 Is Nothing AndAlso drrResult1.Length > 0 Then
    '                                    If drNewRow.Table.Columns.Contains("Result" & intParam) Then
    '                                        drNewRow("Result" & intParam) = drrResult1(0)("Result")
    '                                    End If
    '                                    If drNewRow.Table.Columns.Contains("FinalResult" & intParam) Then
    '                                        drNewRow("FinalResult" & intParam) = drrResult1(0)("Result")
    '                                    End If
    '                                End If
    '                                dtTableColumn.Rows.Add(drNewRow)
    '                            End If
    '                        Else
    '                            Dim drrExistingRows() As DataRow = dtTableColumn.Select(IIf(Len(strFilter) > 0, strFilter, ""))
    '                            If Not drrExistingRows Is Nothing AndAlso drrExistingRows.Length > 0 Then
    '                                If Not strNormalColumn Is Nothing AndAlso Len(strNormalColumn) > 0 Then
    '                                    Dim strSplit() As String = Split(strNormalColumn, ", ")
    '                                    If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
    '                                        For intC As Integer = 0 To strSplit.Length - 1
    '                                            drrExistingRows(0)(strSplit(intC)) = drSampleRows(strSplit(intC))
    '                                        Next
    '                                    End If
    '                                End If
    '                                If Not strRowColumn Is Nothing AndAlso Len(strRowColumn) > 0 Then
    '                                    Dim strSplit() As String = Split(strRowColumn, ", ")
    '                                    If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
    '                                        Dim drrResult() As DataRow = dtTable.Select(GetResult(drParamRow, drSampleRows))
    '                                        If Not drrResult Is Nothing AndAlso drrResult.Length > 0 Then
    '                                            For intC As Integer = 0 To strSplit.Length - 1
    '                                                drrExistingRows(0)(strSplit(intC) & intParam) = drrResult(0)(strSplit(intC))
    '                                            Next
    '                                        Else 'To fill the parameter name which is not in this Samples. Except the result, we are filling other columns to show the caption
    '                                            For intC As Integer = 0 To strSplit.Length - 1
    '                                                drrExistingRows(0)(strSplit(intC) & intParam) = drParamRow(strSplit(intC))
    '                                            Next
    '                                        End If
    '                                    End If
    '                                End If
    '                                Dim drrResult1() As DataRow = dtTable.Select(GetResult(drParamRow, drSampleRows))
    '                                If Not drrResult1 Is Nothing AndAlso drrResult1.Length > 0 Then
    '                                    If drrExistingRows(0).Table.Columns.Contains("Result" & intParam) Then
    '                                        drrExistingRows(0)("Result" & intParam) = drrResult1(0)("Result")
    '                                    End If
    '                                    If drrExistingRows(0).Table.Columns.Contains("FinalResult" & intParam) Then
    '                                        drrExistingRows(0)("FinalResult" & intParam) = drrResult1(0)("Result")
    '                                    End If
    '                                End If
    '                            End If
    '                        End If
    '                    Next
    '                    If (intParam Mod intNumberofColumns) = 0 Then
    '                        intParam = 0
    '                        intPageCount = intPageCount + 1
    '                    End If
    '                Next
    '            End If
    '        End If
    '        Report.DataSource = Nothing
    '        Report.DataMember = Nothing
    '        Report.DataSource = dtTableColumn
    '        Return Report
    '    Catch ex As Exception
    '        Return Report
    '    End Try
    'End Function

    'Public Function GenerateRowsasColumn(ByVal dtTable As DataTable) As DataTable
    '    If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then

    '        Dim dtParameterDetails As DataTable = dtTable.DefaultView.ToTable(True, Split(strRowColumn, ", "))
    '        Dim dtSampleDetails As DataTable = dtTable.DefaultView.ToTable(True, Split(strNormalColumn, ", "))
    '        If Not dtParameterDetails Is Nothing AndAlso dtParameterDetails.Rows.Count > 0 Then
    '            Dim intParam As Integer = 0
    '            Dim intPageCount As Integer = 0
    '            For Each drParamRow As DataRow In dtParameterDetails.Rows
    '                intParam = intParam + 1
    '                For Each drSampleRows As DataRow In dtSampleDetails.Rows
    '                    Dim strFilter As String = setFilter(drSampleRows, intPageCount)
    '                    If intParam = 1 Then
    '                        Dim drNewRow As DataRow = dtTableColumn.NewRow
    '                        drNewRow("PageIndex") = intPageCount
    '                        If Not strNormalColumn Is Nothing AndAlso Len(strNormalColumn) > 0 Then
    '                            Dim strSplit() As String = Split(strNormalColumn, ", ")
    '                            If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
    '                                For intC As Integer = 0 To strSplit.Length - 1
    '                                    drNewRow(strSplit(intC)) = drSampleRows(strSplit(intC))
    '                                Next
    '                            End If
    '                        End If
    '                        If Not strRowColumn Is Nothing AndAlso Len(strRowColumn) > 0 Then
    '                            Dim strSplit() As String = Split(strRowColumn, ", ")
    '                            If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
    '                                Dim drrResult() As DataRow = dtTable.Select(GetResult(drParamRow, drSampleRows))
    '                                If Not drrResult Is Nothing AndAlso drrResult.Length > 0 Then
    '                                    For intC As Integer = 0 To strSplit.Length - 1
    '                                        drNewRow(strSplit(intC) & intParam) = drrResult(0)(strSplit(intC))
    '                                    Next
    '                                End If
    '                            End If
    '                        End If
    '                        Dim drrResult1() As DataRow = dtTable.Select(GetResult(drParamRow, drSampleRows))
    '                        If Not drrResult1 Is Nothing AndAlso drrResult1.Length > 0 Then
    '                            drNewRow("Result" & intParam) = drrResult1(0)("Result")
    '                        End If
    '                        dtTableColumn.Rows.Add(drNewRow)
    '                    Else
    '                        Dim drrExistingRows() As DataRow = dtTableColumn.Select(IIf(Len(strFilter) > 0, strFilter & " and ", "") & " PageIndex = " & intPageCount & "")
    '                        If Not drrExistingRows Is Nothing AndAlso drrExistingRows.Length > 0 Then
    '                            If Not strNormalColumn Is Nothing AndAlso Len(strNormalColumn) > 0 Then
    '                                Dim strSplit() As String = Split(strNormalColumn, ", ")
    '                                If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
    '                                    For intC As Integer = 0 To strSplit.Length - 1
    '                                        drrExistingRows(0)(strSplit(intC)) = drSampleRows(strSplit(intC))
    '                                    Next
    '                                End If
    '                            End If
    '                            If Not strRowColumn Is Nothing AndAlso Len(strRowColumn) > 0 Then
    '                                Dim strSplit() As String = Split(strRowColumn, ", ")
    '                                If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
    '                                    Dim drrResult() As DataRow = dtTable.Select(GetResult(drParamRow, drSampleRows))
    '                                    If Not drrResult Is Nothing AndAlso drrResult.Length > 0 Then
    '                                        For intC As Integer = 0 To strSplit.Length - 1
    '                                            drrExistingRows(0)(strSplit(intC) & intParam) = drrResult(0)(strSplit(intC))
    '                                        Next
    '                                    End If
    '                                End If
    '                            End If
    '                            Dim drrResult1() As DataRow = dtTable.Select(GetResult(drParamRow, drSampleRows))
    '                            If Not drrResult1 Is Nothing AndAlso drrResult1.Length > 0 Then
    '                                drrExistingRows(0)("Result" & intParam) = drrResult1(0)("Result")
    '                            End If
    '                        End If
    '                    End If
    '                Next
    '                If (intParam Mod 3) = 0 Then
    '                    intParam = 0
    '                    intPageCount = intPageCount + 1
    '                End If
    '            Next
    '        End If
    '    End If
    '    Return dtTableColumn
    'End Function
    Public Function SetFormatNumberCN(ByVal dbNumber As Double, ByVal iNoOfFraction As Integer) As String
        Try
            ' Return FormatNumberCN(dbNumber, iNoOfFraction)
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Private Function setFilter(ByVal drSampleRows As DataRow, ByVal intPageCount As Integer) As String
        Dim strFilter As String = String.Empty
        If Not drSampleRows Is Nothing AndAlso Not strUniqueColumn Is Nothing AndAlso Len(strUniqueColumn) > 0 Then
            Dim strUnique() As String = Split(strUniqueColumn, ", ")
            If Not strUnique Is Nothing AndAlso strUnique.Length > 0 Then
                If intPageCount > 0 Then
                    strFilter = "PageIndex" & " = '" & intPageCount & "'"
                End If
                For intstr As Integer = 0 To strUnique.Length - 1
                    If Not IsDBNull(drSampleRows(strUnique(intstr))) AndAlso Len(drSampleRows(strUnique(intstr))) > 0 Then
                        If Len(strFilter) > 0 Then
                            strFilter = strFilter & " and " & strUnique(intstr) & " = '" & drSampleRows(strUnique(intstr)) & "'"
                        Else
                            strFilter = strUnique(intstr) & " = '" & drSampleRows(strUnique(intstr)) & "'"
                        End If
                    End If
                Next
            End If
        End If

        Return strFilter
    End Function

    Private Function GetResult(ByVal drParamRow As DataRow, ByVal drSampleRows As DataRow) As String
        Dim strFilter As String = String.Empty
        If Not drSampleRows Is Nothing AndAlso Not strUniqueColumn Is Nothing AndAlso Len(strUniqueColumn) > 0 Then
            Dim strUnique() As String = Split(strUniqueColumn, ", ")
            If Not strUnique Is Nothing AndAlso strUnique.Length > 0 Then
                For intstr As Integer = 0 To strUnique.Length - 1
                    If Not IsDBNull(drSampleRows(strUnique(intstr))) AndAlso Len(drSampleRows(strUnique(intstr))) > 0 Then
                        If Len(strFilter) > 0 Then
                            strFilter = strFilter & " and " & strUnique(intstr) & " = '" & drSampleRows(strUnique(intstr)) & "'"
                        Else
                            strFilter = strUnique(intstr) & " = '" & drSampleRows(strUnique(intstr)) & "'"
                        End If
                    End If
                Next
            End If
        End If
        If Not drParamRow Is Nothing AndAlso Not strRowColumn Is Nothing AndAlso Len(strRowColumn) > 0 Then
            Dim strRow() As String = Split(strRowColumn, ", ")
            If Not strRow Is Nothing AndAlso strRow.Length > 0 Then
                For intstr As Integer = 0 To strRow.Length - 1
                    If Not IsDBNull(drParamRow(strRow(intstr))) AndAlso Len(drParamRow(strRow(intstr))) > 0 Then
                        If Len(strFilter) > 0 Then
                            strFilter = strFilter & " and " & strRow(intstr) & " = '" & drParamRow(strRow(intstr)) & "'"
                        Else
                            strFilter = strRow(intstr) & " = '" & drParamRow(strRow(intstr)) & "'"
                        End If
                    End If
                Next
            End If
        End If

        Return strFilter
    End Function

    'Public Function CollectAnalyzedBy(ByVal dtTable As DataTable, ByVal strAnalyzedByColumnName As String) As String
    '    Dim strAnalyzedBy As String = String.Empty
    '    Dim arrAnalyzedByExists As New ArrayList
    '    If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then
    '        For Each drRow As DataRow In dtTable.Rows
    '            If Not IsDBNull(drRow(strAnalyzedByColumnName)) AndAlso Len(drRow(strAnalyzedByColumnName)) > 0 Then
    '                If Not arrAnalyzedByExists Is Nothing AndAlso arrAnalyzedByExists.IndexOf(drRow(strAnalyzedByColumnName)) < 0 Then
    '                    arrAnalyzedByExists.Add(drRow(strAnalyzedByColumnName))
    '                    If Not strAnalyzedBy Is Nothing AndAlso Len(strAnalyzedBy) > 0 Then
    '                        strAnalyzedBy = strAnalyzedBy & ", " & drRow(strAnalyzedByColumnName)
    '                    Else
    '                        strAnalyzedBy = drRow(strAnalyzedByColumnName)
    '                    End If
    '                End If
    '            End If
    '        Next
    '    End If
    '    Return strAnalyzedBy
    'End Function

    'Public Function CollectQCBtachID(ByVal dtTable As DataTable, ByVal strQCBatchColumnName As String) As String
    '    Dim strQCBatch As String = String.Empty
    '    Dim arrQCBatchExists As New ArrayList
    '    If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then
    '        For Each drRow As DataRow In dtTable.Rows
    '            If Not IsDBNull(drRow(strQCBatchColumnName)) AndAlso Len(drRow(strQCBatchColumnName)) > 0 Then
    '                If Not arrQCBatchExists Is Nothing AndAlso arrQCBatchExists.IndexOf(drRow(strQCBatchColumnName)) < 0 Then
    '                    arrQCBatchExists.Add(drRow(strQCBatchColumnName))
    '                    If Not strQCBatch Is Nothing AndAlso Len(strQCBatch) > 0 Then
    '                        strQCBatch = strQCBatch & ", " & drRow(strQCBatchColumnName)
    '                    Else
    '                        strQCBatch = drRow(strQCBatchColumnName)
    '                    End If
    '                End If
    '            End If
    '        Next
    '    End If
    '    Return strQCBatch
    'End Function

    Public Function CollectTextSource(ByVal dtTable As DataTable, ByVal strColumnName As String) As String
        Dim strText As String = String.Empty
        Dim arrTextExists As New ArrayList
        If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then
            For Each drRow As DataRow In dtTable.Rows
                If Not IsDBNull(drRow(strColumnName)) AndAlso Len(drRow(strColumnName)) > 0 Then
                    If Not arrTextExists Is Nothing AndAlso arrTextExists.IndexOf(drRow(strColumnName)) < 0 Then
                        arrTextExists.Add(drRow(strColumnName))
                        If Not strText Is Nothing AndAlso Len(strText) > 0 Then
                            strText = strText & ", " & drRow(strColumnName)
                        Else
                            strText = drRow(strColumnName)
                        End If
                    End If
                End If
            Next
        End If
        Return strText
    End Function
    Public Function CollectTextSourceWithSoOn(ByVal dtTable As DataTable, ByVal strColumnName As String) As String
        Dim strText As String = String.Empty
        Dim arrTextExists As New ArrayList
        If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then
            For Each drRow As DataRow In dtTable.Rows
                If Not IsDBNull(drRow(strColumnName)) AndAlso Len(drRow(strColumnName)) > 0 Then
                    If Not arrTextExists Is Nothing AndAlso arrTextExists.IndexOf(drRow(strColumnName)) < 0 Then
                        If arrTextExists.Count = 1 Then
                            If Not strText Is Nothing AndAlso Len(strText) > 0 Then
                                strText = strText & ReportMultilanguage.MultiLanguageModule.GetCurrentLanguageConstant("So On")
                                Return strText
                            End If
                        Else
                            arrTextExists.Add(drRow(strColumnName))
                            strText = drRow(strColumnName)
                        End If
                    End If
                End If
            Next
        End If
        Return strText
    End Function
    Public Function CollectDateMinandMaxValue(ByVal dtTable As DataTable, ByVal strColumnName As String, Optional ByVal FilterColumn As String = "", Optional ByVal FilterColumnValue As String = "") As String
        Dim strText As String = String.Empty
        Dim arrTextExists As New ArrayList
        Dim MinMaxDateValue As String = String.Empty
        Dim MinDateValue As String = String.Empty
        Dim MaxDateValue As String = String.Empty
        Dim minDate As DateTime
        Dim maxDate As DateTime

        If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then
            Dim dv As DataView = dtTable.DefaultView
            dv.Sort = strColumnName & " " & "asc"
            If FilterColumn.Length > 0 AndAlso FilterColumnValue.Length Then
                Dim drrRow() As DataRow = dv.ToTable.Select(FilterColumn & " = '" & (FilterColumnValue) & "'")
                Dim intCount1 As Integer = 0
                intCount1 = drrRow.Length
                If drrRow.Length > 0 Then
                    If Not IsDBNull(drrRow(0)(strColumnName)) AndAlso Not (drrRow(0)(strColumnName)) Is Nothing AndAlso Len(drrRow(0)(strColumnName)) > 0 Then
                        minDate = (drrRow(0)(strColumnName))
                        MinDateValue = DateTimeFormatWithoutAMPM_Report(minDate)
                    End If
                    If Not IsDBNull(drrRow(intCount1 - 1)(strColumnName)) AndAlso Not (drrRow(intCount1 - 1)(strColumnName)) Is Nothing AndAlso Len(drrRow(intCount1 - 1)(strColumnName)) > 0 Then
                        maxDate = (drrRow(intCount1 - 1)(strColumnName))
                        MaxDateValue = DateTimeFormatWithoutAMPM_Report(maxDate)
                    End If
                    If Not IsDBNull(MinDateValue) AndAlso Len(MinDateValue) > 0 AndAlso Not IsDBNull(MaxDateValue) AndAlso Len(MaxDateValue) > 0 Then
                        If MinDateValue = MaxDateValue Then
                            MinMaxDateValue = MaxDateValue
                        Else
                            MinMaxDateValue = (MinDateValue & " - " & MaxDateValue)
                        End If
                    ElseIf Not IsDBNull(MaxDateValue) AndAlso Len(MaxDateValue) > 0 Then
                        MinMaxDateValue = MaxDateValue
                    ElseIf Not IsDBNull(MinDateValue) AndAlso Len(MinDateValue) > 0 Then
                        MinMaxDateValue = MinMaxDateValue
                    End If

                End If
            Else
                Dim dtFilterSampleID As DataTable = dv.ToTable()
                Dim IntCount As Integer = 0
                If Not IsDBNull(dv.ToTable.Rows(IntCount)(strColumnName)) AndAlso Not (dv.ToTable.Rows(IntCount)(strColumnName)) Is Nothing AndAlso Len(dv.ToTable.Rows(IntCount)(strColumnName)) > 0 Then
                    minDate = (dv.ToTable.Rows(IntCount)(strColumnName))
                    MinDateValue = DateTimeFormatWithoutAMPM_Report(minDate)
                End If
                If Not IsDBNull(dv.ToTable.Rows(dv.ToTable.Rows.Count - 1)(strColumnName)) AndAlso Not (dv.ToTable.Rows(dv.ToTable.Rows.Count - 1)(strColumnName)) Is Nothing AndAlso Len(dv.ToTable.Rows(dv.ToTable.Rows.Count - 1)(strColumnName)) > 0 Then
                    maxDate = (dv.ToTable.Rows(dv.ToTable.Rows.Count - 1)(strColumnName))
                    MaxDateValue = DateTimeFormatWithoutAMPM_Report(maxDate)
                End If
                If Not IsDBNull(MinDateValue) AndAlso Len(MinDateValue) > 0 AndAlso Not IsDBNull(MaxDateValue) AndAlso Len(MaxDateValue) > 0 Then
                    If MinDateValue = MaxDateValue Then
                        MinMaxDateValue = MaxDateValue
                    Else
                        MinMaxDateValue = (MinDateValue & " - " & MaxDateValue)
                    End If
                ElseIf Not IsDBNull(MaxDateValue) AndAlso Len(MaxDateValue) > 0 Then
                    MinMaxDateValue = MaxDateValue
                ElseIf Not IsDBNull(MinDateValue) AndAlso Len(MinDateValue) > 0 Then
                    MinMaxDateValue = MinMaxDateValue
                End If
            End If
        End If
        Return MinMaxDateValue
    End Function

    Public Function CollectRemark(ByVal dtTable As DataTable, ByVal strRemarkColumnName As String, Optional ByVal bolCommentWithID As Boolean = True) As String
        Dim strComment As String = String.Empty
        Dim arrCommentExists As New ArrayList
        If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then
            For Each drRow As DataRow In dtTable.Rows
                If Not IsDBNull(drRow(strRemarkColumnName)) AndAlso Len(drRow(strRemarkColumnName)) > 0 Then
                    If bolCommentWithID = True Then
                        If dtTable.Columns.Contains("SampleID") = True AndAlso dtTable.Columns.Contains("QCBatchID") = True AndAlso dtTable.Columns.Contains("RunType") = True Then
                            If Not IsDBNull(drRow("SampleID")) AndAlso Len(drRow("SampleID")) > 0 Then
                                If Not arrCommentExists Is Nothing AndAlso arrCommentExists.IndexOf(drRow("SampleID") & "  " & drRow(strRemarkColumnName)) < 0 Then
                                    arrCommentExists.Add(drRow("SampleID") & "  " & drRow(strRemarkColumnName))
                                    If Not strComment Is Nothing AndAlso Len(strComment) > 0 Then
                                        strComment = strComment & vbNewLine & drRow("SampleID") & "  " & drRow(strRemarkColumnName)
                                    Else
                                        strComment = drRow("SampleID") & "  " & drRow(strRemarkColumnName)
                                    End If
                                End If
                            ElseIf Not IsDBNull(drRow("QCBatchID")) AndAlso Len(drRow("QCBatchID")) > 0 AndAlso Not IsDBNull(drRow("RunType")) AndAlso Len(drRow("RunType")) > 0 Then
                                If Not arrCommentExists Is Nothing AndAlso arrCommentExists.IndexOf(drRow("QCBatchID") & "  " & drRow("RunType") & "  " & drRow(strRemarkColumnName)) < 0 Then
                                    arrCommentExists.Add(drRow("QCBatchID") & "  " & drRow("RunType") & "  " & drRow(strRemarkColumnName))
                                    If Not strComment Is Nothing AndAlso Len(strComment) > 0 Then
                                        strComment = strComment & vbNewLine & drRow("QCBatchID") & "  " & drRow("RunType") & "  " & drRow(strRemarkColumnName)
                                    Else
                                        strComment = drRow("QCBatchID") & "  " & drRow("RunType") & "  " & drRow(strRemarkColumnName)
                                    End If
                                End If
                            Else
                                If Not arrCommentExists Is Nothing AndAlso arrCommentExists.IndexOf(drRow(strRemarkColumnName)) < 0 Then
                                    arrCommentExists.Add(drRow(strRemarkColumnName))
                                    If Not strComment Is Nothing AndAlso Len(strComment) > 0 Then
                                        strComment = strComment & vbNewLine & drRow(strRemarkColumnName)
                                    Else
                                        strComment = drRow(strRemarkColumnName)
                                    End If
                                End If
                            End If
                        Else
                            If Not arrCommentExists Is Nothing AndAlso arrCommentExists.IndexOf(drRow(strRemarkColumnName)) < 0 Then
                                arrCommentExists.Add(drRow(strRemarkColumnName))
                                If Not strComment Is Nothing AndAlso Len(strComment) > 0 Then
                                    strComment = strComment & vbNewLine & drRow(strRemarkColumnName)
                                Else
                                    strComment = drRow(strRemarkColumnName)
                                End If
                            End If
                        End If
                    Else
                        If Not arrCommentExists Is Nothing AndAlso arrCommentExists.IndexOf(drRow(strRemarkColumnName)) < 0 Then
                            arrCommentExists.Add(drRow(strRemarkColumnName))
                            If Not strComment Is Nothing AndAlso Len(strComment) > 0 Then
                                strComment = strComment & vbNewLine & drRow(strRemarkColumnName)
                            Else
                                strComment = drRow(strRemarkColumnName)
                            End If
                        End If
                    End If
                End If
            Next
        End If
        Return strComment
    End Function
    'Public Sub AssignApprovedByUserSign(ByVal picApprovedBy As DevExpress.XtraReports.UI.XRPictureBox)
    '    Try
    '        Dim dtSign As DataTable = BusinessLayer.BLCommon.CurrentUserSign(strApprovedBy)
    '        If Not dtSign Is Nothing AndAlso dtSign.Rows.Count > 0 Then
    '            If Not picApprovedBy Is Nothing AndAlso Not IsDBNull(dtSign.Rows(0)("LoginUserSign")) Then
    '                Dim sig As Byte() = dtSign.Rows(0)("LoginUserSign")
    '                Dim mem As MemoryStream = New MemoryStream(sig)
    '                picApprovedBy.Image = New Bitmap(mem)
    '            End If
    '        End If
    '    Catch ex As Exception
    '    End Try
    'End Sub
    'Public Sub AssignUserSign(ByVal picUser As DevExpress.XtraReports.UI.XRPictureBox, ByVal UserName As String)
    '    Try
    '        Dim dtSign As DataTable = BusinessLayer.BLCommon.CurrentUserSign(UserName)
    '        If Not dtSign Is Nothing AndAlso dtSign.Rows.Count > 0 Then
    '            If Not picUser Is Nothing AndAlso Not IsDBNull(dtSign.Rows(0)("LoginUserSign")) Then
    '                Dim sig As Byte() = dtSign.Rows(0)("LoginUserSign")
    '                Dim mem As MemoryStream = New MemoryStream(sig)
    '                picUser.Image = New Bitmap(mem)
    '            End If
    '        End If
    '    Catch ex As Exception
    '    End Try
    'End Sub
    'Public Sub AssignUserSign(ByVal picUser As DevExpress.XtraReports.UI.XRPictureBox, ByVal dtTable As DataTable, ByVal strColumnName As String, Optional ByVal lblSoOn As DevExpress.XtraReports.UI.XRLabel = Nothing)
    '    Try
    '        If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then
    '            If Not strColumnName Is Nothing AndAlso Len(strColumnName) > 0 AndAlso dtTable.Columns.Contains(strColumnName) = True Then
    '                Dim dtUserFilter As DataTable = New DataView(dtTable, strColumnName & " is not Null and len(" & strColumnName & ") > 0", "", DataViewRowState.CurrentRows).ToTable(True, strColumnName)
    '                If Not dtUserFilter Is Nothing AndAlso dtUserFilter.Rows.Count > 0 Then
    '                    Dim dtSign As DataTable = BusinessLayer.BLCommon.CurrentUserSign(dtUserFilter.Rows(0)(strColumnName))
    '                    If Not dtSign Is Nothing AndAlso dtSign.Rows.Count > 0 Then
    '                        If Not picUser Is Nothing AndAlso Not IsDBNull(dtSign.Rows(0)("LoginUserSign")) Then
    '                            Dim sig As Byte() = dtSign.Rows(0)("LoginUserSign")
    '                            Dim mem As MemoryStream = New MemoryStream(sig)
    '                            picUser.Image = New Bitmap(mem)
    '                        End If
    '                    End If
    '                    If dtUserFilter.Rows.Count > 1 Then
    '                        lblSoOn.Visible = True
    '                    End If
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception
    '    End Try
    'End Sub
    Public Function GetLoginUserSign() As DataTable
        Try
            Dim UserNameSig As String
            UserNameSig = "select (colFirstName) + ' ' + (colLastName) as UserName,colPosition as Position , colsignatureimg as Signature from tbl_User where colUserName =N'" & UserName & " '"
            Dim dttable As DataTable = DynamicReportBusinessLayer.BLCommon.ExecuteDataTable(UserNameSig)
            Return dttable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function GetUserSign(ByVal strUserName As String) As DataTable
        Try
            Dim UserNameSig As String
            Dim UserName As String
            ' UserNameSig = "select (colFirstName) + ' ' + (colLastName) as UserName,colPosition as Position , colsignatureimg as Signature from tbl_User where colUserName =N'" & strUserName & " '"
            UserNameSig = "select (FirstName) + ' ' + (LastName) as UserName,(select top 1 Position from Position where oid=Position) as Position , Signature as Signature from Employee where Oid ='" & strUserName & "'"

            Dim dt As DataTable = ExecuteDataTable(UserNameSig)

            If dt.Rows.Count > 0 Then

                UserName = dt.Rows(0)("UserName").ToString()
            End If

            Dim dttable As DataTable = DynamicReportBusinessLayer.BLCommon.CurrentUserSign(UserName)
            Return dttable
        Catch ex As Exception
            Return Nothing
        End Try
    End Function


    Public Sub AssignUserSign(ByVal picAnalyzedBy As DevExpress.XtraReports.UI.XRPictureBox, ByVal picValidatedBy As DevExpress.XtraReports.UI.XRPictureBox, ByVal picApprovedBy As DevExpress.XtraReports.UI.XRPictureBox, ByVal lblAnalyzedBySoOn As DevExpress.XtraReports.UI.XRLabel)

        Dim dsSign As DataSet = DynamicReportBusinessLayer.BLCommon.Public_UserSign(struqSampleParameterID)
        If Not dsSign Is Nothing AndAlso dsSign.Tables.Count > 0 AndAlso dsSign.Tables(0).Rows.Count > 0 Then
            Try
                If Not picAnalyzedBy Is Nothing AndAlso Not IsDBNull(dsSign.Tables(0).Rows(0)("AnalyzedBySign")) Then
                    Dim sig As Byte() = dsSign.Tables(0).Rows(0)("AnalyzedBySign")
                    Dim mem As MemoryStream = New MemoryStream(sig)
                    picAnalyzedBy.Image = New Bitmap(mem)
                End If
            Catch ex As Exception
            End Try
            Try
                If Not picValidatedBy Is Nothing AndAlso Not IsDBNull(dsSign.Tables(0).Rows(0)("ValidatedBySign")) Then
                    Dim sig As Byte() = dsSign.Tables(0).Rows(0)("ValidatedBySign")
                    Dim mem As MemoryStream = New MemoryStream(sig)
                    picValidatedBy.Image = New Bitmap(mem)
                End If
            Catch ex As Exception
            End Try
            Try
                If Not picApprovedBy Is Nothing AndAlso Not IsDBNull(dsSign.Tables(0).Rows(0)("ApprovedBySign")) Then
                    Dim sig As Byte() = dsSign.Tables(0).Rows(0)("ApprovedBySign")
                    Dim mem As MemoryStream = New MemoryStream(sig)
                    picApprovedBy.Image = New Bitmap(mem)
                End If
            Catch ex As Exception
            End Try
        End If
        If Not lblAnalyzedBySoOn Is Nothing Then
            If Not dsSign Is Nothing AndAlso dsSign.Tables.Count > 1 AndAlso dsSign.Tables(1).Rows.Count > 1 Then
                lblAnalyzedBySoOn.Visible = True
            Else
                lblAnalyzedBySoOn.Visible = False
            End If
        End If
    End Sub


    'Public Sub AssignUserSign(ByVal picUser As DevExpress.XtraReports.UI.XRPictureBox, ByVal UserName As String)
    '    Try
    '        Dim dtSign As DataTable = DynamicReportBusinessLayer.BLCommon.CurrentUserSign(UserName)
    '        If dtSign IsNot Nothing AndAlso dtSign.Rows.Count > 0 Then
    '            If picUser IsNot Nothing AndAlso dtSign.Rows(0)("Signature") IsNot Nothing AndAlso dtSign.Rows(0)("Signature").ToString().Length > 0 Then
    '                Dim sig As Byte() = CType(dtSign.Rows(0)("Signature"), Byte())
    '                Dim mem As MemoryStream = New MemoryStream(sig)
    '                picUser.Image = New Bitmap(mem)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message)
    '    End Try
    'End Sub

    Public Sub AssignUserSign(ByVal picUser As DevExpress.XtraReports.UI.XRPictureBox, ByVal UserName As String)
        Try
            Dim dtSign As DataTable = DynamicReportBusinessLayer.BLCommon.CurrentUserSign(UserName)
            If Not dtSign Is Nothing AndAlso dtSign.Rows.Count > 0 Then
                If Not picUser Is Nothing AndAlso Not IsDBNull(dtSign.Rows(0)("LoginUserSign")) Then
                    Dim sig As Byte() = dtSign.Rows(0)("LoginUserSign")
                    Dim mem As MemoryStream = New MemoryStream(sig)
                    picUser.Image = New Bitmap(mem)
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Function AssignSDMSDataSource(ByVal xtraRep As XtraReport) As XtraReport
        Try
            For Each band As DevExpress.XtraReports.UI.Band In xtraRep.Bands
                For Each control As DevExpress.XtraReports.UI.XRControl In band

                    If control.[GetType]() = GetType(DevExpress.XtraReports.UI.XRTable) Then
                        Dim table As DevExpress.XtraReports.UI.XRTable = CType(control, DevExpress.XtraReports.UI.XRTable)

                        For Each row As DevExpress.XtraReports.UI.XRTableRow In table
                            For Each cell As DevExpress.XtraReports.UI.XRTableCell In row
                                If cell.Tag.ToString() = "AnalyzedDate" Then
                                    cell.Text = strAnalyzedDate
                                ElseIf cell.Tag.ToString() = "AnalyzedBy" Then
                                    cell.Text = strAnalyzedBy
                                ElseIf cell.Tag.ToString() = "ReviewedDate" Then
                                    cell.Text = strReviewedDate
                                ElseIf cell.Tag.ToString() = "ReviewedBy" Then
                                    cell.Text = strReviewedBy
                                ElseIf cell.Tag.ToString() = "VerifiedDate" Then
                                    cell.Text = strVerifiedDate
                                ElseIf cell.Tag.ToString() = "VerifiedBy" Then
                                    cell.Text = strVerifiedBy
                                End If
                            Next
                        Next
                    ElseIf control.[GetType]() = GetType(DevExpress.XtraReports.UI.XRLabel) Or control.[GetType]() = GetType(DevExpress.XtraReports.UI.XRRichText) Then
                        If control.Tag.ToString() = "AnalyzedDate" Then
                            control.Text = strAnalyzedDate
                        ElseIf control.Tag.ToString() = "AnalyzedBy" Then
                            control.Text = strAnalyzedBy
                        ElseIf control.Tag.ToString() = "ReviewedDate" Then
                            control.Text = strReviewedDate
                        ElseIf control.Tag.ToString() = "ReviewedBy" Then
                            control.Text = strReviewedBy
                        ElseIf control.Tag.ToString() = "VerifiedDate" Then
                            control.Text = strVerifiedDate
                        ElseIf control.Tag.ToString() = "VerifiedBy" Then
                            control.Text = strVerifiedBy
                        End If
                    End If
                Next
            Next

            Return xtraRep
        Catch ex As Exception
            Return Nothing
        End Try

    End Function

    Public Function AssignSubReport(ByVal strSubReportName As String, ByVal Optional bolSDMSDataSource As Boolean = False) As XtraReport
        Try
            Dim Report As XtraReport = GetSubReportFromLayOut(strSubReportName)
            'If bolSDMSDataSource = True Then
            '    AssignSDMSDataSource(Report)
            'End If
            Return Report
        Catch ex As Exception
        End Try
    End Function

    Public Function AssignSDMSImages(ByVal strABID As String) As DataTable
        Try
            Return DynamicReportBusinessLayer.BLCommon.GetSDMSImages(strABID)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    'Public Sub AssignJoinSDMSImages(ByVal xrReport As XtraReport, ByVal strABID As String, ByVal strBandName As String, strControlName As String)
    '    Try
    '        'creating picture box
    '        ' Dim strString As String = "SELECT u.colSignatureImg FROM dbo.tbl_User u where u.colUserID IN ( SELECT DISTINCT colUserID FROM tbl_ENV_JoinAnalyzedBy WHERE coluqSampleParameterID IN (" + struqSampleParameterID + ")) AND u.colSignatureImg IS NOT NULL"
    '        Dim dtTable As DataTable = DynamicReportBusinessLayer.BLCommon.GetSDMSImages(strABID)
    '        If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then
    '            Dim IntHerizontal As Integer = 0
    '            Dim IntCommaHerizontal As Integer = 0
    '            Dim IntVertical As Integer = 0
    '            Dim bolNextRow As Boolean = False
    '            Dim IntIndex As Integer = 1


    '            For Each dr As DataRow In dtTable.Rows

    '                Dim xrPicImage As XRPictureBox = New XRPictureBox()
    '                If Not xrPicImage Is Nothing AndAlso Not IsDBNull(dr(1)) Then
    '                    Dim sig As Byte() = dr(1)
    '                    Dim mem As MemoryStream = New MemoryStream(sig)
    '                    xrPicImage.Image = New Bitmap(mem)
    '                End If
    '                'If IntIndex = 1 Then
    '                '    IntHerizontal = 613.38
    '                '    IntCommaHerizontal = IntHerizontal + 100
    '                '    IntVertical = 42.92
    '                '    'ElseIf IntIndex = 2 Then
    '                '    '    IntHerizontal = IntCommaHerizontal + 10
    '                '    '    IntCommaHerizontal = IntHerizontal + 100
    '                '    '    IntVertical = 42.92
    '                'ElseIf IntIndex > 1 And IntIndex < 8 Then
    '                '    If bolNextRow = False Then
    '                '        IntHerizontal = 10
    '                '        IntCommaHerizontal = IntHerizontal + 100
    '                '        IntVertical = IntVertical + 34.43
    '                '        bolNextRow = True
    '                '    Else
    '                '        IntHerizontal = IntCommaHerizontal + 10
    '                '        IntCommaHerizontal = IntHerizontal + 100
    '                '    End If
    '                'ElseIf IntIndex = 14 Then
    '                '    bolNextRow = False
    '                'End If

    '                'If IntIndex = 1 Then

    '                'ElseIf IntIndex > 1 And IntIndex < 8 Then
    '                '    If bolNextRow = False Then
    '                '        IntHerizontal = 10
    '                '        IntCommaHerizontal = IntHerizontal + 100
    '                '        IntVertical = IntVertical + 34.43
    '                '        bolNextRow = True
    '                '    Else
    '                '        IntHerizontal = IntCommaHerizontal + 10
    '                '        IntCommaHerizontal = IntHerizontal + 100
    '                '    End If
    '                'ElseIf IntIndex = 14 Then
    '                '    bolNextRow = False
    '                'End If

    '                xrPicImage.LocationF = New Point(0, (IntIndex + 1) * 500 - 250)
    '                'xrPicImage.LocationF = New Point(IntHerizontal, IntVertical)
    '                xrPicImage.SizeF = New System.Drawing.SizeF(100.0F, 50.0F)
    '                xrPicImage.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
    '                xrPicImage.Name = "xrPicImage" + CStr(IntIndex)

    '                With xrReport.Bands(strBandName).Controls.Item(strControlName)
    '                    'If .Controls.[GetType]() = GetType(DevExpress.XtraReports.UI.XRTable) Then
    '                    .Controls.Add(xrPicImage)
    '                    ' End If
    '                End With

    '                IntIndex = IntIndex + 1
    '            Next
    '        End If
    '    Catch ex As Exception
    '    End Try

    'End Sub

    Public Sub AssignMultipleSDMSImages(ByVal xrReport As XtraReport, ByVal strABID As String, ByVal strBandName As String, strControlName As String)
        Try
            Dim dtTable As DataTable = DynamicReportBusinessLayer.BLCommon.GetSDMSImages(strABID)
            If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then
                Dim IntHerizontal As Single = 20.83
                Dim IntVertical As Single = 10.8
                Dim IntIndex As Integer = 1
                For Each dr As DataRow In dtTable.Rows
                    Dim xrPicImage As XRPictureBox = New XRPictureBox()
                    If Not xrPicImage Is Nothing AndAlso Not IsDBNull(dr(1)) Then
                        Dim sig As Byte() = dr(1)
                        Dim mem As MemoryStream = New MemoryStream(sig)
                        xrPicImage.Image = New Bitmap(mem)
                    End If
                    If IntIndex > 1 Then
                        IntVertical = IntVertical + 230.33
                    End If
                    xrPicImage.LocationF = New Point(IntHerizontal, IntVertical)
                    xrPicImage.SizeF = New System.Drawing.SizeF(600.33F, 188.62F)
                    xrPicImage.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
                    'xrPicImage.ImageAlignment = DevExpress.XtraPrinting.ImageAlignment.TopLeft
                    xrPicImage.Name = "xrPicImage" + CStr(IntIndex)
                    With xrReport.Bands(strBandName).Controls.Item(strControlName)
                        .Controls.Add(xrPicImage)
                    End With
                    IntIndex = IntIndex + 1
                Next
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub
    Public Function GetSubReportFromLayOut(ByVal strReportName As String, ByVal Optional bolOnDesign As Boolean = True) As XtraReport
        Try
            Dim strString As String = ("Select Layout,TableSchema,NormalColumn,RowColumn,UniqueColumn,ColumnCount from SpreadSheetBuilder_Reporting Where Name = N'" & strReportName & "'")
            Dim dtTable As New DataTable
            dtTable = BLCommon.GetData(strString)
            If dtTable IsNot Nothing AndAlso dtTable.Rows.Count > 0 AndAlso dtTable.Rows(0)("Layout") IsNot Nothing AndAlso dtTable.Rows(0)("Layout").ToString().Length > 0 Then
                Dim strLayout As String = dtTable.Rows(0)("Layout").ToString()
                Dim Report As XtraReport = New XtraReport()
                Report = GetReport(strLayout)
                Return Report
            End If

            Return Nothing
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Return Nothing
        End Try
    End Function

    Private Function GetReport(ByVal layout As String) As XtraReport
        Dim ms As MemoryStream = New MemoryStream()
        Dim sw As StreamWriter = New StreamWriter(ms, Encoding.UTF8)
        sw.Write(layout.ToCharArray())
        sw.Flush()
        ms.Seek(0, SeekOrigin.Begin)
        Return XtraReport.FromStream(ms, True)
    End Function

    Public Shared Sub GetReportDetails(ByVal dsSampleInfo As DataSet)
        If dsSampleInfo IsNot Nothing AndAlso dsSampleInfo.Tables.Count > 0 Then
            Dim dtSampleInfo As DataTable = dsSampleInfo.Tables(0)
            If dtSampleInfo IsNot Nothing AndAlso dtSampleInfo.Rows.Count > 0 Then
                strAnalyzedDate = dtSampleInfo.Rows(0)("AnalyzedDate").ToString()
                strReviewedDate = dtSampleInfo.Rows(0)("ReviewedDate").ToString()
                strVerifiedDate = dtSampleInfo.Rows(0)("VerifiedDate").ToString()
                strAnalyzedBy = dtSampleInfo.Rows(0)("AnalyzedBy").ToString()
                strReviewedBy = dtSampleInfo.Rows(0)("ReviewedBy").ToString()
                strVerifiedBy = dtSampleInfo.Rows(0)("VerifiedBy").ToString()
            End If
        End If
    End Sub
    Public Sub AssignJoinUserSign(ByVal picValidatedBy As DevExpress.XtraReports.UI.XRPictureBox, ByVal picApprovedBy As DevExpress.XtraReports.UI.XRPictureBox,
                                  ByVal picAnalyzedBy1 As DevExpress.XtraReports.UI.XRPictureBox, ByVal picAnalyzedBy2 As DevExpress.XtraReports.UI.XRPictureBox,
                                  ByVal picAnalyzedBy3 As DevExpress.XtraReports.UI.XRPictureBox, ByVal picAnalyzedBy4 As DevExpress.XtraReports.UI.XRPictureBox,
                                  ByVal picAnalyzedBy5 As DevExpress.XtraReports.UI.XRPictureBox, ByVal picAnalyzedBy6 As DevExpress.XtraReports.UI.XRPictureBox,
                                  ByVal picAnalyzedBy7 As DevExpress.XtraReports.UI.XRPictureBox, ByVal picAnalyzedBy8 As DevExpress.XtraReports.UI.XRPictureBox,
                                  ByVal picAnalyzedBy9 As DevExpress.XtraReports.UI.XRPictureBox, ByVal picAnalyzedBy10 As DevExpress.XtraReports.UI.XRPictureBox,
                                  ByRef IntRowCount As Integer)
        Try
            Dim dsSign As DataSet = DynamicReportBusinessLayer.BLCommon.Public_UserSign(struqSampleParameterID)
            If Not dsSign Is Nothing AndAlso dsSign.Tables.Count > 0 AndAlso dsSign.Tables(0).Rows.Count > 0 Then
                Try
                    If Not picAnalyzedBy1 Is Nothing AndAlso Not IsDBNull(dsSign.Tables(0).Rows(0)("AnalyzedBySign")) Then
                        Dim sig As Byte() = dsSign.Tables(0).Rows(0)("AnalyzedBySign")
                        Dim mem As MemoryStream = New MemoryStream(sig)
                        picAnalyzedBy1.Image = New Bitmap(mem)
                    End If
                Catch ex As Exception
                End Try
                Try
                    If Not picValidatedBy Is Nothing AndAlso Not IsDBNull(dsSign.Tables(0).Rows(0)("ValidatedBySign")) Then
                        Dim sig As Byte() = dsSign.Tables(0).Rows(0)("ValidatedBySign")
                        Dim mem As MemoryStream = New MemoryStream(sig)
                        picValidatedBy.Image = New Bitmap(mem)
                    End If
                Catch ex As Exception
                End Try
                Try
                    If Not picApprovedBy Is Nothing AndAlso Not IsDBNull(dsSign.Tables(0).Rows(0)("ApprovedBySign")) Then
                        Dim sig As Byte() = dsSign.Tables(0).Rows(0)("ApprovedBySign")
                        Dim mem As MemoryStream = New MemoryStream(sig)
                        picApprovedBy.Image = New Bitmap(mem)
                    End If
                Catch ex As Exception
                End Try
            End If




            Dim strString As String = "SELECT ROW_NUMBER() over (order by colUserID)as RowNo,u.colSignatureImg FROM dbo.tbl_User u where u.colUserID IN ( SELECT DISTINCT colUserID FROM tbl_ENV_JoinAnalyzedBy WHERE coluqSampleParameterID IN (" + struqSampleParameterID + ")) AND u.colSignatureImg IS NOT NULL"
            Dim dtTable As DataTable = DynamicReportBusinessLayer.BLCommon.GetData(strString)
            Dim intCount As Integer = 1
            If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then
                IntRowCount = dtTable.Rows.Count
                For Each dr As DataRow In dtTable.Rows
                    Try
                        Select Case dr(0)
                            Case 1
                                Try
                                    If Not picAnalyzedBy2 Is Nothing AndAlso Not IsDBNull(dr(1)) Then
                                        Dim sig As Byte() = dr(1)
                                        Dim mem As MemoryStream = New MemoryStream(sig)
                                        picAnalyzedBy2.Image = New Bitmap(mem)
                                    End If
                                Catch ex As Exception
                                End Try
                            Case 2
                                Try
                                    If Not picAnalyzedBy3 Is Nothing AndAlso Not IsDBNull(dr(1)) Then
                                        Dim sig As Byte() = dr(1)
                                        Dim mem As MemoryStream = New MemoryStream(sig)
                                        picAnalyzedBy3.Image = New Bitmap(mem)
                                    End If
                                Catch ex As Exception
                                End Try
                            Case 3
                                Try
                                    If Not picAnalyzedBy4 Is Nothing AndAlso Not IsDBNull(dr(1)) Then
                                        Dim sig As Byte() = dr(1)
                                        Dim mem As MemoryStream = New MemoryStream(sig)
                                        picAnalyzedBy4.Image = New Bitmap(mem)
                                    End If
                                Catch ex As Exception
                                End Try
                            Case 4
                                Try
                                    If Not picAnalyzedBy5 Is Nothing AndAlso Not IsDBNull(dr(1)) Then
                                        Dim sig As Byte() = dr(1)
                                        Dim mem As MemoryStream = New MemoryStream(sig)
                                        picAnalyzedBy5.Image = New Bitmap(mem)
                                    End If
                                Catch ex As Exception
                                End Try
                            Case 5
                                Try
                                    If Not picAnalyzedBy6 Is Nothing AndAlso Not IsDBNull(dr(1)) Then
                                        Dim sig As Byte() = dr(1)
                                        Dim mem As MemoryStream = New MemoryStream(sig)
                                        picAnalyzedBy6.Image = New Bitmap(mem)
                                    End If
                                Catch ex As Exception
                                End Try
                            Case 6
                                Try
                                    If Not picAnalyzedBy7 Is Nothing AndAlso Not IsDBNull(dr(1)) Then
                                        Dim sig As Byte() = dr(1)
                                        Dim mem As MemoryStream = New MemoryStream(sig)
                                        picAnalyzedBy7.Image = New Bitmap(mem)
                                    End If
                                Catch ex As Exception
                                End Try
                            Case 7
                                Try
                                    If Not picAnalyzedBy8 Is Nothing AndAlso Not IsDBNull(dr(1)) Then
                                        Dim sig As Byte() = dr(1)
                                        Dim mem As MemoryStream = New MemoryStream(sig)
                                        picAnalyzedBy8.Image = New Bitmap(mem)
                                    End If
                                Catch ex As Exception
                                End Try
                            Case 8
                                Try
                                    If Not picAnalyzedBy9 Is Nothing AndAlso Not IsDBNull(dr(1)) Then
                                        Dim sig As Byte() = dr(1)
                                        Dim mem As MemoryStream = New MemoryStream(sig)
                                        picAnalyzedBy9.Image = New Bitmap(mem)
                                    End If
                                Catch ex As Exception
                                End Try

                            Case 9
                                Try
                                    If Not picAnalyzedBy10 Is Nothing AndAlso Not IsDBNull(dr(1)) Then
                                        Dim sig As Byte() = dr(1)
                                        Dim mem As MemoryStream = New MemoryStream(sig)
                                        picAnalyzedBy10.Image = New Bitmap(mem)
                                    End If
                                Catch ex As Exception
                                End Try

                        End Select

                    Catch ex As Exception
                    End Try
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub AssignJoinAnalyzedBySign(ByVal xrReport As XtraReport, ByVal strBandName As String)
        Try
            'creating picture box
            Dim strString As String = "SELECT u.colSignatureImg FROM dbo.tbl_User u where u.colUserID IN ( SELECT DISTINCT colUserID FROM tbl_ENV_JoinAnalyzedBy WHERE coluqSampleParameterID IN (" + struqSampleParameterID + ")) AND u.colSignatureImg IS NOT NULL"
            Dim dtTable As DataTable = DynamicReportBusinessLayer.BLCommon.GetData(strString)
            If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then
                Dim IntHerizontal As Integer = 0
                Dim IntCommaHerizontal As Integer = 0
                Dim IntVertical As Integer = 0
                Dim bolNextRow As Boolean = False
                Dim IntIndex As Integer = 1
                Dim xrLabel1 As XRLabel = New XRLabel()
                xrLabel1.Text = ", "
                xrLabel1.Font = New Font(xrLabel1.Font.FontFamily, 12, FontStyle.Bold)
                xrLabel1.SizeF = New System.Drawing.SizeF(12.5, 23)
                xrLabel1.LocationF = New Point(602, 42.92)
                xrLabel1.Name = "LabelJoinAnalyzedBy" + CStr(0)
                With xrReport.Bands(strBandName)
                    .Controls.Add(xrLabel1)
                End With

                For Each dr As DataRow In dtTable.Rows

                    Dim xrPJoinAnalyzedBy As XRPictureBox = New XRPictureBox()
                    If Not xrPJoinAnalyzedBy Is Nothing AndAlso Not IsDBNull(dr(0)) Then
                        Dim sig As Byte() = dr(0)
                        Dim mem As MemoryStream = New MemoryStream(sig)
                        xrPJoinAnalyzedBy.Image = New Bitmap(mem)
                    End If
                    If IntIndex = 1 Then
                        IntHerizontal = 613.38
                        IntCommaHerizontal = IntHerizontal + 100
                        IntVertical = 42.92
                        'ElseIf IntIndex = 2 Then
                        '    IntHerizontal = IntCommaHerizontal + 10
                        '    IntCommaHerizontal = IntHerizontal + 100
                        '    IntVertical = 42.92
                    ElseIf IntIndex > 1 And IntIndex < 8 Then
                        If bolNextRow = False Then
                            IntHerizontal = 10
                            IntCommaHerizontal = IntHerizontal + 100
                            IntVertical = IntVertical + 34.43
                            bolNextRow = True
                        Else
                            IntHerizontal = IntCommaHerizontal + 10
                            IntCommaHerizontal = IntHerizontal + 100
                        End If
                    ElseIf IntIndex = 14 Then
                        bolNextRow = False
                    End If
                    xrPJoinAnalyzedBy.LocationF = New Point(IntHerizontal, IntVertical)
                    xrPJoinAnalyzedBy.SizeF = New System.Drawing.SizeF(100.0F, 23.0F)
                    xrPJoinAnalyzedBy.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage
                    xrPJoinAnalyzedBy.Name = "xrPJoinAnalyzedBy" + CStr(IntIndex)
                    Dim xrLabel As XRLabel = New XRLabel()
                    xrLabel.Text = ", "
                    xrLabel.Font = New Font(xrLabel.Font.FontFamily, 12, FontStyle.Bold)
                    xrLabel.SizeF = New System.Drawing.SizeF(12.5, 23)
                    xrLabel.LocationF = New Point(IntCommaHerizontal, IntVertical)
                    xrLabel.Name = "LabelJoinAnalyzedBy" + CStr(IntIndex)
                    With xrReport.Bands(strBandName)
                        .Controls.Add(xrPJoinAnalyzedBy)
                        If IntIndex <> dtTable.Rows.Count Then
                            .Controls.Add(xrLabel)
                        End If
                    End With
                    IntIndex = IntIndex + 1
                Next
            End If
        Catch ex As Exception
        End Try

    End Sub

    Public Function GetDataSourceAfterFilterString(ByVal Report As XtraReport) As DataTable
        If Not Report Is Nothing Then
            Dim ds As DataSet = Report.DataSource
            If Not ds Is Nothing AndAlso ds.Tables.Count > 0 Then
                If Not Report.FilterString Is Nothing AndAlso Len(Report.FilterString) > 0 Then
                    'Dim dvView As New DataView(ds.Tables(0), GetReportFilters(Report), "", DataViewRowState.CurrentRows)
                    'If Not dvView Is Nothing AndAlso dvView.Count > 0 Then
                    '    Return dvView.ToTable
                    'End If
                Else
                    Return ds.Tables(0)
                End If
            End If
        End If
        Return Nothing
    End Function
    Public Function GetJobID() As DataTable
        Dim dtJobID As New DataTable
        dtJobID.Columns.Add(New DataColumn("JobID", GetType(String)))
        Try
            If Len(Trim(strJobID)) > 0 Then
                Dim drJobID As DataRow
                drJobID = dtJobID.NewRow
                drJobID("JobID") = strJobID
                dtJobID.Rows.Add(drJobID)
                Return dtJobID
            Else
                Return dtJobID
            End If
        Catch ex As Exception
            Return dtJobID
        End Try
    End Function
    Public Function GetHeaderDateRange(ByVal dtTable As DataTable) As String
        Dim strDateRange As String = String.Empty
        If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then
            Dim dFrom As Date = IIf(IsDate(dtTable.Rows(0)("AnalysisDateFrom")), dtTable.Rows(0)("AnalysisDateFrom"), Date.MinValue)
            Dim dTo As Date = IIf(IsDate(dtTable.Rows(0)("AnalysisDateTo")), dtTable.Rows(0)("AnalysisDateTo"), Date.MinValue)
            If dFrom <> Date.MinValue AndAlso dTo <> Date.MinValue Then
                strDateRange = "(" & dFrom.Year & " " & Format(dFrom.Month, "00") & " " & Format(dFrom.Day, "00") & " to " &
                              dTo.Year & " " & Format(dTo.Month, "00") & " " & Format(dTo.Day, "00") & ")"
            End If
        End If
        Return strDateRange
    End Function



    Public Function GetDataSource(ByVal Report As XtraReport) As DataTable
        If Not Report Is Nothing Then
            Dim ds As DataSet = Report.DataSource
            If Not ds Is Nothing AndAlso ds.Tables.Count > 0 Then
                Return ds.Tables(0)
            End If
        End If
        Return Nothing
    End Function

    Public Sub DateTimeFormatWithoutAMPM(ByVal sender As Object)
        If Len(Trim(sender.text)) > 0 Then
            Dim d As DateTime
            d = Date.Parse(Trim(sender.text))
            sender.text = ReportMultilanguage.GetDateTimeValueString(d, ReportMultilanguage.DateTimeFormatType.DateTimeWithoutAMPM)
        End If
    End Sub
    Public Function DateTimeFormatWithoutAMPM_Report(ByVal MinAndMaxDate As DateTime) As String
        Dim MinAndMaxDateValue As String = String.Empty
        If Len(Trim(MinAndMaxDate)) > 0 Then
            Dim d As DateTime
            d = Date.Parse(Trim(MinAndMaxDate))
            MinAndMaxDateValue = ReportMultilanguage.GetDateTimeValueString(d, ReportMultilanguage.DateTimeFormatType.DateTimeWithoutAMPM)
            Return MinAndMaxDateValue
        End If
        Return MinAndMaxDateValue
    End Function

    Public Sub DateTimeFormatWithAMPM(ByVal sender As Object)
        If Len(Trim(sender.text)) > 0 Then
            Dim d As DateTime
            d = Date.Parse(Trim(sender.text))
            sender.text = ReportMultilanguage.GetDateTimeValueString(d, ReportMultilanguage.DateTimeFormatType.DateTimeWithAMPM)
        End If
    End Sub
    Public Function DateTimeFormatWithAMPM_Report(ByVal MinAndMaxDate As DateTime) As String
        Dim MinAndMaxDateValue As String = String.Empty
        If Len(Trim(MinAndMaxDate)) > 0 Then
            Dim d As DateTime
            d = Date.Parse(Trim(MinAndMaxDate))
            MinAndMaxDateValue = ReportMultilanguage.GetDateTimeValueString(d, ReportMultilanguage.DateTimeFormatType.DateTimeWithAMPM)
            Return MinAndMaxDateValue
        End If
        Return MinAndMaxDateValue
    End Function

    Public Sub DateTimeFormatShortDate(ByVal sender As Object)
        If Len(Trim(sender.text)) > 0 Then
            Dim d As DateTime
            d = Date.Parse(Trim(sender.text))
            sender.text = ReportMultilanguage.GetDateTimeValueString(d, ReportMultilanguage.DateTimeFormatType.ShortDate)

        End If
    End Sub
    Public Sub DateTimeFormatShortTime(ByVal sender As Object)
        If Len(Trim(sender.text)) > 0 Then
            Dim d As DateTime
            d = Date.Parse(Trim(sender.text))
            sender.text = ReportMultilanguage.GetDateTimeValueString(d, ReportMultilanguage.DateTimeFormatType.ShortTime)
        End If
    End Sub
    Public Sub SetCurrentDateTime(ByVal sender As Object)
        ' sender.text = ServerTimeControl.CurrentDateTime
    End Sub

    Public Function GetCurrentLanguageConstant(ByVal strText As String) As String
        Return ReportMultilanguage.GetCurrentLanguageConstant(strText)
    End Function

    Public Function AssignSubReport(ByVal Subreport As XRSubreport, ByVal strSubReportName As String) As XRSubreport
        Dim Report As XtraReport = GetReportFromLayOut(strSubReportName)
        Subreport.ReportSource = Report
        Return Subreport
    End Function

    Public Function AssignSubReport(ByVal strSubReportName As String) As XtraReport
        Dim Report As XtraReport = GetReportFromLayOut(strSubReportName, False)
        Return Report
    End Function

    Public Function GetSubReport(ByVal strSubReportName As String) As XtraReport
        Return GetReportFromLayOut(strSubReportName)
    End Function

    Public Function AddEmptyRows(ByVal dtTable As DataTable, ByVal intRowsOnPage As Integer, ByVal intLessRowsOnLastPage As Integer) As DataTable
        If Not dtTable Is Nothing Then
            Dim iMod As Integer = dtTable.Rows.Count Mod intRowsOnPage
            If dtTable.Rows.Count = 0 OrElse iMod > 0 Then
                For Int As Integer = 1 To intRowsOnPage - iMod - intLessRowsOnLastPage
                    Dim drRow As DataRow = dtTable.NewRow
                    dtTable.Rows.Add(drRow)
                Next
            End If
        End If
        Return dtTable
    End Function
    Public Sub GetCurrentLoginUser(ByVal sender As Object)
        ' sender.text = UserName
    End Sub
    Public Function GetReportID() As String
        strReportIDDataSource = NewObjLDMReportingVariables.strReportID
        Return strReportIDDataSource
    End Function
    Public Function GetApprovedBy() As String

        Dim currentUserId As String = SecuritySystem.CurrentUserId.ToString()
        strApprovedBy = currentUserId
        Return strApprovedBy
    End Function
    Public Function GetReportedBy() As String
        Return strReportedBy
    End Function
    Public Sub GetReportingDateTime(ByVal sender As Object)
        If Not strReportingDate Is Nothing AndAlso Len(strReportingDate) > 0 Then
            sender.text = strReportingDate
            strReportingDate = String.Empty
        End If
    End Sub
    Public Sub GetCurrentUserSign(ByVal picReportedBy As DevExpress.XtraReports.UI.XRPictureBox)
        Try
            Dim dtSign As DataTable = DynamicReportBusinessLayer.BLCommon.CurrentUserSign(UserName)
            If Not dtSign Is Nothing AndAlso dtSign.Rows.Count > 0 Then
                If Not picReportedBy Is Nothing AndAlso Not IsDBNull(dtSign.Rows(0)("LoginUserSign")) Then
                    Dim sig As Byte() = dtSign.Rows(0)("LoginUserSign")
                    Dim mem As MemoryStream = New MemoryStream(sig)
                    picReportedBy.Image = New Bitmap(mem)
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Function GetUserFullName(ByVal strName As String) As String
        Try
            Dim dtSign As DataTable = DynamicReportBusinessLayer.BLCommon.GetUserFullname(strName)
            If Not dtSign Is Nothing AndAlso dtSign.Rows.Count > 0 Then
                Return dtSign.Rows(0)("FullName")
            Else
                Return ""
            End If
        Catch ex As Exception
        End Try
    End Function
    Public Sub GetUserSign(ByVal picReportedBy As DevExpress.XtraReports.UI.XRPictureBox, ByVal strUserName As String)
        Try
            Dim dtSign As DataTable = DynamicReportBusinessLayer.BLCommon.CurrentUserSign(strUserName)
            If Not dtSign Is Nothing AndAlso dtSign.Rows.Count > 0 Then
                If Not picReportedBy Is Nothing AndAlso Not IsDBNull(dtSign.Rows(0)("LoginUserSign")) Then
                    Dim sig As Byte() = dtSign.Rows(0)("LoginUserSign")
                    Dim mem As MemoryStream = New MemoryStream(sig)
                    picReportedBy.Image = New Bitmap(mem)
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Sub GetReportLogo(ByVal sender As Object)
        sender.Image = ReportMultilanguage.BT_ReportLogo

    End Sub
    Public Sub GetBECKReportHeader(ByVal sender As Object)
        sender.Image = ReportMultilanguage.BT_BECKReportHeader
    End Sub

    Public Function dsQCDataSource() As DataSet
        Return GlobalReportSourceCode.dsQCDataSource
    End Function
    Public Function ExecuteDataTable(ByVal Query As String) As DataTable
        Dim dt As DataTable = DynamicReportBusinessLayer.BLCommon.ExecuteDataTable(Query)
        Return dt
    End Function
    Public Sub ReplaceDataSource(ByVal Report As XtraReport, Optional ByVal dtNewTable As DataTable = Nothing)
        Try
            If Not Report Is Nothing Then
                Dim ds As DataSet
                Dim dtReport As DataTable = Nothing
                If TypeOf Report.DataSource Is DataSet Then
                    ds = Report.DataSource
                    dtReport = ds.Tables(0)
                ElseIf TypeOf Report.DataSource Is DataTable Then
                    dtReport = Report.DataSource
                End If
                If Not dtReport Is Nothing Then
                    If Not dtNewTable Is Nothing Then
                        If Not dtNewTable Is Nothing AndAlso dtNewTable.Rows.Count > 0 Then
                            dtReport.Rows.Clear()
                            For Each dr As DataRow In dtNewTable.Rows
                                dtReport.ImportRow(dr)
                            Next
                        End If
                    Else
                        If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then
                            dtReport.Rows.Clear()
                            For Each dr As DataRow In dtTable.Rows
                                dtReport.ImportRow(dr)
                            Next
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Function GetReportRevision(Optional ByVal ReportID As String = "") As Integer
        Try
            Dim strQryRevisionNo As String, intRevisionNo As Integer
            intRevisionNo = 0
            If ReportID.Length = 0 Then
                'When creating the new revision report
                'If strReportRevision.Length > 0 Then
                intRevisionNo = 0
                'End If
            Else
                'when updating the existing report, query to determine the report is already revised report or not.
                strQryRevisionNo = "Select RevisionNo from Reporting where ReportID = N'" & ReportID & " '"
                Dim dtRevisionNo As DataTable = DynamicReportBusinessLayer.BLCommon.ExecuteDataTable(strQryRevisionNo)
                If Not dtRevisionNo Is Nothing AndAlso dtRevisionNo.Rows.Count > 0 Then
                    If Not IsDBNull(dtRevisionNo.Rows(0)("RevisionNo")) AndAlso IsNumeric(dtRevisionNo.Rows(0)("RevisionNo")) = True Then
                        intRevisionNo = dtRevisionNo.Rows(0)("RevisionNo")
                    End If
                End If
            End If
            Return intRevisionNo
        Catch ex As Exception
            Return 0
        End Try
    End Function
    Public Function GetUQSampleParameterID() As String
        Return struqSampleParameterID
    End Function
    'Public Function GetBlankResult(ByVal TestParameterID As String, ByVal strdate As String) As DataTable
    '    Try
    '        Dim dtBlank As DataTable = BusinessLayer.BLCommon.GetBlankresult(TestParameterID, strdate)
    '        Return dtBlank
    '    Catch ex As Exception
    '    End Try
    'End Function
    'Public Function GetNumToWord(ByVal Num As String) As String
    '    Try
    '        Return clsNumToCNWord.CNWordConv(Num)
    '    Catch ex As Exception
    '        MsgBox(ex.Message())
    '        Return ""
    '    End Try
    'End Function
    'Public Function Report_FormatDecimalValue(ByVal Value As String, ByVal DecimalValue As Integer) As String
    '    Try
    '        Return FormatDecimalValue(Value, DecimalValue)
    '    Catch ex As Exception
    '        MsgBox(ex.Message())
    '        Return ""
    '    End Try
    'End Function
    'Public Function ExecuteDataTable(ByVal Query As String) As DataTable
    '    Dim dt As DataTable = BusinessLayer.BLCommon.ExecuteDataTable(Query)
    '    Return dt
    'End Function
    'Public Sub BindImage(ByVal picimage As DevExpress.XtraReports.UI.XRPictureBox, ByVal strImagePath As String)
    '    Try
    '        'This function using for bind the image, for example, we show the sample checkIn Image in sludge report.. 
    '        Dim _FTP As Rebex.Net.Ftp = GetFTPConnection()
    '        If _FTP.State = FtpState.Ready Then
    '            Dim FileName() As String = Split(strImagePath, "\")
    '            Dim Remotepath As String = strImagePath
    '            Dim createpath As String = Application.UserAppDataPath & ApplicationVariable.LocalFTPPath
    '            Dim Path As String = Application.UserAppDataPath & ApplicationVariable.LocalFTPPath & Stopwatch.GetTimestamp & FileName(FileName.Length - 1)
    '            If Not Directory.Exists(createpath) Then
    '                Directory.CreateDirectory(createpath)
    '            End If
    '            If _FTP.FileExists(Remotepath) Then
    '                If DownloadFile(_FTP, Remotepath, Path) = True Then
    '                    picimage.ImageUrl = Path
    '                Else
    '                    'If flagFromReportSignOff = True Then
    '                    '    BLCommon.InsertExceptionTrackingBL("File not downloaded", Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
    '                    '    flagImageDownloadfailed = True
    '                    'End If
    '                End If
    '            Else
    '                'If flagFromReportSignOff = True Then
    '                '    BLCommon.InsertExceptionTrackingBL("FTP not exists", Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
    '                'End If
    '            End If
    '            _FTP.Disconnect()
    '        Else
    '            'If flagFromReportSignOff = True Then
    '            '    BLCommon.InsertExceptionTrackingBL("FTP not Connect", Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
    '            '    flagImageDownloadfailed = True
    '            'End If
    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub
End Class
Public Class BTDataTableConverter
    Dim strFieldToConvert As String, dt As DataTable, strFilter As String
    Public Sub New()
    End Sub
    Public Sub New(ByVal dt As DataTable, ByVal strFieldToConvert As String)
        Me.dt = dt
        Me.strFieldToConvert = strFieldToConvert
        Me.strFilter = strFilter
    End Sub
    Public Function DataRowToString(ByVal dr As DataRow) As String
        Return dr(strFieldToConvert).ToString()
    End Function
    Public Function DataRowToInteger(ByVal dr As DataRow) As Integer
        If Not IsDBNull(dr(strFieldToConvert)) AndAlso IsNumeric(dr(strFieldToConvert)) Then
            Return CInt(dr(strFieldToConvert))
        Else
            Return 0
        End If
    End Function
    Public Function DataTableToArray(ByVal dt As DataTable, ByVal strFieldToConvert As String, Optional ByVal strFilter As String = "") As String()
        If Not dt Is Nothing AndAlso dt.Columns.Contains(strFieldToConvert) = True Then
            Me.strFieldToConvert = strFieldToConvert
        End If
        Dim drList As DataRow() = dt.Select(strFilter)
        Dim strArr As String() = Array.ConvertAll(drList, New Converter(Of DataRow, String)(AddressOf DataRowToString))
        Return strArr
    End Function
    Public Function DataTableToIntegerArray(ByVal dt As DataTable, ByVal strFieldToConvert As String, Optional ByVal strFilter As String = "") As Integer()
        If Not dt Is Nothing AndAlso dt.Columns.Contains(strFieldToConvert) = True Then
            Me.strFieldToConvert = strFieldToConvert
        End If
        Dim drList As DataRow() = dt.Select(strFilter)
        Dim strArr As Integer() = Array.ConvertAll(drList, New Converter(Of DataRow, Integer)(AddressOf DataRowToInteger))
        Return strArr
    End Function
    Public Function DataTableToArrayList(ByVal dt As DataTable, ByVal strFieldToConvert As String, Optional ByVal strFilter As String = "") As ArrayList
        If Not dt Is Nothing AndAlso dt.Columns.Contains(strFieldToConvert) = True Then
            Me.strFieldToConvert = strFieldToConvert
        End If
        Dim drList As DataRow() = dt.Select(strFilter)
        Dim strArr As String() = Array.ConvertAll(drList, New Converter(Of DataRow, String)(AddressOf DataRowToString))
        Dim strArrList As New ArrayList(strArr)
        Return strArrList
    End Function
    Public Function DataTableToString(ByVal dt As DataTable, ByVal strFieldToConvert As String, Optional ByVal strFilter As String = "") As String
        If Not dt Is Nothing AndAlso dt.Columns.Contains(strFieldToConvert) = True Then
            Me.strFieldToConvert = strFieldToConvert
        End If
        Dim drList As DataRow() = dt.Select(strFilter)
        Dim strArr As String() = Array.ConvertAll(drList, New Converter(Of DataRow, String)(AddressOf DataRowToString))
        If strArr.Length > 0 Then
            Return String.Join(",", strArr)
        Else
            Return ""
        End If
    End Function
    Public Function DataTableToFilteredDataTable(ByVal dt As DataTable, ByVal strFieldToConvert As String, Optional ByVal strFilter As String = "") As DataTable
        If Not dt Is Nothing AndAlso dt.Columns.Contains(strFieldToConvert) = True Then
            Me.strFieldToConvert = strFieldToConvert
        End If
        Dim dvList As DataView = New DataView(dt, strFilter, "", DataViewRowState.CurrentRows)
        Return dvList.ToTable(True, strFieldToConvert)
    End Function
    Public Function ArrayListToString(ByVal pArrayList As ArrayList) As String
        Return String.Join(",", pArrayList.ToArray())
    End Function
    Public Sub ImportDataSource(ByVal Report As XtraReport)
        If Not Report Is Nothing Then
            Dim dtReport As DataTable = Report.DataSource
            If Not dtReport Is Nothing Then
                If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then
                    dtReport.Rows.Clear()
                    dtReport = dtTable.Clone
                    For Each dr As DataRow In dtTable.Rows
                        dtReport.ImportRow(dr)
                    Next
                    Report.DataSource = dtReport
                End If
            End If
        End If
    End Sub
    Public Sub ImportDataSource(ByVal Report As XtraReport, Optional dtNewSource As DataTable = Nothing)
        If Not Report Is Nothing Then
            Dim ds As DataSet
            Dim dtReport As DataTable = Nothing
            If TypeOf Report.DataSource Is DataSet Then
                ds = Report.DataSource
                dtReport = ds.Tables(0)
            ElseIf TypeOf Report.DataSource Is DataTable Then
                dtReport = Report.DataSource
            End If
            If Not dtReport Is Nothing Then
                If Not dtNewSource Is Nothing AndAlso dtNewSource.Rows.Count > 0 Then
                    dtReport.Rows.Clear()
                    dtReport = dtNewSource.Clone
                    For Each dr As DataRow In dtNewSource.Rows
                        dtReport.ImportRow(dr)
                    Next
                    Report.DataSource = dtReport
                    Report.FillDataSource()
                End If
            End If
        End If
    End Sub
    Public Function Split(ByVal strText As String, ByVal Separator() As Char) As String()
        Try
            Dim strSeparator As String = New String(Separator)
            Dim strArray() As String = Nothing
            If Len(Trim(strText)) > 0 Then
                strArray = strText.Split(Separator, StringSplitOptions.RemoveEmptyEntries)
                Return strArray
            End If
            Return strArray
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    'Public Function DataRowArrayToString(ByVal drr As DataRow(), ByVal strFieldToConvert As String, Optional ByVal strFilter As String = "") As String
    '    If Not IsDBNull(drr) AndAlso drr.Length > 0 Then
    '        Me.strFieldToConvert = strFieldToConvert
    '    End If
    '    Dim dt As DataTable = drr.CopyToDataTable()
    '    Dim drList As DataRow() = dt.Select(strFilter)
    '    Dim strArr As String() = Array.ConvertAll(drList, New Converter(Of DataRow, String)(AddressOf DataRowToString))
    '    Return String.Join(",", strArr)
    'End Function
    'Public Function DataRowArrayToStringWithoutDuplicate(ByVal drr As DataRow(), ByVal strFieldToConvert As String, Optional ByVal strFilter As String = "") As String
    '    If Not IsDBNull(drr) AndAlso drr.Length > 0 Then
    '        Me.strFieldToConvert = strFieldToConvert
    '    End If
    '    Dim dt As DataTable = drr.CopyToDataTable()
    '    Dim dtUniqe As DataTable = dt.DefaultView.ToTable(True, strFieldToConvert)
    '    Dim drList As DataRow() = dtUniqe.Select(strFilter)
    '    Dim strArr As String() = Array.ConvertAll(drList, New Converter(Of DataRow, String)(AddressOf DataRowToString))
    '    Return String.Join(",", strArr)
    'End Function
    'Public Function GenericListToDataTable(Of T)(ByVal list As Collection(Of T)) As DataTable
    '    Dim entityType As Type = GetType(T)
    '    Dim table As New DataTable()
    '    Dim properties As PropertyDescriptorCollection = TypeDescriptor.GetProperties(entityType)
    '    For Each prop As PropertyDescriptor In properties
    '        table.Columns.Add(prop.Name, If(Nullable.GetUnderlyingType(prop.PropertyType), prop.PropertyType))
    '    Next
    '    For Each item As T In list
    '        Dim row As DataRow = table.NewRow()
    '        For Each prop As PropertyDescriptor In properties
    '            row(prop.Name) = If(prop.GetValue(item), DBNull.Value)
    '        Next
    '        table.Rows.Add(row)
    '    Next
    '    Return table
    'End Function
End Class


'Public Class ReportSettingsClass
'    Inherits CustomReportDesigner.ReportSettingsInfo

'    Public Function strTableColumnSchema() As String
'        Return TableColumnSchema()
'    End Function

'    Public Function strRowColumn() As String
'        Return RowColumn()
'    End Function

'    Public Function strNormalColumn() As String
'        Return NormalColumn()
'    End Function

'    Public Function strUniqueColumn() As String
'        Return UniqueColumn()
'    End Function

'    Public Function dtTableTableColumn() As DataTable
'        Return TableColumn()
'    End Function

'    Public Function GenerateRowsasColumn(ByVal rep As XtraReport) As XtraReport
'        Dim ds As DataSet = rep.DataSource
'        Dim dtTable As DataTable = ds.Tables(0)
'        Dim dtTableColumn As datatable = dtTableTableColumn
'        If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then

'            Dim dtParameterDetails As DataTable = dtTable.DefaultView.ToTable(True, Split(strRowColumn, ", "))
'            Dim dtSampleDetails As DataTable = dtTable.DefaultView.ToTable(True, Split(strNormalColumn, ", "))
'            If Not dtParameterDetails Is Nothing AndAlso dtParameterDetails.Rows.Count > 0 Then
'                Dim intParam As Integer = 0
'                Dim intPageCount As Integer = 0
'                For Each drParamRow As DataRow In dtParameterDetails.Rows
'                    intParam = intParam + 1
'                    For Each drSampleRows As DataRow In dtSampleDetails.Rows
'                        Dim strFilter As String = setFilter(drSampleRows, intPageCount)
'                        If intParam = 1 Then
'                            Dim drNewRow As DataRow = dtTableColumn.NewRow
'                            drNewRow("PageIndex") = intPageCount
'                            If Not strNormalColumn() Is Nothing AndAlso Len(strNormalColumn) > 0 Then
'                                Dim strSplit() As String = Split(strNormalColumn, ", ")
'                                If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
'                                    For intC As Integer = 0 To strSplit.Length - 1
'                                        drNewRow(strSplit(intC)) = drSampleRows(strSplit(intC))
'                                    Next
'                                End If
'                            End If
'                            If Not strRowColumn() Is Nothing AndAlso Len(strRowColumn) > 0 Then
'                                Dim strSplit() As String = Split(strRowColumn, ", ")
'                                If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
'                                    Dim drrResult() As DataRow = dtTable.Select(GetResult(drParamRow, drSampleRows))
'                                    If Not drrResult Is Nothing AndAlso drrResult.Length > 0 Then
'                                        For intC As Integer = 0 To strSplit.Length - 1
'                                            drNewRow(strSplit(intC) & intParam) = drrResult(0)(strSplit(intC))
'                                        Next
'                                    End If
'                                End If
'                            End If
'                            dtTableColumn.Rows.Add(drNewRow)
'                        Else
'                            Dim drrExistingRows() As DataRow = dtTableColumn.Select(IIf(Len(strFilter) > 0, strFilter & " and ", "") & " PageIndex = " & intPageCount & "")
'                            If Not drrExistingRows Is Nothing AndAlso drrExistingRows.Length > 0 Then
'                                If Not strNormalColumn() Is Nothing AndAlso Len(strNormalColumn) > 0 Then
'                                    Dim strSplit() As String = Split(strNormalColumn, ", ")
'                                    If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
'                                        For intC As Integer = 0 To strSplit.Length - 1
'                                            drrExistingRows(0)(strSplit(intC)) = drSampleRows(strSplit(intC))
'                                        Next
'                                    End If
'                                End If
'                                If Not strRowColumn() Is Nothing AndAlso Len(strRowColumn) > 0 Then
'                                    Dim strSplit() As String = Split(strRowColumn, ", ")
'                                    If Not strSplit Is Nothing AndAlso strSplit.Length > 0 Then
'                                        Dim drrResult() As DataRow = dtTable.Select(GetResult(drParamRow, drSampleRows))
'                                        If Not drrResult Is Nothing AndAlso drrResult.Length > 0 Then
'                                            For intC As Integer = 0 To strSplit.Length - 1
'                                                drrExistingRows(0)(strSplit(intC) & intParam) = drrResult(0)(strSplit(intC))
'                                            Next
'                                        End If
'                                    End If
'                                End If
'                            End If
'                        End If
'                    Next
'                    If (intParam Mod 3) = 0 Then
'                        intParam = 0
'                        intPageCount = intPageCount + 1
'                    End If
'                Next
'            End If
'        End If
'        rep.DataAdapter = Nothing
'        rep.DataSource = Nothing
'        rep.DataMember = String.Empty
'        rep.DataSource = dtTableColumn
'        Return rep
'    End Function

'    Private Function setFilter(ByVal drSampleRows As DataRow, ByVal intPageCount As Integer) As String
'        Dim strFilter As String = String.Empty
'        If Not drSampleRows Is Nothing AndAlso Not strUniqueColumn() Is Nothing AndAlso Len(strUniqueColumn) > 0 Then
'            Dim strUnique() As String = Split(strUniqueColumn, ", ")
'            If Not strUnique Is Nothing AndAlso strUnique.Length > 0 Then
'                If intPageCount > 0 Then
'                    strFilter = "PageIndex" & " = " & intPageCount & ""
'                End If
'                For intstr As Integer = 0 To strUnique.Length - 1
'                    If Len(strFilter) > 0 Then
'                        strFilter = strFilter & " and " & strUnique(intstr) & " = '" & drSampleRows(strUnique(intstr)) & "'"
'                    Else
'                        strFilter = strUnique(intstr) & " = '" & drSampleRows(strUnique(intstr)) & "'"
'                    End If
'                Next
'            End If
'        End If

'        Return strFilter
'    End Function

'    Private Function GetResult(ByVal drParamRow As DataRow, ByVal drSampleRows As DataRow) As String
'        Dim strFilter As String = String.Empty
'        If Not drSampleRows Is Nothing AndAlso Not strUniqueColumn() Is Nothing AndAlso Len(strUniqueColumn) > 0 Then
'            Dim strUnique() As String = Split(strUniqueColumn, ", ")
'            If Not strUnique Is Nothing AndAlso strUnique.Length > 0 Then
'                For intstr As Integer = 0 To strUnique.Length - 1
'                    If Len(strFilter) > 0 Then
'                        strFilter = strFilter & " and " & strUnique(intstr) & " = '" & drSampleRows(strUnique(intstr)) & "'"
'                    Else
'                        strFilter = strUnique(intstr) & " = '" & drSampleRows(strUnique(intstr)) & "'"
'                    End If
'                Next
'            End If
'        End If
'        If Not drParamRow Is Nothing AndAlso Not strRowColumn() Is Nothing AndAlso Len(strRowColumn) > 0 Then
'            Dim strRow() As String = Split(strRowColumn, ", ")
'            If Not strRow Is Nothing AndAlso strRow.Length > 0 Then
'                For intstr As Integer = 0 To strRow.Length - 1
'                    If Len(strFilter) > 0 Then
'                        strFilter = strFilter & " and " & strRow(intstr) & " = '" & drParamRow(strRow(intstr)) & "'"
'                    Else
'                        strFilter = strRow(intstr) & " = '" & drParamRow(strRow(intstr)) & "'"
'                    End If
'                Next
'            End If
'        End If

'        Return strFilter
'    End Function
'End Class


