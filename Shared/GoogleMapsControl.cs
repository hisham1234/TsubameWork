using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 未来シェア;
using static 未来シェア.Form1;

public class GoogleMapsControl
{
    private Gecko.GeckoWebBrowser mGeckoWebBrowser = null;
    private Form1 mThis;
    //コンストラクタ
    public GoogleMapsControl(Form1 obj)
    {
        mThis = obj;
        mGeckoWebBrowser = ((Form1)obj).geckoWebBrowser1;
        //mGeckoWebBrowser = mThis;
    }

    public void navigate(string url)
    {
        mGeckoWebBrowser.Navigate(url);
    }

    public bool initialize(ref string msg)
    {
        //mGeckoWebBrowser.Navigate("https://www.google.com");
        //return true;
        try
        {
            //var infobox = new System.IO.StreamReader("InfoBox.js", System.Text.Encoding.Default);
            //var infoboxStr = infobox.ReadToEnd();
            //infobox.Close();
            //infobox.Dispose();

            //var main = new System.IO.StreamReader("Main.js", System.Text.Encoding.Default);
            //var mainstr = main.ReadToEnd();
            //main.Close();
            //main.Dispose();



            //var map = new System.IO.StreamReader(file, System.Text.Encoding.Default);
            //var mapStr = map.ReadToEnd();
            //map.Close();
            //map.Dispose();



            //mGeckoWebBrowser.Parent = (Control)mThis;


            var file = "GoogleMap.html";
            //var file = "HTMLPage1.html";


            string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //mGeckoWebBrowser.LoadHtml(mapStr);

            mGeckoWebBrowser.Navigate("file:///" + appPath.Substring(0, appPath.LastIndexOf('\\')) + "\\" + file);// appPath);

            mGeckoWebBrowser.AddMessageEventListener("GoogleMap_MouseClick", GoogleMap_MouseClick);
            mGeckoWebBrowser.AddMessageEventListener("GoogleMap_MouseMove", GoogleMap_MouseMove);
            mGeckoWebBrowser.AddMessageEventListener("GoogleMap_Idle", GoogleMap_Idle);
            mGeckoWebBrowser.AddMessageEventListener("GoogleMap_ObjectClick", GoogleMap_ObjectClick);


            //mGeckoWebBrowser.Navigate("https://www.google.com");
            return true;
        }
        catch (Exception ex)
        {
            msg = ex.Message + ex.StackTrace;
            return false;
        }
    }


    public bool mapShown { get; set; }



    public void GoogleMap_MouseClick(string instring)
    {
        mThis.eventGooglemapMouseClick(instring);
    }

    public void GoogleMap_MouseMove(string instring)
    {
    }

    public void GoogleMap_Idle(string instring)
    {
        mapShown = true;
    }

    public void GoogleMap_ObjectClick(string instring)
    {
    }







    public enum JS_Command
    {
        addPolygon = 0,
        addPolyline,
        drawPolygon_editwork,
        changePolygonObjectID,
        removePolygonListALL,
        removePolyline,
        drawPolyline_editwork,
        clearPolyline_editwork,
        clearPolygon_editwork,
        zoomIn,
        zoomOut,
        addCircle,
        removeCircle,
        addIconMarker,
        addInfoWindowLabel,
        addInfoWindowLabel2,
        removeInfoWindowLabel,
        removeIconMarker,
        toCenter
    }

    //private Gecko.GeckoWebBrowser mGeckoWebBrowser;

    public void CallJS_removeCircle()
    {
        mGeckoWebBrowser.Navigate("javascript:removeCircle()");
    }

    public void CallJS_addCircle(int objectID, int r, double Lat, double Lon, string color)
    {
        //string[] o = new string[5 - 1];
        string[] o = { "", "", "", "", "" };
        o[0] = objectID.ToString();
        o[1] = r.ToString();
        o[2] = Lat.ToString();
        o[3] = Lon.ToString();
        o[4] = color;
        mGeckoWebBrowser.Navigate("javascript:" + string.Format("addCircle('{0}','{1}','{2}','{3}','{4}')", o[0], o[1], o[2], o[3], o[4]));
    }

    public void CallJS_addCircle2(int objectID, int r   , double Lat, double Lon, string color, string str)
    {
        string[] o = { "", "", "", "", "", "" };
        o[0] = objectID.ToString();
        o[1] = r.ToString();
        o[2] = Lat.ToString();
        o[3] = Lon.ToString();
        o[4] = color;
        o[5] = str;
        mGeckoWebBrowser.Navigate("javascript:" + string.Format("addCircle('{0}','{1}','{2}','{3}','{4}','{5}')", o[0], o[1], o[2], o[3], o[4], o[5]));
    }

    public void CallJS_addPolygon(int objectID, string LatLonString, string color, string name)
    {
        string[] o = { "", "", "", "", "", "" };
        o[0] = objectID.ToString();
        o[1] = LatLonString;
        o[2] = color;
        o[3] = name;
        mGeckoWebBrowser.Navigate("javascript:" + string.Format("addCircle('{0}','{1}','{2}','{3}')", o[0], o[1], o[2], o[3]));
    }

    public void CallJS_addPolyline(int objectID, string LatLonString, string color,LineType linetype)
    {
        string[] o = { "", "", "", "", "", "" };
        o[0] = objectID.ToString();
        o[1] = LatLonString;
        o[2] = color;
        o[3] = ((int)linetype).ToString();
        mGeckoWebBrowser.Navigate("javascript:" + string.Format("addPolyline('{0}','{1}','{2}','{3}')", o[0], o[1], o[2], o[3]));
    }

    public void CallJS_drawPolygon_editwork(int objectID, string LatLonString, string color)
    {
        string[] o = { "", "", "", "", "", "" };
        o[0] = objectID.ToString();
        o[1] = LatLonString;
        o[2] = color;
        mGeckoWebBrowser.Navigate("javascript:" + string.Format("addCircle('{0}','{1}','{2}')", o[0], o[1], o[2]));
    }

    public void CallJS_changePolygonObjectID(int objectID, string element, string color)
    {
        string[] o = { "", "", "", "", "", "" };
        o[0] = objectID.ToString();
        o[1] = element;
        o[2] = color;
        mGeckoWebBrowser.Navigate("javascript:" + string.Format("addCircle('{0}','{1}','{2}')", o[0], o[1], o[2]));
    }

    public void CallJS_removePolygonListALL()
    {
        mGeckoWebBrowser.Navigate("javascript:" + JS_Command.removePolygonListALL.ToString() + "()");
    }

    public void CallJS_removePolyline()
    {
        mGeckoWebBrowser.Navigate("javascript:" + JS_Command.removePolyline.ToString() + "()");
    }

    public void CallJS_drawPolyline_editwork(int objectID, string LatLonString, string color)
    {
        string[] o = { "", "", "", "", "", "" };
        o[0] = objectID.ToString();
        o[1] = LatLonString;
        o[2] = color;
        mGeckoWebBrowser.Navigate("javascript:" + JS_Command.drawPolyline_editwork.ToString() + string.Format("('{0}','{1}','{2}')", o[0], o[1], o[2]));
    }


    public bool ConvDATAURL(string filename, ref string dataurl)
    {
        try
        {
            if (filename == null)
                return false;

            var sr = new System.IO.StreamReader(filename);
            var br = new System.IO.BinaryReader(sr.BaseStream);
            var b = br.ReadBytes((int)br.BaseStream.Length);
            br.Close();
            sr.Close();
            dataurl = "data:image/png;base64," + System.Convert.ToBase64String(b);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public void CallJS_addIconMarker(double lat, double lon, string iconfilename)
    {
        string[] o = { "", "", "" };

        o[0] = lat.ToString();
        o[1] = lon.ToString();
        var data = "";

        if (ConvDATAURL(iconfilename, ref data))
        {
            o[2] = data;
            mGeckoWebBrowser.Navigate("javascript:" + JS_Command.addInfoWindowLabel.ToString() + string.Format("('{0}','{1}')", o[0], o[1]));
        }
    }

    public void CallJS_toCenter(double lat,double lon)
    {
        string[] o = { "", "", "" };

        o[0] = lat.ToString();
        o[1] = lon.ToString();

        mGeckoWebBrowser.Navigate("javascript:" + JS_Command.toCenter.ToString()
         + string.Format("('{0}','{1}')", o[0], o[1]));
    }

    public void CallJS_addInfoWindowLabel(string iconfilename, string text, string csstext, double lat, double lon, int offsetX, int offsetY)
    {
        var data = "";
        if (!ConvDATAURL(iconfilename, ref data))
        {
            data = null;
        }

        string[] o = { "", "", "", "", "", "", "" };
        o[0] = data;
        o[1] = lat.ToString();
        o[2] = lon.ToString();
        o[3] = text;
        o[4] = csstext;
        o[5] = offsetX.ToString();
        o[6] = offsetY.ToString();

        mGeckoWebBrowser.Navigate("javascript:" + JS_Command.addInfoWindowLabel.ToString()
            + string.Format("('{0}','{1}','{2}','{3}','{4}','{5}','{6}')",
            o[0], o[1], o[2], o[3], o[4], o[5], o[6]));
    }


    public void CallJS_addInfoWindowLabel2(string id, double lat, double lon, int offsetX, int offsetY)
    {
        string[] o = { "", "", "", "", "", "", "" };
        o[0] = id;
        o[1] = lat.ToString();
        o[2] = lon.ToString();
        o[3] = offsetX.ToString();
        o[4] = offsetY.ToString();

        mGeckoWebBrowser.Navigate("javascript:" + JS_Command.addInfoWindowLabel2.ToString()
            + string.Format("('{0}','{1}','{2}','{3}','{4}')",
            o[0], o[1], o[2], o[3], o[4]));
    }



    public void CallJS_removeInfoWindowLabel()
    {
        mGeckoWebBrowser.Navigate("javascript:" + JS_Command.removeInfoWindowLabel.ToString() + "()");
    }

    public void CallJS_removeIconMarker()
    {
        mGeckoWebBrowser.Navigate("javascript:" + JS_Command.removeIconMarker.ToString() + "()");
    }


    public void CallJS_clearPolyline_editwork()
    {
        mGeckoWebBrowser.Navigate("javascript:" + JS_Command.clearPolyline_editwork.ToString() + "()");
    }


    public void CallJS_clearPolygon_editwork()
    {
        mGeckoWebBrowser.Navigate("javascript:" + JS_Command.clearPolygon_editwork.ToString() + "()");
    }

    public void CallJS_zoomIn()
    {
        mGeckoWebBrowser.Navigate("javascript:" + JS_Command.zoomIn.ToString() + "()");
    }

    public void CallJS_zoomOut()
    {
        mGeckoWebBrowser.Navigate("javascript:" + JS_Command.zoomOut.ToString() + "()");
    }
}
