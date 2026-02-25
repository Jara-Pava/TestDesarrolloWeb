using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessDesarrollos
{
    public class DataAccess
    {
        private List<StoreProcedure> storedProcedureCollection;

        private SqlConnection Connection = null;

        private readonly object ObjLock = new object();

        private readonly string connectionStringName = "StringConnection";

        public DataAccess()
        {
            storedProcedureCollection = new List<StoreProcedure>();
            try
            {
                this.Connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);

                this.Connection.Open();

                var config = (QASConfig)System.Configuration.ConfigurationManager.GetSection("QASConfig");

                for (var i = 0; i < config.StoreProcedures.Count; i++)
                {
                    Trace.TraceInformation("Add SP {0}", config.StoreProcedures[i].Name);
                    storedProcedureCollection.Add(new StoreProcedure { Code = config.StoreProcedures[i].Code, Name = config.StoreProcedures[i].Name });
                }
            }
            catch (Exception exc)
            {
                Trace.TraceError("Error occurred while initializing DataAccess class. Message == > {0}", exc.ToString());
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.storedProcedureCollection.Clear();
                this.storedProcedureCollection = null;
            }
            if (Connection != null)
            {
                this.Connection.Dispose();
                this.Connection = null;
            }
        }
    }
}
