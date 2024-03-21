using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Danejw.Attribute
{
    public static class DesignSettingsManager
    {
        private static DesignSettings instance;

        public static DesignSettings Instance
        {
            get
            {
                if (instance == null)
                    instance = Resources.Load<DesignSettings>("DesignSettings");
                return instance;
            }
        }
    }
}
