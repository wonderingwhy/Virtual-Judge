using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;

namespace Helpers
{
    public static class SqlHelper
    {
        public static readonly string connstr =
            ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;

        public static void OpenConnection()
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();
            }
        }
        public static int ExecuteNonQuery(string cmdText,
            params MySqlParameter[] parameters)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();
                return ExecuteNonQuery(conn, cmdText, parameters);
            }
        }

        public static object ExecuteScalar(string cmdText,
            params MySqlParameter[] parameters)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();
                return ExecuteScalar(conn, cmdText, parameters);
            }
        }

        public static DataTable ExecuteDataTable(string cmdText,
            params MySqlParameter[] parameters)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();
                return ExecuteDataTable(conn, cmdText, parameters);
            }
        }

        public static int ExecuteNonQuery(MySqlConnection conn, string cmdText,
           params MySqlParameter[] parameters)
        {
            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }

        public static object ExecuteScalar(MySqlConnection conn, string cmdText,
            params MySqlParameter[] parameters)
        {
            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteScalar();
            }
        }

        public static DataTable ExecuteDataTable(MySqlConnection conn, string cmdText,
            params MySqlParameter[] parameters)
        {
            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
        public static bool ExecuteTransaction(string connectionString, List<string> cmdTexts)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                conn.Open();
                using (MySqlTransaction myTrans = (MySqlTransaction)conn.BeginTransaction())
                {
                    using (MySqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = myTrans;
                        try
                        {
                            foreach (string i in cmdTexts)
                            {
                                cmd.CommandText = i;
                                cmd.ExecuteNonQuery();
                            }

                            myTrans.Commit();
                        }
                        catch
                        {
                            myTrans.Rollback();
                            return false;
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
            }
            return true;
        }
        public static object ToDBValue(this object value)
        {
            return value == null ? DBNull.Value : value;
        }

        public static object FromDBValue(this object dbValue)
        {
            return dbValue == DBNull.Value ? null : dbValue;
        }
    }
}
