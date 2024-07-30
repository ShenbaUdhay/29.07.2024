Public Class frmCustomReportDesignerMDIML
    Inherits ReportMultilanguage.BaseClassML
    Implements ReportMultilanguage.IMultiLang

    Private objForm As frmCustomReportDesignerMDI

    Public Sub New(ByVal obj As frmCustomReportDesignerMDI)
        MyBase.New(obj)
        Me.objForm = obj
    End Sub

    Public Overloads Function GetControlText(ByVal ControlName As String) As String Implements ReportMultilanguage.IMultiLang.GetControlText
        Return MyBase.GetControlText(ControlName)
    End Function
    Public Overloads Function GetGridCaption(ByVal GridColumnCaption As String) As String Implements ReportMultilanguage.IMultiLang.GetGridCaption
        Return MyBase.GetGridCaption(GridColumnCaption)
    End Function
    'Public Overloads Sub GetGridCaption(ByVal gv As DevExpress.XtraGrid.Views.Grid.GridView) Implements ReportMultilanguage.IMultiLang.GetGridCaption
    '    MyBase.GetGridCaption(gv)
    'End Sub
    Public Overloads Function GetMessageBoxText(ByVal MessageKey As String) As String Implements ReportMultilanguage.IMultiLang.GetMessageBoxText
        Return MyBase.GetMessageBoxText(MessageKey)
    End Function

    Public Sub SetupControls(Optional ByVal IsContainer As Boolean = False) Implements ReportMultilanguage.IMultiLang.SetupControls

        objForm.Tag = MyBase.GetFormTitle

        objForm.PrintPreviewBarItem10.Caption = MyBase.GetControlText(objForm.PrintPreviewBarItem10.Name) 'pointer
        objForm.PrintPreviewBarItem11.Caption = MyBase.GetControlText(objForm.PrintPreviewBarItem11.Name) 'hand tool
        objForm.PrintPreviewBarItem12.Caption = MyBase.GetControlText(objForm.PrintPreviewBarItem12.Name) 'magnifier
        objForm.PrintPreviewBarItem21.Caption = MyBase.GetControlText(objForm.PrintPreviewBarItem21.Name) 'page color
        objForm.PrintPreviewBarItem22.Caption = MyBase.GetControlText(objForm.PrintPreviewBarItem22.Name) 'watermark

        objForm.PrintPreviewBarItem10.Hint = MyBase.GetControlText(objForm.PrintPreviewBarItem10.Name)
        objForm.PrintPreviewBarItem11.Hint = MyBase.GetControlText(objForm.PrintPreviewBarItem11.Name)
        objForm.PrintPreviewBarItem12.Hint = MyBase.GetControlText(objForm.PrintPreviewBarItem12.Name)
        objForm.PrintPreviewBarItem21.Hint = MyBase.GetControlText(objForm.PrintPreviewBarItem21.Name)
        objForm.PrintPreviewBarItem22.Hint = MyBase.GetControlText(objForm.PrintPreviewBarItem22.Name)

        objForm.PrintPreviewBarItem28.Caption = MyBase.GetControlText(objForm.PrintPreviewBarItem28.Name) 'margins
        objForm.PrintPreviewBarItem26.Caption = MyBase.GetControlText(objForm.PrintPreviewBarItem26.Name) 'orientation
        objForm.PrintPreviewBarItem27.Caption = MyBase.GetControlText(objForm.PrintPreviewBarItem27.Name) 'size
        objForm.PrintPreviewBarItem3.Caption = MyBase.GetControlText(objForm.PrintPreviewBarItem3.Name) 'find
        objForm.PrintPreviewBarItem1.Caption = MyBase.GetControlText(objForm.PrintPreviewBarItem1.Name) 'bookmarks
        objForm.PrintPreviewBarItem16.Caption = MyBase.GetControlText(objForm.PrintPreviewBarItem16.Name) 'first page
        objForm.PrintPreviewBarItem17.Caption = MyBase.GetControlText(objForm.PrintPreviewBarItem17.Name) 'previous page
        objForm.PrintPreviewBarItem18.Caption = MyBase.GetControlText(objForm.PrintPreviewBarItem18.Name) 'next page
        objForm.PrintPreviewBarItem19.Caption = MyBase.GetControlText(objForm.PrintPreviewBarItem19.Name) 'last page

        objForm.PrintPreviewBarItem9.Caption = MyBase.GetControlText(objForm.PrintPreviewBarItem9.Name) 'scale
        objForm.PrintPreviewBarItem8.Caption = MyBase.GetControlText(objForm.PrintPreviewBarItem8.Name) 'header/Footer
        objForm.PrintPreviewBarItem2.Caption = MyBase.GetControlText(objForm.PrintPreviewBarItem2.Name) 'Parameters
        objForm.PrintPreviewBarItem4.Caption = MyBase.GetControlText(objForm.PrintPreviewBarItem4.Name) 'Options
        objForm.PrintPreviewBarItem6.Caption = MyBase.GetControlText(objForm.PrintPreviewBarItem6.Name) 'quick print
        objForm.PrintPreviewBarItem5.Caption = MyBase.GetControlText(objForm.PrintPreviewBarItem5.Name) 'print
        objForm.PrintPreviewBarItem47.Caption = MyBase.GetControlText(objForm.PrintPreviewBarItem47.Name) 'save
        objForm.PrintPreviewBarItem46.Caption = MyBase.GetControlText(objForm.PrintPreviewBarItem46.Name) 'open

        objForm.CommandBarItem47.Caption = MyBase.GetControlText(objForm.CommandBarItem47.Name) 'back
        objForm.CommandBarItem48.Caption = MyBase.GetControlText(objForm.CommandBarItem48.Name) 'forward
        objForm.CommandBarItem49.Caption = MyBase.GetControlText(objForm.CommandBarItem49.Name) 'home
        objForm.CommandBarItem50.Caption = MyBase.GetControlText(objForm.CommandBarItem50.Name) 'refresh
        objForm.CommandBarItem51.Caption = MyBase.GetControlText(objForm.CommandBarItem51.Name) 'find

        objForm.CommandBarItem46.Caption = MyBase.GetControlText(objForm.CommandBarItem46.Name) 'zoom out
        objForm.CommandBarItem45.Caption = MyBase.GetControlText(objForm.CommandBarItem45.Name) 'zoom in
        objForm.CommandBarItem44.Caption = MyBase.GetControlText(objForm.CommandBarItem44.Name) 'zoom
        objForm.CommandBarItem43.Caption = MyBase.GetControlText(objForm.CommandBarItem43.Name) 'redo
        objForm.CommandBarItem42.Caption = MyBase.GetControlText(objForm.CommandBarItem42.Name) 'undo
        objForm.CommandBarItem41.Caption = MyBase.GetControlText(objForm.CommandBarItem41.Name) 'paste
        objForm.CommandBarItem40.Caption = MyBase.GetControlText(objForm.CommandBarItem40.Name) 'copy
        objForm.CommandBarItem39.Caption = MyBase.GetControlText(objForm.CommandBarItem39.Name) 'cut
        objForm.CommandBarItem34.Caption = MyBase.GetControlText(objForm.CommandBarItem34.Name) 'open
        objForm.CommandBarItem33.Caption = MyBase.GetControlText(objForm.CommandBarItem33.Name) 'save all
        objForm.CommandBarItem32.Caption = MyBase.GetControlText(objForm.CommandBarItem32.Name) 'save
        objForm.CommandBarItem31.Caption = MyBase.GetControlText(objForm.CommandBarItem31.Name) 'new report

        objForm.BarButtonItem2.Caption = MyBase.GetControlText(objForm.BarButtonItem2.Name) 'set columns
        objForm.ScriptsCommandBarItem1.Caption = MyBase.GetControlText(objForm.ScriptsCommandBarItem1.Name) 'Scripts

        objForm.XrDesignRibbonPage1.Text = MyBase.GetControlText(objForm.XrDesignRibbonPage1.Name) 'report desinger
        objForm.PrintPreviewRibbonPage1.Text = MyBase.GetControlText(objForm.PrintPreviewRibbonPage1.Name) 'print preview
        objForm.XrHtmlRibbonPage1.Text = MyBase.GetControlText(objForm.XrHtmlRibbonPage1.Name) ' HTML view
        objForm.XrDesignRibbonPageGroup4.Text = MyBase.GetControlText(objForm.XrDesignRibbonPageGroup4.Name) 'Alignment
        objForm.XrDesignRibbonPageGroup5.Text = MyBase.GetControlText(objForm.XrDesignRibbonPageGroup5.Name) 'Layout
        objForm.XrDesignRibbonPageGroup7.Text = MyBase.GetControlText(objForm.XrDesignRibbonPageGroup7.Name) 'view

        objForm.XrDesignRibbonPageGroup1.Text = MyBase.GetControlText(objForm.XrDesignRibbonPageGroup1.Name) 'Report
        objForm.XrDesignRibbonPageGroup2.Text = MyBase.GetControlText(objForm.XrDesignRibbonPageGroup2.Name) 'Edit
        objForm.XrDesignRibbonPageGroup3.Text = MyBase.GetControlText(objForm.XrDesignRibbonPageGroup3.Name) 'font
        objForm.XrDesignRibbonPageGroup6.Text = MyBase.GetControlText(objForm.XrDesignRibbonPageGroup6.Name) 'zoom
        objForm.XrDesignRibbonPageGroup8.Text = MyBase.GetControlText(objForm.XrDesignRibbonPageGroup8.Name) 'Scripts

        objForm.PrintPreviewRibbonPageGroup1.Text = MyBase.GetControlText(objForm.PrintPreviewRibbonPageGroup1.Name) 'Document
        objForm.PrintPreviewRibbonPageGroup2.Text = MyBase.GetControlText(objForm.PrintPreviewRibbonPageGroup2.Name) 'print
        objForm.PrintPreviewRibbonPageGroup3.Text = MyBase.GetControlText(objForm.PrintPreviewRibbonPageGroup3.Name) 'Page Setup
        objForm.PrintPreviewRibbonPageGroup4.Text = MyBase.GetControlText(objForm.PrintPreviewRibbonPageGroup4.Name) 'Navigation
        objForm.PrintPreviewRibbonPageGroup5.Text = MyBase.GetControlText(objForm.PrintPreviewRibbonPageGroup5.Name) 'zoom
        objForm.PrintPreviewRibbonPageGroup6.Text = MyBase.GetControlText(objForm.PrintPreviewRibbonPageGroup6.Name) 'page background
        objForm.PrintPreviewRibbonPageGroup7.Text = MyBase.GetControlText(objForm.PrintPreviewRibbonPageGroup7.Name) 'export
        objForm.CustomFileBarButton.Caption = MyBase.GetControlText(objForm.CustomFileBarButton.Name) 'Custom
        objForm.bbiExportReport.Caption = MyBase.GetControlText(objForm.bbiExportReport.Name) 'Export Report
        objForm.bbiImportReport.Caption = MyBase.GetControlText(objForm.bbiImportReport.Name) 'Export Report
    End Sub
End Class

Public Class frmChooseReportNameML
    Inherits ReportMultilanguage.BaseClassML
    Implements ReportMultilanguage.IMultiLang

    Private objForm As frmChooseReportName

    Public Sub New(ByVal obj As frmChooseReportName)
        MyBase.New(obj)
        Me.objForm = obj
    End Sub
    Public Overloads Function GetControlText(ByVal ControlName As String) As String Implements ReportMultilanguage.IMultiLang.GetControlText
        Return MyBase.GetControlText(ControlName)
    End Function
    Public Overloads Function GetGridCaption(ByVal GridColumnCaption As String) As String Implements ReportMultilanguage.IMultiLang.GetGridCaption
        Return MyBase.GetGridCaption(GridColumnCaption)
    End Function
    'Public Overloads Sub GetGridCaption(ByVal gv As DevExpress.XtraGrid.Views.Grid.GridView) Implements ReportMultilanguage.IMultiLang.GetGridCaption
    '    MyBase.GetGridCaption(gv)
    'End Sub
    Public Overloads Function GetMessageBoxText(ByVal MessageKey As String) As String Implements ReportMultilanguage.IMultiLang.GetMessageBoxText
        Return MyBase.GetMessageBoxText(MessageKey)
    End Function

    Public Sub SetupControls(Optional ByVal IsContainer As Boolean = False) Implements ReportMultilanguage.IMultiLang.SetupControls
        objForm.Tag = MyBase.GetFormTitle
        objForm.GroupControl1.Text = MyBase.GetControlText(objForm.GroupControl1.Name)
        objForm.cmdOk.Text = MyBase.GetControlText(objForm.cmdOk.Name)
        objForm.cmdDelete.Text = MyBase.GetControlText(objForm.cmdDelete.Name)
    End Sub
End Class

Public Class frmDataTableSetupML
    Inherits ReportMultilanguage.BaseClassML
    Implements ReportMultilanguage.IMultiLang

    Private objForm As frmDataTableSetup

    Public Sub New(ByVal obj As frmDataTableSetup)
        MyBase.New(obj)
        Me.objForm = obj
    End Sub
    Public Overloads Function GetControlText(ByVal ControlName As String) As String Implements ReportMultilanguage.IMultiLang.GetControlText
        Return MyBase.GetControlText(ControlName)
    End Function
    Public Overloads Function GetGridCaption(ByVal GridColumnCaption As String) As String Implements ReportMultilanguage.IMultiLang.GetGridCaption
        Return MyBase.GetGridCaption(GridColumnCaption)
    End Function
    'Public Overloads Sub GetGridCaption(ByVal gv As DevExpress.XtraGrid.Views.Grid.GridView) Implements ReportMultilanguage.IMultiLang.GetGridCaption
    '    MyBase.GetGridCaption(gv)
    'End Sub
    Public Overloads Function GetMessageBoxText(ByVal MessageKey As String) As String Implements ReportMultilanguage.IMultiLang.GetMessageBoxText
        Return MyBase.GetMessageBoxText(MessageKey)
    End Function

    Public Sub SetupControls(Optional ByVal IsContainer As Boolean = False) Implements ReportMultilanguage.IMultiLang.SetupControls
        objForm.Tag = MyBase.GetFormTitle
        objForm.SimpleButton2.Text = MyBase.GetControlText(objForm.SimpleButton2.Name)
        objForm.SimpleButton1.Text = MyBase.GetControlText(objForm.SimpleButton1.Name)
        objForm.btnCancel.Text = MyBase.GetControlText(objForm.btnCancel.Name)
        objForm.btnExit.Text = MyBase.GetControlText(objForm.btnExit.Name)
    End Sub
End Class

Public Class frmReportNameDetailsML
    Inherits ReportMultilanguage.BaseClassML
    Implements ReportMultilanguage.IMultiLang

    Private objForm As frmReportNameDetails

    Public Sub New(ByVal obj As frmReportNameDetails)
        MyBase.New(obj)
        Me.objForm = obj
    End Sub
    Public Overloads Function GetControlText(ByVal ControlName As String) As String Implements ReportMultilanguage.IMultiLang.GetControlText
        Return MyBase.GetControlText(ControlName)
    End Function
    Public Overloads Function GetGridCaption(ByVal GridColumnCaption As String) As String Implements ReportMultilanguage.IMultiLang.GetGridCaption
        Return MyBase.GetGridCaption(GridColumnCaption)
    End Function
    'Public Overloads Sub GetGridCaption(ByVal gv As DevExpress.XtraGrid.Views.Grid.GridView) Implements ReportMultilanguage.IMultiLang.GetGridCaption
    '    MyBase.GetGridCaption(gv)
    'End Sub
    Public Overloads Function GetMessageBoxText(ByVal MessageKey As String) As String Implements ReportMultilanguage.IMultiLang.GetMessageBoxText
        Return MyBase.GetMessageBoxText(MessageKey)
    End Function

    Public Sub SetupControls(Optional ByVal IsContainer As Boolean = False) Implements ReportMultilanguage.IMultiLang.SetupControls
        objForm.Tag = MyBase.GetFormTitle
        objForm.LabelControl1.Text = MyBase.GetControlText(objForm.LabelControl1.Name)
        objForm.cmdOK.Text = MyBase.GetControlText(objForm.cmdOK.Name)
    End Sub
End Class

'End Namespace