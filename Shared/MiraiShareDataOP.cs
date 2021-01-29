using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static 未来シェア.cls02_デマンド登録;
using static 未来シェア.cls04_配車計算の状態確認;
using static 未来シェア.cls05_車両ごとに配車結果を取得;


namespace 未来シェア
{
    class MiraiShareDataOP
    {
        public bool TestMode { set; get; }

        const int MAX_CAR_COUNT = 200;


        public DateTime 処理日時 { get; set; }
        public DateTime データ開始日時 { get; set; }
        public DateTime データ終了日時 { get; set; }
        public DateTime 計算開始日時 { get; set; }
        public DateTime 計算終了日時 { get; set; }
        public string conMiraiDBStr { get; set; }
        public string conYoyakuDBStr { get; set; }
        
        public DataRow LastTask { get; set; }


        private DateTime Testデータ開始日時 { get { return DateTime.Now.Date.AddDays(1); } }


        private clsLogger mLog;

        private ListBox mlist;



        public void setListBox(ListBox list)
        {
            mlist = list;
        }

        public MiraiShareDataOP()
        {
            mLog = new clsLogger(".\\Log", "MiraiShare", 100);
        }

        public void messageLog(string s)
        {
            try
            {
                mLog.messageLog(true, ref mlist, s);
            }
            catch (Exception)
            {
            }
        }

        public enum MiraiShare_demands_status
        {
            PENDING = 1,
            PICKING_UP = 2,
            CANCELED = 3,
            DENIED = 4,
            OTHER = 5,
            ASSIGNING=6
        }

        public enum DEMAND_STATUS
        {
            未処理 = 1,
            登録完了 = 2,
            計算中 = 3,
            計算完了 = 4,
            計算失敗 = 5,
            キャンセル = 6,
            保留中 = 7
        }

        public enum ERR_TYPE
        {
            情報 = 0,
            警告 = 1,
            エラー = 2
        }


        public bool MiraiShareServer_getAllCarStatus(ref string msg)
        {
            var sqlobj = new clsDataBase();

            try
            {
                var apimsg = "";
                var r1 = cls設定情報.車両ステータス確認_Execute(cls設定情報.GetStatusCount._001_to_050, ref apimsg);
                var r2 = cls設定情報.車両ステータス確認_Execute(cls設定情報.GetStatusCount._051_to_100, ref apimsg);
                var r3 = cls設定情報.車両ステータス確認_Execute(cls設定情報.GetStatusCount._101_to_150, ref apimsg);
                var r4 = cls設定情報.車両ステータス確認_Execute(cls設定情報.GetStatusCount._151_to_200, ref apimsg);


                List<cls設定情報.車両ステータス_Respose> list = new List<cls設定情報.車両ステータス_Respose>();
                list.Add(r1);
                list.Add(r2);
                list.Add(r3);
                list.Add(r4);


                if (r1 != null && r2 != null && r3 != null && r4 != null)
                {
                    if (!sqlobj.SetbolConnection(conMiraiDBStr, ref msg))
                    {
                        return false;
                    }

                    var sqlstr = "";
                    sqlstr = "DELETE FROM [dbo].[Mirai_CarsStatus] ";

                    var count = sqlobj.GetshoExecuteNonQuery(sqlstr, ref msg);

                    if (sqlobj.prpbolError == false)//error
                    {
                        msg = "sqlobj.prpbolError == false";
                        return false;
                    }

                    for (int obj = 0; obj < 4; obj++)
                    {
                        cls設定情報.車両ステータス_Respose info = list[obj];
                        for (int i = info.pageSize - 1; i >= 0; i--)
                        {
                            var insertStr = "";
                            insertStr += " INSERT INTO [dbo].[Mirai_CarsStatus] ";
                            insertStr += "  ( ";
                            insertStr += "   [savId] ";
                            insertStr += "  ,[driverId] ";
                            insertStr += "  ,[name] ";
                            insertStr += "  ,[isRest] ";
                            insertStr += "  ,[spaces_Name] ";
                            insertStr += "  ,[spaces_Capacity] ";
                            insertStr += "  ,[position_Lat] ";
                            insertStr += "  ,[position_Lng] ";
                            insertStr += "  ,[position_RetrievedAt] ";
                            insertStr += " ) VALUES ( ";
                            insertStr += "   " + info.savs[i].savId;
                            insertStr += "  ," + info.savs[i].driverId;
                            insertStr += "  ,'" + info.savs[i].name + "'";
                            insertStr += "  ," + (info.savs[i].isRest ? 1 : 0);
                            insertStr += "  ,'" + info.savs[i].spaces[0].name + "'";
                            insertStr += "  ," + info.savs[i].spaces[0].capacity;
                            insertStr += "  ," + info.savs[i].position.lat;
                            insertStr += "  ," + info.savs[i].position.lng;
                            insertStr += "  , '" + info.savs[i].position.retrievedAt + "'";
                            insertStr += " )  ";

                            sqlobj.GetshoExecuteNonQuery(insertStr, ref msg);

                            if (sqlobj.prpbolError == false)//error
                            {
                                msg = "sqlobj.prpbolError == false";
                                return false;
                            }
                        }
                    }
                    msg = "OK";
                    return true;
                }
                msg = "NG";
                return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return false;
            }
            finally
            {
                sqlobj.SetEndConnection();
            }
        }

        //public int checkDemandCalc2(ref DEMAND_STATUS status, ref string msg)
        //{
        //    try
        //    {
        //        status = "";
        //        if (checkDemandCalc(ref status, ref msg))
        //        {
        //            if (status == DEMAND_STATUS.計算完了.ToString())
        //                return 1;
        //            else if (status == DEMAND_STATUS.計算中.ToString())
        //                return 0;
        //            else
        //                return -1;
        //        }
        //        else
        //            return -10;
        //    }
        //    catch (Exception)
        //    {
        //        return -10;
        //    }
        //}
        public bool checkDemandCalc(ref DEMAND_STATUS status, ref string msg)
        {
            msg = "";
            try
            {
                var stat = "";

                if (MiraiShareServer_checkDemandCalculateStatus(ref stat, ref msg))
                {
                    if (stat == MiraiShareDataOP.MiraiShare_demands_status.PICKING_UP.ToString())
                    {
                        //updateStatus(DEMAND_STATUS.計算中, DEMAND_STATUS.計算完了, ref msg);
                        var s = DEMAND_STATUS.計算完了;
                        status = s;
                        msg = "PICKING_UP";
                        updateTaskList(処理日時, null, DateTime.Now, s, msg, ref msg);
                    }
                    else if (stat == MiraiShareDataOP.MiraiShare_demands_status.CANCELED.ToString())
                    {
                        //updateStatus(DEMAND_STATUS.計算中, DEMAND_STATUS.キャンセル, ref msg);
                        var s = DEMAND_STATUS.キャンセル;
                        status = s;
                        msg = "CANCELED";
                        updateTaskList(処理日時, null, DateTime.Now, s, msg, ref msg);
                    }
                    else if (stat == MiraiShareDataOP.MiraiShare_demands_status.DENIED.ToString())
                    {
                        //updateStatus(DEMAND_STATUS.計算中, DEMAND_STATUS.計算失敗, ref msg);
                        var s = DEMAND_STATUS.計算失敗;
                        status = s;
                        msg = "DENIED";
                        updateTaskList(処理日時, null, DateTime.Now, s, msg, ref msg);
                    }
                    else if (stat == MiraiShareDataOP.MiraiShare_demands_status.PENDING.ToString())
                    {
                        var s = DEMAND_STATUS.計算中;
                        status = s;
                        updateTaskList(処理日時, null, null, s, msg, ref msg);
                    }
                    else
                    {
                        //status = (DEMAND_STATUS)stat;
                        //updateStatus(DEMAND_STATUS.計算中, DEMAND_STATUS.計算失敗, ref msg);
                        var s = DEMAND_STATUS.計算失敗;
                        status = s;
                        msg = stat;
                        updateTaskList(処理日時, null, DateTime.Now, s, msg, ref msg);
                    }
                    msg = "OK";
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return false;
            }
        }

        private bool updateStatus(DEMAND_STATUS whereValue,DEMAND_STATUS setValue, ref string msg)
        {
            var sqlobj = new clsDataBase();
            try
            {
                if (!sqlobj.SetbolConnection(conMiraiDBStr, ref msg))
                {
                    return false;
                }

                var updateStr = "";
                updateStr += " UPDATE [dbo].[Mirai_DemandRequest] ";
                updateStr += "  SET ";
                updateStr += "   [状態] = '" + setValue.ToString() + "'";
                updateStr += " WHERE ";
                updateStr += "   [処理日時] = '" + 処理日時.ToString() + "'";
                updateStr += " AND ";
                updateStr += "   状態 = '" + whereValue.ToString() + "'";

                sqlobj.GetshoExecuteNonQuery(updateStr, ref msg);

                if (sqlobj.prpbolError == false)//error
                {
                    msg = "sqlobj.prpbolError == false";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return false;
            }
            finally
            {
                sqlobj.SetEndConnection();
            }
        }


        public bool GetCurrentDemands(ref string msg)
        {
            var sqlobj = new clsDataBase();

            try
            {
                if (!sqlobj.SetbolConnection(conMiraiDBStr, ref msg))
                {
                    return false;
                }

                //■■■入力情報■■■
                var sqlstr = "";
                sqlstr += " SELECT ";
                sqlstr += "   [RecvDemandId] ";
                sqlstr += " FROM  ";
                sqlstr += "   [dbo].[Mirai_DemandRequest] ";
                sqlstr += " WHERE ";
                //sqlstr += "   状態 = '" + DEMAND_STATUS.計算完了.ToString() + "'";
                //sqlstr += " AND ";
                sqlstr += "   [処理日時] = '" + 処理日時.ToString() + "'";

                var tbl = sqlobj.GetdtTableSelectData(sqlstr, ref msg);

                if (tbl.Rows.Count == 0)
                {
                    msg = "計算中データはありません";
                    return false;
                }

                if (sqlobj.prpbolError == false)//error
                {
                    msg = "sqlobj.prpbolError == false";
                    return false;
                }


                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    var demandid = (int)tbl.Rows[i]["RecvDemandId"];
                    var demandvalue = cls04_配車計算の状態確認.GetDemand_Execute(demandid, ref msg);
                    var msg2 = "";
                    if(!insertDemandRecord(demandvalue,ref msg2))
                    {
                        messageLog(msg2);
                    }
                }



                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return false;
            }
        }

        public bool MiraiShareServer_checkDemandCalculateStatus(ref string status, ref string msg)
        {
            var sqlobj = new clsDataBase();

            try
            {
                if (!sqlobj.SetbolConnection(conMiraiDBStr, ref msg))
                {
                    return false;
                }

                //■■■入力情報■■■
                var sqlstr = "";
                sqlstr += " SELECT ";
                sqlstr += "   MIN([RecvDemandId]) demandID ";
                sqlstr += " FROM  ";
                sqlstr += "   [dbo].[Mirai_DemandRequest] ";
                sqlstr += " WHERE ";
                sqlstr += "   状態 = '" + DEMAND_STATUS.計算中.ToString() + "'";
                sqlstr += " AND ";
                sqlstr += "   [処理日時] = '" + 処理日時.ToString() + "'";

                var tbl = sqlobj.GetdtTableSelectData(sqlstr, ref msg);

                if (tbl.Rows.Count == 0)
                {
                    msg = "計算中データはありません";
                    return false;
                }
                if (tbl.Rows[0]["demandID"].GetType().Name == "DBNull")
                {
                    msg = "計算中データはありません";
                    return false;
                }
                var demandID = (int)tbl.Rows[0]["demandID"];


                if (sqlobj.prpbolError == false)//error
                {
                    msg = "sqlobj.prpbolError == false";
                    return false;
                }

                //Task.Delay(360000);
                var r = cls04_配車計算の状態確認.配車計算の状態確認_Execute(demandID, ref msg);//OK
                while( MiraiShareDataOP.MiraiShare_demands_status.ASSIGNING.ToString()==r.status)
                {
                    Task.Delay(60000);
                     r = cls04_配車計算の状態確認.配車計算の状態確認_Execute(demandID, ref msg);
                }
                status = r.status;

                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return false;
            }
        }



        public bool MiraiShareServer_getAllResults(ref string msg)
        {
            msg = "";
            try
            {
                //var resStatus1 = cls06_登録したデマンドの削除.状態確認_Execute(ref msg);//OK

                for (int i = 0; i < MAX_CAR_COUNT; i++)
                {
                    var r = cls05_車両ごとに配車結果を取得.車両ごとに配車結果を取得_Execute(i, ref msg);//OK
                                                                                //var resDelete = cls06_登録したデマンドの削除.登録したデマンドの削除_Execute(, ref msg);

                    if (r != null)
                    {
                        for (int vcount = 0; vcount < r.viaPoints.Length; vcount++)
                        {
                            if (!insertViaPoints(r, r.viaPoints[vcount], ref msg))
                            {
                                continue;
                            }
                        }
                    }
                }
                GetCurrentDemands(ref msg);



                //**************************************************************a


                msg = "OK";
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return false;
            }



            //2147483648
            //1912311234
        }

        public bool MiraiShareServer_deleteAllDemands(ref string msg)
        {
            msg = "";
            try
            {
                while (true)
                {
                    var resStatus1 = cls06_登録したデマンドの削除.状態確認_Execute(ref msg);//OK
                    if(resStatus1.demands.Length == 0)
                    {
                        break;
                    }
                    for (int i = 0; i < resStatus1.demands.Length; i++)
                    {
                        var resDelete = cls06_登録したデマンドの削除.登録したデマンドの削除_Execute(resStatus1.demands[i].demandId, ref msg);
                    }
                    //var resStatus = cls06_登録したデマンドの削除.状態確認_Execute(ref msg);//OK
                }

                msg = "OK";
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return false;
            }
        }

        public bool insertViaPoints(車両ごとに配車結果を取得_Response response, 車両ごとに配車結果を取得_Response.ViaPoints viapoints, ref string msg)
        {
            msg = "";
            var sqlobj = new clsDataBase();
            try
            {
                if (!sqlobj.SetbolConnection(conMiraiDBStr, ref msg))
                {
                    return false;
                }

                var insertStr = "";
                insertStr += " INSERT INTO [Mirai_Result_viaPoints] ";
                insertStr += "  ( ";
                insertStr += "   [処理日時] ";
                insertStr += "  ,[savId] ";
                insertStr += "  ,[demandId] ";
                insertStr += "  ,[passengerId] ";
                insertStr += "  ,[viaPointId] ";
                insertStr += "  ,[action] ";
                insertStr += "  ,[estimatedTime] ";
                insertStr += "  ,[latestEstimatedTime] ";
                insertStr += "  ,[maxEstimationTime] ";
                insertStr += "  ,[minEstimationTime] ";
                //insertStr += "  ,[notes] ";
                insertStr += "  ,[pending] ";
                insertStr += "  ,[position_Lat] ";
                insertStr += "  ,[position_Lng] ";
                insertStr += "  ,[spaces_name] ";
                insertStr += "  ,[spaces_value] ";
                insertStr += "  ,[time] ";
                insertStr += " ) VALUES ( ";
                insertStr += "   '" + this.処理日時 + "'";
                insertStr += " , " + response.savId;
                insertStr += " , " + viapoints.demandId;
                insertStr += " , " + viapoints.passengerId;
                insertStr += " , " + viapoints.viaPointId;
                insertStr += " , '" + viapoints.action + "'";
                insertStr += " , '" + viapoints.estimatedTime.DateTime.ToString() + "'";
                insertStr += " , '" + viapoints.latestEstimatedTime.DateTime.ToString() + "'";
                insertStr += " , '" + viapoints.maxEstimationTime.DateTime.ToString() + "'";
                insertStr += " , '" + viapoints.minEstimationTime.DateTime.ToString() + "'";
                //insertStr += " , '" + viapoints.notes + "'";
                insertStr += " , '" + (int)(viapoints.pending ? 1 : 0) + "'";
                insertStr += " , " + viapoints.position.lat;
                insertStr += " , " + viapoints.position.lng;
                insertStr += " , '" + viapoints.spaces[0].name + "'";
                insertStr += " , " + viapoints.spaces[0].value;
                insertStr += " , '" + viapoints.time.DateTime.ToString() + "'";
                insertStr += " )  ";

                var r = sqlobj.GetshoExecuteNonQuery(insertStr, ref msg);

                if (sqlobj.prpbolError == false)//error
                {
                    msg = "sqlobj.prpbolError == false";
                    return false;
                }

                msg = "OK";
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return false;
            }
            finally
            {
                sqlobj.SetEndConnection();
            }
        }

        public bool insertDemandRecord(配車計算の状態確認_Response demand, ref string msg)
        {
            msg = "";
            var sqlobj = new clsDataBase();
            try
            {
                if (!sqlobj.SetbolConnection(conMiraiDBStr, ref msg))
                {
                    return false;
                }

                //■■■入力情報■■■





                var insertStr = "";
                insertStr += " INSERT INTO [dbo].[Mirai_DemandStatus] "; 
                insertStr += "  ( ";
                insertStr += "   [demandId] ";
                insertStr += "  ,[passengerId] ";
                insertStr += "  ,[createdTime] ";
                insertStr += "  ,[dropOffPosition_Lat] ";
                insertStr += "  ,[dropOffPosition_Lng] ";
                insertStr += "  ,[dropOffTime] ";
                insertStr += "  ,[errorCode] ";
                insertStr += "  ,[estimateDropOffTime] ";
                insertStr += "  ,[estimatePickUpTime] ";
                insertStr += "  ,[latestEstimateDropOffTime] ";
                insertStr += "  ,[latestEstimatePickUpTime] ";
                insertStr += "  ,[notes] ";
                insertStr += "  ,[pickUpPosition_Lat] ";
                insertStr += "  ,[pickUpPosition_Lng] ";
                insertStr += "  ,[pickUpTime] ";
                insertStr += "  ,[pricingInfo] ";
                insertStr += "  ,[reason_code] ";
                insertStr += "  ,[reason_operatedBy] ";
                insertStr += "  ,[sav_name] ";
                insertStr += "  ,[sav_position_lat] ";
                insertStr += "  ,[sav_position_lng] ";
                insertStr += "  ,[sav_id] ";
                insertStr += "  ,[shareable] ";
                insertStr += "  ,[shared] ";
                insertStr += "  ,[spaces_name] ";
                insertStr += "  ,[spaces_value] ";
                insertStr += "  ,[status] ";
                insertStr += " ) VALUES ( ";
                insertStr += "  '" + demand.demandId + "'";
                insertStr += " ,'" + demand.passengerId + "'";
                insertStr += " ,'" + convDate(demand.createdTime) + "'";
                insertStr += " ,'" + demand.dropOffPosition.lat + "'";
                insertStr += " ,'" + demand.dropOffPosition.lng + "'";
                insertStr += " ,'" + convDate(demand.dropOffTime)+ "'";
                insertStr += " ,'" + demand.errorCode + "'";
                insertStr += " ,'" + convDate(demand.estimateDropOffTime) + "'";
                insertStr += " ,'" + convDate(demand.estimatePickUpTime) + "'";
                insertStr += " ,'" + convDate(demand.latestEstimateDropOffTime) + "'";
                insertStr += " ,'" + convDate(demand.latestEstimatePickUpTime) + "'";
                insertStr += " ,'" + demand.notes + "'";
                insertStr += " ,'" + demand.pickUpPosition.lat + "'";
                insertStr += " ,'" + demand.pickUpPosition.lng + "'";
                insertStr += " ,'" + convDate(demand.pickUpTime) + "'";
                insertStr += " ,'" + demand.pricingInfo + "'";
                insertStr += " ,'" + demand.reason.code + "'";
                insertStr += " ,'" + demand.reason.operatedBy + "'";
                insertStr += " ,'" + demand.sav.name + "'";
                insertStr += " ,'" + demand.sav.position.lat + "'";
                insertStr += " ,'" + demand.sav.position.lng + "'";
                insertStr += " ,'" + demand.sav.savId + "'";
                insertStr += " ,'" + (string)(demand.shareable ? "1":"0") + "'";
                insertStr += " ,'" + (string)(demand.shared ? "1" : "0") + "'";
                insertStr += " ,'" + demand.spaces[0].name + "'";
                insertStr += " ,'" + demand.spaces[0].value + "'";
                insertStr += " ,'" + demand.status +"'";
                insertStr += " )  ";


                var c = sqlobj.GetshoExecuteNonQuery(insertStr, ref msg);

                if (sqlobj.prpbolError == false)//error
                {
                    msg = "sqlobj.prpbolError == false";
                    return false;
                }

                msg = "OK";
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return false;
            }
            finally
            {
                sqlobj.SetEndConnection();
            }
        }

        private string convDate(string s)
        {
            if (s == null)
            {
                return DateTime.Parse("1900-1-1").ToString();
            }
            try
            {
                return DateTime.Parse(s).ToString();
            }
            catch (Exception)
            {
                return DateTime.Parse("1900-1-1").ToString();
            }
        }

        public bool insertTaskList(DateTime dt処理日時, DateTime dtデータ開始日時, DateTime dtデータ終了日時, ref string msg)
        {
            msg = "";
            var sqlobj = new clsDataBase();
            try
            {
                if (!sqlobj.SetbolConnection(conMiraiDBStr, ref msg))
                {
                    return false;
                }

                var insertStr = "";
                insertStr += " INSERT INTO [dbo].[Mirai_TaskList] ";
                insertStr += " ( ";
                insertStr += "   [処理日時] ";
                insertStr += "  ,[計算開始日時] ";
                insertStr += "  ,[計算終了日時] ";
                insertStr += "  ,[データ開始日時] ";
                insertStr += "  ,[データ終了日時] ";
                insertStr += "  ,[状態] ";
                insertStr += " ) VALUES ( ";
                insertStr += "   '" + dt処理日時 + "'";
                insertStr += "  ,'1900-1-1'";
                insertStr += "  ,'1900-1-1'";
                insertStr += "  ,'" + dtデータ開始日時 + "'";
                insertStr += "  ,'" + dtデータ終了日時 + "'";
                insertStr += "  ,'" + DEMAND_STATUS.未処理.ToString() + "'";
                insertStr += " ) ";

                var c = sqlobj.GetshoExecuteNonQuery(insertStr, ref msg);

                if (sqlobj.prpbolError == false)//error
                {
                    msg = "sqlobj.prpbolError == false";
                    return false;
                }

                msg = "OK";
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return false;
            }
            finally
            {
                sqlobj.SetEndConnection();
            }
        }

        public bool updateTaskList(DateTime dt処理日時, DateTime? dt計算開始日時, DateTime? dt計算終了日時, DEMAND_STATUS 状態,string stat, ref string msg)
        {
            msg = "";
            var sqlobj = new clsDataBase();
            try
            {
                if (!sqlobj.SetbolConnection(conMiraiDBStr, ref msg))
                {
                    return false;
                }

                var updateStr = "";
                updateStr += " UPDATE [dbo].[Mirai_TaskList] ";
                updateStr += " SET ";
                if(dt計算開始日時 != null)
                {
                    updateStr += "   [計算開始日時] = '" + dt計算開始日時 + "' ,";
                }
                if (dt計算終了日時 != null)
                {
                    updateStr += "  [計算終了日時] = '" + dt計算終了日時 + "' ,";
                }
                updateStr += "  [状態] = '" + 状態.ToString() + "'";
                updateStr += "  ,[TS] = getdate() ";
                updateStr += "  ,[詳細] = '" + stat +"'";
                updateStr += " WHERE 処理日時 = '" + dt処理日時.ToString() + "'";

                var c = sqlobj.GetshoExecuteNonQuery(updateStr, ref msg);

                if (sqlobj.prpbolError == false)//error
                {
                    msg = "sqlobj.prpbolError == false";
                    return false;
                }

                msg = "OK";
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return false;
            }
            finally
            {
                sqlobj.SetEndConnection();
            }
        }


        public bool insertDemandRequest(int 顧客ID, DateTime 乗車日時, string 乗車地名称, double 乗車地Lat, double 乗車地Lng, string 降車地名称, double 降車地Lat, double 降車地Lng, ref string msg)
        {
            msg = "";
            var sqlobj = new clsDataBase();
            try
            {
                if (!sqlobj.SetbolConnection(conMiraiDBStr, ref msg))
                {
                    return false;
                }

                //■■■入力情報■■■




                var insertStr = "";
                insertStr += " INSERT INTO [dbo].[Mirai_DemandRequest] ";
                insertStr += "  ( ";
                insertStr += "   [処理日時] ";
                insertStr += "  ,[状態] ";
                //insertStr += "  ,[demandGroupID] "; //処理グループID
                insertStr += "  ,[予約住所] ";//乗車地名称
                insertStr += "  ,[案内住所] ";//降車地名称
                insertStr += "  ,[SendPpassengerId] ";//顧客ID
                insertStr += "  ,[SendPickUpPosition_Lat] ";//乗車地Lat
                insertStr += "  ,[SendPickUpPosition_Lng] ";//乗車地Lng
                insertStr += "  ,[SendPickUpTime] ";//乗車日時
                insertStr += "  ,[SendDropOffPosition_Lat] ";//降車緯度
                insertStr += "  ,[SendDropOffPosition_Lng] ";//降車経度
                insertStr += " ) VALUES ( ";
                insertStr += "  '" + 処理日時.ToString() + "'";
                insertStr += "  ,'" + DEMAND_STATUS.未処理.ToString() + "'";
                //insertStr += "  ," + 処理グループID;
                insertStr += "  ,'" + 乗車地名称 + "'";
                insertStr += "  ,'" + 降車地名称 + "'";
                insertStr += "  ," + 顧客ID;
                insertStr += "  ," + 乗車地Lat;
                insertStr += "  ," + 乗車地Lng;
                insertStr += "  ,'" + 乗車日時.ToString() + "'";
                insertStr += "  ," + 降車地Lat;
                insertStr += "  ," + 降車地Lng;
                insertStr += " )  ";


                sqlobj.GetshoExecuteNonQuery(insertStr, ref msg);

                if (sqlobj.prpbolError == false)//error
                {
                    msg = "sqlobj.prpbolError == false";
                    return false;
                }

                msg = "OK";
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return false;
            }
            finally
            {
                sqlobj.SetEndConnection();
            }
        }
        public bool MiraiShareServer_calc_execute(ref string msg)
        {
            var sqlobj = new clsDataBase();

            try
            {

                DateTime t;

                if (TestMode)
                {
                    t = Testデータ開始日時;
                }
                else
                {
                    t = データ開始日時;
                }

                var r = cls03_配車計算の実行.配車計算の実行_Execute(t.Date.AddDays(1), ref msg);//OK

                if (r.success)
                {
                    if (!sqlobj.SetbolConnection(conMiraiDBStr, ref msg))
                    {
                        return false;
                    }

                    var updateStr = "";
                    updateStr += " UPDATE [dbo].[Mirai_DemandRequest] ";
                    updateStr += "  SET ";
                    updateStr += "   [状態] = '" + DEMAND_STATUS.計算中.ToString() + "'";
                    updateStr += " WHERE ";
                    updateStr += "   [処理日時] = '" + 処理日時.ToString() + "'";
                    updateStr += " AND ";
                    updateStr += "   状態 = '" + DEMAND_STATUS.登録完了.ToString() + "'";

                    var c = sqlobj.GetshoExecuteNonQuery(updateStr, ref msg);

                    if (sqlobj.prpbolError == false)//error
                    {
                        msg = "sqlobj.prpbolError == false";
                        return false;
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return false;
            }
            finally
            {
                sqlobj.SetEndConnection();
            }
        }

        string convDateTimeto_T(DateTimeOffset dtoffset)
        {
            var s1 = string.Format("{0:yyyy-MM-ddTHH:mm:ss}", dtoffset);
            var offset = dtoffset.ToString().Split(' ')[2];
            return s1+ offset;
        }


        public bool MiraiShareServer_demandRegist(bool carpoolMode, ref string msg)
        {
            var sqlobj = new clsDataBase();

            try
            {
                if (!sqlobj.SetbolConnection(conMiraiDBStr, ref msg))
                {
                    return false;
                }

                //■■■入力情報■■■
                var sqlstr = "";
                sqlstr += " SELECT ";
                sqlstr += "   [ID] ";
                sqlstr += "  ,[SendPpassengerId] ";//顧客ID
                sqlstr += "  ,[SendPickUpPosition_Lat] ";//乗車地緯度
                sqlstr += "  ,[SendPickUpPosition_Lng] ";//乗車地経度
                sqlstr += "  ,[SendPickUpTime] ";//乗車日時
                sqlstr += "  ,[SendDropOffPosition_Lat] ";//降車緯度
                sqlstr += "  ,[SendDropOffPosition_Lng] ";//降車経度
                sqlstr += "  ,[SendDropOffTime] ";//降車経度
                //sqlstr += "  ,[SendSpaces_Name] ";
                //sqlstr += "  ,[SendSpaces_Value] ";
                //sqlstr += "  ,[SendShareable] ";
                sqlstr += " FROM [dbo].[Mirai_DemandRequest] ";
                sqlstr += " WHERE ";
                sqlstr += "       [処理日時] = '" + 処理日時.ToString() + "'";
                //sqlstr += "   AND [demandGroupID] = " + demandGroupID;
                sqlstr += "   AND 状態 = '" + DEMAND_STATUS.未処理.ToString() + "'";

                var tbl = sqlobj.GetdtTableSelectData(sqlstr, ref msg);

                if (sqlobj.prpbolError == false)//error
                {
                    msg = "sqlobj.prpbolError == false";
                    return false;
                }

                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    var ID = (int)tbl.Rows[i]["ID"];
                    var SendPpassengerId = i + 1;//(int)tbl.Rows[i]["SendPpassengerId"];
                    var SendPickUpPosition_Lat = (double)tbl.Rows[i]["SendPickUpPosition_Lat"];
                    var SendPickUpPosition_Lng = (double)tbl.Rows[i]["SendPickUpPosition_Lng"];
                    var SendPickUpTime = (DateTime)tbl.Rows[i]["SendPickUpTime"];
                    var SendDropOffPosition_Lat = (double)tbl.Rows[i]["SendDropOffPosition_Lat"];
                    var SendDropOffPosition_Lng = (double)tbl.Rows[i]["SendDropOffPosition_Lng"];

                    var SendDropOffTime = (DateTime)tbl.Rows[i]["SendDropOffTime"];//***************************追加

                    //var SendSpaces_Name = tbl.Rows[i]["SendSpaces_Name"];
                    //var SendSpaces_Value = tbl.Rows[i]["SendSpaces_Value"];
                    //var SendShareable = tbl.Rows[i]["SendShareable"];


                    デマンド登録_Request param = new デマンド登録_Request();
                    param.passengerId = SendPpassengerId;
                    param.pickUpPosition.lat = SendPickUpPosition_Lat;
                    param.pickUpPosition.lng = SendPickUpPosition_Lng;//うち
                    param.dropOffPosition.lat = SendDropOffPosition_Lat;//金山　タクシー降り場
                    param.dropOffPosition.lng = SendDropOffPosition_Lng;//金山　タクシー降り場
                    param.spaces[0].name = "SEAT";
                    param.spaces[0].value = 1;
                    param.shareable = false;

                    if (carpoolMode)
                    {
                        param.shareable = true;
                        //相乗りモード
                        //param.dropOffTime = SendDropOffTime;//*********************  追加
                        param.dropOffTime = convDateTimeto_T(SendDropOffTime);
                        //2020-02-14T11:00:00+09:00

                    }
                    else
                    {
                        //通常モード
                        //param.pickUpTime = SendPickUpTime;
                        param.pickUpTime = convDateTimeto_T(SendPickUpTime);
                    }
                    
                    


                    var respomse = cls02_デマンド登録.デマンド登録_Execute(param, ref msg);//OK

                    if (respomse == null)
                    {
                        messageLog("デマンド登録_Execute==null:" + msg);
                        continue;
                    }

                    try
                    {
                        var updateStr = "";
                        updateStr += " UPDATE [dbo].[Mirai_DemandRequest] ";
                        updateStr += "  SET ";
                        updateStr += "   [RecvDemandId]    = " + respomse.demandId;
                        updateStr += "  ,[RecvPassengerId] = " + respomse.passengerId;
                        updateStr += "  ,[RecvStatus]      = '" + respomse.status + "'";
                        updateStr += "  ,[RecvPickUpPosition_Lat]  = " + respomse.pickUpPosition.lat;
                        updateStr += "  ,[RecvPickUpPosition_Lng]  = " + respomse.pickUpPosition.lng;
                        updateStr += "  ,[RecvDropOffPosition_Lat] = " + respomse.dropOffPosition.lat;
                        updateStr += "  ,[RecvDropOffPosition_Lng] = " + respomse.dropOffPosition.lng;
                        updateStr += "  ,[RecvSpaces_Name]  = '" + respomse.spaces[0].name + "'";
                        updateStr += "  ,[RecvSpaces_Value] =  " + respomse.spaces[0].value;
                        var resPickupTime = DateTime.SpecifyKind(DateTime.Parse(respomse.pickUpTime), DateTimeKind.Utc).ToLocalTime().ToString();
                        updateStr += "  ,[RecvPickUpTime]   = '" + resPickupTime + "'";
                        // updateStr += "  ,[RecvPickUpTime]   = '" + respomse.pickUpTime + "'";
                        //updateStr += "  ,[RecvDropOffTime]   = '" + respomse.dropOffTime + "'";
                        //updateStr += "  ,[RecvPickUpTime]   = '" + respomse.pickUpTime.DateTime.ToString() + "'";
                        //updateStr += "  ,[RecvDropOffTime]   = '" + respomse.dropOffTime.DateTime.ToString() + "'";

                        updateStr += "  ,[状態] = '" + DEMAND_STATUS.登録完了.ToString() + "'";
                        updateStr += " WHERE ID = " + ID;

                        var c = sqlobj.GetshoExecuteNonQuery(updateStr, ref msg);

                        if (sqlobj.prpbolError == false)//error
                        {
                            msg = "sqlobj.prpbolError == false";
                            continue;
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return false;
            }
            finally
            {
                sqlobj.SetEndConnection();
            }
        }


        public bool finishDemandRequest(DEMAND_STATUS? 状態,ref string msg)
        {
            var sqlobj = new clsDataBase();

            try
            {
                if (!sqlobj.SetbolConnection(conMiraiDBStr, ref msg))
                {
                    return false;
                }

                var updateStr = "";
                updateStr += " UPDATE [dbo].[Mirai_DemandRequest] ";
                updateStr += "  SET ";
                updateStr += "    [Finish] = 1";
                if (状態 != null)
                {
                    updateStr += "   ,[状態] = '" + 状態.ToString() + "'";
                }
                updateStr += " WHERE finish = 0 ";

                var c = sqlobj.GetshoExecuteNonQuery(updateStr, ref msg);

                if (sqlobj.prpbolError == false)//error
                {
                    msg = "sqlobj.prpbolError == false";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return false;
            }
            finally
            {
                sqlobj.SetEndConnection();
            }
        }
        public bool UpdateMirai_手動登録(int 有効, ref string msg)
        {
            var sqlobj = new clsDataBase();

            try
            {
                if (!sqlobj.SetbolConnection(conMiraiDBStr, ref msg))
                {
                    return false;
                }

                var updateStr = "";
                updateStr += " UPDATE [dbo].[Mirai_手動登録] ";
                updateStr += "  SET ";
               
               
                updateStr += "   [有効] = '" + 有効 + "'";
                
                updateStr += " WHERE 有効 = 1 and グループ =2";

                var c = sqlobj.GetshoExecuteNonQuery(updateStr, ref msg);

                if (sqlobj.prpbolError == false)//error
                {
                    msg = "sqlobj.prpbolError == false";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return false;
            }
            finally
            {
                sqlobj.SetEndConnection();
            }
        }
        public void messageLog(ERR_TYPE type, string msg)
        {
            var sqlobj = new clsDataBase();
            try
            {
                if (!sqlobj.SetbolConnection(conMiraiDBStr, ref msg))
                {
                    return;
                }
                var insertStr = "";
                insertStr += " INSERT INTO [dbo].[Mirai_MessageLog] ";
                insertStr += "  ( ";
                insertStr += "   [ERROR_TYPE] ";
                insertStr += "  ,[Message] ";
                insertStr += " ) VALUES ( ";
                insertStr += "    " + type;
                insertStr += "  ,'" + msg + "'";
                insertStr += " )  ";
                sqlobj.GetshoExecuteNonQuery(insertStr, ref msg);
                return;
            }
            catch (Exception)
            {
            }
            finally
            {
                sqlobj.SetEndConnection();
            }
        }

        public bool MiraiShareServer_checkLatLng(ref string msg)
        {
            var sqlobj = new clsDataBase();

            try
            {
                if (!sqlobj.SetbolConnection(conMiraiDBStr, ref msg))
                {
                    return false;
                }

                //■■■入力情報■■■
                var sqlstr = "";
                sqlstr += " SELECT ";
                sqlstr += "   [ID] ";
                sqlstr += "  ,[SendPpassengerId] ";//顧客ID
                sqlstr += "  ,[SendPickUpPosition_Lat] ";//乗車地緯度
                sqlstr += "  ,[SendPickUpPosition_Lng] ";//乗車地経度
                sqlstr += "  ,[SendPickUpTime] ";//乗車日時
                sqlstr += "  ,[SendDropOffPosition_Lat] ";//降車緯度
                sqlstr += "  ,[SendDropOffPosition_Lng] ";//降車経度
                //sqlstr += "  ,[SendSpaces_Name] ";
                //sqlstr += "  ,[SendSpaces_Value] ";
                //sqlstr += "  ,[SendShareable] ";
                sqlstr += " FROM [dbo].[Mirai_DemandRequest] ";
                sqlstr += " WHERE ";
                sqlstr += "        [処理日時] = '" + 処理日時.ToString() + "'";
                //sqlstr += "    AND [demandGroupID] = " + demandGroup;
                sqlstr += "    AND 状態 = '" + DEMAND_STATUS.未処理.ToString() + "'";
                //sqlstr += "    AND FINISH = 0 ";

                var tbl = sqlobj.GetdtTableSelectData(sqlstr, ref msg);

                if (sqlobj.prpbolError == false)//error
                {
                    msg = "sqlobj.prpbolError == false";
                    return false;
                }

                if (tbl.Rows.Count == 0)
                {
                    msg = "データがありません";
                    return false;
                }

                var count = 0;
                var message = "";

                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    var ID = (int)tbl.Rows[i]["ID"];
                    var SendPickUpPosition_Lat = (double)tbl.Rows[i]["SendPickUpPosition_Lat"];
                    var SendPickUpPosition_Lng = (double)tbl.Rows[i]["SendPickUpPosition_Lng"];
                    var SendPickUpTime = (DateTime)tbl.Rows[i]["SendPickUpTime"];

                    var r = cls01_デマンドの乗降位置の確認.デマンドの乗降位置の確認_Execute(SendPickUpPosition_Lat, SendPickUpPosition_Lng, ref msg);//OK
                    if (!r.result)
                    {
                        count++;
                        message += "Message=" + r.message + ", ID=" + ID + ", PickUpTime=" + SendPickUpTime.ToString() + ", Lat=" + SendPickUpPosition_Lat + ", Lng=" + SendPickUpPosition_Lng + "\n";
                    }
                }

                if (count == 0)
                {
                    return true;
                }
                msg = message;
                return false;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return false;
            }
            finally
            {
                sqlobj.SetEndConnection();
            }
        }

        //public bool GetDemandsStatus(ref string msg)
        //{
        //    var r = cls04_配車計算の状態確認.配車計算の状態確認_Execute(100, ref msg);//OK
        //}


        class textimport
        {
            DateTime 処理日時;

            DateTime 予約日時;

            int 予約CD;
            string 予約住所;
            string 予約名前;
            double 予約LatW;
            double 予約LonW;

            int 案内CD;
            string 案内住所;
            string 案内名前;
            double 案内LatW;
            double 案内LonW;
           
        }


        private bool importYoyaku(DataRow r, ref string msg)
        {
            var sqlobj = new clsDataBase();

            try
            {
                if (!sqlobj.SetbolConnection(conMiraiDBStr, ref msg))
                {
                    return false;
                }

                var KokLat = ((int)r["KokLatitude"]) / 36000.0;
                var KokLng = ((int)r["KokLongitude"]) / 36000.0;
                var KokLatW = 0.0;
                var KokLngW = 0.0;

                if (!vb_dll.cls緯度経度変換.bol緯度経度変換_日本to世界(KokLat, KokLng, ref KokLatW, ref KokLngW))
                {
                    messageLog("緯度経度変換に失敗しました。:lat=" + KokLat + ", lng=" + KokLng);
                    return false;
                }

                var AnnaiLat = ((int)r["AnnaiLatitude"]) / 36000.0;
                var AnnaiLng = ((int)r["AnnaiLongitude"]) / 36000.0;
                var AnnaiLatW = 0.0;
                var AnnaiLngW = 0.0;

                if (!vb_dll.cls緯度経度変換.bol緯度経度変換_日本to世界(AnnaiLat, AnnaiLng, ref AnnaiLatW, ref AnnaiLngW))
                {
                    messageLog("緯度経度変換に失敗しました。:lat=" + AnnaiLat + ", lng=" + AnnaiLng);
                    return false;
                }



                var chkres = cls01_デマンドの乗降位置の確認.デマンドの乗降位置の確認_Execute(KokLatW, KokLngW, ref msg);//OK
                if (!chkres.result)
                {
                    messageLog("[乗車]位置が拒否されました。スキップします。" + r["YoyakuDateTime"].ToString() + ", " + r["Kokname"].ToString() + ", " + r["KokAddress"].ToString() + ", LatLng=" + KokLatW + "," + KokLngW);
                    return false;
                }
                var chkres_annai = cls01_デマンドの乗降位置の確認.デマンドの乗降位置の確認_Execute(AnnaiLatW, AnnaiLngW, ref msg);//OK
                if (!chkres_annai.result)
                {
                    messageLog("[降車]位置が拒否されました。スキップします。" + r["YoyakuDateTime"].ToString() + ", " + r["AnnaiName"].ToString() + ", " + r["AnnaiAddress"].ToString() + ", LatLng=" + AnnaiLatW + "," + AnnaiLngW);
                    return false;
                }




                var yoyakuDate = (((DateTime)r["YoyakuDateTime"]).AddYears(0)).ToString();



                var insertStr = "";
                insertStr += " INSERT INTO [dbo].[Mirai_DemandRequest] ";
                insertStr += "  ( ";
                insertStr += "  [状態]  ";
                insertStr += "  ,[予約CD]  ";
                insertStr += "  ,[処理日時]  ";
                insertStr += "  ,[予約住所]  ";
                insertStr += "  ,[予約名前]  ";
                insertStr += "  ,[案内CD]  ";
                insertStr += "  ,[案内住所]  ";
                insertStr += "  ,[案内名前]  ";
                //insertStr += "  ,[demandGroupID]  ";
                insertStr += "  ,[SendPpassengerId]  ";
                insertStr += "  ,[SendPickUpPosition_Lat]  ";
                insertStr += "  ,[SendPickUpPosition_Lng]  ";
                insertStr += "  ,[SendPickUpTime]  ";
                insertStr += "  ,[SendDropOffPosition_Lat]  ";
                insertStr += "  ,[SendDropOffPosition_Lng]  ";
                insertStr += " ) VALUES ( ";

                insertStr += "   '" + DEMAND_STATUS.未処理.ToString() + "'"; //[STATUS]  ";
                insertStr += " , " + r["Yoyaku_cd_key"];//[予約CD]  ";
                insertStr += " , '" + 処理日時.ToString() + "'";//[処理日時]  ";
                insertStr += " , '" + r["KokAddress"] + "'";//[予約住所]  ";
                insertStr += " , '" + r["Kokname"] + "'";//[予約名前]  ";
                insertStr += " , " + r["Annai_cd_key"]; //[案内CD]  ";
                insertStr += " , '" + r["AnnaiAddress"] + "'";//[案内住所]  ";
                insertStr += " , '" + r["AnnaiName"] + "'";//[案内名前]  ";
                //insertStr += " , ";//[demandGroupID]  ";
                insertStr += " , 0 ";//[SendPpassengerId]  ";
                insertStr += " , " + KokLatW;//[SendPickUpPosition_Lat]  ";
                insertStr += " , " + KokLngW;//[SendPickUpPosition_Lng]  ";

                if (TestMode)
                {
                    var dt = (DateTime)r["YoyakuDateTime"];
                    insertStr += " , '" + string.Format("{0:yyyy/MM/dd} {1:HH:mm:ss}", Testデータ開始日時.Date, dt) + "'"; //[SendPickUpTime]  ";
                }
                else
                {
                    insertStr += " , '" + r["YoyakuDateTime"] + "'"; //[SendPickUpTime]  ";
                }


                insertStr += " , " + AnnaiLatW;//[SendDropOffPosition_Lat]  ";
                insertStr += " , " + AnnaiLngW;//[SendDropOffPosition_Lng]  ";
                insertStr += " )  ";

                var inserres = sqlobj.GetshoExecuteNonQuery(insertStr, ref msg);

                if (sqlobj.prpbolError == false)//error
                {
                    msg = "sqlobj.prpbolError == false";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return false;
            }
            finally
            {
                sqlobj.SetEndConnection();
            }
        }
        private bool importYoyaku2(DataRow r, ref string msg)
        {
            var sqlobj = new clsDataBase();

            try
            {
                if (!sqlobj.SetbolConnection(conMiraiDBStr, ref msg))
                {
                    return false;
                }

                var KokLatW = (double)r["予約Lat世界度"];
                var KokLngW = (double)r["予約Lon世界度"];
                var AnnaiLatW = (double)r["案内Lat世界度"];
                var AnnaiLngW = (double)r["案内Lon世界度"];

                var chkres = cls01_デマンドの乗降位置の確認.デマンドの乗降位置の確認_Execute(KokLatW, KokLngW, ref msg);//OK
                if (!chkres.result)
                {
                    messageLog("[乗車]位置が拒否されました。スキップします。" + r["予約日時"].ToString() + ", " + r["予約住所"].ToString() + ", " + r["予約名称"].ToString() + ", LatLng=" + KokLatW + "," + KokLngW);
                    return false;
                }
                var chkres_annai = cls01_デマンドの乗降位置の確認.デマンドの乗降位置の確認_Execute(AnnaiLatW, AnnaiLngW, ref msg);//OK
                if (!chkres_annai.result)
                {
                    messageLog("[降車]位置が拒否されました。スキップします。" + r["予約日時"].ToString() + ", " + r["案内住所"].ToString() + ", " + r["案内名称"].ToString() + ", LatLng=" + AnnaiLatW + "," + AnnaiLngW);
                    return false;
                }


                


                var insertStr = "";
                insertStr += " INSERT INTO [dbo].[Mirai_DemandRequest] ";
                insertStr += "  ( ";
                insertStr += "  [状態]  ";
                insertStr += "  ,[予約CD]  ";
                insertStr += "  ,[処理日時]  ";
                insertStr += "  ,[予約住所]  ";
                insertStr += "  ,[予約名前]  ";
                //insertStr += "  ,[案内CD]  ";
                insertStr += "  ,[案内住所]  ";
                insertStr += "  ,[案内名前]  ";
                //insertStr += "  ,[demandGroupID]  ";
                insertStr += "  ,[SendPpassengerId]  ";
                insertStr += "  ,[SendPickUpPosition_Lat]  ";
                insertStr += "  ,[SendPickUpPosition_Lng]  ";
                insertStr += "  ,[SendPickUpTime]  ";
                insertStr += "  ,[SendDropOffPosition_Lat]  ";
                insertStr += "  ,[SendDropOffPosition_Lng]  ";
                insertStr += "  ,[SendDropOffTime]  ";
                insertStr += " ) VALUES ( ";

                insertStr += "   '" + DEMAND_STATUS.未処理.ToString() + "'"; //[STATUS]  ";
                insertStr += " , " + r["No"];//[予約CD]  ";
                insertStr += " , '" + 処理日時.ToString() + "'";//[処理日時]  ";
                insertStr += " , '" + r["予約住所"] + "'";//[予約住所]  ";
                insertStr += " , '" + r["予約名称"] + "'";//[予約名前]  ";
                //insertStr += " , " + r["Annai_cd_key"]; //[案内CD]  ";
                insertStr += " , '" + r["案内住所"] + "'";//[案内住所]  ";
                insertStr += " , '" + r["案内名称"] + "'";//[案内名前]  ";
                //insertStr += " , ";//[demandGroupID]  ";
                insertStr += " , 0 ";//[SendPpassengerId]  ";
                insertStr += " , " + r["予約Lat世界度"];//[SendPickUpPosition_Lat]  ";
                insertStr += " , " + r["予約Lon世界度"];//[SendPickUpPosition_Lng]  ";


                var dtPickUp = (DateTime)r["予約日時"];
                insertStr += " , '" + string.Format("{0:yyyy/MM/dd} {1:HH:mm:ss}", Testデータ開始日時.Date, dtPickUp) + "'"; //[SendPickUpTime]  ";
               

                insertStr += " , " + r["案内Lat世界度"];//[SendDropOffPosition_Lat]  ";
                insertStr += " , " + r["案内Lon世界度"];//[SendDropOffPosition_Lng]  ";

                var dtDropOff = (DateTime)r["案内先到着日時"];
                insertStr += " , '" + string.Format("{0:yyyy/MM/dd} {1:HH:mm:ss}", Testデータ開始日時.Date, dtDropOff) + "'"; //[SendPickUpTime]  ";
                insertStr += " )  ";

                var inserres = sqlobj.GetshoExecuteNonQuery(insertStr, ref msg);

                if (sqlobj.prpbolError == false)//error
                {
                    msg = "sqlobj.prpbolError == false";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return false;
            }
            finally
            {
                sqlobj.SetEndConnection();
            }
        }

        public bool import予約_手動作成(DateTime dtFrom, DateTime dtTo, ref string msg)
        {
            //var sqlobjYOYAKU = new clsDataBase();
            var sqlobjMIRAI = new clsDataBase();

            try
            {
                if (!sqlobjMIRAI.SetbolConnection(this.conMiraiDBStr, ref msg))
                {
                    return false;
                }

                var sqlStr = "";
                sqlStr += "  SELECT   ";
                sqlStr += "  [グループ] ";
                sqlStr += "  ,[No] ";
                sqlStr += "  ,[有効] ";
                sqlStr += "  ,[予約日時] ";
                sqlStr += "  ,[予約住所] ";
                sqlStr += "  ,[予約名称] ";
                sqlStr += "  ,[予約Lat世界度] ";
                sqlStr += "  ,[予約Lon世界度] ";
                sqlStr += "  ,[案内住所] ";
                sqlStr += "  ,[案内名称] ";
                sqlStr += "  ,[案内Lat世界度] ";
                sqlStr += "  ,[案内Lon世界度] ";
                sqlStr += "  ,[案内先到着日時] ";
                sqlStr += " FROM ";
                sqlStr += "  [dbo].[Mirai_手動登録]";
                sqlStr += " WHERE";
                sqlStr += "   有効 = 1 and グループ = 4";

                var tbl = sqlobjMIRAI.GetdtTableSelectData(sqlStr, ref msg);

                if (tbl == null)
                {
                    return false;
                }

                if (!sqlobjMIRAI.prpbolError)
                {
                    return false;
                }

                //**********************  取得成功
                var errcount = 0;

                if (tbl.Rows.Count == 0)
                {
                    msg = "予約データがありません";
                    return false;
                }

                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    var msgimport = "";
                    if (!importYoyaku2(tbl.Rows[i], ref msgimport))
                    {
                        errcount++;
                        continue;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                sqlobjMIRAI.SetEndConnection();
            }
        }

        public bool import予約(DateTime dtFrom, DateTime dtTo, ref string msg)
        {
            var sqlobjYOYAKU = new clsDataBase();
            var sqlobjMIRAI = new clsDataBase();

            try
            {
                if (!sqlobjYOYAKU.SetbolConnection(this.conYoyakuDBStr, ref msg))
                {
                    return false;
                }

                var sqlStr = "";
                sqlStr += "  SELECT   ";
                sqlStr += "  y.cd_key Yoyaku_cd_key ";
                sqlStr += "  , y.YoyakuDateTime ";
                sqlStr += "  , y.Kokname ";
                sqlStr += "  , y.KokAddress ";
                sqlStr += "  , y.KokLatitude ";
                sqlStr += "  , y.KokLongitude ";
                sqlStr += "  , a.cd_key Annai_cd_key ";
                sqlStr += "  , a.AnnaiName ";
                sqlStr += "  , a.AnnaiAddress ";
                sqlStr += "  , a.AnnaiLatitude ";
                sqlStr += "  , a.AnnaiLongitude ";
                sqlStr += " FROM ";
                sqlStr += "  [tsubameCTI].[dbo].[yoyaku] Y inner join  [tsubameCTI].[dbo].Annaisaki A  on Y.AnnaiCd_key = A.cd_key ";
                sqlStr += " WHERE";
                sqlStr += "   YoyakuDateTime >= '" + dtFrom + "' and YoyakuDateTime  < '" + dtTo + "' ";

                var tbl = sqlobjYOYAKU.GetdtTableSelectData(sqlStr, ref msg);

                if (tbl == null)
                {
                    return false;
                }

                if (!sqlobjYOYAKU.prpbolError)
                {
                    return false;
                }

                //**********************  取得成功
                var errcount = 0;

                if (tbl.Rows.Count == 0)
                {
                    msg = "予約データがありません";
                    return false;
                }

                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    var msgimport = "";
                    if (!importYoyaku(tbl.Rows[i], ref msgimport))
                    {
                        errcount++;
                        continue;
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                sqlobjYOYAKU.SetEndConnection();
            }
        }

        public bool initialize(ref string msg)
        {
            try
            {
                DataRow r = null;
                DataTable tbl = null;
                var stat = "";

                if (!getLastTask(ref r,ref msg))
                {
                    messageLog("getLastTask():" + msg);
                    return false;
                }

                LastTask = null;
                if (r != null)
                {
                    LastTask = r;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool getLastTask(ref DataRow r,ref string msg)
        {
            DataTable tbl = null;
            return getTaskList(true, ref r, ref tbl, ref msg);
        }

        
        public bool getViaPoints(DateTime dt処理日時, int savId,ref DataTable tbl, ref string msg)
        {
            var sqlobjMIRAI = new clsDataBase();

            try
            {
                if (!sqlobjMIRAI.SetbolConnection(this.conMiraiDBStr, ref msg))
                {
                    return false;
                }
                //ROW_NUMBER() OVER(order by [savId] desc) RN
                var sqlStr = "";
                sqlStr += " SELECT ";
                sqlStr += " ROW_NUMBER() OVER(order by [savId] desc) as [No.] ";
                sqlStr += " ,v.ID ";
                sqlStr += " ,v.処理日時 ";
                sqlStr += " ,v.savId as 車両";
                sqlStr += " ,v.demandId ";
                sqlStr += " ,v.action ";
                sqlStr += " ,v.estimatedTime ";
                sqlStr += " ,v.passengerId ";
                sqlStr += " ,v.position_Lat ";
                sqlStr += " ,v.position_Lng ";
                sqlStr += " ,CASE WHEN[action] = 'PICK_UP' THEN '乗車' WHEN[action] = 'DROP_OFF' THEN '降車' END AS 'イベント' ";
                sqlStr += " ,v.time as イベント日時 ";
                sqlStr += " ,CASE WHEN[action] = 'PICK_UP' THEN r.予約住所 WHEN[action] = 'DROP_OFF' THEN r.案内住所 END AS '住所' ";
                sqlStr += " ,CASE WHEN[action] = 'PICK_UP' THEN r.予約名前 WHEN[action] = 'DROP_OFF' THEN r.案内名前 END AS '名称' ";
                sqlStr += " ,r.状態 ";
                sqlStr += " ,r.予約住所 ";
                sqlStr += " ,r.予約名前 ";
                sqlStr += " ,r.案内住所 ";
                sqlStr += " ,r.案内名前 ";
                sqlStr += " ,r.SendPickUpPosition_Lat ";
                sqlStr += " ,r.SendPickUpPosition_Lng ";
                sqlStr += " ,r.SendDropOffPosition_Lat ";
                sqlStr += " ,r.SendDropOffPosition_Lng ";
                sqlStr += " FROM ";
                sqlStr += " Mirai_Result_viaPoints AS v INNER JOIN ";
                sqlStr += " Mirai_DemandRequest AS r ON v.demandId = r.RecvDemandId ";
                sqlStr += " WHERE ";
                sqlStr += " (v.savId = " + savId + " ) AND(v.処理日時 = '" + dt処理日時 + "') ";
                sqlStr += " order by id ";
                

                tbl = sqlobjMIRAI.GetdtTableSelectData(sqlStr, ref msg);

                if (tbl == null)
                {
                    return false;
                }

                if (!sqlobjMIRAI.prpbolError)
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                sqlobjMIRAI.SetEndConnection();
            }
        }

        public bool getViaList(DateTime dt処理日時, ref DataTable tbl, ref string msg)
        {
            var sqlobjMIRAI = new clsDataBase();

            try
            {
                if (!sqlobjMIRAI.SetbolConnection(this.conMiraiDBStr, ref msg))
                {
                    return false;
                }

                var sqlStr = "";
                sqlStr += " SELECT savId as 車両 ,処理日時 ";
                sqlStr += " FROM  [dbo].[Mirai_Result_viaPoints] ";
                sqlStr += " where 処理日時 = '" + dt処理日時 + "'";
                sqlStr += " group by savId,処理日時 ";


                tbl = sqlobjMIRAI.GetdtTableSelectData(sqlStr, ref msg);

                if (tbl == null)
                {
                    return false;
                }

                if (!sqlobjMIRAI.prpbolError)
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                sqlobjMIRAI.SetEndConnection();
            }
        }

        public bool getTaskList(bool isTop, ref DataRow r, ref DataTable tbl, ref string msg)
        {
            var sqlobjMIRAI = new clsDataBase();

            try
            {
                if (!sqlobjMIRAI.SetbolConnection(this.conMiraiDBStr, ref msg))
                {
                    return false;
                }

                var sqlStr = "";
                if (isTop)
                {
                    sqlStr += "  SELECT top 1  ";
                }
                else
                {
                    sqlStr += "  SELECT ";
                }

                //sqlStr += "  [ID]";
                sqlStr += "  [処理日時]";
                sqlStr += " ,[計算開始日時]";
                sqlStr += " ,[計算終了日時]";
                sqlStr += " ,[データ開始日時]";
                sqlStr += " ,[データ終了日時]";
                sqlStr += " ,[状態]";
                //sqlStr += " ,[TS] ";
                sqlStr += " FROM ";
                sqlStr += "  [dbo].[Mirai_TaskList] ";
                sqlStr += " ORDER BY id DESC";

                tbl = sqlobjMIRAI.GetdtTableSelectData(sqlStr, ref msg);

                if (tbl == null)
                {
                    return false;
                }

                if (!sqlobjMIRAI.prpbolError)
                {
                    return false;
                }

                if (tbl.Rows.Count == 0)
                {
                    r = null;
                    return true;
                }
                r = tbl.Rows[0];
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                sqlobjMIRAI.SetEndConnection();
            }
        }
        public bool deleteDemandRequest(ref string msg)
        {
            var sqlobj = new clsDataBase();

            try
            {
                if (!sqlobj.SetbolConnection(conMiraiDBStr, ref msg))
                {
                    return false;
                }

                var sqlstr = "";
                sqlstr = "DELETE FROM [dbo].[Mirai_DemandRequest] ";

                var count = sqlobj.GetshoExecuteNonQuery(sqlstr, ref msg);

                if (sqlobj.prpbolError == false)//error
                {
                    msg = "sqlobj.prpbolError == false";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return false;
            }
        }

        public bool ConvertLatLonMM(ref string msg)
        {
            var sqlobj = new clsDataBase();

            try
            {
                if (!sqlobj.SetbolConnection(conMiraiDBStr, ref msg))
                {
                    return false;
                }

                //■■■入力情報■■■
                var sqlstr = "";
                sqlstr += " SELECT ";
                sqlstr += "  [DataNo] ";
                sqlstr += "  ,[cd_key] ";
                sqlstr += "  ,[Mukae_Lat_mm] ";
                sqlstr += "  ,[Mukae_Long_mm] ";
                sqlstr += "  ,[An_Lat_mm] ";
                sqlstr += "  ,[An_Long_mm] ";
             
                sqlstr += " FROM  ";
                sqlstr += "   [dbo].[TestYoyaku1] ";
                //sqlstr += " WHERE ";
                //sqlstr += "   [処理日時] = '" + 処理日時.ToString() + "'";

                var tbl = sqlobj.GetdtTableSelectData(sqlstr, ref msg);

                if (tbl.Rows.Count == 0)
                {
                    msg = "計算中データはありません";
                    return false;
                }

                if (sqlobj.prpbolError == false)//error
                {
                    msg = "sqlobj.prpbolError == false";
                    return false;
                }


                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    var DataNo = (short)tbl.Rows[i]["DataNo"];
                    var CD_Key = (int)tbl.Rows[i]["cd_key"];
                    var JMilli_mukae_Lat = (int)tbl.Rows[i]["Mukae_Lat_mm"];
                    var JMilli_mukae_Lon = (int)tbl.Rows[i]["Mukae_Long_mm"];
                    var JMilli_an_Lat = (int)tbl.Rows[i]["An_Lat_mm"];
                    var JMilli_an_Lon = (int)tbl.Rows[i]["An_Long_mm"];
                    var W_mukae_DegreeLat = 0.0;
                    var W_mukae_DegreeLon = 0.0;
                    var W_an_DegreeLat = 0.0;
                    var W_an_DegreeLon = 0.0;

                    if (vb_dll.cls緯度経度変換.bol緯度経度変換_日本to世界(JMilli_mukae_Lat / 36000.0, JMilli_mukae_Lon / 36000.0, ref W_mukae_DegreeLat, ref W_mukae_DegreeLon))
                    {
                        if (vb_dll.cls緯度経度変換.bol緯度経度変換_日本to世界(JMilli_an_Lat / 36000.0, JMilli_an_Lon / 36000.0, ref W_an_DegreeLat, ref W_an_DegreeLon))
                        {
                            var sqlstrUpdate = "";
                            sqlstrUpdate += " UPDATE [dbo].[TestYoyaku1] ";
                            sqlstrUpdate += "  SET ";
                            sqlstrUpdate += "    Mukae_Lat  = " + W_mukae_DegreeLat;
                            sqlstrUpdate += "   ,Mukae_Long = " + W_mukae_DegreeLon;
                            sqlstrUpdate += "   ,An_Lat     = " + W_an_DegreeLat;
                            sqlstrUpdate += "   ,An_Long    = " + W_an_DegreeLon;
                            sqlstrUpdate += "  WHERE ";
                            sqlstrUpdate += "      DataNo = " + DataNo;
                            sqlstrUpdate += "  AND cd_key = " + CD_Key;
                            var r = sqlobj.GetshoExecuteNonQuery(sqlstrUpdate, ref msg);

                            if (!sqlobj.prpbolError)
                                continue;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                return false;
            }
        }
    }
}
