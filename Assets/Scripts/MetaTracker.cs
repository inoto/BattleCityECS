using UnityEngine;

namespace SimpleBattleCity
{
    public class MetaTracker : MonoBehaviour
    {
        public static int StageNumber;

        [SerializeField] int smallTankPoints = 100, fastTankPoints = 200, bigTankPoints = 300, armoredTankPoints = 400;
        public int SmallTankPointsWorth => smallTankPoints;
        public int FastTankPointsWorth => fastTankPoints;
        public int BigTankPointsWorth => bigTankPoints;
        public int ArmoredTankPointsWorth => armoredTankPoints;
        [SerializeField] PlayerData[] PlayerData = new PlayerData[1];

        static MetaTracker _instance = null;

        void Awake()
        {
            if (_instance == null)
            {
                DontDestroyOnLoad(gameObject);
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
