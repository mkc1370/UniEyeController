using System;
using SimpleEyeController.Constants;
using UnityEngine;

namespace SimpleEyeController.Model.Setting
{
    [Serializable]
    public class EyeControllerSetting
    {
        [Header("目の可動域（左右, 上下）[オイラー角]")]
        public Vector2 eulerAnglesLimit = new Vector2(15, 10);

        [Header("アップデート方法")]
        public UpdateMethod updateMethod = UpdateMethod.LateUpdate;
    }
}