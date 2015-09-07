using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ConsoleApplication1
{
    class TypeName
    {
        public StringBuilder X => _x.Value;

        private Laziness.LazyMixin<StringBuilder> _x;
    }
}
