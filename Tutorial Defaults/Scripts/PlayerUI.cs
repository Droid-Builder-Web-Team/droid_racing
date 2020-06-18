using UnityEngine;
using UnityEngine.UI;


using System.Collections;


namespace uk.droidbuilders.droid_racing
{
    public class PlayerUI : MonoBehaviour
    {
        #region Public Fields

        [Tooltip("UI Text to display Player's Name")]
        [SerializeField]
        private Text playerNameText;
        
        [Tooltip("UI Text to display Lap Number")]
        [SerializeField]
        private Text lapNumberText;
        
        [Tooltip("UI Text to display Last Lap Time")]
        [SerializeField]
        private Text lastLapTimeText;
        
        [Tooltip("UI Text to display Best Lap Time")]
        [SerializeField]
        private Text bestLapTimeText;
        
        [Tooltip("UI Text to display current elapsed Time")]
        [SerializeField]
        private Text elapsedTimeText;
        
        #endregion
        
        #region Private Fields

        private PlayerMove target;
        #endregion


        #region MonoBehaviour Callbacks
        void Awake()
        {

        }

        void Update()
        {
          
            if (lapNumberText != null)
            {
                lapNumberText.text = "" + target.laps;
            }
            
            if (lastLapTimeText != null)
            {
                lastLapTimeText.text = "Last Lap Time: " + target.lastTime + "s";
            }
            
            if (bestLapTimeText != null)
            {
                bestLapTimeText.text = "Best Lap Time: " + target.bestTime + "s";
            }
            
            if (elapsedTimeText != null)
            {
                elapsedTimeText.text = target.elapsedTime.ToString("F2") + "s";
            }
        } 
        #endregion


        #region Public Methods
        
        public void SetTarget(PlayerMove _target)
        {
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