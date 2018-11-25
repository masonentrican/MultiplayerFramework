using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace Com.MasonEntrican.MultiplayerFramework
{

    /// <summary>
    /// Player name input field. Let the user input their name
    /// </summary>
    [RequireComponent(typeof(PlayerNameInputField))]


    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private Constants

        // Store the PlayerPref Key to avoid typos
        /// <summary>
        /// PlayerPrefs is a simple lookup list of paired entries one is the key, one is the Value.
        /// The Key is a string, and is totally arbitrary, you decide how to name and you will need to stick to it throughout the development.
        /// Because of that, it make sense to always store your PlayerPrefs Keys in one place only, a convenient way is to use a [Static| variable declaration
        /// because it won't change over time during the game and is the same everytime
        /// </summary>        

        const string playerNamePrefKey = "PlayerName";

        #endregion

        
        #region MonoBehaviour CallBacks
        
        void Start ()
        {
            string defaultName = string.Empty;
            InputField _inputField = this.GetComponent<InputField>();

            if (_inputField != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                    _inputField.text = defaultName;
                }
            }

            PhotonNetwork.NickName = defaultName;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the name of the player, and save it in the PlayerPrefs for future sessions.
        /// </summary>
        // <param name="value">The name of the Player </param>        
        public void SetPlayerName(string value)
        {
            // #Important
            if(string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }
            PlayerPrefs.SetString(playerNamePrefKey, value);
        }

        #endregion

    }

}

