using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PourWaterPuzzle.Utility
{
    public static class VectorUtility
    {
        /// <summary>
        /// y值设定
        /// </summary>
        /// <param name="target">设定对象</param>
        /// <param name="value">设定y值</param>
        /// <returns>设定后的target</returns>
        public static Vector3 SetY(this Vector3 target, float value)
        {
            target = new Vector3(target.x, value, target.z);
            return target;
        }

        /// <summary>
        /// z值设定
        /// </summary>
        /// <param name="target">设定对象</param>
        /// <param name="value">设定z值</param>
        /// <returns>设定后的target</returns>
        public static Vector3 SetZ(this Vector3 target, float value)
        {
            target = new Vector3(target.x, target.y, value);
            return target;
        }
    }
}
