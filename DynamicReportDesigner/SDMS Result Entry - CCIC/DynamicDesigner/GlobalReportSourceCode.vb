Imports System.Collections
Imports System.IO
Imports System.Text
Imports DevExpress.XtraReports.UI
Imports DynamicReportBusinessLayer
Imports Modules.BusinessObjects.InfoClass

Public Module GlobalReportSourceCode

    ''' <summary>
    ''' For Filter Purpose
    ''' </summary>
    ''' <remarks></remarks>
    Public struqSamplingSampleID As String = String.Empty
    Public struqSampleParameterID As String = String.Empty
    Public strReportID As String = String.Empty
    Public strQuoteID As String = String.Empty
    Public stringQuoteID As String = String.Empty
    Public strItem As String = String.Empty
    Public struqQCBatchID As String = String.Empty
    Public struqQCTypeID As String = String.Empty
    Public strJobID As String = String.Empty
    Public strviewid As String = String.Empty
    Public strSqlCommandText As String = String.Empty
    Public strGridFilterPanelText As String()
    Public strAccreditation As String = String.Empty
    'Public objeBenchFormInfo As TSSTDSOGMLSSInfo.TSSTDSOGMLSSInfo
    Public dtTable As DataTable
    Public strHeaderPart As String = String.Empty
    Public struqMultiparamCalibrationID As String = String.Empty
    Public struqColormetricCalibrationID As String = String.Empty
    Public struqSampleTestID As String = String.Empty
    Private _dsDataSource As DataSet
    Private _dsQCDataSource As DataSet
    Public strLabwareID As String = String.Empty
    Public struqAssetLedgerCheckinID As String = String.Empty
    Public struqAssetLedgerDistributionID As String = String.Empty
    Public strLabwareInspectionID As String = String.Empty
    Public strLimsReportedDate As String = String.Empty
    Public strLimsReportedBy As String = String.Empty
    Public strLimsSignOffDate As String = String.Empty
    Public strLimsSignOffBy As String = String.Empty
    Public strProjectID As String = String.Empty
    Public strPageNo As String = String.Empty
    Public SuboutOrderID As String = String.Empty
    Public BottlesOrderOid As String = String.Empty
    Public arrreportobject As New ArrayList
    Public strSequenceID As String
    Public SCCOid As String = String.Empty
    Public strTestMethodID As String = String.Empty
    Public strParameterID As String = String.Empty
    Public strABID As String
    Public strDLQCID As String
    ''' <summary>
    ''' For Data Source
    ''' </summary>
    ''' <remarks></remarks>
    Public strReportedBy As String = String.Empty
    Public strApprovedBy As String = String.Empty
    Public UserName As String = String.Empty
    Public strReportingDate As String = String.Empty
    Public strReportIDDataSource As String = String.Empty
    Public strRegistrationID As String = String.Empty
    Public strSampleTransferMode As String = String.Empty
    Public strFromDate As String = String.Empty
    Public strToDate As String = String.Empty
    Public dtReportDataSource As DataTable
    Public strInvoiceComment As String = String.Empty
    Public struqTestParameterID As String = String.Empty
    Public SystemID As String = String.Empty
    Public strUqPrepBatchID As String = String.Empty
    Public strAnalyzedDate As String = String.Empty
    Public strReviewedDate As String = String.Empty
    Public strVerifiedDate As String = String.Empty
    Public strAnalyzedBy As String = String.Empty
    Public strReviewedBy As String = String.Empty
    Public strVerifiedBy As String = String.Empty
    Public strLT As String = String.Empty
    Public strlabwarebarcode As String = String.Empty
    Public strUserName As String = String.Empty
    Public strStorage As String = String.Empty
    Public strReportName As String = String.Empty
    Public NewObjLDMReportingVariables As LDMReportingVariables
    Public strMode As String = String.Empty
    Public strLogo As String = String.Empty
    Public strSampleID As String = String.Empty
    Public bolUpdateFinal As Boolean = True
    Public strpackageName As String = String.Empty
    Public TotalNumberPage As String = String.Empty
    Public strCPageComment As String = String.Empty
    Public CPNoOfSamples As String
    Public CPReportedDate As String
    Public bolSuboutStatus As Boolean = False
    Public CPReportStatus As String = String.Empty
    Public strAnalystBy As ArrayList = New ArrayList
    Public _dtUnitConversion As DataTable

    Public Sub ClearInitialParameters(Optional ByVal bolFromInitial As Boolean = False)
        struqSampleParameterID = String.Empty
        struqQCBatchID = String.Empty
        struqQCTypeID = String.Empty
        strSqlCommandText = String.Empty
        struqMultiparamCalibrationID = String.Empty
        struqColormetricCalibrationID = String.Empty
        struqSampleTestID = String.Empty
        strLabwareID = String.Empty
        struqAssetLedgerCheckinID = String.Empty
        struqAssetLedgerDistributionID = String.Empty
        strLabwareInspectionID = String.Empty
        strRegistrationID = String.Empty
        strFromDate = String.Empty
        strToDate = String.Empty
        strSampleTransferMode = String.Empty
        strUqPrepBatchID = String.Empty
        strJobID = String.Empty
        strUserName = String.Empty
        strStorage = String.Empty
        strLT = String.Empty
        strlabwarebarcode = String.Empty
        strItem = String.Empty
        strTestMethodID = String.Empty
        strParameterID = String.Empty
        'If bolFromInitial = True Then
        '    objeBenchFormInfo = New TSSTDSOGMLSSInfo.TSSTDSOGMLSSInfo
        'End If
    End Sub

    Public Sub ClearSamplingFieldEntryInitialParameters()
        struqSamplingSampleID = String.Empty
    End Sub

    Public Sub CollectuqSamplingSampleID(ByVal drRow As DataRow, ByVal strColumnName As String, Optional ByVal bolSelect As Boolean = True)
        If Not drRow Is Nothing AndAlso Not IsDBNull(drRow(strColumnName)) AndAlso Len(drRow(strColumnName)) > 0 Then
            If bolSelect = True Then
                If Not struqSamplingSampleID Is Nothing AndAlso Len(struqSamplingSampleID) > 0 Then
                    Dim strSamplingSampleID() As String = struqSamplingSampleID.Split(",")
                    Dim arruqSamplingSampleID As ArrayList = New ArrayList(strSamplingSampleID)
                    If arruqSamplingSampleID.IndexOf(drRow(strColumnName).ToString) > -1 Then
                    Else
                        struqSamplingSampleID = struqSamplingSampleID & "," & drRow(strColumnName)
                    End If
                Else
                    struqSamplingSampleID = drRow(strColumnName)
                End If
            Else
                If Not struqSamplingSampleID Is Nothing AndAlso Len(struqSamplingSampleID) > 0 Then
                    Dim strSamplingSampleID() As String = struqSamplingSampleID.Split(",")
                    Dim arruqSamplingSampleID As ArrayList = New ArrayList(strSamplingSampleID)
                    If arruqSamplingSampleID.IndexOf(drRow(strColumnName).ToString) > -1 Then
                        arruqSamplingSampleID.RemoveAt(arruqSamplingSampleID.IndexOf(drRow(strColumnName).ToString))
                        struqSamplingSampleID = String.Join(",", arruqSamplingSampleID.ToArray(GetType(String)))
                    End If

                    '    If Not struqSamplingSampleID Is Nothing AndAlso Len(struqSamplingSampleID) > 0 Then
                    '        If struqSamplingSampleID.Contains("," & drRow(strColmenName) & ",") = True OrElse struqSamplingSampleID.Contains(drRow(strColmenName) & ",") = True OrElse struqSamplingSampleID.Contains("," & drRow(strColmenName)) = True OrElse struqSamplingSampleID.Contains(drRow(strColmenName)) = True Then
                    '        Else
                    '            struqSamplingSampleID = struqSamplingSampleID & "," & drRow(strColmenName)
                    '        End If
                    '    Else
                    '        struqSamplingSampleID = drRow(strColmenName)
                    '    End If
                    'Else
                    '    If Not struqSamplingSampleID Is Nothing AndAlso Len(struqSamplingSampleID) > 0 Then
                    '        If struqSamplingSampleID.Contains("," & drRow(strColmenName) & ",") = True Then
                    '            struqSamplingSampleID = Replace(struqSamplingSampleID, "," & drRow(strColmenName) & ",", ",")
                    '        ElseIf struqSamplingSampleID.Contains(drRow(strColmenName) & ",") = True Then
                    '            struqSamplingSampleID = Replace(struqSamplingSampleID, drRow(strColmenName) & ",", "")
                    '        ElseIf struqSamplingSampleID.Contains("," & drRow(strColmenName)) = True Then
                    '            struqSamplingSampleID = Replace(struqSamplingSampleID, "," & drRow(strColmenName), "")
                    '        ElseIf struqSamplingSampleID.Contains(drRow(strColmenName)) = True Then
                    '            struqSamplingSampleID = Replace(struqSamplingSampleID, drRow(strColmenName), "")
                    '        End If
                End If
            End If
        End If
    End Sub

    Public Sub CollectuqSampleParameterID(ByVal drRow As DataRow, ByVal strColumnName As String, Optional ByVal bolSelect As Boolean = True)
        If Not drRow Is Nothing AndAlso Not IsDBNull(drRow(strColumnName)) AndAlso Len(drRow(strColumnName)) > 0 Then
            If bolSelect = True Then
                If Not struqSampleParameterID Is Nothing AndAlso Len(struqSampleParameterID) > 0 Then
                    Dim strSampleParameterID() As String = struqSampleParameterID.Split(",")
                    Dim arruqSampleParameterID As ArrayList = New ArrayList(strSampleParameterID)
                    If arruqSampleParameterID.IndexOf(drRow(strColumnName).ToString) > -1 Then
                    Else
                        struqSampleParameterID = struqSampleParameterID & "," & drRow(strColumnName)
                    End If
                Else
                    struqSampleParameterID = drRow(strColumnName)
                End If
            Else
                If Not struqSampleParameterID Is Nothing AndAlso Len(struqSampleParameterID) > 0 Then
                    Dim strSampleParameterID() As String = struqSampleParameterID.Split(",")
                    Dim arruqSampleParameterID As ArrayList = New ArrayList(strSampleParameterID)
                    If arruqSampleParameterID.IndexOf(drRow(strColumnName).ToString) > -1 Then
                        arruqSampleParameterID.RemoveAt(arruqSampleParameterID.IndexOf(drRow(strColumnName).ToString))
                        struqSampleParameterID = String.Join(",", arruqSampleParameterID.ToArray(GetType(String)))
                    End If
                    '    If Not struqSampleParameterID Is Nothing AndAlso Len(struqSampleParameterID) > 0 Then
                    '        If struqSampleParameterID.Contains("," & drRow(strColmenName) & ",") = True OrElse struqSampleParameterID.Contains(drRow(strColmenName) & ",") = True OrElse struqSampleParameterID.Contains("," & drRow(strColmenName)) = True OrElse struqSampleParameterID.Contains(drRow(strColmenName)) = True Then
                    '        Else
                    '            struqSampleParameterID = struqSampleParameterID & "," & drRow(strColmenName)
                    '        End If
                    '    Else
                    '        struqSampleParameterID = drRow(strColmenName)
                    '    End If
                    'Else
                    '    If Not struqSampleParameterID Is Nothing AndAlso Len(struqSampleParameterID) > 0 Then
                    '        If struqSampleParameterID.Contains("," & drRow(strColmenName) & ",") = True Then
                    '            struqSampleParameterID = Replace(struqSampleParameterID, "," & drRow(strColmenName) & ",", ",")
                    '        ElseIf struqSampleParameterID.Contains(drRow(strColmenName) & ",") = True Then
                    '            struqSampleParameterID = Replace(struqSampleParameterID, drRow(strColmenName) & ",", "")
                    '        ElseIf struqSampleParameterID.Contains("," & drRow(strColmenName)) = True Then
                    '            struqSampleParameterID = Replace(struqSampleParameterID, "," & drRow(strColmenName), "")
                    '        ElseIf struqSampleParameterID.Contains(drRow(strColmenName)) = True Then
                    '            struqSampleParameterID = Replace(struqSampleParameterID, drRow(strColmenName), "")
                    '        End If
                End If
            End If
        End If
    End Sub
    Public Sub CollectuqMultiparamCalibrationID(ByVal drRow As DataRow, ByVal strColumnName As String, Optional ByVal bolSelect As Boolean = True)
        If Not drRow Is Nothing AndAlso Not IsDBNull(drRow(strColumnName)) AndAlso Len(drRow(strColumnName)) > 0 Then
            If bolSelect = True Then
                If Not struqMultiparamCalibrationID Is Nothing AndAlso Len(struqMultiparamCalibrationID) > 0 Then
                    Dim strMultiparamCalibrationID() As String = struqMultiparamCalibrationID.Split(",")
                    Dim arruqMultiparamCalibrationID As ArrayList = New ArrayList(strMultiparamCalibrationID)
                    If arruqMultiparamCalibrationID.IndexOf(drRow(strColumnName).ToString) > -1 Then
                    Else
                        struqMultiparamCalibrationID = struqMultiparamCalibrationID & "," & drRow(strColumnName)
                    End If
                Else
                    struqMultiparamCalibrationID = drRow(strColumnName)
                End If
            Else
                If Not struqMultiparamCalibrationID Is Nothing AndAlso Len(struqMultiparamCalibrationID) > 0 Then
                    Dim strMultiparamCalibrationID() As String = struqMultiparamCalibrationID.Split(",")
                    Dim arruqMultiparamCalibrationID As ArrayList = New ArrayList(strMultiparamCalibrationID)
                    If arruqMultiparamCalibrationID.IndexOf(drRow(strColumnName).ToString) > -1 Then
                        arruqMultiparamCalibrationID.RemoveAt(arruqMultiparamCalibrationID.IndexOf(drRow(strColumnName).ToString))
                        struqMultiparamCalibrationID = String.Join(",", arruqMultiparamCalibrationID.ToArray(GetType(String)))
                    End If
                    '    If Not struqMultiparamCalibrationID Is Nothing AndAlso Len(struqMultiparamCalibrationID) > 0 Then
                    '        If struqMultiparamCalibrationID.Contains("," & drRow(strColmenName) & ",") = True OrElse struqMultiparamCalibrationID.Contains(drRow(strColmenName) & ",") = True OrElse struqMultiparamCalibrationID.Contains("," & drRow(strColmenName)) = True OrElse struqMultiparamCalibrationID.Contains(drRow(strColmenName)) = True Then
                    '        Else
                    '            struqMultiparamCalibrationID = struqMultiparamCalibrationID & "," & drRow(strColmenName)
                    '        End If
                    '    Else
                    '        struqMultiparamCalibrationID = drRow(strColmenName)
                    '    End If
                    'Else
                    '    If Not struqMultiparamCalibrationID Is Nothing AndAlso Len(struqMultiparamCalibrationID) > 0 Then
                    '        If struqMultiparamCalibrationID.Contains("," & drRow(strColmenName) & ",") = True Then
                    '            struqMultiparamCalibrationID = Replace(struqMultiparamCalibrationID, "," & drRow(strColmenName) & ",", ",")
                    '        ElseIf struqMultiparamCalibrationID.Contains(drRow(strColmenName) & ",") = True Then
                    '            struqMultiparamCalibrationID = Replace(struqMultiparamCalibrationID, drRow(strColmenName) & ",", "")
                    '        ElseIf struqMultiparamCalibrationID.Contains("," & drRow(strColmenName)) = True Then
                    '            struqMultiparamCalibrationID = Replace(struqMultiparamCalibrationID, "," & drRow(strColmenName), "")
                    '        ElseIf struqMultiparamCalibrationID.Contains(drRow(strColmenName)) = True Then
                    '            struqMultiparamCalibrationID = Replace(struqMultiparamCalibrationID, drRow(strColmenName), "")
                    '        End If
                End If
            End If
        End If
    End Sub
    Public Sub CollectuqColormetricCalibrationID(ByVal drRow As DataRow, ByVal strColumnName As String, Optional ByVal bolSelect As Boolean = True)
        If Not drRow Is Nothing AndAlso Not IsDBNull(drRow(strColumnName)) AndAlso Len(drRow(strColumnName)) > 0 Then
            If bolSelect = True Then
                If Not struqColormetricCalibrationID Is Nothing AndAlso Len(struqColormetricCalibrationID) > 0 Then
                    Dim strColormetricCalibrationID() As String = struqColormetricCalibrationID.Split(",")
                    Dim arrColormetricCalibrationID As ArrayList = New ArrayList(strColormetricCalibrationID)
                    If arrColormetricCalibrationID.IndexOf(drRow(strColumnName).ToString) > -1 Then
                    Else
                        struqColormetricCalibrationID = struqColormetricCalibrationID & "," & drRow(strColumnName)
                    End If
                Else
                    struqColormetricCalibrationID = drRow(strColumnName)
                End If
            Else
                If Not struqColormetricCalibrationID Is Nothing AndAlso Len(struqColormetricCalibrationID) > 0 Then
                    Dim strColormetricCalibrationID() As String = struqColormetricCalibrationID.Split(",")
                    Dim arrColormetricCalibrationID As ArrayList = New ArrayList(strColormetricCalibrationID)
                    If arrColormetricCalibrationID.IndexOf(drRow(strColumnName).ToString) > -1 Then
                        arrColormetricCalibrationID.RemoveAt(arrColormetricCalibrationID.IndexOf(drRow(strColumnName).ToString))
                        struqColormetricCalibrationID = String.Join(",", arrColormetricCalibrationID.ToArray(GetType(String)))
                    End If
                    '    If Not struqColormetricCalibrationID Is Nothing AndAlso Len(struqColormetricCalibrationID) > 0 Then
                    '        If struqColormetricCalibrationID.Contains("," & drRow(strColmenName) & ",") = True OrElse struqColormetricCalibrationID.Contains(drRow(strColmenName) & ",") = True OrElse struqColormetricCalibrationID.Contains("," & drRow(strColmenName)) = True OrElse struqColormetricCalibrationID.Contains(drRow(strColmenName)) = True Then
                    '        Else
                    '            struqColormetricCalibrationID = struqColormetricCalibrationID & "," & drRow(strColmenName)
                    '        End If
                    '    Else
                    '        struqColormetricCalibrationID = drRow(strColmenName)
                    '    End If
                    'Else
                    '    If Not struqColormetricCalibrationID Is Nothing AndAlso Len(struqColormetricCalibrationID) > 0 Then
                    '        If struqColormetricCalibrationID.Contains("," & drRow(strColmenName) & ",") = True Then
                    '            struqColormetricCalibrationID = Replace(struqColormetricCalibrationID, "," & drRow(strColmenName) & ",", ",")
                    '        ElseIf struqColormetricCalibrationID.Contains(drRow(strColmenName) & ",") = True Then
                    '            struqColormetricCalibrationID = Replace(struqColormetricCalibrationID, drRow(strColmenName) & ",", "")
                    '        ElseIf struqColormetricCalibrationID.Contains("," & drRow(strColmenName)) = True Then
                    '            struqColormetricCalibrationID = Replace(struqColormetricCalibrationID, "," & drRow(strColmenName), "")
                    '        ElseIf struqColormetricCalibrationID.Contains(drRow(strColmenName)) = True Then
                    '            struqColormetricCalibrationID = Replace(struqColormetricCalibrationID, drRow(strColmenName), "")
                    '        End If
                End If
            End If
        End If
    End Sub
    Public Sub CollectLabwareInspectionID(ByVal LabwareInspectionID As String, Optional ByVal bolSelect As Boolean = True)
        If Not LabwareInspectionID Is Nothing AndAlso Len(LabwareInspectionID) > 0 Then
            If bolSelect = True Then
                strLabwareInspectionID = String.Empty
                If Not strLabwareInspectionID Is Nothing AndAlso Len(strLabwareInspectionID) > 0 Then
                    If strLabwareInspectionID.Contains("," & LabwareInspectionID & ",") = True OrElse strLabwareInspectionID.Contains(LabwareInspectionID & ",") = True OrElse strLabwareInspectionID.Contains("," & LabwareInspectionID) = True OrElse strLabwareInspectionID.Contains(LabwareInspectionID) = True Then
                    Else
                        strLabwareInspectionID = strLabwareInspectionID & "," & LabwareInspectionID
                    End If
                Else
                    strLabwareInspectionID = LabwareInspectionID
                End If
            Else
                If Not strLabwareInspectionID Is Nothing AndAlso Len(strLabwareInspectionID) > 0 Then
                    If strLabwareInspectionID.Contains("," & LabwareInspectionID & ",") = True Then
                        strLabwareInspectionID = Replace(strLabwareInspectionID, "," & LabwareInspectionID & ",", ",")
                    ElseIf strLabwareInspectionID.Contains(LabwareInspectionID & ",") = True Then
                        strLabwareInspectionID = Replace(strLabwareInspectionID, LabwareInspectionID & ",", ",")
                    ElseIf strLabwareInspectionID.Contains("," & LabwareInspectionID) = True Then
                        strLabwareInspectionID = Replace(strLabwareInspectionID, "," & LabwareInspectionID, ",")
                    ElseIf strLabwareInspectionID.Contains(LabwareInspectionID) = True Then
                        strLabwareInspectionID = Replace(strLabwareInspectionID, LabwareInspectionID, "")
                    End If
                End If
            End If
        End If
    End Sub
    Public Sub CollectLabwareID(ByVal LabwareID As String, Optional ByVal bolSelect As Boolean = True)
        If Not LabwareID Is Nothing AndAlso Len(LabwareID) > 0 Then
            If bolSelect = True Then
                'strLabwareID = String.Empty
                If Not strLabwareID Is Nothing AndAlso Len(strLabwareID) > 0 Then
                    If strLabwareID.Contains("," & LabwareID & ",") = True OrElse strLabwareID.Contains(LabwareID & ",") = True OrElse strLabwareID.Contains("," & LabwareID) = True OrElse strLabwareID.Contains(LabwareID) = True Then
                    Else
                        'strLabwareID = strLabwareID & "," & LabwareID
                        strLabwareID = strLabwareID & ",N'" & LabwareID & "'"
                    End If
                Else
                    'strLabwareID = LabwareID
                    strLabwareID = "N'" & LabwareID & "'"
                End If
            Else
                If Not strLabwareID Is Nothing AndAlso Len(strLabwareID) > 0 Then
                    If strLabwareID.Contains("," & LabwareID & ",") = True Then
                        strLabwareID = Replace(strLabwareID, "," & LabwareID & ",", ",")
                    ElseIf strLabwareID.Contains(LabwareID & ",") = True Then
                        strLabwareID = Replace(strLabwareID, LabwareID & ",", ",")
                    ElseIf strLabwareID.Contains("," & LabwareID) = True Then
                        strLabwareID = Replace(strLabwareID, "," & LabwareID, ",")
                    ElseIf strLabwareID.Contains(LabwareID) = True Then
                        strLabwareID = Replace(strLabwareID, LabwareID, "")
                    End If
                End If
            End If
        End If
    End Sub
    Public Sub CollectuqAssetLedgerCheckinID(ByVal drRow As DataRow, ByVal strColumnName As String, Optional ByVal bolSelect As Boolean = True)
        If Not drRow Is Nothing AndAlso Not IsDBNull(drRow(strColumnName)) AndAlso Len(drRow(strColumnName)) > 0 Then
            If bolSelect = True Then
                If Not struqAssetLedgerCheckinID Is Nothing AndAlso Len(struqAssetLedgerCheckinID) > 0 Then
                    Dim strAssetLedgerCheckinID() As String = struqAssetLedgerCheckinID.Split(",")
                    Dim arruqAssetLedgerCheckinID As ArrayList = New ArrayList(strAssetLedgerCheckinID)
                    If arruqAssetLedgerCheckinID.IndexOf(drRow(strColumnName).ToString) > -1 Then
                    Else
                        struqAssetLedgerCheckinID = struqAssetLedgerCheckinID & "," & drRow(strColumnName)
                    End If
                Else
                    struqAssetLedgerCheckinID = drRow(strColumnName)
                End If
            Else
                If Not struqAssetLedgerCheckinID Is Nothing AndAlso Len(struqAssetLedgerCheckinID) > 0 Then
                    Dim strAssetLedgerCheckinID() As String = struqAssetLedgerCheckinID.Split(",")
                    Dim arruqAssetLedgerCheckinID As ArrayList = New ArrayList(strAssetLedgerCheckinID)
                    If arruqAssetLedgerCheckinID.IndexOf(drRow(strColumnName).ToString) > -1 Then
                        arruqAssetLedgerCheckinID.RemoveAt(arruqAssetLedgerCheckinID.IndexOf(drRow(strColumnName).ToString))
                        struqAssetLedgerCheckinID = String.Join(",", arruqAssetLedgerCheckinID.ToArray(GetType(String)))
                    End If
                End If
            End If
        End If
    End Sub
    Public Sub CollectuqAssetLedgerDistributionID(ByVal drRow As DataRow, ByVal strColumnName As String, Optional ByVal bolSelect As Boolean = True)
        If Not strColumnName Is Nothing AndAlso Len(strColumnName) > 0 Then
            If Not drRow Is Nothing AndAlso Not IsDBNull(drRow(strColumnName)) AndAlso Len(drRow(strColumnName)) > 0 Then
                If bolSelect = True Then
                    If Not struqAssetLedgerDistributionID Is Nothing AndAlso Len(struqAssetLedgerDistributionID) > 0 Then
                        Dim strAssetLedgerDistributionID() As String = struqAssetLedgerDistributionID.Split(",")
                        Dim arruqAssetLedgerDistributionID As ArrayList = New ArrayList(strAssetLedgerDistributionID)
                        If arruqAssetLedgerDistributionID.IndexOf(drRow(strColumnName).ToString) > -1 Then
                        Else
                            struqAssetLedgerDistributionID = struqAssetLedgerDistributionID & "," & drRow(strColumnName)
                        End If
                    Else
                        struqAssetLedgerDistributionID = drRow(strColumnName)
                    End If
                Else
                    If Not struqAssetLedgerDistributionID Is Nothing AndAlso Len(struqAssetLedgerDistributionID) > 0 Then
                        Dim strAssetLedgerDistributionID() As String = struqAssetLedgerDistributionID.Split(",")
                        Dim arruqAssetLedgerDistributionID As ArrayList = New ArrayList(strAssetLedgerDistributionID)
                        If arruqAssetLedgerDistributionID.IndexOf(drRow(strColumnName).ToString) > -1 Then
                            arruqAssetLedgerDistributionID.RemoveAt(arruqAssetLedgerDistributionID.IndexOf(drRow(strColumnName).ToString))
                            struqAssetLedgerDistributionID = String.Join(",", arruqAssetLedgerDistributionID.ToArray(GetType(String)))
                        End If
                    End If
                End If
            End If
        End If
    End Sub
    Public Sub CollectuqSampleParameterID(ByVal dvView As DataView, ByVal strColumnName As String, Optional ByVal bolSelect As Boolean = True)
        'If Not dvView Is Nothing AndAlso dvView.Count > 0 Then
        '    For Each drvView As DataRowView In dvView
        '        If Not IsDBNull(drvView(strColumnName)) AndAlso Len(drvView(strColumnName)) > 0 Then
        '            If bolSelect = True Then
        '                If Not struqSampleParameterID Is Nothing AndAlso Len(struqSampleParameterID) > 0 Then
        '                    Dim strSampleParameterID() As String = struqSampleParameterID.Split(",")
        '                    Dim arruqSampleParameterID As ArrayList = New ArrayList(strSampleParameterID)
        '                    If arruqSampleParameterID.IndexOf(drvView(strColumnName).ToString) > -1 Then
        '                    Else
        '                        struqSampleParameterID = struqSampleParameterID & "," & drvView(strColumnName)
        '                    End If
        '                Else
        '                    struqSampleParameterID = drvView(strColumnName)
        '                End If
        '            Else
        '                If Not struqSampleParameterID Is Nothing AndAlso Len(struqSampleParameterID) > 0 Then
        '                    Dim strSampleParameterID() As String = struqSampleParameterID.Split(",")
        '                    Dim arruqSampleParameterID As ArrayList = New ArrayList(strSampleParameterID)
        '                    If arruqSampleParameterID.IndexOf(drvView(strColumnName).ToString) > -1 Then
        '                        arruqSampleParameterID.RemoveAt(arruqSampleParameterID.IndexOf(drvView(strColumnName).ToString))
        '                        struqSampleParameterID = String.Join(",", arruqSampleParameterID.ToArray(GetType(String)))
        '                    End If
        '                    '    If Not struqSampleParameterID Is Nothing AndAlso Len(struqSampleParameterID) > 0 Then
        '                    '        If struqSampleParameterID.Contains("," & drvView(strColmenName) & ",") = True OrElse struqSampleParameterID.Contains(drvView(strColmenName) & ",") = True OrElse struqSampleParameterID.Contains("," & drvView(strColmenName)) = True OrElse struqSampleParameterID.Contains(drvView(strColmenName)) = True Then
        '                    '        Else
        '                    '            struqSampleParameterID = struqSampleParameterID & "," & drvView(strColmenName)
        '                    '        End If
        '                    '    Else
        '                    '        struqSampleParameterID = drvView(strColmenName)
        '                    '    End If
        '                    'Else
        '                    '    If Not struqSampleParameterID Is Nothing AndAlso Len(struqSampleParameterID) > 0 Then
        '                    '        If struqSampleParameterID.Contains("," & drvView(strColmenName) & ",") = True Then
        '                    '            struqSampleParameterID = Replace(struqSampleParameterID, "," & drvView(strColmenName) & ",", ",")
        '                    '        ElseIf struqSampleParameterID.Contains(drvView(strColmenName) & ",") = True Then
        '                    '            struqSampleParameterID = Replace(struqSampleParameterID, drvView(strColmenName) & ",", ",")
        '                    '        ElseIf struqSampleParameterID.Contains("," & drvView(strColmenName)) = True Then
        '                    '            struqSampleParameterID = Replace(struqSampleParameterID, "," & drvView(strColmenName), ",")
        '                    '        ElseIf struqSampleParameterID.Contains(drvView(strColmenName)) = True Then
        '                    '            struqSampleParameterID = Replace(struqSampleParameterID, drvView(strColmenName), "")
        '                    '        End If
        '                End If
        '            End If
        '        End If
        '    Next
        'End If

        If Not dvView Is Nothing AndAlso dvView.Count > 0 Then
            Dim dtSampleParameter As DataTable = dvView.ToTable
            struqSampleParameterID = String.Empty
            Dim objConvert As New BTDataTableConverter
            struqSampleParameterID = objConvert.DataTableToString(dtSampleParameter, strColumnName, bolSelect & " and " & strColumnName & " > 0")
        End If

    End Sub

    Public Sub CollectuqQCBatchID(ByVal drRow As DataRow, ByVal strColumnName As String, Optional ByVal bolSelect As Boolean = True)
        If Not drRow Is Nothing AndAlso Not IsDBNull(drRow(strColumnName)) AndAlso Len(drRow(strColumnName)) > 0 Then
            If bolSelect = True Then
                If Not struqQCBatchID Is Nothing AndAlso Len(struqQCBatchID) > 0 Then
                    Dim strQCBatchID() As String = struqQCBatchID.Split(",")
                    Dim arruqQCBatchID As ArrayList = New ArrayList(strQCBatchID)
                    If arruqQCBatchID.IndexOf(drRow(strColumnName).ToString) > -1 Then
                    Else
                        struqQCBatchID = struqQCBatchID & "," & drRow(strColumnName)
                    End If
                Else
                    struqQCBatchID = drRow(strColumnName)
                End If
            Else
                If Not struqQCBatchID Is Nothing AndAlso Len(struqQCBatchID) > 0 Then
                    Dim strQCBatchID() As String = struqQCBatchID.Split(",")
                    Dim arruqQCBatchID As ArrayList = New ArrayList(strQCBatchID)
                    If arruqQCBatchID.IndexOf(drRow(strColumnName).ToString) > -1 Then
                        arruqQCBatchID.RemoveAt(arruqQCBatchID.IndexOf(drRow(strColumnName).ToString))
                        struqQCBatchID = String.Join(",", arruqQCBatchID.ToArray(GetType(String)))
                    End If

                    '    If Not struqQCBatchID Is Nothing AndAlso Len(struqQCBatchID) > 0 Then
                    '        If struqQCBatchID.Contains("," & drRow(strColmenName) & ",") = True OrElse struqQCBatchID.Contains(drRow(strColmenName) & ",") = True OrElse struqQCBatchID.Contains("," & drRow(strColmenName)) = True OrElse struqQCBatchID.Contains(drRow(strColmenName)) = True Then
                    '        Else
                    '            struqQCBatchID = struqQCBatchID & "," & drRow(strColmenName)
                    '        End If
                    '    Else
                    '        struqQCBatchID = drRow(strColmenName)
                    '    End If
                    'Else
                    '    If Not struqQCBatchID Is Nothing AndAlso Len(struqQCBatchID) > 0 Then
                    '        If struqQCBatchID.Contains("," & drRow(strColmenName) & ",") = True Then
                    '            struqQCBatchID = Replace(struqQCBatchID, "," & drRow(strColmenName) & ",", ",")
                    '        ElseIf struqQCBatchID.Contains(drRow(strColmenName) & ",") = True Then
                    '            struqQCBatchID = Replace(struqQCBatchID, drRow(strColmenName) & ",", ",")
                    '        ElseIf struqQCBatchID.Contains("," & drRow(strColmenName)) = True Then
                    '            struqQCBatchID = Replace(struqQCBatchID, "," & drRow(strColmenName), ",")
                    '        ElseIf struqQCBatchID.Contains(drRow(strColmenName)) = True Then
                    '            struqQCBatchID = Replace(struqQCBatchID, drRow(strColmenName), "")
                    '        End If
                End If
            End If
        End If
    End Sub

    Public Sub CollectuqQCBatchID(ByVal dvView As DataView, ByVal strColumnName As String, Optional ByVal bolSelect As Boolean = True)
        If Not dvView Is Nothing AndAlso dvView.Count > 0 Then
            For Each drvView As DataRowView In dvView
                If Not IsDBNull(drvView(strColumnName)) AndAlso Len(drvView(strColumnName)) > 0 Then
                    If bolSelect = True Then
                        If Not struqQCBatchID Is Nothing AndAlso Len(struqQCBatchID) > 0 Then
                            Dim strQCBatchID() As String = struqQCBatchID.Split(",")
                            Dim arrQCBatchID As ArrayList = New ArrayList(strQCBatchID)
                            If arrQCBatchID.IndexOf(drvView(strColumnName).ToString) > -1 Then
                            Else
                                struqQCBatchID = struqQCBatchID & "," & drvView(strColumnName)
                            End If
                        Else
                            struqQCBatchID = drvView(strColumnName)
                        End If
                    Else
                        If Not struqQCBatchID Is Nothing AndAlso Len(struqQCBatchID) > 0 Then
                            Dim strQCBatchID() As String = struqQCBatchID.Split(",")
                            Dim arrQCBatchID As ArrayList = New ArrayList(strQCBatchID)
                            If arrQCBatchID.IndexOf(drvView(strColumnName).ToString) > -1 Then
                                arrQCBatchID.RemoveAt(arrQCBatchID.IndexOf(drvView(strColumnName).ToString))
                                struqQCBatchID = String.Join(",", arrQCBatchID.ToArray(GetType(String)))
                            End If
                            '    If Not struqQCBatchID Is Nothing AndAlso Len(struqQCBatchID) > 0 Then
                            '        If struqQCBatchID.Contains("," & drvView(strColmenName) & ",") = True OrElse struqQCBatchID.Contains(drvView(strColmenName) & ",") = True OrElse struqQCBatchID.Contains("," & drvView(strColmenName)) = True OrElse struqQCBatchID.Contains(drvView(strColmenName)) = True Then
                            '        Else
                            '            struqQCBatchID = struqQCBatchID & "," & drvView(strColmenName)
                            '        End If
                            '    Else
                            '        struqQCBatchID = drvView(strColmenName)
                            '    End If
                            'Else
                            '    If Not struqQCBatchID Is Nothing AndAlso Len(struqQCBatchID) > 0 Then
                            '        If struqQCBatchID.Contains("," & drvView(strColmenName) & ",") = True Then
                            '            struqQCBatchID = Replace(struqQCBatchID, "," & drvView(strColmenName) & ",", ",")
                            '        ElseIf struqQCBatchID.Contains(drvView(strColmenName) & ",") = True Then
                            '            struqQCBatchID = Replace(struqQCBatchID, drvView(strColmenName) & ",", ",")
                            '        ElseIf struqQCBatchID.Contains("," & drvView(strColmenName)) = True Then
                            '            struqQCBatchID = Replace(struqQCBatchID, "," & drvView(strColmenName), ",")
                            '        ElseIf struqQCBatchID.Contains(drvView(strColmenName)) = True Then
                            '            struqQCBatchID = Replace(struqQCBatchID, drvView(strColmenName), "")
                            '        End If
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    Public Sub CollectuqQCTypeID(ByVal drRow As DataRow, ByVal strColumnName As String, Optional ByVal bolSelect As Boolean = True)
        If Not drRow Is Nothing AndAlso Not IsDBNull(drRow(strColumnName)) AndAlso Len(drRow(strColumnName)) > 0 Then
            If bolSelect = True Then
                If Not struqQCTypeID Is Nothing AndAlso Len(struqQCTypeID) > 0 Then
                    Dim strQCTypeID() As String = struqQCTypeID.Split(",")
                    Dim arruqQCTypeID As ArrayList = New ArrayList(strQCTypeID)
                    If arruqQCTypeID.IndexOf(drRow(strColumnName).ToString) > -1 Then
                    Else
                        struqQCTypeID = struqQCTypeID & "," & drRow(strColumnName)
                    End If
                Else
                    struqQCTypeID = drRow(strColumnName)
                End If
            Else
                If Not struqQCTypeID Is Nothing AndAlso Len(struqQCTypeID) > 0 Then
                    Dim strQCTypeID() As String = struqQCTypeID.Split(",")
                    Dim arruqQCTypeID As ArrayList = New ArrayList(strQCTypeID)
                    If arruqQCTypeID.IndexOf(drRow(strColumnName).ToString) > -1 Then
                        arruqQCTypeID.RemoveAt(arruqQCTypeID.IndexOf(drRow(strColumnName).ToString))
                        struqQCTypeID = String.Join(",", arruqQCTypeID.ToArray(GetType(String)))
                    End If
                    '    If Not struqQCTypeID Is Nothing AndAlso Len(struqQCTypeID) > 0 Then
                    '        If struqQCTypeID.Contains("," & drRow(strColmenName) & ",") = True OrElse struqQCTypeID.Contains(drRow(strColmenName) & ",") = True OrElse struqQCTypeID.Contains("," & drRow(strColmenName)) = True OrElse struqQCTypeID.Contains(drRow(strColmenName)) = True Then
                    '        Else
                    '            struqQCTypeID = struqQCTypeID & "," & drRow(strColmenName)
                    '        End If
                    '    Else
                    '        struqQCTypeID = drRow(strColmenName)
                    '    End If
                    'Else
                    '    If Not struqQCTypeID Is Nothing AndAlso Len(struqQCTypeID) > 0 Then
                    '        If struqQCTypeID.Contains("," & drRow(strColmenName) & ",") = True Then
                    '            struqQCTypeID = Replace(struqQCTypeID, "," & drRow(strColmenName) & ",", ",")
                    '        ElseIf struqQCTypeID.Contains(drRow(strColmenName) & ",") = True Then
                    '            struqQCTypeID = Replace(struqQCTypeID, drRow(strColmenName) & ",", ",")
                    '        ElseIf struqQCTypeID.Contains("," & drRow(strColmenName)) = True Then
                    '            struqQCTypeID = Replace(struqQCTypeID, "," & drRow(strColmenName), ",")
                    '        ElseIf struqQCTypeID.Contains(drRow(strColmenName)) = True Then
                    '            struqQCTypeID = Replace(struqQCTypeID, drRow(strColmenName), "")
                    '        End If
                End If
            End If
        End If
    End Sub

    Public Sub CollectuqQCTypeID(ByVal dvView As DataView, ByVal strColumnName As String, Optional ByVal bolSelect As Boolean = True)
        If Not dvView Is Nothing AndAlso dvView.Count > 0 Then
            For Each drvView As DataRowView In dvView
                If Not IsDBNull(drvView(strColumnName)) AndAlso Len(drvView(strColumnName)) > 0 Then
                    If bolSelect = True Then
                        If Not struqQCTypeID Is Nothing AndAlso Len(struqQCTypeID) > 0 Then
                            Dim strQCTypeID() As String = struqQCTypeID.Split(",")
                            Dim arruqQCTypeID As ArrayList = New ArrayList(strQCTypeID)
                            If arruqQCTypeID.IndexOf(drvView(strColumnName).ToString) > -1 Then
                            Else
                                struqQCTypeID = struqQCTypeID & "," & drvView(strColumnName)
                            End If
                        Else
                            struqQCTypeID = drvView(strColumnName)
                        End If
                    Else
                        If Not struqQCTypeID Is Nothing AndAlso Len(struqQCTypeID) > 0 Then
                            Dim strQCTypeID() As String = struqQCTypeID.Split(",")
                            Dim arruqQCTypeID As ArrayList = New ArrayList(strQCTypeID)
                            If arruqQCTypeID.IndexOf(drvView(strColumnName).ToString) > -1 Then
                                arruqQCTypeID.RemoveAt(arruqQCTypeID.IndexOf(drvView(strColumnName).ToString))
                                struqQCTypeID = String.Join(",", arruqQCTypeID.ToArray(GetType(String)))
                            End If
                            '    If Not struqQCTypeID Is Nothing AndAlso Len(struqQCTypeID) > 0 Then
                            '        If struqQCTypeID.Contains("," & drvView(strColmenName) & ",") = True OrElse struqQCTypeID.Contains(drvView(strColmenName) & ",") = True OrElse struqQCTypeID.Contains("," & drvView(strColmenName)) = True OrElse struqQCTypeID.Contains(drvView(strColmenName)) = True Then
                            '        Else
                            '            struqQCTypeID = struqQCTypeID & "," & drvView(strColmenName)
                            '        End If
                            '    Else
                            '        struqQCTypeID = drvView(strColmenName)
                            '    End If
                            'Else
                            '    If Not struqQCTypeID Is Nothing AndAlso Len(struqQCTypeID) > 0 Then
                            '        If struqQCTypeID.Contains("," & drvView(strColmenName) & ",") = True Then
                            '            struqQCTypeID = Replace(struqQCTypeID, "," & drvView(strColmenName) & ",", ",")
                            '        ElseIf struqQCTypeID.Contains(drvView(strColmenName) & ",") = True Then
                            '            struqQCTypeID = Replace(struqQCTypeID, drvView(strColmenName) & ",", ",")
                            '        ElseIf struqQCTypeID.Contains("," & drvView(strColmenName)) = True Then
                            '            struqQCTypeID = Replace(struqQCTypeID, "," & drvView(strColmenName), ",")
                            '        ElseIf struqQCTypeID.Contains(drvView(strColmenName)) = True Then
                            '            struqQCTypeID = Replace(struqQCTypeID, drvView(strColmenName), "")
                            '        End If
                        End If
                    End If
                End If
            Next
        End If
    End Sub
    Public Sub CollectuqPrepBatcID(ByVal drRow As DataRow, ByVal strColumnName As String, Optional ByVal bolSelect As Boolean = True)
        If Not drRow Is Nothing AndAlso Not IsDBNull(drRow(strColumnName)) AndAlso Len(drRow(strColumnName)) > 0 Then
            If Not drRow Is Nothing AndAlso Not IsDBNull(drRow(strColumnName)) AndAlso Len(drRow(strColumnName)) > 0 Then
                If bolSelect = True Then
                    If Not strUqPrepBatchID Is Nothing AndAlso Len(strUqPrepBatchID) > 0 Then
                        Dim strbUqPrepBatchID() As String = struqAssetLedgerDistributionID.Split(",")
                        Dim arrUqPrepBatchID As ArrayList = New ArrayList(strbUqPrepBatchID)
                        If arrUqPrepBatchID.IndexOf(drRow(strColumnName).ToString) > -1 Then
                        Else
                            strUqPrepBatchID = strUqPrepBatchID & "," & drRow(strColumnName)
                        End If
                    Else
                        strUqPrepBatchID = drRow(strColumnName)
                    End If
                Else
                    If Not strUqPrepBatchID Is Nothing AndAlso Len(strUqPrepBatchID) > 0 Then
                        Dim strbUqPrepBatchID() As String = strUqPrepBatchID.Split(",")
                        Dim arrUqPrepBatchID As ArrayList = New ArrayList(strbUqPrepBatchID)
                        If arrUqPrepBatchID.IndexOf(drRow(strColumnName).ToString) > -1 Then
                            arrUqPrepBatchID.RemoveAt(arrUqPrepBatchID.IndexOf(drRow(strColumnName).ToString))
                            strUqPrepBatchID = String.Join(",", arrUqPrepBatchID.ToArray(GetType(String)))
                        End If
                    End If
                End If
            End If
        End If
    End Sub
    'Public Sub AddReportControls(ByVal objBBReport As DevExpress.XtraBars.BarSubItem, ByVal FormName As DynamicReportingFormName, Optional ByVal objForm As Object = Nothing)
    '    Dim drFilterByFormN() As DataRow = Nothing
    '    If CheckUserName(UserName) = True Then
    '        Dim dtReport As DataTable = GetQueryData("EBenchFroms_ReportFormDetails")
    '        If Not dtReport Is Nothing AndAlso dtReport.Rows.Count > 0 Then
    '            drFilterByFormN = dtReport.Select("FormNumber ='" & FormName & "'")
    '        End If
    '    Else
    '        Dim dtReport As DataTable = GetQueryData("EBenchFroms_ReportFormDetails_WithUser")
    '        If Not dtReport Is Nothing AndAlso dtReport.Rows.Count > 0 Then
    '            drFilterByFormN = dtReport.Select("FormNumber ='" & FormName & "' and UserName = '" & UserName & "'")
    '        End If
    '    End If

    '    If Not drFilterByFormN Is Nothing AndAlso drFilterByFormN.Length > 0 Then
    '        objBBReport.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
    '        For Each drR As DataRow In drFilterByFormN
    '            Dim bbItems As New DevExpress.XtraBars.BarButtonItem
    '            bbItems.Name = drR("CustomReportDesignerName_Default")
    '            bbItems.Caption = drR("CustomReportDesignerName")
    '            bbItems.Tag = drR("CustomReportDesignerName_Default")
    '            objBBReport.AddItem(bbItems)
    '            If Not objForm Is Nothing Then
    '                AddHandler bbItems.ItemClick, AddressOf objForm.mReport_ItemClick
    '            Else
    '                AddHandler bbItems.ItemClick, AddressOf bbReportItems_ItemClick
    '            End If
    '        Next
    '    Else
    '        objBBReport.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    '    End If
    'End Sub

    'Public Sub AddReportControls(ByVal objBBReport As DevExpress.XtraBars.BarSubItem, ByVal dtReportDetails As DataTable, _
    '                             ByVal FormName As DynamicReportingFormName)
    '    If Not objBBReport Is Nothing Then
    '        objBBReport.ClearLinks()
    '    End If

    '    If Not dtReportDetails Is Nothing AndAlso dtReportDetails.Rows.Count > 0 Then
    '        Dim drFilterByFormN() As DataRow = Nothing
    '        drFilterByFormN = dtReportDetails.Select("FormNumber ='" & FormName & "'")
    '        If Not drFilterByFormN Is Nothing AndAlso drFilterByFormN.Length > 0 Then
    '            objBBReport.Visibility = DevExpress.XtraBars.BarItemVisibility.Always
    '            For Each drR As DataRow In drFilterByFormN
    '                Dim bbItems As New DevExpress.XtraBars.BarButtonItem
    '                bbItems.Name = drR("CustomReportDesignerName_Default")
    '                bbItems.Caption = drR("CustomReportDesignerName")
    '                bbItems.Tag = drR("CustomReportDesignerName_Default")
    '                objBBReport.AddItem(bbItems)
    '                AddHandler bbItems.ItemClick, AddressOf bbReportItems_ItemClick
    '            Next
    '        Else
    '            objBBReport.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    '        End If
    '    Else
    '        objBBReport.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
    '    End If
    'End Sub

    'Public Sub bbReportItems_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs)
    '    Try
    '        Dim xtraRep As XtraReport = GetReportFromLayOut(e.Item.Tag, False)
    '        If Not xtraRep Is Nothing Then
    '            'xtraRep.ShowPreview()
    '        End If
    '    Catch ex As Exception
    '        BLCommon.InsertExceptionTrackingBL(ex, "", Reflection.MethodInfo.GetCurrentMethod.Name)
    '    End Try
    'End Sub
    'Public Function bbReportItems_ReturnReort(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) As XtraReport
    '    Try
    '        Dim xtraRep As XtraReport = GetReportFromLayOut(e.Item.Tag, False)
    '        Return xtraRep
    '    Catch ex As Exception
    '        BLCommon.InsertExceptionTrackingBL(ex, "", Reflection.MethodInfo.GetCurrentMethod.Name)
    '    End Try
    'End Function
    Private Function SetReportWhereCause(ByVal strReport As String) As String
        Dim strFilter As String = String.Empty
        If Not strReport Is Nothing AndAlso strReport.Length > 0 Then


        End If
        If strReport.Contains(" uqSampleParameterID ") AndAlso Not struqSampleParameterID Is Nothing AndAlso Len(struqSampleParameterID) > 0 Then
            strFilter = "uqSampleParameterID IN (" & struqSampleParameterID & ")"
        End If
        If strReport.Contains(" uqQCBatchID") AndAlso Not struqQCBatchID Is Nothing AndAlso Len(struqQCBatchID) > 0 Then
            If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                strFilter = strFilter & " and " & "uqQCBatchID IN (" & struqQCBatchID & ")"
            Else
                strFilter = "uqQCBatchID IN (" & struqQCBatchID & ")"
            End If
        End If
        If strReport.Contains(" coluqQCBatchID ") AndAlso strReport.Contains("coluqQCTypeID") AndAlso
                Not struqQCBatchID Is Nothing AndAlso Len(struqQCBatchID) > 0 AndAlso Not struqQCTypeID Is Nothing AndAlso Len(struqQCTypeID) > 0 Then
            If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                strFilter = strFilter & " or " & "(coluqQCBatchID IN (" & struqQCBatchID & ") and coluqQCTypeID IN (" & struqQCTypeID & "))"
            Else
                strFilter = "(coluqQCBatchID IN (" & struqQCBatchID & ") and coluqQCTypeID IN (" & struqQCTypeID & "))"
            End If
        End If
        If strReport.Contains(" JobID ") AndAlso Not strJobID Is Nothing AndAlso Len(strJobID) > 0 Then
            If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                strFilter = strFilter & " and " & "JobID IN ('" & strJobID & "')"
            Else
                strFilter = "JobID IN (" & strJobID & ")"
            End If
        End If
        If strReport.Contains(" coluqSamplingSampleID ") AndAlso Not struqSamplingSampleID Is Nothing AndAlso Len(struqSamplingSampleID) > 0 Then
            If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                strFilter = strFilter & " and " & "coluqSamplingSampleID IN (" & struqSamplingSampleID & ")"
            Else
                strFilter = "coluqSamplingSampleID IN (" & struqSamplingSampleID & ")"
            End If
        End If
        If strReport.Contains(" ReportID ") AndAlso Not strReportID Is Nothing AndAlso Len(strReportID) > 0 Then
            If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                strFilter = strFilter & " and " & "ReportID IN ('" & strReportID & "')"
            Else
                strFilter = "ReportID IN ('" & strReportID & "')"
            End If
        End If
        If strReport.Contains("coluqPrepBatchID") AndAlso Not strReportID Is Nothing AndAlso Len(strUqPrepBatchID) > 0 Then
            If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                strFilter = strFilter & " and " & "coluqPrepBatchID IN ('" & strUqPrepBatchID & "')"
            Else
                strFilter = "coluqPrepBatchID IN ('" & strUqPrepBatchID & "')"
            End If
        End If
        If strReport.Contains(" coluqMultiParamEntryCalibrationID ") AndAlso Not struqMultiparamCalibrationID Is Nothing AndAlso Len(struqMultiparamCalibrationID) > 0 Then
            strFilter = "coluqMultiParamEntryCalibrationID IN (" & struqMultiparamCalibrationID & ")"
        End If
        If strReport.Contains(" coluqColormetricCalibrationID ") AndAlso Not struqColormetricCalibrationID Is Nothing AndAlso Len(struqColormetricCalibrationID) > 0 Then
            strFilter = "coluqColormetricCalibrationID IN (" & struqColormetricCalibrationID & ")"
        End If
        If strReport.Contains(" uqSampleTestID ") AndAlso Not struqSampleTestID Is Nothing AndAlso Len(struqSampleTestID) > 0 Then
            strFilter = "uqSampleTestID IN (" & struqSampleTestID & ")"
        End If
        If Not strLabwareID Is Nothing AndAlso Len(strLabwareID) > 0 Then
            If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                strFilter = strFilter & " and " & "LabwareID IN (" & strLabwareID & ")"
            Else
                strFilter = "LabwareID IN (" & strLabwareID & ")"
            End If
        End If
        If Not struqAssetLedgerCheckinID Is Nothing AndAlso Len(struqAssetLedgerCheckinID) > 0 Then
            If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                strFilter = strFilter & " and " & "uqAssetLedgerCheckinID IN (" & struqAssetLedgerCheckinID & ")"
            Else
                strFilter = "uqAssetLedgerCheckinID IN (" & struqAssetLedgerCheckinID & ")"
            End If
        End If
        If Not struqAssetLedgerDistributionID Is Nothing AndAlso Len(struqAssetLedgerDistributionID) > 0 Then
            If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                strFilter = strFilter & " and " & "uqAssetLedgerDistributionID IN (" & struqAssetLedgerDistributionID & ")"
            Else
                strFilter = "uqAssetLedgerDistributionID IN (" & struqAssetLedgerDistributionID & ")"
            End If
        End If
        If Not strLabwareInspectionID Is Nothing AndAlso Len(strLabwareInspectionID) > 0 Then
            If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                strFilter = strFilter & " and " & "uqLabwareInspectionID IN ('" & strLabwareInspectionID & "')"
            Else
                strFilter = "uqLabwareInspectionID IN ('" & strLabwareInspectionID & "')"
            End If
        End If
        If strReport.Contains("Task Sample Transfer Report") Then
            If Not strRegistrationID Is Nothing AndAlso strRegistrationID.Length > 0 Then
                If strSampleTransferMode = "Pending" Then
                    If strFromDate Is Nothing OrElse strToDate Is Nothing Then
                        strFilter = "RegistrationID IN ('" & strRegistrationID & "')"
                    Else
                        strFilter = "RegistrationID IN ('" & strRegistrationID & "')" & " and " & "AssignedDate Between '" & strFromDate & "' and '" & strToDate & "'"
                    End If
                ElseIf strSampleTransferMode = "UnPending" Then
                    If strFromDate Is Nothing OrElse strToDate Is Nothing Then
                        strFilter = "RegistrationID IN ('" & strRegistrationID & "')"
                    Else
                        strFilter = "RegistrationID IN ('" & strRegistrationID & "')" & " and " & "BottleReceivedDate Between '" & strFromDate & "' and '" & strToDate & "'"
                    End If
                End If
            End If
        End If
        If Not strUserName Is Nothing AndAlso Len(strUserName) > 0 Then
            If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                strFilter = strFilter & "and UserName IN (" & strUserName & ")"
            Else
                strFilter = "UserName IN (" & strUserName & ")"
            End If
        End If
        If Not strStorage Is Nothing AndAlso Len(strStorage) > 0 Then
            If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                strFilter = strFilter & "and Storage IN (" & strStorage & ")"
            Else
                strFilter = "Storage IN (" & strStorage & ")"
            End If
        End If
        If Not strlabwarebarcode Is Nothing AndAlso Len(strlabwarebarcode) > 0 Then
            If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                strFilter = strFilter & "and LabwareID IN (" & strlabwarebarcode & ")"
            Else
                strFilter = "LabwareID IN (" & strlabwarebarcode & ")"
            End If
        End If

        If Not strLT Is Nothing AndAlso Len(strLT) > 0 Then
            If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                strFilter = strFilter & "and LT IN (" & strLT & ")"
            Else
                strFilter = "LT IN (" & strLT & ")"
            End If
        End If
        'vignesh
        If strReport.Contains("ABID") AndAlso Not strABID Is Nothing AndAlso Len(strABID) > 0 Then
            If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                strFilter = strFilter & " and " & "ABID IN (" & strABID & ")"
            Else
                strFilter = "ABID IN (" & strABID & ")"
            End If
        End If
        'end
        'quote
        If strReport.Contains(" Oid ") AndAlso Not strQuoteID Is Nothing AndAlso Len(strQuoteID) > 0 Then
            If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                strFilter = strFilter & " and " & "Oid IN ('" & strQuoteID & "')"
            Else
                strFilter = "Oid IN ('" & strQuoteID & "')"
            End If
        End If

        If strReport.Contains(" QuoteID ") AndAlso Not stringQuoteID Is Nothing AndAlso Len(stringQuoteID) > 0 Then
            If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                strFilter = strFilter & " and " & "QuoteID IN ('" & stringQuoteID & "')"
            Else
                strFilter = "QuoteID IN ('" & stringQuoteID & "')"
            End If
        End If
        'end
        'Item
        If strReport.Contains(" Oid ") AndAlso Not strItem Is Nothing AndAlso Len(strItem) > 0 Then
            If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                strFilter = strFilter & " and " & "Oid IN ('" & strItem & "')"
            Else
                strFilter = "Oid IN ('" & strItem & "')"
            End If
        End If
        If strReport.Contains(" SampleID ") AndAlso Not NewObjLDMReportingVariables.strSampleID Is Nothing AndAlso Len(NewObjLDMReportingVariables.strSampleID) > 0 Then
            If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                strFilter = strFilter & " and " & "SampleID IN (" & NewObjLDMReportingVariables.strSampleID & ")"
            Else
                strFilter = "SampleID IN (" & NewObjLDMReportingVariables.strSampleID & ")"
            End If
        End If
        If strReport.Contains(" SuboutOrderID ") AndAlso Not NewObjLDMReportingVariables.strSuboutOrderID Is Nothing AndAlso Len(NewObjLDMReportingVariables.strSuboutOrderID) > 0 Then
            If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                strFilter = strFilter & " and " & "SuboutOrderID IN (" & NewObjLDMReportingVariables.strSuboutOrderID & ")"
            Else
                strFilter = "SuboutOrderID IN (" & NewObjLDMReportingVariables.strSuboutOrderID & ")"
            End If
        End If
        If strReport.Contains(" SCCOid ") AndAlso Not NewObjLDMReportingVariables.SCCOid Is Nothing AndAlso Len(NewObjLDMReportingVariables.SCCOid) > 0 Then
            If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                strFilter = strFilter & " and " & "SCCOid IN (" & NewObjLDMReportingVariables.SCCOid & ")"
            Else
                strFilter = "SCCOid IN (" & NewObjLDMReportingVariables.SCCOid & ")"
            End If
        End If

        'end
        ''Item
        'If Not strItem Is Nothing AndAlso Len(strItem) > 0 Then
        '    If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
        '        strFilter = strFilter & "and Oid IN (" & strItem & ")"
        '    Else
        '        strFilter = "Oid IN (" & strItem & ")"
        '    End If
        'End If
        ''end
        If strReport.Contains(" uqTestMethodID ") AndAlso Not strTestMethodID Is Nothing AndAlso Len(strTestMethodID) > 0 Then
            If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                strFilter = strFilter & " and " & "uqTestMethodID IN (" & strTestMethodID & ")"
            Else
                strFilter = "uqTestMethodID IN (" & strTestMethodID & ")"
            End If
        End If
        If strReport.Contains(" uqParameterID ") AndAlso Not strParameterID Is Nothing AndAlso Len(strParameterID) > 0 Then
            If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                strFilter = strFilter & " and " & "uqParameterID IN (" & strParameterID & ")"
            Else
                strFilter = "uqParameterID IN (" & strParameterID & ")"
            End If
        End If
        If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
            strReport = Replace(strReport, "1=2", strFilter)
        End If
        Return strReport
    End Function
    Private Function SetReportWhereCause(ByVal strReport As String, ByVal ObjReportingVariables As LDMReportingVariables) As String
        Dim strFilter As String = String.Empty
        If Not strReport Is Nothing AndAlso strReport.Length > 0 Then
            If strReport.Contains(" uqSampleParameterID ") AndAlso Not ObjReportingVariables.struqSampleParameterID Is Nothing AndAlso Len(ObjReportingVariables.struqSampleParameterID) > 0 Then
                strFilter = "uqSampleParameterID IN (" & ObjReportingVariables.struqSampleParameterID & ")"
            End If
            If strReport.Contains(" JobID ") AndAlso Not ObjReportingVariables.strJobID Is Nothing AndAlso Len(ObjReportingVariables.strJobID) > 0 Then
                If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                    strFilter = strFilter & " and " & "JobID IN (" & ObjReportingVariables.strJobID & ")"
                Else
                    strFilter = "JobID IN (" & ObjReportingVariables.strJobID & ")"
                End If
            End If
            'If Not ObjReportingVariables.strviewid Is Nothing Then
            '    strviewid = ObjReportingVariables.strviewid
            '    If strviewid Is "SampleParameter_ListView_Copy_CustomReporting" Then
            '        If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
            '            strFilter = strFilter & " and " & "Status In(3)"
            '        Else
            '            strFilter = "Status In(3)"
            '        End If
            '    End If
            'End If
            If strReport.Contains(" SampleID ") AndAlso Not ObjReportingVariables.strSampleID Is Nothing AndAlso Len(ObjReportingVariables.strSampleID) > 0 Then
                If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                    strFilter = strFilter & " and " & "SampleID IN (" & ObjReportingVariables.strSampleID & ")"
                Else
                    strFilter = "SampleID IN (" & ObjReportingVariables.strSampleID & ")"
                End If
            End If
            If strReport.Contains(" ReportID ") AndAlso Not ObjReportingVariables.strReportID Is Nothing AndAlso Len(ObjReportingVariables.strReportID) > 0 Then
                If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                    strFilter = strFilter & " and " & "ReportID IN ('" & ObjReportingVariables.strReportID & "')"
                Else
                    strFilter = "ReportID IN ('" & ObjReportingVariables.strReportID & "')"
                End If
            End If
            If strReport.Contains(" SuboutOrderID ") AndAlso Not ObjReportingVariables.strSuboutOrderID Is Nothing AndAlso Len(ObjReportingVariables.strSuboutOrderID) > 0 Then
                If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                    strFilter = strFilter & " and " & "SuboutOrderID IN (" & ObjReportingVariables.strSuboutOrderID & ")"
                Else
                    strFilter = "SuboutOrderID IN (" & ObjReportingVariables.strSuboutOrderID & ")"
                End If
            End If
            If Not ObjReportingVariables.strlabwarebarcode Is Nothing AndAlso Len(ObjReportingVariables.strlabwarebarcode) > 0 Then
                If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                    strFilter = strFilter & "and LabwareID IN (" & ObjReportingVariables.strlabwarebarcode & ")"
                Else
                    strFilter = "LabwareID IN (" & ObjReportingVariables.strlabwarebarcode & ")"
                End If
            End If

            If Not ObjReportingVariables.strLT Is Nothing AndAlso Len(ObjReportingVariables.strLT) > 0 Then
                If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                    strFilter = strFilter & "and LT IN (" & ObjReportingVariables.strLT & ")"
                Else
                    strFilter = "LT IN (" & ObjReportingVariables.strLT & ")"
                End If
            End If
            If Not ObjReportingVariables.strLTOid Is Nothing AndAlso Len(ObjReportingVariables.strLTOid) > 0 Then
                If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                    strFilter = strFilter & "and Oid IN (" & ObjReportingVariables.strLTOid & ")"
                Else
                    strFilter = "Oid IN (" & ObjReportingVariables.strLTOid & ")"
                End If
            End If
            If Not ObjReportingVariables.strPOID Is Nothing AndAlso Len(ObjReportingVariables.strPOID) > 0 Then
                If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                    strFilter = strFilter & "and POID IN (" & ObjReportingVariables.strPOID & ")"
                Else
                    strFilter = "POID IN (" & ObjReportingVariables.strPOID & ")"
                End If
            End If
            'quote
            If Not ObjReportingVariables.strQuoteID Is Nothing AndAlso Len(ObjReportingVariables.strQuoteID) > 0 Then
                If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                    strFilter = strFilter & "and Oid IN (" & ObjReportingVariables.strQuoteID & ")"   'ObjReportingVariables.strQuoteID
                Else
                    strFilter = "Oid IN (" & ObjReportingVariables.strQuoteID & ")"
                    'ObjReportingVariables.strQuoteID
                End If
            End If
            If Not ObjReportingVariables.stringQuoteID Is Nothing AndAlso Len(ObjReportingVariables.stringQuoteID) > 0 Then
                If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                    strFilter = strFilter & "and QuoteID IN (" & ObjReportingVariables.stringQuoteID & ")"   'ObjReportingVariables.strQuoteID
                Else
                    strFilter = "QuoteID IN (" & ObjReportingVariables.stringQuoteID & ")"
                    'ObjReportingVariables.strQuoteID
                End If
            End If
            If strFilter Is Nothing Then
                If Not ObjReportingVariables.strJobID Is Nothing AndAlso Len(ObjReportingVariables.strJobID) > 0 Then
                    If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                        strFilter = strFilter & "and JobID IN (" & ObjReportingVariables.strJobID & ")"   'ObjReportingVariables.SampleReceipt
                    Else
                        strFilter = "JobID IN (" & ObjReportingVariables.strJobID & ")"
                        'ObjReportingVariables.strQuoteID
                    End If
                End If
            End If
            If strReport.Contains(" SCCOid ") AndAlso Not ObjReportingVariables.SCCOid Is Nothing AndAlso Len(ObjReportingVariables.SCCOid) > 0 Then
                If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                    strFilter = strFilter & " and " & "SuboutOrderID IN (" & ObjReportingVariables.SCCOid & ")"
                Else
                    strFilter = "SCCOid IN (" & ObjReportingVariables.SCCOid & ")"
                End If
            End If
            If strReport.Contains(" BottleOrderOid ") AndAlso Not ObjReportingVariables.BottlesOrderOid Is Nothing AndAlso Len(ObjReportingVariables.BottlesOrderOid) > 0 Then
                If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                    strFilter = strFilter & " and " & "BottleOrderOid IN (" & ObjReportingVariables.BottlesOrderOid & ")"
                Else
                    strFilter = "BottleOrderOid IN (" & ObjReportingVariables.BottlesOrderOid & ")"
                End If
            End If
            If strReport.Contains(" InvoiceID ") AndAlso Not NewObjLDMReportingVariables.strInvoiceID Is Nothing AndAlso Len(NewObjLDMReportingVariables.strInvoiceID) > 0 Then
                If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                    strFilter = strFilter & " and " & "InvoiceID IN (" & NewObjLDMReportingVariables.strInvoiceID & ")"
                Else
                    strFilter = "InvoiceID IN (" & NewObjLDMReportingVariables.strInvoiceID & ")"
                End If
            End If
            'end
            'Item
            If Not ObjReportingVariables.strItem Is Nothing AndAlso Len(ObjReportingVariables.strItem) > 0 Then
                If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                    strFilter = strFilter & "and Oid IN (" & ObjReportingVariables.strItem & ")"
                Else
                    strFilter = "Oid IN (" & ObjReportingVariables.strItem & ")"
                End If
            End If
            'TestMethod
            If strReport.Contains(" uqTestMethodID ") AndAlso Not ObjReportingVariables.strTestMethodID Is Nothing AndAlso Len(ObjReportingVariables.strTestMethodID) > 0 Then
                If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                    strFilter = strFilter & " and " & "uqTestMethodID IN (" & ObjReportingVariables.strTestMethodID & ")"
                Else
                    strFilter = "uqTestMethodID IN (" & ObjReportingVariables.strTestMethodID & ")"
                End If
            End If
            'Parameter
            If strReport.Contains(" uqParameterID ") AndAlso Not ObjReportingVariables.strParameterID Is Nothing AndAlso Len(ObjReportingVariables.strParameterID) > 0 Then
                If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                    strFilter = strFilter & " and " & "uqParameterID IN (" & ObjReportingVariables.strParameterID & ")"
                Else
                    strFilter = "uqParameterID IN (" & ObjReportingVariables.strParameterID & ")"
                End If
            End If
            If strReport.Contains(" DLQCID ") AndAlso Not ObjReportingVariables.strDLQCID Is Nothing AndAlso Len(ObjReportingVariables.strDLQCID) > 0 Then
                If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                    strFilter = strFilter & " and " & "strDLQCID IN (" & ObjReportingVariables.strDLQCID & ")"
                Else
                    strFilter = "DLQCID IN (" & ObjReportingVariables.strDLQCID & ")"
                End If
            End If
            'end
            If Not strFilter Is Nothing AndAlso Len(strFilter) > 0 Then
                strReport = Replace(strReport, "1=2", strFilter)
            End If
        End If
        Return strReport
    End Function

    'Private Function SetReportWhereCause(ByVal strReport As String, ByVal xtraRep As XtraReport) As String
    '    If Not strReport Is Nothing AndAlso strReport.Length > 0 Then
    '        If Len(xtraRep.FilterString) > 0 Then
    '            For intParameter As Integer = 0 To xtraRep.Parameters.Count - 1
    '                If xtraRep.Parameters(intParameter).Name = "uqSampleParameterID" Then
    '                    xtraRep.FilterString = Replace(xtraRep.FilterString, "?" & xtraRep.Parameters(intParameter).Name, struqSampleParameterID)
    '                ElseIf xtraRep.Parameters(intParameter).Name = "uqQCBatchID" Then
    '                    xtraRep.FilterString = Replace(xtraRep.FilterString, "?" & xtraRep.Parameters(intParameter).Name, struqQCBatchID)
    '                ElseIf xtraRep.Parameters(intParameter).Name = "uqQCTypeID" Then
    '                    xtraRep.FilterString = Replace(xtraRep.FilterString, "?" & xtraRep.Parameters(intParameter).Name, struqQCTypeID)
    '                ElseIf xtraRep.Parameters(intParameter).Name = "JobID" Then
    '                    xtraRep.FilterString = Replace(xtraRep.FilterString, "?" & xtraRep.Parameters(intParameter).Name, IIf(Not objeBenchFormInfo Is Nothing, IIf(Not objeBenchFormInfo.JobID Is Nothing AndAlso Len(objeBenchFormInfo.JobID) > 0, "'" & objeBenchFormInfo.JobID & "'", "''"), "''"))
    '                ElseIf xtraRep.Parameters(intParameter).Name = "QCBatchID" Then
    '                    xtraRep.FilterString = Replace(xtraRep.FilterString, "?" & xtraRep.Parameters(intParameter).Name, IIf(Not objeBenchFormInfo Is Nothing, IIf(Not objeBenchFormInfo.QCBatchID Is Nothing AndAlso Len(objeBenchFormInfo.QCBatchID) > 0, "'" & objeBenchFormInfo.QCBatchID & "'", "''"), "''"))
    '                ElseIf xtraRep.Parameters(intParameter).Name = "Test" Then
    '                    xtraRep.FilterString = Replace(xtraRep.FilterString, "?" & xtraRep.Parameters(intParameter).Name, IIf(Not objeBenchFormInfo Is Nothing, IIf(Not objeBenchFormInfo.TestName Is Nothing AndAlso Len(objeBenchFormInfo.TestName) > 0, "'" & objeBenchFormInfo.TestName & "'", "''"), "''"))
    '                ElseIf xtraRep.Parameters(intParameter).Name = "uqTestParameterID" Then
    '                    xtraRep.FilterString = Replace(xtraRep.FilterString, "?" & xtraRep.Parameters(intParameter).Name, IIf(Not objeBenchFormInfo Is Nothing, objeBenchFormInfo.TestParameterID, 0))
    '                ElseIf xtraRep.Parameters(intParameter).Name = "uqSamplingSampleID" Then
    '                    xtraRep.FilterString = Replace(xtraRep.FilterString, "?" & xtraRep.Parameters(intParameter).Name, struqSamplingSampleID)
    '                End If
    '                xtraRep.Parameters(intParameter).Visible = False
    '            Next

    '            strReport = Replace(strReport, "1=2", xtraRep.FilterString)
    '        End If
    '    End If
    '    Return strReport
    'End Function

    'Private Sub SetReportFilters(ByVal xtraRep As XtraReport)
    '    If Not xtraRep Is Nothing Then

    '        xtraRep.RequestParameters = False

    '        If Len(xtraRep.FilterString) > 0 Then
    '            For intParameter As Integer = 0 To xtraRep.Parameters.Count - 1
    '                If xtraRep.Parameters(intParameter).Name = "uqSampleParameterID" Then
    '                    xtraRep.FilterString = Replace(xtraRep.FilterString, "?" & xtraRep.Parameters(intParameter).Name, struqSampleParameterID)
    '                ElseIf xtraRep.Parameters(intParameter).Name = "uqQCBatchID" Then
    '                    xtraRep.FilterString = Replace(xtraRep.FilterString, "?" & xtraRep.Parameters(intParameter).Name, struqQCBatchID)
    '                ElseIf xtraRep.Parameters(intParameter).Name = "uqQCTypeID" Then
    '                    xtraRep.FilterString = Replace(xtraRep.FilterString, "?" & xtraRep.Parameters(intParameter).Name, struqQCTypeID)
    '                ElseIf xtraRep.Parameters(intParameter).Name = "JobID" Then
    '                    xtraRep.FilterString = Replace(xtraRep.FilterString, "?" & xtraRep.Parameters(intParameter).Name, IIf(Not objeBenchFormInfo Is Nothing, IIf(Not objeBenchFormInfo.JobID Is Nothing AndAlso Len(objeBenchFormInfo.JobID) > 0, "'" & objeBenchFormInfo.JobID & "'", "''"), "''"))
    '                ElseIf xtraRep.Parameters(intParameter).Name = "QCBatchID" Then
    '                    xtraRep.FilterString = Replace(xtraRep.FilterString, "?" & xtraRep.Parameters(intParameter).Name, IIf(Not objeBenchFormInfo Is Nothing, IIf(Not objeBenchFormInfo.QCBatchID Is Nothing AndAlso Len(objeBenchFormInfo.QCBatchID) > 0, "'" & objeBenchFormInfo.QCBatchID & "'", "''"), "''"))
    '                ElseIf xtraRep.Parameters(intParameter).Name = "Test" Then
    '                    xtraRep.FilterString = Replace(xtraRep.FilterString, "?" & xtraRep.Parameters(intParameter).Name, IIf(Not objeBenchFormInfo Is Nothing, IIf(Not objeBenchFormInfo.TestName Is Nothing AndAlso Len(objeBenchFormInfo.TestName) > 0, "'" & objeBenchFormInfo.TestName & "'", "''"), "''"))
    '                ElseIf xtraRep.Parameters(intParameter).Name = "uqTestParameterID" Then
    '                    xtraRep.FilterString = Replace(xtraRep.FilterString, "?" & xtraRep.Parameters(intParameter).Name, IIf(Not objeBenchFormInfo Is Nothing, objeBenchFormInfo.TestParameterID, 0))
    '                ElseIf xtraRep.Parameters(intParameter).Name = "uqSamplingSampleID" Then
    '                    xtraRep.FilterString = Replace(xtraRep.FilterString, "?" & xtraRep.Parameters(intParameter).Name, struqSamplingSampleID)
    '                End If
    '                xtraRep.Parameters(intParameter).Visible = False
    '            Next
    '        End If

    '        'xtraRep.PrintingSystem.ExecCommand(DevExpress.XtraPrinting.PrintingSystemCommand.Parameters, New Object() {False})
    '        'xtraRep.PrintingSystem.SetCommandVisibility(DevExpress.XtraPrinting.PrintingSystemCommand.Parameters, DevExpress.XtraPrinting.CommandVisibility.None)
    '        'Dim strFilter() As String = xtraRep.FilterString.Split(" ")
    '        'Dim strFilterText As String = String.Empty
    '        'For i As Integer = 0 To strFilter.Length - 1
    '        '    If strFilter(i).Contains("?") Then
    '        '        Dim strReportParameter As String = String.Empty
    '        '        strReportParameter = Replace(strFilter(i), "?", "")
    '        '        strFilter(i) = strReportParameter.Replace(strReportParameter, "'" & xtraRep.Parameters(strReportParameter).Value & "'")
    '        '    End If
    '        '    strFilterText = strFilterText & " " & strFilter(i)
    '        'Next
    '        'If Len(Trim(strFilterText)) > 0 Then
    '        '    Dim ds As DataSet = xtraRep.DataSource
    '        '    Dim dvparam As DataView = New DataView(ds.Tables(0), strFilterText, "", DataViewRowState.CurrentRows)
    '        '    xtraRep.DataSource = dvparam.ToTable
    '        'End If
    '    End If
    'End Sub

    'Public Function GetReportFilters(ByVal xtraRep As XtraReport) As String
    '    If Not xtraRep Is Nothing Then
    '        If Len(xtraRep.FilterString) > 0 Then
    '            For intParameter As Integer = 0 To xtraRep.Parameters.Count - 1
    '                If xtraRep.Parameters(intParameter).Name = "uqSampleParameterID" Then
    '                    xtraRep.FilterString = Replace(xtraRep.FilterString, "?" & xtraRep.Parameters(intParameter).Name, struqSampleParameterID)
    '                ElseIf xtraRep.Parameters(intParameter).Name = "uqQCBatchID" Then
    '                    xtraRep.FilterString = Replace(xtraRep.FilterString, "?" & xtraRep.Parameters(intParameter).Name, struqQCBatchID)
    '                ElseIf xtraRep.Parameters(intParameter).Name = "uqQCTypeID" Then
    '                    xtraRep.FilterString = Replace(xtraRep.FilterString, "?" & xtraRep.Parameters(intParameter).Name, struqQCTypeID)
    '                ElseIf xtraRep.Parameters(intParameter).Name = "JobID" Then
    '                    xtraRep.FilterString = Replace(xtraRep.FilterString, "?" & xtraRep.Parameters(intParameter).Name, IIf(Not objeBenchFormInfo Is Nothing, IIf(Not objeBenchFormInfo.JobID Is Nothing AndAlso Len(objeBenchFormInfo.JobID) > 0, "'" & objeBenchFormInfo.JobID & "'", "''"), "''"))
    '                ElseIf xtraRep.Parameters(intParameter).Name = "QCBatchID" Then
    '                    xtraRep.FilterString = Replace(xtraRep.FilterString, "?" & xtraRep.Parameters(intParameter).Name, IIf(Not objeBenchFormInfo Is Nothing, IIf(Not objeBenchFormInfo.QCBatchID Is Nothing AndAlso Len(objeBenchFormInfo.QCBatchID) > 0, "'" & objeBenchFormInfo.QCBatchID & "'", "''"), "''"))
    '                ElseIf xtraRep.Parameters(intParameter).Name = "Test" Then
    '                    xtraRep.FilterString = Replace(xtraRep.FilterString, "?" & xtraRep.Parameters(intParameter).Name, IIf(Not objeBenchFormInfo Is Nothing, IIf(Not objeBenchFormInfo.TestName Is Nothing AndAlso Len(objeBenchFormInfo.TestName) > 0, "'" & objeBenchFormInfo.TestName & "'", "''"), "''"))
    '                ElseIf xtraRep.Parameters(intParameter).Name = "uqTestParameterID" Then
    '                    xtraRep.FilterString = Replace(xtraRep.FilterString, "?" & xtraRep.Parameters(intParameter).Name, IIf(Not objeBenchFormInfo Is Nothing, objeBenchFormInfo.TestParameterID, 0))
    '                ElseIf xtraRep.Parameters(intParameter).Name = "uqSamplingSampleID" Then
    '                    xtraRep.FilterString = Replace(xtraRep.FilterString, "?" & xtraRep.Parameters(intParameter).Name, struqSamplingSampleID)
    '                End If
    '            Next
    '            Return xtraRep.FilterString
    '        End If
    '    End If
    '    Return String.Empty
    'End Function


    'This is the Default Function from BTLIMS May be we use this function in future in LDM
    Public Function GetReportFromLayOut(ByVal strReportName As String, Optional ByVal bolOnDesign As Boolean = True) As XtraReport
        Try
            Dim strString As String = "Select colCustomReportDesignerLayOut,colTableSchema,colNormalColumn,colRowColumn,colUniqueColumn,colDataSourceQuery,colVerticalColumnCount from tbl_Public_CustomReportDesignerDetails where colCustomReportDesignerName = N'" & strReportName & "'"
            Dim dtTable As DataTable = BLCommon.GetData(strString)
            If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 AndAlso Not IsDBNull(dtTable.Rows(0)("colCustomReportDesignerLayOut")) AndAlso Len(dtTable.Rows(0)("colCustomReportDesignerLayOut")) > 0 Then
                If Not IsDBNull(dtTable.Rows(0)("colTableSchema")) AndAlso Len(dtTable.Rows(0)("colTableSchema")) > 0 Then
                    strTableColumnSchema = dtTable.Rows(0)("colTableSchema")
                    dtTableColumn = BLCommon.GetData(strTableColumnSchema)
                Else
                    strTableColumnSchema = String.Empty
                End If
                If Not IsDBNull(dtTable.Rows(0)("colNormalColumn")) AndAlso Len(dtTable.Rows(0)("colNormalColumn")) > 0 Then
                    strNormalColumn = dtTable.Rows(0)("colNormalColumn")
                Else
                    strNormalColumn = String.Empty
                End If
                If Not IsDBNull(dtTable.Rows(0)("colRowColumn")) AndAlso Len(dtTable.Rows(0)("colRowColumn")) > 0 Then
                    strRowColumn = dtTable.Rows(0)("colRowColumn")
                Else
                    strRowColumn = String.Empty
                End If
                If Not IsDBNull(dtTable.Rows(0)("colUniqueColumn")) AndAlso Len(dtTable.Rows(0)("colUniqueColumn")) > 0 Then
                    strUniqueColumn = dtTable.Rows(0)("colUniqueColumn")
                Else
                    strUniqueColumn = String.Empty
                End If
                If Not IsDBNull(dtTable.Rows(0)("colDataSourceQuery")) AndAlso Len(dtTable.Rows(0)("colDataSourceQuery")) > 0 Then
                    strDataSourceQuery = dtTable.Rows(0)("colDataSourceQuery")
                Else
                    strDataSourceQuery = String.Empty
                End If
                If Not IsDBNull(dtTable.Rows(0)("colVerticalColumnCount")) AndAlso Len(dtTable.Rows(0)("colVerticalColumnCount")) > 0 Then
                    strColumnCount = dtTable.Rows(0)("colVerticalColumnCount")
                Else
                    strColumnCount = String.Empty
                End If
                Dim strReportLayOut As String = Replace(dtTable.Rows(0)("colCustomReportDesignerLayOut"), "SQLCONNECTIONSTRING", GetSQLConnectionReportUPdate)
                If bolOnDesign = False Then
                    Dim xtrDefaultReport As XtraReport = GetReport(strReportLayOut)
                    If Not xtrDefaultReport Is Nothing AndAlso Len(xtrDefaultReport.FilterString) > 0 Then
                        '  strReportLayOut = SetReportWhereCause(strReportLayOut, xtrDefaultReport)
                    Else
                        strReportLayOut = SetReportWhereCause(strReportLayOut)
                    End If
                End If

                Dim xtraReport As XtraReport = GetReport(strReportLayOut)

                If Not xtraReport.DataAdapter Is Nothing Then
                    Dim myAdapter As System.Data.SqlClient.SqlDataAdapter = xtraReport.DataAdapter
                    myAdapter.SelectCommand.CommandTimeout = 0
                    myAdapter.SelectCommand.Connection.ConnectionString = GetSQLConnection()
                End If


                If bolOnDesign = False Then
                    'SetReportFilters(xtraReport)
                End If

                Return xtraReport
            End If
            Return Nothing
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Function GetReportFromLayOut(ByVal strReportName As String, ByVal ObjLDMReportingVariables As LDMReportingVariables, Optional ByVal bolOnDesign As Boolean = True) As XtraReport
        Try
            Dim watch As Stopwatch = Stopwatch.StartNew()
            watch.Start()
            NewObjLDMReportingVariables = ObjLDMReportingVariables
            Dim strString As String = "Select colCustomReportDesignerLayOut,colTableSchema,colNormalColumn,colRowColumn,colUniqueColumn,colDataSourceQuery,colVerticalColumnCount from tbl_Public_CustomReportDesignerDetails Where colCustomReportDesignerName = N'" & strReportName & "'"
            Dim dtTable As DataTable = BLCommon.GetData(strString)
            If Not dtTable Is Nothing AndAlso dtTable.Rows.Count > 0 AndAlso Not IsDBNull(dtTable.Rows(0)("colCustomReportDesignerLayOut")) AndAlso Len(dtTable.Rows(0)("colCustomReportDesignerLayOut")) > 0 Then
                If Not IsDBNull(dtTable.Rows(0)("colTableSchema")) AndAlso Len(dtTable.Rows(0)("colTableSchema")) > 0 Then
                    strTableColumnSchema = dtTable.Rows(0)("colTableSchema")
                    dtTableColumn = BLCommon.GetData(strTableColumnSchema)
                Else
                    strTableColumnSchema = String.Empty
                End If
                If Not IsDBNull(dtTable.Rows(0)("colNormalColumn")) AndAlso Len(dtTable.Rows(0)("colNormalColumn")) > 0 Then
                    strNormalColumn = dtTable.Rows(0)("colNormalColumn")
                Else
                    strNormalColumn = String.Empty
                End If
                If Not IsDBNull(dtTable.Rows(0)("colRowColumn")) AndAlso Len(dtTable.Rows(0)("colRowColumn")) > 0 Then
                    strRowColumn = dtTable.Rows(0)("colRowColumn")
                Else
                    strRowColumn = String.Empty
                End If
                If Not IsDBNull(dtTable.Rows(0)("colUniqueColumn")) AndAlso Len(dtTable.Rows(0)("colUniqueColumn")) > 0 Then
                    strUniqueColumn = dtTable.Rows(0)("colUniqueColumn")
                Else
                    strUniqueColumn = String.Empty
                End If
                If Not IsDBNull(dtTable.Rows(0)("colDataSourceQuery")) AndAlso Len(dtTable.Rows(0)("colDataSourceQuery")) > 0 Then
                    strDataSourceQuery = dtTable.Rows(0)("colDataSourceQuery")
                Else
                    strDataSourceQuery = String.Empty
                End If
                If Not IsDBNull(dtTable.Rows(0)("colVerticalColumnCount")) AndAlso Len(dtTable.Rows(0)("colVerticalColumnCount")) > 0 Then
                    strColumnCount = dtTable.Rows(0)("colVerticalColumnCount")
                Else
                    strColumnCount = String.Empty
                End If

                'Dim strReportLayOut As String = Replace(dtTable.Rows(0)("colCustomReportDesignerLayOut"), "SQLCONNECTIONSTRING", GetSQLConnectionReportUPdate)
                ''DevExpress.Security.Resources.AccessSettings.ReportingSpecificResources.SetRules(DevExpress.XtraReports.Security.SerializationFormatRule.Allow(DevExpress.XtraReports.UI.SerializationFormat.Code, DevExpress.XtraReports.UI.SerializationFormat.Xml))
                'Dim xtrDefaultReport As XtraReport = GetReport(strReportLayOut)
                'If Not xtrDefaultReport.DataAdapter Is Nothing Then
                '    Dim myAdapter As System.Data.SqlClient.SqlDataAdapter = xtrDefaultReport.DataAdapter
                '    myAdapter.SelectCommand.CommandTimeout = 0
                '    myAdapter.SelectCommand.Connection.ConnectionString = GetSQLConnection()
                '    If bolOnDesign = False Then
                '        If Not xtrDefaultReport Is Nothing AndAlso Len(xtrDefaultReport.FilterString) > 0 Then
                '            '  strReportLayOut = SetReportWhereCause(strReportLayOut, xtrDefaultReport)
                '        Else
                '            strReportLayOut = SetReportWhereCause(strReportLayOut, ObjLDMReportingVariables)
                '        End If
                '    End If
                '    'Dim xtraReport As XtraReport = GetReport(strReportLayOut)
                'End If
                ''If bolOnDesign = False Then
                ''    'SetReportFilters(xtraReport)
                ''End If
                'Return xtrDefaultReport

                Dim strReportLayOut As String = Replace(dtTable.Rows(0)("colCustomReportDesignerLayOut"), "SQLCONNECTIONSTRING", GetSQLConnectionReportUPdate)
                'If bolOnDesign = False Then
                '    Dim xtrDefaultReport As XtraReport = GetReport(strReportLayOut)
                '    If Not xtrDefaultReport Is Nothing AndAlso Len(xtrDefaultReport.FilterString) > 0 Then
                '        '  strReportLayOut = SetReportWhereCause(strReportLayOut, xtrDefaultReport)
                '    Else
                '        strReportLayOut = SetReportWhereCause(strReportLayOut, ObjLDMReportingVariables)
                '    End If
                'End If
                strReportLayOut = SetReportWhereCause(strReportLayOut, ObjLDMReportingVariables)
                Dim watch2 As Stopwatch = Stopwatch.StartNew()
                watch2.Start()
                Dim xtraReport As XtraReport = GetReport(strReportLayOut)
                watch2.Stop()
                Dim elapsedTime = watch2.Elapsed
                If Not xtraReport.DataAdapter Is Nothing Then
                    Dim myAdapter As System.Data.SqlClient.SqlDataAdapter = xtraReport.DataAdapter
                    myAdapter.SelectCommand.CommandTimeout = 0
                    myAdapter.SelectCommand.Connection.ConnectionString = GetSQLConnection()
                End If


                'If bolOnDesign = False Then
                '    'SetReportFilters(xtraReport)
                'End If

                watch.Stop()
                Dim timeelapsed As String = watch.Elapsed.ToString()
                Return xtraReport
            End If
            Return Nothing
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Property dsDataSource() As DataSet
        Get
            Return _dsDataSource
        End Get
        Set(ByVal value As DataSet)
            _dsDataSource = value
        End Set
    End Property
    Public Property dsQCDataSource() As DataSet
        Get
            Return _dsQCDataSource
        End Get
        Set(ByVal value As DataSet)
            _dsQCDataSource = value
        End Set
    End Property

    Private Function GetReport(ByVal layout As String) As XtraReport
        Dim ms As New MemoryStream()
        Dim sw As New StreamWriter(ms, Encoding.UTF8)
        sw.Write(layout.ToCharArray())
        sw.Flush()
        ms.Seek(0, SeekOrigin.Begin)
        Return XtraReport.FromStream(ms, True)
    End Function
    Public Sub ReportOptionsSetPageInfo(ByVal xrReport As XtraReport, ByVal Visibility As Boolean)
        Try
            If Not xrReport Is Nothing Then
                With xrReport.Bands(BandKind.PageFooter)
                    For Each objCtrl As Object In .Controls
                        If TypeOf objCtrl Is XRPageInfo Then
                            Dim xrPageInfo As XRPageInfo = CType(objCtrl, XRPageInfo)
                            xrPageInfo.Visible = Visibility
                        End If
                    Next
                End With
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Function AssignRunTimeDatasource(ByVal xtraRep As XtraReport, ByVal strReportName As String, ByVal dtTextDatasource As DataTable, ByVal dtImageDatasource As DataTable) As XtraReport
        If (Not dtImageDatasource Is Nothing AndAlso dtImageDatasource.Rows.Count > 0) Or (Not dtTextDatasource Is Nothing AndAlso dtTextDatasource.Rows.Count > 0) Then
            For Each band As DevExpress.XtraReports.UI.Band In xtraRep.Bands
                For Each control As DevExpress.XtraReports.UI.XRControl In band
                    If control.GetType() Is GetType(DevExpress.XtraReports.UI.XRTable) Then
                        Dim table As DevExpress.XtraReports.UI.XRTable = DirectCast(control, DevExpress.XtraReports.UI.XRTable)
                        For Each row As DevExpress.XtraReports.UI.XRTableRow In table
                            For Each cell As DevExpress.XtraReports.UI.XRTableCell In row
                                If cell.Tag = "RunTime" Then
                                    If Not dtTextDatasource Is Nothing AndAlso dtTextDatasource.Rows.Count > 0 Then
                                        Dim drrRow() As DataRow = dtTextDatasource.Select("ReportDesignerName = '" & strReportName & "' and ControlName = '" & cell.Name & "'")
                                        If Not drrRow Is Nothing AndAlso drrRow.Length > 0 Then
                                            If Not IsDBNull(drrRow(0)("CustomText")) Then
                                                cell.Text = drrRow(0)("CustomText")
                                            End If
                                        End If
                                    End If
                                Else
                                    For Each control2 As DevExpress.XtraReports.UI.XRControl In cell
                                        If control2.GetType() Is GetType(DevExpress.XtraReports.UI.XRLabel) Or control.GetType() Is GetType(DevExpress.XtraReports.UI.XRRichText) Then
                                            If control2.Tag = "RunTime" Then
                                                If Not dtTextDatasource Is Nothing AndAlso dtTextDatasource.Rows.Count > 0 Then
                                                    Dim drrRow() As DataRow = dtTextDatasource.Select("ReportDesignerName = '" & strReportName & "' and ControlName = '" & control2.Name & "'")
                                                    If Not drrRow Is Nothing AndAlso drrRow.Length > 0 Then
                                                        If Not IsDBNull(drrRow(0)("CustomText")) Then
                                                            control2.Text = drrRow(0)("CustomText")
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                        If control2.GetType() Is GetType(DevExpress.XtraReports.UI.XRPictureBox) Then
                                            If control2.Tag = "RunTime" Then
                                                If Not dtImageDatasource Is Nothing AndAlso dtImageDatasource.Rows.Count > 0 Then
                                                    Dim drrRow() As DataRow = dtImageDatasource.Select("ReportDesignerName = '" & strReportName & "' and ControlName = '" & control2.Name & "'")
                                                    If Not drrRow Is Nothing AndAlso drrRow.Length > 0 Then
                                                        If Not IsDBNull(drrRow(0)("CustomImageSourcePath")) Then
                                                            Dim pic As DevExpress.XtraReports.UI.XRPictureBox = control2
                                                            pic.ImageUrl = drrRow(0)("CustomImageSourcePath")
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    Next
                                End If
                            Next
                        Next
                    ElseIf control.GetType() Is GetType(DevExpress.XtraReports.UI.XRLabel) Or control.GetType() Is GetType(DevExpress.XtraReports.UI.XRRichText) Then
                        If control.Tag = "RunTime" Then
                            If Not dtTextDatasource Is Nothing AndAlso dtTextDatasource.Rows.Count > 0 Then
                                Dim drrRow() As DataRow = dtTextDatasource.Select("ReportDesignerName = '" & strReportName & "' and ControlName = '" & control.Name & "'")
                                If Not drrRow Is Nothing AndAlso drrRow.Length > 0 Then
                                    If Not IsDBNull(drrRow(0)("CustomText")) Then
                                        If Not IsDBNull(drrRow(0)("ControlName")) AndAlso Len(drrRow(0)("ControlName")) > 0 AndAlso drrRow(0)("ControlName") = "ReportSequenceID" Then
                                            strSequenceID = drrRow(0)("CustomText")
                                        End If
                                        control.Text = drrRow(0)("CustomText")
                                    End If
                                End If
                            End If
                        End If
                    ElseIf control.GetType() Is GetType(DevExpress.XtraReports.UI.XRPictureBox) Then
                        If control.Tag = "RunTime" Then
                            If Not dtImageDatasource Is Nothing AndAlso dtImageDatasource.Rows.Count > 0 Then
                                Dim drrRow() As DataRow = dtImageDatasource.Select("ReportDesignerName = '" & strReportName & "' and ControlName = '" & control.Name & "'")
                                If Not drrRow Is Nothing AndAlso drrRow.Length > 0 Then
                                    If Not IsDBNull(drrRow(0)("CustomImageSourcePath")) Then
                                        Dim pic As DevExpress.XtraReports.UI.XRPictureBox = control
                                        pic.ImageUrl = drrRow(0)("CustomImageSourcePath")
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next
            Next
        End If
        Return xtraRep
    End Function
    Public Function AssignLimsDatasource(ByVal xtraRep As XtraReport) As XtraReport
        Dim objReport As New ReportSettingsInfo
        For Each band As DevExpress.XtraReports.UI.Band In xtraRep.Bands
            For Each control As DevExpress.XtraReports.UI.XRControl In band
                If control.GetType() Is GetType(DevExpress.XtraReports.UI.XRTable) Then
                    Dim table As DevExpress.XtraReports.UI.XRTable = DirectCast(control, DevExpress.XtraReports.UI.XRTable)
                    For Each row As DevExpress.XtraReports.UI.XRTableRow In table
                        For Each cell As DevExpress.XtraReports.UI.XRTableCell In row
                            If cell.Tag = "ReportedDate" Then
                                cell.Text = strLimsReportedDate
                            ElseIf cell.Tag = "ReportedBy" Then
                                cell.Text = strLimsReportedBy
                            ElseIf cell.Tag = "SignOffDate" Then
                                cell.Text = strLimsSignOffDate
                            ElseIf cell.Tag = "SignOffBy" Then
                                cell.Text = strLimsSignOffBy
                            ElseIf cell.Tag = "ProjectID" Then
                                If Not strSequenceID Is Nothing AndAlso Len(strSequenceID) > 0 Then
                                    cell.Text = strProjectID & strSequenceID
                                Else
                                    cell.Text = strProjectID
                                End If
                            ElseIf cell.Tag = "AnalyzedDate" Then
                                cell.Text = strAnalyzedDate
                            ElseIf cell.Tag = "AnalyzedBy" Then
                                cell.Text = strAnalyzedBy
                            ElseIf cell.Tag = "ReviewedDate" Then
                                cell.Text = strReviewedDate
                            ElseIf cell.Tag = "ReviewedBy" Then
                                cell.Text = strReviewedBy
                            ElseIf cell.Tag = "VerifiedDate" Then
                                cell.Text = strVerifiedDate
                            ElseIf cell.Tag = "VerifiedBy" Then
                                cell.Text = strVerifiedBy
                            ElseIf cell.Tag = "ReportID" Then
                                cell.Text = strReportID
                            End If
                        Next
                    Next
                ElseIf control.GetType() Is GetType(DevExpress.XtraReports.UI.XRLabel) Or control.GetType() Is GetType(DevExpress.XtraReports.UI.XRRichText) Then
                    If control.Tag = "ReportedDate" Then
                        control.Text = strLimsReportedDate
                    ElseIf control.Tag = "ReportedBy" Then
                        control.Text = strLimsReportedBy
                    ElseIf control.Tag = "SignOffDate" Then
                        control.Text = strLimsSignOffDate
                    ElseIf control.Tag = "SignOffBy" Then
                        control.Text = strLimsSignOffBy
                    ElseIf control.Tag = "ProjectID" Then
                        If Not strSequenceID Is Nothing AndAlso Len(strSequenceID) > 0 Then
                            control.Text = strProjectID & strSequenceID
                        Else
                            control.Text = strProjectID
                        End If
                    ElseIf control.Tag = "PageNo" Then
                        control.Text = strPageNo
                    ElseIf control.Tag = "AnalyzedDate" Then
                        control.Text = strAnalyzedDate
                    ElseIf control.Tag = "AnalyzedBy" Then
                        control.Text = strAnalyzedBy
                    ElseIf control.Tag = "ReviewedDate" Then
                        control.Text = strReviewedDate
                    ElseIf control.Tag = "ReviewedBy" Then
                        control.Text = strReviewedBy
                    ElseIf control.Tag = "VerifiedDate" Then
                        control.Text = strVerifiedDate
                    ElseIf control.Tag = "VerifiedBy" Then
                        control.Text = strVerifiedBy
                    ElseIf control.Tag = "ReportID" Then
                        control.Text = strReportID
                    End If
                ElseIf control.GetType() Is GetType(DevExpress.XtraReports.UI.XRPictureBox) Then

                End If
            Next
        Next
        Return xtraRep
    End Function

    Public Function AssignLimsDatasource(ByVal xtraRep As XtraReport, ByVal ObjLDMReportVariables As LDMReportingVariables) As XtraReport
        Dim objReport As New ReportSettingsInfo
        If xtraRep IsNot Nothing Then
            For Each band As DevExpress.XtraReports.UI.Band In xtraRep.Bands
                For Each control As DevExpress.XtraReports.UI.XRControl In band
                    If control.GetType() Is GetType(DevExpress.XtraReports.UI.XRTable) Then
                        Dim table As DevExpress.XtraReports.UI.XRTable = DirectCast(control, DevExpress.XtraReports.UI.XRTable)
                        For Each row As DevExpress.XtraReports.UI.XRTableRow In table
                            For Each cell As DevExpress.XtraReports.UI.XRTableCell In row
                                If cell.Tag = "ReportedDate" Then
                                    cell.Text = ObjLDMReportVariables.strLimsReportedDate
                                    'ElseIf cell.Tag = "ReportedBy" Then
                                    '    cell.Text = strLimsReportedBy
                                    'ElseIf cell.Tag = "SignOffDate" Then
                                    '    cell.Text = strLimsSignOffDate
                                    'ElseIf cell.Tag = "SignOffBy" Then
                                    '    cell.Text = strLimsSignOffBy
                                    'ElseIf cell.Tag = "ProjectID" Then
                                    '    If Not strSequenceID Is Nothing AndAlso Len(strSequenceID) > 0 Then
                                    '        cell.Text = strProjectID & strSequenceID
                                    '    Else
                                    '        cell.Text = strProjectID
                                    '    End If
                                    'ElseIf cell.Tag = "AnalyzedDate" Then
                                    '    cell.Text = strAnalyzedDate
                                    'ElseIf cell.Tag = "AnalyzedBy" Then
                                    '    cell.Text = strAnalyzedBy
                                    'ElseIf cell.Tag = "ReviewedDate" Then
                                    '    cell.Text = strReviewedDate
                                    'ElseIf cell.Tag = "ReviewedBy" Then
                                    '    cell.Text = strReviewedBy
                                    'ElseIf cell.Tag = "VerifiedDate" Then
                                    '    cell.Text = strVerifiedDate
                                    'ElseIf cell.Tag = "VerifiedBy" Then
                                    '    cell.Text = strVerifiedBy
                                ElseIf cell.Tag = "ReportID" Then
                                    cell.Text = ObjLDMReportVariables.strReportID
                                End If
                            Next
                        Next
                    ElseIf control.GetType() Is GetType(DevExpress.XtraReports.UI.XRLabel) Or control.GetType() Is GetType(DevExpress.XtraReports.UI.XRRichText) Then
                        If control.Tag = "ReportedDate" Then
                            control.Text = ObjLDMReportVariables.strLimsReportedDate
                            'ElseIf control.Tag = "ReportedBy" Then
                            '    control.Text = strLimsReportedBy
                            'ElseIf control.Tag = "SignOffDate" Then
                            '    control.Text = strLimsSignOffDate
                            'ElseIf control.Tag = "SignOffBy" Then
                            '    control.Text = strLimsSignOffBy
                            'ElseIf control.Tag = "ProjectID" Then
                            '    If Not strSequenceID Is Nothing AndAlso Len(strSequenceID) > 0 Then
                            '        control.Text = strProjectID & strSequenceID
                            '    Else
                            '        control.Text = strProjectID
                            '    End If
                            'ElseIf control.Tag = "PageNo" Then
                            '    control.Text = strPageNo
                            'ElseIf control.Tag = "AnalyzedDate" Then
                            '    control.Text = strAnalyzedDate
                            'ElseIf control.Tag = "AnalyzedBy" Then
                            '    control.Text = strAnalyzedBy
                            'ElseIf control.Tag = "ReviewedDate" Then
                            '    control.Text = strReviewedDate
                            'ElseIf control.Tag = "ReviewedBy" Then
                            '    control.Text = strReviewedBy
                            'ElseIf control.Tag = "VerifiedDate" Then
                            '    control.Text = strVerifiedDate
                            'ElseIf control.Tag = "VerifiedBy" Then
                            '    control.Text = strVerifiedBy
                        ElseIf control.Tag = "ReportID" Then
                            control.Text = ObjLDMReportVariables.strReportID
                        End If
                    ElseIf control.GetType() Is GetType(DevExpress.XtraReports.UI.XRPictureBox) Then

                    End If
                Next
            Next
        End If
        Return xtraRep
    End Function

    Public Sub CollectuqTestParameterID(ByVal drRow As DataRow, ByVal strColumnName As String, Optional ByVal bolSelect As Boolean = True)
        If Not drRow Is Nothing AndAlso Not IsDBNull(drRow(strColumnName)) AndAlso Len(drRow(strColumnName)) > 0 Then
            If bolSelect = True Then
                If Not struqTestParameterID Is Nothing AndAlso Len(struqTestParameterID) > 0 Then
                    Dim strTestParameterID() As String = struqTestParameterID.Split(",")
                    Dim arruqTestParameterID As ArrayList = New ArrayList(strTestParameterID)
                    If arruqTestParameterID.IndexOf(drRow(strColumnName).ToString) > -1 Then
                    Else
                        struqTestParameterID = struqTestParameterID & "," & drRow(strColumnName)
                    End If
                Else
                    struqTestParameterID = drRow(strColumnName)
                End If

            Else
                If Not struqTestParameterID Is Nothing AndAlso Len(struqTestParameterID) > 0 Then
                    Dim strTestParameterID() As String = struqSampleParameterID.Split(",")
                    Dim arruqTestParameterID As ArrayList = New ArrayList(strTestParameterID)
                    If arruqTestParameterID.IndexOf(drRow(strColumnName).ToString) > -1 Then
                        arruqTestParameterID.RemoveAt(arruqTestParameterID.IndexOf(drRow(strColumnName).ToString))
                        struqTestParameterID = String.Join(",", arruqTestParameterID.ToArray)
                    End If

                    '    If Not struqTestParameterID Is Nothing AndAlso Len(struqTestParameterID) > 0 Then
                    '        If struqTestParameterID.Contains("," & drRow(strColmenName) & ",") = True OrElse struqTestParameterID.Contains(drRow(strColmenName) & ",") = True OrElse struqTestParameterID.Contains("," & drRow(strColmenName)) = True OrElse struqTestParameterID.Contains(drRow(strColmenName)) = True Then
                    '        Else
                    '            struqTestParameterID = struqTestParameterID & "," & drRow(strColmenName)
                    '        End If
                    '    Else
                    '        struqTestParameterID = drRow(strColmenName)
                    '    End If
                    'Else
                    '    If Not struqTestParameterID Is Nothing AndAlso Len(struqTestParameterID) > 0 Then
                    '        If struqTestParameterID.Contains("," & drRow(strColmenName) & ",") = True Then
                    '            struqTestParameterID = Replace(struqTestParameterID, "," & drRow(strColmenName) & ",", ",")
                    '        ElseIf struqTestParameterID.Contains(drRow(strColmenName) & ",") = True Then
                    '            struqTestParameterID = Replace(struqTestParameterID, drRow(strColmenName) & ",", "")
                    '        ElseIf struqTestParameterID.Contains("," & drRow(strColmenName)) = True Then
                    '            struqTestParameterID = Replace(struqTestParameterID, "," & drRow(strColmenName), "")
                    '        ElseIf struqTestParameterID.Contains(drRow(strColmenName)) = True Then
                    '            struqTestParameterID = Replace(struqTestParameterID, drRow(strColmenName), "")
                    '        End If
                End If
            End If
        End If
    End Sub
    Public Sub CollectuqTestParameterID(ByVal dvView As DataView, ByVal strColumnName As String, Optional ByVal bolSelect As Boolean = True)
        Dim dtTestParameter As DataTable = dvView.ToTable
        struqTestParameterID = String.Empty
        Dim objConvert As New BTDataTableConverter
        If Not dvView Is Nothing AndAlso dvView.Count > 0 Then
            struqTestParameterID = objConvert.DataTableToString(dtTestParameter, strColumnName, bolSelect & " and " & strColumnName & " > 0")
        End If
    End Sub
    Public Sub CollectSystemID(ByVal drRow As DataRow, ByVal strColumnName As String, ByVal strQCColumnName As String, Optional ByVal bolSelect As Boolean = True, Optional ByVal FormName As String = "E-benchForm")
        If Not drRow Is Nothing AndAlso Not IsDBNull(drRow(strColumnName)) AndAlso Len(drRow(strColumnName)) > 0 Then
            If FormName <> "E-benchForm" Then
                If bolSelect = True Then
                    If Not SystemID Is Nothing AndAlso Len(SystemID) > 0 Then
                        Dim strSystemmID() As String = SystemID.Split(",")
                        Dim arrSystemID As ArrayList = New ArrayList(strSystemmID)
                        If arrSystemID.IndexOf("N'" & drRow(strColumnName) & "'".ToString) > -1 Then
                        Else
                            SystemID = SystemID & ",N'" & drRow(strColumnName) & "'"
                        End If
                    Else
                        SystemID = "N'" & drRow(strColumnName) & "'"
                    End If
                Else
                    If Not SystemID Is Nothing AndAlso Len(SystemID) > 0 Then
                        Dim strSystemmID() As String = SystemID.Split(",")
                        Dim arrSystemID As ArrayList = New ArrayList(strSystemmID)
                        If arrSystemID.IndexOf("N'" & drRow(strColumnName) & "'".ToString) > -1 Then
                            arrSystemID.RemoveAt(arrSystemID.IndexOf("N'" & drRow(strColumnName) & "'".ToString))
                            SystemID = String.Join(",", arrSystemID.ToArray)
                        End If
                    End If
                End If
            ElseIf FormName = "E-benchForm" Then
                If bolSelect = True Then
                    If Not SystemID Is Nothing AndAlso Len(SystemID) > 0 Then
                        Dim strSystemmID() As String = SystemID.Split(",")
                        Dim arrSystemID As ArrayList = New ArrayList(strSystemmID)
                        If arrSystemID.IndexOf("N'" & drRow(strColumnName) & "'".ToString & "|" & drRow(strQCColumnName)) > -1 Then
                        Else
                            SystemID = SystemID & ",N'" & drRow(strColumnName) & "'" & "|" & drRow(strQCColumnName)
                        End If
                    Else
                        SystemID = "N'" & drRow(strColumnName) & "'" & "|" & drRow(strQCColumnName)
                    End If
                Else
                    If Not SystemID Is Nothing AndAlso Len(SystemID) > 0 Then
                        Dim strSystemmID() As String = SystemID.Split(",")
                        Dim arrSystemID As ArrayList = New ArrayList(strSystemmID)
                        If arrSystemID.IndexOf("N'" & drRow(strColumnName) & "'".ToString & "|" & drRow(strQCColumnName)) > -1 Then
                            arrSystemID.RemoveAt(arrSystemID.IndexOf("N'" & drRow(strColumnName) & "'".ToString & "|" & drRow(strQCColumnName)))
                            SystemID = String.Join(",", arrSystemID.ToArray)
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Public Sub CollectSystemID(ByVal dvview As DataView, ByVal strColumnName As String, ByVal strQCColumnName As String, Optional ByVal bolSelect As Boolean = True, Optional ByVal FormName As String = "E-benchForm")
        If Not dvview Is Nothing AndAlso dvview.Count > 0 Then
            Dim dtSystemID As DataTable = dvview.ToTable
            For Each drrow As DataRow In dtSystemID.Rows
                If Not drrow Is Nothing AndAlso Not IsDBNull(drrow(strColumnName)) AndAlso Len(drrow(strColumnName)) > 0 Then
                    If FormName <> "E-benchForm" Then
                        If bolSelect = True Then
                            If Not SystemID Is Nothing AndAlso Len(SystemID) > 0 Then
                                Dim strSystemmID() As String = SystemID.Split(",")
                                Dim arrSystemID As ArrayList = New ArrayList(strSystemmID)
                                If arrSystemID.IndexOf("N'" & drrow(strColumnName) & "'".ToString) > -1 Then
                                Else
                                    SystemID = SystemID & ",N'" & drrow(strColumnName) & "'"
                                End If
                            Else
                                SystemID = "N'" & drrow(strColumnName) & "'"
                            End If
                        Else
                            If Not SystemID Is Nothing AndAlso Len(SystemID) > 0 Then
                                Dim strSystemmID() As String = SystemID.Split(",")
                                Dim arrSystemID As ArrayList = New ArrayList(strSystemmID)
                                If arrSystemID.IndexOf("N'" & drrow(strColumnName) & "'".ToString) > -1 Then
                                    arrSystemID.RemoveAt(arrSystemID.IndexOf("N'" & drrow(strColumnName) & "'".ToString))
                                    SystemID = String.Join(",", arrSystemID.ToArray)
                                End If
                            End If
                        End If
                    ElseIf FormName = "E-benchForm" Then
                        If bolSelect = True Then
                            If Not SystemID Is Nothing AndAlso Len(SystemID) > 0 Then
                                Dim strSystemmID() As String = SystemID.Split(",")
                                Dim arrSystemID As ArrayList = New ArrayList(strSystemmID)
                                If arrSystemID.IndexOf("N'" & drrow(strColumnName) & "'".ToString & "|" & drrow(strQCColumnName)) > -1 Then
                                Else
                                    SystemID = SystemID & ",N'" & drrow(strColumnName) & "'" & "|" & drrow(strQCColumnName)
                                End If
                            Else
                                SystemID = "N'" & drrow(strColumnName) & "'" & "|" & drrow(strQCColumnName)
                            End If
                        Else
                            If Not SystemID Is Nothing AndAlso Len(SystemID) > 0 Then
                                Dim strSystemmID() As String = SystemID.Split(",")
                                Dim arrSystemID As ArrayList = New ArrayList(strSystemmID)
                                If arrSystemID.IndexOf("N'" & drrow(strColumnName) & "'".ToString & "|" & drrow(strQCColumnName)) > -1 Then
                                    arrSystemID.RemoveAt(arrSystemID.IndexOf("N'" & drrow(strColumnName) & "'".ToString & "|" & drrow(strQCColumnName)))
                                    SystemID = String.Join(",", arrSystemID.ToArray)
                                End If
                            End If
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    Public Sub CollectLT(ByVal drRow As DataRow, ByVal strColumnName As String, Optional ByVal bolSelect As Boolean = True)
        If Not drRow Is Nothing AndAlso Not IsDBNull(drRow(strColumnName)) AndAlso Len(drRow(strColumnName)) > 0 Then
            If bolSelect = True Then
                If Not strLT Is Nothing AndAlso Len(strLT) > 0 Then
                    Dim strSplitLT() As String = strLT.Split(",")
                    Dim arrLT As ArrayList = New ArrayList(strSplitLT)
                    If arrLT.IndexOf("N'" & drRow(strColumnName) & "'".ToString) > -1 Then
                    Else
                        strLT = strLT & ",N'" & drRow(strColumnName) & "'"
                    End If
                Else
                    strLT = "N'" & drRow(strColumnName) & "'"
                End If

            Else
                If Not strLT Is Nothing AndAlso Len(strLT) > 0 Then
                    Dim strSplitLT() As String = strLT.Split(",")
                    Dim arrLT As ArrayList = New ArrayList(strSplitLT)
                    If arrLT.IndexOf("N'" & drRow(strColumnName) & "'".ToString) > -1 Then
                        arrLT.RemoveAt(arrLT.IndexOf("N'" & drRow(strColumnName) & "'".ToString))
                        strLT = String.Join(",", arrLT.ToArray)
                    End If
                End If
            End If
        End If
    End Sub
    Public Sub CollectLabware(ByVal drRow As DataRow, ByVal strColumnName As String, Optional ByVal bolSelect As Boolean = True)
        If Not drRow Is Nothing AndAlso Not IsDBNull(drRow(strColumnName)) AndAlso Len(drRow(strColumnName)) > 0 Then
            If bolSelect = True Then
                If Not strlabwarebarcode Is Nothing AndAlso Len(strlabwarebarcode) > 0 Then
                    Dim strSplitLT() As String = strlabwarebarcode.Split(",")
                    Dim arrLT As ArrayList = New ArrayList(strSplitLT)
                    If arrLT.IndexOf("N'" & drRow(strColumnName) & "'".ToString) > -1 Then
                    Else
                        strlabwarebarcode = strlabwarebarcode & ",N'" & drRow(strColumnName) & "'"
                    End If
                Else
                    strlabwarebarcode = "N'" & drRow(strColumnName) & "'"
                End If

            Else
                If Not strlabwarebarcode Is Nothing AndAlso Len(strlabwarebarcode) > 0 Then
                    Dim strSplitLT() As String = strlabwarebarcode.Split(",")
                    Dim arrLT As ArrayList = New ArrayList(strSplitLT)
                    If arrLT.IndexOf("N'" & drRow(strColumnName) & "'".ToString) > -1 Then
                        arrLT.RemoveAt(arrLT.IndexOf("N'" & drRow(strColumnName) & "'".ToString))
                        strlabwarebarcode = String.Join(",", arrLT.ToArray)
                    End If
                End If
            End If
        End If
    End Sub

    Public Sub CollectUserName(ByVal drRow As DataRow, ByVal strColumnName As String, Optional ByVal bolSelect As Boolean = True)
        If Not drRow Is Nothing AndAlso Not IsDBNull(drRow(strColumnName)) AndAlso Len(drRow(strColumnName)) > 0 Then
            If bolSelect = True Then
                If Not strUserName Is Nothing AndAlso Len(strUserName) > 0 Then
                    Dim strSplitUserName() As String = strUserName.Split(",")
                    Dim arrLT As ArrayList = New ArrayList(strSplitUserName)
                    If arrLT.IndexOf("N'" & drRow(strColumnName) & "'".ToString) > -1 Then
                    Else
                        strUserName = strUserName & ",N'" & drRow(strColumnName) & "'"
                    End If
                Else
                    strUserName = "N'" & drRow(strColumnName) & "'"
                End If

            Else
                If Not strUserName Is Nothing AndAlso Len(strUserName) > 0 Then
                    Dim strSplitUserName() As String = strUserName.Split(",")
                    Dim arrLT As ArrayList = New ArrayList(strSplitUserName)
                    If arrLT.IndexOf("N'" & drRow(strColumnName) & "'".ToString) > -1 Then
                        arrLT.RemoveAt(arrLT.IndexOf("N'" & drRow(strColumnName) & "'".ToString))
                        strUserName = String.Join(",", arrLT.ToArray)
                    End If
                End If
            End If
        End If
    End Sub

    Public Sub CollectStorage(ByVal drRow As DataRow, ByVal strColumnName As String, Optional ByVal bolSelect As Boolean = True)
        If Not drRow Is Nothing AndAlso Not IsDBNull(drRow(strColumnName)) AndAlso Len(drRow(strColumnName)) > 0 Then
            If bolSelect = True Then
                If Not strStorage Is Nothing AndAlso Len(strStorage) > 0 Then
                    Dim strSplitStorage() As String = strStorage.Split(",")
                    Dim arrStorage As ArrayList = New ArrayList(strSplitStorage)
                    If arrStorage.IndexOf("N'" & drRow(strColumnName) & "'".ToString) > -1 Then
                    Else
                        strStorage = strStorage & ",N'" & drRow(strColumnName) & "'"
                    End If
                Else
                    strStorage = "N'" & drRow(strColumnName) & "'"
                End If

            Else
                If Not strStorage Is Nothing AndAlso Len(strStorage) > 0 Then
                    Dim strSplitStorage() As String = strStorage.Split(",")
                    Dim arrStorage As ArrayList = New ArrayList(strSplitStorage)
                    If arrStorage.IndexOf("N'" & drRow(strColumnName) & "'".ToString) > -1 Then
                        arrStorage.RemoveAt(arrStorage.IndexOf("N'" & drRow(strColumnName) & "'".ToString))
                        strStorage = String.Join(",", arrStorage.ToArray)
                    End If
                End If
            End If
        End If
    End Sub

End Module
