using System.Collections;
using Assets.MorpehRes.Data.Creator;
using Morpeh;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace SimpleBattleCity
{
    public class GameController : MonoBehaviourPunCallbacks
    {
        public static GameController Instance = null;

        [SerializeField] CreatorSystem creator;
        [SerializeField] GameObject infoBack;
        [SerializeField] TextMeshProUGUI infoText;
        [SerializeField] TextMeshProUGUI livesText;
        [SerializeField] GameObject ecs;

        int singlePlayerLives;

        #region UNITY

        public void Awake()
        {
            Instance = this;
        }

        public override void OnEnable()
        {
            base.OnEnable();

            CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
        }

        public void Start()
        {
            infoBack.SetActive(true);
            infoText.text = "Waiting for other players...";

            Hashtable props = new Hashtable()
            {
                {PlayerData.KeyLivesLeft, PlayerData.LivesMax},
                {PlayerData.KeyPlayerLoadedStage, false}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            if (!PhotonNetwork.IsConnected)
                StartCoroutine(PlaySingle());
        }

        IEnumerator PlaySingle()
        {
            infoBack.SetActive(false);

            singlePlayerLives = 5;
            livesText.text = singlePlayerLives.ToString();

            yield return new WaitForSeconds(0.5f);

            CreatorPlayerData data = new CreatorPlayerData();
            data.Owner.Player = 1;
            creator.Players.Enqueue(data);

            yield return null;
        }

        public void PlayerDiedSinglePlayer()
        {
            singlePlayerLives -= 1;
            livesText.text = singlePlayerLives.ToString();
            if (singlePlayerLives <= 0)
            {
                StartCoroutine(EndOfGame("2", 0));
            }
        }

        public void EagleDiedSinglePlayer(int owner)
        {
            StartCoroutine(EndOfGame(owner.ToString(), 0));
        }

        public override void OnDisable()
        {
            base.OnDisable();

            CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
        }

        #endregion

        #region COROUTINES

        IEnumerator EndOfGame(string winner, int score)
        {
            ecs.SetActive(false);
            float timer = 5.0f;

            while (timer > 0.0f)
            {
                infoBack.SetActive(true);
                infoText.text = string.Format(
                    "Player {0} won with {1} points.\n\n\nReturning to login screen in {2} seconds.",
                    winner, score, timer.ToString("n2"));

                yield return new WaitForEndOfFrame();

                timer -= Time.deltaTime;
            }

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LeaveRoom();
            }
            else
            {
                SceneManager.LoadScene("Lobby");
            }
        }

        #endregion

        #region PUN CALLBACKS

        public override void OnDisconnected(DisconnectCause cause)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
        }

        public override void OnLeftRoom()
        {
            PhotonNetwork.Disconnect();
        }

        // public override void OnMasterClientSwitched(Player newMasterClient)
        // {
        //     if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        //     {
        //         
        //     }
        // }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            CheckEndOfGame();
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(PlayerData.KeyLivesLeft))
            {
                CheckEndOfGame();
                return;
            }

            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            if (changedProps.ContainsKey(PlayerData.KeyPlayerLoadedStage))
            {
                if (CheckAllPlayerLoadedLevel())
                {
                    Hashtable props = new Hashtable
                    {
                        {CountdownTimer.CountdownStartTime, (float) PhotonNetwork.Time}
                    };
                    PhotonNetwork.CurrentRoom.SetCustomProperties(props);
                }
            }
        }

        #endregion

        void StartGame()
        {
            CreatorPlayerData data = new CreatorPlayerData();
            data.Owner.Player = PhotonNetwork.LocalPlayer.ActorNumber;
            Debug.Log($"player {data.Owner.Player} started game");

            creator.Players.Enqueue(data);
        }

        bool CheckAllPlayerLoadedLevel()
        {
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object playerLoadedStage;

                if (p.CustomProperties.TryGetValue(PlayerData.KeyPlayerLoadedStage, out playerLoadedStage))
                {
                    if ((bool)playerLoadedStage)
                    {
                        continue;
                    }
                }

                return false;
            }

            return true;
        }

        void CheckEndOfGame()
        {
            bool noMoreLives = false;

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object lives;
                if (p.CustomProperties.TryGetValue(PlayerData.KeyLivesLeft, out lives))
                {
                    if ((int)lives <= 0)
                    {
                        noMoreLives = true;
                        break;
                    }
                }
            }

            if (noMoreLives)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    StopAllCoroutines();
                }

                string winner = "";
                int score = -1;

                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    if (p.GetScore() > score)
                    {
                        winner = p.NickName;
                        score = p.GetScore();
                    }
                }

                StartCoroutine(EndOfGame(winner, score));
            }
        }

        void OnCountdownTimerIsExpired()
        {
            StartGame();
        }
    }
}