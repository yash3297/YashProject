﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pozative
{
    public class DataContractAttribute : Attribute
    {
        public string Namespace { get; set; }

        public string Name { get; set; }
    }
}
