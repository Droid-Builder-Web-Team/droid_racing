using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using System.Collections;


namespace uk.droidbuilders.droid_racing
{
    public class PlayerUI : MonoBehaviour
    {
        #region Public Fields

        [Tooltip("UI Text to display Player's Name")]
        [SerializeField]
        public Text playerNameText;

        [Tooltip("UI Text to display Lap Number")]
        [SerializeField]
        public Text lapNumberText;

        [Tooltip("UI Text to display Last Lap Time")]
        [SerializeField]
        public Text lastLapTimeText;

        [Tooltip("UI Text to display Best Lap Time")]
        [SerializeField]
        public Text bestLapTimeText;

        [Tooltip("UI Text to display current elapsed Time")]
        [SerializeField]
        public Text elapsedTimeText;

        public Text isMaster;

        #endregion

        #region Private Fields

        private PlayerMove target;
        #endregion


        #region MonoBehaviour Callbacks
        void Awake()
        {
            Debug.Log("PlayerUI: Awake called");
            this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        }

        void Update()
        {

            if (target == null)
            {
                Destroy(this.gameObject);
                return;
            }

            if (lapNumberText != null)
            {
                lapNumberText.text = "" + target.laps;
            }

            if (lastLapTimeText != null)
            {
                lastLapTimeText.text = "Last Lap Time: " + target.lastTime.ToString("F2") + "s";
            }

            if (bestLapTimeText != null)
            {
                bestLapTimeText.text = "Best Lap Time: " + target.bestTime.ToString("F2") + "s";
            }

            if (elapsedTimeText != null)
            {
                elapsedTimeText.text = target.elapsedTime.ToString("F2") + "s";
            }

            if (target.GetComponent<AudioListener>().enabled) {
                isMaster.text = "Y";
            } else {
                isMaster.text = "N";
            }

        }
        #endregion


        #region Public Methods

        public void SetTarget(PlayerMove _target)
        {
            Debug.Log("PlayerUI: SetTarget called: " + target);
            if (_target == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;
            }
            // Cache references for efficiency
            target = _target;
            if (playerNameText != null)
            {
                playerNameText.text = target.photonView.Owner.NickName;
            }
          }

        #endregion



    }
}
