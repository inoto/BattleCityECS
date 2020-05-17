using UnityEngine;

namespace SimpleBattleCity
{
    public class StageController : MonoBehaviour
    {
        [SerializeField] int stageNumber = 1;
        public int StageNumber => stageNumber;

        [SerializeField] int numberOfSmallTanks = 4;
        public int NumberOfSmallTanks => numberOfSmallTanks;
        [SerializeField] int numberOfFastTanks = 4;
        public int NumberOfFastTanks => numberOfFastTanks;
        [SerializeField] int numberOfBigTanks = 4;
        public int NumberOfBigTanks => numberOfBigTanks;
        [SerializeField] int numberOfArmoredTanks = 4;
        public int NumberOfArmoredTanks => numberOfArmoredTanks;

        public int NumberOfTanksSum => numberOfSmallTanks + numberOfFastTanks
                                       + numberOfBigTanks + numberOfArmoredTanks;

        void Start()
        {
            MetaTracker.StageNumber = stageNumber;
        }
    }
}