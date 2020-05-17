using TMPro;
using UnityEngine;

namespace SimpleBattleCity
{
    public class UIWorldSpaceController : MonoBehaviour
    {
        [SerializeField] StageController stage;
        [SerializeField] TextMeshProUGUI PlayerLifesText;
        [SerializeField] TextMeshProUGUI StageText;

        void Start()
        {
            
        }

        void UpdatePlayerLives(int player)
        {

        }

        void UpdateStageNumber()
        {
            StageText.text = $"stage {stage.StageNumber}";
        }
    }
}