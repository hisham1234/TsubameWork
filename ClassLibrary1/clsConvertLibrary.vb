Public Class clsConvertLibrary

    Public Shared Function �x���b���~���b�ɕϊ�(Lat_�x���b As Integer, Lon_�x���b As Integer, ByRef Lat_�~���b As Integer, ByRef Lon_�~���b As Integer) As Boolean
        Try

            Lat_�~���b = 0
            Lon_�~���b = 0
            Dim ���{�ܓxlat As Double = Conv�����ܓxTo�x(Lat_�x���b)
            Dim ���{�o�xlon As Double = Conv�����o�xTo�x(Lon_�x���b)
            Lat_�~���b = CInt(���{�ܓxlat * 36000)
            Lon_�~���b = CInt(���{�o�xlon * 36000)

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function



    Private Shared Function Conv�o�xTo�����b(ByVal value As Integer) As Double

        '�o�x�p
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

    Private Shared Function Conv�ܓxTo�����b(ByVal value As Integer) As Double

        '�ܓx�p
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

    Private Sub proc(ByVal in_����ido_1 As Integer, ByVal in_����keido_1 As Integer, ByVal in_����ido_2 As Integer, ByVal in_����keido_2 As Integer, ByRef out_ido_spn As String, ByRef out_keido_spn As String)


        If in_����ido_1 = 0 Or in_����keido_1 = 0 Or in_����ido_2 = 0 Or in_����keido_2 = 0 Then
            out_ido_spn = " 000"
            out_keido_spn = " 000"
            Return
        End If

        Dim ido_1 As Double = Conv�ܓxTo�����b(in_����ido_1)
        Dim keido_1 As Double = Conv�o�xTo�����b(in_����keido_1)

        Dim ido_2 As Double = Conv�ܓxTo�����b(in_����ido_2)
        Dim keido_2 As Double = Conv�o�xTo�����b(in_����keido_2)

        Dim spn�ܓx As Double = ido_2 - ido_1
        Dim spn�o�x As Double = keido_2 - keido_1

        Dim ido_spn As Integer = 0
        Dim keido_spn As Integer = 0

        out_ido_spn = ""
        out_keido_spn = ""

        If Not Conv�����bToInt(spn�ܓx, ido_spn) Then
            Throw New Exception
        End If
        If Not Conv�����bToInt(spn�o�x, keido_spn) Then
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


    Private Function Conv�����bToInt(ByVal inVal As Double, ByRef outVal As Integer) As Boolean


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

    Public Shared Sub conv����to���E�x(ByVal ����lat As Integer, ByVal ����lon As Integer, ByRef ���E�ܓxlat As Double, ByRef ���E�o�xlon As Double)

        Dim ���{�ܓxlat As Double = Conv�����ܓxTo�x(����lat)
        Dim ���{�o�xlon As Double = Conv�����o�xTo�x(����lon)


        cls�ܓx�o�x�ϊ�.bol�ܓx�o�x�ϊ�_���{to���E(���{�ܓxlat, ���{�o�xlon, ���E�ܓxlat, ���E�o�xlon)


    End Sub

    Private Shared Function Conv�����o�xTo�x(ByVal value As Integer) As Double

        '�o�x�p
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
    Private Shared Function Conv�����ܓxTo�x(ByVal value As Integer) As Double

        '�ܓx�p
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
    Private Shared Function �؏グ(ByVal dValue As Double) As Double
        Dim dCoef As Double = System.Math.Pow(10, 0)

        If dValue > 0 Then
            Return System.Math.Ceiling(dValue * dCoef) / dCoef
        Else
            Return System.Math.Floor(dValue * dCoef) / dCoef
        End If
    End Function

    Private Shared Function �؎̂�(ByVal dValue As Double) As Double
        Dim dCoef As Double = System.Math.Pow(10, 0)

        If dValue > 0 Then
            Return System.Math.Floor(dValue * dCoef) / dCoef
        Else
            Return System.Math.Ceiling(dValue * dCoef) / dCoef
        End If
    End Function

    Private Shared Function �l�̌ܓ�(ByVal dValue As Double) As Integer
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
