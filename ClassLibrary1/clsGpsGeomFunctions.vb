Imports System.Math

Public Class clsGpsGeomFunctions

    Public Shared Sub GetDistance(ByVal ido1 As Double, ByVal keido1 As Double, ByVal ido2 As Double, ByVal keido2 As Double, ByRef r2 As Double, ByRef s2 As Double)
        Dim f1 As Double = 0.0                                   '     /* �Q�n�_�̈ܓx(��) */
        Dim f2 As Double = 0.0                                   '    /* �Q�n�_�̈ܓx(��) */
        Dim fr1 As Double = 0.0                                  '   /* �Q�n�_�̈ܓx(rad) */ 
        Dim fr2 As Double = 0.0                                  '   /* �Q�n�_�̈ܓx(rad) */ 
        Dim g1 As Double = 0.0                                   '   /* �Q�n�_�̌o�x�i���o��j(��) */ 
        Dim g2 As Double = 0.0                                   '   /* �Q�n�_�̌o�x�i���o��j(��) */ 
        Dim gr1 As Double = 0.0                                  ' /* �Q�n�_�̌o�x�i���o��j(rad) */
        Dim gr2 As Double = 0.0                                  ' /* �Q�n�_�̌o�x�i���o��j(rad) */
        Dim h1 As Double = 0.0                                   '  /* �W��(m) */
        Dim h2 As Double = 0.0                                   '  /* �W��(m) */
        Dim a As Double = 6378136.0                            '/* �ԓ����a(m) */ 
        Dim e2 As Double = 0 '0.00669447                       '/* �n���̗��S���̎��� */ 
        Dim x1 As Double = 0.0                       '/* �Q�n�_�̒������W�l(m) */
        Dim y1 As Double = 0.0                       '/* �Q�n�_�̒������W�l(m) */
        Dim z1 As Double = 0.0                       '/* �Q�n�_�̒������W�l(m) */
        Dim x2 As Double = 0.0                       '/* �Q�n�_�̒������W�l(m) */
        Dim y2 As Double = 0.0                       '/* �Q�n�_�̒������W�l(m) */
        Dim z2 As Double = 0.0                       '/* �Q�n�_�̒������W�l(m) */
        Dim r As Double = 0.0                         '                  /* �Q�n�_�Ԃ̒�����(m) */ 
        Dim s As Double = 0.0                         '                 /* �Q�n�_�Ԃ̒n�\�ʋ���(m) */ 
        Dim w As Double = 0.0                         '                /* �Q�n�_�Ԃ̔��˒��p(��) (���S�p�̂P�^�Q) */ 
        Dim wr As Double = 0.0                        '                /* �Q�n�_�Ԃ̔��˒��p(rad) */ 
        Dim rad As Double = 0.0                       '               /* �x�����W�A���ϊ��W�� */
        Dim N1 As Double = 0.0                        '           /* �ܓx�␳�����n���̔��a(m) */
        Dim N2 As Double = 0.0                        '           /* �ܓx�␳�����n���̔��a(m) */


        Try
            rad = PI / 180.0

            h1 = 0.0     '                                  /* �����ł́A�W���𖳎� */ 
            h2 = 0.0 '                                      /* �����ł́A�W���𖳎� */ 


            '//����GPS�̈ܓx�o�x����x�P�ʂɕϊ�����-------------------------------------------
            'Dim p1_ido_do As Double = CDbl(ido1.Substring(0, 2))
            'Dim p1_ido_hun As Double = CDbl(CDbl(ido1.Substring(2, 2)) / 60)
            'Dim p1_ido_byou As Double = CDbl(CDbl(ido1.Substring(4, 3)) / 60 / 60 / 10)
            Dim p1_ido As Double = ido1

            'Dim p2_ido_do As Double = CDbl(ido2.Substring(0, 2))
            'Dim p2_ido_hun As Double = CDbl(CDbl(ido2.Substring(2, 2)) / 60)
            'Dim p2_ido_byou As Double = CDbl(CDbl(ido2.Substring(4, 3)) / 60 / 60 / 10)
            Dim p2_ido As Double = ido2

            'Dim p1_keido_do As Double = CDbl(keido1.Substring(0, 3))
            'Dim p1_keido_hun As Double = CDbl(CDbl(keido1.Substring(3, 2)) / 60)
            'Dim p1_keido_byou As Double = CDbl(CDbl(keido1.Substring(5, 3)) / 60 / 60 / 10)
            Dim p1_keido As Double = keido1

            'Dim p2_keido_do As Double = CDbl(keido2.Substring(0, 3))
            'Dim p2_keido_hun As Double = CDbl(CDbl(keido2.Substring(3, 2)) / 60)
            'Dim p2_keido_byou As Double = CDbl(CDbl(keido2.Substring(5, 3)) / 60 / 60 / 10)
            Dim p2_keido As Double = keido2
            '------------------------------------------------------------------------------------

            'printf("��P�n�_�̈ܓx�A�o�x(��,���o�͕�����)����͂��ĉ������B--->");
            'scanf("%lf %lf",&f1,&g1); 
            f1 = p1_ido
            g1 = p1_keido

            If g1 < 0 Then
                g1 = 360.0 + g1
            End If
            fr1 = f1 * rad
            gr1 = g1 * rad

            'printf("��Q�n�_�̈ܓx�A�o�x(��,���o�͕�����)����͂��ĉ������B--->"); 
            'scanf("%lf %lf",&f2,&g2); 
            f2 = p2_ido
            g2 = p2_keido

            If g2 < 0 Then
                g2 = 360.0 + g2
            End If
            fr2 = f2 * rad
            gr2 = g2 * rad

            N1 = a / (Sqrt(1.0 - e2 * Sin(fr1) * Sin(fr1)))
            x1 = (N1 + h1) * Cos(fr1) * Cos(gr1)
            y1 = (N1 + h1) * Cos(fr1) * Sin(gr1)
            z1 = (N1 * (1.0 - e2) + h1) * Sin(fr1)
            N2 = a / (Sqrt(1.0 - e2 * Sin(fr2) * Sin(fr2)))
            x2 = (N2 + h2) * Cos(fr2) * Cos(gr2)
            y2 = (N2 + h2) * Cos(fr2) * Sin(gr2)
            z2 = (N2 * (1.0 - e2) + h2) * Sin(fr2)
            r = Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2) + (z1 - z2) * (z1 - z2)) ';      /* ������ */ 
            wr = Asin(r / 2 / a) ';                                                               /* ���˒��p(rad) */
            w = wr / rad ';                                                                      /* ���˒��p(��) */ 
            s = a * 2 * wr ';                                                                      /* �n�\�ʋ��� */   
            'printf("��P�n�_�̈ܓx�F%lf �o�x�F%lf\n",f1,g1); 

            'printf("��Q�n�_�̈ܓx�F%lf �o�x�F%lf\n",f2,g2); 

            'printf("���@���@�� �F%9.3lf km\n",r/1000);
            'printf("�n�\�ʋ��� �F%9.3lf km\n",s/1000); 
            'printf("�� �� �� �p�F%8.4lf ��\n",w); 

            r2 = r
            s2 = s

            'Return s / 1000
            'Return w
        Catch ex As Exception

        End Try

    End Sub

    'function geoDirection(lat1, lng1, lat2, lng2) {
    '  // �ܓx�o�x lat1, lng1 �̓_���o���Ƃ��āA�ܓx�o�x lat2, lng2 �ւ̕���
    '  // �k���O�x�ŉE���̊p�x�O�`�R�U�O�x
    '  var Y = Math.cos(lng2 * Math.PI / 180) * Math.sin(lat2 * Math.PI / 180 - lat1 * Math.PI / 180);
    '  var X = Math.cos(lng1 * Math.PI / 180) * Math.sin(lng2 * Math.PI / 180) - Math.sin(lng1 * Math.PI / 180) * Math.cos(lng2 * Math.PI / 180) * Math.cos(lat2 * Math.PI / 180 - lat1 * Math.PI / 180);
    '  var dirE0 = 180 * Math.atan2(Y, X) / Math.PI; // ���������O�x�̕���
    '  if (dirE0 < 0) {
    '    dirE0 = dirE0 + 360; //0�`360 �ɂ���B
    '  }
    '  var dirN0 = (dirE0 + 90) % 360; //(dirE0+90)��360�̗]����o�� �k�������O�x�̕���
    '  return dirN0;
    '}

    Public Shared Function geoDirection(ByVal lat1 As Double, ByVal lng1 As Double, ByVal lat2 As Double, ByVal lng2 As Double) As Double

        Dim Y As Double = Math.Cos(lng2 * Math.PI / 180) * Math.Sin(lat2 * Math.PI / 180 - lat1 * Math.PI / 180)
        Dim X As Double = Math.Cos(lng1 * Math.PI / 180) * Math.Sin(lng2 * Math.PI / 180) - Math.Sin(lng1 * Math.PI / 180) * Math.Cos(lng2 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180 - lat1 * Math.PI / 180)

        Dim dirE0 As Double = 180 * Math.Atan2(Y, X) / Math.PI '// ���������O�x�̕���
        If dirE0 < 0 Then
            dirE0 = dirE0 + 360 '//0�`360 �ɂ���B
        End If

        Dim dirN0 As Double = (dirE0 + 90) Mod 360 '//(dirE0+90)��360�̗]����o�� �k�������O�x�̕���
        Return dirN0

    End Function

End Class

Public Class ComputeFromDir

    Public Shared Function ComputeFormDir(ByVal ���E�ܓxLat�x As Double, ByVal ���E�o�xLng�x As Double, ByVal ���� As Double, ByVal �p�x As Double, ByRef lat2 As Double, ByRef lng2 As Double) As Boolean
        '���͈ܓx�o�x�ɋ����Ɗp�x���w�肵�ړI�n�ܓx�o�x���擾����
        '���E�ܓxLat�x�@�@�@���E���n�n�i�x�j
        '���E�o�xLng�x�@�@�@���E���n�n�i�x�j
        '�����@(���[�g���j
        '�p�x �x

        'lat 35.164781
        'lng 136.894991
        '45   0.5km  WGS84/NAD83/GRS80
        Try
            ���� /= 1000
            '35.167967722054996
            '136.89887188487526
            '//get select  values
            Dim signlat1, signlon1, dc As Double
            Dim lat1, lon1 As Double
            Dim d12, crs12 As Double
            'Dim a, invf As Double
            '/* Input and validate data */
            Dim signlatlon_NS1 As Integer = 1
            Dim signlatlon_EW1 As Integer = -1
            signlat1 = signlatlon_NS1
            signlon1 = signlatlon_EW1
            lat1 = (Math.PI / 180) * signlat1 * ���E�ܓxLat�x
            lon1 = (Math.PI / 180) * signlon1 * ���E�o�xLng�x
            d12 = ���� 'document.InputFormDir.d12.value
            dc = 1.852 '//km//dconv(document.OutputFormDir.Dunit)
            crs12 = �p�x * Math.PI / 180 '.  // radians
            '"WGS84",6378.137/1.852,298.257223563
            'ellipse=getEllipsoid(document.OutputFormDir.Model) //get ellipse
            Dim ret_lat, ret_lon As Double
            If direct_ell(lat1, -lon1, crs12, d12, ret_lat, ret_lon) Then
                lat2 = ret_lat * (180 / Math.PI)
                lng2 = ret_lon * (180 / Math.PI)                  '// ellipse uses East negative
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Shared Function direct_ell(ByVal glat1 As Double, ByVal glon1 As Double, ByVal faz As Double, ByVal s As Double, ByRef lat2 As Double, ByRef lng2 As Double) As Boolean
        Try
            Dim EPS As Double = 0.00000000005
            Dim r, tu, sf, cf, b, cu, su, sa, c2a, x, c, d, y, sy, cy, cz, E As Double
            Dim glat2, glon2, baz, f As Double
            If ((Math.Abs(Math.Cos(glat1)) < EPS) And Not (Math.Abs(Math.Sin(faz)) < EPS)) Then
                'alert("Only N-S courses are meaningful, starting at a pole!")
                Return False
            End If
            Dim a As Double = 6378.137 / 1
            f = 1 / 298.257223563
            r = 1 - f
            tu = r * Math.Tan(glat1)
            sf = Math.Sin(faz)
            cf = Math.Cos(faz)
            If cf = 0 Then
                b = 0.0
            Else
                b = 2.0 * Atan2(tu, cf)
            End If
            cu = 1.0 / Math.Sqrt(1 + tu * tu)
            su = tu * cu
            sa = cu * sf
            c2a = 1 - sa * sa
            x = 1.0 + Math.Sqrt(1.0 + c2a * (1.0 / (r * r) - 1.0))
            x = (x - 2.0) / x
            c = 1.0 - x
            c = (x * x / 4.0 + 1.0) / c
            d = (0.375 * x * x - 1.0) * x
            tu = s / (r * a * c)
            y = tu
            c = y + 1
            While Math.Abs(y - c) > EPS
                sy = Math.Sin(y)
                cy = Math.Cos(y)
                cz = Math.Cos(b + y)
                E = 2.0 * cz * cz - 1.0
                c = y
                x = E * cy
                y = E + E - 1.0
                y = (((sy * sy * 4.0 - 3.0) * y * cz * d / 6.0 + x) * d / 4.0 - cz) * sy * d + tu
            End While
            b = cu * cy * cf - su * sy
            c = r * Math.Sqrt(sa * sa + b * b)
            d = su * cy + cu * sy * cf
            glat2 = modlat(Atan2(d, c))
            c = cu * cy - su * sy * cf
            x = Atan2(sy * sf, c)
            c = ((-3.0 * c2a + 4.0) * f + 4.0) * c2a * f / 16.0
            d = ((E * cy * c + cz) * sy * c + y) * sa
            glon2 = modlon(glon1 + x - (1.0 - c) * d * f)
            baz = modcrs(Atan2(sa, b) + Math.PI)
            lat2 = glat2
            lng2 = glon2
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Shared Function modlon(ByVal x As Double) As Double
        Return mod_(x + Math.PI, 2 * Math.PI) - Math.PI
    End Function
    Private Shared Function modlat(ByVal x As Double) As Double
        Return mod_(x + Math.PI / 2, 2 * Math.PI) - Math.PI / 2
    End Function
    Private Shared Function mod_(ByVal x As Double, ByVal y As Double) As Double
        Return x - y * Math.Floor(x / y)
    End Function
    Private Shared Function modcrs(ByVal x As Double) As Double
        Return mod_(x, 2 * Math.PI)
    End Function
End Class