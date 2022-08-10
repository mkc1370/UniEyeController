using System;
using UniEyeController.Core.Constants;
using UnityEngine;

namespace UniEyeController.Core.Setting
{
    [Serializable]
    public class EyelidSetting
    {
        public EyelidType eyelidType;
        public SkinnedMeshRenderer blendShapeMesh;
        public int[] blendShapeIndexes;
        public string[] blendShapeNames;
    }
}