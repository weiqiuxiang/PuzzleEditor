using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public enum PuzzleManagerState
    {
        Init = 1,               //初期化
        SelectBottle = 2,       //选择杯子
        PourWaterAnimation = 3, //倒水动画
        VictoryCheck = 4,       //胜利判定
    }

    public class PuzzleManager : MonoBehaviour
    {
        public UnityEvent OnVictory = null;         //谜题解开时的callback
        private int bottleAmount = 0;               //杯子数量
        private List<Bottle> bottleList = null;     //杯子列表
        public List<Bottle> BottleList => (bottleList ?? (bottleList = new List<Bottle>()));
        private int victoryBottleId = 0;            //胜利条件中的杯子id
        private int victoryAmount = 0;              //胜利条件中的杯子容量

        void OnEnable()
        {

        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //GameLoop

        }
    }
}