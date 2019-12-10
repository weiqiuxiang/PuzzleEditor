using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Bottle : MonoBehaviour
    {
        [Header("Reference")]
        public RectTransform PouredPositionObj = null;     //被注水的位置
        public Image BottleImage = null;
        public GameObject SelectedEffect = null;
        public CanvasGroup ImageMaskCanvasGourp = null;
        public RectTransform ImageMaskRectTransform = null;
        public TMPro.TextMeshProUGUI VictoryValue = null;
        public Button Button = null;

        [SerializeField]
        private Image bottleWater = null;
        [SerializeField]
        private TMPro.TextMeshProUGUI percentText = null;

        public int MaxAmount { get; set; }
        public int CurrentAmount { get; set; }
        public int ChangeAmount { get; set; }
        private bool isSelected = false;

        /// <summary>
        /// 加水/倒水
        /// </summary>
        /// <param name="amount">加水(正数)/倒水(负数)的量</param>
        /// <returns>计算后的加水(正数)/倒水(负数)的量</returns>
        public void ChangeWaterAmount(int amount)
        {
            int prevAmount = this.CurrentAmount;
            int nextAmount = this.CurrentAmount + amount;
            if (nextAmount < 0)
            {
                // 给别的杯子注水
                this.CurrentAmount = 0;
                this.ChangeAmount = -prevAmount;
            }
            else
            {
                // 自己被别的杯子倒水
                if (nextAmount > this.MaxAmount)
                {
                    this.CurrentAmount = this.MaxAmount;
                    this.ChangeAmount = this.MaxAmount - prevAmount;
                }
                else
                {
                    this.CurrentAmount = nextAmount;
                    this.ChangeAmount = amount;
                }
            }
        }

        /// <summary>
        /// 在UI上显示现在的数值
        /// </summary>
        public void SetWaterCurrentAmountUI()
        {
            this.bottleWater.fillAmount = (float)this.CurrentAmount / this.MaxAmount;
            this.percentText.text = this.CurrentAmount.ToString() + " / " + this.MaxAmount.ToString();
        }

        /// <summary>
        /// 在UI上显示settingAmount的数值
        /// </summary>
        /// <param name="amount">在UI上显示的数值</param>
        public void SetWaterAmountUI(float amount)
        {
            if ((amount < 0) || (amount > this.MaxAmount))
            {
                return;
            }

            this.bottleWater.fillAmount = amount / this.MaxAmount;
            this.percentText.text = amount.ToString("F0") + " / " + this.MaxAmount.ToString();
        }

        /// <summary>
        /// 被点击时显示效果
        /// </summary>
        /// <param name="active">true : 显示效果</param>
        public void SetSelected(bool active)
        {
            this.isSelected = active;
            this.SelectedEffect.SetActive(active);
        }

        public void ShowVictoryValue(bool active,float value = 0)
        {
            this.VictoryValue.text = $"VictoryValue : {value}";
            this.VictoryValue.gameObject.SetActive(active);
        }
    }
}