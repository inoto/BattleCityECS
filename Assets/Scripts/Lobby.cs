using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SimpleBattleCity
{
    public class Lobby : MonoBehaviourPunCallbacks
    {
        [SerializeField] TextMeshProUGUI StatusText;
        [SerializeField] Button SinglePlayerButton;
        [SerializeField] TextMeshProUGUI MultiPlayerButtonText;

        #region UNITY

        public void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public void StartSingleGame()
        {
            SceneManager.LoadScene("Stage1");
        }

        public void StartMultiPlayer()
        {
            if (!PhotonNetwork.InRoom)
            {
                SinglePlayerButton.interactable = false;
                MultiPlayerButtonText.text = "Stop waiting";
                StatusText.text = "connecting...";
                PhotonNetwork.LocalPlayer.NickName = "player 1";
                PhotonNetwork.ConnectUsingSettings();
            }
            else
            {
                SinglePlayerButton.interactable = true;
                MultiPlayerButtonText.text = "Create/Join multiplayer";
                StatusText.text = "disconnected";
                PhotonNetwork.Disconnect();
            }
        }

        #endregion

        #region PUN CALLBACKS

        public override void OnConnectedToMaster()
        {
            StatusText.text = "connected to master\nwaiting for other players...";
            if (PhotonNetwork.CountOfRooms > 0)
            {
                PhotonNetwork.JoinRoom("test");
            }
            else
            {
                PhotonNetwork.CreateRoom("test");
            }
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            StatusText.text = $"create room failed {message}";
            SinglePlayerButton.interactable = true;
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            StatusText.text = $"join room failed {message}";
            SinglePlayerButton.interactable = true;
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            StatusText.text = $"player entered room";
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            PhotonNetwork.LoadLevel("Stage1");
        }

        #endregion
    }
}