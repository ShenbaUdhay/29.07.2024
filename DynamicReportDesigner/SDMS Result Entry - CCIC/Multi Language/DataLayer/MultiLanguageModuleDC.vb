Imports System.Data.SqlClient

Public Module MultiLanguageModuleDC
    Private _BTAppName As String
    Private _BTAppDescription As String
    Private _currentLanguage As BTLanguages
    Private dtConst As DataTable

    Public Enum BTLanguages
        EN 'English
        CN 'Chinese
    End Enum
    'This enum should be same as in MultiLanguageModule in the BTLIMS project
    Enum DateTimeFormatType
        DateTimeWithAMPM
        DateTimeWithoutAMPM
        ShortDate
        ShortTime
    End Enum

    Public Property BT_AppName() As String
        Get
            Return _BTAppName
        End Get
        Set(ByVal Value As String)
            _BTAppName = Value
        End Set
    End Property
    Public Property BT_AppDescription() As String
        Get
            Return _BTAppDescription
        End Get
        Set(ByVal Value As String)
            _BTAppDescription = Value
        End Set
    End Property
    Public Property BT_Language() As BTLanguages
        Get
            Return _currentLanguage
        End Get
        Set(ByVal Value As BTLanguages)
            _currentLanguage = Value
        End Set
    End Property

    'This function should be same as in MultiLanguageModule in the BTLIMS project
    Public Function GetDateTimeValue(ByVal obj As Object, ByVal objFormat As DateTimeFormatType) As DateTime
        Dim dtValue As DateTime
        Try
            dtValue = DateTime.Parse(obj)
            Dim dtFormatString As String = String.Empty
            If BT_Language = BTLanguages.CN Then
                If objFormat = DateTimeFormatType.DateTimeWithAMPM Then
                    dtFormatString = "D"
                ElseIf objFormat = DateTimeFormatType.ShortDate Then
                    dtFormatString = "d"
                ElseIf objFormat = DateTimeFormatType.DateTimeWithoutAMPM Then
                    dtFormatString = "yyyy.MM.dd HH:mm"
                ElseIf objFormat = DateTimeFormatType.ShortTime Then
                    dtFormatString = "t"
                End If
            Else
                If objFormat = DateTimeFormatType.DateTimeWithAMPM Then
                    dtFormatString = "D"
                ElseIf objFormat = DateTimeFormatType.ShortDate Then
                    dtFormatString = "d"
                ElseIf objFormat = DateTimeFormatType.DateTimeWithoutAMPM Then
                    dtFormatString = "MM/dd/yyyy HH:mm"
                ElseIf objFormat = DateTimeFormatType.ShortTime Then
                    dtFormatString = "t"
                End If
            End If
            Return Format(dtValue, dtFormatString)
        Catch ex As Exception
            Return Nothing
        End Try
        Return dtValue
    End Function
    Public Function GetDateTimeValueString(ByVal obj As Object, ByVal objFormat As DateTimeFormatType) As String
        Dim dtValue As DateTime
        Try
            dtValue = DateTime.Parse(obj)
            Dim dtFormatString As String = String.Empty
            If BT_Language = BTLanguages.CN Then
                If objFormat = DateTimeFormatType.DateTimeWithAMPM Then
                    dtFormatString = "D"
                ElseIf objFormat = DateTimeFormatType.ShortDate Then
                    dtFormatString = "d"
                ElseIf objFormat = DateTimeFormatType.DateTimeWithoutAMPM Then
                    dtFormatString = "yyyy.MM.dd HH:mm"
                ElseIf objFormat = DateTimeFormatType.ShortTime Then
                    dtFormatString = "t"
                End If
            Else
                If objFormat = DateTimeFormatType.DateTimeWithAMPM Then
                    dtFormatString = "D"
                ElseIf objFormat = DateTimeFormatType.ShortDate Then
                    dtFormatString = "d"
                ElseIf objFormat = DateTimeFormatType.DateTimeWithoutAMPM Then
                    dtFormatString = "MM/dd/yyyy HH:mm"
                ElseIf objFormat = DateTimeFormatType.ShortTime Then
                    dtFormatString = "t"
                End If
            End If
            Return Format(dtValue, dtFormatString)
        Catch ex As Exception
            Return Nothing
        End Try
        Return dtValue
    End Function

    Public Function GetConstant(ByVal strCurrentLang As String) As String
        Dim dv As DataView
        If dtConst Is Nothing Then
            Dim objMLBL As New PUBLICDC.MultiLanguageDC
            dtConst = objMLBL.GetConstantData
        End If
        If Not dtConst Is Nothing AndAlso dtConst.Rows.Count > 0 Then
            dv = New DataView(dtConst, "[ConstantCurrentLang] = '" & strCurrentLang & "'", "", DataViewRowState.CurrentRows)
            If dv.Count > 0 Then
                If IsDBNull(dv(0)("Constant")) Then
                    Return String.Empty
                Else
                    Return dv(0)("Constant")
                End If
            Else
                Return String.Empty
            End If
        Else
            Return String.Empty
        End If
    End Function
    Public Function GetCurrentLanguageConstant(ByVal strConstant As String) As String
        Dim dv As DataView
        If dtConst Is Nothing Then
            Dim objMLBL As New PUBLICDC.MultiLanguageDC
            dtConst = objMLBL.GetConstantData
        End If
        If Not dtConst Is Nothing AndAlso dtConst.Rows.Count > 0 Then
            dv = New DataView(dtConst, "[Constant] = '" & strConstant & "'", "", DataViewRowState.CurrentRows)
            If dv.Count > 0 Then
                If IsDBNull(dv(0)("ConstantCurrentLang")) Then
                    Return String.Empty
                Else
                    Return dv(0)("ConstantCurrentLang")
                End If
            Else
                Return String.Empty
            End If
        Else
            Return String.Empty
        End If
    End Function
    Public Function GetFormTitle(ByVal FormName As String) As DataTable
        Try
            Dim params() As SqlParameter = New SqlParameter() {New SqlParameter("@FormName", FormName)}
            Dim d As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "Public_GetFormTitle_SP", params)
            Return d
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
End Module
