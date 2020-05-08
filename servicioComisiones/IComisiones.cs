namespace servicioComisiones
{
    using System.ServiceModel;

    /// <summary>
    /// Servicio integral de comisiones, expuesto a la red mundial para autorizaciones de prestamo.
    /// </summary>
    [ServiceContract]
    public interface IComisiones
    {
        [OperationContract]
        void confirmarPrestamo(int idPrestamo, int idPartner, int accionSolicitud);

    }
}
