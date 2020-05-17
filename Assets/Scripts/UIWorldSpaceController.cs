using TMPro;
using UnityEngine;

namespace SimpleBattleCity
{
    public class UIWorldSpaceController : MonoBehaviour
    {
        [SerializeField] StageController stage;
        [SerializeField] GameObject grid;
        [SerializeField] GameObject bot;
        [SerializeField] TextMeshProUGUI PlayerLifesText;
        [SerializeField] TextMeshProUGUI StageText;

        void Start()
        {
            bot.SetActive(false);
            for (int i = 0; i < stage.NumberOfTanksSum; i++)
            {
                GameObject go = Instantiate(bot, grid.transform);
                go.SetActive(true);
            }



        }

        void UpdateBotReserve()
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