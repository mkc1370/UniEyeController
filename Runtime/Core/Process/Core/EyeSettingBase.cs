using System;
using UnityEngine;

namespace UniEyeController.Core.Process.Core
{
    [Serializable]
    public class EyeSettingBase
    {
        /// <summary>
        /// 適用度合い
        /// </summary>
        [Range(0f, 1f)]
        public float weight = 1f;
    }
}