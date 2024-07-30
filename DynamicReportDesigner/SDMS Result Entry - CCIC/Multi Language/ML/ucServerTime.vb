Public Class ucServerTime
    Private cDT As DateTime
    Private NextLocalDT As DateTime
    Private lDiff As Long

    Public Property CurrentDateTime() As DateTime
        Get
            Try
                If NextLocalDT < Now Then
                    '  cDT = GetServerTimes()
                    NextLocalDT = DateAdd(DateInterval.Second, 30, Now)
                Else
                    lDiff = DateDiff(DateInterval.Second, NextLocalDT, Now)
                    If lDiff > 30 OrElse lDiff < -30 Then
                        '  cDT = GetServerTimes()
                        NextLocalDT = DateAdd(DateInterval.Second, 30, Now)
                    End If
                End If
                Return cDT
            Catch ex As Exception
                '  BLCommon.InsertExceptionTrackingBL(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
                Return cDT
            End Try
        End Get
        Set(ByVal value As DateTime)
            cDT = value
        End Set
    End Property

    Public Sub New()
        Try
            'cDT = GetServerTimes()
            NextLocalDT = DateAdd(DateInterval.Second, 30, Now)
        Catch ex As Exception
            'BLCommon.InsertExceptionTrackingBL(ex, Me.GetType.Name, Reflection.MethodInfo.GetCurrentMethod.Name)
        End Try
    End Sub

End Class
