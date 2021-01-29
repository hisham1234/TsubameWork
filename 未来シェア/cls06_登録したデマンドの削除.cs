using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static 未来シェア.MiraiShare_SAVS_API;

namespace 未来シェア
{
    class cls06_登録したデマンドの削除
    {
        public static MiraiShareResult 登録したデマンドの削除_Execute(int demandId, ref string msg)
        {
            msg = "";
            try
            {
                登録したデマンドの削除_Request req = new 登録したデマンドの削除_Request();
                req.reasonCode = "finished";

                var json = JsonConvert.SerializeObject(req);
                var res = GenericDelete(miraiShareURL + "sav/demands/" + demandId, json);

                msg = "OK";
                return res;
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

        public static 状態確認_Response 状態確認_Execute(ref string msg)
        {
            msg = "";
            try
            {
                Task<MiraiShareResult> r = Task.Run(async () =>
                {
                    var resGet = await GenericGet(miraiShareURL + "sav/demands?status=PENDING%2CASSIGNED%2CPICKING_UP%2CBOARDING&page_size=50");
                    return resGet;
                    //
                });
                Task.WaitAll(r);
                msg = "OK";
                var o = JsonConvert.DeserializeObject(r.Result.receiveJson);
                //return r.Result;//JsonConvert.DeserializeObject<デマンド登録_Response>(r.Result.receiveJson);

                return JsonConvert.DeserializeObject<状態確認_Response>(r.Result.receiveJson);

            }
            catch (Exception ex)
            {
                return null;
            }
        }//1901010001

        [JsonObject]
        public class 登録したデマンドの削除_Request
        {
            [JsonProperty("reasonCode")]
            public string reasonCode { get; set; }
        }


        public class 状態確認_Response
        {
            public class Demands
            {
                public Demands()
                {
                    pickUpPosition = new Position();
                    dropOffPosition = new Position();
                    reason = new Reason();
                    sav = new Sav();
                    spaces = new Spaces[1];
                    spaces[0] = new Spaces();
                }

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
                public int demandId { get; set; }
                public int passengerId { get; set; }
                public string status { get; set; }
                public Position pickUpPosition { get; set; }
                public Position dropOffPosition { get; set; }
                public Spaces[] spaces { get; set; }
                public string pickUpTime { get; set; }
                public string dropOffTime { get; set; }
                public string estimatePickUpTime { get; set; }
                public string estimateDropOffTime { get; set; }
                public string latestEstimatePickUpTime { get; set; }
                public string latestEstimateDropOffTime { get; set; }
                public bool shareable { get; set; }
                public bool shared { get; set; }

                public class Sav
                {
                    public Sav()
                    {
                        position = new Position();
                    }
                    public int savId;
                    public string name;
                    public Position position;
                }

                public Sav sav { get; set; }

                public string errorCode;
                public string createdTime;
                public string pricingInfo;

                public class Reason
                {
                    string code;
                    string operatedBy;
                }

                public Reason reason;
                public object notes;
            }


            public 状態確認_Response()
            {
                //demands = new Demands();
            }

            [JsonProperty("pageSize")]
            public int pageSize { get; set; }


            [JsonProperty("nextPageToken")]
            public string nextPageToken { get; set; }


            [JsonProperty("demands")]
            public Demands[] demands { get; set; }
        }
        

        //[JsonObject]
        //public class 状態確認_Response__old
        //{

        //    public class Position
        //    {
        //        public double lat { get; set; }
        //        public double lng { get; set; }
        //    }

        //    public class Spaces
        //    {
        //        public string name { get; set; }
        //        public int value { get; set; }
        //    }

        //    public class Reason
        //    {
        //        public object code;

        //        public string operatedBy;
        //    }


        //    public 状態確認_Response()
        //    {
        //        pickUpPosition = new Position();
        //        dropOffPosition = new Position();
        //    }


        //    [JsonProperty("demandId")]
        //    public int demandId { get; set; }


        //    [JsonProperty("passengerId")]
        //    public int passengerId { get; set; }


        //    [JsonProperty("status")]
        //    public string status { get; set; }


        //    [JsonProperty("pickUpPosition")]
        //    public Position pickUpPosition { get; set; }


        //    [JsonProperty("dropOffPosition")]
        //    public Position dropOffPosition { get; set; }


        //    [JsonProperty("spaces")]
        //    public Spaces[] spaces { get; set; }


        //    [JsonProperty("pickUpTime")]
        //    public string pickUpTime { get; set; }


        //    [JsonProperty("dropOffTime")]
        //    public string dropOffTime { get; set; }


        //    [JsonProperty("estimatePickUpTime")]
        //    public string estimatePickUpTime { get; set; }


        //    [JsonProperty("estimateDropOffTime")]
        //    public string estimateDropOffTime { get; set; }


        //    [JsonProperty("latestEstimatePickUpTime")]
        //    public string latestEstimatePickUpTime { get; set; }


        //    [JsonProperty("latestEstimateDropOffTime")]
        //    public string latestEstimateDropOffTime { get; set; }


        //    [JsonProperty("shareable")]
        //    public bool shareable { get; set; }


        //    [JsonProperty("shared")]
        //    public bool shared { get; set; }


        //    [JsonProperty("sav")]
        //    public object sav { get; set; }


        //    [JsonProperty("errorCode")]
        //    public string errorCode { get; set; }


        //    [JsonProperty("createdTime")]
        //    public string createdTime { get; set; }


        //    [JsonProperty("pricingInfo")]
        //    public object pricingInfo { get; set; }


        //    [JsonProperty("notes")]
        //    public object notes { get; set; }
        //}
    }
}
