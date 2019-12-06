using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _17599075_PROG7312_Task1
{
    class RefList 
    {

        public string refName { get; set; }
        public string version { get; set; }
       
       
        public override string ToString()
        {
            return "Assembly Name : " + refName + " Version " + version;
        }

       
    }
}
