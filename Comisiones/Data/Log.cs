using System;
using System.IO;

namespace Comisiones.Data
{
    /*public class Log
    {
    }*/
    public class Log
    {
        private string Path = "";


        public Log(string Path)
        {
            this.Path = Path;
        }

        public void Add(string nameController, string sLog)
        {
            CreateDirectory();
            string nombre = GetNameFile(nameController);
            string cadena = "";

            cadena += DateTime.Now + " - " + sLog + Environment.NewLine;

            StreamWriter sw = new StreamWriter(Path + "/" + nombre, true);
            sw.Write(cadena);
            sw.Close();

        }

        #region HELPER
        private string GetNameFile(string nameController)
        {
            string nombre = "";

            nombre = "log_" + nameController + "__" + DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day + ".txt";

            return nombre;
        }

        private void CreateDirectory()
        {
            try
            {
                if (!Directory.Exists(Path))
                    Directory.CreateDirectory(Path);


            }
            catch (DirectoryNotFoundException ex)
            {
                throw new Exception(ex.Message);

            }
        }
        #endregion
    }
}