using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Laziness;

class TypeName
{
    public StringBuilder X => _x.Value;
    private LazyMixin<StringBuilder> _x;
}

class A { }
struct B { }
class C
{
    private A _a;
    private B _b;
    public readonly A[] _c;
    public A[,] _d;
    protected readonly B[] _e;
    protected B[,] _f;
}
