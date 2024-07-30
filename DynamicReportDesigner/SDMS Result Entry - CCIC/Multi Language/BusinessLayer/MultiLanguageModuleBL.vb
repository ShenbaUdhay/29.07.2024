Public Module MultiLanguageModuleBL
    Private _BTAppName As String
    Private _BTAppDescription As String
    Private _currentLanguage As BTLanguages

    Public Enum BTLanguages
        EN 'English
        CN 'Chinese
    End Enum

    Public Property BT_AppName() As String
        Get
            Return _BTAppName
        End Get
        Set(ByVal Value As String)
            _BTAppName = Value
            DynamicReportDataLayer.MultiLanguageModuleDC.BT_AppName = Value
        End Set
    End Property
    Public Property BT_AppDescription() As String
        Get
            Return _BTAppDescription
        End Get
        Set(ByVal Value As String)
            _BTAppDescription = Value
            DynamicReportDataLayer.MultiLanguageModuleDC.BT_AppDescription = Value
        End Set
    End Property
    Public Property BT_Language() As BTLanguages
        Get
            Return _currentLanguage
        End Get
        Set(ByVal Value As BTLanguages)
            _currentLanguage = Value
            DynamicReportDataLayer.MultiLanguageModuleDC.BT_Language = Value
        End Set
    End Property

    Public Function GetFormTitle(ByVal FormName As String) As DataTable
        Return DynamicReportDataLayer.MultiLanguageModuleDC.GetFormTitle(FormName)
    End Function

End Module

