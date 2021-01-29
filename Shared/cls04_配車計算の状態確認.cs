using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static 未来シェア.MiraiShare_SAVS_API;


namespace 未来シェア
{
    class cls04_配車計算の状態確認
    {
        public static 配車計算の状態確認_Response 配車計算の状態確認_Execute(int demandId, ref string msg)
        {
            msg = "";
            try
            {
                Task<MiraiShareResult> r = Task.Run(async () =>
                {
                    //var json = JsonConvert.SerializeObject(param);
                  
                   
                    
                        var resGet = await GenericGet(miraiShareURL + "sav/demands/" + demandId);
                        return resGet;
                    
                    
                });
                Task.WaitAll(r);
                msg = "OK";
                var o = JsonConvert.DeserializeObject(r.Result.receiveJson);
                //return r.Result;//JsonConvert.DeserializeObject<デマンド登録_Response>(r.Result.receiveJson);

                return JsonConvert.DeserializeObject<配車計算の状態確認_Response>(r.Result.receiveJson);

            }
            catch (Exception)
            {
                return null;
            }
        }//1901010001


        public static 配車計算の状態確認_Response GetDemand_Execute(int demandId, ref string msg)
        {
            msg = "";
            try
            {
                Task<MiraiShareResult> r = Task.Run(async () =>
                {
                    var resGet = await GenericGet(miraiShareURL + "sav/demands/" + demandId);
                    return resGet;
                });
                Task.WaitAll(r);
                msg = "OK";
                var o = JsonConvert.DeserializeObject(r.Result.receiveJson);
                //return r.Result;//JsonConvert.DeserializeObject<デマンド登録_Response>(r.Result.receiveJson);

                return JsonConvert.DeserializeObject<配車計算の状態確認_Response>(r.Result.receiveJson);

            }
            catch (Exception)
            {
                return null;
            }
        }//1901010001







































        /*
{
  "demandId":4848,
  "passengerId":1,
  "status":"PICKING_UP",
  "pickUpPosition":
  {
    "lat":35.0521903709796,
    "lng":136.984605977309
  },
  "dropOffPosition":
  {
    "lat":35.1622324325526,
    "lng":136.885997988024
  },
  "pickUpLocationName":null,
  "dropOffLocationName":null,
  "spaces":
  [
    {
      "name":"SEAT",
      "value":1
    }
  ],
  "pickUpTime":"2019-08-01T04:15:00+09:00",
  "dropOffTime":null,
  "estimatePickUpTime":"2019-08-01T04:15:00+09:00",
  "estimateDropOffTime":"2019-08-01T05:12:55+09:00",
  "latestEstimatePickUpTime":"2019-08-01T04:15:00+09:00",
  "latestEstimateDropOffTime":"2019-08-01T05:12:55+09:00",
  "shareable":false,
  "shared":false,
  "sav":
  {
    "savId":160,
    "name":"160号車",
    "position":
    {
      "lat":35.088128,
      "lng":136.963071
    }
  },
  "errorCode":null,
  "createdTime":"2019-07-17T04:53:13+09:00",
  "pricingInfo":null,
  "reason":
  {
    "code":null,
    "operatedBy":"ENGINE"
  },
  "notes":
  [
  ]
}
 */




        public class 配車計算の状態確認_Response
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

            public class Reason
            {
                public string code;
                public string operatedBy;
            }

            public 配車計算の状態確認_Response()
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


            [JsonProperty("pickUpTime")]
            public string pickUpTime { get; set; }


            [JsonProperty("dropOffTime")]
            public string dropOffTime { get; set; }


            [JsonProperty("estimatePickUpTime")]
            public string estimatePickUpTime { get; set; }


            [JsonProperty("estimateDropOffTime")]
            public string estimateDropOffTime { get; set; }


            [JsonProperty("latestEstimatePickUpTime")]
            public string latestEstimatePickUpTime { get; set; }


            [JsonProperty("latestEstimateDropOffTime")]
            public string latestEstimateDropOffTime { get; set; }


            [JsonProperty("shareable")]
            public bool shareable { get; set; }


            [JsonProperty("shared")]
            public bool shared { get; set; }


            //[JsonProperty("sav")]
            //public object sav { get; set; }


            [JsonProperty("errorCode")]
            public string errorCode { get; set; }


            [JsonProperty("createdTime")]
            public string createdTime { get; set; }


            [JsonProperty("pricingInfo")]
            public string pricingInfo { get; set; }


            [JsonProperty("notes")]
            public object notes { get; set; }


            [JsonProperty("pickUpLocationName")]
            public string pickUpLocationName { get; set; }


            public string dropOffLocationName { get; set; }


            public class Sav
            {
                public int savId { get; set; }
                public string name { get; set; }
                public Position position { get; set; }
                public Sav()
                {
                    position = new Position();
                }
            }

            [JsonProperty("sav")]
            public Sav sav { get; set; }

            public Reason reason { get; set; }
        }
    }
}
