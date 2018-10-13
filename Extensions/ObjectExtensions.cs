using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace ConsoleApp1.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object @this, bool indent = false )
        {
            return JsonConvert.SerializeObject(@this, indent ? Formatting.Indented : Formatting.None);
        } 
    }
}
