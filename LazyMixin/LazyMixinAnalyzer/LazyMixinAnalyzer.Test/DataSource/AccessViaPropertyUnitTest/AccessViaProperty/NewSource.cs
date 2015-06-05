using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Laziness;

namespace ConsoleApplication1
{
    class TypeName
    {
        public StringBuilder X => _x.Value;

        private LazyMixin<StringBuilder> _x;
    }
}
