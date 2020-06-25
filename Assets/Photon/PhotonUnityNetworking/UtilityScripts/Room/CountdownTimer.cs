// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CountdownTimer.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Utilities,
// </copyright>
// <summary>
// This is a basic CountdownTimer. In order to start the timer, the MasterClient can add a certain entry to the Custom Room Properties,
// which contains the property's name 'StartTime' and the actual start time describing the moment, the timer has been started.
// To have a synchronized timer, the best practice is to use PhotonNetwork.Time.
// In order to subscribe to the CountdownTimerHasExpired event you can call CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
// from Unity's OnEnable function for example. For unsubscribing simply call CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;.
// You can do this from Unity's OnDisable function for example.
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Pun;

namespace Photon.Pun.UtilityScripts
{
    /// <summary>
    /// This is a basic CountdownTimer. In order to start the timer, the MasterClient can add a certain entry to the Custom Room Properties,
    /// which contains the property's name 'StartTime' and the actual start time describing the moment, the timer has been started.
    /// To have a synchronized timer, the best practice is to use PhotonNetwork.Time.
    /// In order to subscribe to the CountdownTimerHasExpired event you can call CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
    /// from Unity's OnEnable function for example. For unsubscribing simply call CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;.
    /// You can do this from Unity's OnDisable function for example.
    /// </summary>
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
        public static event CountdownTimerHasExpired OnCountdownTimerHasExpired;

        public bool isTimerRunning;

        public float startTime;

        [Header("Reference to a Text component for visualizing the countdown")]
        public TextMesh Text;

        [Header("Countdown time in seconds")]
        public float Countdown = 5.0f;

        public void Start()
        {
            Debug.Log("CountdownTimer: Start called");
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
                Debug.Log("CountdownTimer: isTimerRunning is False");
                return;
            }
            
            Debug.Log("CountdownTimer: isTimerRunning is True");

            float timer = (float)PhotonNetwork.Time - startTime;
            //Debug.Log("CountdownTimer: timer value: " + timer);
            float countdown = Countdown - timer;
            //Debug.Log("CountdownTimer: countdown value: " + countdown);

            Text.text = string.Format("Game starts in {0} seconds", countdown.ToString("n0"));

            if (countdown > 0.0f)
            {
                Debug.Log("CountdownTimer: Count down timer still running: " + countdown);
                return;
            }

            Debug.Log("CountdownTimer: Count down has finished");
            isTimerRunning = false;

            Text.text = string.Empty;

            if (OnCountdownTimerHasExpired != null)
            {
                OnCountdownTimerHasExpired();
            }
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            object startTimeFromProps;
            
            //Debug.Log("CountdownTimer: OnRoomPropertiesUpdate called");

            if (propertiesThatChanged.TryGetValue(CountdownStartTime, out startTimeFromProps))
            {
                //Debug.Log("CountdownTimer: Got value" + startTimeFromProps);
                isTimerRunning = true;
                startTime = (float)startTimeFromProps;
            }
        }
    }
}