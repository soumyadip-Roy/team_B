using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestVoterApplication
{
    public abstract class Database
    {
        public abstract void Fetchfromtable();
        public abstract void Updatetotable();
        public abstract void Deletefromtable();
        public abstract void Savetotable();
    }
}
