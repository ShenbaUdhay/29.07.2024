Imports DynamicReportBusinessLayer
Public Class frmDataTableSetup
    Public objML As frmDataTableSetupML
    Dim dtTable As DataTable

    Sub New(ByVal dtTable1 As DataTable)
        MyBase.new()
        Try
            objML = New frmDataTableSetupML(Me)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
            ' Add any initialization after the InitializeComponent() call.
            Call objML.SetupControls()
            dtTable = dtTable1
        Catch ex As Exception
        End Try
    End Sub

    Private Sub frmDataTableSetup_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not strTableColumnSchema Is Nothing AndAlso strTableColumnSchema.Length > 0 Then
            Dim dt As DataTable = GetData(strTableColumnSchema)
            If Not dt Is Nothing AndAlso dt.Columns.Count > 0 Then
                If ListBoxControl1.Items.Count > 0 Then
                    ListBoxControl1.Items.Clear()
                End If
                For i As Integer = 0 To dt.Columns.Count - 1
                    ListBoxControl1.Items.Add(dt.Columns(i).Caption)
                Next
            End If
        End If

        If Not dtTable Is Nothing AndAlso dtTable.Columns.Count > 0 Then
            Dim dtColumn As New DataTable
            dtColumn.Columns.Add("All", GetType(Boolean))
            dtColumn.Columns.Add("ColumnName", GetType(String))
            dtColumn.Columns.Add("AsColumn", GetType(Boolean))
            dtColumn.Columns.Add("HowMany", GetType(String))
            dtColumn.Columns.Add("UniqueColumn", GetType(Boolean))

            For intC As Integer = 0 To dtTable.Columns.Count - 1
                Dim drNewRow As DataRow = dtColumn.NewRow
                drNewRow("All") = False
                drNewRow("ColumnName") = dtTable.Columns(intC).Caption
                drNewRow("AsColumn") = False
                drNewRow("HowMany") = ""
                drNewRow("UniqueColumn") = False
                dtColumn.Rows.Add(drNewRow)
            Next
            GridControl1.DataSource = dtColumn
            With GridView1
                .Columns("All").Width = 40
                .Columns("ColumnName").Width = 120
                .Columns("AsColumn").Width = 70
                .Columns("HowMany").Width = 70
                .Columns("UniqueColumn").Width = 90
            End With
            Dim strRowColumnSplit() As String = strRowColumn.Split(",")
            Dim strUniqueColumnSplit() As String = strUniqueColumn.Split(",")
            Dim strNormalColumnSplit() As String = strNormalColumn.Split(",")
            Dim intlength As Integer = strRowColumnSplit.Length
            For intValue As Integer = 0 To intlength - 1
                Dim strRowColumnName As String
                strRowColumnName = strRowColumnSplit(intValue)
                Dim drRowColumn() As DataRow = dtColumn.Select("ColumnName='" & Trim(strRowColumnName) & "'", "", DataViewRowState.CurrentRows)
                If Not drRowColumn Is Nothing And drRowColumn.Length > 0 Then
                    drRowColumn(0)("AsColumn") = True
                    drRowColumn(0)("All") = True
                    drRowColumn(0)("HowMany") = strColumnCount
                End If
            Next
            For Each strNormalColumName As String In strNormalColumnSplit
                Dim drNormalColumn() As DataRow = dtColumn.Select("ColumnName='" & Trim(strNormalColumName) & "'", "", DataViewRowState.CurrentRows)
                If Not drNormalColumn Is Nothing And drNormalColumn.Length > 0 Then
                    drNormalColumn(0)("All") = True
                End If
            Next
            For Each strUniqueColumnName As String In strUniqueColumnSplit
                Dim drUniqueColumn() As DataRow = dtColumn.Select("ColumnName='" & Trim(strUniqueColumnName) & "'", "", DataViewRowState.CurrentRows)
                If Not drUniqueColumn Is Nothing And drUniqueColumn.Length > 0 Then
                    drUniqueColumn(0)("UniqueColumn") = True
                    drUniqueColumn(0)("All") = True
                End If
            Next
        End If
    End Sub

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        strTableColumnSchema = String.Empty
        strNormalColumn = String.Empty
        strRowColumn = String.Empty
        strUniqueColumn = String.Empty
        strColumnCount = String.Empty
        Dim intCount As Integer = 1
        If ListBoxControl1.Items.Count > 0 Then
            ListBoxControl1.Items.Clear()
        End If
        ListBoxControl1.Items.Add("PageIndex")
        Dim intResultCount As Integer = 0
        If Not GridView1 Is Nothing AndAlso GridView1.RowCount > 0 Then
            strTableColumnSchema = "Select '' AS PageIndex, "
            For intC As Integer = 0 To GridView1.RowCount - 1
                Dim drRow As DataRow = GridView1.GetDataRow(intC)
                If Not drRow Is Nothing AndAlso Not IsDBNull(drRow("All")) AndAlso drRow("All") = True Then
                    Dim strColumn As String = String.Empty
                    If Not IsDBNull(drRow("UniqueColumn")) AndAlso drRow("UniqueColumn") = True Then
                        If Not strUniqueColumn Is Nothing AndAlso strUniqueColumn.Length > 0 Then
                            strUniqueColumn = strUniqueColumn & ", " & drRow("ColumnName")
                        Else
                            strUniqueColumn = drRow("ColumnName")
                        End If
                    End If

                    If (Not IsDBNull(drRow("HowMany")) AndAlso Len(drRow("HowMany")) > 0 AndAlso IsNumeric(drRow("HowMany"))) OrElse (Not IsDBNull(drRow("AsColumn")) AndAlso drRow("AsColumn") = True) Then
                        If Not IsDBNull(drRow("HowMany")) AndAlso Len(drRow("HowMany")) > 0 AndAlso IsNumeric(drRow("HowMany")) Then
                            If intCount >= 2 Then
                                If drRow("Howmany") <> strColumnCount Then
                                    drRow("Howmany") = strColumnCount

                                End If
                            End If
                            strColumnCount = drRow("HowMany")
                            intCount = intCount + 1
                            For intR As Integer = 1 To Val(drRow("HowMany"))
                                intResultCount = Val(drRow("HowMany"))
                                strColumn = drRow("ColumnName") & intR
                                ListBoxControl1.Items.Add(strColumn)
                                Dim strType As System.Type = drRow("ColumnName").GetType
                                If strType.Name = "String" Then
                                    strTableColumnSchema = strTableColumnSchema & "'' AS " & strColumn & ", "
                                ElseIf strType.Name = "DateTime" Then
                                    strTableColumnSchema = strTableColumnSchema & "Convert(DateTime,NULL) AS " & strColumn & ", "
                                ElseIf strType.Name = "Integer" Then
                                    strTableColumnSchema = strTableColumnSchema & "Convert(Int,NULL) AS " & strColumn & ", "
                                ElseIf strType.Name = "Boolean" Then
                                    strTableColumnSchema = strTableColumnSchema & "Convert(Bit,NULL) AS " & strColumn & ", "
                                End If
                            Next
                            If Not IsDBNull(drRow("AsColumn")) AndAlso drRow("AsColumn") = True Then

                                If Not strRowColumn Is Nothing AndAlso strRowColumn.Length > 0 Then
                                    strRowColumn = strRowColumn & ", " & drRow("ColumnName")
                                Else
                                    strRowColumn = drRow("ColumnName")
                                End If
                            End If
                            'If Not strNormalColumn Is Nothing AndAlso strNormalColumn.Length > 0 Then
                            '    strNormalColumn = strNormalColumn & ", " & drRow("ColumnName")
                            'Else
                            '    strNormalColumn = drRow("ColumnName")
                            'End If
                        Else
                            'If Not strNormalColumn Is Nothing AndAlso strNormalColumn.Length > 0 Then
                            '    strNormalColumn = strNormalColumn & ", " & drRow("ColumnName")
                            'Else
                            '    strNormalColumn = drRow("ColumnName")
                            'End If
                            strColumn = drRow("ColumnName")
                            ListBoxControl1.Items.Add(strColumn)
                            Dim strType As System.Type = drRow("ColumnName").GetType
                            If strType.Name = "String" Then
                                strTableColumnSchema = strTableColumnSchema & "'' AS " & strColumn & ", "
                            ElseIf strType.Name = "DateTime" Then
                                strTableColumnSchema = strTableColumnSchema & "Convert(DateTime,NULL) AS " & strColumn & ", "
                            ElseIf strType.Name = "Integer" Then
                                strTableColumnSchema = strTableColumnSchema & "Convert(Int,NULL) AS " & strColumn & ", "
                            ElseIf strType.Name = "Boolean" Then
                                strTableColumnSchema = strTableColumnSchema & "Convert(Bit,NULL) AS " & strColumn & ", "
                            End If
                        End If
                    Else
                        If Not strNormalColumn Is Nothing AndAlso strNormalColumn.Length > 0 Then
                            strNormalColumn = strNormalColumn & ", " & drRow("ColumnName")
                        Else
                            strNormalColumn = drRow("ColumnName")
                        End If
                        strColumn = drRow("ColumnName")
                        ListBoxControl1.Items.Add(strColumn)
                        Dim strType As System.Type = drRow("ColumnName").GetType
                        If strType.Name = "String" Then
                            strTableColumnSchema = strTableColumnSchema & "'' AS " & strColumn & ", "
                        ElseIf strType.Name = "DateTime" Then
                            strTableColumnSchema = strTableColumnSchema & "Convert(DateTime,NULL) AS " & strColumn & ", "
                        ElseIf strType.Name = "Integer" Then
                            strTableColumnSchema = strTableColumnSchema & "Convert(Int,NULL) AS " & strColumn & ", "
                        ElseIf strType.Name = "Boolean" Then
                            strTableColumnSchema = strTableColumnSchema & "Convert(Bit,NULL) AS " & strColumn & ", "
                        End If
                    End If

                End If
            Next
            'For intR As Integer = 1 To intResultCount
            '    Dim strColumn As String = String.Empty
            '    strColumn = "Result" & intR
            '    ListBoxControl1.Items.Add(strColumn)
            '    strTableColumnSchema = strTableColumnSchema & "'' AS " & strColumn & ", "
            'Next
            strTableColumnSchema = Microsoft.VisualBasic.Left(RTrim(strTableColumnSchema), strTableColumnSchema.Length - 2)
            If Not strTableColumnSchema Is Nothing AndAlso Len(strTableColumnSchema) > 0 Then
                strTableColumnSchema = strTableColumnSchema & " WHERE 1=2"
            End If
        End If
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        dtTableColumn = GetData(strTableColumnSchema)
        Me.Close()
    End Sub

    Private Sub GridView1_CellValueChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        Dim drGridView1 As DataRow = GridView1.GetDataRow(e.RowHandle)
        If GridView1.FocusedColumn.ToString = "How Many" Then
            Dim columnCount As String = drGridView1("HowMany")
            If columnCount <> String.Empty Then
                If drGridView1("AsColumn") = True Then
                    Dim intCount As Integer
                    For intCount = 0 To GridView1.RowCount - 1
                        Dim drRow As DataRow = GridView1.GetDataRow(intCount)
                        If drRow("AsColumn") = True Then
                            drRow("HowMany") = columnCount
                        End If
                        strColumnCount = columnCount
                    Next
                ElseIf drGridView1("AsColumn") = False Then
                    drGridView1("AsColumn") = True
                    GridView1_CellValueChanged(sender, e)
                End If
            ElseIf columnCount = String.Empty Then
                drGridView1("AsColumn") = False
            End If
        End If
        If GridView1.FocusedColumn.ToString = "As Column" Then
            If drGridView1("Ascolumn") = False Then
                drGridView1("HowMany") = String.Empty
            ElseIf drGridView1("Ascolumn") = True And drGridView1("HowMany") = String.Empty Then
                drGridView1("HowMany") = strColumnCount
            End If
        End If
    End Sub

    Private Sub GridView1_ShowingEditor(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles GridView1.ShowingEditor
        Try
            If GridView1.FocusedColumn.FieldName = "ColumnName" Then
                e.Cancel = True
            Else
                e.Cancel = False
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

    End Sub
End Class