using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static 未来シェア.cls02_デマンド登録;
using static 未来シェア.MiraiShareDataOP;

namespace 未来シェア
{
    public partial class Form1 : Form
    {
        MiraiShareDataOP mDataOP;
        GoogleMapsControl mGoogleMaps;
        //DateTime dt処理日;

        Hashtable hashColumnName = new Hashtable();
        private bool mLabel = true;

        public class LatLng
        {
            public string name;
            public string latlon;
        }

        public Form1()
        {

            ////   35080719	136575365
            //int LatMill7 = 0, Lon_Milli7 = 0;
            ////CarsLat	CarsLong  3509137 13652130

            ////602   KokLatitude	KokLongitude  1265492 4928345   度分秒	3509092 13653545
            
            //// uti  35°9′8.98″	136°53′54.5″          126548980	492834500
            //if (vb_dll.clsConvertLibrary.度分秒をミリ秒に変換(35080719,    136575365, ref LatMill7, ref Lon_Milli7))
            //{
            //}

            Gecko.Xpcom.Initialize(".\\LIB\\xulrunner-33\\xulrunner");
            InitializeComponent();

            mGoogleMaps = new GoogleMapsControl(this);

            var msg = "";


            timer1.Enabled = false;
            timer1.Interval = 1000;



            dtDateTimePicker予約.Value = DateTime.Now.Date;




            mDataOP = new MiraiShareDataOP();
            //mDataOP.conMiraiDBStr = "Server=vm-k1002-sv;uid=MiraiShare;database=Test;pwd=miraishare987";
            mDataOP.conMiraiDBStr = "Server=192.168.1.115;uid=AiTest;database=GpsAiDispatch;pwd=332211";
            //mDataOP.conYoyakuDBStr = "Server=gpslogserver;uid=hirose;database=tsubameCTI;pwd=998877";
            mDataOP.conYoyakuDBStr = "Server=gpslogserver;uid=CTI0001;database=tsubameCTI;pwd=222cti333";
            
            mDataOP.setListBox(listBox1);

            mDataOP.messageLog("start");

            List<string[,]> l = new List<string[,]>();
            l.Add(new string[,] { { "Willdo松原", "35.155795, 136.895486" } });
            l.Add(new string[,] { { "アスナル金山", "35.144415, 136.901190" } });
            l.Add(new string[,] { { "栄三越", "35.168743, 136.907851" } });
            l.Add(new string[,] { { "名駅", "35.171076, 136.884044" } });
            l.Add(new string[,] { { "伏見", "35.169130, 136.897206" } });
            l.Add(new string[,] { { "大須", "35.159939, 136.907075" } });



            // DataTableを作成
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = new DataTable();

            dataTable1.Columns.Add("名称");
            dataTable1.Columns.Add("LatLon");
            dataTable2.Columns.Add("名称");
            dataTable2.Columns.Add("LatLon");


            for (int i = 0; i < l.Count; i++)
            {
                // DataTableにデータを追加
                DataRow row1 = dataTable1.NewRow();
                DataRow row2 = dataTable2.NewRow();
                row1["名称"] = l[i][0, 0];
                row1["LatLon"] = l[i][0, 1];
                row2["名称"] = l[i][0, 0];
                row2["LatLon"] = l[i][0, 1];
                dataTable1.Rows.Add(row1);
                dataTable2.Rows.Add(row2);
            }





            // コンボボックスにデータテーブルをセット
            this.comboPos1.DataSource = dataTable1;
            this.comboPos2.DataSource = dataTable2;
            //// 表示用の列を設定
            this.comboPos1.DisplayMember = "名称";
            this.comboPos2.DisplayMember = "名称";

            //// データ用の列を設定
            this.comboPos1.ValueMember = "LatLon";
            this.comboPos2.ValueMember = "LatLon";

            msg = "";
            if (!mDataOP.initialize(ref msg))
            {
                var s = "初期化に失敗しました:" + msg;
                MessageBox.Show(s);
                messageLog(s);
                Environment.Exit(1);
            }

            if (mDataOP.LastTask != null)
            {
                var tmp状態 = mDataOP.LastTask["状態"].ToString();

                //行がある
                if (tmp状態 == DEMAND_STATUS.計算中.ToString())
                {
                    var r = mDataOP.LastTask;
                    mDataOP.処理日時 = DateTime.Parse(r["処理日時"].ToString());
                    mDataOP.計算開始日時 = DateTime.Parse(r["計算開始日時"].ToString());
                    mDataOP.計算終了日時 = DateTime.Parse(r["計算終了日時"].ToString());
                    mDataOP.データ開始日時 = DateTime.Parse(r["データ開始日時"].ToString());
                    mDataOP.データ終了日時 = DateTime.Parse(r["データ終了日時"].ToString());

                    timer1.Enabled = true;
                    //var s = DEMAND_STATUS.計算完了;
                    //var s = DEMAND_STATUS.キャンセル;
                    //var s = DEMAND_STATUS.計算失敗;
                    //var s = DEMAND_STATUS.計算中;
                }
            }
            //行がなければ何もしない



            if (!mGoogleMaps.initialize(ref msg))
            {
                MessageBox.Show("GoogleMap.js読み込み時にエラーが起きました。継続できません。アプリを終了します\\n" + msg);
                Environment.Exit(1);
            }


            ////////dgvDetail.Columns["No."].Width = 30;
            ////////dgvDetail.Columns["イベント"].Width = 50;
            ////////dgvDetail.Columns["イベント日時"].Width = 110;
            ////////dgvDetail.Columns["住所"].Width = 300;
            ////////dgvDetail.Columns["名称"].Width = 300;

            hashColumnName.Add("No.", 30);
            hashColumnName.Add("イベント", 50);
            hashColumnName.Add("イベント日時", 110);
            hashColumnName.Add("住所", 150);
            hashColumnName.Add("名称", 300);
            hashColumnName.Add("passengerId", 50);
            hashColumnName.Add("demandId", 50);

            //getdata();

            //DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(0,0);
            //dataGridView1_CellClick(this, e);

            //DataGridViewCellEventArgs e2 = new DataGridViewCellEventArgs(0, 0);
            //dataGridView2_CellClick(this, e2);

            mCssText = string.Format("opacity: 0.8;font-size: {0}pt ; border: 0px solid #000000 ; margin-top: 0px ; background: white ; padding: 0px;textAlign: center ; ", 0);
            offsetX = -10 - 16;
            offsetY = -10 - 16;

            while (true)
            {
                if (mGoogleMaps.mapShown)
                {
                    mGoogleMaps.mapShown = false;
                    getdata();
                    DataGridViewCellEventArgs ee = new DataGridViewCellEventArgs(0, 0);
                    dataGridView1_CellClick(this, ee);
                    DataGridViewCellEventArgs ee2 = new DataGridViewCellEventArgs(0, 0);
                    dataGridView2_CellClick(this, ee2);
                    break;
                }
                Application.DoEvents();
                System.Threading.Thread.Sleep(50);
            }
        }

        private bool minit = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            this.radio_Label_無.Checked = true;
        }

        ////private void button1_Click(object sender, EventArgs e)
        ////{
        ////    var msg = "";
        ////    var r = cls01_デマンドの乗降位置の確認.デマンドの乗降位置の確認_Execute(35.19078742, 137.0487333, ref msg);//OK
        ////}

        ////private void button2_Click(object sender, EventArgs e)
        ////{

        ////}

        ////private void button4_Click(object sender, EventArgs e)
        ////{
        ////    var msg = "";
        ////    var status = "";

        ////    //MessageBox.Show(mDataOP.checkDemandCalc(ref status, ref msg)? status : "失敗しました:" + msg);
        ////}

        ////private void button5_Click(object sender, EventArgs e)
        ////{
        ////    var msg = "";
        ////    mDataOP.MiraiShareServer_getAllResults(ref msg);
        ////}


        ////private void button6_Click(object sender, EventArgs e)
        ////{
        ////    var msg = "";
        ////    MessageBox.Show(mDataOP.MiraiShareServer_deleteAllDemands(ref msg) ? "成功しました" : "失敗しました:" + msg);
        ////}

        ////private void button7_Click(object sender, EventArgs e)
        ////{
        ////    var msg = "";
        ////    var dt = new DateTime(2019, 5, 1, 0, 0, 0);
        ////    var r = cls設定情報.車両初期位置変更_Execute(1, 35.19078742, 137.0487333, dt, ref msg);//OK
        ////}

        ////private void button8_Click(object sender, EventArgs e)
        ////{
        ////    var msg = "";
        ////    var r = cls設定情報.車両の無効化_Execute(1, true, ref msg);
        ////}

        ////private void button9_Click(object sender, EventArgs e)
        ////{
        ////    var msg = "";
        ////    MessageBox.Show(mDataOP.MiraiShareServer_getAllCarStatus(ref msg) ? "成功しました" : "失敗しました:" + msg);
        ////}

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                var msg = "";

                //DateTime 処理日 = dt処理日.Date;
                int 顧客ID = (int)num顧客ID.Value;
                DateTime 乗車日時 = dt乗車日時.Value;
                //int 処理グループID = (int)numグループID.Value;
                string 乗車地名称 = ((string)((DataRowView)comboPos1.SelectedItem).Row.ItemArray[0]).Trim();
                string 降車地名称 = ((string)((DataRowView)comboPos2.SelectedItem).Row.ItemArray[0]).Trim();

                double 乗車地Lat = double.Parse(((string)((DataRowView)comboPos1.SelectedItem).Row.ItemArray[1]).Split((char)',')[0].Trim());
                double 乗車地Lng = double.Parse(((string)((DataRowView)comboPos1.SelectedItem).Row.ItemArray[1]).Split((char)',')[1].Trim());

                double 降車地Lat = double.Parse(((string)((DataRowView)comboPos2.SelectedItem).Row.ItemArray[1]).Split((char)',')[0].Trim());
                double 降車地Lng = double.Parse(((string)((DataRowView)comboPos2.SelectedItem).Row.ItemArray[1]).Split((char)',')[1].Trim());

                MessageBox.Show(mDataOP.insertDemandRequest(顧客ID, 乗車日時, 乗車地名称, 乗車地Lat, 乗車地Lng, 降車地名称, 降車地Lat, 降車地Lng, ref msg) ? "成功しました" : "失敗しました:" + msg);
            }
            catch (Exception)
            {
                MessageBox.Show("例外");
            }
        }
        //private void button3_Click(object sender, EventArgs e)
        //{
        //}

        //private void button10_Click(object sender, EventArgs e)
        //{
        //    var msg = "";
        //    //var groupID = (int)numグループID.Value;

        //    if (!mDataOP.MiraiShareServer_checkLatLng(ref msg))
        //    {
        //        MessageBox.Show("事前処理：緯度経度に問題があります\n" + msg);
        //        return;
        //    }

        //    if (!mDataOP.MiraiShareServer_demandRegist(ref msg))
        //    {
        //        MessageBox.Show("未来シェアへの登録に失敗しました\n" + msg);
        //        return;
        //    }
        //    var r = mDataOP.MiraiShareServer_calc_execute(ref msg);//OK
        //}

        private void messageLog(string s)
        {
            mDataOP.messageLog(s);
        }

     

        DateTime dtLastCheck;

        private void controlStat(bool b)
        {
            this.dtDateTimePicker予約.Enabled = b;
            this.dtDateTimePicker予約.Enabled = b;
            this.btn計算開始.Enabled = b;
            this.num_From_H.Enabled = b;
            this.num_From_M.Enabled = b;
            this.num_To_H.Enabled = b;
            this.num_To_M.Enabled = b;
            Update();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var msg = "";
            var status = DEMAND_STATUS.計算中;

            if ((DateTime.Now - dtLastCheck).TotalSeconds > 10)
            {
                dtLastCheck = DateTime.Now;

                mDataOP.checkDemandCalc(ref status, ref msg);
                messageLog("計算確認:" + status);

                if (status == DEMAND_STATUS.計算完了)
                {
                    timer1.Enabled = false;
                    controlStat(true);
                    messageLog("計算終了");
                    lblMessage.Text = "計算が終了しました\n計算結果を取得しています";
                    lblMessage.BackColor = Color.Lime;


                    if (!mDataOP.finishDemandRequest(DEMAND_STATUS.計算完了, ref msg))
                    {
                        messageLog("finishDemandRequest():失敗" + msg);
                    }
                    messageLog("finishDemandRequest():終了");


                    if (mDataOP.MiraiShareServer_getAllResults(ref msg))
                    {
                        messageLog("結果取得成功");
                        lblMessage.Text = "計算結果を取得しました";
                        lblMessage.BackColor = Color.White;
                    }
                    else
                    {
                        messageLog("計算結果の取得に失敗しました:" + msg);
                        lblMessage.Text = "計算結果の取得に失敗しました";
                        lblMessage.BackColor = Color.Red;
                    }
                }
                else if (status == DEMAND_STATUS.キャンセル || status == DEMAND_STATUS.計算失敗)
                {
                    if (!mDataOP.finishDemandRequest(status, ref msg))
                    {
                    }
                    messageLog("finishDemandRequest():失敗;" + status);

                    timer1.Enabled = false;
                    controlStat(true);
                    messageLog("計算失敗:status=" + status);
                    lblMessage.Text = "計算が失敗しました";
                    lblMessage.BackColor = Color.Red;
                }
            }
        }

        //void initDatagridView()
        //{

        //}

        void getdata()
        {
            DataTable tbl = null;
            DataRow r = null;
            var msg = "";
            if (!mDataOP.getTaskList(false, ref r, ref tbl, ref msg))
            {
                MessageBox.Show("getTaskList" + msg);
            }
            dgvTaskList.DataSource = tbl;
            dgvTaskList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTaskList.Columns["処理日時"].Width = 110;
            dgvTaskList.Columns["計算開始日時"].Width = 110;
            dgvTaskList.Columns["計算終了日時"].Width = 110;
            dgvTaskList.Columns["データ開始日時"].Width = 110;
            dgvTaskList.Columns["データ終了日時"].Width = 110;
            dgvTaskList.Columns["状態"].Width = 60;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            getdata();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (dgvTaskList.Rows.Count < 1)
                return;

            var msg = "";
            var dt = DateTime.Parse(dgvTaskList.Rows[e.RowIndex].Cells["処理日時"].Value.ToString());
            DataTable tbl = null;

            if (!mDataOP.getViaList(dt, ref tbl, ref msg))
            {

            }
            dgvSavList.DataSource = tbl;
            dgvSavList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSavList.Columns["車両"].Width = 110;
            dgvSavList.Columns["処理日時"].Visible = false;
        }


        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            mCurrentIndex = e.RowIndex;
            drawMap(true,mCurrentIndex);
        }

        private int mCurrentIndex;

        public enum LineType
        {
            実線 = 0,
            破線 = 1
        }
        void drawMap(bool ToCenter,int index)
        {
            var msg = "";

            if (index < 0)
                return;

            if (dgvSavList.Rows.Count < 1)
                return;

            var savId = int.Parse(dgvSavList.Rows[index].Cells["車両"].Value.ToString());
            var dt = DateTime.Parse(dgvSavList.Rows[index].Cells["処理日時"].Value.ToString());
            DataTable tbl = null;

            if (!mDataOP.getViaPoints(dt, savId, ref tbl, ref msg))
            {

            }

            dgvDetail.DataSource = tbl;
            dgvDetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //dgvDetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //dgvDetail.Columns["車両"].Width = 80;
            dgvDetail.Columns["No."].Width = (int)hashColumnName["No."];
            dgvDetail.Columns["イベント"].Width = (int)hashColumnName["イベント"];
            dgvDetail.Columns["イベント日時"].Width = (int)hashColumnName["イベント日時"];
            dgvDetail.Columns["住所"].Width = (int)hashColumnName["住所"];
            dgvDetail.Columns["名称"].Width = (int)hashColumnName["名称"];
            dgvDetail.Columns["passengerId"].Width = (int)hashColumnName["passengerId"];
            dgvDetail.Columns["demandId"].Width = (int)hashColumnName["demandId"];
            //dgvDetail.Columns["状態"].Width = 80;

            //passengerId
            //    position_Lat
            //    position_Lng

            dgvDetail.Columns["demandId"].Visible = true;
            dgvDetail.Columns["passengerId"].Visible = true;

            dgvDetail.Columns["車両"].Visible = false;
            dgvDetail.Columns["position_Lat"].Visible = true;
            dgvDetail.Columns["position_Lng"].Visible = true;
            dgvDetail.Columns["処理日時"].Visible = false;
            dgvDetail.Columns["ID"].Visible = false;
            
            dgvDetail.Columns["action"].Visible = false;
            dgvDetail.Columns["estimatedTime"].Visible = false;
            dgvDetail.Columns["予約住所"].Visible = false;
            dgvDetail.Columns["予約名前"].Visible = false;
            dgvDetail.Columns["案内住所"].Visible = false;
            dgvDetail.Columns["案内名前"].Visible = false;
            dgvDetail.Columns["SendPickUpPosition_Lat"].Visible = false;
            dgvDetail.Columns["SendPickUpPosition_Lng"].Visible = false;
            dgvDetail.Columns["SendDropOffPosition_Lat"].Visible = false;
            dgvDetail.Columns["SendDropOffPosition_Lng"].Visible = false;
            dgvDetail.Columns["状態"].Visible = false;

            //dataGridView3.Columns["savId"].Width = 110;

            //tbl
            //mGoogleMaps.
            //dgvDetail[]



            double lat1 = 0.0;
            double lng1 = 0.0;
            double lat2 = 0.0;
            double lng2 = 0.0;

            mGoogleMaps.CallJS_removePolyline();
            mGoogleMaps.CallJS_removeInfoWindowLabel();

            var lasteventtime = DateTime.Parse("2000/1/1");
            double lastLat = 0.0;
            double lastLon = 0.0;

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                
                var id = (tbl.Rows[i]["No."]).ToString();
                var stat = (string)(tbl.Rows[i]["イベント"]);
                var eventDt = ((DateTime)(tbl.Rows[i]["イベント日時"])).ToString();
                var lat = (double)(tbl.Rows[i]["position_Lat"]);
                var lon = (double)(tbl.Rows[i]["position_Lng"]);

                var 移動時間分 = 0;
                var 移動距離 = 0.0;

                if (lasteventtime.Date != DateTime.Parse("2000/1/1"))
                {
                    //最初の1回目じゃない
                    移動時間分 = (int)Math.Truncate((DateTime.Parse(eventDt) - lasteventtime).TotalMinutes);
                    //
                    var r2 = 0.0;
                    var s2 = 0.0;
                    vb_dll.clsGpsGeomFunctions.GetDistance( lat, lon, lastLat, lastLon,ref r2,ref s2 );
                    移動距離 = (r2 / 1000);
                }
                lastLat = lat;
                lastLon = lon;
                lasteventtime = DateTime.Parse(eventDt);

                if (stat == "乗車")
                {
                    lat1 = (double)(tbl.Rows[i]["position_Lat"]);
                    lng1 = (double)(tbl.Rows[i]["position_Lng"]);

                    var add = (string)(tbl.Rows[i]["予約住所"]);
                    var name = (string)(tbl.Rows[i]["予約名前"]);

                    if (lat1 != 0.0 && lng1 != 0.0 && lat2 != 0.0 && lng2 != 0.0)
                    {
                        var p = "" + lat1 + "," + lng1 + "," + lat2 + "," + lng2;
                        mGoogleMaps.CallJS_addPolyline(0, p, "#0000FF", LineType.破線);
                    }
                    if (mLabel)
                    {
                        mGoogleMaps.CallJS_addInfoWindowLabel2(id, lat1, lng1, 0, 0);
                    }
                }
                if (stat == "降車")
                {
                    lat2 = (double)(tbl.Rows[i]["position_Lat"]);
                    lng2 = (double)(tbl.Rows[i]["position_Lng"]);
                    var add = (string)(tbl.Rows[i]["案内住所"]);
                    var name = (string)(tbl.Rows[i]["案内名前"]);
                    if (lat1 != 0.0 && lng1 != 0.0 && lat2 != 0.0 && lng2 != 0.0)
                    {
                        var p = "" + lat1 + "," + lng1 + "," + lat2 + "," + lng2;
                        mGoogleMaps.CallJS_addPolyline(0, p, "#FF0000", LineType.実線);
                    }
                    if (mLabel)
                    {
                        mGoogleMaps.CallJS_addInfoWindowLabel2(id, lat2, lng2, 0, 0);
                    }
                }
                if (i > 0)
                {
                    var labelLatCenter = lat2 + ((lat1 - lat2) / 2.0);
                    var labelLonCenter = lng2 + ((lng1 - lng2) / 2.0);

                    double d = Math.Floor(移動距離*10)/10;// (Math.Floor(移動距離)*10)/10.0;

                    var css = mCssText;
                    mGoogleMaps.CallJS_addInfoWindowLabel(null,
                        "" + 移動時間分 + "分<br>" + d +"km",
                        css,
                        labelLatCenter,
                        labelLonCenter,
                        offsetX, offsetY);
                }
            }
            lat1 = 0.0;
            lng1 = 0.0;
            lat2 = 0.0;
            lng2 = 0.0;

            if (ToCenter)
            {
                if(tbl.Rows.Count > 0)
                {
                    mGoogleMaps.CallJS_toCenter((double)(tbl.Rows[0]["position_Lat"]), (double)(tbl.Rows[0]["position_Lng"]));
                }
            }
        }
        private string mCssText = "";
        private int offsetX = 0;
        private int offsetY = 0;

        private void radio_Label_CheckedChanged(object sender, EventArgs e)
        {
            var check = ((RadioButton)sender).Checked;
            if (((RadioButton)sender).Name == this.radio_Label_大.Name)
            {
                if (check)
                {
                    mCssText = string.Format("opacity: 0.8;font-size: {0}pt ; border: 1px solid #0000ff ; margin-top: 0px ; background: white ; padding: 0px;textAlign: center ; ", 20);
                    offsetX = -10 - 20;
                    offsetY = -10 - 20;
                }
            }
            else if (((RadioButton)sender).Name == this.radio_Label_中.Name)
            {
                if (check)
                {
                    mCssText = string.Format("opacity: 0.8;font-size: {0}pt ; border: 1px solid #0000ff ; margin-top: 0px ; background: white ; padding: 0px;textAlign: center ; ", 16);
                    offsetX = -10 - 16;
                    offsetY = -10 - 16;
                }
            }
            else if (((RadioButton)sender).Name == this.radio_Label_小.Name)
            {
                if (check)
                {
                    mCssText = string.Format("opacity: 0.8;font-size: {0}pt ; border: 1px solid #0000ff ; margin-top: 0px ; background: white ; padding: 0px;textAlign: center ; ", 10);
                    offsetX = -10 - 10;
                    offsetY = -10 - 10;
                }
            }
            else if (((RadioButton)sender).Name == this.radio_Label_無.Name)
            {
                if (check)
                {
                    mCssText = string.Format("opacity: 0.8;font-size: {0}pt ; border: 0px solid #000000 ; margin-top: 0px ; background: white ; padding: 0px;textAlign: center ; ", 0);
                }
            }
            //
            drawMap(false, mCurrentIndex);
            if (mCssText != "")
            {
                
            }
        }

        //private void checkBox1_CheckedChanged(object sender, EventArgs e)
        //{
        //    //this.panel1.Visible = checkBox1.Checked;
        //}

        //private void checkBox2_CheckedChanged(object sender, EventArgs e)
        //{
        //    //mLabel = checkBox2.Checked;
        //    drawMap(mCurrentIndex);
        //}

        private void geckoWebBrowser1_Load(object sender, Gecko.DomEventArgs e)
        {

        }

        private void dgvDetail_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            if (hashColumnName.ContainsKey(e.Column.Name))
            {
                hashColumnName[e.Column.Name] = e.Column.Width;
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if(e.Action == TabControlAction.Selected && e.TabPage.Text == "計算結果")
            {
                getdata();
            }
        }

        private void dgvDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //mCurrentIndex = e.RowIndex;
            //drawMap(mCurrentIndex);
            if (e.RowIndex < 0)
                return;
            
            var tbl = (DataTable)this.dgvDetail.DataSource;
            var no =  int.Parse(dgvDetail.SelectedRows[0].Cells["No."].Value.ToString());





            for (int i=0;i<tbl.Rows.Count;i++)
            {
                if( no == int.Parse(tbl.Rows[i]["No."].ToString()))
                {
                    var lat = (double)tbl.Rows[i]["position_Lat"];
                    var lon = (double)tbl.Rows[i]["position_Lng"];
                    mGoogleMaps.CallJS_toCenter(lat,lon);
                    return;
                }
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.radio_Label_無.Checked = true;
        }

        private void radio_calc_real_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Name == this.radio_calc_simulate.Name)
            {
                this.mDataOP.TestMode = ((RadioButton)sender).Checked;

                if (((RadioButton)sender).Checked)
                {
                    this.dtDateTimePicker予約.Value = new DateTime(2018, 4, 1);
                }
                else
                {
                    this.dtDateTimePicker予約.Value = DateTime.Now.Date;
                }
                
            }
        }

        private void 計算開始()
        {
            //**********************
            //******計算ボタン******
            //**********************
            controlStat(false);

            var dt = DateTime.Now;
            var dt処理日時 = dt;
            dt処理日時 = dt.Date;
            dt処理日時 = dt処理日時.AddHours(dt.Hour);
            dt処理日時 = dt処理日時.AddMinutes(dt.Minute);

            mDataOP.処理日時 = dt処理日時;// DateTime.Parse("2020-01-16 23:12:00.000");
            //mDataOP.処理日時 = DateTime.Parse("2020-01-16 23:12:00.000");

            lblMessage.Text = "しばらくお待ち下さい";
            lblMessage.BackColor = Color.White;

            var dt計算対象日 = this.dtDateTimePicker予約.Value.Date;
            //mDataOP.setDate(this.dtDate.Value.Date);

            var from = new DateTime(dt計算対象日.Year, dt計算対象日.Month, dt計算対象日.Day, (int)num_From_H.Value, (int)num_From_M.Value, 0);
            var to = new DateTime(dt計算対象日.Year, dt計算対象日.Month, dt計算対象日.Day, (int)num_To_H.Value, (int)num_To_M.Value, 0);

            //var from = DateTime.Parse("2020-01-16 23:12:00.000");
            //var to = DateTime.Parse("2020-01-16 23:12:00.000");



            mDataOP.データ開始日時 = from;
            mDataOP.データ終了日時 = to;


            var msg = "";

            messageLog("処理開始");


            //未来シェアにあるデマンドをすべて削除
            if (!mDataOP.MiraiShareServer_deleteAllDemands(ref msg))
            {
                messageLog("MiraiShareServer_deleteAllDemands():失敗" + msg);
                lblMessage.Text = "失敗しました 1";
                lblMessage.BackColor = Color.Red;
                controlStat(true);
                return;
            }
            messageLog("MiraiShareServer_deleteAllDemands():終了");




            //DBにあるデマンドをすべて削除
            //if (!mDataOP.deleteDemandRequest(ref msg))
            //{
            //    messageLog("deleteDemandRequest():失敗" + msg);
            //    lblMessage.Text = "失敗しました 2";
            //    lblMessage.BackColor = Color.Red;
            //    controlStat(true);
            //    return;
            //}
            //messageLog("deleteDemandRequest():終了");



            if (!mDataOP.finishDemandRequest(null, ref msg))
            {
                messageLog("finishDemandRequest():失敗" + msg);
                lblMessage.Text = "失敗しました 2";
                lblMessage.BackColor = Color.Red;
                controlStat(true);
                return;
            }
            messageLog("finishDemandRequest():終了");



            //予約テーブルから「Mirai_DemandRequest」にインポートする
            //if (!mDataOP.import予約(from, to, ref msg))
            if (!mDataOP.import予約_手動作成(from, to, ref msg))
            {
                messageLog("inport予約():失敗:" + msg);
                lblMessage.Text = "失敗しました 3";
                lblMessage.BackColor = Color.Red;
                controlStat(true);
                return;
            }
            messageLog("import予約():終了");

            //タスクリストを作成
            if (!mDataOP.insertTaskList(mDataOP.処理日時, from, to, ref msg))
            {
                messageLog("insertTaskList():失敗:" + msg);
                lblMessage.Text = "失敗しました 4";
                lblMessage.BackColor = Color.Red;
                controlStat(true);
                return;
            }
            messageLog("insertTaskList():終了");


            if (!mDataOP.MiraiShareServer_demandRegist(this.radioCarpoolMode.Checked,ref msg))
            {
                messageLog("MiraiShareServer_demandRegist():失敗:" + msg);
                lblMessage.Text = "失敗しました 5";
                lblMessage.BackColor = Color.Red;
                controlStat(true);
                return;
            }
            messageLog("MiraiShareServer_demandRegist():終了");


            var r = mDataOP.MiraiShareServer_calc_execute(ref msg);//OK
            if (!r)
            {
                messageLog("MiraiShareServer_calc_execute():失敗:" + msg);
                lblMessage.Text = "失敗しました 6";
                lblMessage.BackColor = Color.Red;
                controlStat(true);
                return;
            }
            messageLog("MiraiShareServer_calc_execute():終了");

            mDataOP.計算開始日時 = DateTime.Now;

            if (!mDataOP.updateTaskList(mDataOP.処理日時, mDataOP.計算開始日時, new DateTime(1900, 1, 1), MiraiShareDataOP.DEMAND_STATUS.計算中, "", ref msg))
            {
                messageLog("insertTaskList():失敗:" + msg);
                lblMessage.Text = "失敗しました 7";
                lblMessage.BackColor = Color.Red;
                controlStat(true);
                return;
            }
            messageLog("updateTaskList():終了");

            messageLog("計算開始");
            lblMessage.Text = "計算中";
            lblMessage.BackColor = Color.Yellow;


            timer1.Enabled = true;
        }
        private void btn計算開始_Click(object sender, EventArgs e)
        {
            計算開始();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var msg = "";
            if(mDataOP.ConvertLatLonMM(ref msg))
            {
                MessageBox.Show("成功しました");
            }
            else
            {
                MessageBox.Show("失敗しました："+msg);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var msg = "";
            if(mDataOP.MiraiShareServer_getAllCarStatus(ref msg))
            {
                MessageBox.Show("成功しました");
            }
            else
            {
                MessageBox.Show("失敗しました：" + msg);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            var msg = "";
            for(int i = 1; i <= 200; i++)
            {
                cls01_デマンドの乗降位置の確認.車両有効無効(true, i, ref msg);
            }
            
        }

        private void button13_Click(object sender, EventArgs e)
        {
            var msg = "";
            for (int i = 6; i <= 200; i++)
            {
                cls01_デマンドの乗降位置の確認.車両有効無効(false, i, ref msg);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        public void eventGooglemapMouseClick(string data)
        {
            var ar = data.Split('\t');
            var latlon = ar[1] + "," + ar[2];

            if (radioButtonDirection開始.Checked)
            {
                this.textBoxDirection開始.Text = latlon;
                radioButtonDirection開始.Checked = false;
            }
            if (radioButtonDirection終了.Checked)
            {
                this.textBoxDirection終了.Text = latlon;
                radioButtonDirection終了.Checked = false;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {

            //エラー（一つしか取れないパターン）
            //35.165704613485524,136.89787427422488
            //35.15572679654294,136.89563194748843




            var param = new DirectioinAPI.DirectionParam();
            //var pickLat = pick_Lat.Text;
            //var pickLon = pick_Lon.Text;

            //var destLat = dest_Lat.Text;
            //var destLon = dest_Lon.Text;

            //param.origin = pickLat + "," + pickLon;
            //param.destination = destLat + "," + destLon;
            //"34.666046, 135.500482";

            param.origin = this.textBoxDirection開始.Text;
            //param.destination = "35.144601,136.901160";
            param.IsHighwayEnable = false;//isHighWay.Checked;

            param.destination = this.textBoxDirection終了.Text; 

            var apiBL = new DirectioinAPI.APIAccessBL();
            var res = apiBL.APIGetRoutes(param);

            if (!res.isSuccess)
            {
                MessageBox.Show("ERR \n" + res.Message);
                return;
            }
            //Route1_Dist.Text = res.DistanceInMeters1.ToString();
            //Route2_Dist.Text = res.DistanceInMeters2.ToString();

            //Route1_Dur.Text = res.DurationRoute1;
            //Route2_Dur.Text = res.DurationRoute2;


            var stepArray1 = res.OrverviewPolyline1.Split('/');
            mGoogleMaps.CallJS_removePolyline();
            for (int i = 0; i < stepArray1.Length-1; i++)
            {
                mGoogleMaps.CallJS_addPolyline(0, stepArray1[i] + "," +stepArray1[i+1] , "#0000FF", LineType.実線);
            }

            var stepArray2 = res.OrverviewPolyline2.Split('/');
            for (int i = 0; i < stepArray2.Length-1; i++)
            {
                mGoogleMaps.CallJS_addPolyline(0, stepArray2[i] + "," + stepArray2[i + 1], "#FF0000", LineType.実線);
            }
            //var p = "" + lat1 + "," + lng1 + "," + lat2 + "," + lng2;       
            //mGoogleMaps.CallJS_addPolyline(0, p, "#0000FF", LineType.破線);

        }
    }
}

