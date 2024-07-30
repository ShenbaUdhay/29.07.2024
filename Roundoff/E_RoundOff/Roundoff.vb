Imports System.Globalization
Imports System.Math
Imports System.Text
Public Class Roundoff
    Dim dblnum As Double
    Dim SF As Integer
    Dim IsExponential As Boolean = False
    Public Const Base10 As Double = 10
    Dim IsNegativeInput As Boolean
    Public Function RoundoffInput(ByVal txtnum As String, ByVal txtSF As String) As String
        IsExponential = False
        IsNegativeInput = False
        If txtnum.Length > 0 AndAlso txtnum <> "" Then
            If txtnum.StartsWith("-") Then
                IsNegativeInput = True
                dblnum = Convert.ToDouble(txtnum.Replace("-", ""))
            Else
                IsNegativeInput = False
                dblnum = Convert.ToDouble(txtnum)
            End If
        Else
            dblnum = 0
        End If

        If txtSF.Length > 0 AndAlso txtSF <> "" Then
            SF = Convert.ToDouble(txtSF)
        Else
            SF = 0
        End If

        Dim output As String
        Dim outputwithnotation As String = String.Empty
        If SF > 0 Then
            If dblnum > 0 Then
                output = RoundSignificantDbl(dblnum, SF).ToString()
                If IsExponential = True Then
                    If output.Length > 0 AndAlso output <> "" Then
                        outputwithnotation = FormatAsPowerOfTen_Exp(output, SF).ToString()
                    End If
                Else
                    Dim dbl As Double
                    If output.Length > 0 AndAlso output <> "" Then
                        dbl = Convert.ToDouble(output)
                    End If
                    outputwithnotation = FormatAsPowerOfTen(dbl, SF).ToString()
                End If
                If IsNegativeInput = True Then
                    output = "-" & output
                    outputwithnotation = "-" & outputwithnotation
                End If
                RoundoffInput = output + "|" + outputwithnotation
            End If
        End If
    End Function

    Public Function RoundSignificantDbl(ByVal Value As Double, ByVal Digits As Integer) As String
        Dim Exponent As Double
        Dim Scaling As Double
        Dim Half As Object
        Dim ScaledValue As Object
        Dim ReturnValue As Double
        ' Only round if result can be different from zero.
        If (Value = 0 Or Digits <= 0) Then
            ' Nothing to round.
            ' Return Value as is.
            ReturnValue = Value
        Else
            ' Calculate scaling factor.
            Exponent = Int(Log10(Abs(Value))) + 1 - Digits
            Scaling = Base10 ^ Exponent

            If Scaling = 0 Then
                ' A very large value for Digits has minimized scaling.
                ' Return Value as is.
                ReturnValue = Value
            Else
                ' Very large values for Digits can cause an out-of-range error when dividing.
                On Error Resume Next
                ScaledValue = CDec(Value / Scaling)
                If Err.Number <> 0 Then
                    ' Return value as is.
                    ReturnValue = Value
                Else
                    ' Perform rounding.
                    ' Round to even.
                    ReturnValue = CDbl(Round(ScaledValue)) * Scaling
                    'End If
                    If Err.Number <> 0 Then
                        ' Rounding failed because values are near one of the boundaries of type Double.
                        ' Return value as is.
                        ReturnValue = Value
                    End If
                End If
            End If
        End If

        Dim ch As String
        Dim ch1 As String = ""
        Dim ch2 As String = ""

        If ReturnValue.ToString().Contains(".") Then
            ''### Need to Get length of Fractional part to set sigfig  

            '' Remove leading zero
            ch = ReturnValue.ToString.TrimStart("0"c)

            '' If result have the exponential numbers[E] then convert it to orginal format
            If ch.Contains("E") Then

                Dim intNumberofFraction As Integer
                intNumberofFraction = GetNoOfFractionDigits(CDbl(Scaling))
                ch = [String].Format("{0:F" & intNumberofFraction & "}", ReturnValue)

                Dim intNumberofFraction1 As Integer
                intNumberofFraction1 = GetNoOfFractionDigits(CDbl(ReturnValue))
                ch2 = [String].Format("{0:F" & intNumberofFraction1 & "}", ReturnValue)

                Dim ch2_split() As String
                ch2_split = ch2.Split(".")
                ch2 = ch2_split(1)

            Else
                '' get length after decimal point(fractional part)
                ch2 = FractionPart(ReturnValue)
            End If

            '' Remove Decimal for remove leading zero after decimal point
            ch = ch.ToString.Replace(".", "")
            ''remove leading zero after decimal point
            ch = ch.ToString.TrimStart("0"c)

            '' If Length of Output is Less than SigFig,then Format with Trailing Zero
            If Digits > ch.Length Then
                ch1 = Format(ReturnValue, "0." & GetTrialingZero(ch2.Length + Digits - ch.Length))
                RoundSignificantDbl = ch1
            Else

            End If
        End If

        '' To find Length of Output need to remove decimal points
        '' Declaration
        Dim fpart As String
        Dim spart As String
        Dim flpart As String

        ''Get the Interger of decimal numbers
        fpart = Before_FractionPart(ReturnValue)

        ''Get Fractional part of decimal numbers
        spart = After_FractionPart(ReturnValue)

        ''combine both Integer and fractional parts
        If spart IsNot Nothing AndAlso spart.Length > 0 Then
            flpart = fpart + spart
        Else
            flpart = fpart
        End If

        If flpart IsNot Nothing AndAlso flpart.Contains("E") Then
            IsExponential = True
        End If


        If ch1 IsNot Nothing AndAlso ch1.Length >= Digits Then
            RoundSignificantDbl = ch1
            ''RoundSignificantDbl = ReturnValue
        ElseIf ReturnValue.ToString.Length >= Digits Then
            '  ElseIf flpart.Length >= Digits Then
            RoundSignificantDbl = ReturnValue
        Else
            Dim strFormattedOutput As String = String.Empty

            Dim intNumberofFraction As Integer
            intNumberofFraction = GetNoOfFractionDigits(CDbl(Scaling))

            strFormattedOutput = [String].Format("{0:F" & intNumberofFraction & "}", ReturnValue)

            Dim strFormattedOutput_1 As String
            strFormattedOutput_1 = strFormattedOutput.TrimStart("0"c)
            strFormattedOutput_1 = strFormattedOutput_1.ToString.Replace(".", "")
            strFormattedOutput_1 = strFormattedOutput_1.TrimStart("0"c)
            '' 0.005 - 9 - 0.0050000000
            '' this is commented when get value less than length of sigfig
            '' SF and digits 
            If strFormattedOutput_1.Length > SF Then
                strFormattedOutput = [String].Format("{0:F" & intNumberofFraction - 1 & "}", ReturnValue)
            End If

            RoundSignificantDbl = strFormattedOutput
        End If

        If RoundSignificantDbl.Contains("E") Then

            IsExponential = True
            Dim strFormattedOutput_1 As String = String.Empty
            Dim intNumberofFraction As Integer
            intNumberofFraction = GetNoOfFractionDigits(CDbl(Scaling))

            strFormattedOutput_1 = [String].Format("{0:F" & intNumberofFraction & "}", ReturnValue)

            Dim strFormattedOutput_2 As String
            strFormattedOutput_2 = strFormattedOutput_1.TrimStart("0"c)
            strFormattedOutput_2 = strFormattedOutput_2.ToString.Replace(".", "")
            strFormattedOutput_2 = strFormattedOutput_2.TrimStart("0"c)
            '' 0.005 - 9 - 0.0050000000
            '' this is commented when get value less than length of sigfig
            If strFormattedOutput_2.Length > SF Then
                strFormattedOutput_1 = [String].Format("{0:F" & intNumberofFraction - 1 & "}", ReturnValue)
            End If

            RoundSignificantDbl = strFormattedOutput_1
        End If
    End Function

    Public Function GetTrialingZero(ByVal NoOfZero As Integer) As String
        Try
            Select Case NoOfZero
                Case 0
                    Return "0"
                Case 1
                    Return "0"
                Case 2
                    Return "00"
                Case 3
                    Return "000"
                Case 4
                    Return "0000"
                Case 5
                    Return "00000"
                Case 6
                    Return "000000"
                Case 7
                    Return "0000000"
                Case 8
                    Return "00000000"
                Case 9
                    Return "000000000"
            End Select
        Catch ex As Exception
            Return "0"
        End Try
    End Function

    Public Function Log10(ByVal Value As Double) As Double
        Log10 = Log(Value) / Log(Base10)
    End Function

    Public Function FormatAsPowerOfTen(ByVal value As Double, ByVal decimals As Integer) As String

        Dim NotationOutput As String
        Const Mantissa As String = "{{0:F{0}}}"

        '' Use Floor to round negative numbers so, that the number will have one digit before the decimal separator, rather than none.
        Dim exp = Convert.ToInt32(Math.Floor(Math.Log10(value)))

        Dim m As Double = Math.Pow(10, exp)

        Dim IsDecimalvalue As Boolean
        Dim isvlaueless As Boolean = False
        Dim digitcount As Integer = 0
        IsDecimalvalue = False

        ''Get the Interger of decimal numbers
        Dim firstpart As String = Before_FractionPart(value)
        ''Get the Interger of decimal numbers
        Dim secondpart As String = After_FractionPart(value)
        Dim fullpart As String = String.Empty

        '' if first part have exponential values then change it into deciaml format
        If firstpart.Contains("E") Then
            Dim strCal11 As String = String.Empty
            Dim intNumberofFraction As Integer
            intNumberofFraction = firstpart.Length
            strCal11 = [String].Format("{0:F" & intNumberofFraction & "}", value)
            firstpart = strCal11
            If firstpart.Contains(".") Then
                firstpart = firstpart.Replace(".", "")
            End If
        End If

        '' if first part have exponential values then change it into deciaml format
        If secondpart.Contains("E") Then
            Dim strCal12 As String = String.Empty
            Dim intNumberofFraction As Integer
            intNumberofFraction = secondpart.Length
            strCal12 = [String].Format("{0:F" & intNumberofFraction & "}", value)
            secondpart = strCal12
        End If

        '' Combine integer and fractional part of decimal numbers
        If secondpart <> "" OrElse secondpart.Length > 0 Then
            fullpart = firstpart & secondpart
            IsDecimalvalue = True
        Else
            IsDecimalvalue = False
            fullpart = firstpart
        End If

        ''Remove trailing zeros
        If fullpart.Contains("0") Then
            fullpart = fullpart.ToString.TrimStart("0"c)
        End If

        Dim whole As Double
        If fullpart.Length = decimals Then
            whole = value / m

            If whole >= 1 Then
                Dim fp As String = Before_FractionPart(whole)
                digitcount = fp.Length
                If decimals > 1 Then
                    fullpart = fullpart.Insert(digitcount, ".")
                End If
            Else
            End If
        ElseIf fullpart.Length <= decimals Then
            whole = value / m

            For s As Integer = fullpart.Length To decimals - 1
                fullpart = fullpart.Insert(fullpart.Length, "0")
            Next
            isvlaueless = True
            If whole >= 1 Then
                Dim fp As String = Before_FractionPart(whole)
                digitcount = fp.Length
                fullpart = fullpart.Insert(digitcount, ".")
            Else
            End If
        Else
            whole = value / m
            Dim fp As String = Before_FractionPart(whole)
            digitcount = fp.Length
            fullpart = fullpart.Insert(digitcount, ".")
        End If

        Dim dblOutput As Double = Convert.ToDouble(fullpart)
        ''Get Length of output
        Dim ss1 As String = FractionPart(dblOutput)
        Dim MantissaFormat As String

        If ss1 = "" OrElse ss1.Length = 0 Then
            MantissaFormat = String.Format(Mantissa, 1)
        Else
            MantissaFormat = String.Format(Mantissa, ss1.Length)
        End If

        '' Do not show 10^0, as this is not commonly used

        '' Don't Add or remove any Spaces,If change any spaces error may occur in LIMS Evaluator function
        '' ## Start ##
        If exp <> 0 Then MantissaFormat = String.Concat(MantissaFormat, " X 10 {1}")
        '' ## End ##

        If ss1 = "" OrElse ss1.Length = 0 OrElse isvlaueless = True Then

            '' Error - 100 - 1 - 1.00 ###  Correct - 100 - 1 - 1 
            '' Error - 100 - 2 - 1.00 ### Correct - 100 - 2 - 1.0 
            '' To Correct above Error coded below
            '' Start
            If fullpart.Length - 1 <> decimals Then

                For s1 As Integer = decimals To fullpart.Length - 1
                    fullpart = fullpart.Remove(fullpart.Length - 1, 1)
                    If fullpart.Length - 1 = decimals Then
                        Exit For
                    End If
                Next
                If decimals = 1 Then
                    fullpart = fullpart.Replace(".", "")
                End If
            End If
            '' End

            NotationOutput = String.Format(MantissaFormat, fullpart, FormatExponentWithSuperscript(exp))
        Else

            Dim strd1 As String = dblOutput.ToString()
            If strd1.Contains(".") AndAlso strd1.Length - 1 <> decimals Then
                If fullpart.Length - 1 <> decimals Then
                    For s1 As Integer = decimals To fullpart.Length - 1
                        fullpart = fullpart.Remove(fullpart.Length - 1, 1)
                        If fullpart.Length - 1 = decimals Then
                            Exit For
                        End If
                    Next
                    If decimals = 1 Then
                        fullpart = fullpart.Replace(".", "")
                    End If
                End If
                NotationOutput = String.Format(MantissaFormat, fullpart, FormatExponentWithSuperscript(exp))
            Else
                NotationOutput = String.Format(MantissaFormat, dblOutput, FormatExponentWithSuperscript(exp))
            End If
        End If
        Return NotationOutput
    End Function

    Private Shared Function Truncate(ByVal value As Double, ByVal places As Integer) As Double
        Dim f = Math.Pow(10, places)
        Return Math.Truncate(value * f) / f
    End Function

    Public Function Before_FractionPart(ByVal instance As Double) As String
        Dim result = String.Empty
        Dim ic = CultureInfo.InvariantCulture
        Dim strTemp As String = instance.ToString(ic)

        Dim splits As String() = strTemp.Split(ic.NumberFormat.NumberDecimalSeparator.ToString().ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
        If splits.Count() > 0 Then
            result = splits(0)
        End If
        Return result
    End Function

    Public Shared Function After_FractionPart(ByVal instance As Double) As String
        Dim result = String.Empty
        Dim ic = CultureInfo.InvariantCulture
        Dim strTemp As String = instance.ToString(ic)

        Dim splits As String() = strTemp.Split(ic.NumberFormat.NumberDecimalSeparator.ToString().ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
        If splits.Count() > 0 Then
            result = splits(0)
        End If
        Return result
    End Function

    Public Shared Function FractionPart(ByVal instance As Double) As String
        Dim result = String.Empty
        Dim ic = CultureInfo.InvariantCulture
        Dim strTemp As String = instance.ToString(ic)

        Dim splits As String() = strTemp.Split(ic.NumberFormat.NumberDecimalSeparator.ToString().ToCharArray(), StringSplitOptions.RemoveEmptyEntries)

        If splits.Count() > 1 Then
            result = splits(1)
        End If

        Return result
    End Function

    Private Function FormatExponentWithSuperscript(ByVal exp As Integer) As String
        Dim sb = New StringBuilder()
        Dim isNegative As Boolean = False

        If exp < 0 Then
            isNegative = True
            exp = -exp
        End If

        If exp.ToString.Length >= 2 Then
            While exp <> 0
                sb.Insert(0, GetSuperscript(exp Mod 10))
                Dim dbl As Double
                dbl = exp / 10
                Dim strs As String
                strs = Before_FractionPart(dbl)
                exp = Convert.ToInt32(strs)
                '' exp = exp / 10
            End While
        Else
            sb.Insert(0, GetSuperscript(exp Mod 10))
        End If

        If isNegative Then
            sb.Insert(0, "⁻")
        End If

        Return sb.ToString()
    End Function

    Private Shared Function GetSuperscript(ByVal digit As Integer) As String
        Select Case digit
            Case 0
                Return "⁰"
            Case 1
                Return "¹"
            Case 2
                Return "²"
            Case 3
                Return "³"
            Case 4
                Return "⁴"
            Case 5
                Return "⁵"
            Case 6
                Return "⁶"
            Case 7
                Return "⁷"
            Case 8
                Return "⁸"
            Case 9
                Return "⁹"
            Case Else
                Return String.Empty
        End Select
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



    Public Function FormatAsPowerOfTen_Exp(ByVal value As String, ByVal decimals As Integer) As String

        Const Mantissa As String = "{{0:F{0}}}"

        '' Use Floor to round negative numbers so, that the number will have one digit before the decimal separator, rather than none.
        Dim exp = Convert.ToInt32(Math.Floor(Math.Log10(value)))
        'Dim e1 = ((Log10(value)))

        Dim m As Double = Math.Pow(10, exp)

        Dim IsDecimalvalue As Boolean
        Dim isvlaueless As Boolean = False
        Dim digitcount As Integer = 0
        IsDecimalvalue = False

        Dim spt() As String
        spt = value.Split(".")

        Dim firstpart As String = spt(0)
        Dim secondpart As String = spt(1)

        'Dim firstpart As String = Before_FractionPart(value)
        'Dim secondpart As String = After_FractionPart(value)
        Dim fullpart As String = String.Empty


        If firstpart.Contains("E") Then
            Dim strCal11 As String = String.Empty
            Dim intNumberofFraction As Integer
            intNumberofFraction = firstpart.Length
            strCal11 = [String].Format("{0:F" & intNumberofFraction & "}", value)
            firstpart = strCal11
            If firstpart.Contains(".") Then
                firstpart = firstpart.Replace(".", "")
            End If
        End If

        If secondpart.Contains("E") Then
            Dim strCal12 As String = String.Empty
            Dim intNumberofFraction As Integer
            intNumberofFraction = secondpart.Length
            strCal12 = [String].Format("{0:F" & intNumberofFraction & "}", value)
            secondpart = strCal12
            If secondpart.Contains(".") Then
                secondpart = secondpart.Replace(".", "")
            End If
        End If


        If secondpart <> "" OrElse secondpart.Length > 0 Then
            fullpart = firstpart & secondpart
            IsDecimalvalue = True
        Else
            IsDecimalvalue = False
            fullpart = firstpart

        End If


        If fullpart.Contains("0") Then
            fullpart = fullpart.ToString.TrimStart("0"c)
        End If

        Dim whole As Double
        If fullpart.Length = decimals Then
            whole = value / m

            If whole >= 1 Then
                Dim fp As String = Before_FractionPart(whole)
                digitcount = fp.Length
                ' If IsDecimalvalue = True Then
                If decimals > 1 Then
                    fullpart = fullpart.Insert(digitcount, ".")
                End If

                'End If
            Else

            End If
        ElseIf fullpart.Length <= decimals Then
            whole = value / m

            For s As Integer = fullpart.Length To decimals - 1
                fullpart = fullpart.Insert(fullpart.Length, "0")
            Next
            isvlaueless = True
            If whole >= 1 Then
                Dim fp As String = Before_FractionPart(whole)
                digitcount = fp.Length
                fullpart = fullpart.Insert(digitcount, ".")


            Else

            End If
        Else
            whole = value / m

            Dim fp As String = Before_FractionPart(whole)
            ''Dim dtmp As Double
            ''dtmp = Double.Parse(fullpart, System.Globalization.NumberStyles.Float)
            digitcount = fp.Length
            ''Dim firstpart1 As String = Before_FractionPart(dtmp)
            fullpart = fullpart.Insert(digitcount, ".")
        End If

        Dim d1 As Double = Convert.ToDouble(fullpart)
        Dim a = Truncate(d1, decimals)
        Dim ss1 As String = FractionPart(d1)
        Dim fmt As String

        If ss1 = "" OrElse ss1.Length = 0 Then
            fmt = String.Format(Mantissa, 1)
        Else
            fmt = String.Format(Mantissa, ss1.Length)
        End If

        '' Do not show 10^0, as this is not commonly used in scientific publications.
        If exp <> 0 Then fmt = String.Concat(fmt, " × 10{1}")

        Dim fnl As String

        'If ss1 = "" OrElse ss1.Length = 0 Then
        fnl = String.Format(fmt, fullpart, FormatExponentWithSuperscript(exp))
        'Else
        'fnl = String.Format(fmt, d1, FormatExponentWithSuperscript(exp))
        ' End If

        Return fnl
    End Function

    Public Function FormatDecimalValue(ByVal Value As String, ByVal DecimalValue As Integer) As String
        Dim FormatedValue As String = String.Empty
        Try
            If Len(Value) > 0 Then
                If IsNumeric(Value) Then
                    Select Case DecimalValue
                        Case 0
                            FormatedValue = Format(CDbl(Value), "0")
                        Case 1
                            FormatedValue = Format(CDbl(Value), "0.0")
                        Case 2
                            FormatedValue = Format(CDbl(Value), "0.00")
                        Case 3
                            FormatedValue = Format(CDbl(Value), "0.000")
                        Case 4
                            FormatedValue = Format(CDbl(Value), "0.0000")
                        Case 5
                            FormatedValue = Format(CDbl(Value), "0.00000")
                        Case 6
                            FormatedValue = Format(CDbl(Value), "0.000000")
                        Case 7
                            FormatedValue = Format(CDbl(Value), "0.0000000")
                        Case 8
                            FormatedValue = Format(CDbl(Value), "0.00000000")
                        Case 9
                            FormatedValue = Format(CDbl(Value), "0.000000000")
                        Case Else
                            FormatedValue = Value
                    End Select

                    Return FormatedValue
                Else
                    Return ""
                End If
            Else
                Return ""
            End If
        Catch ex As Exception
            Return ""
        End Try
    End Function

End Class
