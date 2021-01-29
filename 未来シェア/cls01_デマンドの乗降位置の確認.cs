using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static 未来シェア.MiraiShare_SAVS_API;

namespace 未来シェア
{
    public class cls01_デマンドの乗降位置の確認
    {
        public static デマンドの乗降位置の確認_Response デマンドの乗降位置の確認_Execute(double lat, double lon, ref string msg)
        {
            msg = "";
            try
            {
                Task<MiraiShareResult> r = Task.Run(() =>
                {
                    PointCheck point = new PointCheck();
                    point.latitude = lat;
                    point.longitude = lon;
                    var json = JsonConvert.SerializeObject(point);
                    var res = GenericPost(miraiShareURL + "sav/geo/point", json);
                    return res;
                });
                Task.WaitAll(r);
                msg = "OK";
                return JsonConvert.DeserializeObject<デマンドの乗降位置の確認_Response>(r.Result.receiveJson);
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return null;
            }
        }

        public static bool 車両有効無効(bool enable,int num, ref string msg)
        {
            msg = "";
            try
            {
                var json = "";
                if (enable)
                {
                    json = "{\"value\": false }";
                }
                else
                {
                    json = "{\"value\": true }";
                }
                //
                //{ "value": true }
                Task<MiraiShareResult> r = Task.Run(() =>
                {
                    var res = GenericPost(miraiShareURL + "sav/savs/" + num + "/is_rest", json);
                    return res;
                });
                Task.WaitAll(r);
                msg = "OK";
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return false;
            }
        }


        [JsonObject]
        public class PointCheck //デマンドの乗降位置の確認
        {
            [JsonProperty("latitude")]
            public double latitude { get; set; }

            [JsonProperty("longitude")]
            public double longitude { get; set; }
        }

        [JsonObject]
        public class デマンドの乗降位置の確認_Response
        {
            [JsonProperty("result")]
            public bool result { get; set; }

            [JsonProperty("code")]
            public string code { get; set; }

            [JsonProperty("message")]
            public string message { get; set; }
        }
    }
}
