using System.Collections.Generic;
using Morpeh.Globals;
using TMPro;
using UnityEngine;

namespace SimpleBattleCity
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] GlobalEventInt GameOverEvent;
        [SerializeField] GameObject GameoverBack;

        void Awake()
        {
            GameOverEvent.Subscribe(GameOver);
        }

        void OnDestroy()
        {
            // GameOverEvent.
        }

        void GameOver(IEnumerable<int> e)
        {
            Debug.Log("game over event");
            GameoverBack.SetActive(true);
            foreach (var value in e)
            {
                GameoverBack.GetComponentInChildren<TextMeshProUGUI>().text =
                    $"Player {value} defeated";
            }
            
        }
    }
}