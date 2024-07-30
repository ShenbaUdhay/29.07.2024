Imports System.ComponentModel
Imports System.Data.SqlClient

Public Module DataFunctions
    Private con As SqlConnection
    Private Trans As SqlTransaction
    Public CurrentUserName As String
    Public Sub InsertExceptionTrackingDC(ByVal Ex As Exception, Optional ByVal strFormName As String = "", Optional ByVal strFunctionName As String = "")
        Try
            Dim params() As SqlParameter = New SqlParameter(4) {}
            params(0) = New SqlParameter("@colErrorMessage", Ex.Message)
            params(1) = New SqlParameter("@colStackTrace", Ex.StackTrace)
            params(2) = New SqlParameter("@colLoginBy", CurrentUserName)
            params(3) = New SqlParameter("@FormName", strFormName)
            params(4) = New SqlParameter("@FunctionName", strFunctionName)
            SqlHelper.ExecuteNonQuery(ConnectionSettings.cnString, CommandType.StoredProcedure, "ET_InsertExceptionTracking_SP", params)
        Catch ex1 As Exception
        End Try
    End Sub
    Public Function GetQcTypeRPDSource(ByVal QctypeID As Integer) As String
        Try
            Dim params() As SqlParameter = New SqlParameter(0) {}
            params(0) = New SqlParameter("@QCTypeID", QctypeID)
            Dim dt As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "EBF_GetQytpeRPDSource_SP", params)
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                Return dt.Rows(0)(0)
            Else
                Return ""
            End If
            Return True
        Catch ex1 As Exception
            Return ""
        End Try
    End Function
    Public Function GetUserFullName(ByVal LoginUser As String) As DataTable
        Try
            Dim params() As SqlParameter = New SqlParameter(0) {}
            params(0) = New SqlParameter("@LoginUser", LoginUser)
            Return SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "[PUBLIC_CurrentUserFullname_SP]", params)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function CurrentUserSign(ByVal UserName As String) As DataTable
        Try
            Dim params1() As SqlParameter = New SqlParameter(0) {}
            params1(0) = New SqlParameter("@UserName", UserName)
            Return SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure,
                                               "GetUserSign", params1)
        Catch ex As Exception
        End Try
    End Function
    Public Function GetSDMSImages(ByVal strABID As String) As DataTable
        Try
            Dim params1() As SqlParameter = New SqlParameter(0) {}
            params1(0) = New SqlParameter("@ABID", strABID)
            Return SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure,
                                               "GetSDMSImages_SP", params1)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Sub InsertExceptionTrackingDC(ByVal Ex As String, Optional ByVal strFormName As String = "", Optional ByVal strFunctionName As String = "")
        Try
            Dim params() As SqlParameter = New SqlParameter(4) {}
            params(0) = New SqlParameter("@colErrorMessage", Ex)
            params(1) = New SqlParameter("@colStackTrace", "")
            params(2) = New SqlParameter("@colLoginBy", CurrentUserName)
            params(3) = New SqlParameter("@FormName", strFormName)
            params(4) = New SqlParameter("@FunctionName", strFunctionName)
            SqlHelper.ExecuteNonQuery(ConnectionSettings.cnString, CommandType.StoredProcedure, "ET_InsertExceptionTracking_SP", params)
        Catch ex1 As Exception
        End Try
    End Sub

    Public Function CheckFullPremission(ByVal UserName As String) As Boolean
        Try
            Dim params() As SqlParameter = New SqlParameter(0) {}
            params(0) = New SqlParameter("@UserName", UserName)
            Dim dt As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "GetFullAccess_SP", params)
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Sub InsertExceptionTrackingFormDC(ByVal UserID As Integer, ByVal VersionNo As String)
        Try
            Dim params() As SqlParameter = New SqlParameter(1) {}
            params(0) = New SqlParameter("@UserID", UserID)
            params(1) = New SqlParameter("@VersionNo", VersionNo)
            SqlHelper.ExecuteNonQuery(ConnectionSettings.cnString, CommandType.StoredProcedure, "ET_InsertExceptionTrackingForm_SP", params)
        Catch ex1 As Exception
        End Try
    End Sub
    Public Sub UpdateExceptionTrackingFormDC(ByVal UserID As Integer, ByVal strFormName As String)
        Try
            Dim params() As SqlParameter = New SqlParameter(1) {}
            params(0) = New SqlParameter("@UserID", UserID)
            params(1) = New SqlParameter("@FormName", strFormName)
            SqlHelper.ExecuteNonQuery(ConnectionSettings.cnString, CommandType.StoredProcedure, "ET_UpdateExceptionTrackingForm_SP", params)
        Catch ex1 As Exception
        End Try
    End Sub
    Public Sub RemoveExceptionTrackingFormDC(ByVal UserID As Integer)
        Try
            Dim params() As SqlParameter = New SqlParameter(1) {}
            params(0) = New SqlParameter("@UserID", UserID)
            SqlHelper.ExecuteNonQuery(ConnectionSettings.cnString, CommandType.StoredProcedure, "ET_RemoveExceptionTrackingForm_SP", params)
        Catch ex1 As Exception
        End Try
    End Sub
    Public Function EBF_CheckValidation(ByVal Type As String, ByVal uqQCBatchID As Integer, ByVal QCSampleID As Integer, ByVal QCTypeID As Integer) As Integer
        Try
            Dim params() As SqlParameter = New SqlParameter(3) {}
            params(0) = New SqlParameter("@Type", Type)
            params(1) = New SqlParameter("@QCBatchID", uqQCBatchID)
            params(2) = New SqlParameter("@QCSampleID", QCSampleID)
            params(3) = New SqlParameter("@QCTypeID", QCTypeID)
            Dim Count As Integer = SqlHelper.ExecuteScalar(ConnectionSettings.cnString, _
            CommandType.StoredProcedure, "EBF_QCSampleCheckStatus_SP", params)
            Return Count
        Catch ex As Exception
            Return 0
        End Try
    End Function
    Public Function EBF_DeleteSampleResult(ByVal drSample() As DataRow, ByVal Mode As String, ByVal strUserName As String, ByVal strFormName As String) As Boolean
        Try
            If Not drSample Is Nothing AndAlso drSample.Length Then
                Dim dr As DataRow
                For Each dr In drSample
                    If dr("All") = True Then
                        Dim params() As SqlParameter = New SqlParameter(1) {}
                        If dr.Table.Columns.Contains("coluqSampleParameterID") = True Then
                            params(0) = New SqlParameter("@coluqSampleParameterID", dr("coluqSampleParameterID"))
                        End If
                        If dr.Table.Columns.Contains("SampleParameterID") = True Then
                            params(0) = New SqlParameter("@coluqSampleParameterID", dr("SampleParameterID"))
                        End If
                        params(1) = New SqlParameter("@colMode", Mode)
                        SqlHelper.ExecuteNonQuery(ConnectionSettings.cnString, _
                        CommandType.StoredProcedure, "RE_DeleteSampleResult_SP", params)

                        Dim params1() As SqlParameter = New SqlParameter(3) {}
                        If dr.Table.Columns.Contains("coluqSampleTestID") = True Then
                            params1(0) = New SqlParameter("@coluqSampleTestID", dr("coluqSampleTestID"))
                        End If
                        If dr.Table.Columns.Contains("SampleTestID") = True Then
                            params1(0) = New SqlParameter("@coluqSampleTestID", dr("SampleTestID"))
                        End If
                        params1(1) = New SqlParameter("@colFunctionTypeID", 5)
                        params1(2) = New SqlParameter("@colUser", strUserName)
                        params1(3) = New SqlParameter("@colFormName", strFormName)
                        SqlHelper.ExecuteNonQuery(ConnectionSettings.cnString, _
                        CommandType.StoredProcedure, "Env_SetDeleteStatus_SP", params1)
                    End If
                Next
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function EBF_DeleteQCSampleResult(ByVal dr As DataRow, ByVal Type As String) As Boolean
        Try
            Dim params() As SqlParameter = New SqlParameter(5) {}
            params(0) = New SqlParameter("@Type", Type)
            If Type = "Blank" Then
                If dr.Table.Columns.Contains("coluqTestParameterID") = True Then
                    params(1) = New SqlParameter("@QCSampleID", dr("coluqTestParameterID"))
                ElseIf dr.Table.Columns.Contains("TestParameterID") = True Then
                    params(1) = New SqlParameter("@QCSampleID", dr("TestParameterID"))
                End If
            ElseIf Type = "Spike" Then
                If dr.Table.Columns.Contains("coluqTestQCSpike") = True Then
                    params(1) = New SqlParameter("@QCSampleID", dr("coluqTestQCSpike"))
                ElseIf dr.Table.Columns.Contains("TestQCSpike") = True Then
                    params(1) = New SqlParameter("@QCSampleID", dr("TestQCSpike"))
                End If
            ElseIf Type = "Standard" Then
                If dr.Table.Columns.Contains("coluqTestQCStandardSpikeID") = True Then
                    params(1) = New SqlParameter("@QCSampleID", dr("coluqTestQCStandardSpikeID"))
                ElseIf dr.Table.Columns.Contains("TestQCstdSpikeID") = True Then
                    params(1) = New SqlParameter("@QCSampleID", dr("TestQCstdSpikeID"))
                End If
            End If
            If dr.Table.Columns.Contains("coluqQCTypeID") = True Then
                params(2) = New SqlParameter("@QCTypeID", dr("coluqQCTypeID"))
            ElseIf dr.Table.Columns.Contains("uqQCTypeID") = True Then
                params(2) = New SqlParameter("@QCTypeID", dr("uqQCTypeID"))
            End If
            If dr.Table.Columns.Contains("coluqQCBatchID") = True Then
                params(3) = New SqlParameter("@uqQCBatchID", dr("coluqQCBatchID"))
            ElseIf dr.Table.Columns.Contains("uqQCBatchID") = True Then
                params(3) = New SqlParameter("@uqQCBatchID", dr("uqQCBatchID"))
            End If
            params(4) = New SqlParameter("@QCRunNo", dr("QCRunNo"))
            params(5) = New SqlParameter("@FormName", "ElNResultEntry")
            SqlHelper.ExecuteNonQuery(ConnectionSettings.cnString, _
            CommandType.StoredProcedure, "EBF_DeleteQCSampleResult_SP", params)

            'Dim params1() As SqlParameter = New SqlParameter(1) {}
            'If dr.Table.Columns.Contains("coluqQCBatchID") = True Then
            '    params1(0) = New SqlParameter("@QCBatch", dr("coluqQCBatchID"))
            'ElseIf dr.Table.Columns.Contains("uqQCBatchID") = True Then
            '    params1(0) = New SqlParameter("@QCBatch", dr("uqQCBatchID"))
            'End If
            'params1(1) = New SqlParameter("@QCType", dr("RunType"))
            'SqlHelper.ExecuteNonQuery(ConnectionSettings.cnString, _
            '   CommandType.StoredProcedure, "EBF_UpdateQCBatchQCType_SP", params1)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function ICM_CountPendingDetails(ByVal strMode As String) As DataTable
        Try
            Dim params() As SqlParameter = New SqlParameter(0) {}
            params(0) = New SqlParameter("@colMode", strMode)
            Return SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "ICM_CountPendingDetails_SP", params)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function GetConnectionJobIDType() As String
        Try
            Dim dt As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "ConnectionJobIDType_SP")
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                If Not IsDBNull(dt.Rows(0)(0)) AndAlso Len(dt.Rows(0)(0)) > 0 Then
                    Return dt.Rows(0)(0)
                Else
                    Return "Normal"
                End If
            Else
                Return "Normal"
            End If
        Catch ex As Exception
            Return "Normal"
        End Try
    End Function
    Public Function GetSpikeResult(ByVal QCBatchID As String, ByVal TestQCSpike As Integer, ByVal TestParameter As Integer, ByVal RunType As String, ByVal colQCType As String, ByVal Mode As String) As DataTable
        Try
            Dim params() As SqlParameter = New SqlParameter(5) {}
            params(0) = New SqlParameter("@colUserQCBatchID", QCBatchID)
            params(1) = New SqlParameter("@coluqTestParameterID", TestParameter)
            params(2) = New SqlParameter("@coluqTestQCSpike", TestQCSpike)
            params(3) = New SqlParameter("@colRunType", RunType)
            params(4) = New SqlParameter("@colQCType", "N'" & colQCType & "'")
            params(5) = New SqlParameter("@colMode", Mode)
            Dim dt As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, _
                "EBF_EntryTSSGetQCResultID_SP", params)
            Return dt
        Catch ex As Exception
            Return New DataTable
        End Try
    End Function
    Public Function OneClickRollback(ByVal dv As DataView) As Boolean
        Try
            Dim arrSampleParameterID As New ArrayList
            Dim arrSpikeResultID As New ArrayList
            Dim arrResultBlankID As New ArrayList
            Dim arrSpikeStandardResultID As New ArrayList
            Dim strTabResult As String = String.Empty
            If Not dv Is Nothing AndAlso dv.Count > 0 Then
                For Each dr As DataRowView In dv
                    If dr("All") = "True" Then
                        If dv.Table.Columns.Contains("ResultTab") Then
                            If Not IsDBNull(dr("ResultTab")) Then
                                strTabResult = dr("ResultTab")
                            End If
                        ElseIf dv.Table.Columns.Contains("TabResult") Then
                            If Not IsDBNull(dr("TabResult")) Then
                                strTabResult = dr("TabResult")
                            End If
                        End If
                        'Rollback Sample Status
                        If Len(strTabResult) > 0 AndAlso strTabResult = "Sample" Then
                            Dim params1() As SqlParameter = New SqlParameter(1) {}
                            params1(1) = New SqlParameter("@mode", "PendingComplete")
                            If dv.Table.Columns.Contains("SampleParameterID") Then
                                params1(0) = New SqlParameter("@coluqSampleParameterID", dr("SampleParameterID"))
                                If arrSampleParameterID.IndexOf(dr("SampleParameterID")) < 0 Then
                                    arrSampleParameterID.Add(dr("SampleParameterID"))
                                    SqlHelper.ExecuteNonQuery(ConnectionSettings.cnString, _
                                    CommandType.StoredProcedure, "Delete_sample_status_sp", params1)
                                End If
                            ElseIf dv.Table.Columns.Contains("coluqSampleParameterID") Then
                                params1(0) = New SqlParameter("@coluqSampleParameterID", dr("coluqSampleParameterID"))
                                If arrSampleParameterID.IndexOf(dr("coluqSampleParameterID")) < 0 Then
                                    arrSampleParameterID.Add(dr("coluqSampleParameterID"))
                                    SqlHelper.ExecuteNonQuery(ConnectionSettings.cnString, _
                                    CommandType.StoredProcedure, "Delete_sample_status_sp", params1)
                                End If
                            End If
                        End If
                        'Rollback Spike status
                        If Len(strTabResult) > 0 AndAlso strTabResult = "Spike" Then
                            Dim params2() As SqlParameter = New SqlParameter(1) {}
                            Dim dt As DataTable = Nothing
                            If dv.Table.Columns.Contains("uqQCBatchID") AndAlso dv.Table.Columns.Contains("TestQCSpike") Then
                                dt = GetSpikeResult(dr("uqQCBatchID"), dr("TestQCSpike"), 0, "Spike", "", "")
                            ElseIf dv.Table.Columns.Contains("coluqQCBatchID") AndAlso dv.Table.Columns.Contains("coluqTestQCSpike") Then
                                dt = GetSpikeResult(dr("coluqQCBatchID"), dr("coluqTestQCSpike"), 0, "Spike", "", "")
                            End If
                            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                                For Each dr1 As DataRow In dt.Rows
                                    params2(0) = New SqlParameter("@coluqSpikeResultID", dr1("coluqSpikeResultID"))
                                Next
                                params2(1) = New SqlParameter("@mode", "Complete")
                                If arrSpikeResultID.IndexOf(params2(0).Value) < 0 Then
                                    arrSpikeResultID.Add(params2(0).Value)
                                    SqlHelper.ExecuteNonQuery(ConnectionSettings.cnString, _
                                    CommandType.StoredProcedure, "Delete_QCSpike_Status_SP", params2)
                                End If
                            End If
                        End If
                        'Rollback Blank Status
                        If Len(strTabResult) > 0 AndAlso strTabResult = "Blank" Then
                            Dim params3() As SqlParameter = New SqlParameter(1) {}
                            Dim dt As DataTable = Nothing
                            If dv.Table.Columns.Contains("uqQCBatchID") AndAlso dv.Table.Columns.Contains("TestParameterID") Then
                                dt = GetSpikeResult(dr("uqQCBatchID"), 0, dr("TestParameterID"), "MethodBlank", dr("RunType"), "")
                            ElseIf dv.Table.Columns.Contains("coluqQCBatchID") AndAlso dv.Table.Columns.Contains("coluqTestParameterID") Then
                                dt = GetSpikeResult(dr("coluqQCBatchID"), 0, dr("coluqTestParameterID"), "MethodBlank", dr("RunType"), "")
                            End If
                            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                                For Each dr1 As DataRow In dt.Rows
                                    params3(0) = New SqlParameter("@coluqResultBlankID", dr1("coluqResultBlankID"))
                                Next
                                params3(1) = New SqlParameter("@mode", "Complete")
                                If arrResultBlankID.IndexOf(params3(0).Value) < 0 Then
                                    arrResultBlankID.Add(params3(0).Value)
                                    SqlHelper.ExecuteNonQuery(ConnectionSettings.cnString, _
                                    CommandType.StoredProcedure, "Delete_blank_status_sp", params3)
                                End If
                            End If
                        End If
                        'Rollback Standard status
                        If Len(strTabResult) > 0 AndAlso strTabResult = "STD" Then
                            Dim params3() As SqlParameter = New SqlParameter(1) {}
                            Dim dt As DataTable = Nothing
                            If dv.Table.Columns.Contains("uqQCBatchID") AndAlso dv.Table.Columns.Contains("TestQCstdSpikeID") Then
                                dt = GetSpikeResult(dr("uqQCBatchID"), dr("TestQCstdSpikeID"), 0, "Standard", "", "")
                            ElseIf dv.Table.Columns.Contains("coluqQCBatchID") AndAlso dv.Table.Columns.Contains("coluqTestQCStandardSpikeID") Then
                                dt = GetSpikeResult(dr("coluqQCBatchID"), dr("coluqTestQCStandardSpikeID"), 0, "Standard", "", "")
                            End If
                            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                                For Each dr1 As DataRow In dt.Rows
                                    params3(0) = New SqlParameter("@coluqSpikeStandardResultID", dr1("coluqSpikeStandardResultID"))
                                Next
                                params3(1) = New SqlParameter("@mode", "Complete")
                                If arrSpikeStandardResultID.IndexOf(params3(0).Value) < 0 Then
                                    arrSpikeStandardResultID.Add(params3(0).Value)
                                    SqlHelper.ExecuteNonQuery(ConnectionSettings.cnString, _
                                    CommandType.StoredProcedure, "Delete_Statndard_status_sp", params3)
                                End If
                            End If
                        End If
                        'Delete QcSampleResult and update DataEntered as 0
                        If Len(strTabResult) > 0 AndAlso strTabResult <> "Sample" Then
                            Dim params9() As SqlParameter = New SqlParameter(5) {}
                            If Len(strTabResult) > 0 AndAlso strTabResult = "Blank" Then
                                params9(0) = New SqlParameter("@Type", "Blank")
                                If dv.Table.Columns.Contains("TestParameterID") Then
                                    params9(1) = New SqlParameter("@QCSampleID", dr("TestParameterID"))
                                ElseIf dv.Table.Columns.Contains("coluqTestParameterID") Then
                                    params9(1) = New SqlParameter("@QCSampleID", dr("coluqTestParameterID"))
                                End If
                                If dv.Table.Columns.Contains("uqQCTypeID") Then
                                    params9(2) = New SqlParameter("@QCTypeID", dr("uqQCTypeID"))
                                ElseIf dv.Table.Columns.Contains("coluqQCTypeID") Then
                                    params9(2) = New SqlParameter("@QCTypeID", dr("coluqQCTypeID"))
                                End If
                                If dv.Table.Columns.Contains("uqQCBatchID") Then
                                    params9(3) = New SqlParameter("@uqQCBatchID", dr("uqQCBatchID"))
                                ElseIf dv.Table.Columns.Contains("coluqQCBatchID") Then
                                    params9(3) = New SqlParameter("@uqQCBatchID", dr("coluqQCBatchID"))
                                End If
                            ElseIf Len(strTabResult) > 0 AndAlso strTabResult = "Spike" Then
                                params9(0) = New SqlParameter("@Type", "Spike")
                                If dv.Table.Columns.Contains("TestQCSpike") Then
                                    params9(1) = New SqlParameter("@QCSampleID", dr("TestQCSpike"))
                                ElseIf dv.Table.Columns.Contains("coluqTestQCSpike") Then
                                    params9(1) = New SqlParameter("@QCSampleID", dr("coluqTestQCSpike"))
                                End If
                                If dv.Table.Columns.Contains("uqQCTypeID") Then
                                    params9(2) = New SqlParameter("@QCTypeID", dr("uqQCTypeID"))
                                ElseIf dv.Table.Columns.Contains("coluqQCTypeID") Then
                                    params9(2) = New SqlParameter("@QCTypeID", dr("coluqQCTypeID"))
                                End If
                                If dv.Table.Columns.Contains("uqQCBatchID") Then
                                    params9(3) = New SqlParameter("@uqQCBatchID", dr("uqQCBatchID"))
                                ElseIf dv.Table.Columns.Contains("coluqQCBatchID") Then
                                    params9(3) = New SqlParameter("@uqQCBatchID", dr("coluqQCBatchID"))
                                End If
                            ElseIf Len(strTabResult) > 0 AndAlso strTabResult = "STD" Then
                                params9(0) = New SqlParameter("@Type", "Standard")
                                If dv.Table.Columns.Contains("TestQCstdSpikeID") Then
                                    params9(1) = New SqlParameter("@QCSampleID", dr("TestQCstdSpikeID"))
                                ElseIf dv.Table.Columns.Contains("coluqTestQCStandardSpikeID") Then
                                    params9(1) = New SqlParameter("@QCSampleID", dr("coluqTestQCStandardSpikeID"))
                                End If
                                If dv.Table.Columns.Contains("uqQCTypeID") Then
                                    params9(2) = New SqlParameter("@QCTypeID", dr("uqQCTypeID"))
                                ElseIf dv.Table.Columns.Contains("coluqQCTypeID") Then
                                    params9(2) = New SqlParameter("@QCTypeID", dr("coluqQCTypeID"))
                                End If
                                If dv.Table.Columns.Contains("uqQCBatchID") Then
                                    params9(3) = New SqlParameter("@uqQCBatchID", dr("uqQCBatchID"))
                                ElseIf dv.Table.Columns.Contains("coluqQCBatchID") Then
                                    params9(3) = New SqlParameter("@uqQCBatchID", dr("coluqQCBatchID"))
                                End If
                            End If
                            params9(4) = New SqlParameter("@QCRunNo", dr("QCRunNo"))
                            params9(5) = New SqlParameter("@FormName", "ElNResultEntry")
                            SqlHelper.ExecuteNonQuery(ConnectionSettings.cnString, _
                                CommandType.StoredProcedure, "EBF_DeleteQCSampleResult_SP", params9)
                            'Dim params8() As SqlParameter = New SqlParameter(1) {}
                            'If dv.Table.Columns.Contains("uqQCBatchID") Then
                            '    params8(0) = New SqlParameter("@QCBatch", dr("uqQCBatchID"))
                            'ElseIf dv.Table.Columns.Contains("coluqQCBatchID") Then
                            '    params8(0) = New SqlParameter("@QCBatch", dr("coluqQCBatchID"))
                            'End If
                            'params8(1) = New SqlParameter("@QCType", dr("RunType"))
                            'SqlHelper.ExecuteNonQuery(ConnectionSettings.cnString, _
                            '   CommandType.StoredProcedure, "EBF_UpdateQCBatchQCType_SP", params8)
                        End If
                    End If
                Next
            End If
            Return True
        Catch ex As Exception
            DataFunctions.InsertExceptionTrackingDC(ex, "DataFunctions", Reflection.MethodInfo.GetCurrentMethod.Name)
            Return False
        End Try
    End Function
    Public Function GetUserPerCheck(ByVal ModuleName As String, ByVal FormName As String, ByVal PermInt As Integer) As DataTable
        Try
            Dim params() As SqlParameter = New SqlParameter(2) {}
            params(0) = New SqlParameter("@ModuleName", ModuleName)
            params(1) = New SqlParameter("@FormName", FormName)
            params(2) = New SqlParameter("@Permission", PermInt)
            Return SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, _
                    "SL_GetPermited_SP", params)
        Catch ex As Exception
            Return New DataTable
        End Try
    End Function
    Public Function GetServerTimes() As DateTime
        Try
            Dim d As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "Public_GetServerTime_SP")
            If d IsNot Nothing Then
                Return d.Rows(0)(0)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            DataFunctions.InsertExceptionTrackingDC(ex, "DataFunctions", Reflection.MethodInfo.GetCurrentMethod.Name)
            Return Nothing
        End Try
    End Function
    Public Function GetUsersByPermission(ByVal ModuleName As String, ByVal FormName As String, ByVal Permission As String) As DataTable
        Try
            Dim iPermission As Integer
            iPermission = 0
            Select Case Permission
                Case "None"
                    iPermission = 2
                Case "View"
                    iPermission = 4
                Case "Enter"
                    iPermission = 8
                Case "Validate"
                    iPermission = 16
                Case "Approve"
                    iPermission = 32
                Case "Edit"
                    iPermission = 64
                Case "Sign Off"
                    iPermission = 100
                Case "Delete"
                    iPermission = 128
            End Select
            Dim params() As SqlParameter = New SqlParameter(2) {}
            params(0) = New SqlParameter("@ModuleName", ModuleName)
            params(1) = New SqlParameter("@FormName", FormName)
            params(2) = New SqlParameter("@Permission", iPermission)
            Return SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, _
                    "Public_GetUsersByPermission_SP", params)
        Catch ex As Exception
            MsgBox(ex.Message)
            Return New DataTable
        End Try
    End Function
    Public Function CheckQualifier(ByVal Qualifier As String) As Integer
        Try
            Dim params() As SqlParameter = New SqlParameter(0) {}
            params(0) = New SqlParameter("@Qualifier", Qualifier)
            Return SqlHelper.ExecuteScalar(ConnectionSettings.cnString, CommandType.StoredProcedure, _
            "Public_CheckQualifier_SP", params)
        Catch ex As Exception
            Return 0
        End Try
    End Function
    Public Function GetFtpFolderPath(ByVal FormName As String) As String
        Dim params() As SqlParameter = New SqlParameter() {New SqlParameter("@colFormName", FormName)}
        Dim dtPath As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "FtpFolderPath_Select_SP", params)
        Dim strFtpFolderPath As String = String.Empty
        If Not dtPath Is Nothing AndAlso dtPath.Rows.Count > 0 Then
            Dim drPath() As DataRow = dtPath.Select("FormName = '" & FormName & "'")
            If Not drPath Is Nothing AndAlso drPath.Length > 0 Then
                strFtpFolderPath = drPath(0)("FolderPath")
            End If
        End If
        Return strFtpFolderPath
    End Function
    Public Function GetFtpConection() As DataTable
        Dim DataSource As String = "FTPConnection"
        Dim params() As SqlParameter = New SqlParameter() {New SqlParameter("@colDataSource", DataSource)}
        Dim d As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "FtpPath_select_SP", params)
        Return d
    End Function
    Public Function GetDashBoardConnection() As DataTable
        Dim DataSource As String = "DashBoardConnection"
        Dim params() As SqlParameter = New SqlParameter() {New SqlParameter("@colDataSource", DataSource)}
        Dim d As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "DataBoard_select_SP", params)
        Return d
    End Function
    Public Sub InsertDashBoardConnection(ByVal ServerName As String, ByVal DataBaseName As String, ByVal UserName As String, ByVal Password As String)
        Dim DataSource As String = "DashBoardConnection"
        Dim Aparams() As SqlParameter = New SqlParameter(4) {}
        Aparams(0) = New SqlParameter("@colDataSource", DataSource)
        Aparams(1) = New SqlParameter("@colServerName", ServerName)
        Aparams(2) = New SqlParameter("@colUserName", UserName)
        Aparams(3) = New SqlParameter("@colPassword", Password)
        Aparams(4) = New SqlParameter("@colDataBaseName", DataBaseName)
        SqlHelper.ExecuteNonQuery(ConnectionSettings.cnString, CommandType.StoredProcedure, "DashBoard_Insert_SP", Aparams)
    End Sub
    Public Sub UpdateDashBoardConnection(ByVal ServerName As String, ByVal DataBaseName As String, ByVal UserName As String, ByVal Password As String)
        Dim DataSource As String = "DashBoardConnection"
        Dim Aparams() As SqlParameter = New SqlParameter(4) {}
        Aparams(0) = New SqlParameter("@colDataSource", DataSource)
        Aparams(1) = New SqlParameter("@colServerName", ServerName)
        Aparams(2) = New SqlParameter("@colUserName", UserName)
        Aparams(3) = New SqlParameter("@colPassword", Password)
        Aparams(4) = New SqlParameter("@colDataBaseName", DataBaseName)
        SqlHelper.ExecuteNonQuery(ConnectionSettings.cnString, CommandType.StoredProcedure, "DashBoard_Update_Sp", Aparams)
    End Sub
    Public Function GetCurrentVersion() As DataTable
        Dim d As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "Version_SelectCurrentVersion_SP", Nothing)
        Return d
    End Function
    Public Sub InsertFtpConection(ByVal ServerName As String, ByVal UserName As String, ByVal Password As String, ByVal FTPPath As String)
        Dim DataSource As String = "FTPConnection"
        Dim Aparams() As SqlParameter = New SqlParameter(4) {}
        Aparams(0) = New SqlParameter("@colDataSource", DataSource)
        Aparams(1) = New SqlParameter("@colServerName", ServerName)
        Aparams(2) = New SqlParameter("@colUserName", UserName)
        Aparams(3) = New SqlParameter("@colPassword", Password)
        Aparams(4) = New SqlParameter("@colFTPPath", FTPPath)
        SqlHelper.ExecuteNonQuery(ConnectionSettings.cnString, CommandType.StoredProcedure, "FtpPath_Insert_SP", Aparams)
    End Sub
    Public Sub UpdateFtpConection(ByVal ServerName As String, ByVal UserName As String, ByVal Password As String, ByVal FTPPath As String)
        Dim DataSource As String = "FTPConnection"
        Dim Aparams() As SqlParameter = New SqlParameter(4) {}
        Aparams(0) = New SqlParameter("@colDataSource", DataSource)
        Aparams(1) = New SqlParameter("@colServerName", ServerName)
        Aparams(2) = New SqlParameter("@colUserName", UserName)
        Aparams(3) = New SqlParameter("@colPassword", Password)
        Aparams(4) = New SqlParameter("@colFTPPath", FTPPath)
        SqlHelper.ExecuteNonQuery(ConnectionSettings.cnString, CommandType.StoredProcedure, "FtpPath_Update_SP", Aparams)
    End Sub
    Public Sub UpdateJobIDSampleType(ByVal SampleType As String)
        Dim Aparams() As SqlParameter = New SqlParameter(0) {}
        Aparams(0) = New SqlParameter("@colSampleType", SampleType)
        SqlHelper.ExecuteNonQuery(ConnectionSettings.cnString, CommandType.StoredProcedure, "JobIDSampleType_Update_SP", Aparams)
    End Sub
    Public Sub UpdateLanguage(ByVal Language As String)
        Dim Aparams() As SqlParameter = New SqlParameter(0) {}
        Aparams(0) = New SqlParameter("@colLanguage", Language)
        SqlHelper.ExecuteNonQuery(ConnectionSettings.cnString, CommandType.StoredProcedure, "Language_Update_SP", Aparams)
    End Sub
    Public Sub UpdateDS(ByVal mode As String, ByVal val As Integer)
        Try
            Dim params() As SqlParameter = New SqlParameter(1) {}
            params(0) = New SqlParameter("@Mode", mode)
            params(1) = New SqlParameter("@Keyword", val)
            SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, _
           "ebenchDefaultSettings_SP ", params)
        Catch ex As Exception
        End Try
    End Sub
    Public Function GetQueryDataDF(ByVal qCode As String) As DataTable
        Dim params() As SqlParameter = New SqlParameter() {New SqlParameter("@keycode", qCode)}
        Dim d As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "LIMS_QuerySelect_SP", params)
        Return d
    End Function
    Public Function Public_CheckUserFullAccess(ByVal UserName As String) As Integer
        Try
            Dim params() As SqlParameter = New SqlParameter() {New SqlParameter("@colUserName", UserName)}
            Dim d As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "Public_CheckUserFullAccess_SP", params)
            If Not d Is Nothing AndAlso d.Rows.Count > 0 AndAlso Not IsDBNull(d.Rows(0)("Permission")) AndAlso Len(d.Rows(0)("Permission")) > 0 Then
                Return d.Rows(0)("Permission")
            Else
                Return 0
            End If
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Public Function GetSVQueryDataDF(ByVal qCode As String) As String
        Dim params() As SqlParameter = New SqlParameter() {New SqlParameter("@keycode", qCode)}
        Return SqlHelper.ExecuteScalar(ConnectionSettings.cnString, CommandType.StoredProcedure, "LIMS_QuerySelect_SP", params)
    End Function

    Public Function SqlBeginTransaction() As SqlTransaction
        con = New SqlConnection(ConnectionSettings.cnString)
        con.Open()
        Trans = con.BeginTransaction(IsolationLevel.ReadUncommitted)
        Return Trans
    End Function
    Public Function SqlBeginTransactionReadCommitted() As SqlTransaction
        con = New SqlConnection(ConnectionSettings.cnString)
        con.Open()
        Trans = con.BeginTransaction(IsolationLevel.ReadCommitted)
        Return Trans
    End Function

    Public Sub RollBack()
        Trans.Rollback()
        con.Close()
    End Sub

    Public Sub SQLCloseTransaction()
        Trans.Commit()
        con.Close()
    End Sub
    Public Function getDate(ByVal coluqSampleTestID As String) As DataSet
        Dim params() As SqlParameter = New SqlParameter(0) {}
        params(0) = New SqlParameter("@coluqSampleTEstID", coluqSampleTestID)

        Return SqlHelper.ExecuteDataset(ConnectionSettings.cnString, CommandType.StoredProcedure, _
                       "Public_GetPreviousFunctionDate_SP", params)

    End Function
    Public Function GetHoursbyTAT(ByVal TAT As String, ByVal choice As String) As DataTable
        Dim params() As SqlParameter = New SqlParameter(1) {}
        params(0) = New SqlParameter("@colChoice", choice)
        params(1) = New SqlParameter("@colTAT", TAT)
        Dim d As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "SL_GetHoursbyTAT_SP", params)
        Return d
    End Function
    Public Function GetDefaultCompuntListName() As String
        Try
            Dim dt As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "GetDefaultCompoundListName_SP")
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                If Not IsDBNull(dt.Rows(0)(0)) AndAlso Len(dt.Rows(0)(0)) > 0 Then
                    Return dt.Rows(0)(0)
                Else
                    Return String.Empty
                End If
            Else
                Return String.Empty
            End If
        Catch ex As Exception
            MsgBox("Error retrieving Default Comp List Name")
            Return String.Empty
        End Try
    End Function
    Public Function GetDefaultRPDSource() As String
        Try
            Dim dt As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "GetDefaultRPDSource_SP")
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                If Not IsDBNull(dt.Rows(0)(0)) AndAlso Len(dt.Rows(0)(0)) > 0 Then
                    Return dt.Rows(0)(0)
                Else
                    Return String.Empty
                End If
            Else
                Return String.Empty
            End If
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function
    Public Function GetDefaultConstantByVal(ByVal Constant As String) As String
        Try
            Dim params() As SqlParameter = New SqlParameter(0) {}
            params(0) = New SqlParameter("@Constant", Constant)
            Dim dt As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, _
            CommandType.StoredProcedure, "GetDefaultConstantByVal_SP", params)
            If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                If Not IsDBNull(dt.Rows(0)(0)) AndAlso Len(dt.Rows(0)(0)) > 0 Then
                    Return dt.Rows(0)(0)
                Else
                    Return String.Empty
                End If
            Else
                Return String.Empty
            End If
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    Public Function DueDateCalculation(ByVal TAT As String, ByVal ReceivedDate As DateTime, ByVal TATChoice As String) As String
        Try
            If Not IsDBNull(ReceivedDate) AndAlso Len(ReceivedDate) > 0 Then
                If Len(TAT) > 0 Then
                    Dim DueDate As String = String.Empty
                    Dim tempdt As DateTime
                    Dim phtt, pmin, dhrs As Integer
                    Dim startHr, EndHr As Integer
                    startHr = 8
                    EndHr = 17
                    Dim flgTATinTime As Boolean = True
                    Dim dtH As DataTable = GetHoursbyTAT(TAT, TATChoice)
                    If Not dtH Is Nothing AndAlso dtH.Rows.Count > 0 Then
                        If Not IsDBNull(dtH.Rows(0)(0)) AndAlso Len(dtH.Rows(0)(0)) > 0 Then
                            phtt = dtH.Rows(0)(0)
                        Else
                            phtt = 0
                        End If
                    Else
                        phtt = 0
                    End If
                    tempdt = ReceivedDate
                    If tempdt.DayOfWeek = DayOfWeek.Saturday Or tempdt.DayOfWeek = DayOfWeek.Sunday Then
                        If tempdt.DayOfWeek = DayOfWeek.Saturday Then
                            'tempdt = tempdt.AddDays(2)
                            CheckSaturdaySundayWorking(tempdt)
                            CheckHoliday(tempdt)
                        ElseIf tempdt.DayOfWeek = DayOfWeek.Sunday Then
                            'tempdt = tempdt.AddDays(1)
                            CheckSundayWorking(tempdt)
                            CheckHoliday(tempdt)
                        End If
                        pmin = 0
                    Else
                        pmin = tempdt.Minute
                    End If
                    tempdt = New DateTime(tempdt.Year, tempdt.Month, tempdt.Day, tempdt.Hour, tempdt.Minute, 0)
                    If tempdt.Hour < 8 Then
                        tempdt = New DateTime(tempdt.Year, tempdt.Month, tempdt.Day, 8, 0, 0)
                    ElseIf tempdt.Hour > 17 Then
                        tempdt = tempdt.AddDays(1)
                        tempdt = New DateTime(tempdt.Year, tempdt.Month, tempdt.Day, 8, 0, 0)
                    End If
                    dhrs = 24
                    Do While phtt > 0
                        CheckHoliday(tempdt)
                        If tempdt.DayOfWeek = DayOfWeek.Saturday Then
                            'tempdt = tempdt.AddDays(2)
                            CheckSaturdaySundayWorking(tempdt)
                            CheckHoliday(tempdt)
                        ElseIf tempdt.DayOfWeek = DayOfWeek.Sunday Then
                            'tempdt = tempdt.AddDays(1)
                            CheckSundayWorking(tempdt)
                            CheckHoliday(tempdt)
                        End If
                        If dhrs < phtt Then
                            tempdt = tempdt.AddDays(1)
                            phtt = phtt - dhrs
                            flgTATinTime = False
                        ElseIf dhrs = phtt Then
                            tempdt = tempdt.AddDays(1)
                            CheckHoliday(tempdt)
                            If tempdt.DayOfWeek = DayOfWeek.Saturday Then
                                'tempdt = tempdt.AddDays(2)
                                CheckLastSaturdaySundayWorking(tempdt)
                                CheckHoliday(tempdt)
                            ElseIf tempdt.DayOfWeek = DayOfWeek.Sunday Then
                                'tempdt = tempdt.AddDays(1)
                                CheckSundayWorking(tempdt)
                                CheckHoliday(tempdt)
                            End If
                            DueDate = Format(tempdt, "MM/dd/yyyy HH:mm")
                            Return DueDate
                        Else
                            If flgTATinTime Then
                                tempdt = tempdt.AddHours(phtt)
                            End If
                            DueDate = Format(tempdt, "MM/dd/yyyy HH:mm")
                            Return DueDate
                        End If
                    Loop
                Else
                    Return String.Empty
                End If
            Else
                Return String.Empty
            End If
            Return String.Empty
        Catch ex As Exception
            Return String.Empty
        End Try
    End Function
    Public Sub CheckLastSaturdaySundayWorking(ByRef tempdt As DateTime)
        Try
            Dim dtHoliday As DataTable = GetQueryDataDF("WEEKENDWORKING_DATE")
            Dim drHoliday As DataRow
            Dim tempdt1 As DateTime
            tempdt1 = tempdt
            Dim bolExclude As Boolean = False
            For Each drHoliday In dtHoliday.Rows
                Dim hdate As DateTime
                hdate = drHoliday(0)
                bolExclude = False
                If tempdt1.DayOfWeek = DayOfWeek.Saturday Then
                    If tempdt1.Date.Day = hdate.Date.Day And tempdt1.Date.Month = hdate.Date.Month Then
                        bolExclude = True
                        Exit For
                    End If
                End If
            Next
            If bolExclude = False Then
                tempdt = tempdt.AddDays(1)
                tempdt1 = tempdt1.AddDays(1)
                For Each drHoliday In dtHoliday.Rows
                    Dim hdate As DateTime
                    hdate = drHoliday(0)
                    bolExclude = False
                    If tempdt1.DayOfWeek = DayOfWeek.Sunday Then
                        If tempdt1.Date.Day = hdate.Date.Day And tempdt1.Date.Month = hdate.Date.Month Then
                            bolExclude = True
                            Exit For
                        End If
                    End If
                Next
                If bolExclude = False Then
                    tempdt = tempdt.AddDays(1)
                End If
            End If
        Catch ex As Exception
            InsertExceptionTrackingDC(ex, "Apllication Global", Reflection.MethodInfo.GetCurrentMethod.Name)
        End Try
    End Sub
    Public Sub CheckSaturdaySundayWorking(ByRef tempdt As DateTime)
        Try
            Dim dtHoliday As DataTable = GetQueryDataDF("WEEKENDWORKING_DATE")
            Dim drHoliday As DataRow
            Dim tempdt1 As DateTime
            tempdt1 = tempdt
            Dim bolExclude As Boolean = False
            For Each drHoliday In dtHoliday.Rows
                Dim hdate As DateTime
                hdate = drHoliday(0)
                bolExclude = False
                If tempdt1.DayOfWeek = DayOfWeek.Saturday Then
                    If tempdt1.Date.Day = hdate.Date.Day And tempdt1.Date.Month = hdate.Date.Month Then
                        bolExclude = True
                        Exit For
                    End If
                End If
            Next
            If bolExclude = False Then
                tempdt = tempdt.AddDays(1)
                tempdt1 = tempdt1.AddDays(1)
            Else
                tempdt1 = tempdt1.AddDays(1)
            End If
            For Each drHoliday In dtHoliday.Rows
                Dim hdate As DateTime
                hdate = drHoliday(0)
                bolExclude = False
                If tempdt1.DayOfWeek = DayOfWeek.Sunday Then
                    If tempdt1.Date.Day = hdate.Date.Day And tempdt1.Date.Month = hdate.Date.Month Then
                        bolExclude = True
                        Exit For
                    End If
                End If
            Next
            If bolExclude = False Then
                tempdt = tempdt.AddDays(1)
            End If
        Catch ex As Exception
            InsertExceptionTrackingDC(ex, "Apllication Global", Reflection.MethodInfo.GetCurrentMethod.Name)
        End Try
    End Sub
    Public Sub CheckSundayWorking(ByRef tempdt As DateTime)
        Try
            Dim dtHoliday As DataTable = GetQueryDataDF("WEEKENDWORKING_DATE")
            Dim drHoliday As DataRow
            Dim tempdt1 As DateTime
            tempdt1 = tempdt
            Dim bolExclude As Boolean = False
            For Each drHoliday In dtHoliday.Rows
                Dim hdate As DateTime
                hdate = drHoliday(0)
                bolExclude = False
                If tempdt1.DayOfWeek = DayOfWeek.Sunday Then
                    If tempdt1.Date.Day = hdate.Date.Day And tempdt1.Date.Month = hdate.Date.Month Then
                        bolExclude = True
                        Exit For
                    End If
                End If
            Next
            If bolExclude = False Then
                tempdt = tempdt.AddDays(1)
            End If
        Catch ex As Exception
            InsertExceptionTrackingDC(ex, "Apllication Global", Reflection.MethodInfo.GetCurrentMethod.Name)
        End Try
    End Sub
    Public Sub CheckHoliday(ByRef tempdt As DateTime)
        Try
            Dim dtHoliday As DataTable = GetQueryDataDF("HOLIDAY_DATE")
            Dim drHoliday As DataRow
            For Each drHoliday In dtHoliday.Rows
                Dim hdate As DateTime
                hdate = drHoliday(0)
                If Not hdate.DayOfWeek = DayOfWeek.Saturday Or Not hdate.DayOfWeek = DayOfWeek.Sunday Then
                    If tempdt.Date.Day = hdate.Date.Day And tempdt.Date.Month = hdate.Date.Month Then
                        tempdt = tempdt.AddDays(1)
                        tempdt = New DateTime(tempdt.Year, tempdt.Month, tempdt.Day, tempdt.Hour, tempdt.Minute, 0)
                        If tempdt.DayOfWeek = DayOfWeek.Saturday Then
                            tempdt = tempdt.AddDays(2)
                            tempdt = New DateTime(tempdt.Year, tempdt.Month, tempdt.Day, tempdt.Hour, tempdt.Minute, 0)
                        ElseIf tempdt.DayOfWeek = DayOfWeek.Sunday Then
                            tempdt = tempdt.AddDays(1)
                            tempdt = New DateTime(tempdt.Year, tempdt.Month, tempdt.Day, tempdt.Hour, tempdt.Minute, 0)
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
        End Try
    End Sub
    Public Function InsertWebServiceURL(ByVal URL As String) As Boolean
        Dim params() As SqlParameter = New SqlParameter(0) {}
        params(0) = New SqlParameter("@colWebServiceURL", URL)
        If SqlHelper.ExecuteNonQuery(ConnectionSettings.cnString, CommandType.StoredProcedure, "Public_InsertWebserviceURL_sp", params) < 0 Then
            Return False
        End If
        Return True
    End Function

    Public Function GetSQLConnection() As String
        Return ConnectionSettings.cnStringReport
    End Function
    Public Function GetSQLConnectionReportUPdate() As String
        Return ConnectionSettings.cnStringReportInstances
    End Function
    Public Function GetSQLConnectionWithoutPasswordReportUpdate() As String
        Return ConnectionSettings.cnStringReportWithOutPasswordInstances
    End Function
    Public Function GetSQLConnectionWithOutPassword() As String
        Return ConnectionSettings.cnStringWithOutPassword
    End Function
    Public Function EBF_GetReportingMatrix(ByVal Matrix As String) As DataTable
        Try
            Dim params() As SqlParameter = New SqlParameter(0) {}
            params(0) = New SqlParameter("@Matrix", Matrix)
            Return SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "EBF_GetReportingMatrix_SP", params)
        Catch ex As Exception
            Return New DataTable
        End Try
    End Function
    Public Function GetControlCaptionByFormName(ByVal FormName As String) As DataSet
        Dim params() As SqlParameter = New SqlParameter() {New SqlParameter("@FormName", FormName)}
        Return SqlHelper.ExecuteDataset(ConnectionSettings.cnString, CommandType.StoredProcedure, "Public_GetControlCaptionByFormName_SP", params)
    End Function
    Public Function GetFtpFormFolderPath(ByVal FormName As String) As String
        Dim params() As SqlParameter = New SqlParameter() {New SqlParameter("@colFormName", FormName)}
        Dim dtPath As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "FtpFolderPath_Select_SP", params)
        Dim strFtpFolderPath As String = String.Empty
        If Not dtPath Is Nothing AndAlso dtPath.Rows.Count > 0 Then
            Dim drPath() As DataRow = dtPath.Select("FormName = '" & FormName & "'")
            If Not drPath Is Nothing AndAlso drPath.Length > 0 Then
                strFtpFolderPath = drPath(0)("FolderPath")
            End If
        End If
        Return strFtpFolderPath
    End Function
    Public Function Public_UserSign(ByVal coluqSampleParameterID As String) As DataSet
        Try
            Dim params() As SqlParameter = New SqlParameter(0) {}
            params(0) = New SqlParameter("@coluqSampleParameterID", coluqSampleParameterID)
            Return SqlHelper.ExecuteDataset(ConnectionSettings.cnString, CommandType.StoredProcedure, "PUBLIC_USERSIGN_SP", params)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function SelectSourceSampleTestResult(ByVal coluqTestParameterID As Integer, ByVal coluqSampleParameterID As Integer, ByVal coluqQCBatchID As Integer, ByVal coluqQCTypeID As Integer) As DataTable
        Dim params1() As SqlParameter = New SqlParameter(3) {}
        params1(0) = New SqlParameter("@coluqTestParameterID", coluqTestParameterID)
        params1(1) = New SqlParameter("@coluqSampleParameterID", coluqSampleParameterID)
        params1(2) = New SqlParameter("@coluqQCBatchID", coluqQCBatchID)
        params1(3) = New SqlParameter("@coluqQCTypeID", coluqQCTypeID)
        Return SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, _
                                       "EBF_SelectSourceSampleTestResult_SP", params1)
    End Function
    Public Function SelectSourceTemplateColumnResult(ByVal coluqTestParameterID As Integer, ByVal coluqSampleParameterID As Integer, ByVal coluqQCBatchID As Integer, ByVal coluqQCTypeID As Integer, ByVal SourceTemplateFormulaColumnID As Integer, ByVal strQCSourceSampleID As String, ByVal SampleRunNo As Integer, ByVal QcRunNo As Integer) As DataTable
        Dim params() As SqlParameter = New SqlParameter(7) {}
        params(0) = New SqlParameter("@coluqTestParameterID", coluqTestParameterID)
        params(1) = New SqlParameter("@coluqSampleParameterID", coluqSampleParameterID)
        params(2) = New SqlParameter("@coluqQCBatchID", coluqQCBatchID)
        params(3) = New SqlParameter("@coluqQCTypeID", coluqQCTypeID)
        params(4) = New SqlParameter("@SourceTemplateFormulaColumnID", SourceTemplateFormulaColumnID)
        params(5) = New SqlParameter("@QCSourceSampleID", strQCSourceSampleID)
        params(6) = New SqlParameter("@SampleRunNo", SampleRunNo)
        params(7) = New SqlParameter("@QcRunNo", QcRunNo)
        Return SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, _
                                       "EBF_SelectSourceTemplatecolumnResult_SP", params)
    End Function

    Public Function SelectSignature(ByVal UserID As String) As DataTable
        Try
            Dim params() As SqlParameter = New SqlParameter(0) {}
            params(0) = New SqlParameter("@colUserID", UserID)
            Dim dt As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "Env_SelectSignature_SP", params)
            Return dt
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function ExecuteQueryCode(ByVal strQuery As String) As DataTable
        Try
            Return SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.Text, strQuery)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function InsertLimsAuditTrailBulkpost(ByVal dt As DataTable) As Boolean
        Try

            Dim sqlC As SqlBulkCopy = New SqlBulkCopy(ConnectionSettings.cnString, SqlBulkCopyOptions.Default)
            sqlC.DestinationTableName = "tbl_LIMSAuditTrail"
            sqlC.BatchSize = 2
            sqlC.WriteToServer(dt)
            Return True
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function InsertQCBulkpost(ByVal qcDt As DataTable) As Boolean
        Try
            Dim sqlc As SqlBulkCopy = New SqlBulkCopy(ConnectionSettings.cnString, SqlBulkCopyOptions.Default)
            sqlc.DestinationTableName = "tbl_ENV_QCBatchAttachSample"
            sqlc.BatchSize = 50
            sqlc.WriteToServer(qcDt)
            'Dim sqlcopy As SqlBulkCopy = New SqlBulkCopy(ConnectionSettings.cnString, SqlBulkCopyOptions.Default)
            'sqlcopy.DestinationTableName = "tbl_env_QcbatchAttachSampleRun"
            'sqlcopy.BatchSize = 20
            'sqlcopy.WriteToServer(dtqcrun)
            'Return True

        Catch ex As Exception
            Return Nothing
        End Try

    End Function
    Public Function InsertELNBulkpost(ByVal dtELN As DataTable, ByVal DestinationTableName As String) As Boolean
        Try
            Dim sqlcopy As SqlBulkCopy = New SqlBulkCopy(ConnectionSettings.cnString, SqlBulkCopyOptions.Default)
            sqlcopy.DestinationTableName = DestinationTableName
            sqlcopy.BatchSize = 50
            sqlcopy.WriteToServer(dtELN)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function GetunCollectedSampleInformation(ByVal strJobID As String) As DataTable
        Dim params() As SqlParameter = New SqlParameter(0) {}
        If Not strJobID Is Nothing AndAlso Len(strJobID) > 0 Then
            strJobID = Replace(strJobID, ", N", ",")
        End If
        params(0) = New SqlParameter("@strJobID", strJobID)

        Return SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, _
                       "Reporting_GetunCollectedSamples_Select_SP", params)
    End Function
End Module

Public Module GeneralDataFunction
    Public Function GetUnits() As DataTable
        Return GetQueryDataDF("UNITS")
    End Function
    Public Function GetGetParameters() As DataTable
        Return GetQueryDataDF("PARAMETER_NAME")
    End Function
    Public Function GetTAT() As DataTable
        Return GetQueryDataDF("TAT")
    End Function
    Public Function GetUser() As DataTable
        Return GetQueryDataDF("User")
    End Function
    Public Function GetMatrix() As DataTable
        Return GetQueryDataDF("ANALYTICAL_MATRIX")
    End Function
    Public Function GetLIMS() As DataTable
        Return GetQueryDataDF("LIMS")
    End Function
End Module

Public Class BTDataTableConverter
    Dim strFieldToConvert As String, dt As DataTable, strFilter As String
    Public Sub New()
    End Sub
    Public Sub New(ByVal dt As DataTable, ByVal strFieldToConvert As String)
        Me.dt = dt
        Me.strFieldToConvert = strFieldToConvert
        Me.strFilter = strFilter
    End Sub
    Public Enum ReturnType
        [Number] = 0
        [String] = 1
    End Enum
    Public Function SplitToNVarcharMax(ByVal Value As String, Optional ByVal Spliter As String = "|") As String
        Try
            Dim intSPIndex As Integer = -1
            'Do Until strSampleParamter.Length > 15000
            '    strSampleParamter = strSampleParamter & "," & strSampleParamter
            'Loop
            Dim intSPCount As Integer = 0
            For intLoop As Integer = 1 To Value.Length Step 3800
                intSPCount = intSPCount + 1
                If Value.Length >= intSPCount * 3800 Then
                    intSPIndex = Value.IndexOf(",", intSPCount * 3800)
                    If intSPIndex > -1 Then
                        Value = Value.Remove(intSPIndex, 1).Insert(intSPIndex, Spliter)
                    End If
                End If
            Next
            Return Value
        Catch ex As Exception
            ' MsgBox(ex.Message, MsgBoxStyle.Critical, BT_AppName)
            Return Value
        End Try
    End Function
    Public Function DataRowToString(ByVal dr As DataRow) As String
        Return dr(strFieldToConvert).ToString()
    End Function
    Public Function DataTableToArray(ByVal dt As DataTable, ByVal strFieldToConvert As String, Optional ByVal strFilter As String = "") As String()
        If Not dt Is Nothing AndAlso dt.Columns.Contains(strFieldToConvert) = True Then
            Me.strFieldToConvert = strFieldToConvert
        End If
        Dim drList As DataRow() = dt.Select(strFilter)
        Dim strArr As String() = Array.ConvertAll(drList, New Converter(Of DataRow, String)(AddressOf DataRowToString))
        Return strArr
    End Function
    Public Function DataTableToArrayList(ByVal dt As DataTable, ByVal strFieldToConvert As String, Optional ByVal strFilter As String = "") As ArrayList
        If Not dt Is Nothing AndAlso dt.Columns.Contains(strFieldToConvert) = True Then
            Me.strFieldToConvert = strFieldToConvert
        End If
        Dim drList As DataRow() = dt.Select(strFilter)
        Dim strArr As String() = Array.ConvertAll(drList, New Converter(Of DataRow, String)(AddressOf DataRowToString))
        Dim strArrList As New ArrayList(strArr)
        Return strArrList
    End Function
    'Public Function DataTableToString(ByVal dt As DataTable, ByVal strFieldToConvert As String, Optional ByVal strFilter As String = "") As String
    '    If Not dt Is Nothing AndAlso dt.Columns.Contains(strFieldToConvert) = True Then
    '        Me.strFieldToConvert = strFieldToConvert
    '    End If
    '    Dim drList As DataRow() = dt.Select(strFilter)
    '    Dim strArr As String() = Array.ConvertAll(drList, New Converter(Of DataRow, String)(AddressOf DataRowToString))
    '    Return String.Join(",", strArr)
    'End Function
    Public Function DataTableToString(ByVal dt As DataTable, ByVal strFieldToConvert As String, Optional ByVal strFilter As String = "", Optional ByVal ReturnType As ReturnType = ReturnType.Number) As String
        Try
            Dim drList As DataRow() = Nothing
            If Not dt Is Nothing AndAlso dt.Columns.Contains(strFieldToConvert) = True Then
                Me.strFieldToConvert = strFieldToConvert
                Dim dvFilteredData As DataView = New DataView(dt, strFilter, "", DataViewRowState.CurrentRows)
                Dim dtDistinctField As DataTable = Nothing
                dtDistinctField = dvFilteredData.ToTable(True, strFieldToConvert)
                drList = dtDistinctField.Select()
            Else
                drList = dt.Select(strFilter)
            End If
            'Dim drList As DataRow() = dt.Select(strFilter)
            Dim strArr As String() = Array.ConvertAll(drList, New Converter(Of DataRow, String)(AddressOf DataRowToString))
            If ReturnType = BTDataTableConverter.ReturnType.Number Then
                Return String.Join(",", strArr)
            Else
                Dim strTemp As String = String.Join("','", strArr)
                If strTemp.Length > 0 Then
                    strTemp = "'" & strTemp & "'"
                End If
                Return strTemp
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Public Function DataTableToFilteredDataTable(ByVal dt As DataTable, ByVal strFieldToConvert As String, Optional ByVal strFilter As String = "") As DataTable
        If Not dt Is Nothing AndAlso dt.Columns.Contains(strFieldToConvert) = True Then
            Me.strFieldToConvert = strFieldToConvert
        End If
        Dim dvList As DataView = New DataView(dt, strFilter, "", DataViewRowState.CurrentRows)
        Return dvList.ToTable(True, strFieldToConvert)
    End Function
    Public Function ArrayListToString(ByVal pArrayList As ArrayList) As String
        Return String.Join(",", pArrayList.ToArray())
    End Function
    Public Function DataRowArrayToString(ByVal drr As DataRow(), ByVal strFieldToConvert As String, Optional ByVal strFilter As String = "") As String
        If Not IsDBNull(drr) AndAlso drr.Length > 0 Then
            Me.strFieldToConvert = strFieldToConvert
        End If
        'Dim dt As DataTable = drr.CopyToDataTable
        Dim drList As DataRow() = dt.Select(strFilter)
        Dim strArr As String() = Array.ConvertAll(drList, New Converter(Of DataRow, String)(AddressOf DataRowToString))
        Return String.Join(",", strArr)
    End Function
    Public Function DataRowArrayToStringWithoutDuplicate(ByVal drr As DataRow(), ByVal strFieldToConvert As String, Optional ByVal strFilter As String = "") As String
        If Not IsDBNull(drr) AndAlso drr.Length > 0 Then
            Me.strFieldToConvert = strFieldToConvert
        End If
        'Dim dt As DataTable = drr.CopyToDataTable
        Dim dtUniqe As DataTable = dt.DefaultView.ToTable(True, strFieldToConvert)
        Dim drList As DataRow() = dtUniqe.Select(strFilter)
        Dim strArr As String() = Array.ConvertAll(drList, New Converter(Of DataRow, String)(AddressOf DataRowToString))
        Return String.Join(",", strArr)
    End Function
    Public Function GenericListToDataTable(Of T)(list As List(Of T)) As DataTable
        Dim entityType As Type = GetType(T)
        Dim table As New DataTable()
        Dim properties As PropertyDescriptorCollection = TypeDescriptor.GetProperties(entityType)
        For Each prop As PropertyDescriptor In properties
            table.Columns.Add(prop.Name, If(Nullable.GetUnderlyingType(prop.PropertyType), prop.PropertyType))
        Next
        For Each item As T In list
            Dim row As DataRow = table.NewRow()
            For Each prop As PropertyDescriptor In properties
                row(prop.Name) = If(prop.GetValue(item), DBNull.Value)
            Next
            table.Rows.Add(row)
        Next
        Return table
    End Function
    Public Function GetQcComboReportTRRp_DataSet(ByVal SPName As String, ByVal SampleID As String, ByVal SampleParameterID As String, ByVal TestMethodID As String, ByVal ParameterID As String) As DataSet
        Try
            'SampleID = RemoveDeliemeters(SampleID, "'")
            SampleParameterID = RemoveDeliemeters(SampleParameterID, "'")
            Dim parts() As String = SampleParameterID.Split(","c)


            Dim formattedString As String = ""
            If parts.Length >= 2 Then
                For Each part As String In parts
                    'Trim any extra spaces
                    Dim trimmedPart As String = part.Trim()

                    ' Add single quotes around each part and append to formattedString
                    If formattedString.Length > 0 Then
                        formattedString += "'" + trimmedPart + "',"
                        'ElseIf formattedString.Length > 1 Then
                        '    formattedString = "'" + trimmedPart + ","
                    Else
                        formattedString = trimmedPart + "',"
                    End If

                    'Dim trimmedPart As String = part.Trim()

                    '' Add single quotes around each part and append to formattedString
                    'If formattedString.Length > 0 Then
                    '    formattedString += "'" + trimmedPart + "',"
                    '    'ElseIf formattedString.Length > 1 Then
                    '    '    formattedString = "'" + trimmedPart + ","
                    'Else
                    '    formattedString = "'" + trimmedPart + "',"
                    'End If
                Next
            Else
                formattedString = SampleParameterID
            End If
            If formattedString.Length > 0 AndAlso parts.Length >= 2 Then
                formattedString = formattedString.Substring(0, formattedString.Length - 2)
            End If
            ' Remove the trailing comma

            'TestMethodID = RemoveDeliemeters(TestMethodID, "'")
            'ParameterID = RemoveDeliemeters(ParameterID, "'")

            'SampleID = "'" + SampleID + "'"
            'SampleParameterID = "'" + SampleParameterID + "'"
            'TestMethodID = "'" + TestMethodID + "'"
            'ParameterID = "'" + ParameterID + "'"
            Dim params() As SqlParameter = New SqlParameter(1) {}
            params(0) = New SqlParameter("@SampleTestID", formattedString)
            params(1) = New SqlParameter("@BenchForm", 0)
            'params(2) = New SqlParameter("@uqTestMethodID", TestMethodID)
            'params(3) = New SqlParameter("@uqParameterID", ParameterID)
            Dim ds As DataSet = SqlHelper.ExecuteDataset(ConnectionSettings.cnString, CommandType.StoredProcedure, "QCEnvCombo_RPT", params)
            Dim dtLCS As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "QCEnvComboLCS_RPT", params)
            Dim dtLCSD As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "QCEnvComboLCSD_RPT", params)
            Dim dtMS As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "QCEnvComboMS_RPT", params)
            Dim dtMSD As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "QCEnvComboMSD_RPT", params)
            Dim dtLoginParam As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "QCEnvComboLCSLoginParam_RPT", params)
            Dim drLoginParam As DataRow

            If Not ds.Tables(2) Is Nothing Then
                If ds.Tables(2).Rows.Count > 0 Then
                    Dim dr2 As DataRow
                    For Each dr2 In ds.Tables(2).Rows
                        If IsDBNull(dr2("QCSampleResult")) Then
                            Dim params1() As SqlParameter = New SqlParameter(1) {}
                            params1(0) = New SqlParameter("@SampleID", dr2("QcSampleID"))
                            params1(1) = New SqlParameter("@Parameter", dr2("Parameter"))
                            Dim dt1 As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure,
                            "QCC_SelectDupQCSampleResult_SP", params1)
                            If Not dt1 Is Nothing Then
                                If dt1.Rows.Count > 0 Then
                                    dr2("QCSampleResult") = dt1.Rows(0)(0)
                                End If
                            End If
                        End If
                    Next
                End If
            End If
            If Not dtLCS Is Nothing Then
                If dtLCS.Rows.Count > 0 Then
                    Dim dr3 As DataRow
                    For Each dr3 In dtLCS.Rows

                        Dim dv1 As New DataView(dtLCSD, "coluqQCBatchID = '" & dr3("coluqQCBatchID") & "' and  Parameter = '" & ReplaceEscapeChars(dr3("Parameter")) & "' and  Method = '" & dr3("Method") & "'", "", DataViewRowState.CurrentRows)
                        If Not dv1 Is Nothing Then
                            If dv1.Count > 0 Then
                                dr3("LCSDResult") = dv1.Item(0)("LCSResult")
                                dr3("LCSDRec") = dv1.Item(0)("LCSRec")
                                dr3("RPD") = dv1.Item(0)("RPD")
                                dr3("RPDCLimits") = dv1.Item(0)("RPDCLimits")
                                If SPName = "QCEnvCombo_RPT" Or SPName = "QC_Louisiana" Or SPName = "Env_QCPotraitRegular_RPT_SP" Then
                                    dr3("LCSDSpikeAdded") = dv1.Item(0)("SpikeAmount")
                                End If

                                If Not IsDBNull(dr3("Qualifier")) AndAlso Len(dr3("Qualifier")) > 0 Then
                                    Dim s1() As String = Split(dr3("Qualifier"), ",")
                                    Dim s As String
                                    If Not IsDBNull(dv1.Item(0)("Qualifier")) AndAlso Len(dv1.Item(0)("Qualifier")) > 0 Then
                                        Dim s2() As String = Split(dv1.Item(0)("Qualifier"), ",")
                                        Dim ss As String
                                        Dim fla As Boolean
                                        For Each ss In s2
                                            fla = False
                                            For Each s In s1
                                                If s = ss Then
                                                    fla = True
                                                End If
                                            Next
                                            If fla = False Then
                                                dr3("Qualifier") = dr3("Qualifier") & "," & ss
                                            End If
                                        Next
                                    End If
                                Else
                                    dr3("Qualifier") = dv1.Item(0)("Qualifier")
                                End If
                            End If

                            For Each drLoginParam In dtLoginParam.Rows
                                If Not IsDBNull(dr3("Parameter")) AndAlso Not IsDBNull(drLoginParam("Parameter")) AndAlso dr3("Parameter") = drLoginParam("Parameter") Then
                                    dr3("colLoginAndReqularParam") = 1
                                End If
                            Next
                        End If
                    Next
                End If
            End If

            Dim dr3LR As DataRow
            For Each dr3LR In dtLCS.Rows
                If Not IsDBNull(dr3LR("colLoginAndReqularParam")) Then
                    If dr3LR("colLoginAndReqularParam") = 0 Then
                        dr3LR.Delete()
                    End If
                End If
            Next

            If Not dtMS Is Nothing Then
                If dtMS.Rows.Count > 0 Then
                    Dim dr4 As DataRow
                    For Each dr4 In dtMS.Rows
                        If Len(dr4("QCSampleResult")) = 0 Then
                            Dim dv3 As New DataView(dtMSD, "QcSampleID = '" & dr4("QcSampleID") & "' and  Parameter = '" & dr4("Parameter") & "'", "", DataViewRowState.CurrentRows)
                            If Not dv3 Is Nothing Then
                                If dv3.Count > 0 Then
                                    dr4("QCSampleResult") = dv3.Item(0)("QCSampleResult")
                                End If
                            End If
                        End If

                        Dim dv4 As New DataView(dtMSD, "coluqQCBatchID = '" & dr4("coluqQCBatchID") & "' and  Parameter = '" & dr4("Parameter") & "' and  Sampleid = '" & dr4("Sampleid") & "' and  Method = '" & dr4("Method") & "'", "", DataViewRowState.CurrentRows)
                        If Not dv4 Is Nothing Then
                            If dv4.Count > 0 Then
                                dr4("MSDResult") = dv4.Item(0)("MSResult")
                                dr4("MSDRec") = dv4.Item(0)("MSRec")
                                ' dr4("MSRec") = dv4.Item(0)("MSRec")
                                dr4("RPD") = dv4.Item(0)("MSDRPD")
                                dr4("RPDCLimits") = dv4.Item(0)("RPDCLimits")
                                'If SPName = "QCEnvCombo_RPT" Or SPName = "QC_Louisiana" Or SPName = "Env_QCPotraitRegular_RPT_SP" Then
                                dr4("MSDSpikeAdded") = dv4.Item(0)("SpikeAdded")
                                ' dr4("MSDSpikeAdded") = dv4.Item(0)("SpikeAmount")
                                'End If

                                If Not IsDBNull(dr4("Qualifier")) AndAlso Len(dr4("Qualifier")) > 0 Then
                                    Dim s1() As String = Split(dr4("Qualifier"), ",")
                                    Dim s As String
                                    If Not IsDBNull(dv4.Item(0)("Qualifier")) AndAlso Len(dv4.Item(0)("Qualifier")) > 0 Then
                                        Dim s2() As String = Split(dv4.Item(0)("Qualifier"), ",")
                                        Dim ss As String
                                        Dim fla As Boolean
                                        For Each ss In s2
                                            fla = False
                                            For Each s In s1
                                                If s = ss Then
                                                    fla = True
                                                End If
                                            Next
                                            If fla = False Then
                                                dr4("Qualifier") = dr4("Qualifier") & "," & ss
                                            End If
                                        Next
                                    End If
                                Else
                                    dr4("Qualifier") = dv4.Item(0)("Qualifier")
                                End If
                            End If
                        End If

                        For Each drLoginParam In dtLoginParam.Rows
                            If dr4("Parameter") = drLoginParam("Parameter") Then
                                dr4("colLoginAndReqularParam") = 1
                            End If
                        Next
                    Next
                End If
            End If

            Dim dr4LR As DataRow
            For Each dr4LR In dtMS.Rows
                If Not IsDBNull(dr4LR("colLoginAndReqularParam")) Then
                    If dr4LR("colLoginAndReqularParam") = 0 Then
                        dr4LR.Delete()
                    End If
                End If
            Next
            dtMS.AcceptChanges()
            dtLCS.AcceptChanges()

            ds.Tables.Add(dtLCS)
            ds.Tables.Add(dtMS)

            'For Check the Qc SubReport Data is available or not. By Aravind, 23 Sep 2017.
            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 AndAlso ds.Tables(0) IsNot Nothing AndAlso ds.Tables(0).Rows.Count > 0 Then
                Dim dtDeleteRow As DataTable = ds.Tables(0).Clone()
                Dim bolISAvailable As Boolean = False
                Dim i As Integer = 0
                For Each drr As DataRow In ds.Tables(0).Rows
                    If drr IsNot Nothing AndAlso Not IsDBNull(drr("Method")) AndAlso Len(drr("Method")) > 0 Then
                        If ds.Tables(1) IsNot Nothing AndAlso ds.Tables(1).Rows.Count > 0 Then
                            For Each dr1 As DataRow In ds.Tables(1).Rows
                                If bolISAvailable = False AndAlso Not IsDBNull(dr1("Method")) AndAlso Len(dr1("Method")) > 0 _
                                  AndAlso Not IsDBNull(dr1("QcBatchID")) AndAlso Len(dr1("QcBatchID")) > 0 AndAlso dr1("QcBatchID") = drr("QcBatchID") Then
                                    bolISAvailable = True
                                    GoTo A
                                    Exit For
                                End If
                            Next
                        End If
                        If ds.Tables(2) IsNot Nothing AndAlso ds.Tables(2).Rows.Count > 0 Then
                            For Each dr2 As DataRow In ds.Tables(2).Rows
                                If bolISAvailable = False AndAlso Not IsDBNull(dr2("Method")) AndAlso Len(dr2("Method")) > 0 _
                                  AndAlso Not IsDBNull(dr2("QcBatchID")) AndAlso Len(dr2("QcBatchID")) > 0 AndAlso dr2("QcBatchID") = drr("QcBatchID") Then
                                    bolISAvailable = True
                                    GoTo A
                                    Exit For
                                End If
                            Next
                        End If
                        'If ds.Tables(3) IsNot Nothing AndAlso ds.Tables(3).Rows.Count > 0 Then
                        '    For Each dr3 As DataRow In ds.Tables(3).Rows
                        '        If bolISAvailable = False AndAlso Not IsDBNull(dr3("Method")) AndAlso Len(dr3("Method")) > 0 AndAlso dr3("Method") = drr("Method") _
                        '          AndAlso Not IsDBNull(dr3("QcBatchID")) AndAlso Len(dr3("QcBatchID")) > 0 AndAlso dr3("QcBatchID") = drr("QcBatchID") Then
                        '            bolISAvailable = True
                        '            Exit For
                        '        End If
                        '    Next
                        'End If
                        If ds.Tables(5) IsNot Nothing AndAlso ds.Tables(5).Rows.Count > 0 Then
                            For Each dr5 As DataRow In ds.Tables(5).Rows
                                If bolISAvailable = False AndAlso Not IsDBNull(dr5("Method")) AndAlso Len(dr5("Method")) > 0 _
                                  AndAlso Not IsDBNull(dr5("QcBatchID")) AndAlso Len(dr5("QcBatchID")) > 0 AndAlso dr5("QcBatchID") = drr("QcBatchID") Then
                                    bolISAvailable = True
                                    GoTo A
                                    Exit For
                                End If
                            Next
                        End If
                        If ds.Tables(6) IsNot Nothing AndAlso ds.Tables(6).Rows.Count > 0 Then
                            For Each dr6 As DataRow In ds.Tables(6).Rows
                                If bolISAvailable = False AndAlso Not IsDBNull(dr6("Method")) AndAlso Len(dr6("Method")) > 0 _
                                  AndAlso Not IsDBNull(dr6("QcBatchID")) AndAlso Len(dr6("QcBatchID")) > 0 AndAlso dr6("QcBatchID") = drr("QcBatchID") Then
                                    bolISAvailable = True
                                    GoTo A
                                    Exit For
                                End If
                            Next
                        End If
A:                      If bolISAvailable = True Then
                            dtDeleteRow.ImportRow(drr)
                        End If
                    End If
                Next
                'If Not dtDeleteRow Is Nothing AndAlso dtDeleteRow.Rows.Count > 0 Then
                ds.Tables(0).Rows.Clear()
                ds.Tables(0).Merge(dtDeleteRow)
                'End If
            End If
            ds.AcceptChanges()
            Return ds
        Catch ex As Exception
            'MsgBox(ex.Message)
            Return New DataSet
        End Try
    End Function
    Public Shared Function ReplaceEscapeChars(ByVal Str As String) As String
        If Str = "" Then
            Return Str
        End If
        Str = Trim(Str)
        Str = Str.Replace("'", "''")
        Str = Str.Replace("""", """")
        Return Str
    End Function
    Public Shared Function RemoveDeliemeters(ByVal strString As String, ByVal strDeliemter As String) As String
        Try
            If strString <> "" AndAlso strDeliemter <> "" Then
                strString = Trim(strString)
                strString = strString.Replace(strDeliemter, "")
                Return strString
            End If
            Return strString
        Catch ex As Exception
            Return strString
        End Try
    End Function
End Class
