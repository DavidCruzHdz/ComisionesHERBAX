﻿using System;
using System.Web.UI;

namespace servicioComisiones
{
    public partial class puenteConexion : System.Web.UI.Page
    {
        private string webMethodCall { get; set; }
        private proxie.Comisiones.ComisionesClient _proxieComisiones = new proxie.Comisiones.ComisionesClient();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["webMethod"]))
            {
                webMethodCall = Request.QueryString["webMethod"].ToString();
            }

            if (webMethodCall == "AutorizarPrestamo")
            {
                int idPrestamo; int idPartner; int accionSolicitud;

                idPrestamo = int.Parse(Request.QueryString["idPrestamo"].ToString());
                idPartner = int.Parse(Request.QueryString["idPartner"].ToString());
                accionSolicitud = int.Parse(Request.QueryString["accionSolicitud"].ToString());
                _proxieComisiones.confirmarPrestamo(idPrestamo, idPartner, accionSolicitud);
            }

            Page.ClientScript.RegisterOnSubmitStatement(typeof(Page), "closePage", "window.onunload = CloseWindow();");

        }
    }
}