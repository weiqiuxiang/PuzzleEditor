using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PourWaterPuzzle.Utility
{
    public class InputUtility : SingletonMonoBehaviorCreateDontDestory<InputUtility>
    {
        private float touchTime = 0;

        private Touch prevTouch;
        private Touch nowTouch;
        public float TouchTime => touchTime;

        protected override void Awake()
        {
            base.Awake();
        }

        void Update()
        {
            this.touchUpdate();
        }

        private void touchUpdate()
        {
#if UNITY_EDITOR
            this.touchUpdateMouse();
#else
            this.touchUpdateDevice();
#endif
        }

        private void touchUpdateMouse()
        {
            if (Input.GetMouseButton(0))
            {
                this.touchTime += Time.deltaTime;
            }
            else
            {
                this.SetTouchTimeToZero();
            }
        }

        private void touchUpdateDevice()
        {
            this.prevTouch = this.nowTouch;
            if (Input.touchCount > 0)
            {
                nowTouch = Input.GetTouch(0);
                if (this.nowTouch.phase == TouchPhase.Began)
                {
                    this.touchTime += Time.deltaTime;
                }
                else if (this.nowTouch.phase == TouchPhase.Moved)
                {
                    this.touchTime += Time.deltaTime;
                }
                else if (this.nowTouch.phase == TouchPhase.Ended)
                {
                    this.touchTime = 0;
                }
            }
        }

        public void SetTouchTimeToZero()
        {
            this.touchTime = 0;
        }

        /// <summary>
        /// touch screen one time
        /// </summary>
        /// <returns>true : detected touch screen one time</returns>
        public bool TouchScreenDown()
        {
            bool isInput = false;
            if (Application.isEditor)
            {
                isInput = Input.GetMouseButtonDown(0);
            }
            else
            {
                isInput = this.nowTouch.phase == TouchPhase.Began && this.prevTouch.phase != TouchPhase.Began;
            }
            return isInput;
        }
    }
}