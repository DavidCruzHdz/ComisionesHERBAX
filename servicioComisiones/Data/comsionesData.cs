namespace servicioComisiones.Data
{
    public class comsionesData : contextoData, Interfaz.IcomisionesData
    {
        public string generaProgramacionDeCobro(int idPrestamo, int idPartner, int accionSolicitud)
        {
            string strRespuesta = string.Empty;
            System.Data.DataTable _dtRespuestaSp = getDt("EXEC [dbo].[CO_st_ProgramacionCobro] '" + idPartner + "'," + idPrestamo + "," + accionSolicitud + ";");
            if (_dtRespuestaSp != null)
            {
                if (_dtRespuestaSp.Rows.Count > 0)
                {
                    strRespuesta = _dtRespuestaSp.Rows[0]["cuerpoCorreoNotificacion"].ToString();
                }
            }
            return strRespuesta;
        }
    }
}