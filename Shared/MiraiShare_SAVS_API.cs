using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;
using System.Runtime.Serialization.Json;
using System.IO;
using Newtonsoft.Json;
using System.Net;

namespace 未来シェア
{
    public class MiraiShare_SAVS_API
    {
        
        //public const string miraiShareURL = "https://stg-tsubame.savs.miraishare.com/";   //DGW 197
        public const string miraiShareURL = "https://dev-tsubame-hospital-ou4vie.savs.miraishare.com/";   //DGW 197
        //const string miraiShareURL =      "https://stg-tsubame.sav.miraishare.com/";   //DGW 197

            
        byte[] byteArray = Encoding.ASCII.GetBytes("54300FE5-F441-46E7-BBCA-EEB0C6843FF3:Raiy$oo6Ua+shob4");
      

        const string Header1_ContentType_ItemName = "ContentType";
        const string Header1_ContentType_Value = "application/json";

        const string Header2_Accept_ItemName = "Accept";
        const string Header2_Accept_Value = "application/json";

        const string Header3_Authorize_ItemName = "Authorization";
        const string Header3_Authorize_Value = "Basic RkEyOEVBMDAtNjZEOC00M0IzLThDOTUtRDQ2RUVGQkRBRjYyOmhNMkhZWEd6UjBDMDA4Rw==";




     
       

    

        /*
         * 
         １. デマンドの乗降位置の確認
        ２. デマンド登録
３. 配車計算の実行
４. 配車計算の状態を確認
５. 車両ごとに配車結果を取得
６. 登録したデマンドの削除
         * */


        public async static Task<MiraiShareResult> GenericPost(string strUrl,string json)
        {
            try
            {
                // POST 先の
                Uri uri = new Uri(strUrl);

                //HttpClient を作成しヘッダーを設定します
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
               // httpClient.DefaultRequestHeaders.Add(Header3_Authorize_ItemName, Header3_Authorize_Value);

                var byteArray = Encoding.ASCII.GetBytes("54300FE5-F441-46E7-BBCA-EEB0C6843FF3:Raiy$oo6Ua+shob4");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                //contentの確定
                StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                // データを送信します
                HttpResponseMessage response = await httpClient.PostAsync(uri, content);

                //結果を取得
                var result = response.Content.ReadAsStringAsync().Result;

                var r = new MiraiShareResult();
                r.receiveJson = result;
                r.message = "OK";
                r.StatusCode = (int)response.StatusCode;
                r.success = true;
                return r;
            }
            catch (Exception ex)
            {
                var r = new MiraiShareResult();
                r.receiveJson = "";
                r.message = ex.Message + ex.StackTrace;
                r.StatusCode = -1;
                r.success = false; ;
                return r;
            }
        }


        public async static Task<MiraiShareResult> GenericPut(string strUrl, string json)
        {
            try
            {
                // POST 先の
                Uri uri = new Uri(strUrl);

                //HttpClient を作成しヘッダーを設定します
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // httpClient.DefaultRequestHeaders.Add(Header3_Authorize_ItemName, Header3_Authorize_Value);

                var byteArray = Encoding.ASCII.GetBytes("54300FE5-F441-46E7-BBCA-EEB0C6843FF3:Raiy$oo6Ua+shob4");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                //contentの確定
                StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                // データを送信します
                HttpResponseMessage response = await httpClient.PutAsync(uri, content);

                //結果を取得
                var result = response.Content.ReadAsStringAsync().Result;

                var r = new MiraiShareResult();
                r.receiveJson = result;
                r.message = "OK";
                r.StatusCode = (int)response.StatusCode;
                r.success = true;
                return r;
            }
            catch (Exception ex)
            {
                var r = new MiraiShareResult();
                r.receiveJson = "";
                r.message = ex.Message + ex.StackTrace;
                r.StatusCode = -1;
                r.success = false; ;
                return r;
            }
        }

        /*











"{\"demandId\":4845,\"passengerId\":1,\"status\":\"CANCELED\",\"pickUpPosition\":{\"lat\":35.155795,\"lng\":136.895486},\"dropOffPosition\":{\"lat\":35.144415,\"lng\":136.90119},\"pickUpLocationName\":null,\"dropOffLocationName\":null,\"spaces\":[{\"name\":\"SEAT\",\"value\":1}],\"pickUpTime\":\"2019-07-16T22:50:47+09:00\",\"dropOffTime\":null,\"estimatePickUpTime\":null,\"estimateDropOffTime\":null,\"latestEstimatePickUpTime\":null,\"latestEstimateDropOffTime\":null,\"shareable\":false,\"shared\":false,\"sav\":null,\"errorCode\":null,\"createdTime\":\"2019-07-16T22:02:47+09:00\",\"pricingInfo\":null,\"reason\":{\"code\":\"finished\",\"operatedBy\":\"OPERATOR\"},\"notes\":[]}"





         
             */

        public async static Task<MiraiShareResult> GenericGet(string strUrl)
        {
            try
            {
                // POST 先の
                Uri uri = new Uri(strUrl);

                //HttpClient を作成しヘッダーを設定します
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // httpClient.DefaultRequestHeaders.Add(Header3_Authorize_ItemName, Header3_Authorize_Value);

                var byteArray = Encoding.ASCII.GetBytes("54300FE5-F441-46E7-BBCA-EEB0C6843FF3:Raiy$oo6Ua+shob4");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                //contentの確定
                //StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                // データを送信します
                HttpResponseMessage response = await httpClient.GetAsync(uri);

                //結果を取得
                var result = response.Content.ReadAsStringAsync().Result;

                var r = new MiraiShareResult();
                r.receiveJson = result;
                r.message = "OK";
                r.StatusCode = (int)response.StatusCode;
                r.success = true;
                return r;
            }
            catch (Exception ex)
            {
                var r = new MiraiShareResult();
                r.receiveJson = "";
                r.message = ex.Message + ex.StackTrace;
                r.StatusCode = -1;
                r.success = false; ;
                return r;
            }
        }

        public static MiraiShareResult GenericDelete(string strUrl,string json)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // httpClient.DefaultRequestHeaders.Add(Header3_Authorize_ItemName, Header3_Authorize_Value);

                var byteArray = Encoding.ASCII.GetBytes("54300FE5-F441-46E7-BBCA-EEB0C6843FF3:Raiy$oo6Ua+shob4");
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
                request.Method = "DELETE";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                request.Headers.Add(Header3_Authorize_ItemName, Header3_Authorize_Value);

                request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(byteArray);

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                using (var httpResponse = (HttpWebResponse)request.GetResponse())
                {
                    // do something with response
                    var r = new MiraiShareResult();
                    //r.receiveJson = httpResponse;
                    r.message = "OK";
                    r.StatusCode = (int)httpResponse.StatusCode;
                    r.success = true;
                    return r;
                }
            }
            catch (Exception ex)
            {
                var r = new MiraiShareResult();
                r.receiveJson = "";
                r.message = ex.Message + ex.StackTrace;
                r.StatusCode = -1;
                r.success = false; ;
                return r;
            }
        }

        //public static async Task<MiraiShareResult> DeleteTEst(string jsonSer)
        //{
        //    try
        //    {
        //        // POST 先の
        //        Uri uri = new Uri("https://stg-tsubame.sav.miraishare.com/sav/demands");

        //        //HttpClient を作成しヘッダーを設定します
        //        HttpClient httpClient = new HttpClient();
        //        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        httpClient.DefaultRequestHeaders.Add(Header3_Authorize_ItemName, Header3_Authorize_Value);


        //        デマンド登録_Request demands = new DemandsRequestBody();
        //        demands.passengerId = 1;
        //        demands.pickUpPosition.lat = 35.175667;
        //        demands.pickUpPosition.lng = 136.889744;
        //        demands.dropOffPosition.lat = 35.175667;
        //        demands.dropOffPosition.lng = 136.889744;

        //        var s = new DemandsRequestBody.Spaces[1];
        //        s[0] = new DemandsRequestBody.Spaces();
        //        s[0].name = "AAAA";
        //        s[0].value = 1;
        //        demands.spaces = s;

        //        demands.shareable = false;
        //        demands.pickUpTime = DateTimeOffset.Now;


        //        httpClient .pos

        //        //DataContractJsonSerializer jsonSer = new
        //        //DataContractJsonSerializer(typeof(DemandsRequestBody));

        //        MemoryStream ms = new MemoryStream();
        //        jsonSer.WriteObject(ms, demands);
        //        ms.Position = 0;

        //        StreamReader sr = new StreamReader(ms);


        //        // JSON が簡単なものであれば、シリアル化のための上記 5 行は無視して自分でシリアル化し、
        //        // これを StringContent コンストラクタの最初の引数として渡すこともできることに注意
        //        var contentStr = sr.ReadToEnd();
        //        StringContent content = new StringContent(contentStr, System.Text.Encoding.UTF8, "application/json");


        //        // データを送信します
        //        HttpResponseMessage response = await httpClient.PostAsync(uri, content);


        //        var result = response.Content.ReadAsStringAsync().Result;

        //        var r = new MiraiShareResult();
        //        r.receiveJson = result;
        //        r.message = "OK";
        //        r.StatusCode = (int)response.StatusCode;
        //        r.success = true;
        //        return r;
        //    }
        //    catch (Exception ex)
        //    {
        //        var r = new MiraiShareResult();
        //        r.receiveJson = "";
        //        r.message = ex.Message + ex.StackTrace;
        //        r.StatusCode = -1;
        //        r.success = false; ;
        //        return r;
        //    }
        //}







        /*
        content-type: application/json
        accept: application/json
        authorization: Basic RkEyOEVBMDAtNjZEOC00M0IzLThDOTUtRDQ2RUVGQkRBRjYyOmhNMkhZWEd6UjBDMDA4Rw==
        {
          "position":{
            "lat": 41.8144607,
            "lng": 140.7571585
           },
           "time": "2019-04-15T17:30:00+09:00" 
        }
         */


        /*
        １．位置情報の変更　      　PUT   https://stg-tsubame.sav.miraishare.com/sav/savs/{id}/location　　{id}は車両ID    "position":{ "lat": 41.8144607, "lng": 140.7571585 }, "time": "2019-04-15T17:30:00+09:00" 



        ２．利用車両数の変更
             車両の有効化／無効化　　　POST  https://stg-tsubame.sav.miraishare.com/sav/savs/{id}/is_rest　 　　{id}は車両ID    "value:  無効化=true　有効化=false"


        ３．車両ステータス（有効／無効）の確認
        　　　　GET　　https://stg-tsubame.sav.miraishare.com/sav/savs?page_token=50&page_size=50       １号車から５０号車のステータス取得
        　　　　GET　　https://stg-tsubame.sav.miraishare.com/sav/savs?page_token=100&page_size=50       ５１号車から１００号車のステータス取得


        ４．デマンドの乗降位置の確認　　　POST  https://stg-tsubame.sav.miraishare.com/sav/geo/point
        {
        　"latitude": (lat度),
          "longitude": (lon度)
        }

        *** レスポンス
        {
           "result":false or true
           "code":""
           "message":""
        }
        */


        private enum SENDTYPE
        {
            POST,
            PUT,
            GET
        }

        private void aa()
        {
            FormUrlEncodedContent content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "foo", "111" },
                    { "bar", "222" },
                    { "baz", "333" },
                });
        }


        public class MiraiShareResult
        {
            public string result = "";
            public int StatusCode = 0;
            public string message = "";
            public string receiveJson = "";
            public bool success = false;
        }

        private static async Task<MiraiShareResult> HttpClientSendExecute(HttpMethod type, string fullUrl)
        {

            //await aaaa(fullUrl);

            MiraiShareResult resultMessage = new MiraiShareResult();

            resultMessage.StatusCode = -1;
            resultMessage.message = "";
            resultMessage.receiveJson = "";
            resultMessage.result = "";
            resultMessage.success = false;

            try
            {
                //        content - type: application / json
                //accept: application / json
                //authorization: Basic RkEyOEVBMDAtNjZEOC00M0IzLThDOTUtRDQ2RUVGQkRBRjYyOmhNMkhZWEd6UjBDMDA4Rw==


                /*** コード書いてもいい  **/

                //Httpクライアント
                var httpClient = new HttpClient();
                HttpResponseMessage response;
                //リクエスト
                var httpRequest = new HttpRequestMessage(type, fullUrl);

                if (type == HttpMethod.Post)
                {
                    FormUrlEncodedContent param = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "latitude", "35.19078742" },
                        { "longitude", "137.0487333" },
                    });

                    var content = new ByteArrayContent(Encoding.UTF8.GetBytes(""));
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(Header1_ContentType_Value);
                    //content.Headers.Add("authorization", "Basic RkEyOEVBMDAtNjZEOC00M0IzLThDOTUtRDQ2RUVGQkRBRjYyOmhNMkhZWEd6UjBDMDA4Rw==");

                    //httpRequest.Content = new ByteArrayContent(Encoding.UTF8.GetBytes("{}"));
                    //httpRequest.Content.Headers.TryAddWithoutValidation(@"Content-Type", Header1_ContentType_Value); // OK
                    //httpRequest.Headers.Add(Header1_ContentType_ItemName, Header1_ContentType_Value);
                    httpRequest.Headers.Add(Header2_Accept_ItemName, Header2_Accept_Value);
                    httpRequest.Headers.Add(Header3_Authorize_ItemName, Header3_Authorize_Value);
                    httpRequest.Content = content;

                    //response = await httpClient.PostAsync(fullUrl, content);
                    response = await httpClient.SendAsync(httpRequest);
                }
                else if (type == HttpMethod.Put)
                {
                    httpRequest.Headers.Add(Header1_ContentType_ItemName, Header1_ContentType_Value);
                    httpRequest.Headers.Add(Header2_Accept_ItemName, Header2_Accept_Value);
                    httpRequest.Headers.Add(Header3_Authorize_ItemName, Header3_Authorize_Value);
                }
                else if (type == HttpMethod.Get)
                {
                    httpRequest.Headers.Add(Header2_Accept_ItemName, Header2_Accept_Value);
                    httpRequest.Headers.Add(Header3_Authorize_ItemName, Header3_Authorize_Value);
                }
                else
                {
                    resultMessage.message = "method type err:" + type.ToString();
                    return resultMessage;
                }

                //var response = await httpClient.SendAsync(httpRequest,content );

                var r = new MiraiShareResult();

                return r;
            }
            catch (Exception ex)
            {
                resultMessage.message = "Exception:" + ex.Message + ex.StackTrace;
                return resultMessage;
            }
        }



        //public static MiraiShareResult デマンドの乗降位置の確認(double lat, double lon)
        //{
        //    var r = Task.Run(async () =>
        //    {
        //        //FormUrlEncodedContent param = new FormUrlEncodedContent(new Dictionary<string, string>
        //        //{
        //        //    { "latitude", lat.ToString() },
        //        //    { "longitude", lon.ToString() },
        //        //});
        //        return await HttpClientSendExecute(HttpMethod.Post, miraiShareURL + "sav/geo/point");
        //    });
        //    return r.Result;
        //}

    }


}
