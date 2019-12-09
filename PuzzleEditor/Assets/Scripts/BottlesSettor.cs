using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class BottleData
    {
        public bool IsTarget;
        public Sprite BottleImage;
        public Sprite BottleMask;
        public int CurrentAmount;
        public int MaxAmount;
    }

    public class BottlesSettor : MonoBehaviour
    {
        [SerializeField]
        private PuzzleManager puzzleManager = null;
        [SerializeField]
        private GameObject bottleParent = null;
        [SerializeField]
        private GameObject bottlePrefab = null;
        [SerializeField]
        public int MaxBottleAmount = 0;
        [SerializeField]
        public int VictoryTarget = 0;
        [SerializeField]
        public int VictoryAmount = 0;

        [SerializeField]
        private List<BottleData> bottleDatas = null;
        public List<BottleData> BottleDatas
        {
            get
            {
                return bottleDatas ?? (bottleDatas = new List<BottleData>());
            }
            set
            {
                bottleDatas = value;
            }
        }

        [SerializeField, HideInInspector]
        private List<Bottle> bottleList = null;     // 杯子列表
        public List<Bottle> BottleList => (bottleList ?? (bottleList = new List<Bottle>()));

        void OnEnable()
        {
            this.getPrefab();
            this.resetList();
            this.setBottleDatas();
        }

        void OnDisable()
        {
            try
            {
                for (int i = 0; i < this.BottleList.Count; i++)
                {
                    var bottle = this.BottleList[i];
                }
            }
            catch{}
        }

        public void ResetData()
        {
            gameObject.SetActive(false);
            gameObject.SetActive(true);
        }

        private void getPrefab()
        {
            if (this.bottlePrefab == null)
            {
                var prefab = Resources.Load(GameStatic.BottlePrefabPath) as GameObject;
                if (prefab == null)
                {
                    Debug.LogError($"Resource is no exist in {GameStatic.BottlePrefabPath}");
                    return;
                }
                this.bottlePrefab = prefab;
            }
        }

        private void resetList()
        {
            this.BottleList.Clear();
            this.BottleList.AddRange(this.bottleParent.GetComponentsInChildren<Bottle>(true).ToList());
            if (this.BottleList.Count >= this.MaxBottleAmount)
            {
                try
                {
                    var clearList = this.BottleList.Skip(this.MaxBottleAmount).ToList();
                    clearList.ForEach(x => Destroy(x.gameObject));
                    this.bottleList = this.BottleList.Take(this.MaxBottleAmount).ToList();
                }
                catch { }
            }
            else
            {
                int addAmount = this.MaxBottleAmount - this.BottleList.Count;
                for (int i = 0; i < addAmount; i++)
                {
                    GameObject obj = Instantiate(this.bottlePrefab, this.bottleParent.transform);
                    this.BottleList.Add(obj.GetComponent<Bottle>());
                }
            }
        }

        private void setBottleDatas()
        {
            try
            {
                for (int i = 0; i < this.BottleList.Count; i++)
                {
                    var bottle = this.BottleList[i];
                    bottle.CurrentAmount = this.BottleDatas[i].CurrentAmount;
                    bottle.MaxAmount = this.BottleDatas[i].MaxAmount;
                    bottle.SetSelected(false);
                    bottle.SetWaterCurrentAmountUI();
                    bottle.Button.onClick.RemoveAllListeners();
                    bottle.Button.onClick.AddListener(() => this.puzzleManager.OnClickBottle(bottle));

                    if(i == this.VictoryTarget)
                    {
                        bottle.ShowVictoryValue(true,this.VictoryAmount);
                    }
                    else
                    {
                        bottle.ShowVictoryValue(false);
                    }
                }
            }
            catch{
                Debug.LogError("setBottleDatas method error");
            }
        }
    }
}