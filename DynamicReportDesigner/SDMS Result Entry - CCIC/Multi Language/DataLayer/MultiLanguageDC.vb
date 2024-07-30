Imports System.Data.SqlClient

Namespace PUBLICDC

    Public Class MultiLanguageDC
        Public ConnectionString As String
        Public Function GetMultiLangData(ByVal FormName As String, ByVal UserID As Integer) As DataSet
            Try
                Dim params() As SqlParameter = New SqlParameter(0) {}
                params(0) = New SqlParameter("@FormName", FormName)
                Dim d As DataSet = SqlHelper.ExecuteDataset(ConnectionSettings.cnString, CommandType.StoredProcedure, "Public_GetLangData_SP", params)
                ConnectionString = ConnectionSettings.cnString
                Return d

            Catch ex As Exception
                DataFunctions.InsertExceptionTrackingDC(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
                Return Nothing
            End Try
        End Function
        Public Function GetNavItemData(ByVal UserID As Integer) As DataSet
            Try
                Dim params() As SqlParameter = New SqlParameter(0) {}
                params(0) = New SqlParameter("@UserID", UserID)
                Dim d As DataSet = SqlHelper.ExecuteDataset(ConnectionSettings.cnString, CommandType.StoredProcedure, "Public_GetNavItem_SP", params)
                Return d
            Catch ex As Exception
                DataFunctions.InsertExceptionTrackingDC(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
                Return Nothing
            End Try
        End Function
        Public Function CheckSamplingDatabase() As DataTable
            Try
                Dim d As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "Public_GetSamplingDatabase_SP")
                Return d
            Catch ex As Exception
                DataFunctions.InsertExceptionTrackingDC(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
                Return Nothing
            End Try
        End Function
        Public Function GetAppMultiLangData() As DataTable
            Try
                Dim d As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "Public_GetAppLangData_SP")
                Return d
            Catch ex As Exception
                DataFunctions.InsertExceptionTrackingDC(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
                Return Nothing
            End Try
        End Function
        Public Function GetAppMultiLangData_New() As DataSet
            Try
                Dim d As DataSet = SqlHelper.ExecuteDataset(ConnectionSettings.cnString, CommandType.StoredProcedure, "Public_GetAppLangData_New_SP")
                Return d
            Catch ex As Exception
                DataFunctions.InsertExceptionTrackingDC(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
                Return Nothing
            End Try
        End Function
        Public Function GetConstantData() As DataTable
            Try
                Dim d As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, "Public_GetConstantDatas_SP")
                Return d
            Catch ex As Exception
                DataFunctions.InsertExceptionTrackingDC(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
                Return Nothing
            End Try
        End Function
        Public Function CheckUserExists(ByVal UserName As String, ByVal Action As String) As DataTable
            Dim params() As SqlParameter = New SqlParameter(1) {}
            params(0) = New SqlParameter("@colUserName", UserName)
            params(1) = New SqlParameter("@Action", Action)
            Dim dt As DataTable = SqlHelper.ExecuteDataTable(ConnectionSettings.cnString, CommandType.StoredProcedure, _
                    "User_CheckLimsOpened_SP", params)

            If dt.Rows.Count > 0 Then
                Return dt
            Else
                Return Nothing
            End If
        End Function
    End Class
End Namespace
