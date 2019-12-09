using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Utility
{
    public static class ImageUtility
    {
        /// <summary>
        /// alpha值设定
        /// </summary>
        /// <param name="target">设定对象</param>
        /// <param name="alpha">设定对象的alpha值</param>
        public static void SetAlpha (this Image target, float alpha)
        {
            var color = target.color;
            color.a = alpha;
            target.color = color;
        }
    }
}
