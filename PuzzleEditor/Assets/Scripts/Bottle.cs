using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Bottle : MonoBehaviour
    {
        public int maxAmount { get; private set; }
        public int currentAmount { get; private set; }

        /// <summary>
        /// 加水/倒水
        /// </summary>
        /// <param name="amount">加水(正数)/倒水(负数)的量</param>
        /// <returns>true : 加水/倒水成功</returns>
        public bool AddWater(int amount)
        {
            int restAmount = this.currentAmount + amount;
            if ( (restAmount < 0) || (restAmount > maxAmount) )
            {
                return false;
            }

            return true;
        }
    }
}