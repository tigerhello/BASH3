using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using Kxz.Database;
using System.Threading;
using Demo.ui;

namespace Demo.driver.cam
{
    public class TestRecordManager
    {
        Dictionary<string, DataTable> dicDT;
        Dictionary<string, BindingSource> dicBS;
        Dictionary<string, SQLiteDataAdapter> dicAdp;
        public TestRecordManager(string dbFileName)
        {
            try
            {
                dicDT = new Dictionary<string, DataTable>();
                dicBS = new Dictionary<string, BindingSource>();
                dicAdp = new Dictionary<string, SQLiteDataAdapter>();

                //Conn = new SQLiteConnection(DbHelper.CreateConnectstring(dbFileName));
                Conn = new SQLiteConnection(dbFileName);
                Conn.Open();
            }
            catch
            {

            }
        }
        //public BindingSource BindingSources(string tableName, bool Clear = false)
        //{
        //    BindingSource bs;
        //    if (Clear)
        //    {
        //        dicDT.Remove(tableName);
        //        dicBS.Remove(tableName);
        //        dicAdp.Remove(tableName);
        //    }

        //    if (dicBS.TryGetValue(tableName, out bs))
        //        return bs;
        //    bs = new BindingSource();

        //    bs.DataSource = getDatatable3(tableName);
        //    dicBS.Add(tableName, bs);
        //    return bs;

        //}
     
        public SQLiteConnection Conn;// = AppConfig.DbConn;

        DataTable getDatatable(string tableName)
        {
            DataTable dt;

            if (dicDT.TryGetValue(tableName, out dt))
                return dt;

            dt = DbHelper.GetDataTable(Conn, string.Format("select * from {0} where id<0 ", tableName));
            dt.Columns["ID"].AutoIncrement = true;
            dt.Columns["ID"].AutoIncrementSeed = 1;
            dt.Columns["ID"].AutoIncrementStep = 1;
            dicDT.Add(tableName, dt);
            var adp = DbHelper.GetAdpter(Conn, tableName, "id<0");
            dicAdp.Add(tableName, adp);
            return dt;
        }

      public  DataTable getDatatable2(string tableName)
        {

            DataTable dt;

            if (dicDT.TryGetValue(tableName, out dt))
                return dt;

            
            dt = DbHelper.GetDataTable(Conn, string.Format("select * from {0} where id<0 ", tableName));
            dicDT.Add(tableName, dt);

            return dt;
        }

        public DataTable getDatatable3(string tableName,string SN)
        {

            DataTable dt;

            if (dicDT.TryGetValue(tableName, out dt))
                return dt;


            dt = DbHelper.GetDataTable(Conn, string.Format("select * from {0} where SN={1}>0 ", tableName,SN));
            dicDT.Add(tableName, dt);

            return dt;
        }

        public static DataTable GetDataTable(SQLiteConnection conn, string SelectSql)
        {
            var cmd = conn.CreateCommand();
            cmd.CommandText = SelectSql;
            SQLiteDataAdapter adp = new SQLiteDataAdapter();
            DataTable result = new DataTable();
            adp.SelectCommand = cmd;
            adp.Fill(result);
            return result;
        }


        public void DeletRecord(string tableName, string Sn)
        {
            var cmd = Conn.CreateCommand();
            cmd.CommandText = string.Format(" delete from {0} where  id='{1}'", tableName, Sn);
            cmd.ExecuteNonQuery();
            //string updateQuery = "Update studentInfo set sName=‘小李‘" + "Where ID=‘200131500145‘";
        }

        public int GetSnCount(string tableName, string Sn)
        {
            var cmd = Conn.CreateCommand();
            cmd.CommandText = string.Format(" select count(*) from {0} where  id='{1}'", tableName, Sn);

            var count = cmd.ExecuteScalar();
            return Convert.ToInt32(count);
        }
     

        public void DataRecord(string tableName, string idd, string time, string type)
        {
            try
            {
                string result = "";
                SQLiteDataReader reader;
                var cmd = Conn.CreateCommand();
                //// cmd.CommandText = string.Format(" delete from {0} where  id='{1}'", tableName, Sn);
                cmd.CommandText = string.Format(" select count(*) from {0} where  userid='{1}'", "userinfo", idd.ToString());

                var count = cmd.ExecuteScalar();

                if (Convert.ToInt32(count) > 0)
                {
                    cmd.CommandText = string.Format(" select name from {0} where  userid='{1}'", "userinfo", idd.ToString());
                    reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            result = reader[0].ToString();
                        }
                    }
                    reader.Close();
                }             
                cmd.CommandText = string.Format(" insert into {0}(userid,name, time, type) values('{1}','{2}','{3}','{4}')", tableName, idd.ToString(), result, time, type);
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());                   
            }
        }

        public string Get_ALL_Record(string tableName, string SN,int startIndex)
        {
                _rwlock.AcquireReaderLock(1200);
                try
                {
                    string result = "";
                    SQLiteDataReader reader;
                    var cmd = Conn.CreateCommand();
                    cmd.CommandTimeout = 1;

                    cmd.CommandText = string.Format(" select * from {0} where  SN='{1}' order by Time desc", tableName, SN);
                    reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            for (int j = startIndex; j < reader.FieldCount; j++)
                            {
                                result += reader[j].ToString() + ",";
                            }
                            //result = reader[0].ToString();
                        }
                    }
                    reader.Close();
                    return result;

                }
                finally
                { // 释放写锁 
                    _rwlock.ReleaseReaderLock();
                }
             
        }

        public string Get_ALLOCR_Record(string tableName, string OCRSN, int startIndex)
        {
            try
            {
                _rwlock.AcquireReaderLock(1200);
                string result = "";
                SQLiteDataReader reader;
                var cmd = Conn.CreateCommand();
                cmd.CommandTimeout = 1;

                cmd.CommandText = string.Format(" select * from {0} where  OCR='{1}' order by Time desc", tableName, OCRSN);
                reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        for (int j = startIndex; j < reader.FieldCount; j++)
                        {
                            result += reader[j].ToString() + ",";
                        }
                        //result = reader[0].ToString();
                    }
                }
                reader.Close();
                return result;

            }
            finally
            { // 释放写锁 
                _rwlock.ReleaseReaderLock();
            }

        }



        //public string Get_ALLOCR_Record(string tableName, string OCRSN, int startIndex)
        //{

        //    try
        //    {
        //        _rwlock.AcquireReaderLock(-1);
        //        try
        //        {
        //            string result = "";
        //            SQLiteDataReader reader;
        //            var cmd = Conn.CreateCommand();
        //            cmd.CommandTimeout = 5;

        //            cmd.CommandText = string.Format(" select * from {0} where  OCR='{1}' order by Time desc", tableName, OCRSN);
        //            reader = cmd.ExecuteReader();
        //            if (reader != null)
        //            {
        //                if (reader.Read())
        //                {
        //                    for (int j = startIndex; j < reader.FieldCount; j++)
        //                    {
        //                        result += reader[j].ToString() + ",";
        //                    }
        //                    //result = reader[0].ToString();
        //                }
        //            }
        //            reader.Close();
        //            return result;

        //        }
        //        finally
        //        { // 释放写锁 
        //            _rwlock.ReleaseReaderLock();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //        return "";
        //    }
        //}

        public void TruncatTable(string tableName)
        {
            var cmd = Conn.CreateCommand();
            cmd.CommandText = string.Format(" delete from {0} where  id>0", tableName);
            cmd.ExecuteNonQuery();
        }


        public void DataRecord_COPY(string tableName, string idd, string time, string type)
        {
            var cmd = Conn.CreateCommand();
            // cmd.CommandText = string.Format(" delete from {0} where  id='{1}'", tableName, Sn);
            cmd.CommandText = string.Format(" select count(*) from {0} where  userid='{1}'", tableName, idd.ToString());

            var count = cmd.ExecuteScalar();

            if (Convert.ToInt32(count) > 0)
                // cmd.CommandText = string.Format(" Update {0}  (userid, time, type) values('{1}','{2}','{3}')", tableName, idd.ToString(), time, type);
                cmd.CommandText = string.Format(" Update {0} set userid='{1}', time='{2}', type='{3}'", tableName, idd.ToString(), time, type);
            else
                // cmd.CommandText = string.Format(" insert into {0}(userid, time, type) value({1},{2},{3})", tableName,idd.ToString() , time,type);
                cmd.CommandText = string.Format(" insert into {0}(userid, time, type) values('{1}','{2}','{3}')", tableName, idd.ToString(), time, type);
            cmd.ExecuteNonQuery();
            //string updateQuery = "Update studentInfo set sName=‘小李‘" + "Where ID=‘200131500145‘";
        }

        public void DataDelet(string tableName, string idd)
        {
            var cmd = Conn.CreateCommand();
            SQLiteDataReader reader;
            string result = "";



            cmd.CommandText = string.Format(" select count(*) from {0} where  userid='{1}'", tableName, idd.ToString());
            var count = cmd.ExecuteScalar();



            if (Convert.ToInt32(count) > 0)
            {
                cmd.CommandText = string.Format(" select id from {0} where  userid='{1}'", "userinfo", idd);
                reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        result = reader[0].ToString();
                    }
                }

                reader.Close();



                cmd.CommandText = string.Format(" delete from {0} where  userid='{1}'", tableName, idd);
                int sa= cmd.ExecuteNonQuery();


                if (result != "" && sa>0)
                {
                    cmd.CommandText = string.Format(" update {0} set id=id-1 where  id>{1}", tableName, result);
                    cmd.ExecuteNonQuery();
                }
            }                                     
        }


        public void DataRecordui(string tableName, string idd, string flag, string userType, string FingerID1, string FingerID2, string FingerID3, string FingerID4, string password, string password_len, string rfid)
        {
            var cmd = Conn.CreateCommand();           
            // cmd.CommandText = string.Format(" delete from {0} where  id='{1}'", tableName, Sn);
            cmd.CommandText = string.Format(" select count(*) from {0} where  userid='{1}'", tableName, idd.ToString());

           var count0 = cmd.ExecuteScalar();

            if (Convert.ToInt32(count0) > 0)
                // cmd.CommandText = string.Format(" Update {0}  (userid, time, type) values('{1}','{2}','{3}')", tableName, idd.ToString(), time, type);
                cmd.CommandText = string.Format(" Update {0} set userid='{1}', flag='{2}', userType='{3}', FingerID1='{4}', FingerID2='{5}', FingerID3='{6}', FingerID4='{7}', password='{8}', password_len='{9}', rfid='{10}' where  userid='{11}'", tableName, idd,flag,userType,FingerID1, FingerID2,FingerID3,FingerID4,password,password_len,rfid, idd);
            else
                // cmd.CommandText = string.Format(" insert into {0}(userid, time, type) value({1},{2},{3})", tableName,idd.ToString() , time,type);
                cmd.CommandText = string.Format(" insert into {0}(userid,flag, userType, FingerID1, FingerID2, FingerID3, FingerID4, password, password_len, rfid) values('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')", tableName, idd, flag, userType, FingerID1, FingerID2, FingerID3, FingerID4, password, password_len, rfid);
            cmd.ExecuteNonQuery();
            //string updateQuery = "Update studentInfo set sName=‘小李‘" + "Where ID=‘200131500145‘";
        }

        public void DataRecordui2(string tableName, string idd, string flag, string userType, string FingerID1, string FingerID2, string FingerID3, string FingerID4, string password, string password_len, string rfid, string F1, string F2, string F3, string F4)
        {
         var cmd = Conn.CreateCommand();        
         cmd.CommandText = string.Format("replace into {0}(userid,flag, userType, FingerID1, FingerID2, FingerID3, FingerID4, password, password_len, rfid) values('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}')", 
         tableName, idd, flag, userType, FingerID1, FingerID2, FingerID3, FingerID4, password, password_len, rfid,F1,F2,F3,F4);
         cmd.ExecuteNonQuery();        
        }

        public void DataRecordui_mode(string tableName, string idd,string F1)
        {
            var cmd = Conn.CreateCommand();
            cmd.CommandText = string.Format("replace into {0} (Fid,F1) values('{1}','{2}')", tableName, idd, F1);
            cmd.ExecuteNonQuery();
        }

        public void UpdateRecord(string tableName, string Sn)
        {
            var cmd = Conn.CreateCommand();
            cmd.CommandText = string.Format(" Update {0} set name='{1}' Where id='3'", tableName, Sn);
            cmd.ExecuteNonQuery();
            //string updateQuery = "Update studentInfo set sName=‘小李‘" + "Where ID=‘200131500145‘";
        }

        public void UpdateRecord820V4(string tableName, string Sn, string AOI, string MES)
        {
            var cmd = Conn.CreateCommand();
            cmd.CommandText = string.Format(" Update {0} set AOI='{1}' , MES='{2}' Where id='{3}'", tableName, AOI, MES, Sn);
            cmd.ExecuteNonQuery();
        }
        //public void InsertRecord820v4(string tableName, string SN, string PN, string Time, string AOI, string MES)
        //{
        //    var cmd = Conn.CreateCommand();
        //    cmd.CommandText = string.Format(" insert into {0}(SN,PN,Time, AOI, MES) values('{1}','{2}','{3}','{4}','{5}')", "TestResult", SN, PN, Time, AOI, MES);
        //    cmd.ExecuteNonQuery();
        //}

        //public void InsertRecord820v4(string tableName, string SN, string ITEM, string VALUE)
        //{
        //    var cmd = Conn.CreateCommand();
        //    // cmd.CommandText = string.Format(" delete from {0} where  id='{1}'", tableName, Sn);
        //    cmd.CommandText = string.Format(" select count(*) from {0} where  SN='{1}'", tableName, SN);

        //    var count0 = cmd.ExecuteScalar();

        //    if (Convert.ToInt32(count0) > 0)
        //        // cmd.CommandText = string.Format(" Update {0}  (userid, time, type) values('{1}','{2}','{3}')", tableName, idd.ToString(), time, type);
        //        cmd.CommandText = string.Format(" Update {0} set {1}='{2}' where  SN='{3}'", tableName, ITEM, VALUE, SN);
        //    else
        //        // cmd.CommandText = string.Format(" insert into {0}(userid, time, type) value({1},{2},{3})", tableName,idd.ToString() , time,type);
        //        cmd.CommandText = string.Format(" insert into {0}('{1}',SN) values('{2}','{3}')", tableName, ITEM, VALUE, SN);

        //    cmd.ExecuteNonQuery();
        //}

        public static ReaderWriterLock _rwlock = new ReaderWriterLock();


        public void InsertRecord820v4(string tableName, string SN, string ITEM, string VALUE)
        {
            int vv = -2;


            try
            {
                _rwlock.AcquireWriterLock(2200);
                var cmd = Conn.CreateCommand();
                cmd.CommandTimeout = 2;
                // cmd.CommandText = string.Format(" delete from {0} where  id='{1}'", tableName, Sn);
                cmd.CommandText = string.Format(" select count(*) from {0} where  SN='{1}'", tableName, SN);

                var count0 = cmd.ExecuteScalar();

                if (Convert.ToInt32(count0) > 0)
                    // cmd.CommandText = string.Format(" Update {0}  (userid, time, type) values('{1}','{2}','{3}')", tableName, idd.ToString(), time, type);
                    cmd.CommandText = string.Format(" Update {0} set {1}='{2}' where  SN='{3}'", tableName, ITEM, VALUE, SN);
                else
                    // cmd.CommandText = string.Format(" insert into {0}(userid, time, type) value({1},{2},{3})", tableName,idd.ToString() , time,type);
                    cmd.CommandText = string.Format(" insert into {0}('{1}',SN) values('{2}','{3}')", tableName, ITEM, VALUE, SN);

                vv = cmd.ExecuteNonQuery();

            }
            finally
            { // 释放写锁 
                _rwlock.ReleaseWriterLock();
            }

        }

        public void InsertRecordBlockData(string tableName, string SN, List<string> ITEMs, List<string> VALUEs)
        {
            int vv = -2;


            try
            {
                _rwlock.AcquireWriterLock(2200);
                var cmd = Conn.CreateCommand();
                cmd.CommandTimeout = 2;

                cmd.CommandText = string.Format(" select count(*) from {0} where  SN='{1}'", tableName, SN);

                var count0 = cmd.ExecuteScalar();

                if (Convert.ToInt32(count0) > 0)
                {
                    cmd.CommandText = " Update  " + tableName + " set";

                    for (int i = 0; i < ITEMs.Count; i++)
                    {
                        cmd.CommandText += "  " + ITEMs[i] + "=" + @"'" + VALUEs[i] + @"' ,";
                    }
                    cmd.CommandText = cmd.CommandText.TrimEnd(",".ToCharArray()) + " where  SN=" + @" '" + SN + @"' ";
                }
                else
                {
                    string CommandText0 = "";

                    for (int i = 0; i < ITEMs.Count; i++)
                    {
                        CommandText0 += @"  '" + ITEMs[i] + @"'  ,";
                    }
                    CommandText0 = CommandText0.TrimEnd(",".ToCharArray());


                    string CommandText1 = "";

                    for (int i = 0; i < VALUEs.Count; i++)
                    {
                        CommandText1 += @"  '" + VALUEs[i] + @"'  ,";
                    }
                    CommandText1 = CommandText1.TrimEnd(",".ToCharArray());

                    cmd.CommandText = string.Format(" insert into {0}({1},SN) values({2},'{3}')", tableName, CommandText0, CommandText1, SN);
                }

                vv = cmd.ExecuteNonQuery();

            }
            finally
            { // 释放写锁 
                _rwlock.ReleaseWriterLock();

                //MainFrame.Log("數據庫注册完成： " + SN + "   " + ITEM + "  " + VALUE + "     " + vv + "   " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), MainFrame.logpath, "SUANFA");
            }

        }


        //public void InsertRecord820v4(string tableName, string SN, string ITEM, string VALUE)
        //{
        //    int vv = -2;
        //    try
        //    {
        //        _rwlock.AcquireWriterLock(-1);
        //        try
        //        {
        //            var cmd = Conn.CreateCommand();
        //            cmd.CommandTimeout = 5;
        //            // cmd.CommandText = string.Format(" delete from {0} where  id='{1}'", tableName, Sn);
        //            cmd.CommandText = string.Format(" select count(*) from {0} where  SN='{1}'", tableName, SN);

        //            var count0 = cmd.ExecuteScalar();

        //            if (Convert.ToInt32(count0) > 0)
        //                // cmd.CommandText = string.Format(" Update {0}  (userid, time, type) values('{1}','{2}','{3}')", tableName, idd.ToString(), time, type);
        //                cmd.CommandText = string.Format(" Update {0} set {1}='{2}' where  SN='{3}'", tableName, ITEM, VALUE, SN);
        //            else
        //                // cmd.CommandText = string.Format(" insert into {0}(userid, time, type) value({1},{2},{3})", tableName,idd.ToString() , time,type);
        //                cmd.CommandText = string.Format(" insert into {0}('{1}',SN) values('{2}','{3}')", tableName, ITEM, VALUE, SN);

        //            vv = cmd.ExecuteNonQuery();

        //        }
        //        finally
        //        { // 释放写锁 
        //            _rwlock.ReleaseWriterLock();

        //            //MainFrame.Log("數據庫注册完成： " + SN + "   " + ITEM + "  " + VALUE + "     " + vv + "   " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), MainFrame.logpath, "SUANFA");
        //        }
        //    }
        //    catch (ApplicationException EX)
        //    {
        //        //MainFrame.Log("數據庫注册完成： " + SN + "   " + ITEM + "  " + VALUE + "     " + EX.ToString() + "   " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), MainFrame.logpath, "SUANFA");
        //    }
        //}

        public void DataReplace820v4(string tableName, string SN, string Time, string AOI, string MES)
        {
            var cmd = Conn.CreateCommand();
            cmd.CommandText = string.Format("replace into {0}(SN,Time, AOI, MES) values('{1}','{2}','{3}','{4}') Where SN='{1}'", tableName, SN, Time, AOI, MES);
            cmd.ExecuteNonQuery();
        }

        public DataTable GetDataTable(string SelectSql)
        {
            DataTable result = new DataTable();

            _rwlock.AcquireReaderLock(-1);
            try
            {
                var cmd = Conn.CreateCommand();
                cmd.CommandText = SelectSql;
                cmd.CommandTimeout = 5;
                SQLiteDataAdapter adp = new SQLiteDataAdapter();

                adp.SelectCommand = cmd;
                adp.Fill(result);
                return result;

            }
            catch
            {
                return result;
            }
            finally
            { // 释放写锁 
                _rwlock.ReleaseReaderLock();
            }

        }

        public int GetCount(string tableName, string Condict)
        {
            
            try
            {
                _rwlock.AcquireReaderLock(-1);
                try
                {
                    var cmd = Conn.CreateCommand();
                    cmd.CommandTimeout = 5;
                    cmd.CommandText = string.Format(" select count(*) from {0} where  {1}", tableName, Condict);

                    var count = cmd.ExecuteScalar();
                    return Convert.ToInt32(count);

                }
                finally
                { // 释放写锁 
                    _rwlock.ReleaseReaderLock();
                }
             
            }
            catch 
            {
                //MessageBox.Show(ex.ToString());
                return 0;
            }
        }


        public string GetTime(string tableName, string Condict)
        {
            try
            {
                var cmd = Conn.CreateCommand();
                cmd.CommandText = string.Format(" select Time from {0} where  {1} order by Time desc", tableName, Condict);

                var count = cmd.ExecuteScalar();
                return Convert.ToString(count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return "";
            }
        }

        public string GetPN(string tableName, string Condict)
        {
            try
            {
                var cmd = Conn.CreateCommand();
                cmd.CommandText = string.Format(" select PN from {0} where  {1} order by Time desc", tableName, Condict);

                var count = cmd.ExecuteScalar();
                return Convert.ToString(count);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return "";
            }
        }

        public string GetItemValue(string tableName, string Item, string Condict)
        {
            try
            {
                _rwlock.AcquireReaderLock(-1);
                try
                {
                    var cmd = Conn.CreateCommand();
                    cmd.CommandTimeout = 5;
                    cmd.CommandText = string.Format(" select {0} from {1} where  {2} order by Time desc", Item, tableName, Condict);

                    var count = cmd.ExecuteScalar();
                    return Convert.ToString(count);

                }
                finally
                { // 释放写锁 
                    _rwlock.ReleaseReaderLock();
                }

            }
            catch 
            {
                //MessageBox.Show(ex.ToString());
                return "";
            }
        }

    }
}
