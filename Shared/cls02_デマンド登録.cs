using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static 未来シェア.MiraiShare_SAVS_API;

namespace 未来シェア
{
    public class cls02_デマンド登録
    {
        public static デマンド登録_Response デマンド登録_Execute(デマンド登録_Request param, ref string msg)
        {
            msg = "";
            try
            {
                Task<MiraiShareResult> r = Task.Run(() =>
                {
                    var json = JsonConvert.SerializeObject(param);
                    var res = GenericPost(miraiShareURL + "sav/demands", json);
                    return res;
                });
                Task.WaitAll(r);
                msg = "OK";
                return JsonConvert.DeserializeObject<デマンド登録_Response>(r.Result.receiveJson);
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return null;
            }
        }

        [JsonObject]
        public class デマンド登録_Request //デマンド登録
        {
            public class Position
            {
                public double lat { get; set; }
                public double lng { get; set; }
            }

            public class Spaces
            {
                public string name { get; set; }
                public int value { get; set; }
            }

            public デマンド登録_Request()
            {
                pickUpPosition = new Position();
                dropOffPosition = new Position();
                spaces = new Spaces[1];
                spaces[0] = new Spaces();
            }

            [JsonProperty("passengerId")]
            public int passengerId { get; set; }

            [JsonProperty("pickUpPosition")]
            public Position pickUpPosition { get; set; }

            [JsonProperty("dropOffPosition")]
            public Position dropOffPosition { get; set; }

            [JsonProperty("spaces")]
            public Spaces[] spaces { get; set; }

            [JsonProperty("shareable")]
            public bool shareable { get; set; }

            [JsonProperty("pickUpTime")]
            //public DateTimeOffset pickUpTime { get; set; }
            public string pickUpTime { get; set; }

            [JsonProperty("dropOffTime")]
            //public DateTimeOffset dropOffTime { get; set; }
            public string dropOffTime { get; set; }
        }

        [JsonObject]
        public class デマンド登録_Response //レスポンス
        {
            public class Position
            {
                public double lat { get; set; }
                public double lng { get; set; }
            }

            public class Spaces
            {
                public string name { get; set; }
                public int value { get; set; }
            }

            public デマンド登録_Response()
            {
                pickUpPosition = new Position();
                dropOffPosition = new Position();
            }

            [JsonProperty("demandId")]
            public int demandId { get; set; }

            [JsonProperty("passengerId")]
            public int passengerId { get; set; }

            [JsonProperty("status")]
            public string status { get; set; }

            [JsonProperty("pickUpPosition")]
            public Position pickUpPosition { get; set; }


            [JsonProperty("dropOffPosition")]
            public Position dropOffPosition { get; set; }

            [JsonProperty("spaces")]
            public Spaces[] spaces { get; set; }


            //DateTimeOffset m_pickUpTime;
            string m_pickUpTime;
            [JsonProperty("pickUpTime")]
            //public string pickUpTime
            public string pickUpTime {
                get
                {
                    return m_pickUpTime;
                }
                set
                {
                    if(value == null)
                    {
                        m_pickUpTime = "1900-01-01 00:00:00";
                    }else
                    {
                        m_pickUpTime = value;
                    }
                    
                }
            }

            [JsonProperty("dropOffTime")]
            //public DateTimeOffset dropOffTime { get; set; }
            public string dropOffTime { get; set; }
        }

    }
}
