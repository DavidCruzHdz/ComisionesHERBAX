﻿namespace Comisiones
{
    using System;
    using System.Web;

    public static class sesion
    {
        #region claims of the page
        private static object getsesion(string _key)
        {
            if (HttpContext.Current.Session == null || HttpContext.Current.Session[_key] == null)
            {
                return null;
            }
            else
                return HttpContext.Current.Session[_key];

            return null;
        }

        private static void setSesion(string _key, object value)
        {
            HttpContext.Current.Session[_key] = value;
        }
        #endregion

        #region manejo de variables de sesion de la pagina
        public static Boolean EstatusCalculo
        {
            get
            {
                try
                {
                    return (Boolean)getsesion("EstatusCalculo");
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                setSesion("EstatusCalculo", value);
            }
        }

        public static String urlMain
        {
            get
            {
                try
                {
                    return (String)getsesion("urlMain");
                }
                catch
                {
                    return string.Empty;
                }
            }
            set
            {
                setSesion("urlMain", value);
            }
        }

        /*
        public static Comisiones.Models.ModelsEvolution.CU_tb_Periodo periodoActivo
        {
            get
            {
                try
                {
                    return (Comisiones.Models.ModelsEvolution.CU_tb_Periodo)getsesion("periodoActivo");
                }
                catch
                {
                    return default(Comisiones.Models.ModelsEvolution.CU_tb_Periodo);
                }
            }
            set
            {
                setSesion("periodoActivo", value);
            }
        }*/
        #endregion
    }
}