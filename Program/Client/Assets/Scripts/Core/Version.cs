using System.Collections;
using System.Collections.Generic;

namespace Chowizard.UnityNetwork.Client.Core
{

    public static class Version
    {
        public const int Major = 1;
        public const int Minor = 4;

        public static uint Code
        {
            get
            {
                return (uint)((Major << 16) | ((Minor << 16) >> 16));
            }
        }

        public static string String
        {
            get
            {
                return string.Format("{0}.{1:00}", Major, Minor);
            }
        }
    }
}
