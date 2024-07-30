Imports DynamicReportBusinessLayer
Public Class frmReportNameDetails
    Dim boolsave As Boolean = False
    Public objML As frmReportNameDetailsML
    Public Sub New()
        MyBase.new()
        Try
            objML = New frmReportNameDetailsML(Me)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
            ' Add any initialization after the InitializeComponent() call.
            Call objML.SetupControls()
        Catch ex As Exception
        End Try
    End Sub
    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        If Len(txtReportName.Text) > 0 Then
            Dim strString As String = "Select * from tbl_Public_CustomReportDesignerDetails Where colCustomReportDesignerName = N'" & txtReportName.Text & "'"
            Dim dtTable As DataTable = GetData(strString)
            If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 Then
                MsgBox("ReportName already exists. Please enter another name.", MsgBoxStyle.Information)
                txtReportName.Text = String.Empty
            Else
                strReportName = txtReportName.Text
                boolsave = True
                Me.Close()
            End If
        End If
    End Sub

    Private Sub frmReportNameDetails_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If boolsave = False Then
            strReportName = ""
        Else
            boolsave = False
        End If
    End Sub
End Class