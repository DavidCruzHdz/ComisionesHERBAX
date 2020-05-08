namespace Comisiones.Data.Prestamo
{
    using Entidades;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;

    public class dataPrestamo : dataBase
    {

        public static dataPrestamo instance
        {
            get
            {
                return new dataPrestamo();
            }
        }

        public List<socio> obtieneSocios(string partner_id, string FirstName, string LastName)
        {
            List<socio> _objResponse = new List<socio>();
            SqlCommand dbC = new SqlCommand("dbo.CO_st_obtieneSocios", base.cnn_Evo_Comisiones);
            dbC.CommandType = CommandType.StoredProcedure;
            try
            {
                if (!string.IsNullOrEmpty(partner_id))
                    dbC.Parameters.Add("@partner_id", SqlDbType.VarChar).Value = partner_id;

                if (!string.IsNullOrEmpty(FirstName))
                    dbC.Parameters.Add("@FirstName", SqlDbType.VarChar).Value = FirstName;

                if (!string.IsNullOrEmpty(LastName))
                    dbC.Parameters.Add("@LastName", SqlDbType.VarChar).Value = LastName;

                dbC.Connection.Open();
                foreach (IDataRecord record in base.GetFromReader(dbC.ExecuteReader()))
                {
                    _objResponse.Add(readerToObject<socio>(record));
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

        public List<solicitudPrestamo> enviarSolicitudPrestamo(string partner_id)
        {
            List<solicitudPrestamo> _objResponse = new List<solicitudPrestamo>();
            SqlCommand dbC = new SqlCommand("dbo.CO_st_validaCapacidadPago", base.cnn_Evo_Comisiones);
            dbC.CommandType = CommandType.StoredProcedure;
            try
            {
                if (!string.IsNullOrEmpty(partner_id))
                    dbC.Parameters.Add("@partner_id", SqlDbType.VarChar).Value = partner_id;

                dbC.Connection.Open();
                foreach (IDataRecord record in base.GetFromReader(dbC.ExecuteReader()))
                {
                    _objResponse.Add(readerToObject<solicitudPrestamo>(record));
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