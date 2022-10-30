using System;
using UniEyeController.Core.Controller.Eyelid.Constants;
using UnityEngine;

namespace UniEyeController.Core.Controller.Eyelid
{
    [Serializable]
    public partial class EyelidSetting
    {
        public EyelidType eyelidType;
        public SkinnedMeshRenderer blendShapeMesh;
        public int[] blendShapeIndexes;
        public string[] blendShapeNames;
    }
}