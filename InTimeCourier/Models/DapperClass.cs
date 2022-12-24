using Dapper;
using Dapper.ColumnMapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace InTimeCourier.Models
{
    public class DapperClass
    {
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd;
        SqlDataAdapter da;
        DataSet ds = new DataSet();
        private SqlParameter objParm;
        private static string strValue;

        public int Exec_SPrc(string Sprc, Hashtable htable, string connectionstring)
        {
            int retval;
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionstring].ConnectionString.ToString());
            cmd = new SqlCommand(Sprc, conn);



            foreach (DictionaryEntry dist in htable)
            {
                cmd.Parameters.AddWithValue((string)dist.Key, dist.Value);
            }
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            retval = cmd.ExecuteNonQuery();
            conn.Close();
            return retval;
        }

        public IEnumerable<T> Exec_SPrc<T>(string sproc, object parameters, string connectionString, string database = null, bool IsCollumnsMappingEnabled = false)
        {
            connectionString = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
            if (database != null)
            {
                SqlConnectionStringBuilder connStringBuilder = new SqlConnectionStringBuilder(connectionString)
                {
                    InitialCatalog = database
                };
                connectionString = connStringBuilder.ConnectionString;
            }
            if (IsCollumnsMappingEnabled)
            {
                SqlMapper.SetTypeMap(typeof(T), new ColumnTypeMapper(typeof(T)));
            }
            using (conn = new SqlConnection(connectionString))
            {

                conn.Open();
                return conn.Query<T>(sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: 3 * 60).ToList();
            }
        }


        public IEnumerable<T> Exc_Raw<T>(string sql, object parameters, string connectionstring, string database = null, bool IsCollumnsMappingEnabled = false)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[connectionstring].ConnectionString;
            if (database != null)
            {
                SqlConnectionStringBuilder connStringBuilder = new SqlConnectionStringBuilder(connectionString)
                {
                    InitialCatalog = database
                };
                connectionstring = connStringBuilder.ConnectionString;
            }
            if (IsCollumnsMappingEnabled)
            {
                SqlMapper.SetTypeMap(typeof(T), new ColumnTypeMapper(typeof(T)));
            }
            using (conn = new SqlConnection(connectionstring))
            {
                conn.Open();
                return conn.Query<T>(sql, parameters).ToList();
            }
        }

        public DataSet GetDataset(string Sprc, Hashtable htable, string connectionstring)
        {
            ds.Clear();
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionstring].ConnectionString.ToString());
            cmd = new SqlCommand(Sprc, conn);
            if (htable != null)
            {
                foreach (DictionaryEntry dist in htable)
                {
                    cmd.Parameters.AddWithValue((string)dist.Key, dist.Value);
                }
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 3 * 60;
            da = new SqlDataAdapter(cmd);
            try
            {
                da.Fill(ds);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                da.Dispose();
            }
            return ds;
        }

        public string execute_sprc_with_output(string sprc_name, Hashtable htable, string parm_name, string strDBKey)
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings[strDBKey].ConnectionString.ToString());
            cmd = new SqlCommand(sprc_name, conn);
            conn.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (DictionaryEntry dist in htable)
            {
                cmd.Parameters.AddWithValue((string)dist.Key, dist.Value);
            }
            objParm = cmd.Parameters.AddWithValue(parm_name, DbType.String.ToString());
            objParm.Size = 60;
            objParm.Direction = ParameterDirection.Output;
            cmd.ExecuteNonQuery();
            if (cmd.Parameters[parm_name].Value != null)
            {
                strValue = cmd.Parameters[parm_name].Value.ToString();
            }
            else
            {
                strValue = "INVALID";
            }

            conn.Close();
            cmd.Dispose();

            return strValue;
        }

    }
}