using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OracleClient;

namespace FeriaVirtual.Negocio
{
    public class ConexionOracle
    {
        public OracleConnection Conexion()
        {
            OracleConnection ora = new OracleConnection("DATA SOURCE = localhost:1521/pdborcl; PASSWORD = fvadm; USER ID = fvadm;");
            return ora;
        }
    }
}
