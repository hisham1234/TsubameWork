using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace データアップロードツール
{
    public partial class Form1 : Form
    {
        clsGetData dataAccess;
        public Form1()
        {
            InitializeComponent();
            dataAccess = new clsGetData();
            dataAccess.conMiraiDBStr = "Server=192.168.1.115;uid=AiTest;database=GpsAiDispatch;pwd=332211";
            dataAccess.conTsubameCTI = "Server=gpslogserver;uid=hisham;database=tsubameCTI;pwd=hisham987";
            lblResult.Visible = false;  
        }

        private void button1_Click(object sender, EventArgs e)
        {

            lblResult.Visible = false;
            var dateFrom = this.dtDateTimePickerFrom.Value.Date;
            var dateTo = this.dtDateTimePickerTo.Value.Date;
            //mDataOP.setDate(this.dtDate.Value.Date);

            var fromDateTime = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, (int)num_From_H.Value, (int)num_From_M.Value, 0);
            var toDateTime = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, (int)num_To_H.Value, (int)num_To_M.Value, 0);

            var msg = "";
             var f= fromDateTime.ToString();
            var t = toDateTime.ToString();
             dataAccess.getData(ref msg, f, t);
            lblResult.Visible = true;
            lblResult.Text = msg;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
