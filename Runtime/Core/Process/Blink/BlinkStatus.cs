using System;
using UniEyeController.Core.Process.Core;
using UnityEngine;

namespace UniEyeController.Core.Process.Blink
{
    [Serializable]
    public struct BlinkStatus : IEyeStatusBase
    {
        [HideInInspector]
        public bool blinkOffFromOutside;
    }
}