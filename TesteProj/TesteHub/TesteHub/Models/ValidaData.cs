using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesteHub.Models
{
    public static class ValidaData
    {
        public static bool IsDate(string dtValida)
        {
            try
            {
                var c = DateTime.Parse(dtValida);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}