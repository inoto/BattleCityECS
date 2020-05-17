using UnityEngine;

namespace SimpleBattleCity
{
    public class StageController : MonoBehaviour
    {
        [SerializeField] int stageNumber = 1;
        public int StageNumber => stageNumber;

        void Start()
        {
            MetaTracker.StageNumber = stageNumber;
        }
    }
}