Imports System.Math
Namespace LIMSFormula
    Public Class LIMSFormula

        Public Function FormatSF(ByVal dblInput As Double, ByVal intSF As Integer, ByVal bolIsNotation As Boolean) As String
            Try
                If dblInput = 0 Then
                    Return dblInput.ToString
                End If
                Dim strResult As String = String.Empty
                Dim strCal As String = String.Empty
                Dim intCorrPower As Integer         'Exponent used in rounding calculation
                Dim intSign As Integer              'Holds sign of dblInput since logs are used in 
                Dim intInputDigitCount As Integer
                '-- Store sign of dblInput --
                intSign = Sign(dblInput)

                Dim strInput As String = String.Empty
                strInput = CStr(dblInput)
                If Not strInput Is Nothing AndAlso Len(strInput) > 0 Then
                    Dim strInputSplit() As String = Split(strInput, ".")
                    If Not strInputSplit Is Nothing AndAlso strInputSplit.Length > 0 Then
                        If strInputSplit(0).Contains("-") Then
                            intInputDigitCount = Len(strInputSplit(0)) - 1
                        Else
                            intInputDigitCount = Len(strInputSplit(0))
                        End If
                    End If
                End If

                '-- Calculate exponent of dblInput --
                intCorrPower = Int(Log10(Abs(dblInput)))
                '--START: Modified by Mohan, on 17th December 2014 for http://ablabs.net/btlimstracker/edit_bug.aspx?id=2334
                strCal = dblInput
                If intCorrPower >= 0 AndAlso Mid(strCal, 1, intSF).Contains(".") Then 'strCal.Contains(".") Then
                    Try
                        Dim chArrCal As Char() = strInput.ToCharArray()
                        Dim strTempstrCal As String = String.Empty
                        strTempstrCal = Replace(Replace(strCal, "-", ""), ".", "")
                        'When length of input value is more than significant number and 
                        'next char after significant number is greater than 4
                        If strTempstrCal.Length > intSF AndAlso Mid(strTempstrCal, intSF + 1, 1) > 4 Then
                            Dim intExtraDigitsToCare As Integer = 0
                            If New String(chArrCal, 0, intSF).Contains("-") = True AndAlso New String(chArrCal, 0, intSF).Contains(".") = True Then
                                intExtraDigitsToCare = 2
                            ElseIf New String(chArrCal, 0, intSF).Contains("-") = True OrElse New String(chArrCal, 0, intSF).Contains(".") = True Then
                                intExtraDigitsToCare = 1
                            End If
                            '- Task ID 3302, if number is 4.251 and sigfig 2, here 
                            ' 1st condition checks last significant digit [2] is even or odd, ==> 4.251 = 4.2 (2 is even) => 4.25
                            ' 2nd condition checks the following significant digit [5] is 5 or not, ==>(after sigfig is 5) => 4.251
                            ' 3rd condition checks is there any number after significant digit [5] except 0 or empty, ==>(number after 5 is 1, so we need to add 1 to last significant digit i.e 2+1 = 3) => 4.3
                            ' 1,2 and 3 conditions satisfies then result is 4.3
                            '  
                            'If strTempstrCal.Length > intSF + 2 AndAlso Mid(strTempstrCal, intSF, 1) Mod 2 = 0 AndAlso Mid(strTempstrCal, intSF + 1, 1) = 5 AndAlso _
                            '  (Mid(strTempstrCal, intSF + 2, 1) = 0 OrElse Mid(strTempstrCal, intSF + 2, 1) = "") Then '-- TASK ID : 2925

                            'AndAlso Mid(strTempstrCal, intSF + 1, 1) = 5
                            If Mid(strTempstrCal, intSF, 1) Mod 2 = 0 AndAlso Mid(strTempstrCal, intSF + 1, 1) = 5 Then
                                If strTempstrCal.Length = intSF + 1 Then
                                    'When last significant digit is Even number
                                    strCal = New String(chArrCal, 0, intSF + intExtraDigitsToCare)
                                End If
                            Else
                                'When last significant digit is Odd number
                                strCal = New String(chArrCal, 0, intSF + intExtraDigitsToCare)
                                If Val(Microsoft.VisualBasic.Right(strCal, 1)) > 8 AndAlso strCal.Contains(".") Then
                                    'strCal = New String(chArrCal, 0, intSF + intExtraDigitsToCare - 1) & (Microsoft.VisualBasic.Right(strCal, 1) + 1)
                                    Dim intNoofDecimals As Integer = strCal.Length - InStr(strCal, ".")
                                    Dim strDecimal As String = "0."
                                    Do Until strDecimal.Length >= intNoofDecimals + 1
                                        strDecimal = strDecimal & "0"
                                    Loop
                                    strCal = CDbl(strCal) + CDbl((strDecimal & "1"))
                                ElseIf Val(Microsoft.VisualBasic.Right(strCal, 1)) > 8 Then
                                    strCal = Val(strCal) + 1
                                Else
                                    strCal = New String(chArrCal, 0, intSF + intExtraDigitsToCare - 1) & (Microsoft.VisualBasic.Right(strCal, 1) + 1)
                                End If
                            End If
                        End If

                    Catch ex As Exception
                        'If any error, assign the original input value
                        strCal = strInput
                    Finally
                        dblInput = CDbl(strCal)
                    End Try
                ElseIf Mid(strCal, 1, intSF).Contains(".") Then 'strCal.Contains(".") Then
                    Try
                        Dim chArrCal As Char() = strInput.ToCharArray()
                        Dim strTempstrCal As String = String.Empty
                        strTempstrCal = Replace(Replace(strCal, "-", ""), ".", "")
                        Dim strTempstrCal_1 As String = strTempstrCal
                        Dim intZeroCount As Integer = Len(strTempstrCal) - Len(strTempstrCal_1.TrimStart("0"c))
                        'Dim strSplit() As String = Split(strCal, ".")
                        'If Len(Replace(strSplit(0), "-", "")) > 0 Then
                        '    intZeroCount = intZeroCount - Len(Replace(strSplit(0), "-", ""))
                        'End If
                        If (intZeroCount + intSF + 1) >= 0 AndAlso (intZeroCount + intSF + 1) <= strTempstrCal.Length Then
                            'If strTempstrCal.Length > intSF AndAlso Mid(strTempstrCal, strTempstrCal.Length - (intSF - (intZeroCount + 1)), 1) > 4 Then
                            If strTempstrCal.Length > intSF AndAlso Mid(strTempstrCal, (intZeroCount + intSF + 1), 1) > 4 Then
                                Dim intExtraDigitsToCare As Integer = 0
                                If New String(chArrCal, 0, intSF).Contains("-") = True AndAlso New String(chArrCal, 0, intSF).Contains(".") = True Then
                                    intExtraDigitsToCare = 2 + intZeroCount
                                ElseIf New String(chArrCal, 0, intSF).Contains("-") = True OrElse New String(chArrCal, 0, intSF).Contains(".") = True Then
                                    intExtraDigitsToCare = 1 + intZeroCount
                                End If
                                If Mid(strTempstrCal, intSF + intZeroCount, 1) Mod 2 = 0 AndAlso Mid(strTempstrCal, intSF + 1 + intZeroCount, 1) = 5 AndAlso _
                                  (Mid(strTempstrCal, intSF + 2 + intZeroCount, 1) = "" OrElse Mid(strTempstrCal, intSF + 2 + intZeroCount, 1) = 0) Then
                                    strCal = New String(chArrCal, 0, intSF + intExtraDigitsToCare)
                                Else
                                    strCal = New String(chArrCal, 0, intSF + intExtraDigitsToCare)
                                    If Val(Microsoft.VisualBasic.Right(strCal, 1)) > 8 AndAlso strCal.Contains(".") Then
                                        Dim intNoofDecimals As Integer = strCal.Length - InStr(strCal, ".")
                                        Dim strDecimal As String = "0."
                                        Do Until strDecimal.Length >= intNoofDecimals + 1
                                            strDecimal = strDecimal & "0"
                                        Loop
                                        strCal = CDbl(strCal) + CDbl((strDecimal & "1"))
                                    ElseIf Val(Microsoft.VisualBasic.Right(strCal, 1)) > 8 Then
                                        strCal = Val(strCal) + 1
                                    Else
                                        strCal = New String(chArrCal, 0, intSF + intExtraDigitsToCare - 1) & (Microsoft.VisualBasic.Right(strCal, 1) + 1)
                                    End If
                                End If
                            End If
                        End If
                    Catch ex As Exception
                        strCal = strInput
                    Finally
                        dblInput = CDbl(strCal)
                    End Try
                End If
                '--End: Modified by Mohan, on 17th December 2014 for http://ablabs.net/btlimstracker/edit_bug.aspx?id=2334
                strCal = Round(dblInput * 10 ^ ((intSF - 1) - intCorrPower))   'integer value with no sig fig
                'If bolIsNotation = False Or (bolIsNotation = True AndAlso intCorrPower > 0) Then
                '    strCal = strCal * 10 ^ (intCorrPower - (intSF - 1))         'raise to original power
                'End If

                If bolIsNotation = False Then
                    Dim strDot As String = String.Empty
                    If (intSF - (intInputDigitCount + 1)) >= 0 AndAlso (intSF - (intInputDigitCount + 1)) <= strCal.Length Then
                        If CDbl(Mid(strCal, strCal.Length - (intSF - (intInputDigitCount + 1)), strCal.Length)) = 0 Then
                            strDot = Mid(strCal, strCal.Length - (intSF - (intInputDigitCount + 1)), strCal.Length)
                        End If
                    End If
                    strCal = strCal * 10 ^ (intCorrPower - (intSF - 1))        'raise to original power
                    Dim intNumberofFraction As Integer
                    intNumberofFraction = GetNoOfFractionDigits(CDbl(strCal))
                    strCal = [String].Format("{0:F" & intNumberofFraction & "}", (CDbl(strCal)))
                    If Len(strDot) > 0 Then
                        If strCal.Contains(".") Then
                            strCal = strCal & strDot
                        Else
                            strCal = strCal & "." & strDot
                        End If
                    End If
                    'If flagcal = False Then
                    '    If (intCorrPower - (intSF - 1)) > 0 AndAlso intCorrPower > 1 Then
                    '        flagcal = True
                    '        Return FormatSF(CDbl(strCal), intSF, True)
                    '    End If
                    'End If
                ElseIf (bolIsNotation = True AndAlso intCorrPower > 0) Then
                    If InStr(dblInput, ".") = 0 AndAlso Len(strCal.Replace("-", "")) = intSF Then
                    Else
                        Dim strDot As String = String.Empty
                        If (intSF - (intInputDigitCount + 1)) >= 0 AndAlso (intSF - (intInputDigitCount + 1)) <= strCal.Length Then
                            If CDbl(Mid(strCal, strCal.Length - (intSF - (intInputDigitCount + 1)), strCal.Length)) = 0 Then
                                strDot = Mid(strCal, strCal.Length - (intSF - (intInputDigitCount + 1)), strCal.Length)
                            End If
                        End If
                        If intCorrPower <> intSF Then
                            strCal = strCal * 10 ^ (intCorrPower - (intSF - 1))        'raise to original power
                        Else

                        End If
                        Dim intNumberofFraction As Integer
                        intNumberofFraction = GetNoOfFractionDigits(CDbl(strCal))
                        strCal = [String].Format("{0:F" & intNumberofFraction & "}", (CDbl(strCal)))
                        If Len(strDot) > 0 Then
                            strCal = strCal & "." & strDot
                        End If
                    End If
                    'If (intCorrPower - (intSF - 1)) > 0 AndAlso intCorrPower > 1 Then
                    '    Return FormatSF(CDbl(strCal), intSF, True)
                    'End If
                End If
                ''-- Answer sometimes needs padding with 0s --
                'If InStr(strCal, ".") = 0 Then
                '    If Len(strCal) < intSF Then
                '        strCal = Format(strCal, "##0." & CStr(intSF - Len(strCal)))
                '    End If
                'End If
                If intSF > 1 And Abs(CDbl(strCal)) < 1 Then
                    Do Until Microsoft.VisualBasic.Left(Microsoft.VisualBasic.Right(strCal, intSF), 1) <> "0" And Microsoft.VisualBasic.Left(Microsoft.VisualBasic.Right(strCal, intSF), 1) <> "."
                        strCal = strCal & "0"
                    Loop
                End If

                If bolIsNotation = False Then
                    If Val(strCal) > 0 Then
                        Dim strTem As String = Replace(strCal, ".", "")
                        Dim Icount As Integer = intSF - Len(strTem)
                        For iCheck As Integer = 1 To Icount
                            strCal = strCal & "0"
                        Next
                    End If
                    strResult = strCal
                    Return strResult
                ElseIf bolIsNotation = True Then
                    If intCorrPower <> 0 Then

                        Dim strFisrt As String = Microsoft.VisualBasic.Left(strCal, 1)
                        If strFisrt = "-" Then
                            strFisrt = Microsoft.VisualBasic.Left(strCal, 2)
                        End If
                        Dim strSecont As String = Microsoft.VisualBasic.Right(strCal, strCal.Length - 1)
                        If Len(strSecont) > 0 Then
                            strResult = strFisrt & "." & Replace(strSecont, ".", "") & " X 10 " & SplitChar(intCorrPower)
                        Else
                            strResult = strFisrt & " X 10 " & SplitChar(intCorrPower)
                        End If

                        'strResult = strFisrt & "." & Replace(strSecont, ".", "") & " * 10 ^ " & intCorrPower
                        Return strResult
                    Else
                        Dim strDot As String = String.Empty
                        If (intSF - (intInputDigitCount + 1)) >= 0 AndAlso (intSF - (intInputDigitCount + 1)) <= strCal.Length Then
                            If CDbl(Mid(strCal, strCal.Length - (intSF - (intInputDigitCount + 1)), strCal.Length)) = 0 Then
                                strDot = Mid(strCal, strCal.Length - (intSF - (intInputDigitCount + 1)), strCal.Length)
                            End If
                        End If
                        strCal = strCal * 10 ^ (intCorrPower - (intSF - 1))        'raise to original power
                        If Len(strDot) > 0 Then
                            strCal = strCal & "." & strDot
                        End If
                        strResult = strCal
                        Return strResult
                    End If
                End If
                Return strResult
            Catch ex As Exception
                Return dblInput.ToString
                'Finally
                '    flagcal = False
            End Try
        End Function
        Public Function GetNoOfFractionDigits(ByVal dbl As String) As Integer
            Try
                Dim iFCount As Integer
                If dbl IsNot Nothing AndAlso dbl.Trim.Length > 0 AndAlso IsNumeric(dbl) AndAlso dbl.Contains(".") = True Then
                    iFCount = dbl.Trim.Split(CChar(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))(1).Length()
                End If
                Return iFCount
            Catch ex As Exception
                Return 0
            End Try
        End Function
        Public Function GetNoOfFractionDigits(ByVal dbl As Double) As Integer
            Try

                Dim iFCount As Integer
                Dim strNumber As String = CStr(dbl)
                If Not strNumber Is Nothing AndAlso Len(strNumber) > 0 AndAlso IsNumeric(strNumber) Then

                    Dim decDbNumber As Decimal
                    decDbNumber = Decimal.Parse(strNumber, System.Globalization.NumberStyles.Any)
                    Dim strNumberSplit() As String = decDbNumber.ToString.Split(CChar(System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator))
                    If strNumberSplit.Length > 1 Then
                        iFCount = strNumberSplit(1).Length
                    End If

                End If
                Return iFCount
            Catch ex As Exception
                Return 0
            End Try
        End Function
        Private Function SplitChar(ByVal strVal As String) As String
            Dim RetChar As String = String.Empty
            Dim singleChar As Char
            For Each singleChar In strVal
                If Len(RetChar) > 0 Then
                    RetChar = RetChar & SuperScriptChar(singleChar)
                Else
                    RetChar = SuperScriptChar(singleChar)
                End If
            Next
            Return RetChar
        End Function

        Private Function SuperScriptChar(ByVal strVal As String) As String
            If strVal = "0" Then
                Return "⁰"
            End If
            If strVal = "1" Then
                Return "¹"
            End If
            If strVal = "2" Then
                Return "²"
            End If
            If strVal = "3" Then
                Return "³"
            End If
            If strVal = "4" Then
                Return "⁴"
            End If
            If strVal = "5" Then
                Return "⁵"
            End If
            If strVal = "6" Then
                Return "⁶"
            End If
            If strVal = "7" Then
                Return "⁷"
            End If
            If strVal = "8" Then
                Return "⁸"
            End If
            If strVal = "9" Then
                Return "⁹"
            End If
            If strVal = "+" Then
                Return "⁺"
            End If
            If strVal = "-" Then
                Return "⁻"
            End If
            Return ""
        End Function
        Public Function SubScriptChar(ByVal strvalue As String) As String
            If strvalue = "⁰" Then
                Return "0"
            End If
            If strvalue = "¹" Then
                Return "1"
            End If
            If strvalue = "²" Then
                Return "2"
            End If
            If strvalue = "³" Then
                Return "3"
            End If
            If strvalue = "⁴" Then
                Return "4"
            End If
            If strvalue = "⁵" Then
                Return "5"
            End If
            If strvalue = "⁶" Then
                Return "6"
            End If
            If strvalue = "⁷" Then
                Return "7"
            End If
            If strvalue = "⁸" Then
                Return "8"
            End If
            If strvalue = "⁹" Then
                Return "9"
            End If
            If strvalue = "⁺" Then
                Return "+"
            End If
            If strvalue = "⁻" Then
                Return "-"
            End If
            If strvalue = "⁻¹" Then
                Return "-1"
            End If
            If strvalue = "⁻²" Then
                Return "-2"
            End If
            If strvalue = "⁻³" Then
                Return "-3"
            End If
            If strvalue = "⁻⁴" Then
                Return "-4"
            End If
            If strvalue = "⁻⁵" Then
                Return "-5"
            End If
            If strvalue = "⁻⁶" Then
                Return "-6"
            End If
            If strvalue = "⁻⁷" Then
                Return "-7"
            End If
            If strvalue = "⁻⁸" Then
                Return "-8"
            End If
            If strvalue = "⁻⁹" Then
                Return "-9"
            End If
            Return ""
        End Function
    End Class
End Namespace