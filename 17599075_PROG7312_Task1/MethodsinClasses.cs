using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _17599075_PROG7312_Task1
{
    class MethodsinClasses : IComparable<MethodsinClasses>
    {
        public string className { get; set; }
        public int methodNum { get; set; }
        public int numClassLines { get; set; }
        public int avLines { get; set ; }




        public override string ToString()
        {
            
            return String.Format("{0} has {1} methods and an average of {2} lines per method.", className, methodNum, avLines);
        }

        public int CompareTo(MethodsinClasses other)
        {
            MethodsinClasses temp = other as MethodsinClasses;
            if (temp != null)
            {
                if (this.avLines < temp.avLines)
                    return 1;
                if (this.avLines > temp.avLines)
                    return -1;
                else
                    return 0;
            }
            else
                throw new ArgumentException("Parameter is not a MethodsinClasses!");
        }

    }
}
