using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectioinAPI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
           

            InitializeComponent();
            Route1_Dist.Text = "";
            Route2_Dist.Text = "";

            Route1_Dur.Text = "";
            Route2_Dur.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var param = new DirectionParam();
            var pickLat= pick_Lat.Text;
            var pickLon = pick_Lon.Text;

            var destLat = dest_Lat.Text;
            var destLon = dest_Lon.Text;

            param.origin = pickLat + "," + pickLon;
            param.destination = destLat + "," + destLon;
            //"34.666046, 135.500482";

          //   param.origin = "35.166058 ,136.895141";
            //param.destination = "35.160610,136.901899"; //"35.166340,136.902254";
            param.IsHighwayEnable = isHighWay.Checked;



            var apiBL = new APIAccessBL();
            var res= apiBL.APIGetRoutes(param);

            if (res.isSuccess==0)
            {
                MessageBox.Show("error "+res.Message);
                return;
            }
            Route1_Dist.Text = res.DistanceInMeters1.ToString();
            Route2_Dist.Text = res.DistanceInMeters2.ToString();

            Route1_Dur.Text = res.DurationRoute1;
            Route2_Dur.Text = res.DurationRoute2;
        }
    }
}
