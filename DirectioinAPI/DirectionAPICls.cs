using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DirectioinAPI
{
    public class DirectionAPICls
    {
        
   
        public async  Task<MiraiShareResult> GenericGet(string strUrl)
        {
            try
            {
                // POST 先の
                Uri uri = new Uri(strUrl);

                //HttpClient を作成しヘッダーを設定します
                HttpClient httpClient = new HttpClient();
               // httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
              

                //contentの確定
                //StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                //contentの確定
               // StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                // データを送信します
                //HttpResponseMessage response = await httpClient.PostAsync(uri, content);

                //結果を取得
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
                //msg = ex.Message + ex.StackTrace;
                return null;
            }
        }
   }


    public class MiraiShareResult
    {
        public string result = "";
        public int StatusCode = 0;
        public string message = "";
        public string receiveJson = "";
        public bool success = false;
    }
}