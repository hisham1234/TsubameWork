using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DirectioinAPI
{
    class APIAccessCls
    {
        public static DirectionAPIResponse APIExecute(string url)
        {
            var d = new DirectionAPICls();

            try
            {
                Task<MiraiShareResult> r = Task.Run(() =>
                {
                    //var json = JsonConvert.SerializeObject(param);
                    var res = d.GenericGet(url);
                    return res;
                });
                Task.WaitAll(r);
                //msg = "OK";
                var o = JsonConvert.DeserializeObject<DirectionAPIResponse>(r.Result.receiveJson);
                o.isSuccess = true;
                return o;
            }
            catch (Exception ex)
            {
                //msg = ex.Message + ex.StackTrace;
                return new DirectionAPIResponse() { isSuccess = false, Message = ex.Message + ex.StackTrace };
            }
        }
    }

    public class DirectionParam
    {
        public string origin { get; set; }
        public string destination { get; set; }

        public bool IsHighwayEnable { get; set; }


    }
    public class ResponseSummary
    {
        public string DurationRoute1 { get; set; }
        public string DistanceInMeters1 { get; set; }
        public string DurationRoute2 { get; set; }
        public string DistanceInMeters2 { get; set; }
        public string Steps1 { get; set; }
        public string Steps2 { get; set; }

        public string RouteOneStatus { get; set; }

        public string RouteTwoStatus { get; set; }
        public string StartAddress { get; set; }
        public string EndAddress { get; set; }

        public string OrverviewPolyline1 { get; set; } // 2020/3/11 hirose 追加
        public string OrverviewPolyline2 { get; set; } // 2020/3/11 hirose 追加

        public bool isSuccess;// 2020/3/11 hirose 追加
        public string Message;// 2020/3/11 hirose 追加
    }
    public class DirectionAPIResponse
    {
        public class Geocoded_waypoints
        {
            public string geocoder_status { get; set; }
            public string place_id { get; set; }
            public string[] types { get; set; }

        }

        public class Routes
        {
            public class Northeast
            {
                public double lat { get; set; }
                public double lng { get; set; }
            }

            public class Southwest
            {
                public double lat { get; set; }
                public double lng { get; set; }
            }

            //public class Bounds
            //{
            //    public Northeast northeast { get; set; }
            //    public Southwest southwest { get; set; }
            //}
            //public Bounds bounds { get; set; }
            public string copyrights { get; set; }
            public class Legs
            {
                public class Distance
                {
                    public string text { get; set; }
                    public int value { get; set; }
                }
                public Distance distance { get; set; }
                public class Duration
                {
                    public string text { get; set; }
                    public int value { get; set; }
                }
                public Duration duration { get; set; }
                public string end_address { get; set; }
                public class end_location
                {
                    public double lat { get; set; }
                    public double lng { get; set; }
                }

                public string start_address { get; set; }
                public class start_location
                {
                    public double lat { get; set; }
                    public double lng { get; set; }
                }
                public class Steps
                {
                    public class Distance
                    {
                        public string text { get; set; }
                        public int value { get; set; }
                    }

                    public class Duration
                    {
                        public string text { get; set; }
                        public int value { get; set; }
                    }
                    public class End_location
                    {
                        public double lat { get; set; }
                        public double lng { get; set; }
                    }
                    public string Html_instructions { get; set; }
                    public class polyline { public string points { get; set; } }
                    public class Start_location
                    {
                        public double lat { get; set; }
                        public double lng { get; set; }
                    }
                    public Start_location start_location { get; set; }
                    public End_location end_location { get; set; }
                    public string Travel_mode { get; set; }
                }
                public IList<Steps> steps { get; set; }
                public string[] traffic_speed_entry { get; set; }
                public string[] via_waypoint { get; set; }
            }

            public IList<Legs> legs { get; set; }
            public class Overview_polyline // 2020/3/11 hirose  名前変更
            {
                public string points { get; set; }
            }

            public Overview_polyline overview_polyline { get; set; } // 2020/3/11 hirose　追加

            public string summary { get; set; }
            public string[] warnings { get; set; }
            public string[] waypoint_order { get; set; }
        }

        [JsonProperty("geocoded_waypoints")]
        public Geocoded_waypoints[] geocoded_waypoints { get; set; }


        [JsonProperty("routes")]
        public Routes[] routes { get; set; }

        //[JsonProperty("legs")]
        //public legs legs { get; set; }

        public bool isSuccess { get; set; }// 2020/3/11 hirose

        public string Message { get; set; }// 2020/3/11 hirose
    }
}
