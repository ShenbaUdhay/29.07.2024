Imports System.Data.SqlClient


Public Class DataManager

    Public Shared Function ExecuteQuery(ByVal query As String) As DataTable
        Try
            Dim myConnection As SqlConnection = New SqlConnection(ConnectionSettings.cnString)
            Dim myAdapter As SqlDataAdapter = New SqlDataAdapter(query, myConnection)
            myAdapter.SelectCommand.CommandTimeout = 6000
            Dim dt As DataTable = New DataTable
            myAdapter.Fill(dt)
            dt.TableName = ""
            If Not dt Is Nothing Then
                Return dt
            Else
                Return New DataTable
            End If
        Catch ex As Exception
            Return New DataTable
        End Try
    End Function
    Public Shared Function ExecuteScalar(ByVal query As String) As Int16
        Dim myConnection As SqlConnection
        myConnection = New SqlConnection(ConnectionSettings.cnString)
        Try
            myConnection.Open()
            Dim ScalarQuery As New SqlClient.SqlCommand(query, myConnection)
            Return (CInt(ScalarQuery.ExecuteScalar()))
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            If myConnection.State = ConnectionState.Open Then
                myConnection.Close()
            End If
        End Try
    End Function
    Public Shared Sub ExecuteNonQuery(ByVal query As String)
        Dim myConnection As SqlConnection
        myConnection = New SqlConnection(ConnectionSettings.cnString)
        Try
            myConnection.Open()

            Dim myCommand As New SqlCommand(query, myConnection)
            myCommand.ExecuteNonQuery()
        Catch ex As Exception
            Try
                Utils.LogError(ex, "Error in ExecuteQuery() method in DataManager. <BR><BR>" & query)
            Catch
            End Try
            Throw
        Finally
            If myConnection.State = ConnectionState.Open Then
                myConnection.Close()
            End If
        End Try
    End Sub
End Class

Public Class DataManagerTransaction
    Private con As SqlConnection
    Private trans As SqlTransaction

    Public Sub SQLBeginTransaction()
        con = New SqlConnection(ConnectionSettings.cnString)
        con.Open()
        trans = con.BeginTransaction(IsolationLevel.ReadUncommitted)
    End Sub

    Public Function ExecuteNonQuery(ByVal query As String) As Boolean
        Try
            Dim myCommand As New SqlCommand(query, con, trans)
            myCommand.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function ExecuteQuery(ByVal query As String) As DataTable
        Try
            'Dim myConnection As SqlConnection = New SqlConnection(ConnectionSettings.cnString)
            Dim myAdapter As SqlDataAdapter = New SqlDataAdapter(New SqlCommand(query, con, trans))
            Dim dt As DataTable = New DataTable
            myAdapter.Fill(dt)

            dt.TableName = ""
            If dt.Rows.Count > 0 Then
                Return dt
            Else
                Return New DataTable
            End If
        Catch ex As Exception
            Try
                Utils.LogError(ex, "Error in Transaction ExecuteQuery . " & vbCrLf & query)
            Catch
            End Try
            Return New DataTable
        End Try
    End Function

    Public Sub RollBack()
        trans.Rollback()
        con.Close()
    End Sub

    Public Sub SQLCloseTransaction()
        trans.Commit()
        con.Close()
    End Sub

End Class


