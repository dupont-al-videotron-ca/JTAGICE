using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsWin32Api
{
    public interface IDataWrapper<T> where T : struct
    {
        internal T _src { get; set; }
    }
}
