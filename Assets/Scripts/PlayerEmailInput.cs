using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEmailInput : MonoBehaviour

  {
      #region Private Constants


      // Store the PlayerPref Key to avoid typos
      const string playerEmailPrefKey = "PlayerEmail";


      #endregion


      #region MonoBehaviour CallBacks


      /// <summary>
      /// MonoBehaviour method called on GameObject by Unity during initialization phase.
      /// </summary>
      void Start () {


          string defaultEmail = string.Empty;
          InputField _inputField = this.GetComponent<InputField>();
          if (_inputField!=null)
          {
              if (PlayerPrefs.HasKey(playerEmailPrefKey))
              {
                  defaultEmail = PlayerPrefs.GetString(playerEmailPrefKey);
                  _inputField.text = defaultEmail;
              }
          }

      }


      #endregion


      #region Public Methods


      /// <summary>
      /// Sets the name of the player, and save it in the PlayerPrefs for future sessions.
      /// </summary>
      /// <param name="value">The name of the Player</param>
      public void SetPlayerEmail(string value)
      {
          // #Important
          if (string.IsNullOrEmpty(value))
          {
              Debug.LogError("Player email is null or empty");
              return;
          }


          PlayerPrefs.SetString(playerEmailPrefKey,value);
      }


      #endregion
  }
