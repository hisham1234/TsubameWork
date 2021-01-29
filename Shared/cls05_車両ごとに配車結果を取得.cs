using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static 未来シェア.cls06_登録したデマンドの削除;
using static 未来シェア.MiraiShare_SAVS_API;

namespace 未来シェア
{
    public class cls05_車両ごとに配車結果を取得
    {
        public static 車両ごとに配車結果を取得_Response 車両ごとに配車結果を取得_Execute(int savId, ref string msg)
        {
            msg = "";
            try
            {
                Task<MiraiShareResult> r = Task.Run(async () =>
                {
                    //var json = JsonConvert.SerializeObject(param);
                    var resGet = await GenericGet(miraiShareURL + "sav/savs/" + savId + "/route");
                    return resGet;
                });
                Task.WaitAll(r);
                msg = "OK";
                var o = JsonConvert.DeserializeObject(r.Result.receiveJson);
                //return r.Result;//JsonConvert.DeserializeObject<デマンド登録_Response>(r.Result.receiveJson);

                return JsonConvert.DeserializeObject<車両ごとに配車結果を取得_Response>(r.Result.receiveJson);

            }
            catch (Exception ex)
            {
                return null;
            }
        }//1901010001

        public class 車両ごとに配車結果を取得_Response
        {
            public class ViaPoints
            {
                public class Position
                {
                    public double lat;
                    public double lng;
                }

                public class Spaces
                {
                    public string name;
                    public int value;
                }
                public int viaPointId;
                public int passengerId;
                public int demandId;
                public string action;
                public Position position;
                public DateTimeOffset estimatedTime;
                public DateTimeOffset time;
                public DateTimeOffset latestEstimatedTime;
                public DateTimeOffset minEstimationTime;
                public DateTimeOffset maxEstimationTime;
                public Spaces[] spaces;
                public object notes;
                public bool pending;
            }

            public class Passengers
            {
                public int passengerId;
                public string name;
                public string phone;
                public object phone2;
                public object notes;
            }

            [JsonProperty("savId")]
            public int savId;

            [JsonProperty("viaPoints")]
            public ViaPoints[] viaPoints;

            [JsonProperty("passengers")]
            public Passengers[] passengers;
        }
    }
}
