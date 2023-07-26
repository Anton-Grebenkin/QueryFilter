using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QueryFilter.Tests.Extensions
{
    internal static class EnumerableExtensions
    {
        internal static T GetRandom<T>(this IEnumerable<T> source) where T : class
        {
            var random = new Random();
            int index = random.Next(source.Count());
            return source.ElementAt(index);
        }
    }
}
