using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
namespace IntraPortal.Db
{
    public class clsDb
    {
        private string mstrConString = "";
        private bool mbolError = false;
        private SqlConnection mConnection = null;
        private SqlTransaction mTrn;
        private bool mbolTRN;

        public clsDb()
        {
            mbolError = false;
            mbolTRN = false;
        }

        public bool SetbolConnection(string vstrConString, ref string msg)
        {
            try
            {
                mstrConString = vstrConString;
                mConnection = new SqlConnection(mstrConString);
                mConnection.Open();
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace + ex.Source;
                return false;
            }

            return true;
        }

        //トランザクション指定
        public void SetBeginTRN()
        {
            mTrn = mConnection.BeginTransaction();
            mbolTRN = true;
        }

        //コミット
        public void SetCommitTRN()
        {
            mTrn.Commit();
        }

        //ロールバック
        public void SetRollbackTRN()
        {
            mTrn.Rollback();
        }

        //コネクション切断
        public void SetEndConnection()
        {
            try
            {
                mConnection.Close();
                mConnection.Dispose();
            }
            catch (Exception)
            {
            }
        }

        //*---------------------------------------------------------------*
        //データセットを返す
        //*---------------------------------------------------------------*
        public DataSet GetdtsetSelectData(string vstrSQL)
        {
            SqlDataAdapter da = new SqlDataAdapter(vstrSQL, mConnection);
            DataSet ds = new DataSet();

            try
            {
                //da = new SqlDataAdapter(vstrSQL, mConnection);
                da.Fill(ds);
                mbolError = true;
                return ds;
            }
            catch (Exception)
            {
                mbolError = false;
                return ds;
            }
            finally
            {
                da.Dispose();
            }
        }

        //*---------------------------------------------------------------*
        //データテーブルを返す
        //*---------------------------------------------------------------*
        public DataTable GetdtTableSelectData(string vstrSQL, ref string msg)
        {
            SqlDataAdapter da = new SqlDataAdapter(vstrSQL, mConnection);
            DataTable dt = new DataTable();

            try
            {
                da.Fill(dt);
                mbolError = true;
                return dt;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace + ex.Source;
                mbolError = false;
                return dt;
            }
            finally
            {
                da.Dispose();
            }
        }

        public int GetshoExecuteNonQuery(string vstrSQL, ref string msg)
        {
            SqlCommand cmd;
            int intCount = 0;

            //トランザクション指定在る無し
            if (mbolTRN)
            {
                cmd = new SqlCommand(vstrSQL, mConnection, mTrn);
            }
            else
            {
                cmd = new SqlCommand(vstrSQL, mConnection);
            }

            try
            {
                intCount = cmd.ExecuteNonQuery();
                mbolError = true;
                //戻り値成功レコード数
                return intCount;
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace + ex.Source;
                mbolError = false;
                return -1;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        //*---------------------------------------------------------------*
        //シングルカラム値を返す(string)
        //*---------------------------------------------------------------*
        public string GetstrSingleColumnSelectData(string vstrSQL)
        {
            SqlCommand cmd = new SqlCommand();

            if (mbolTRN)
            {
                cmd.Connection = mConnection;
                cmd.CommandText = vstrSQL;
                cmd.Transaction = mTrn;
            }
            else
            {
                cmd.Connection = mConnection;
                cmd.CommandText = vstrSQL;
            }

            try
            {
                string strData = (string)cmd.ExecuteScalar();
                mbolError = true;
                return strData;
            }
            catch (Exception)
            {
                mbolError = false;
                return "";
            }
        }

        //*---------------------------------------------------------------*
        //シングルカラム値を返す(string)
        //*---------------------------------------------------------------*
        public int GetintExecuteScalar(string vstrSQL)
        {
            SqlCommand cmd;
            int intCount = 0;

            //トランザクション指定在る無し
            if (mbolTRN)
            {
                cmd = new SqlCommand(vstrSQL, mConnection, mTrn);
            }
            else
            {
                cmd = new SqlCommand(vstrSQL, mConnection);
            }

            try
            {
                intCount = (int)cmd.ExecuteScalar();
                mbolError = true;
                //'戻り値成功レコード数
                return intCount;
            }
            catch (Exception)
            {
                mbolError = false;
                return -1;
            }
            finally
            {
                cmd.Dispose();
            }
        }

        //*---------------------------------------------------------------*
        // 単一行検索(string)
        //*---------------------------------------------------------------*
        public bool GetstrSingleSelectData(string vstrSQL, ref SqlDataReader dr)
        {
            SqlCommand cmd = new SqlCommand();

            if (mbolTRN)
            {
                cmd.Connection = mConnection;
                cmd.CommandText = vstrSQL;
                cmd.Transaction = mTrn;
            }
            else
            {
                cmd.Connection = mConnection;
                cmd.CommandText = vstrSQL;
            }

            try
            {
                dr = cmd.ExecuteReader();

                mbolError = true;
                return true;

            }
            catch (Exception)
            {
                mbolError = false;
                return false;

            }
            finally
            {
                cmd.Dispose();
            }
        }

        public bool prpbolError
        { // 3. プロパティ
            get { return mbolError; }
            set { mbolError = value; }
        }

        //コネクション
        public SqlConnection GetConnection
        {
            get { return mConnection; }
        }

        //トランザクション
        public SqlTransaction GetTransaction
        {
            get { return mTrn; }
        }
    }
}