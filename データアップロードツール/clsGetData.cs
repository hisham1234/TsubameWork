using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace データアップロードツール
{
    public class clsGetData

    {
        public string conTsubameCTI { get; set; }
        public string conMiraiDBStr { get; set; }
        public bool getData(ref string msg,string from ,string to)
        {
            var sqlobjMIRAI = new clsDataBase();

            try
            {
                if (!sqlobjMIRAI.SetbolConnection(this.conTsubameCTI, ref msg))
                {
                    msg = "アップロードに失敗しました";
                    return false;
                }

               // from = "2017-09-13 06:40:00";
               // to = "2017-09-13 06:41:00";

                var sqlStr = "select y.cd_key,YoyakuDateTime,KokLatitude,KokLongitude,AnnaiLatitude,AnnaiLongitude,AnnaiYoyakudateTime,a.AnnaiAddress,a.AnnaiBuil,y.KokAddress"
                             + " from  tempAnnai a inner join yoyaku y on a.cd_key=y.AnnaiCd_key where AnnaiLatitude is not null and AnnaiLongitude is not null and  YoyakuDateTime between '" + from +"' and '"+to +"'";
             

                var tbl = sqlobjMIRAI.GetdtTableSelectData(sqlStr, ref msg);

                if (tbl == null)
                {
                    msg = "予約データがありません";
                    return false;
                }

                if (!sqlobjMIRAI.prpbolError)
                {
                    msg = "アップロードに失敗しました";
                    return false;
                }              
               

                if (tbl.Rows.Count == 0)
                {
                    msg = "予約データがありません";
                    return false;
                }
                var errcount = 0;
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    var msgimport = "";
                    if (!insertData(tbl.Rows[i], ref msgimport))
                    {
                        msg = "アップロードに失敗しました";
                        errcount++;
                        continue;
                    }
                }
                msg = "アップロードしました";
                return true;
            }
            catch (Exception)
            {
                msg = "アップロードに失敗しました";
                return false;
            }
            finally
            {
                sqlobjMIRAI.SetEndConnection();
            }
        }

        public bool insertData(DataRow r,ref string msg )
        {
            var sqlobj = new clsDataBase();

            try
            {
                if (!sqlobj.SetbolConnection(this.conMiraiDBStr, ref msg))
                {

                    return false;
                }
                var insertStr = "";
                insertStr += " INSERT INTO [dbo].[Mirai_手動登録] ";
                insertStr += "  ( ";
                insertStr += "  [グループ]  ";
                insertStr += "  ,[有効]  ";
                insertStr += "  ,[予約日時]  ";
                insertStr += "  ,[予約Lat世界度]  ";
                insertStr += "  ,[予約Lon世界度]  ";

                insertStr += "  ,[No]  ";
                insertStr += "  ,[案内Lat世界度]  ";
                insertStr += "  ,[案内Lon世界度]  ";
                insertStr += "  ,[案内先到着日時]  ";
                insertStr += "  ,[予約住所]  ";
                insertStr += "  ,[案内住所]  ";
                insertStr += "  ,[案内名称]  ";
                insertStr += " ) VALUES ( ";

                insertStr += "   '  4  '"; //[グループ]  ";
                insertStr += "  , '  1  '";//[有効]  ";
                var YoyakuDateTime = (DateTime)r["YoyakuDateTime"];
                var AnnaiDateTime = (DateTime)r["AnnaiYoyakudateTime"];
                insertStr += " , '" + YoyakuDateTime + "'";//[予約日時]  ";

                var KokLat = ((int)r["KokLatitude"]) / 36000.0;
                var KokLng = ((int)r["KokLongitude"]) / 36000.0;
                var AnnaiLat = ((int)r["AnnaiLatitude"]) / 36000.0;
                var AnnaiLng = ((int)r["AnnaiLongitude"]) / 36000.0;
                var KokLatW = 0.0;
                var KokLngW = 0.0;
                var AnnaiLatW = 0.0;
                var AnnaiLngW = 0.0;

                if (!vb_dll.cls緯度経度変換.bol緯度経度変換_日本to世界(KokLat, KokLng, ref KokLatW, ref KokLngW))
                {
                    //messageLog("緯度経度変換に失敗しました。:lat=" + KokLat + ", lng=" + KokLng);
                    return false;
                }
                if (!vb_dll.cls緯度経度変換.bol緯度経度変換_日本to世界(AnnaiLat, AnnaiLng, ref AnnaiLatW, ref AnnaiLngW))
                {
                    //messageLog("緯度経度変換に失敗しました。:lat=" + KokLat + ", lng=" + KokLng);
                    return false;
                }
                KokLatW = Math.Round(KokLatW, 6);
                KokLngW = Math.Round(KokLngW, 6);
                AnnaiLatW = Math.Round(AnnaiLatW, 6);
                AnnaiLngW = Math.Round(AnnaiLngW, 6);
                insertStr += " , '" + KokLatW + "'";//[予約Lat世界度]  ";

                insertStr += " , '" + KokLngW + "'";//[案内住所]  ";
                insertStr += " , " + r["cd_key"];
                insertStr += " , '" + AnnaiLatW + "'";
                insertStr += " , '" + AnnaiLngW + "'";
                insertStr += " , '" + AnnaiDateTime + "'";
                insertStr += " , '" + r["KokAddress"]+ "'";
                insertStr += " , '" + r["AnnaiAddress"] + "'";
                insertStr += " , '" + r["AnnaiBuil"] + "'";
                insertStr += " )  ";
                var inserres = sqlobj.GetshoExecuteNonQuery(insertStr, ref msg);

                if (sqlobj.prpbolError == false)//error
                {
                    msg = "sqlobj.prpbolError == false";
                    return false;
                }
                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                sqlobj.SetEndConnection();
            }

        }
    }
}
