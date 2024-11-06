using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    public static class InteropFactory
    {
        public static ISoftDentInterop GetSoftDentInterop()
        {
            return (ISoftDentInterop)SoftDentInterop.Instance();
        }

        public static ISoftDentInterop SoftDent
        {
            get
            {
                return InteropFactory.GetSoftDentInterop();
            }
        }
    }
}
