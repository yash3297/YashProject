using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    internal static class SoftDentImportsFactory
    {
        internal static ISoftDentImports GetSoftDentImporter()
        {
            if (SoftDentVersion.MajorPart == 12)
            {
                if (SoftDentVersion.IsClientServer)
                    return (ISoftDentImports)new SoftDentImports12CS();
                return (ISoftDentImports)new SoftDentImports12();
            }
            if (SoftDentVersion.MajorPart == 14 && SoftDentVersion.MinorPart == 0 && SoftDentVersion.BuildPart < 2)
            {
                if (SoftDentVersion.IsClientServer)
                    return (ISoftDentImports)new SoftDentImports14CS();
                return (ISoftDentImports)new SoftDentImports14();
            }
            if (SoftDentVersion.MajorPart == 14 && SoftDentVersion.MinorPart == 0 && SoftDentVersion.BuildPart == 2)
            {
                if (SoftDentVersion.IsClientServer)
                    return (ISoftDentImports)new SoftDentImports14_2CS();
                return (ISoftDentImports)new SoftDentImports14_2();
            }
            if (SoftDentVersion.IsClientServer)
                return (ISoftDentImports)new SoftDentImportsCS();
            return (ISoftDentImports)new SoftDentImports();
        }
    }
}
