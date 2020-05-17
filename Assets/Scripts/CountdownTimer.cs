using ExitGames.Client.Photon;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace SimpleBattleCity
{
    public class CountdownTimer : MonoBehaviourPunCallbacks
    {
        public const string CountdownStartTime = "StartTime";

        /// <summary>
        /// OnCountdownTimerHasExpired delegate.
        /// </summary>
        public delegate void CountdownTimerHasExpired();

        /// <summary>
        /// Called when the timer has expired.
        /// </summary>
        public static event Photon.Pun.UtilityScripts.CountdownTimer.CountdownTimerHasExpired OnCountdownTimerHasExpired;

        private bool isTimerRunning;

        private float startTime;

        [Header("Reference to a Text component for visualizing the countdown")]
        [SerializeField] TextMeshProUGUI Text;
        [SerializeField] GameObject Back;

        [Header("Countdown time in seconds")]
        public float Countdown = 5.0f;

        public void Start()
        {
            if (Text == null)
            {
                Debug.LogError("Reference to 'Text' is not set. Please set a valid reference.", this);
                return;
            }
        }

        public void Update()
        {
            if (!isTimerRunning)
            {
                return;
            }

            float timer = (float)PhotonNetwork.Time - startTime;
            float countdown = Countdown - timer;

            Text.text = string.Format("Game starts in {0} seconds", countdown.ToString("n0"));

            if (countdown > 0.0f)
            {
                return;
            }

            isTimerRunning = false;

            Text.text = string.Empty;
            Back.SetActive(false);

            if (OnCountdownTimerHasExpired != null)
            {
                OnCountdownTimerHasExpired();
            }
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            object startTimeFromProps;

            if (propertiesThatChanged.TryGetValue(CountdownStartTime, out startTimeFromProps))
            {
                isTimerRunning = true;
                startTime = (float)startTimeFromProps;
            }
        }
    }
}