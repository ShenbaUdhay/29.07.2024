
Namespace PUBLICBO
    Public Class MultiLanguageBL
        Dim objMLDC As DynamicReportDataLayer.PUBLICDC.MultiLanguageDC
        Public Sub New()
            Try
                objMLDC = New DynamicReportDataLayer.PUBLICDC.MultiLanguageDC

            Catch ex As Exception
                BLCommon.InsertExceptionTrackingBL(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
            End Try
        End Sub

        Public Function FillMultiLangData(ByVal FormName As String, Optional ByVal UserID As Integer = -1) As DataSet
            Try
                Return objMLDC.GetMultiLangData(FormName, UserID)
            Catch ex As Exception
                BLCommon.InsertExceptionTrackingBL(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
                Return Nothing
            End Try
        End Function
        Public Function GetNavItemData(ByVal UserID As Integer) As DataSet
            Try
                Return objMLDC.GetNavItemData(UserID)
            Catch ex As Exception
                BLCommon.InsertExceptionTrackingBL(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
                Return Nothing
            End Try
        End Function
        Public Function CheckSamplingDatabase() As DataTable
            Try
                Return objMLDC.CheckSamplingDatabase()
            Catch ex As Exception
                BLCommon.InsertExceptionTrackingBL(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
                Return Nothing
            End Try
        End Function
        Public Function FillAppMultiLangData() As DataTable
            Try
                Return objMLDC.GetAppMultiLangData()
            Catch ex As Exception
                BLCommon.InsertExceptionTrackingBL(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
                Return Nothing
            End Try
        End Function
        Public Function FillAppMultiLangData_New() As DataSet
            Try
                Return objMLDC.GetAppMultiLangData_New()
            Catch ex As Exception
                BLCommon.InsertExceptionTrackingBL(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
                Return Nothing
            End Try
        End Function
        Public Function GetConstantData() As DataTable
            Try
                Return objMLDC.GetConstantData()
            Catch ex As Exception
                BLCommon.InsertExceptionTrackingBL(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
                Return Nothing
            End Try
        End Function
        Public Function CheckUserExists(ByVal UserName As String, ByVal Action As String) As DataTable
            Try
                Return objMLDC.CheckUserExists(UserName, Action)
            Catch ex As Exception
                Return Nothing
            End Try
        End Function
    End Class
End Namespace
