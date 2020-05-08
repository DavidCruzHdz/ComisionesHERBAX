namespace servicioComisiones.Interfaz
{
    public interface IcomisionesData
    {
        string generaProgramacionDeCobro(int idPrestamo, int idPartner, int accionSolicitud);
    }
}
