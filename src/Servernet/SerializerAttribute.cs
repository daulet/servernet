﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servernet
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SerializerAttribute : Attribute
    {
    }
}
