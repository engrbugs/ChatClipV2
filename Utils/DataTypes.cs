using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClipV2.Utils
{
    internal class DataTypes
    {
        public class SuccessReturn
        {
            public bool IsSuccessful { get; set; }
            public string? ErrorMessage { get; set; }

            public static implicit operator bool(SuccessReturn successReturn)
            {
                return successReturn.IsSuccessful;
            }
        }

    }
}
