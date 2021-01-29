using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static 未来シェア.MiraiShare_SAVS_API;

namespace 未来シェア
{
    class cls設定情報
    {
        public static MiraiShareResult 車両初期位置変更_Execute(int savId,double lat,double lng,DateTime dt, ref string msg)
        {
            msg = "";
            try
            {
                Task<MiraiShareResult> r = Task.Run(async () =>
                {
                    var obj = new 車両初期位置変更_Request();
                    obj.position.lat = lat;
                    obj.position.lng = lng;
                    obj.time = dt;

                    var json = JsonConvert.SerializeObject(obj);
                    var res = await GenericPut(miraiShareURL + "sav/savs/" + savId + "/location", json);
                    return res;
                });
                Task.WaitAll(r);
                msg = "OK";
                return r.Result;//JsonConvert.DeserializeObject<デマンド登録_Response>(r.Result.receiveJson);
            }
            catch (Exception ex)
            {
                MiraiShareResult r = new MiraiShareResult();
                r.message = msg = ex.Message + ex.StackTrace;
                r.receiveJson = "";
                r.result = "";
                r.StatusCode = -1;
                r.success = false;
                msg = "ERR";
                return r;
            }
        }//1901010001

        public static MiraiShareResult 車両の無効化_Execute(int savId, bool value ,ref string msg)
        {
            msg = "";
            try
            {
                Task<MiraiShareResult> r = Task.Run(async () =>
                {
                    var obj = new 車両の無効化_Request();
                    obj.value = value;
                    var json = JsonConvert.SerializeObject(obj);
                    var res = await GenericPost(miraiShareURL + "sav/savs/" + savId + "/is_rest", json);
                    return res;
                });
                Task.WaitAll(r);
                msg = "OK";
                return r.Result;//JsonConvert.DeserializeObject<デマンド登録_Response>(r.Result.receiveJson);
            }
            catch (Exception ex)
            {
                MiraiShareResult r = new MiraiShareResult();
                r.message = msg = ex.Message + ex.StackTrace;
                r.receiveJson = "";
                r.result = "";
                r.StatusCode = -1;
                r.success = false;
                msg = "ERR";
                return r;
            }
        }

        public enum GetStatusCount
        {
            _001_to_050 = 1,
            _051_to_100 = 2,
            _101_to_150 = 3,
            _151_to_200 = 4
        }

        public static 車両ステータス_Respose 車両ステータス確認_Execute(GetStatusCount c,ref string msg)
        {
            msg = "";
            try
            {
                Task<MiraiShareResult> r = Task.Run(async () =>
                {
                    var range = (int)c * 50;
                    var res = await GenericGet(miraiShareURL + "sav/savs?page_token=" + range + "&page_size=50");
                    return res;
                });
                Task.WaitAll(r);
                msg = "OK";
                return JsonConvert.DeserializeObject<車両ステータス_Respose>(r.Result.receiveJson);
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return null;
            }
        }//1901010001

        [JsonObject]
        public class 車両初期位置変更_Request
        {
            public 車両初期位置変更_Request()
            {
                position = new Position();
            }

            public class Position
            {
                public double lat { get; set; }
                public double lng { get; set; }
            }

            [JsonProperty]
            public Position position { get; set; }

            [JsonProperty]
            public DateTimeOffset time { get; set; }
        }

        [JsonObject]
        public class 車両の無効化_Request
        {
            [JsonProperty]
            public bool value { get; set; }
        }






        [JsonObject]
        public class 車両ステータス_Respose
        {
            public class Sav
            {
                public Sav()
                {
                    position = new Position();
                    spaces = new Space[1];
                    spaces[0] = new Space();
                }

                public class Position
                {
                    public double lat { get; set; }

                    public double lng { get; set; }

                    public string retrievedAt { get; set; }
                }

                public class Space
                {
                    public string name { get; set; }
                    public int capacity { get; set; }
                }

                public int savId { get; set; }

                public int driverId { get; set; }

                public string name { get; set; }

                public bool isRest { get; set; }

                public Position position { get; set; }

                public Space[] spaces { get; set; }
            }

            [JsonProperty]
            public int pageSize { get; set; }

            [JsonProperty]
            public string nextPageToken { get; set; }

            [JsonProperty]
            public Sav[] savs { get; set; }
        }
    }
}
