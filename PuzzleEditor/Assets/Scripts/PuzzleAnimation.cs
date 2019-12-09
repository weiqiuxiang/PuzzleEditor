using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Utility;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

namespace Game
{
    public class PuzzleAnimation : MonoBehaviour
    {
        [SerializeField,NonEditable]
        private PuzzleManager puzzleManager = null;
        [SerializeField]
        private GameObject guardObj = null;
        [SerializeField]
        CanvasGroup canvasGroup = null;
        [SerializeField]
        TMPro.TextMeshProUGUI victoryTextGui = null;

        private Sequence gameStartSequence = null;
        private Sequence victorySequence = null;
        private Sequence playingPourWaterSequence = null;

        void OnEnable()
        {
            this.guardObj.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        }
        void OnDisable()
        {
            this.playingPourWaterSequence?.Complete();
            this.guardObj.gameObject.SetActive(false);
            this.canvasGroup.alpha = 0;
            this.victoryTextGui.alpha = 0;
        }

        public void PlayGameStartAnimation(Action OnAnimationEnd)
        {
            this.guardObj?.SetActive(true);
            Sequence sequence = DOTween.Sequence();
            sequence.OnComplete(() =>
            {
                OnAnimationEnd?.Invoke();
                this.guardObj?.SetActive(false);
            });

            var fadeOut = DOTween.To(() => this.canvasGroup.alpha, (value) => this.canvasGroup.alpha = value, 1, GameStatic.GameStartFadeInTime);
            this.gameStartSequence = sequence.Play();
        }

        public void PlayBottlePourWaterAnimation(int FromId, int ToId, Action OnAnimationEnd)
        {
            Bottle fromBottle = null;
            Bottle toBottle = null;

            try
            {
                fromBottle = this.puzzleManager.BottleList[this.puzzleManager.selectedFromBottleId];
                toBottle = this.puzzleManager.BottleList[this.puzzleManager.selectedToBottleId];
            }
            catch
            {
                Debug.LogError("PlayGameStartAnimation method error");
                return;
            }

            // 将guard有效，登录动画结束时的callback
            this.guardObj?.SetActive(true);
            Sequence sequence = DOTween.Sequence();
            sequence.OnComplete(() =>
            {
                OnAnimationEnd?.Invoke();
                this.guardObj?.SetActive(false);
            });

            //保存初始位置
            Vector3 originPosition = fromBottle.ImageMaskRectTransform.position;
            Vector3 pourPosition = toBottle.PouredPositionObj.position;

            // 倒水
            int fromPrevAmount = fromBottle.CurrentAmount - fromBottle.ChangeAmount;
            var pourWaterFrom = DOTween.To(() => fromPrevAmount, (value) => fromBottle.SetWaterAmountUI(value), fromBottle.CurrentAmount, GameStatic.PourTime)
                                        .OnComplete(() => fromBottle.SetWaterCurrentAmountUI());
            int toPrevAmount = toBottle.CurrentAmount - toBottle.ChangeAmount;
            var pourWaterTo = DOTween.To(() => toPrevAmount, (value) => toBottle.SetWaterAmountUI(value), toBottle.CurrentAmount, GameStatic.PourTime)
                                        .OnComplete(() => toBottle.SetWaterCurrentAmountUI());

            // 将动画加入sequence
            sequence.Append(this.moveDownRelative(fromBottle));
            sequence.Join(this.fadeOut(fromBottle).OnComplete(() =>
            {
                fromBottle.ImageMaskRectTransform.position = toBottle.PouredPositionObj.position.SetY(toBottle.PouredPositionObj.position.y + GameStatic.BottleMoveDist);
                fromBottle.ImageMaskRectTransform.eulerAngles = fromBottle.ImageMaskRectTransform.eulerAngles.SetZ(GameStatic.BottleRotateAngle);
            }));
            sequence.Append(this.moveDownRelative(fromBottle));
            sequence.Join(this.fadeIn(fromBottle));
            sequence.Append(pourWaterFrom);
            sequence.Join(pourWaterTo);
            sequence.Append(this.moveUpRelative(fromBottle));
            sequence.Join(this.fadeOut(fromBottle).OnComplete(() =>
            {
                fromBottle.ImageMaskRectTransform.position = originPosition.SetY(originPosition.y - GameStatic.BottleMoveDist);
                fromBottle.ImageMaskRectTransform.eulerAngles = Vector3.zero;
            }));
            sequence.Append(this.moveUpRelative(fromBottle));
            sequence.Join(this.fadeIn(fromBottle));
            

            // 播放动画
            this.playingPourWaterSequence = sequence.Play();
        }

        private TweenerCore<Vector3, Vector3, VectorOptions> moveUpRelative(Bottle bottle)
        {
            return bottle.ImageMaskRectTransform.DOMoveY(GameStatic.BottleMoveDist, GameStatic.BottleFadeMoveTime).SetRelative();
        }

        private TweenerCore<Vector3, Vector3, VectorOptions> moveDownRelative(Bottle bottle)
        {
            return bottle.ImageMaskRectTransform.DOMoveY(-GameStatic.BottleMoveDist, GameStatic.BottleFadeMoveTime).SetRelative();
        }

        private TweenerCore<float, float, FloatOptions> fadeIn(Bottle bottle)
        {
            return DOTween.To(() => bottle.ImageMaskCanvasGourp.alpha, (value) => bottle.ImageMaskCanvasGourp.alpha = value, 1, GameStatic.BottleFadeOutTime);
        }

        private TweenerCore<float, float, FloatOptions> fadeOut(Bottle bottle)
        {
            return DOTween.To(() => bottle.ImageMaskCanvasGourp.alpha, (value) => bottle.ImageMaskCanvasGourp.alpha = value, 0, GameStatic.BottleFadeOutTime);
        }

        public void PlayVictoryAnimationAnimation(Action OnAnimationEnd)
        {
            Sequence sequence = DOTween.Sequence();
            this.guardObj?.SetActive(true);

            var fadeOut = DOTween.To(() => this.victoryTextGui.alpha, (value) => this.victoryTextGui.alpha = value, 1, GameStatic.GameStartFadeInTime);
            sequence.Append(fadeOut).OnComplete(() => OnAnimationEnd?.Invoke());
            this.victorySequence = sequence.Play();
        }
    }
}
