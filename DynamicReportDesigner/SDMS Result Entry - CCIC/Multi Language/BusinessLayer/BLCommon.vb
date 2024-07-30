Imports DynamicReportDataLayer

Public Module BLCommon

    Public ReadOnly Property SQLUserID() As String
        Get
            Return DynamicReportDataLayer.ConnectionSettings.SQLUserID
        End Get
    End Property
    Public Function ExecuteDataTable(ByVal strQuery As String) As DataTable
        Return SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.Text, strQuery)
    End Function
    Public ReadOnly Property SQLPassword() As String
        Get
            Return DynamicReportDataLayer.ConnectionSettings.SQLPassword
        End Get
    End Property
    Public Function ExecuteDataSet(ByVal strQuery As String) As DataSet
        Return SqlHelper.ExecuteDataset(ConnectionSettings.cnString, CommandType.Text, strQuery)
    End Function
    Public ReadOnly Property SQLServerName() As String
        Get
            Return DynamicReportDataLayer.ConnectionSettings.SQLServerName
        End Get
    End Property
    Public ReadOnly Property SQLDatabaseName() As String
        Get
            Return DynamicReportDataLayer.ConnectionSettings.SQLDatabaseName
        End Get
    End Property
    Public Function Public_UserSign(ByVal coluqSampleParameterID As String) As DataSet
        Return DataFunctions.Public_UserSign(coluqSampleParameterID)
    End Function
    Public Function GetUserFullname(ByVal LoginUser As String) As DataTable
        Return DataFunctions.GetUserFullName(LoginUser)
    End Function
    Public Function CurrentUserSign(ByVal UserName As String) As DataTable
        Return DataFunctions.CurrentUserSign(UserName)
    End Function
    Public Function GetSDMSImages(ByVal strABID As String) As DataTable
        Try
            Dim dt As New DataTable
            dt = DataFunctions.GetSDMSImages(strABID)
            Return dt

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
    End Function
    Public Function GetDefaultCompuntListName() As String
        Return DataFunctions.GetDefaultCompuntListName()
    End Function
    Public Function GetDefaultConstantByVal(ByVal Constant As String) As String
        Return DataFunctions.GetDefaultConstantByVal(Constant)
    End Function
    Public Function GetDefaultRPDSource() As String
        Return DataFunctions.GetDefaultRPDSource()
    End Function
    Public Function GetHoursbyTAT(ByVal TAT As String, ByVal choice As String) As DataTable
        Return DataFunctions.GetHoursbyTAT(TAT, choice)
    End Function
    Public Sub SetCurrentUser(ByVal UserName As String)
        CurrentUserName = UserName
    End Sub
    Public Function GetControlCaptionByFormName(ByVal FormName As String) As DataSet
        Return DataFunctions.GetControlCaptionByFormName(FormName)
    End Function
    Public Function GetFtpFormFolderPath(ByVal FormName As String) As String
        Return DataFunctions.GetFtpFormFolderPath(FormName)
    End Function
    Public Function GetFtpConection() As DataTable
        Return DataFunctions.GetFtpConection()
    End Function
    Public Function GetDashBoardConnection() As DataTable
        Return DataFunctions.GetDashBoardConnection()
    End Function
    Public Sub UpdateDashBoardConection(ByVal ServerName As String, ByVal DataBaseName As String, ByVal UserName As String, ByVal Password As String)
        DataFunctions.UpdateDashBoardConnection(ServerName, DataBaseName, UserName, Password)
    End Sub
    Public Sub InsertDashBoardConection(ByVal ServerName As String, ByVal DataBaseName As String, ByVal UserName As String, ByVal Password As String)
        DataFunctions.InsertDashBoardConnection(ServerName, DataBaseName, UserName, Password)
    End Sub
    Public Function GetCurrentVersion() As DataTable
        Return DataFunctions.GetCurrentVersion()
    End Function
    Public Sub InsertFtpConection(ByVal ServerName As String, ByVal UserName As String, ByVal Password As String, ByVal FTPPath As String)
        DataFunctions.InsertFtpConection(ServerName, UserName, Password, FTPPath)
    End Sub
    Public Sub UpdateJobIDSampleType(ByVal SampleType As String)
        DataFunctions.UpdateJobIDSampleType(SampleType)
    End Sub
    Public Sub UpdateLanguage(ByVal Language As String)
        DataFunctions.UpdateLanguage(Language)
    End Sub
    Public Sub UpdateFtpConection(ByVal ServerName As String, ByVal UserName As String, ByVal Password As String, ByVal FTPPath As String)
        DataFunctions.UpdateFtpConection(ServerName, UserName, Password, FTPPath)
    End Sub
    Public Sub InsertExceptionTrackingBL(ByVal Ex As Exception, Optional ByVal strFormName As String = "", Optional ByVal strFunctionName As String = "")
        InsertExceptionTrackingDC(Ex, strFormName, strFunctionName)
    End Sub
    Public Sub InsertExceptionTrackingBL(ByVal Ex As String, Optional ByVal strFormName As String = "", Optional ByVal strFunctionName As String = "")
        InsertExceptionTrackingDC(Ex, strFormName, strFunctionName)
    End Sub
    Public Function CheckFullPremission(ByVal UserName As String) As Boolean
        Return DataFunctions.CheckFullPremission(UserName)
    End Function
    Public Sub InsertExceptionTrackingFormBL(ByVal UserID As Integer, ByVal VersionNo As String)
        InsertExceptionTrackingFormDC(UserID, VersionNo)
    End Sub
    Public Sub UpdateExceptionTrackingFormBL(ByVal UserID As Integer, ByVal strFormName As String)
        UpdateExceptionTrackingFormDC(UserID, strFormName)
    End Sub
    Public Sub RemoveExceptionTrackingFormBL(ByVal UserID As Integer)
        RemoveExceptionTrackingFormDC(UserID)
    End Sub
    Public Function EBF_GetReportingMatrix(ByVal Matrix As String) As DataTable
        Return DataFunctions.EBF_GetReportingMatrix(Matrix)
    End Function

    Public Function GetUnits() As DataTable
        Return GeneralDataFunction.GetUnits
    End Function
    Public Function GetParameters() As DataTable
        Return GeneralDataFunction.GetGetParameters
    End Function
    Public Function GetTAT() As DataTable
        Return GeneralDataFunction.GetTAT
    End Function
    Public Function GetUser() As DataTable
        Return GeneralDataFunction.GetUser
    End Function

    Public Function GetMatrix() As DataTable
        Return GeneralDataFunction.GetMatrix
    End Function
    Public Function GetLIMS() As DataTable
        Return GeneralDataFunction.GetLIMS
    End Function

    Public Function GetQueryData(ByVal qk As String) As DataTable
        Return DataFunctions.GetQueryDataDF(qk)
    End Function
    Public Function Public_CheckUserFullAccess(ByVal UserName As String) As Integer
        Return DataFunctions.Public_CheckUserFullAccess(UserName)
    End Function
    Public Function GetData(ByVal query As String) As DataTable
        Return DataManager.ExecuteQuery(query)
    End Function
    Public Function GetServerTimes() As DateTime
        Return DynamicReportDataLayer.DataFunctions.GetServerTimes
    End Function
    Public Function GetQcTypeRPDSource(ByVal QctypeID As Integer) As String
        Return DynamicReportDataLayer.DataFunctions.GetQcTypeRPDSource(QctypeID)
    End Function
    Public Function GetUsersByPermission(ByVal ModuleName As String, ByVal FormName As String, ByVal Permission As String) As DataTable
        Return DynamicReportDataLayer.DataFunctions.GetUsersByPermission(ModuleName, FormName, Permission)
    End Function
    Public Function CheckQualifier(ByVal Qualifier As String) As Integer
        Return DynamicReportDataLayer.DataFunctions.CheckQualifier(Qualifier)
    End Function
    Public Function getDate(ByVal coluqSampleTestID As String) As DataSet
        Return DynamicReportDataLayer.DataFunctions.getDate(coluqSampleTestID)
    End Function
    Public Function GetConnectionString() As String
        Return DynamicReportDataLayer.ConnectionSettings.cnString
    End Function
    Public Function GetUserPerCheck(ByVal ModuleName As String, ByVal FormName As String, ByVal Permission As Integer) As DataTable
        Return DynamicReportDataLayer.DataFunctions.GetUserPerCheck(ModuleName, FormName, Permission)
    End Function
    Public Function GetConnectionJobIDType() As String
        Return DataFunctions.GetConnectionJobIDType()
    End Function
    Public Function EBF_CheckValidation(ByVal Type As String, ByVal uqQCBatchID As Integer, ByVal QCSampleID As Integer, ByVal QCTypeID As Integer) As Integer
        '  Dim Y As New DataLayer.CODStraightTitrationDC
        Return DataFunctions.EBF_CheckValidation(Type, uqQCBatchID, QCSampleID, QCTypeID)
    End Function
    Public Function EBF_DeleteSampleResult(ByVal drSample() As DataRow, ByVal Mode As String, ByVal strUserName As String, ByVal strFormName As String) As Boolean
        ' Dim Y As New DataLayer.CODStraightTitrationDC
        Return DataFunctions.EBF_DeleteSampleResult(drSample, Mode, strUserName, strFormName)
    End Function
    Public Function OneClickStatusReturnBL(ByVal dv As DataView) As Boolean
        Try
            Return DataFunctions.OneClickRollback(dv)
        Catch ex As Exception

        End Try
    End Function
    Public Function EBF_DeleteQCSampleResult(ByVal dr As DataRow, ByVal Type As String) As Boolean
        '   Dim Y As New DataLayer.CODStraightTitrationDC
        Return DataFunctions.EBF_DeleteQCSampleResult(dr, Type)
    End Function
    Public Function ICM_CountPendingDetails(ByVal strMode As String) As DataTable
        Return DataFunctions.ICM_CountPendingDetails(strMode)
    End Function
    Public Function Permission(ByVal ModuleName As String, ByVal FormName As String, ByVal UserID As Integer) As Integer
        Try
            If UserID = 1 OrElse UCase(CurrentUserName) = "SERVICE" Then Return 1
            Dim q1 As String = " select distinct colrights from tbl_module a inner join tbl_rights b on a.colmoduleid = b.colmoduleid " & _
                               " where b.coluserid = " & UserID
            ' " where a.[colmodulename-EN] = '" & ModuleName & "' and b.coluserid = " & UserID
            Dim q2 As String
            Dim dtAdminPermission As DataTable = DataManager.ExecuteQuery(q1)
            'Power User code Change
            'Code change by Bala
            'If dtAdminPermission.Rows.Count > 0 Then

            If dtAdminPermission.Rows.Count > 0 AndAlso 1 = 2 Then
                If dtAdminPermission.Rows(0)(0) = 1 Then
                    Return 1
                End If
            Else
                'If StrComp(ModuleName, "ICM", CompareMethod.Text) = 0 Then
                '    q2 = " select count(*) from tbl_formpermission a " & _
                '        " inner join tbl_form b on a.colformid = b.colformid " & _
                '        " inner join tbl_module c on a.colmoduleid = c.colmoduleid " & _
                '        " where a.coluserid = " & UserID & " and c.colmodulename ='" & ModuleName & _
                '        "' and colformname = '" & FormName & "' "

                '    Dim dtPermission As DataTable = DataManager.ExecuteQuery(q2)
                '    If dtPermission.Rows.Count > 0 Then
                '        Return Val(dtPermission.Rows(0)(0))
                '    Else
                '        Return 0
                '    End If
                'Else
                'q2 = " select colpermissionvalue from tbl_form a inner join tbl_userpermission b " & _
                '    " on a.colmoduleid = b.colmoduleid and a.colformid = b.colformid " & _
                '    " inner join tbl_module c on b.colmoduleid = c.colmoduleid " & _
                '    " where [colformname-EN] = '" & FormName & "' and b.coluserid = " & UserID & _
                '    " and c.[colmodulename-EN] = '" & ModuleName & "'"

                'q2 = " select colpermissionvalue from tbl_form a " & _
                '     " inner join tbl_module c on c.colmoduleid = a.colmoduleid " & _
                '     " inner join tbl_userpermission b  on a.colmoduleid = b.colmoduleid and a.colformid = b.colformid " & _
                '     " where [colformname-EN] ='" & IIf(FormName = "ReportingV5", "Reporting", FormName) & "' and b.coluserid = " & UserID & _
                '     " and c.[colmodulename-EN] = '" & ModuleName & "'"
                q2 = " select colpermissionvalue from tbl_form a " & _
                     " inner join tbl_userpermission b  on a.colformid = b.colformid " & _
                   " where [colformname-EN] ='" & IIf(FormName = "ReportingV5", "Reporting", FormName) & "' and b.coluserid = " & UserID & ""

                Dim dtPermission As DataTable = DataManager.ExecuteQuery(q2)
                If dtPermission.Rows.Count > 0 Then
                    Return Val(dtPermission.Rows(0)(0))
                Else
                    Return 2
                End If
                'End If
            End If
        Catch ex As Exception
            Return 0
        End Try
    End Function
    Public Function PermissionByModuleID(ByVal ModuleName As Integer, ByVal FormName As String, ByVal UserID As Integer) As Integer
        Try
            If UserID = 1 OrElse UCase(CurrentUserName) = "SERVICE" Then Return 1
            Dim q1 As String = " select colrights from tbl_module a inner join tbl_rights b on a.colmoduleid = b.colmoduleid " & _
                               " where b.coluserid = " & UserID
            '" where a.[colModuleID] = " & ModuleName & " and b.coluserid = " & UserID
            Dim q2 As String
            Dim dtAdminPermission As DataTable = DataManager.ExecuteQuery(q1)

            'Power User code Change
            'Code change by Bala
            'If dtAdminPermission.Rows.Count > 0  Then
            If dtAdminPermission.Rows.Count > 0 AndAlso 1 = 2 Then
                If dtAdminPermission.Rows(0)(0) = 1 Then
                    Return 1
                End If
            Else
                'If StrComp(ModuleName, "ICM", CompareMethod.Text) = 0 Then
                '    q2 = " select count(*) from tbl_formpermission a " & _
                '        " inner join tbl_form b on a.colformid = b.colformid " & _
                '        " inner join tbl_module c on a.colmoduleid = c.colmoduleid " & _
                '        " where a.coluserid = " & UserID & " and c.colmodulename ='" & ModuleName & _
                '        "' and colformname = '" & FormName & "' "

                '    Dim dtPermission As DataTable = DataManager.ExecuteQuery(q2)
                '    If dtPermission.Rows.Count > 0 Then
                '        Return Val(dtPermission.Rows(0)(0))
                '    Else
                '        Return 0
                '    End If
                'Else
                'q2 = " select colpermissionvalue from tbl_form a inner join tbl_userpermission b " & _
                '    " on a.colmoduleid = b.colmoduleid and a.colformid = b.colformid " & _
                '    " inner join tbl_module c on b.colmoduleid = c.colmoduleid " & _
                '    " where [colformname-EN] = '" & FormName & "' and b.coluserid = " & UserID & _
                '    " and c.[colmodulename-EN] = '" & ModuleName & "'"

                q2 = " select colpermissionvalue from tbl_form a " & _
                     " inner join tbl_module c on c.colmoduleid = a.colmoduleid " & _
                     " inner join tbl_userpermission b  on a.colmoduleid = b.colmoduleid and a.colformid = b.colformid " & _
                     " where [colformname-EN] ='" & IIf(FormName = "ReportingV5", "Reporting", FormName) & "' and b.coluserid = " & UserID & _
                     " and c.[colModuleID] = " & ModuleName

                Dim dtPermission As DataTable = DataManager.ExecuteQuery(q2)
                If dtPermission.Rows.Count > 0 Then
                    Return Val(dtPermission.Rows(0)(0))
                Else
                    Return 2
                End If
                'End If
            End If
        Catch ex As Exception
            Return 0
        End Try
    End Function
    Public Sub SetDBConnection(Optional ByVal boolNetworkLibrary As Boolean = True)
        DynamicReportDataLayer.Utils.ReadConfigFile(boolNetworkLibrary)
    End Sub
    Public Sub SetDBConnection(ByVal LDMSQLServerName As String, ByVal LDMSQLDatabaseName As String, ByVal LDMSQLUserID As String, ByVal LDMSQLPassword As String)
        DynamicReportDataLayer.Utils.SetLDM_WEB_ConnectionString(LDMSQLServerName, LDMSQLDatabaseName, LDMSQLUserID, LDMSQLPassword)
    End Sub
    Public Function InsertWebServiceURL(ByVal URL As String) As Boolean
        Return DataFunctions.InsertWebServiceURL(URL)
    End Function
    Public Function GetSQLConnection() As String
        Return DataFunctions.GetSQLConnection
    End Function
    Public Function GetSQLConnectionReportUPdate() As String
        Return DataFunctions.GetSQLConnectionReportUPdate
    End Function
    Public Function GetSQLConnectionWithoutPasswordReportUpdate() As String
        Return DataFunctions.GetSQLConnectionWithoutPasswordReportUpdate
    End Function
    Public Function GetSQLConnectionWithOutPassword() As String
        Return DataFunctions.GetSQLConnectionWithOutPassword
    End Function

    Public Sub ExecuteNonQuery(ByVal query As String)
        DataManager.ExecuteNonQuery(query)
    End Sub

    Public Function SelectSourceSampleTestResult(ByVal coluqTestParameterID As Integer, ByVal coluqSampleParameterID As Integer, ByVal coluqQCBatchID As Integer, ByVal coluqQCTypeID As Integer) As DataTable
        Return DataFunctions.SelectSourceSampleTestResult(coluqTestParameterID, coluqSampleParameterID, coluqQCBatchID, coluqQCTypeID)
    End Function
    Public Function SelectSourceTemplateColumnResult(ByVal coluqTestParameterID As Integer, ByVal coluqSampleParameterID As Integer, ByVal coluqQCBatchID As Integer, ByVal coluqQCTypeID As Integer, ByVal SourceTemplateFormulaColumnID As Integer, ByVal strQCSourceSampleID As String, ByVal SampleRunNo As Integer, ByVal QcRunNo As Integer) As DataTable
        Return DynamicReportDataLayer.DataFunctions.SelectSourceTemplateColumnResult(coluqTestParameterID, coluqSampleParameterID, coluqQCBatchID, coluqQCTypeID, SourceTemplateFormulaColumnID, strQCSourceSampleID, SampleRunNo, QcRunNo)
    End Function
    Public Function SelectSignature(ByVal UserID As String) As DataTable
        Try
            Return DataFunctions.SelectSignature(UserID)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function ExecuteQueryCode(ByVal strQuery As String) As DataTable
        Try
            Return DataFunctions.ExecuteQueryCode(strQuery)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Sub UpdateDS(ByVal mode As String, ByVal val As Integer)
        Try
            DataFunctions.UpdateDS(mode, val)
        Catch ex As Exception
        End Try
    End Sub

    Public Function InsertLimsAuditTrailBulkpost(ByVal dt As DataTable) As Boolean
        Try
            If Not dt Is Nothing And dt.Rows.Count > 0 Then
                If dt.Rows(0)("ColActionType") = "Delete" Then
                    Return DataFunctions.InsertLimsAuditTrailBulkpost(dt)
                Else
                    Dim dv As New DataView(dt, "colOldValue is not null and len(colOldValue) > 0 ", "", DataViewRowState.CurrentRows) 'Update
                    Dim dt1 As DataTable = dv.ToTable
                    If Not dt1 Is Nothing And dt1.Rows.Count > 0 Then
                        Return DataFunctions.InsertLimsAuditTrailBulkpost(dt1)
                    Else
                        Return False
                    End If
                End If
            End If
        Catch ex As Exception
            Return Nothing
        End Try

        'Try
        '    Return DataFunctions.InsertLimsAuditTrailBulkpost(dt)
        'Catch ex As Exception
        '    Return Nothing
        'End Try
    End Function
    Public Function InsertQCBulkpost(ByVal qcDt As DataTable) As Boolean
        Try
            If Not qcDt Is Nothing AndAlso qcDt.Rows.Count > 0 Then
                Return DataFunctions.InsertQCBulkpost(qcDt)
            End If
        Catch ex As Exception

        End Try
    End Function

    Public Function GetunCollectedSampleInformation(ByVal strJobID As String) As DataTable
        Return DataFunctions.GetunCollectedSampleInformation(strJobID)
    End Function
    Public Function InsertELNBulkpost(ByVal dtELN As DataTable, ByVal DestinationTableName As String) As Boolean
        Try
            If Not dtELN Is Nothing AndAlso dtELN.Rows.Count > 0 Then
                Return DataFunctions.InsertELNBulkpost(dtELN, DestinationTableName)
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Sub SetConnectionString(ByVal ConSDMS As String)
        ConnectionSettings.cnString = ConSDMS
    End Sub

    Public Function GetQcComboReportTRRp_DataSet(ByVal SPName As String, ByVal SampleID As String, ByVal SampleParameterID As String, ByVal TestMethodID As String, ByVal ParameterID As String) As DataSet
        Dim Y As New DynamicReportDataLayer.BTDataTableConverter
        Return Y.GetQcComboReportTRRp_DataSet(SPName, SampleID, SampleParameterID, TestMethodID, ParameterID)
    End Function

End Module
