using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace 未来シェア
{
    public class IniFile
    {
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(
          string lpApplicationName,
          string lpKeyName,
          string lpDefault,
          StringBuilder lpReturnedstring,
          int nSize,
          string lpFileName);

        [DllImport("kernel32.dll")]
        private static extern int WritePrivateProfileString(
            string lpApplicationName,
            string lpKeyName,
            string lpstring,
            string lpFileName);


        public static string GetStringValue(string file, string section, string key)
        {
            StringBuilder sb = new StringBuilder(256);
            GetPrivateProfileString(section, key, string.Empty, sb, sb.Capacity, file);
            return sb.ToString();
        }

        public static int SetStringValue(string file, string section, string key,string value)
        {
            return WritePrivateProfileString(section, key, value, file);
        }
    }

    public class clsLogger
    {
        //'
        //'*****************************
        //'*** Logger V2 2009/06/01
        //'*****************************
        //'
        //'変更点:
        //'１．ディスク空き容量ゼロのときにログ書き込みを行うと、アプリがハングする現象を修正
        //'２．一定日数以降の古いログを削除できるように変更
        private string m_directory = "";
        private string m_basename = "";
        private int m_maxline = 0;
        private int m_logFileCount = 0;

        public clsLogger(string appdir,string name ,int maxline)
        {
            m_maxline = maxline;
            m_directory = appdir;
            m_basename = name;
        }

        public clsLogger(string appdir ,string name ,int maxline ,int logfilecount)
        {
            m_maxline = maxline;
            m_directory = appdir;
            m_basename = name;
            m_logFileCount = logfilecount;
        }

        private delegate void LogWriteCallback(string appdir, string basename, string logmessage, ref string outmessage);

        private void LogWrite(string appdir, string basename, string logmessage, ref string outmessage)
        {
            LogWriteCallback func = new LogWriteCallback(LogWriteExec);
            func.Invoke(appdir, basename, logmessage, ref outmessage);
        }

        public void DeleteOldLogFile()
        {
            DeleteOldLogFileExec(System.Windows.Forms.Application.StartupPath, m_basename, m_logFileCount);
        }

        private void DeleteOldLogFileExec(string  appdir , string basename , int  count)
        {
            //'
            //'************************* 
            //'*** 古いログを削除する
            //'*************************
            //'
            if(count< 1)
            {
                return;
            }
            try
            {
                string[] f = System.IO.Directory.GetFiles(appdir + "\\log\\", basename + " *.Log");
                if( f.Length > count)
                {
                    Array.Sort(f);
                    Array.Reverse(f);
                    for(int i = count;i < f.Length - 1; i++)
                    {
                        try
                        {
                            System.IO.File.Delete(f[i]);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                return;
            }
            catch (Exception)
            {
                return;
            }
        }
           

        private void LogWriteExec(string  appdir , string   basename , string   logmessage , ref string  outmessage)
        {
            var dt = string.Format("{0:yyyyMMdd}", DateTime.Now);
            System.IO.StreamWriter swFile = null;
            
            lock (this)
            {
                try
                {
                    if (!System.IO.Directory.Exists(appdir + "\\Log") )
                    {
                        System.IO.Directory.CreateDirectory(appdir + "\\Log");
                    }

                    swFile = new System.IO.StreamWriter(appdir + "\\Log\\" + basename + dt + ".log", true, System.Text.Encoding.Default);
                    swFile.WriteLine(logmessage);
                    swFile.Flush();
                    swFile.Close();
                    return;
                }
                catch (Exception)
                {
                    return;
                }
                finally
                {
                    //'
                    //'****追加**************************************
                    //'** 空容量がないときに書き込みに失敗してハングする問題の対策
                    try
                    {
                        if (swFile != null) {
                            swFile.Dispose();
                        }
                    }
                    catch (Exception)
                    {
                    }
                    //'******************************************
                    swFile = null;
                }
            }
        }
          


     

        public void messageLog(bool writeMode, ref System.Windows.Forms.ListBox LISTBOX , string str)
        {
            OutputListBox(writeMode, ref LISTBOX, str);
        }

        

        

        public void messageLog(bool writeMode , ref System.Windows.Forms.TextBox TEXTBOX , string str)
        {
            OutputTextBox(writeMode, ref TEXTBOX, str);
        }

        public string messageLog(bool dateflag ,string  str)
        {
            var outstr = "";
            if (dateflag)
            {
                outstr = String.Format("{0:yyyy/MM/dd HH:mm:ss.fff}", DateTime.Now) + " " + str;
            }
            else
            {
                outstr = str;
            }

            var a ="";
            LogWrite(System.Windows.Forms.Application.StartupPath, m_basename, outstr,ref a);
            
            return outstr;
        }



        //'スレッドセーフ　コールバック宣言
        delegate void OutputListBoxCallback(bool writeMode, ref System.Windows.Forms.ListBox LISTBOX, string str);

        private void OutputListBox(bool writeMode , ref System.Windows.Forms.ListBox LISTBOX , string str)
        {
            if (LISTBOX != null)
            {
                if (LISTBOX.InvokeRequired)
                {
                    OutputListBoxCallback d = new OutputListBoxCallback(OutputListBox);
                    LISTBOX.Invoke(d, new Object[] { writeMode, LISTBOX, str });
                    d = null;
                }
                else
                {
                    var outstr = String.Format("{0:yyyy/MM/dd HH:mm:ss.fff}", DateTime.Now) + " " + str;
                    LISTBOX.Items.Add(outstr);
                    if (LISTBOX.Items.Count > m_maxline)
                    {
                        LISTBOX.Items.RemoveAt(0);
                    }
                    LISTBOX.SetSelected(LISTBOX.Items.Count - 1, true);
                    LISTBOX.SetSelected(LISTBOX.Items.Count - 1, false);
                    LISTBOX.Update();

                    if (writeMode)
                    {
                        var a = "";
                        LogWrite(System.Windows.Forms.Application.StartupPath, m_basename, outstr, ref a);
                    }
                }
            }
        }
        
        

        //'スレッドセーフ　コールバック宣言
        delegate void OutputTextBoxCallback(bool writeMode ,ref System.Windows.Forms.TextBox TEXTBOX ,string str);


        private void OutputTextBox(bool writeMode ,ref System.Windows.Forms.TextBox TEXTBOX , string str)
        {
            if (TEXTBOX != null )
            {
                if (TEXTBOX.InvokeRequired)
                {
                    Delegate d = new OutputTextBoxCallback(OutputTextBox);
                    TEXTBOX.Invoke(d, new Object[] { writeMode, TEXTBOX, str });

                }
                else
                {
                    var outstr = String.Format("{0:yyyy/MM/dd HH:mm:ss.fff}", DateTime.Now) + " " + str;
                    TEXTBOX.Text = outstr;
                    if (writeMode)
                    {
                        var a = "";
                        LogWrite(System.Windows.Forms.Application.StartupPath, m_basename, outstr, ref a);
                    }
                }
            }
        }


        public void infomationLog(ref System.Windows.Forms.ListBox LISTBOX , string  str )
        {
            OutputListBox(true, ref LISTBOX, str);
        }
       

        public void infomationLog(ref System.Windows.Forms.TextBox TEXTBOX ,string str)
        {
            OutputTextBox(true, ref TEXTBOX, str);
        }
    }
}
