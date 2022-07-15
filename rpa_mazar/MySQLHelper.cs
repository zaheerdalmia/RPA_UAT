﻿using System;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;

namespace rpa_mazar
{
    class MySQLHelper
    {
        public String ConnectionString = ConfigurationManager.AppSettings["MySQLKey"].ToString();
        public MySqlConnection connection;

        #region Initiallize 
        public MySQLHelper()
        {
            Initialize();
        }

        //Initialize values 
        private void Initialize()
        {
            ConnectionString = ReadConnectionString();

            connection = new MySqlConnection(ConnectionString);
        }

        public String ReadConnectionString()
        {
            return ConnectionString = ConfigurationManager.AppSettings["MySQLKey"].ToString();


        }

        #endregion

        #region DB ConnectionOpen 
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return false;
        }
        #endregion
        #region DB Connection Close 
        //Close connection 
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }
        }
        #endregion
        #region ExecuteNonQuery for insert/Update and Delete 
        //For Insert/Update/Delete 
        public int ExecuteNonQuery_IUD(String Querys)
        {
            int result = 0;
            try
            {
                using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                {
                    Logger.Log(Querys, w);
                }
                //open connection 
                if (OpenConnection() == true)
                {
                    //create command and assign the query and connection from the constructor 
                    MySqlCommand cmd = new MySqlCommand(Querys, connection);
                    //Execute command 
                    cmd.CommandTimeout = 0;
                    result = cmd.ExecuteNonQuery();
                    //close connection 
                    //Console.WriteLine("" :result);
                    CloseConnection();
                }
            }
            catch (Exception ex)
            {
                CloseConnection();
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                {
                    Logger.Log(ex.Message + "\n\n" + ex.StackTrace, w);
                }
            }
            return result;

        }
        #endregion

        #region Dataset for select result and return as Dataset 
        //for select result and return as Dataset 
        public DataSet DataSet_return(String Querys)
        {
            DataSet ds = new DataSet();
            //open connection 
            if (OpenConnection() == true)
            {
                //for Select Query              
                MySqlCommand cmdSel = new MySqlCommand(Querys, connection);
                MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
                da.Fill(ds);
                //close connection 
                CloseConnection();
            }
            return ds;
        }
        #endregion

        #region DataTable for select result and return as DataTable 
        //for select result and return as DataTable 
        public DataTable DataTable_return(String Querys)
        {
            DataTable dt = new DataTable();
            try
            {
                using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                {
                    Logger.Log(Querys, w);
                }
                //open connection 
                if (OpenConnection() == true)
                {
                    //for Select Query          

                    MySqlCommand cmdSel = new MySqlCommand(Querys, connection);
                    cmdSel.CommandTimeout = 0;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
                    da.Fill(dt);
                    //close connection 
                    CloseConnection();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                CloseConnection();
                using (StreamWriter w = new StreamWriter(ConfigurationManager.AppSettings["errlog"].ToString() + DateTime.Now.ToString("dd_MMM_yyyy") + ".txt", true))
                {
                    Logger.Log(ex.Message + "\n\n" + ex.StackTrace, w);
                }
            }
            return dt;
        }
        #endregion


        #region Dataset for Stored Procedure and return as DataTable 
        //for select result and return as DataTable 
        public DataSet SP_DataTable_return(String ProcName, params MySqlParameter[] commandParameters)
        {
            DataSet ds = new DataSet();
            //open connection 
            if (OpenConnection() == true)
            {
                //for Select Query                

                MySqlCommand cmdSel = new MySqlCommand(ProcName, connection);
                cmdSel.CommandType = CommandType.StoredProcedure;
                // Assign the provided values to these parameters based on parameter order 
                AssignParameterValues(commandParameters, commandParameters);
                AttachParameters(cmdSel, commandParameters);
                MySqlDataAdapter da = new MySqlDataAdapter(cmdSel);
                da.Fill(ds);
                //close connection 
                CloseConnection();
            }



            return ds;
        }
        private static void AttachParameters(MySqlCommand command, MySqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (MySqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned 
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        private static void AssignParameterValues(MySqlParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                // Do nothing if we get no data 
                return;
            }

            // We must have the same number of values as we pave parameters to put them in 
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            // Iterate through the SqlParameters, assigning the values from the corresponding position in the  
            // value array 
            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                // If the current array value derives from IDbDataParameter, then assign its Value property 
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                    if (paramInstance.Value == null)
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = paramInstance.Value;
                    }
                }
                else if (parameterValues[i] == null)
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = parameterValues[i];
                }
            }
        }
        #endregion
    }
}
