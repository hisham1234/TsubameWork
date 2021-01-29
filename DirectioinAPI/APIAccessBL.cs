using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectioinAPI
{
    class APIAccessBL
    {
        const string APIKey = "AIzaSyD8aUoYHUb9GL47BIVhPYrF88ijEZgNPtI";
        const string DirectionAPIUrl = "https://maps.googleapis.com/maps/api/directions/json?alternatives=true&language=ja&";

        public ResponseSummary APIGetRoutes(DirectionParam param)
        {
            try
            {
                var res = new ResponseSummary();
                var url = DirectionAPIUrl + "origin=" + param.origin + "&destination=" + param.destination + ((param.IsHighwayEnable) ? "" : "&avoid=tolls|highways") + "&key=" + APIKey;

                var r = APIAccessCls.APIExecute(url);
                var steps1 = "";
                var steps2 = "";

                if (r == null)
                {
                    return new ResponseSummary { isSuccess = 0, Message = "APIExecute = null" };
                }
                if (!r.isSuccess)
                {
                    return new ResponseSummary { isSuccess = 0, Message = r.Message };
                }

                var r1 = r.routes[0].legs[0].steps;
                int i = 1;
                foreach (var m in r1)
                {

                    steps1 += m.start_location.lat.ToString() + "," + m.start_location.lng.ToString() + "/" + m.end_location.lat.ToString() + "," + m.end_location.lng.ToString() + ((i < r1.Count) ? "/" : "");
                    i++;
                }

                var IsHighwayAvailable1 = false;

                res.RouteOneStatus = "一般道路";
                foreach (var m in r1)
                {
                    var IsHighWay = m.Html_instructions.Contains("高速");
                    var IsToll = m.Html_instructions.Contains("有料");

                    if (IsHighWay && IsToll)
                    {
                        IsHighwayAvailable1 = true;
                        res.RouteOneStatus = "高速道路";
                        break;
                    }
                }

                res.OrverviewPolyline1 = GooglePoints.Decode(r.routes[0].overview_polyline.points);//  2020/3/11 hirose 追加
                res.DistanceInMeters1 = r.routes[0].legs[0].distance.value.ToString();
                res.Steps1 = steps1;
                res.DurationRoute1 = r.routes[0].legs[0].duration.text.ToString();               
             
                res.Summary1 = ((IsHighwayAvailable1? r.routes[0].summary.ToString():""));
                res.isSuccess = 1;

                if (r.routes.Length > 1)
                {
                    var r2 = r.routes[1].legs[0].steps;
                int j = 1;
                foreach (var m in r2)
                {

                    steps2 += m.start_location.lat.ToString() + "," + m.start_location.lng.ToString() + "/" + m.end_location.lat.ToString() + "," + m.end_location.lng.ToString() + ((j < r2.Count) ? "/" : "");
                    j++;
                }
              
                var IsHighwayAvailable2 = false;
               
                    res.RouteTwoStatus = "一般道路";
                    foreach (var m in r2)
                    {
                        var IsHighWay = m.Html_instructions.Contains("高速");
                        var IsToll = m.Html_instructions.Contains("有料");

                        if (IsHighWay && IsToll)
                        {
                            IsHighwayAvailable2 = true;
                            res.RouteTwoStatus = "高速道路";
                            break;
                        }
                    }



                    res.OrverviewPolyline2 = GooglePoints.Decode(r.routes[1].overview_polyline.points);//  2020/3/11 hirose 追加             
                    res.DistanceInMeters2 = r.routes[1].legs[0].distance.value.ToString();
                    res.DurationRoute2 = r.routes[1].legs[0].duration.text.ToString();
                    res.Steps2 = steps2;
                    res.Summary2 = ((IsHighwayAvailable2 ? r.routes[1].summary.ToString() : ""));
                    res.isSuccess = 2;
                }
               


                res.StartAddress = r.routes[0].legs[0].start_address;
                res.EndAddress = r.routes[0].legs[0].end_address;
                
                res.Message="";
                return res;
            }
            catch (Exception ex)
            {
                return new ResponseSummary { isSuccess = 0, Message = ex.Message + ex.StackTrace };
            }
        }
    }
}
