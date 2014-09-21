using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EyeKey
{
    public static class GazeHelper
    {
        private static bool isEnabled = true;

        public static bool Enabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                isEnabled = value;
            }
        }
    }
}
