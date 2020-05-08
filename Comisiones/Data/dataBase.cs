namespace Comisiones.Data
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Reflection;

    public class dataBase
    {

        #region Metodos Globales
        protected SqlConnection cnn_Evo_Comisiones
        {
            get
            {
                return new SqlConnection(ConfigurationManager.ConnectionStrings["cnnEvo_Comisiones"].ConnectionString);
            }
        }

        public static T readerToObject<T>(IDataRecord reader) where T : class, new()
        {
            try
            {
                T obj = new T();
                foreach (var prop in obj.GetType().GetProperties())
                {
                    try
                    {
                        PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                        propertyInfo.SetValue(obj, Convert.ChangeType(reader[prop.Name], propertyInfo.PropertyType), null);
                    }
                    catch
                    {
                        continue;
                    }
                }
                return obj;
            }
            catch
            {
                return null;
            }
        }

        protected IEnumerable<IDataRecord> GetFromReader(IDataReader reader)
        {
            while (reader.Read()) yield return reader;
        }
        #endregion


    }
}