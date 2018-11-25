using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Com.MasonEntrican.MultiplayerFramework
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        [Tooltip("The max number of players per room. When max number is met, a new room is created for new players")]
        [SerializeField]
        private byte maxPlayersPerRoom = 10;

        #endregion


        #region Private Fields

        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        string gameVersion = "1";

        #endregion


        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            Connect();
        }

        #endregion


        #region Public Methods


        /// <summary>
        /// Start the connection process.
        /// - If already connected, we attempt joining a random room
        /// - if not yet connected, Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }


        #endregion



        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("MultiplayerFramework/Launcher: OnConnectedToMaster() was called by PUN");

            // #Critical
            // First thing we do is join an existing room. If there is no room we'll be called back with OnJoinRandomFailed()
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("MultiplayerFramework/Launcher: OnDisconnected() was called by PUN with reaason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("MultiplayerFramework/Launcher: OnJoinRandomFailed() was called by PUN. No random room available, so create one. \n Calling: PhotonNetwork.CreateRoom");
            
            // #Critical
            // We failed to join a random room, none exists or they are all full. Create a new room
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("MultiplayerFramework/Launcher: OnJoinedRoom() was called by PUN. Now this client is in a room");
        }

        #endregion

    }
}