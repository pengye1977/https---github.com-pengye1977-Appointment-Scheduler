using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPoco
{
    public class DBCommandEventArgs :EventArgs
    {
        public IDbCommand DbCommand { get; set; }   
        public DBCommandEventArgs()
        {
            
        }

        public DBCommandEventArgs(IDbCommand iDbCommand)
        {
            DbCommand = iDbCommand;
        }
    }
}
