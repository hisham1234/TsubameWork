using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static 未来シェア.MiraiShare_SAVS_API;

namespace 未来シェア
{
    public class cls03_配車計算の実行
    {
        public static MiraiShareResult 配車計算の実行_Execute(DateTime dt, ref string msg)
        {
            msg = "";
            try
            {
                配車計算の実行_Request param = new 配車計算の実行_Request();
                param.date = string.Format("{0:yyyy-MM-dd}", dt);

                Task<MiraiShareResult> r = Task.Run(() =>
                {
                    var json = JsonConvert.SerializeObject(param);
                    var res = GenericPost(miraiShareURL + "sav/batch/demands", json);
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

        [JsonObject]
        public class 配車計算の実行_Request
        {
            [JsonProperty("date")]
            public string date { get; set; }
        }
    }
}

     