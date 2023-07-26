using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QueryFilter.Presets
{
    internal static class StringPresets
    {
        internal static readonly Type StringType = typeof(string);

        internal static readonly MethodInfo ContainsMethod = StringType.GetRuntimeMethod("Contains", new[] { StringType });
    }
}
