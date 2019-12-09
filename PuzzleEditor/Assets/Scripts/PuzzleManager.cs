using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public enum GameLoop
    {
        Init = 1,               //初期化
        SelectBottle = 2,       //选择杯子
        PourWaterAnimation = 3, //倒水动画
        VictoryCheck = 4,       //胜利判定
        Victory = 5,            //胜利
    }

    public enum SelectBottleState
    {
        From = 1,
        To = 2,
    }

    [RequireComponent(typeof(PuzzleAnimation),typeof(BottlesSettor))]
    public class PuzzleManager : MonoBehaviour
    {
        [Header("Reference")]
        [NonEditable]
        public PuzzleAnimation PuzzleAnimationRef = null;   // 动画播放class的参照
        [NonEditable]
        public BottlesSettor bottlesSettor = null;

        [Header("Event")]
        public UnityEvent OnVictory = null;         // 谜题解开时的callback

        #region 不显示在inspector上的变量
        public List<Bottle> BottleList => this.bottlesSettor.BottleList;
        private GameLoop gameLoopState = GameLoop.Init; // 游戏循环状态
        private SelectBottleState selectBottleState = SelectBottleState.From;
        public int selectedFromBottleId { get; private set; }       // 倒水的瓶子的Id
        public int selectedToBottleId { get; private set; }         // 注水的瓶子的Id
        #endregion

        void OnEnable()
        {
            this.gameLoopState = GameLoop.Init;
            this.selectBottleState = SelectBottleState.From;

            // 播放开始动画，并在动画结束时将执行callback
            this.PuzzleAnimationRef.PlayGameStartAnimation(() => this.gameLoopState = GameLoop.SelectBottle);
        }

        void Update()
        {
            // 游戏循环
            switch (gameLoopState)
            {
                case GameLoop.Init:
                    break;

                case GameLoop.SelectBottle:
                    break;

                case GameLoop.PourWaterAnimation:
                    break;

                case GameLoop.VictoryCheck:
                    break;

                case GameLoop.Victory:
                    break;
            }
        }

        // 倒水
        private void pourWater()
        {
            try
            {
                Bottle from = this.BottleList[this.selectedFromBottleId];
                Bottle to = this.BottleList[this.selectedToBottleId];
                to.ChangeWaterAmount(from.CurrentAmount);
                from.ChangeWaterAmount(-to.ChangeAmount);
            }
            catch
            {
                Debug.LogError("pourWater method Error");
            }
        }

        /// <summary>
        /// 是否解开谜题判定
        /// </summary>
        private void victoryCheck()
        {
            try
            {
                var bottle = this.BottleList[this.bottlesSettor.VictoryTarget];
                if (bottle.CurrentAmount == this.bottlesSettor.VictoryAmount)
                {
                    // 播放胜利时的动画，并在动画结束时执行callback
                    this.PuzzleAnimationRef.PlayVictoryAnimationAnimation(
                        () => 
                        {
                            this.OnVictory?.Invoke();
                            this.gameLoopState = GameLoop.Victory;
                        }
                    );           
                }
                else
                {
                    this.gameLoopState = GameLoop.SelectBottle;
                }
            }
            catch
            {
                Debug.LogError("victoryCheck method Error");
            }
        }

        /// <summary>
        /// 点击杯子的时候
        /// </summary>
        public void OnClickBottle(Bottle bottle)
        {
            if (bottle == null)
            {
                Debug.LogError("bottle is null");
                return;
            }
            
            switch (this.selectBottleState)
            {
                case SelectBottleState.From:
                    // 选择倒水杯子
                    bottle.SetSelected(true);
                    this.selectedFromBottleId = this.BottleList.IndexOf(bottle);
                    if (this.selectedFromBottleId == -1)
                    {
                        Debug.LogError("bottle is no contained by BottleList");
                    }
                    // 下一步
                    this.selectBottleState = SelectBottleState.To;
                    break;

                case SelectBottleState.To:
                    // 选择一样的杯子会取消前边的选择
                    if (this.selectedFromBottleId == this.BottleList.IndexOf(bottle))
                    {
                        bottle.SetSelected(false);
                        this.selectBottleState = SelectBottleState.From;
                        break;
                    }

                    // 选择被倒水杯子
                    this.selectedToBottleId = this.BottleList.IndexOf(bottle);
                    if (this.selectedToBottleId == -1)
                    {
                        Debug.LogError("bottle is no contained by BottleList");
                    }

                    // 倒水
                    this.pourWater();

                    // 设置部分变量的值
                    this.selectBottleState = SelectBottleState.From;
                    this.gameLoopState = GameLoop.PourWaterAnimation;
                    this.BottleList[this.selectedFromBottleId].SetSelected(false);

                    // 播放倒水动画，并在动画结束时将执行callback
                    this.PuzzleAnimationRef.PlayBottlePourWaterAnimation(
                        this.selectedFromBottleId,
                        this.selectedToBottleId,
                        () =>
                        {
                            this.gameLoopState = GameLoop.VictoryCheck;
                            this.victoryCheck();
                        }
                    );
                    break;
            }
        }
    }
}