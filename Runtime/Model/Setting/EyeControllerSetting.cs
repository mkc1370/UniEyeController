using System;
using UnityEngine;

namespace SimpleEyeController.Model.Setting
{
    [Serializable]
    public class EyeControllerSetting
    {
        /// <summary>
        /// 左右に動かす範囲と上下に動かす範囲をオイラー角で指定する
        /// </summary>
        [Header("左右に動かす範囲と上下に動かす範囲をオイラー角で指定する")]
        public Vector2 eulerAnglesLimit = new Vector2(20, 10);

        [Header("アップデート方法")]
        public UpdateMethod updateMethod = UpdateMethod.LateUpdate;
    }
}