Public Class clsConvertLibrary

    Public Shared Function 度分秒をミリ秒に変換(Lat_度分秒 As Integer, Lon_度分秒 As Integer, ByRef Lat_ミリ秒 As Integer, ByRef Lon_ミリ秒 As Integer) As Boolean
        Try

            Lat_ミリ秒 = 0
            Lon_ミリ秒 = 0
            Dim 日本緯度lat As Double = Conv松下緯度To度(Lat_度分秒)
            Dim 日本経度lon As Double = Conv松下経度To度(Lon_度分秒)
            Lat_ミリ秒 = CInt(日本緯度lat * 36000)
            Lon_ミリ秒 = CInt(日本経度lon * 36000)

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function



    Private Shared Function Conv経度To時分秒(ByVal value As Integer) As Double

        '経度用
        If value = 0 Then
            Return 0
        End If
        Try
            Dim s As String = value.ToString

            Dim p2_keido_do As Double = CDbl(s.Substring(0, 3)) * 60 * 60
            Dim p2_keido_hun As Double = CDbl(CDbl(s.Substring(3, 2)) * 60)
            Dim p2_keido_byou As Double = CDbl(CDbl(s.Substring(5, 3)) / 10)
            Dim p2_keido As Double = p2_keido_do + p2_keido_hun + p2_keido_byou

            Return p2_keido

        Catch ex As Exception

            Return -1

        End Try

    End Function

    Private Shared Function Conv緯度To時分秒(ByVal value As Integer) As Double

        '緯度用
        If value = 0 Then
            Return 0
        End If

        Try

            Dim s As String = value.ToString
            ' "35 09 534"
            Dim p1_ido_do As Double = CDbl(s.Substring(0, 2)) * 60 * 60
            Dim p1_ido_hun As Double = CDbl(CDbl(s.Substring(2, 2)) * 60)
            Dim p1_ido_byou As Double = CDbl(CDbl(s.Substring(4, 3)) / 10)
            Dim p1_ido As Double = p1_ido_do + p1_ido_hun + p1_ido_byou

            Return p1_ido

        Catch ex As Exception

            Return -1

        End Try

    End Function

    Private Sub proc(ByVal in_松下ido_1 As Integer, ByVal in_松下keido_1 As Integer, ByVal in_松下ido_2 As Integer, ByVal in_松下keido_2 As Integer, ByRef out_ido_spn As String, ByRef out_keido_spn As String)


        If in_松下ido_1 = 0 Or in_松下keido_1 = 0 Or in_松下ido_2 = 0 Or in_松下keido_2 = 0 Then
            out_ido_spn = " 000"
            out_keido_spn = " 000"
            Return
        End If

        Dim ido_1 As Double = Conv緯度To時分秒(in_松下ido_1)
        Dim keido_1 As Double = Conv経度To時分秒(in_松下keido_1)

        Dim ido_2 As Double = Conv緯度To時分秒(in_松下ido_2)
        Dim keido_2 As Double = Conv経度To時分秒(in_松下keido_2)

        Dim spn緯度 As Double = ido_2 - ido_1
        Dim spn経度 As Double = keido_2 - keido_1

        Dim ido_spn As Integer = 0
        Dim keido_spn As Integer = 0

        out_ido_spn = ""
        out_keido_spn = ""

        If Not Conv時分秒ToInt(spn緯度, ido_spn) Then
            Throw New Exception
        End If
        If Not Conv時分秒ToInt(spn経度, keido_spn) Then
            Throw New Exception
        End If

        If ido_spn < 0 Then
            out_ido_spn = String.Format("-{0:0##}", Math.Abs(ido_spn))
        Else
            out_ido_spn = String.Format(" {0:0##}", Math.Abs(ido_spn))
        End If

        If keido_spn < 0 Then
            out_keido_spn = String.Format("-{0:0##}", Math.Abs(keido_spn))
        Else
            out_keido_spn = String.Format(" {0:0##}", Math.Abs(keido_spn))
        End If


    End Sub


    Private Function Conv時分秒ToInt(ByVal inVal As Double, ByRef outVal As Integer) As Boolean


        Try
            outVal = 0

            Dim mi As Boolean = False
            If inVal < 0 Then
                inVal = Math.Abs(inVal)
                mi = True
            End If

            Dim _do As Integer = CInt(System.Math.Floor(inVal / 3600))

            inVal -= _do * 3600


            Dim _hun As Integer = CInt(System.Math.Floor(inVal / 60))

            inVal -= _hun * 60

            Dim _byou As Integer = CInt(inVal * 10)

            'Dim s As String = String.Format("{0:00#},", _do) & String.Format("{0:0#},", _hun) & String.Format("{0:0#}", _byou)
            If _hun > 0 Then
                Return False
            End If

            'Dim s As String = String.Format("{0:0##}", _byou)


            If mi Then
                outVal = -_byou
            Else
                outVal = _byou
            End If

            Return True

        Catch ex As Exception

            Return False

        End Try

    End Function

    Public Shared Sub conv松下to世界度(ByVal 松下lat As Integer, ByVal 松下lon As Integer, ByRef 世界緯度lat As Double, ByRef 世界経度lon As Double)

        Dim 日本緯度lat As Double = Conv松下緯度To度(松下lat)
        Dim 日本経度lon As Double = Conv松下経度To度(松下lon)


        cls緯度経度変換.bol緯度経度変換_日本to世界(日本緯度lat, 日本経度lon, 世界緯度lat, 世界経度lon)


    End Sub

    Private Shared Function Conv松下経度To度(ByVal value As Integer) As Double

        '経度用
        If value = 0 Then
            Return 0
        End If
        Try
            Dim s As String = value.ToString

            Dim p2_keido_do As Double = CDbl(s.Substring(0, 3)) ' * 60 * 60
            Dim p2_keido_hun As Double = CDbl(CDbl(s.Substring(3, 2)) / 60)
            Dim p2_keido_byou As Double = CDbl(CDbl(s.Substring(5, 3)) / 36000)
            Dim p2_keido As Double = p2_keido_do + p2_keido_hun + p2_keido_byou

            Return p2_keido

        Catch ex As Exception

            Return -1

        End Try

    End Function
    Private Shared Function Conv松下緯度To度(ByVal value As Integer) As Double

        '緯度用
        If value = 0 Then
            Return 0
        End If

        Try

            Dim s As String = value.ToString
            ' "35 09 534"
            Dim p1_ido_do As Double = CDbl(s.Substring(0, 2)) '* 60 * 60
            Dim p1_ido_hun As Double = CDbl(CDbl(s.Substring(2, 2)) / 60)
            Dim p1_ido_byou As Double = CDbl(CDbl(s.Substring(4, 3)) / 36000)
            Dim p1_ido As Double = p1_ido_do + p1_ido_hun + p1_ido_byou

            Return p1_ido

        Catch ex As Exception

            Return -1

        End Try

    End Function
    Private Shared Function 切上げ(ByVal dValue As Double) As Double
        Dim dCoef As Double = System.Math.Pow(10, 0)

        If dValue > 0 Then
            Return System.Math.Ceiling(dValue * dCoef) / dCoef
        Else
            Return System.Math.Floor(dValue * dCoef) / dCoef
        End If
    End Function

    Private Shared Function 切捨て(ByVal dValue As Double) As Double
        Dim dCoef As Double = System.Math.Pow(10, 0)

        If dValue > 0 Then
            Return System.Math.Floor(dValue * dCoef) / dCoef
        Else
            Return System.Math.Ceiling(dValue * dCoef) / dCoef
        End If
    End Function

    Private Shared Function 四捨五入(ByVal dValue As Double) As Integer
        '
        Dim dCoef As Double = System.Math.Pow(10, 0)
        '
        If dValue > 0 Then
            Return CInt(System.Math.Floor((dValue * dCoef) + 0.5) / dCoef)
        Else
            Return CInt(System.Math.Ceiling((dValue * dCoef) - 0.5) / dCoef)
        End If
        '
    End Function

End Class
