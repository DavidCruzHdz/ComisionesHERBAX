using servicioComisiones.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.Net;
using System.Net.Mail;

namespace servicioComisiones
{
    public class Comisiones : UserNamePasswordValidator, IComisiones
    {
        private comsionesData _cData = new comsionesData();

        string currentUser { get; set; }

        public override void Validate(string userName, string password)
        {
            string strUser = System.Configuration.ConfigurationManager.AppSettings["userAutenticationService"];
            string strPass = System.Configuration.ConfigurationManager.AppSettings["userAutenticationService"];
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("userName");
            }
            else if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }
            else if (userName == strUser && password == strPass)
            {
                currentUser = userName;
            }
            else
            {
                throw new NotImplementedException();
            }

        }

        /// <summary>
        /// Proceso que genera el flujo y las acciones de  autorizacion de prestamo o la negacion del mismo.
        /// si es una autorizacion:
        ///     1. El proceso debe de mandar el correo a los destinatarios correspondientes de enterado
        ///     2. Generar los movimientos de cobros con los montos prorrateados
        ///     3. 
        /// </summary>
        /// <param name="idPrestamo"></param>
        /// <param name="idPartner"></param>
        /// <param name="accionSolicitud"></param>
        public void confirmarPrestamo(int idPrestamo, int idPartner, int accionSolicitud)
        {
            string strHTMLBody = string.Empty;
            strHTMLBody = _cData.generaProgramacionDeCobro(idPrestamo, idPartner, accionSolicitud);

            if (!string.IsNullOrEmpty(strHTMLBody))
            {
                if (strHTMLBody.Length > 10)
                {
                    sendMail("hecmar87@gmail.com", strHTMLBody);
                }
            }

        }

        private void sendMail(string pNombreA, string pHtmlBody)
        {

            //Archivos a adjuntar
            List<String> strFiles = default(List<String>);
            Attachment data = null;

            //////Correo central de envios de solitudes
            var fromAddress = new MailAddress("hector.hernandez@grupovitek.com", "HERBAX de Mexico");
            string fromPassword = "1Vitek1961";

            ///Obtener el correo del autorizador de la solicitud
            //var toAddress = new MailAddress("hecmar87@gmail.com");//--- correo de la persona que manda la solicitud (Mariano)
            var toAddress = pNombreA;

            ///Asunto del correo
            string subject = "Solicitud de préstamo";

            //Contenido
            string body = pHtmlBody;

            //Adjunto
            if (strFiles != null)
            {
                if (strFiles.Count > 0)
                {
                    data = new Attachment("pathFile", System.Net.Mime.MediaTypeNames.Application.Octet);
                    System.Net.Mime.ContentDisposition disposition = data.ContentDisposition;
                    disposition.CreationDate = System.IO.File.GetCreationTime("pathFile");
                    disposition.ModificationDate = System.IO.File.GetLastWriteTime("pathFile");
                    disposition.ReadDate = System.IO.File.GetLastAccessTime("pathFile");
                }
            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            var message = new MailMessage("hector.hernandez@grupovitek.com", toAddress);
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;

            //CC
            //message.CC.Add(new MailAddress("mhernandez@herbax.com.mx"));
            //message.CC.Add(new MailAddress("ammartinez@herbax.com.mx"));
            //message.CC.Add(new MailAddress("valentin.santos@grupovitek.com.mx"));

            if (strFiles != null)
            {
                if (strFiles.Count > 0)
                {
                    message.Attachments.Add(data);
                }
            }

            smtp.Send(message);

        }

    }
}
