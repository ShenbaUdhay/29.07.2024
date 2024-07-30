Imports DynamicReportBusinessLayer
Public Class frmChooseReportName
    Public objML As frmChooseReportNameML
    Public Sub New()
        MyBase.new()
        Try
            objML = New frmChooseReportNameML(Me)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
            ' Add any initialization after the InitializeComponent() call.
            Call objML.SetupControls()
        Catch ex As Exception
        End Try
    End Sub
    Private Sub frmChooseReportName_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strString As String = "Select * from tbl_Public_CustomReportDesignerDetails"
        Dim dtTable As DataTable = BLCommon.GetData(strString)
        If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then
            For Each drRow As DataRow In dtTable.Rows
                If Not IsDBNull(drRow("colCustomReportDesignerName")) AndAlso Len(drRow("colCustomReportDesignerName")) > 0 Then
                    chkReportName.Items.Add(drRow("colCustomReportDesignerName"), CheckState.Unchecked)
                End If
            Next
        End If
    End Sub

    Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdOk.Click
        If chkReportName.CheckedItems.Count > 0 Then
            strReportName = chkReportName.CheckedItems(0).ToString
            Me.Close()
        End If
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        'Dim ReportName As String = chkReportName.CheckedItems(CheckState.Checked)

        For i As Integer = 0 To chkReportName.Items.Count - 1
            If chkReportName.Items(i).CheckState = CheckState.Checked Then
                Dim ReportName As String = chkReportName.Items(i).ToString()
                'If Len(ReportName) > 0 Then
                Dim strString As String = "Delete from tbl_Public_CustomReportDesignerDetails where colCustomReportDesignerName=" & "N'" & ReportName & "'"
                ExecuteNonQuery(strString)
            End If
        Next

        Dim strString1 As String = "Select * from tbl_Public_CustomReportDesignerDetails"
        Dim dtTable As DataTable = BLCommon.GetData(strString1)
        If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then
            chkReportName.Items.Clear()
            For Each drRow As DataRow In dtTable.Rows
                If Not IsDBNull(drRow("colCustomReportDesignerName")) AndAlso Len(drRow("colCustomReportDesignerName")) > 0 Then
                    chkReportName.Items.Add(drRow("colCustomReportDesignerName"), CheckState.Unchecked)
                End If
            Next
        End If
    End Sub

End Class