Imports DevExpress.XtraReports.UI
Public Module HorizontalReportDesignGenerator
    Public strTableColumnSchema As String = String.Empty
    Public dtTableColumn As DataTable
    Public strNormalColumn As String = String.Empty
    Public strRowColumn As String = String.Empty
    Public strUniqueColumn As String = String.Empty
    Public strColumnCount As String = String.Empty

    Public Function GenerateRowsasColumn(ByVal rep As XtraReport) As XtraReport
        Dim ds As DataSet = rep.DataSource
        Dim dtTable As DataTable = ds.Tables(0)
        If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then
            Dim str() As String = Split(strRowColumn, ", ")
            Dim dtParameterDetails1 As DataTable = dtTable.DefaultView.ToTable(True, str)
            Dim dtParameterDetails As DataTable = dtTable.DefaultView.ToTable(True, str)
            str = Split(strNormalColumn, ", ")
            Dim dtSampleDetails As DataTable = dtTable.DefaultView.ToTable(True, str)
            If Not dtParameterDetails Is Nothing AndAlso dtParameterDetails.Rows.Count > 0 Then
                Dim intParam As Integer = 0
                Dim intPageCount As Integer = 0
                For Each drParamRow As DataRow In dtParameterDetails.Rows
                    intParam = intParam + 1
                    For Each drSampleRows As DataRow In dtSampleDetails.Rows
                        Dim strFilter As String = setFilter(drSampleRows)
                        If intParam = 1 Then
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
                            If Not strRowColumn Is Nothing AndAlso Len(strRowColumn) > 0 Then
                                Dim strSplit() As String = Split(strRowColumn, ", ")
                                If Not strSplit Is Nothing AndAlso Len(strSplit) > 0 Then
                                    For intC As Integer = 0 To strSplit.Length - 1
                                        drNewRow(strSplit(intC) & intParam) = drSampleRows(strSplit(intC))
                                    Next
                                End If
                            End If
                            dtTableColumn.Rows.Add(drNewRow)
                        Else
                            Dim drrExistingRows() As DataRow = dtTableColumn.Select(IIf(Len(strFilter) > 0, strFilter & " and ", "") & " PageIndex = " & intPageCount & "")
                            If Not drrExistingRows Is Nothing AndAlso drrExistingRows.Length > 0 Then
                                If Not strNormalColumn Is Nothing AndAlso Len(strNormalColumn) > 0 Then
                                    Dim strSplit() As String = Split(strNormalColumn, ", ")
                                    If Not strSplit Is Nothing AndAlso Len(strSplit) > 0 Then
                                        For intC As Integer = 0 To strSplit.Length - 1
                                            drrExistingRows(0)(strSplit(intC)) = drSampleRows(strSplit(intC))
                                        Next
                                    End If
                                End If
                                If Not strRowColumn Is Nothing AndAlso Len(strRowColumn) > 0 Then
                                    Dim strSplit() As String = Split(strRowColumn, ", ")
                                    If Not strSplit Is Nothing AndAlso Len(strSplit) > 0 Then
                                        For intC As Integer = 0 To strSplit.Length - 1
                                            drrExistingRows(0)(strSplit(intC) & intParam) = drSampleRows(strSplit(intC))
                                        Next
                                    End If
                                End If
                            End If
                        End If
                    Next
                    If (intParam Mod 10) = 0 Then
                        intParam = 0
                        intPageCount = intPageCount + 1
                    End If
                Next
            End If
        End If
        rep.DataSource = dtTableColumn
        Return rep
    End Function

    Private Function setFilter(ByVal drSampleRows As DataRow) As String
        Dim strFilter As String = String.Empty
        If Not drSampleRows Is Nothing AndAlso Not strUniqueColumn Is Nothing AndAlso Len(strUniqueColumn) > 0 Then
            Dim strUnique() As String = Split(strUniqueColumn, ", ")
            If Not strUnique Is Nothing AndAlso strUnique.Length > 0 Then
                For intstr As Integer = 0 To strUnique.Length - 1
                    If Len(strFilter) > 0 Then
                        strFilter = " and " & strUnique(intstr) & " = '" & drSampleRows(strUnique(intstr)) & "'"
                    Else
                        strFilter = strUnique(intstr) & " = '" & drSampleRows(strUnique(intstr)) & "'"
                    End If
                Next
            End If
        End If

        Return strFilter
    End Function

    Public Function DataBindingsToReport(ByVal xtrReport As XtraReport) As XtraReport

        'xtrReport.JobID.DataBindings.Add("Text", xtrReport.DataSource, "ReportingDate")

        Return xtrReport
    End Function


End Module


