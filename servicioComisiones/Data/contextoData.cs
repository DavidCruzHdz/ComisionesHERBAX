﻿namespace servicioComisiones.Data
{
    using System;
    using System.Data.SqlClient;


    public class contextoData
    {
        private SqlDataAdapter da { get; set; }
        private SqlCommand cmm { get; set; }

        public static contextoData instance
        {
            get
            {
                return new contextoData();
            }
        }

        public System.Data.DataTable getDt(string sqlText)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            Boolean alreadyExists = false;

            foreach (String key in System.Configuration.ConfigurationManager.AppSettings)
            {
                if (key.StartsWith("cnn"))
                {
                    try
                    {
                        if (!alreadyExists)
                        {
                            da = new SqlDataAdapter(sqlText, new SqlConnection(System.Configuration.ConfigurationManager.AppSettings[key]));
                            da.SelectCommand.CommandText = sqlText;
                            da.SelectCommand.CommandType = System.Data.CommandType.Text;
                            da.Fill(dt);
                            alreadyExists = true;
                        }
                    }
                    catch
                    {
                        alreadyExists = false;
                    }
                }

            }

            return dt;
        }



    }
}