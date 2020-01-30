using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PourWaterPuzzle
{
    public class GameStatic
    {
        # region 动画时间定数
        static readonly public float GameStartFadeInTime = 0.5f;      // 游戏开始时的渐出时间
        static readonly public float BottleFadeOutTime = 0.4f;       // 瓶子的渐出时间
        static readonly public float BottleFadeInTime = 0.4f;        // 瓶子的渐入时间
        static readonly public float BottleFadeMoveTime = 0.4f;      // 拿瓶子时的移动时间
        static readonly public float PourRorateTime = 0.4f;          // 注水时瓶子旋转时间
        static readonly public float PourTime = 0.4f;                // 注水时间
        #endregion

        #region 动画移动定数
        static readonly public float BottleMoveDist = 30f;        // 瓶子的渐出时间
        static readonly public float BottleRotateAngle = -125;    // 瓶子的旋转角度
        #endregion
    }
}