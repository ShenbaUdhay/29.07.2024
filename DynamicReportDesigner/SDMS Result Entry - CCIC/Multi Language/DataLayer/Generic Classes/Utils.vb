Imports System.Windows.Forms
Imports System.Xml

Public Class Utils
    Public Shared Function LogError(ByVal Ex As Exception, Optional ByVal Message As String = "") As Boolean
        Try
            Dim msg As String = String.Empty
            If Not Ex Is Nothing Then
                msg = "Exception : " & Ex.Message & vbCrLf
            End If
            MsgBox(msg)
        Catch ex1 As Exception
        End Try
        Return True
    End Function
    Public Shared Sub ReadConfigFile(Optional ByVal boolNetworkLibrary As Boolean = True)
        Try
            Dim xmlDoc As New XmlDocument
            Dim nodes As XmlNodeList
            Dim bnodes As XmlNodeList
            Dim strServerName As String = ""
            Dim strDataBaseName As String = ""
            Dim strUserName As String = ""
            Dim strPassword As String = ""
            'xmlDoc.Load(System.Environment.CurrentDirectory & "\config.xml")
            xmlDoc.Load(Application.StartupPath & "\config.xml")
            '  xmlDoc.Load(System.Reflection.Assembly.GetExecutingAssembly.Location.Replace("DataLayer.dll", "config.xml"))
            nodes = xmlDoc.GetElementsByTagName("SDMSDataBaseConnection")
            For Each node As XmlNode In nodes
                bnodes = node.ChildNodes
                For Each bnode As XmlNode In bnodes
                    If bnode.Name = "SDMSServerName" Then
                        'strServerName = DeCrypt(bnode.InnerText)
                        strServerName = bnode.InnerText
                    End If
                    If bnode.Name = "SDMSDataBaseName" Then
                        'strDataBaseName = DeCrypt(bnode.InnerText)
                        strDataBaseName = bnode.InnerText
                    End If
                    If bnode.Name = "SDMSUserName" Then
                        ' strUserName = DeCrypt(bnode.InnerText)
                        strUserName = bnode.InnerText
                    End If
                    If bnode.Name = "SDMSPassword" Then
                        'strPassword = DeCrypt(bnode.InnerText)
                        strPassword = bnode.InnerText
                    End If
                Next
            Next
            SQLServerName = strServerName
            SQLUserID = strUserName
            SQLPassword = strPassword
            SQLDatabaseName = strDataBaseName
            'strServerName = arrSQLServerDetails.Item(0)
            'strUserName = arrSQLServerDetails.Item(1)
            'strPassword = arrSQLServerDetails.Item(2)
            'strDataBaseName = arrSQLServerDetails.Item(3)
            'SQLServerName = arrSQLServerDetails.Item(0)
            'SQLUserID = arrSQLServerDetails.Item(1)
            'SQLPassword = arrSQLServerDetails.Item(2)
            'SQLDatabaseName = arrSQLServerDetails.Item(3)
            'ConnectionSettings.cnString = "initial catalog=" & strDataBaseName & ";data source=" & strServerName & ";user id=" & strUserName & ";pwd=" & strPassword & ";Connect Timeout=200"
            If boolNetworkLibrary Then
                SQLHasNetworkLibrary = True
                Dim strserverNewinstance As String = Replace(SQLServerName, "\", "\\")
                ConnectionSettings.cnString = "data source=" & SQLServerName & ";Network Library=DBMSSOCN;initial catalog=" & SQLDatabaseName & ";user id=" & SQLUserID & ";pwd=" & SQLPassword & ";Connect Timeout=200"
                ConnectionSettings.cnStringReport = "data source=" & SQLServerName & ";Network Library=DBMSSOCN;initial catalog=" & SQLDatabaseName & ";user id=" & SQLUserID & ";pwd=" & SQLPassword & ";Connect Timeout=200"
                ConnectionSettings.cnStringWithOutPassword = "server=" & SQLServerName & ";database=" & SQLDatabaseName & ";uid=" & SQLUserID
                ConnectionSettings.cnStringReportWithOutPasswordInstances = "server=" & strserverNewinstance & ";database=" & SQLDatabaseName & ";uid=" & SQLUserID & ";Connect Timeout=0"
                ConnectionSettings.cnStringReportInstances = "server=" & strserverNewinstance & ";database=" & SQLDatabaseName & ";uid=" & SQLUserID & ";pwd=" & SQLPassword & ";Connect Timeout=0"
            Else
                SQLHasNetworkLibrary = False
                Dim strserverNewinstance As String = Replace(SQLServerName, "\", "\\")
                ConnectionSettings.cnString = "data source=" & SQLServerName & ";initial catalog=" & SQLDatabaseName & ";user id=" & SQLUserID & ";pwd=" & SQLPassword & ";Connect Timeout=200"
                ConnectionSettings.cnStringReport = "server=" & SQLServerName & ";database=" & SQLDatabaseName & ";uid=" & SQLUserID & ";pwd=" & SQLPassword & ";Connect Timeout=0"
                ConnectionSettings.cnStringWithOutPassword = "server=" & SQLServerName & ";database=" & SQLDatabaseName & ";uid=" & SQLUserID
                ConnectionSettings.cnStringReportWithOutPasswordInstances = "server=" & strserverNewinstance & ";database=" & SQLDatabaseName & ";uid=" & SQLUserID & ";Connect Timeout=0"
                ConnectionSettings.cnStringReportInstances = "server=" & strserverNewinstance & ";database=" & SQLDatabaseName & ";uid=" & SQLUserID & ";pwd=" & SQLPassword & ";Connect Timeout=0"
            End If
        Catch ex As Exception
            MsgBox("ReadConfigFile: Error reading from config.xml")
        End Try
    End Sub
    Public Shared Function SetLDM_WEB_ConnectionString(ByVal LDMSQLServerName As String, ByVal LDMSQLDatabaseName As String, ByVal LDMSQLUserID As String, ByVal LDMSQLPassword As String) As Boolean
        If Len(LDMSQLServerName) > 0 AndAlso Len(LDMSQLDatabaseName) > 0 _
  AndAlso Len(LDMSQLUserID) > 0 AndAlso Len(LDMSQLPassword) > 0 Then

            'ConnectionSettings.cnString = "data source=" & LDMSQLServerName & ";Network Library=DBMSSOCN;initial catalog=" & LDMSQLDatabaseName & ";user id=" & LDMSQLUserID & ";pwd=" & LDMSQLPassword & ";Connect Timeout=200"
            ConnectionSettings.cnString = "data source=" & LDMSQLServerName & ";initial catalog=" & LDMSQLDatabaseName & ";user id=" & LDMSQLUserID & ";pwd=" & LDMSQLPassword & ";Connect Timeout=200"
            ConnectionSettings.cnStringReport = "server=" & LDMSQLServerName & ";database=" & LDMSQLDatabaseName & ";uid=" & LDMSQLUserID & ";pwd=" & LDMSQLPassword & ";Connect Timeout=0"
            Return True
        End If
        Return False

    End Function
    Public Shared Function GetConnectionStringByDatabase(ByVal strDatabaseName As String) As String
        Dim strConnectionString As String = String.Empty
        If Len(SQLServerName) > 0 AndAlso _
                Len(SQLUserID) > 0 AndAlso _
                Len(SQLPassword) > 0 Then
            If SQLHasNetworkLibrary Then
                strConnectionString = "data source=" & SQLServerName & ";Network Library=DBMSSOCN;initial catalog=" & strDatabaseName & ";user id=" & SQLUserID & ";pwd=" & SQLPassword & ";Connect Timeout=200"
            Else
                strConnectionString = "data source=" & SQLServerName & ";initial catalog=" & strDatabaseName & ";user id=" & SQLUserID & ";pwd=" & SQLPassword & ";Connect Timeout=200"
            End If
        End If
        Return strConnectionString
    End Function

    Private Shared Function DeCrypt(ByVal Text As String) As String
        ' Encrypts/decrypts the passed string using 
        ' a simple ASCII value-swapping algorithm
        Dim strTempChar As String = String.Empty, i As Integer
        For i = 1 To Len(Text)
            strTempChar = CType(AscW(Mid$(Text, i, 1)) - 128, String)
            Mid$(Text, i, 1) = ChrW(CType(strTempChar, Integer))
        Next i
        Return Text
    End Function
End Class
