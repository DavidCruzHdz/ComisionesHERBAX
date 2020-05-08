namespace Comisiones.Data.Comisiones
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    public class dataComisiones : dataBase
    {

        public static dataComisiones instance
        {
            get
            {
                return new dataComisiones();
            }
        }

        public List<CO_st_registrosPago_pr_Result> CO_st_registrosPago_pr(string TipoConsulta, int compañia)
        {
            List<CO_st_registrosPago_pr_Result> _objResponse = new List<CO_st_registrosPago_pr_Result>();
            SqlCommand dbC = new SqlCommand("dbo.CO_st_registrosPago_pr", base.cnn_Evo_Comisiones);
            dbC.CommandType = CommandType.StoredProcedure;
            try
            {

                dbC.Parameters.Add("@compania", SqlDbType.Int).Value = compañia;
                dbC.Parameters.Add("@TipoConsulta", SqlDbType.Char).Value = TipoConsulta;

                dbC.Connection.Open();
                foreach (IDataRecord record in base.GetFromReader(dbC.ExecuteReader()))
                {
                    _objResponse.Add(readerToObject<CO_st_registrosPago_pr_Result>(record));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbC.Connection.Close();
                SqlConnection.ClearAllPools();
            }

            return _objResponse;
        }


    }
}