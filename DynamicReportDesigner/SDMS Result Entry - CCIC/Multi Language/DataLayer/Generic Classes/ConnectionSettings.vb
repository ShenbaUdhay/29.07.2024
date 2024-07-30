Public Module ConnectionSettings

    Private _cnString As String
    Private _cnSDFString As String
    Private _cnSamplingFileType As String
    Private _cnReportString As String
    Private _SQLUserID As String
    Private _SQLPassword As String
    Private _SQLServerName As String
    Private _SQLDatabaseName As String
    Private _SQLHasNetworkLibrary As Boolean

    Private _cnStringWithOutPassword As String
    Private _cnStringReport As String
    Private _cnStringReportInstances As String
    Private _cnStringReportWithOutPasswordInstances As String


    Public Property SQLHasNetworkLibrary() As Boolean
        Get
            Return _SQLHasNetworkLibrary
        End Get
        Set(ByVal Value As Boolean)
            _SQLHasNetworkLibrary = Value
        End Set
    End Property
    Public Property SQLUserID() As String
        Get
            Return _SQLUserID
        End Get
        Set(ByVal Value As String)
            _SQLUserID = Value
        End Set
    End Property
    Public Property SQLPassword() As String
        Get
            Return _SQLPassword
        End Get
        Set(ByVal Value As String)
            _SQLPassword = Value
        End Set
    End Property
    Public Property SQLServerName() As String
        Get
            Return _SQLServerName
        End Get
        Set(ByVal Value As String)
            _SQLServerName = Value
        End Set
    End Property
    Public Property SQLDatabaseName() As String
        Get
            Return _SQLDatabaseName
        End Get
        Set(ByVal Value As String)
            _SQLDatabaseName = Value
        End Set
    End Property

    Public Property cnString() As String
        Get
            Return _cnString
            'SQL Server Connection String
            'Return "server=win2003;database=btlims;uid=sa;pwd=bclims"
            'Return "server=ctech-sys4;database=btlims;uid=sa;pwd=bclims"
            'Return "server=lab_d;database=btlims;uid=sa;pwd=bclims"
        End Get
        Set(ByVal Value As String)
            _cnString = Value
        End Set
    End Property
    Public Property cnSDFString() As String
        Get
            Return _cnSDFString
        End Get
        Set(ByVal value As String)
            _cnSDFString = value
        End Set
    End Property
    Public Property cnSamplingFileType() As String
        Get
            Return _cnSamplingFileType
        End Get
        Set(ByVal value As String)
            _cnSamplingFileType = value
        End Set
    End Property

    Public Property cnStringReport() As String
        Get
            Return _cnStringReport
        End Get
        Set(ByVal Value As String)
            _cnStringReport = Value
        End Set
    End Property
    Public Property cnStringWithOutPassword() As String
        Get
            Return _cnStringWithOutPassword
        End Get
        Set(ByVal Value As String)
            _cnStringWithOutPassword = Value
        End Set
    End Property
    Public Property cnStringReportInstances() As String
        Get
            Return _cnStringReportInstances
        End Get
        Set(ByVal Value As String)
            _cnStringReportInstances = Value
        End Set
    End Property
    Public Property cnStringReportWithOutPasswordInstances() As String
        Get
            Return _cnStringReportWithOutPasswordInstances
        End Get
        Set(ByVal Value As String)
            _cnStringReportWithOutPasswordInstances = Value
        End Set
    End Property
End Module

