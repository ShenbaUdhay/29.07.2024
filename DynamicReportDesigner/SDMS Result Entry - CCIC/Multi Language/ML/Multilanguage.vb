Imports System.Drawing
Imports System.Math
Imports DynamicReportBusinessLayer

'Namespace ReportMultilanguage
Public Interface IMultiLang
    Sub SetupControls(Optional ByVal IsContainer As Boolean = False)
    Function GetControlText(ByVal ControlName As String) As String
    Function GetGridCaption(ByVal GridColumnCaption As String) As String
    'Sub GetGridCaption(ByVal gv As DevExpress.XtraGrid.Views.Grid.GridView)
    Function GetMessageBoxText(ByVal MessageKey As String) As String
End Interface

Public Module MultiLanguageModule
    Private _BTAppName As String
    Private _BTAppDescription As String
    Private _currentLanguage As BTLanguages
    Private _currentAddress1 As String
    Private _currentAddress2 As String
    Private _currentAddress3 As String
    Private _currentPhone As String
    Private _currentFax As String
    Private _currentWebLink As String
    Private _currentVersion As String
    Private _BT_IsRPDorRSD As Integer
    Private _currentFont As String
    Private _currentClient As Clients
    Private dtConst As DataTable
    Private _BTIcon As Icon
    Private _UserID As Integer
    Private _objServerTime As ucServerTime
    Private _CallOldCalculation As Boolean
    Private _BT_AESI_ACCREDIATIONLogo As System.Drawing.Image
    Private _ReleasedBy As String
    Public Enum BTLanguages
        EN 'English
        CN 'Chinese
    End Enum
    Public Enum Clients
        ZHKY
        SDHJ
        ZHSS
        WHJC
        CTECH
        QBHJ
        HBPC
    End Enum
    Enum AlternativeControlText
        Text1 = 1
        Text2 = 2
        Text3 = 3
        Text4 = 4
        Text5 = 5
        Text6 = 6
        Text7 = 7
        Text8 = 8
        Text9 = 9
        Text10 = 10
        Text11 = 11
        Text12 = 12
    End Enum
    'If change is made in this enum then make the same change in DL also
    Enum DateTimeFormatType
        DateTimeWithAMPM
        DateTimeWithoutAMPM
        DateTimeWithoutAMPMWithSeconds
        ShortDate
        ShortTime
    End Enum

    Public Function GetAppFont(ByVal fntSize As Single, ByVal boolBold As Boolean) As System.Drawing.Font
        Try
            If boolBold Then
                Return New System.Drawing.Font(BT_Font, fntSize, FontStyle.Bold)
                'LiSu
                'Return New System.Drawing.Font("Code2000", fntSize, FontStyle.Bold)
            Else
                Return New System.Drawing.Font(BT_Font, fntSize)
                ' Return New System.Drawing.Font("Code2000", fntSize)
            End If
            Return Nothing
        Catch ex As Exception
            BLCommon.InsertExceptionTrackingBL(ex, "Multilanguage", Reflection.MethodInfo.GetCurrentMethod.Name)
            Return Nothing
        End Try
    End Function
    Public Property ServerTimeControl() As ucServerTime
        Get
            Return _objServerTime
        End Get
        Set(ByVal Value As ucServerTime)
            _objServerTime = Value
        End Set
    End Property
    Public Property BT_Font() As String
        Get
            Try
                Return _currentFont
            Catch ex As Exception
                BLCommon.InsertExceptionTrackingBL(ex, "Multilanguage", Reflection.MethodInfo.GetCurrentMethod.Name)
                Return String.Empty
            End Try
        End Get
        Set(ByVal Value As String)
            Try
                _currentFont = Value
            Catch ex As Exception
                BLCommon.InsertExceptionTrackingBL(ex, "Multilanguage", Reflection.MethodInfo.GetCurrentMethod.Name)
            End Try
        End Set
    End Property

    Public Property BT_Icon() As Icon
        Get
            Try
                Return _BTIcon
            Catch ex As Exception
                BLCommon.InsertExceptionTrackingBL(ex, "Multilanguage", Reflection.MethodInfo.GetCurrentMethod.Name)
                Return Nothing
            End Try
        End Get
        Set(ByVal Value As Icon)
            Try
                _BTIcon = Value
            Catch ex As Exception
                BLCommon.InsertExceptionTrackingBL(ex, "Multilanguage", Reflection.MethodInfo.GetCurrentMethod.Name)
            End Try
        End Set
    End Property
    Public Property BT_AESI_ACCREDIATIONLogo() As System.Drawing.Image
        Get
            Return _BT_AESI_ACCREDIATIONLogo
        End Get
        Set(value As System.Drawing.Image)
            _BT_AESI_ACCREDIATIONLogo = value
        End Set
    End Property
    Public Property BT_ReleasedBy() As String
        Get
            Return _ReleasedBy
        End Get
        Set(ByVal Value As String)
            _ReleasedBy = Value
        End Set
    End Property

    'Public ReadOnly Property BT_Icon() As Icon
    '    Get
    '        Try
    '            Return objrefMDI.Icon
    '        Catch ex As Exception
    '            BLCommon.InsertExceptionTrackingBL(ex, "Multilanguage", Reflection.MethodInfo.GetCurrentMethod.Name)
    '            Return Nothing
    '        End Try
    '    End Get
    'End Property
    Public Property BT_Language() As BTLanguages
        Get
            Try
                Return _currentLanguage
            Catch ex As Exception
                BLCommon.InsertExceptionTrackingBL(ex, "Multilanguage", Reflection.MethodInfo.GetCurrentMethod.Name)
                Return BTLanguages.EN
            End Try
        End Get
        Set(ByVal Value As BTLanguages)
            Try
                _currentLanguage = Value
                'BusinessLayer.MultiLanguageModuleBL.BT_Language = Value
            Catch ex As Exception
                BLCommon.InsertExceptionTrackingBL(ex, "Multilanguage", Reflection.MethodInfo.GetCurrentMethod.Name)
            End Try
        End Set
    End Property
    Public Property BT_AppName() As String
        Get
            Try

                Return _BTAppName
            Catch ex As Exception
                BLCommon.InsertExceptionTrackingBL(ex, "Multilanguage", Reflection.MethodInfo.GetCurrentMethod.Name)
                Return "BTLIMS"
            End Try
        End Get
        Set(ByVal Value As String)
            Try
                _BTAppName = Value
                '  BusinessLayer.MultiLanguageModuleBL.BT_AppName = Value
            Catch ex As Exception
                BLCommon.InsertExceptionTrackingBL(ex, "Multilanguage", Reflection.MethodInfo.GetCurrentMethod.Name)
            End Try
        End Set
    End Property
    Public Property BT_AppDescription() As String
        Get
            Try
                Return _BTAppDescription
            Catch ex As Exception
                BLCommon.InsertExceptionTrackingBL(ex, "Multilanguage", Reflection.MethodInfo.GetCurrentMethod.Name)
                Return "BTLIMS"
            End Try
        End Get
        Set(ByVal Value As String)
            Try
                _BTAppDescription = Value
                '   BusinessLayer.MultiLanguageModuleBL.BT_AppDescription = Value
            Catch ex As Exception
                BLCommon.InsertExceptionTrackingBL(ex, "Multilanguage", Reflection.MethodInfo.GetCurrentMethod.Name)
            End Try
        End Set
    End Property
    Public Property BT_Address1() As String
        Get
            Return _currentAddress1
        End Get
        Set(ByVal Value As String)
            _currentAddress1 = Value
        End Set
    End Property
    Public Property BT_Address2() As String
        Get
            Return _currentAddress2
        End Get
        Set(ByVal Value As String)
            _currentAddress2 = Value
        End Set
    End Property
    Public Property BT_Address3() As String
        Get
            Return _currentAddress3
        End Get
        Set(ByVal Value As String)
            _currentAddress3 = Value
        End Set
    End Property
    Public Property BT_Phone() As String
        Get
            Return _currentPhone
        End Get
        Set(ByVal Value As String)
            _currentPhone = Value
        End Set
    End Property
    Public Property BT_Fax() As String
        Get
            Return _currentFax
        End Get
        Set(ByVal Value As String)
            _currentFax = Value
        End Set
    End Property
    Public Property BT_WebLink() As String
        Get
            Return _currentWebLink
        End Get
        Set(ByVal Value As String)
            _currentWebLink = Value
        End Set
    End Property
    Public Property BT_Client() As Clients
        Get
            Return _currentClient
        End Get
        Set(ByVal Value As Clients)
            _currentClient = Value
        End Set
    End Property
    Public ReadOnly Property BT_ReportLogo() As System.Drawing.Image
        Get
            If BT_Client = Clients.ZHKY Then
                Return My.Resources.BTResource.BTCN_ZHKYLogo
            ElseIf BT_Client = Clients.SDHJ Then
                Return My.Resources.BTResource.BTCN_SDHJLogo
            Else
                Return Nothing
            End If
        End Get
    End Property
    Public ReadOnly Property BT_BECKReportHeader() As System.Drawing.Image
        Get
            Return My.Resources.BTResource.BECK_ReportHeader
        End Get
    End Property

    Public Property BT_Version() As String
        Get
            Return _currentVersion
        End Get
        Set(ByVal Value As String)
            _currentVersion = Value
        End Set
    End Property
    Public Property BT_IsRPDorRSD() As Integer
        Get
            Return _BT_IsRPDorRSD
        End Get
        Set(ByVal Value As Integer)
            _BT_IsRPDorRSD = Value
        End Set
    End Property
    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal Value As Integer)
            _UserID = Value
        End Set
    End Property
    Public Function GetApplicationData() As DataTable
        Try
            '  Dim objMLBL As BusinessLayer.PUBLICBO.MultiLanguageBL = New BusinessLayer.PUBLICBO.MultiLanguageBL
            'Return objMLBL.FillAppMultiLangData()
        Catch ex As Exception
            BLCommon.InsertExceptionTrackingBL(ex, "Multilanguage", Reflection.MethodInfo.GetCurrentMethod.Name)
            Return Nothing
        End Try
    End Function

    Public Sub FillConstantData()
        Try
            'Dim objMLBL As BusinessLayer.PUBLICBO.MultiLanguageBL = New BusinessLayer.PUBLICBO.MultiLanguageBL
            'dtConst = objMLBL.GetConstantData
        Catch ex As Exception
            BLCommon.InsertExceptionTrackingBL(ex, "Multilanguage", Reflection.MethodInfo.GetCurrentMethod.Name)
        End Try
    End Sub
    Public Function GetConstant(ByVal strCurrentLang As String) As String
        Try
            Dim dv As DataView
            If dtConst Is Nothing Then
                'Dim objMLBL As BusinessLayer.PUBLICBO.MultiLanguageBL = New BusinessLayer.PUBLICBO.MultiLanguageBL
                'dtConst = objMLBL.GetConstantData
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
        Catch ex As Exception
            BLCommon.InsertExceptionTrackingBL(ex, "Multilanguage", Reflection.MethodInfo.GetCurrentMethod.Name)
            Return String.Empty
        End Try
    End Function
    Public Function GetCurrentLanguageConstant(ByVal strConstant As String) As String
        Try
            Dim dv As DataView
            If dtConst Is Nothing Then
                Dim objMLBL As DynamicReportBusinessLayer.PUBLICBO.MultiLanguageBL = New DynamicReportBusinessLayer.PUBLICBO.MultiLanguageBL
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
        Catch ex As Exception
            BLCommon.InsertExceptionTrackingBL(ex, "Multilanguage", Reflection.MethodInfo.GetCurrentMethod.Name)
            Return String.Empty
        End Try
    End Function

    'Set culture based on selected Language
    Public Sub SetCurrentCulture()
        Try
            If ReportMultilanguage.BT_Language = BTLanguages.CN Then
                Dim cu As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("zh-CN")
                cu.DateTimeFormat.FullDateTimePattern = "yyyy.MM.dd hh:mm tt"
                cu.DateTimeFormat.DateSeparator = "."
                cu.DateTimeFormat.ShortDatePattern = "yyyy.MM.dd"
                cu.DateTimeFormat.ShortTimePattern = "HH:mm"
                cu.DateTimeFormat.LongDatePattern = "yyyy.MM.dd hh:mm tt"
                cu.DateTimeFormat.LongTimePattern = "hh:mm tt"

                Dim cuUI As System.Globalization.CultureInfo
                cuUI = New System.Globalization.CultureInfo("zh-CHS")
                System.Threading.Thread.CurrentThread.CurrentCulture = cu
                System.Threading.Thread.CurrentThread.CurrentUICulture = cuUI
            Else
                Dim cu As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-US")
                cu.DateTimeFormat.FullDateTimePattern = "MM/dd/yyyy hh:mm tt"
                cu.DateTimeFormat.DateSeparator = "/"
                cu.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy"
                cu.DateTimeFormat.ShortTimePattern = "HH:mm"
                cu.DateTimeFormat.LongDatePattern = "MM/dd/yyyy hh:mm tt"
                cu.DateTimeFormat.LongTimePattern = "hh:mm tt"

                Dim cuUI As System.Globalization.CultureInfo
                cuUI = New System.Globalization.CultureInfo("en")
                System.Threading.Thread.CurrentThread.CurrentCulture = cu
                System.Threading.Thread.CurrentThread.CurrentUICulture = cuUI
            End If

            'Dim assem As System.Reflection.Assembly
            'Dim assemname As System.Reflection.AssemblyName
            'Dim assemVersion As System.Version
            'assem = System.Reflection.Assembly.GetExecutingAssembly
            'assemname = assem.GetName
            'assemVersion = assemname.Version

            Dim spstr() As String = Split(System.Windows.Forms.Application.ProductVersion, ".")
            If spstr.Length > 0 Then
                BT_Version = "Version " & Format(Now, "yy") & ".1." & spstr(spstr.Length - 1)
            Else
                BT_Version = "Version " & Format(Now, "yy") & ".1.1"
            End If

            Try
                Dim MyIPAddress As String = String.Empty
                Dim ip() As System.Net.IPAddress = System.Net.Dns.GetHostEntry(String.Empty).AddressList
                If ip.Length > 0 Then
                    MyIPAddress = ip(0).ToString
                End If
                ' Dim obj As New BusinessLayer.VersionTrackingBL
                ' obj.InsertVersionTrackingData(My.Computer.Name, MyIPAddress, Multilanguage.MultiLanguageModule.BT_Version)
            Catch ex As Exception
                BLCommon.InsertExceptionTrackingBL(ex, "Multilanguage", Reflection.MethodInfo.GetCurrentMethod.Name)
            End Try
        Catch ex As Exception
            BLCommon.InsertExceptionTrackingBL(ex, "Multilanguage", Reflection.MethodInfo.GetCurrentMethod.Name)
        End Try
    End Sub

    Public Sub SetDateTimeFormat(ByVal obj As Object, ByVal objFormat As DateTimeFormatType)
        Try
            Dim dtFormatString As String = String.Empty
            If BT_Language = BTLanguages.CN Then
                If objFormat = DateTimeFormatType.DateTimeWithAMPM Then
                    dtFormatString = "D"
                ElseIf objFormat = DateTimeFormatType.ShortDate Then
                    dtFormatString = "d"
                ElseIf objFormat = DateTimeFormatType.DateTimeWithoutAMPM Then
                    dtFormatString = "yyyy.MM.dd HH:mm"
                ElseIf objFormat = DateTimeFormatType.DateTimeWithoutAMPMWithSeconds Then
                    dtFormatString = "yyyy.MM.dd HH:mm:ss"
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
                ElseIf objFormat = DateTimeFormatType.DateTimeWithoutAMPMWithSeconds Then
                    dtFormatString = "MM/dd/yyyy HH:mm:ss"
                ElseIf objFormat = DateTimeFormatType.ShortTime Then
                    dtFormatString = "t"
                End If
            End If
            If TypeOf (obj) Is DevExpress.XtraEditors.DateEdit Then
                Dim de As DevExpress.XtraEditors.DateEdit
                de = obj
                de.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                de.Properties.DisplayFormat.FormatString = dtFormatString
                de.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                de.Properties.EditFormat.FormatString = dtFormatString
                de.Properties.EditMask = dtFormatString
                'de.Properties.NullDate = Date.Today
                'de.Properties.NullDateCalendarValue = Date.Today
            ElseIf TypeOf (obj) Is DevExpress.XtraGrid.Columns.GridColumn Then
                Dim gc As DevExpress.XtraGrid.Columns.GridColumn
                gc = obj
                gc.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                gc.DisplayFormat.FormatString = dtFormatString
                'Dim A As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit = gc.RealColumnEdit
                'A.NullDateCalendarValue = Date.Now
            ElseIf TypeOf (obj) Is DevExpress.XtraEditors.Repository.RepositoryItemDateEdit Then
                Dim rde As DevExpress.XtraEditors.Repository.RepositoryItemDateEdit
                rde = obj
                rde.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                rde.DisplayFormat.FormatString = dtFormatString
                rde.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime
                rde.EditFormat.FormatString = dtFormatString
                rde.EditMask = dtFormatString
                'rde.NullDateCalendarValue = ServerTimeControl.CurrentDateTime
            End If
        Catch ex As Exception
            BLCommon.InsertExceptionTrackingBL(ex, "Multilanguage", Reflection.MethodInfo.GetCurrentMethod.Name)
        End Try
    End Sub
    'If change is made in this function then make the same change in DL also
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
                ElseIf objFormat = DateTimeFormatType.DateTimeWithoutAMPMWithSeconds Then
                    dtValue = CDate(obj)
                    dtFormatString = "yyyy.MM.dd HH:mm:ss"
                    Return Format(dtValue, dtFormatString)
                ElseIf objFormat = DateTimeFormatType.ShortTime Then
                    dtFormatString = "HH:mm"
                End If
            Else
                If objFormat = DateTimeFormatType.DateTimeWithAMPM Then
                    dtFormatString = "D"
                ElseIf objFormat = DateTimeFormatType.ShortDate Then
                    dtFormatString = "d"
                ElseIf objFormat = DateTimeFormatType.DateTimeWithoutAMPM Then
                    dtFormatString = "MM/dd/yyyy HH:mm"
                ElseIf objFormat = DateTimeFormatType.DateTimeWithoutAMPMWithSeconds Then
                    dtValue = CDate(obj)
                    dtFormatString = "MM/dd/yyyy HH:mm:ss"
                    Return Format(dtValue, dtFormatString)
                ElseIf objFormat = DateTimeFormatType.ShortTime Then
                    dtFormatString = "HH:mm"
                End If
            End If
            'Return Format(dtValue, dtFormatString)
            Return dtValue
        Catch ex As Exception
            BLCommon.InsertExceptionTrackingBL(ex, "Multilanguage", Reflection.MethodInfo.GetCurrentMethod.Name)
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
                ElseIf objFormat = DateTimeFormatType.DateTimeWithoutAMPMWithSeconds Then
                    dtValue = CDate(obj)
                    dtFormatString = "yyyy.MM.dd HH:mm:ss"
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
                ElseIf objFormat = DateTimeFormatType.DateTimeWithoutAMPMWithSeconds Then
                    dtValue = CDate(obj)
                    dtFormatString = "MM/dd/yyyy HH:mm:ss"
                ElseIf objFormat = DateTimeFormatType.ShortTime Then
                    dtFormatString = "t"
                End If
            End If
            Return Format(dtValue, dtFormatString)
        Catch ex As Exception
            BLCommon.InsertExceptionTrackingBL(ex, "Multilanguage", Reflection.MethodInfo.GetCurrentMethod.Name)
            Return Nothing
        End Try
        Return dtValue
    End Function
    Public Function GetFormTitle(ByVal FormName As String) As String
        Try
            Dim dt As DataTable
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0)("User Control Caption")) OrElse Len(Trim(dt.Rows(0)("User Control Caption"))) <= 0 Then
                    If IsDBNull(dt.Rows(0)("Form Title")) Then
                        Return String.Empty
                    Else
                        Return dt.Rows(0)("Form Title")
                    End If
                Else
                    Return dt.Rows(0)("User Control Caption")
                End If
            Else
                Return String.Empty
            End If
        Catch ex As Exception
            BLCommon.InsertExceptionTrackingBL(ex, "Multilanguage", Reflection.MethodInfo.GetCurrentMethod.Name)
            Return String.Empty
        End Try
    End Function
    Public Function FormatNumberCN(ByVal dbNumber As Double, ByVal iNoOfFraction As Integer) As String
        Try
            Dim iFCount As Integer
            Dim strFraction, strReturnValue As String
            Dim iTemp, iTempValue As Integer

            Dim strdbNumber As String = CStr(dbNumber)
            Dim decdbNumber As Decimal
            decdbNumber = Decimal.Parse(strdbNumber, System.Globalization.NumberStyles.Any)

            Dim strNumberSplit() As String = decdbNumber.ToString.Split(CChar(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
            If strNumberSplit.Length > 1 Then
                iFCount = strNumberSplit(1).Length
                strFraction = decdbNumber.ToString.Split(CChar(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))(1)
                strReturnValue = String.Empty
                '35.123 456
                'Fraction=3
                If iNoOfFraction <= 0 Then
                    iNoOfFraction = 0
                    '& CChar(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                    If iFCount > 0 Then
                        strReturnValue = Mid(strNumberSplit(0), 1, strNumberSplit(0).Length - 1)
                        iTempValue = Val(Mid(strNumberSplit(0), strNumberSplit(0).Length, 1))

                        If Mid(strFraction, 1, 1) = "5" Then
                            'iTempValue = Val(Mid(strFraction, 1, 1))
                            iTemp = iTempValue Mod 2
                            If iTemp = 0 Then
                                If 2 <= iFCount Then
                                    If Val(Mid(strFraction, 2, strFraction.Length)) > 0 Then
                                        GoTo FormatStep2
                                    Else
                                        strReturnValue &= iTempValue
                                    End If
                                Else
                                    strReturnValue &= iTempValue
                                End If
                            Else
FormatStep2:                    If iTempValue = 9 Then
                                    If Not strReturnValue Is Nothing AndAlso Len(strReturnValue) > 0 Then
                                        Dim tnof As Integer
                                        Dim addStr As String
                                        Dim addDbl As Double

                                        strReturnValue &= iTempValue
                                        tnof = GetNoOfFractionDigits(strReturnValue)
                                        If tnof = 1 Then
                                            addStr = "0.1"
                                        ElseIf tnof > 1 Then
                                            addStr = "0."
                                            For j As Integer = 0 To tnof - 2
                                                addStr &= "0"
                                            Next
                                            addStr &= "1"
                                        Else
                                            addStr = "1"
                                        End If
                                        addDbl = Val(addStr)
                                        strReturnValue = CDbl(strReturnValue) + addDbl
                                        strReturnValue = Format(CDbl(strReturnValue), GetNumberFormatString(tnof))

                                        'Dim flagAdd As Boolean
                                        'flagAdd = True
                                        'For intLoopC As Integer = Len(strReturnValue) - 1 To 0 Step -1
                                        '    If strReturnValue(intLoopC) <> "-" AndAlso strReturnValue(intLoopC) <> "." AndAlso flagAdd = True Then
                                        '        If strReturnValue(intLoopC) = "9" AndAlso intLoopC <> 0 Then
                                        '            Mid(strReturnValue, intLoopC) = "0"
                                        '        Else
                                        '            Mid(strReturnValue, intLoopC + 1, 1) = Val(strReturnValue(intLoopC)) + 1
                                        '            flagAdd = False
                                        '        End If
                                        '    End If
                                        'Next
                                        'strReturnValue &= 0
                                    End If
                                Else
                                    strReturnValue &= (iTempValue + 1)
                                End If
                            End If
                        Else
                            strReturnValue = Format(dbNumber, GetNumberFormatString(iNoOfFraction))
                        End If
                    Else
                        strReturnValue = dbNumber.ToString
                    End If
                Else
                    strReturnValue = strNumberSplit(0) & CChar(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)
                    If iNoOfFraction < iFCount Then
                        If Mid(strFraction, iNoOfFraction + 1, 1) = "5" Then
                            For i As Integer = 1 To iFCount
                                If i = iNoOfFraction Then
                                    iTempValue = Val(Mid(strFraction, i, 1))
                                    iTemp = iTempValue Mod 2
                                    If iTemp = 0 Then
                                        If iNoOfFraction + 2 <= iFCount Then
                                            If Val(Mid(strFraction, iNoOfFraction + 2, strFraction.Length)) > 0 Then
                                                GoTo FormatStep1
                                            Else
                                                strReturnValue &= iTempValue
                                            End If
                                        Else
                                            strReturnValue &= iTempValue
                                        End If
                                    Else
FormatStep1:                            If iTempValue = 9 Then
                                            If Not strReturnValue Is Nothing AndAlso Len(strReturnValue) > 0 Then
                                                Dim tnof As Integer
                                                Dim addStr As String
                                                Dim addDbl As Double

                                                strReturnValue &= iTempValue
                                                tnof = GetNoOfFractionDigits(strReturnValue)
                                                If tnof = 1 Then
                                                    addStr = "0.1"
                                                ElseIf tnof > 1 Then
                                                    addStr = "0."
                                                    For j As Integer = 0 To tnof - 2
                                                        addStr &= "0"
                                                    Next
                                                    addStr &= "1"
                                                Else
                                                    addStr = "1"
                                                End If
                                                addDbl = Val(addStr)
                                                strReturnValue = CDbl(strReturnValue) + addDbl
                                                strReturnValue = Format(CDbl(strReturnValue), GetNumberFormatString(tnof))
                                                'If tnof = 0 Then
                                                '    strReturnValue &= "."
                                                'End If

                                                'Dim flagAdd As Boolean
                                                'flagAdd = True
                                                'For intLoopC As Integer = Len(strReturnValue) - 1 To 0 Step -1
                                                '    If strReturnValue(intLoopC) <> "-" AndAlso strReturnValue(intLoopC) <> "." AndAlso flagAdd = True Then
                                                '        If strReturnValue(intLoopC) = "9" AndAlso intLoopC <> 0 Then
                                                '            Mid(strReturnValue, intLoopC) = "0"
                                                '        Else
                                                '            Mid(strReturnValue, intLoopC + 1, 1) = Val(strReturnValue(intLoopC)) + 1
                                                '            flagAdd = False
                                                '        End If
                                                '    End If
                                                'Next
                                                'strReturnValue &= 0
                                            End If
                                        Else
                                            strReturnValue &= (iTempValue + 1)
                                        End If
                                    End If
                                    Exit For
                                Else
                                    strReturnValue &= Mid(strFraction, i, 1)
                                End If
                            Next
                        Else
                            strReturnValue = Format(dbNumber, GetNumberFormatString(iNoOfFraction))
                        End If
                    Else
                        strReturnValue = Format(dbNumber, GetNumberFormatString(iNoOfFraction))
                    End If
                End If

                Return strReturnValue
            Else
                Return Format(dbNumber, GetNumberFormatString(iNoOfFraction))
            End If
        Catch ex As Exception
            BLCommon.InsertExceptionTrackingBL(ex, "Apllication Global", Reflection.MethodInfo.GetCurrentMethod.Name)
            Return dbNumber.ToString
        End Try
    End Function
    Public Function GetNoOfFractionDigits(ByVal dbl As String) As Integer
        Try
            Dim iFCount As Integer
            If dbl IsNot Nothing AndAlso dbl.Trim.Length > 0 Then
                iFCount = dbl.Trim.Split(CChar(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))(1).Length()
            End If
            Return iFCount
        Catch ex As Exception
            Return 0
        End Try
    End Function
    Public Function GetNumberFormatString(ByVal iNoOfFraction As Integer) As String
        If iNoOfFraction > 0 Then
            Dim strFractionFormat As String = "0."
            For i As Integer = 0 To iNoOfFraction - 1
                strFractionFormat &= "0"
            Next
            Return strFractionFormat
        Else
            Return "0"
        End If
    End Function
    Public Function FromatSigFigNum(ByVal dblResult As Double, ByVal intSig As Integer) As String
        Try
            Dim strFN As String = String.Empty
            Dim intCorrPower As Integer
            Dim intSign As Integer
            Dim FormatSF As Double
            intSign = Sign(dblResult)
            intCorrPower = Int(Log10(Abs(dblResult)))
            FormatSF = Round(dblResult * 10 ^ ((intSig - 1) - intCorrPower))   'integer value with no sig fig
            FormatSF = FormatSF * 10 ^ (intCorrPower - (intSig - 1))         'raise to original power
            '-- Reconsitute final answer --
            FormatSF = FormatSF * intSign
            Dim strP As String = ""
            strFN = FormatSF
            '-- Answer sometimes needs padding with 0s --
            If InStr(FormatSF, ".") = 0 Then
                If Len(FormatSF) < intSig Then
                    strFN = Format(FormatSF, "##0." & strP.PadLeft(intSig - Len(FormatSF), "0"))
                End If
            End If
            If intSig > 1 And Abs(FormatSF) < 1 Then
                Do Until Left(Right(FormatSF, intSig), 1) <> "0" And Left(Right(FormatSF, intSig), 1) <> "."
                    strFN = FormatSF & "0"
                Loop
            End If
            Return strFN
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function
    Public Property CallOldCalculation() As Boolean
        Get
            Return _CallOldCalculation
        End Get
        Set(ByVal Value As Boolean)
            _CallOldCalculation = Value
        End Set
    End Property

End Module

Public Class BaseClassML
    Private dsMultiLangData As DataSet
    Private Const strEmpty As String = "[Text]"
    Private arrEventList As ArrayList
    Private arrColumnSortOrder As ArrayList

    Public Sub New(ByVal objForm As Object)
        Try
            UpdateMultiLangDataSet(objForm.GetType.ToString)
        Catch ex As Exception
            BLCommon.InsertExceptionTrackingBL(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
        End Try
    End Sub
    Private Sub gv_EndSorting(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim gv As DevExpress.XtraGrid.Views.Grid.GridView
        Dim iArr As Integer
        Try
            gv = sender
            iArr = arrEventList.IndexOf(gv.Name)
            If iArr >= 0 Then
                If gv.SortedColumns.Count > 0 Then
                    If arrColumnSortOrder(iArr) = DevExpress.Data.ColumnSortOrder.Descending Then
                        gv.Columns(gv.SortedColumns(0).FieldName).SortOrder = DevExpress.Data.ColumnSortOrder.None
                        arrColumnSortOrder(iArr) = DevExpress.Data.ColumnSortOrder.None
                    Else
                        arrColumnSortOrder(iArr) = gv.Columns(gv.SortedColumns(0).FieldName).SortOrder
                    End If
                Else
                    arrColumnSortOrder(iArr) = DevExpress.Data.ColumnSortOrder.None
                End If
            End If
        Catch ex As Exception
            BLCommon.InsertExceptionTrackingBL(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
        End Try
    End Sub

    Private Sub UpdateMultiLangDataSet(ByVal FormName As String)
        Try
            Dim objMLBL As DynamicReportBusinessLayer.PUBLICBO.MultiLanguageBL = New DynamicReportBusinessLayer.PUBLICBO.MultiLanguageBL
            FormName = Replace(FormName, "DynamicDesigner.", "")
            'FormName = Replace(FormName, "SDMS_ResultEntry.", "")
            'FormName = Replace(FormName, "SpreadSheetTemplateBuilder.", "")
            dsMultiLangData = objMLBL.FillMultiLangData(FormName, UserID)
        Catch ex As Exception
            BLCommon.InsertExceptionTrackingBL(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
        End Try
    End Sub
    Public Function GetFormTitle() As String
        Try
            If dsMultiLangData.Tables.Count > 0 AndAlso Me.dsMultiLangData.Tables(0).Rows.Count > 0 Then
                If Not Me.dsMultiLangData.Tables(0).Rows(0)("Form Title") Is Nothing AndAlso Not IsDBNull(Me.dsMultiLangData.Tables(0).Rows(0)("Form Title")) Then
                    Return Me.dsMultiLangData.Tables(0).Rows(0)("Form Title")
                Else
                    Return strEmpty
                End If
            Else
                Return strEmpty
            End If
        Catch ex As Exception
            BLCommon.InsertExceptionTrackingBL(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
            Return strEmpty
        End Try
    End Function
    Public Function GetUserControlCaption() As String
        Try
            If dsMultiLangData.Tables.Count > 0 AndAlso Me.dsMultiLangData.Tables(0).Rows.Count > 0 Then
                If IsDBNull(Me.dsMultiLangData.Tables(0).Rows(0)("User Control Caption")) Then
                    Return strEmpty
                Else
                    Return Me.dsMultiLangData.Tables(0).Rows(0)("User Control Caption")
                End If
            Else
                Return strEmpty
            End If
        Catch ex As Exception
            BLCommon.InsertExceptionTrackingBL(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
            Return strEmpty
        End Try
    End Function
    Public Function GetControlText(ByVal ControlName As String) As String
        Try
            Dim dv As DataView
            If Not dsMultiLangData Is Nothing AndAlso Me.dsMultiLangData.Tables.Count > 1 Then
                dv = New DataView(Me.dsMultiLangData.Tables(1), "[Control Name] = '" & ControlName & "'", "", DataViewRowState.CurrentRows)
                If dv.Count > 0 Then
                    If IsDBNull(dv(0)("ControlText")) OrElse Len(Trim(dv(0)("ControlText"))) = 0 Then
                        Return strEmpty
                    Else
                        If BT_IsRPDorRSD = 1 Then
                            Return Replace(dv(0)("ControlText"), "RPD", "RSD")
                        ElseIf BT_IsRPDorRSD = 2 Then
                            Return Replace(dv(0)("ControlText"), "RPD", "RD")
                        Else
                            Return dv(0)("ControlText")
                        End If
                    End If
                Else
                    Return strEmpty
                End If
            Else
                Return strEmpty
            End If
        Catch ex As Exception
            BLCommon.InsertExceptionTrackingBL(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
            Return strEmpty
        End Try
    End Function
    Public Function GetCustomAppFont(ByVal fntSize As Single, ByVal boolBold As Boolean, ByVal boolItalic As Boolean, ByVal ControlName As String) As System.Drawing.Font
        Try
            Dim drr() As DataRow
            Dim strFont As String = BT_Font
            If Not dsMultiLangData Is Nothing AndAlso Me.dsMultiLangData.Tables.Count > 1 Then
                drr = Me.dsMultiLangData.Tables(1).Select("[Control Name] = '" & ControlName & "'")
                If drr.Length > 0 Then
                    If Not IsDBNull(drr(0)("Font")) AndAlso Len(Trim(drr(0)("Font"))) > 0 Then
                        strFont = Trim(drr(0)("Font"))
                    End If
                    If Not IsDBNull(drr(0)("Size")) AndAlso Len(Trim(drr(0)("Size"))) > 0 AndAlso IsNumeric(drr(0)("Size")) Then
                        fntSize = Trim(drr(0)("Size"))
                    End If
                    If Not IsDBNull(drr(0)("Bold")) AndAlso Len(Trim(drr(0)("Bold"))) > 0 AndAlso Trim(drr(0)("Bold")) = "True" Then
                        boolBold = True
                    End If
                    If Not IsDBNull(drr(0)("Italic")) AndAlso Len(Trim(drr(0)("Italic"))) > 0 AndAlso Trim(drr(0)("Italic")) = "True" Then
                        boolItalic = True
                    End If
                End If
            End If
            If boolBold AndAlso boolItalic Then
                Return New System.Drawing.Font(strFont, fntSize, FontStyle.Bold Or FontStyle.Italic)
            ElseIf boolBold Then
                Return New System.Drawing.Font(strFont, fntSize, FontStyle.Bold)
            ElseIf boolItalic Then
                Return New System.Drawing.Font(strFont, fntSize, FontStyle.Italic)
            Else
                Return New System.Drawing.Font(strFont, fntSize)
            End If
            Return Nothing
        Catch ex As Exception
            BLCommon.InsertExceptionTrackingBL(ex, "Multilanguage", Reflection.MethodInfo.GetCurrentMethod.Name)
            Return Nothing
        End Try
    End Function
    Public Function GetGridCaption(ByVal GridColumnCaption As String) As String
        Try
            Dim dv As DataView
            If Not dsMultiLangData Is Nothing AndAlso Me.dsMultiLangData.Tables.Count > 2 Then
                dv = New DataView(Me.dsMultiLangData.Tables(2), "[GridColumnCaption] = '" & GridColumnCaption & "'", "", DataViewRowState.CurrentRows)
                If dv.Count > 0 Then
                    If IsDBNull(dv(0)("CurrentColumnCaption")) OrElse Len(Trim(dv(0)("CurrentColumnCaption"))) = 0 Then
                        Return strEmpty
                    Else
                        If BT_IsRPDorRSD = 1 Then
                            Return Replace(dv(0)("CurrentColumnCaption"), "RPD", "RSD")
                        ElseIf BT_IsRPDorRSD = 2 Then
                            Return Replace(dv(0)("CurrentColumnCaption"), "RPD", "RD")
                        Else
                            Return dv(0)("CurrentColumnCaption")
                        End If
                    End If
                Else
                    Return strEmpty
                End If
            Else
                Return strEmpty
            End If
        Catch ex As Exception
            BLCommon.InsertExceptionTrackingBL(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
            Return strEmpty
        End Try
    End Function
    'Public Sub GetGridCaption(ByVal gv As DevExpress.XtraGrid.Views.Grid.GridView)
    '    Try
    '        If arrEventList IsNot Nothing Then
    '            If arrEventList.IndexOf(gv.Name) < 0 Then
    '                arrEventList.Add(gv.Name)
    '                arrColumnSortOrder.Add(DevExpress.Data.ColumnSortOrder.None)
    '                AddHandler gv.EndSorting, AddressOf gv_EndSorting
    '            End If
    '        Else
    '            arrEventList = New ArrayList
    '            arrColumnSortOrder = New ArrayList
    '            arrEventList.Add(gv.Name)
    '            arrColumnSortOrder.Add(DevExpress.Data.ColumnSortOrder.None)
    '            AddHandler gv.EndSorting, AddressOf gv_EndSorting
    '        End If

    '        Dim dv As DataView
    '        Dim gc As DevExpress.XtraGrid.Columns.GridColumn
    '        If Not gv Is Nothing Then
    '            If Not dsMultiLangData Is Nothing AndAlso Me.dsMultiLangData.Tables.Count > 2 Then
    '                For Each gc In gv.Columns
    '                    Dim strCaption As String = String.Empty
    '                    If Len(gc.Caption) > 0 Then
    '                        strCaption = gc.Caption
    '                    Else
    '                        strCaption = gc.FieldName
    '                    End If
    '                    dv = New DataView(Me.dsMultiLangData.Tables(2), "[GridColumnCaption] = '" & strCaption & "'", "", DataViewRowState.CurrentRows)
    '                    If dv.Count > 0 Then
    '                        If IsDBNull(dv(0)("CurrentColumnCaption")) OrElse Len(Trim(dv(0)("CurrentColumnCaption"))) = 0 Then
    '                            gc.Caption = strEmpty
    '                        Else
    '                            If BT_IsRPDorRSD = 1 Then
    '                                gc.Caption = Replace(dv(0)("CurrentColumnCaption"), "RPD", "RSD")
    '                            ElseIf BT_IsRPDorRSD = 2 Then
    '                                gc.Caption = Replace(dv(0)("CurrentColumnCaption"), "RPD", "RD")
    '                            Else
    '                                gc.Caption = dv(0)("CurrentColumnCaption")
    '                            End If
    '                        End If
    '                    Else
    '                        gc.Caption = strEmpty
    '                    End If
    '                Next gc
    '            Else
    '                For Each gc In gv.Columns
    '                    gc.Caption = strEmpty
    '                Next gc
    '            End If
    '        End If
    '    Catch ex As Exception
    '        BLCommon.InsertExceptionTrackingBL(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
    '    End Try
    'End Sub
    'Public Sub GetGridCaption(ByVal gv As DevExpress.XtraGrid.Views.Card.CardView)
    '    Try
    '        Dim dv As DataView
    '        Dim gc As DevExpress.XtraGrid.Columns.GridColumn
    '        If Not gv Is Nothing Then
    '            If Not dsMultiLangData Is Nothing AndAlso Me.dsMultiLangData.Tables.Count > 2 Then
    '                For Each gc In gv.Columns
    '                    Dim strCaption As String = String.Empty
    '                    If Len(gc.Caption) > 0 Then
    '                        strCaption = gc.Caption
    '                    Else
    '                        strCaption = gc.FieldName
    '                    End If
    '                    dv = New DataView(Me.dsMultiLangData.Tables(2), "[GridColumnCaption] = '" & strCaption & "'", "", DataViewRowState.CurrentRows)
    '                    If dv.Count > 0 Then
    '                        If IsDBNull(dv(0)("CurrentColumnCaption")) OrElse Len(Trim(dv(0)("CurrentColumnCaption"))) = 0 Then
    '                            gc.Caption = strEmpty
    '                        Else
    '                            If BT_IsRPDorRSD = 1 Then
    '                                gc.Caption = Replace(dv(0)("CurrentColumnCaption"), "RPD", "RSD")
    '                            ElseIf BT_IsRPDorRSD = 2 Then
    '                                gc.Caption = Replace(dv(0)("CurrentColumnCaption"), "RPD", "RD")
    '                            Else
    '                                gc.Caption = dv(0)("CurrentColumnCaption")
    '                            End If
    '                        End If
    '                    Else
    '                        gc.Caption = strEmpty
    '                    End If
    '                Next gc
    '            Else
    '                For Each gc In gv.Columns
    '                    gc.Caption = strEmpty
    '                Next gc
    '            End If
    '        End If
    '    Catch ex As Exception
    '        BLCommon.InsertExceptionTrackingBL(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
    '    End Try
    'End Sub
    Public Function GetMessageBoxText(ByVal MessageKey As String) As String
        Try
            Dim dv As DataView
            If Not dsMultiLangData Is Nothing AndAlso Me.dsMultiLangData.Tables.Count > 3 Then
                dv = New DataView(Me.dsMultiLangData.Tables(3), "[Message Key] = '" & MessageKey & "'", "", DataViewRowState.CurrentRows)
                If dv.Count > 0 Then
                    If IsDBNull(dv(0)("Message")) OrElse Len(Trim(dv(0)("Message"))) = 0 Then
                        Return strEmpty
                    Else
                        If BT_IsRPDorRSD = 1 Then
                            Return Replace(dv(0)("Message"), "RPD", "RSD")
                        ElseIf BT_IsRPDorRSD = 2 Then
                            Return Replace(dv(0)("Message"), "RPD", "RD")
                        Else
                            Return dv(0)("Message")
                        End If
                    End If
                Else
                    Return strEmpty
                End If
            Else
                Return strEmpty
            End If
        Catch ex As Exception
            BLCommon.InsertExceptionTrackingBL(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
            Return strEmpty
        End Try
    End Function

End Class
'End Namespace