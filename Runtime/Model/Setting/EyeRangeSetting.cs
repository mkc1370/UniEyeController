using System;
using UnityEngine;

namespace SimpleEyeController.Model.Setting
{
    /// <summary>
    /// 目の可動域の設定
    /// </summary>
    [Serializable]
    public class EyeRangeSetting
    {
        [Header("目の可動域（左右, 上下）[オイラー角]")]
        public Vector2 eulerAnglesLimit = new Vector2(15, 10);
    }
}