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
using System.Runtime.InteropServices;

namespace Demo.driver.sqlite
{
    public class TestRecordManager
    {
        Dictionary<string, DataTable> dicDT;
        Dictionary<string, BindingSource> dicBS;
        Dictionary<string, SQLiteDataAdapter> dicAdp;
        private DataTable SchemaTable;
        private SQLiteConnection conn;
        private string tableName;

        public static DataTable GetDataTable(SQLiteConnection conn, string SelectSql)
        {
            SQLiteCommand command = conn.CreateCommand();
            command.CommandText = SelectSql;
            SQLiteDataAdapter adapter = new SQLiteDataAdapter();
            DataTable dataTable = new DataTable();
            adapter.SelectCommand = command;
            adapter.Fill(dataTable);
            return dataTable;
        }

        public static string CreateConnectstring(string dbfile)
        {
            return string.Format("data source={0}", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbfile));
        }


        public  SQLiteDataAdapter GetAdpter(SQLiteConnection conn, string tableName, [Optional, DefaultParameterValue("")] string where)
        {
            this.conn = conn;
            this.tableName = tableName;

            this.GetSchemaTable();

            SQLiteDataAdapter adapter = new SQLiteDataAdapter
            {
                SelectCommand = this.conn.CreateCommand()
            };
            string str = "select * from " + this.tableName;
            if (!string.IsNullOrEmpty(where))
            {
                str = str + " where " + where;
            }
            adapter.SelectCommand.CommandText = str;
            string[] strArray = ((IEnumerable<string>)EnumerableRowCollectionExtensions.Select<DataRow, string>(DataTableExtensions.AsEnumerable(this.SchemaTable), r => r["ColumnName"].ToString())).ToArray<string>();
            string[] source = (from c in strArray select $"[{c}]=@{c}").ToArray<string>();
            adapter.InsertCommand = this.conn.CreateCommand();
            string str2 = string.Join(",", (from c in strArray select $"@{c}").ToArray<string>()).Replace("@ID", "NULL");
            adapter.InsertCommand.CommandText = $"insert into {this.tableName} values({str2})";
            adapter.InsertCommand.Parameters.AddRange(this.GetParameters());
            string str3 = string.Join(",", source.Skip<string>(1).ToArray<string>());
            adapter.UpdateCommand = this.conn.CreateCommand();
            adapter.UpdateCommand.CommandText = $"update {this.tableName} set {str3} where {source[0]}";
            adapter.UpdateCommand.Parameters.AddRange(this.GetParameters());
            adapter.DeleteCommand = this.conn.CreateCommand();
            adapter.DeleteCommand.CommandText = $"delete from {this.tableName} where {source[0]}";
            adapter.DeleteCommand.Parameters.Add(this.CreateParameter(this.SchemaTable.Rows[0]["ColumnName"].ToString(), this.SchemaTable.Rows[0]["DataType"].ToString()));
            return adapter;
        }



        private SQLiteParameter[] GetParameters()
        {
            var rows = EnumerableRowCollectionExtensions.Select(DataTableExtensions.AsEnumerable(this.SchemaTable), r => new {
                name = r["ColumnName"].ToString(),
                type = r["DataType"].ToString()
            });
            List<SQLiteParameter> list = new List<SQLiteParameter>();
            foreach (var type in rows)
            {
                SQLiteParameter item = this.CreateParameter(type.name.Trim(), type.type.Trim());
                list.Add(item);
            }
            return list.ToArray();
        }


        private SQLiteParameter CreateParameter(string colname, string coltype)
        {
            DbType dbType = DbType.Object;
            switch (coltype)
            {
                case "System.Int32":
                    dbType = DbType.Int32;
                    break;

                case "System.Int64":
                    dbType = DbType.Int64;
                    break;

                case "System.String":
                    dbType = DbType.String;
                    break;

                case "System.Boolean":
                    dbType = DbType.Boolean;
                    break;
            }
            return new SQLiteParameter(colname, dbType, colname);
        }


        public DataTable GetSchemaTable()
        {
            SQLiteCommand command = conn.CreateCommand();
            command.CommandText = $"select * from {this.tableName}  LIMIT 1";
            this.SchemaTable = command.ExecuteReader().GetSchemaTable();
            return this.SchemaTable;
        }








        public TestRecordManager(string dbFileName)
        {
            try
            {
                dicDT = new Dictionary<string, DataTable>();
                dicBS = new Dictionary<string, BindingSource>();
                dicAdp = new Dictionary<string, SQLiteDataAdapter>();

                Conn = new SQLiteConnection(CreateConnectstring(dbFileName));

                Conn.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public BindingSource BindingSources(string tableName, bool Clear = false)
        {
            try
            {
                BindingSource bs;
                if (Clear)
                {
                    dicDT.Remove(tableName);
                    dicBS.Remove(tableName);
                    dicAdp.Remove(tableName);
                }

                if (dicBS.TryGetValue(tableName, out bs))
                    return bs;
                bs = new BindingSource();

                bs.DataSource = getDatatable3(tableName);
                dicBS.Add(tableName, bs);
                return bs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SQLiteConnection Conn;// = AppConfig.DbConn;

        DataTable getDatatable(string tableName)
        {
            try
            {
                DataTable dt;

                if (dicDT.TryGetValue(tableName, out dt))
                    return dt;

                dt = GetDataTable(Conn, string.Format("select * from {0} where id<0 ", tableName));
                dt.Columns["ID"].AutoIncrement = true;
                dt.Columns["ID"].AutoIncrementSeed = 1;
                dt.Columns["ID"].AutoIncrementStep = 1;
                dicDT.Add(tableName, dt);
                var adp = GetAdpter(Conn, tableName, "id<0");
                dicAdp.Add(tableName, adp);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getDatatable2(string tableName)
        {
            try
            {
                DataTable dt;
                //Dictionary<string, DataTable> dicDT8=new Dictionary<string, DataTable>();
                dt = GetDataTable(Conn, string.Format("select * from {0} where id>0 ", tableName));
                //dicDT8.Add(tableName, dt);
                //if (dicDT8.TryGetValue(tableName, out dt))
                //    return dt;
                int aa = dt.Rows.Count;
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getDatatable3(string tableName)
        {
            try
            {
                DataTable dt;

                if (dicDT.TryGetValue(tableName, out dt))
                    return dt;

                dt = GetDataTable(Conn, string.Format("select * from {0} where id>0 ", tableName));
                dicDT.Add(tableName, dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public void DeletRecord(string tableName, string Sn)
        {
            try
            {
                var cmd = Conn.CreateCommand();
                cmd.CommandText = string.Format(" delete from {0} where  id='{1}'", tableName, Sn);
                cmd.ExecuteNonQuery();
                //string updateQuery = "Update studentInfo set sName=‘小李‘" + "Where ID=‘200131500145‘";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetSnCount(string tableName, string Sn)
        {
            try
            {
                var cmd = Conn.CreateCommand();
                cmd.CommandText = string.Format(" select count(*) from {0} where  id='{1}'", tableName, Sn);

                var count = cmd.ExecuteScalar();
                return Convert.ToInt32(count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Get_Mode_Record(string tableName, string idd, string F1)
        {
            try
            {
                string result = "";
                SQLiteDataReader reader;
                var cmd = Conn.CreateCommand();

                cmd.CommandText = string.Format(" select {0} from {1} where  id='{2}'", F1.ToString(), "FingerM", idd.ToString());
                reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        result = reader[0].ToString();
                    }
                }
                reader.Close();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Get_uinfo_Record(string tableName, string idd)
        {
            try
            {
                string result = "";
                SQLiteDataReader reader;
                var cmd = Conn.CreateCommand();

                cmd.CommandText = string.Format(" select * from {0} where  id='{1}'", tableName, idd.ToString());
                reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        for (int j = 2; j < reader.FieldCount - 4; j++)
                            result += reader[j].ToString() + ",";
                    }
                }
                reader.Close();
                return result.TrimEnd(',');
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void TruncatTable(string tableName)
        {
            try
            {
                var cmd = Conn.CreateCommand();
                cmd.CommandText = string.Format(" delete from {0} where  id>0", tableName);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void DataRecord_COPY(string tableName, string idd, string time, string type)
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DataDelet(string tableName, string idd)
        {
            try
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
                    int sa = cmd.ExecuteNonQuery();


                    if (result != "" && sa > 0)
                    {
                        cmd.CommandText = string.Format(" update {0} set id=id-1 where  id>{1}", tableName, result);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void DataRecordui(string tableName, List<string> FR)
        {
            SQLiteTransaction tranction = Conn.BeginTransaction();
            try
            {
                Dictionary<string, string> RecD= CreateDic(tableName, FR);
                if (RecD == null ? true: RecD.Count<0)
                {
                    return;
                }

                if (!RecD.Keys.Contains("userid"))
                    return;

                
                var cmd = Conn.CreateCommand();
                cmd.CommandText = string.Format(" select count(*) from {0} where  userid='{1}'", tableName, RecD["userid"]);

                var count0 = cmd.ExecuteScalar();

                if (Convert.ToInt32(count0) <= 0)
                {
                    cmd.CommandText = string.Format(" insert into {0}(userid) values('{1}')", tableName, RecD["userid"]);
                    cmd.ExecuteNonQuery();                  
                }

                foreach (KeyValuePair<string, string> item in RecD)
                {

                    cmd.CommandText = $" Update {tableName} set {item.Key}='{item.Value}' where  userid='{RecD["userid"]}'";
                    cmd.ExecuteNonQuery();
                }
                tranction.Commit();
            }
            catch (Exception ex)
            {
                tranction.Rollback();
                throw ex;
            }
        }

        private Dictionary<string, string> CreateDic(string TbName,List<string> FR)
        {
            try
            {
                Dictionary<string, string> dcWValue = new Dictionary<string, string>();

                switch (TbName)            
                {
                    case "userinfo":
                        dcWValue.Add("flag", "");
                        dcWValue.Add("userType", "");
                        dcWValue.Add("userid", "");
                        dcWValue.Add("FingerID1", "");
                        dcWValue.Add("FingerID2", "");
                        dcWValue.Add("FingerID3", "");
                        dcWValue.Add("FingerID4", "");
                        dcWValue.Add("password", "");
                        dcWValue.Add("password_len", "");
                        dcWValue.Add("rfid", ""); 
                        dcWValue.Add("VT", "");

                        for (int index = 0; index < FR.Count; index++)
                        {
                            dcWValue[dcWValue.ElementAt(index).Key] = FR[index];                     
                        }

                        break;

                    default:

                        break;
                }

                return dcWValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DataRecordui_mode(string tableName, string idd, string F1)
        {
            try
            {
                var cmd = Conn.CreateCommand();
                cmd.CommandText = string.Format("replace into {0} (Fid,Content) values('{1}','{2}')", tableName, idd, F1);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DataRecordImageRes(string tableName,DateTime TestingTime, string SN, string StationCode,int Channel,string JResultOrig,string JResultFinal,bool Conclution)
        {
            try
            {
                var cmd = Conn.CreateCommand();
                cmd.CommandText = string.Format("replace into {0} values('{1}','{2}','{3}',{4},'{5}','{6}',{7})", tableName, TestingTime, SN,  StationCode,  Channel,  JResultOrig,  JResultFinal, Conclution);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateRecord(string tableName, string Sn)
        {
            try
            {
                var cmd = Conn.CreateCommand();
                cmd.CommandText = string.Format(" Update {0} set name='{1}' Where id='3'", tableName, Sn);
                cmd.ExecuteNonQuery();
                //string updateQuery = "Update studentInfo set sName=‘小李‘" + "Where ID=‘200131500145‘";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateDataBase(SQLiteConnection conn, DataTable dt, string TableName)
        {

            SQLiteTransaction tranction = conn.BeginTransaction();

            try
            {
                string strSql = string.Format("select * from {0}", TableName);

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(strSql, conn);

                SQLiteCommandBuilder builder = new SQLiteCommandBuilder(adapter);


                var cmd = conn.CreateCommand();

                cmd.CommandText = string.Format("delete from {0}", TableName);

                cmd.ExecuteNonQuery();

                DataSet ds = new DataSet();

                adapter.Fill(ds, "flag");

                DataTable table = ds.Tables["flag"];

                table.Clear();
                //将新数据传到对应的datatable中

                foreach (DataRow drNew in dt.Rows)
                {
                    if (drNew.RowState == DataRowState.Deleted)
                        continue;

                    DataRow row = table.NewRow();


                    foreach (DataColumn dc in table.Columns)
                    {
                        row[dc.ColumnName] = drNew[dc.ColumnName];
                    }
                    table.Rows.Add(row);

                }
                //更新数据库
                adapter.Update(table);


            }
            catch (Exception ex)
            {
                tranction.Rollback();
                throw ex;
            }
            finally
            {
                tranction.Commit();
            }
        }

    }
}
