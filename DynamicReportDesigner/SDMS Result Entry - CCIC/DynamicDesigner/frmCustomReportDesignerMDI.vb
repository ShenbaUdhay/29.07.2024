Imports DevExpress.XtraReports.UI
Imports DynamicReportBusinessLayer

Public Class frmCustomReportDesignerMDI
    Public objML As frmCustomReportDesignerMDIML
    Public Sub New()
        MyBase.New()
        Try
            BLCommon.SetDBConnection()
            objML = New frmCustomReportDesignerMDIML(Me)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()
            Call objML.SetupControls()
            ' Add any initialization after the InitializeComponent() call.
        Catch ex As Exception

        End Try
    End Sub
    Public Sub New(ByVal ServerName As String, ByVal DBName As String, ByVal UserName As String, ByVal Password As String)
        MyBase.New()
        BLCommon.SetDBConnection(ServerName, DBName, UserName, Password)
        objML = New frmCustomReportDesignerMDIML(Me)
        ' This call is required by the designer.
        InitializeComponent()
        Call objML.SetupControls()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Dim strMode As String
    Dim strReportNameSaveAs As String
    Public Sub New(ByVal strMode1 As String, ByVal strReportName1 As String)
        MyBase.New()
        Try
            objML = New frmCustomReportDesignerMDIML(Me)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
            Call objML.SetupControls()
            strMode = strMode1
            strReportNameSaveAs = strReportName1
            ' Add any initialization after the InitializeComponent() call.
        Catch ex As Exception

        End Try
    End Sub
    Private Sub frmCustomReportDesignerMDI_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim frm As New XRDesignFormEx()
        'frm.DesignPanel.AddCommandHandler(New CommandHandler(frm.DesignPanel))
        'frm.OpenReport(New XtraReport())
        'frm.ShowDialog()
        XrDesignPanel1.AddCommandHandler(New CommandHandler(XrDesignPanel1))
        If Not strMode Is Nothing AndAlso Len(strMode) > 0 Then
            If strMode = "Edit" AndAlso Not strReportNameSaveAs Is Nothing AndAlso Len(strReportNameSaveAs) > 0 Then
                strReportName = strReportNameSaveAs
                strTableColumnSchema = String.Empty
                strNormalColumn = String.Empty
                strRowColumn = String.Empty
                strUniqueColumn = String.Empty
                XrDesignPanel1.OpenReport(GetReportFromLayOut(strReportName))
            ElseIf strMode = "SaveAS" Then
                strReportName = strReportNameSaveAs
                strTableColumnSchema = String.Empty
                strNormalColumn = String.Empty
                strRowColumn = String.Empty
                strUniqueColumn = String.Empty
                XrDesignPanel1.OpenReport(GetReportFromLayOut(strReportName))
                'Dim iiCommandHandler As New CommandHandler(XrDesignPanel1)
                'iiCommandHandler.HandleCommand(DevExpress.XtraReports.UserDesigner.ReportCommand.SaveFileAs, Nothing, True)
                Dim iiCommandHandler As New CommandHandler(XrDesignPanel1)
                iiCommandHandler.HandleCommand(DevExpress.XtraReports.UserDesigner.ReportCommand.SaveFileAs, Nothing)
            End If
        End If
    End Sub

    Private Sub CommandBarItem36_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles CommandBarItem36.ItemClick
        strReportName = String.Empty
        strTableColumnSchema = String.Empty
        strNormalColumn = String.Empty
        strRowColumn = String.Empty
        strUniqueColumn = String.Empty
        'Dim iiCommandHandler As New CommandHandler(XrDesignPanel1)
        'iiCommandHandler.HandleCommand(DevExpress.XtraReports.UserDesigner.ReportCommand.NewReport, Nothing, True)
        Dim iiCommandHandler As New CommandHandler(XrDesignPanel1)
        iiCommandHandler.HandleCommand(DevExpress.XtraReports.UserDesigner.ReportCommand.NewReport, Nothing)
    End Sub

    Private Sub CommandBarItem35_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles CommandBarItem35.ItemClick
        strReportName = String.Empty
        strTableColumnSchema = String.Empty
        strNormalColumn = String.Empty
        strRowColumn = String.Empty
        strUniqueColumn = String.Empty
        XrDesignPanel1.OpenReport(New XtraReport)
        'Dim iiCommandHandler As New CommandHandler(XrDesignPanel1)
        'iiCommandHandler.HandleCommand(DevExpress.XtraReports.UserDesigner.ReportCommand.NewReport, Nothing, True)
        Dim iiCommandHandler As New CommandHandler(XrDesignPanel1)
        iiCommandHandler.HandleCommand(DevExpress.XtraReports.UserDesigner.ReportCommand.NewReport, Nothing)
    End Sub

    Private Sub CommandBarItem31_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles CommandBarItem31.ItemClick
        strReportName = String.Empty
        strTableColumnSchema = String.Empty
        strNormalColumn = String.Empty
        strRowColumn = String.Empty
        strUniqueColumn = String.Empty
        XrDesignPanel1.OpenReport(New XtraReport)
        'Dim iiCommandHandler As New CommandHandler(XrDesignPanel1)
        'iiCommandHandler.HandleCommand(DevExpress.XtraReports.UserDesigner.ReportCommand.NewReport, Nothing, False)
        Dim iiCommandHandler As New CommandHandler(XrDesignPanel1)
        iiCommandHandler.HandleCommand(DevExpress.XtraReports.UserDesigner.ReportCommand.NewReport, Nothing)
    End Sub

    Private Sub BarButtonItem2_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem2.ItemClick
        Dim Rep As XtraReport = XrDesignPanel1.Report
        If Not Rep Is Nothing Then
            Dim ds As DataSet = Rep.DataSource
            If Not ds Is Nothing Then
                Dim obj As New frmDataTableSetup(ds.Tables(0))
                obj.ShowDialog()
            End If
        End If

    End Sub
    Private Sub bbiExportReport_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiExportReport.ItemClick
        Try
            Dim Rep As XtraReport = XrDesignPanel1.Report
            If Not Rep Is Nothing Then
                Dim objsave As New SaveFileDialog()
                objsave.SupportMultiDottedExtensions = False
                objsave.Filter = "Report Files (*.Repx)|*.Repx"
                objsave.FileName = strReportName
                If objsave.ShowDialog = Windows.Forms.DialogResult.OK Then
                    Rep.SaveLayout(objsave.FileName)
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub bbiImportReport_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles bbiImportReport.ItemClick
        Try
            Dim Rep As XtraReport = XrDesignPanel1.Report
            'If Not Rep Is Nothing Then
            'Else
            Dim objsave As New OpenFileDialog()
            objsave.SupportMultiDottedExtensions = False
            objsave.Filter = "Report Files (*.Repx)|*.Repx"
            If objsave.ShowDialog = Windows.Forms.DialogResult.OK Then
                XrDesignPanel1.OpenReport(objsave.FileName)
                'strReportName = Replace(objsave.SafeFileName, ".Repx", "")
                strReportName = String.Empty
            End If
            'End If
        Catch ex As Exception
        End Try
    End Sub
End Class
