using UnityEngine;

namespace SimpleBattleCity
{
    public class MetaTracker : MonoBehaviour
    {
        public static int StageNumber;

        [SerializeField] int smallTankPoints = 100, fastTankPoints = 200, bigTankPoints = 300, armoredTankPoints = 400;

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
