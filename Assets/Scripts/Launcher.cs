using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace uk.droidbuilders.droid_racing
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields
        /// <summary>
        /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
        /// </summary>
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 6;

        #endregion

        #region Public Fields
        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        public GameObject loginPanel;
        [Tooltip("The UI Panel to show rooms")]
        [SerializeField]
        public GameObject lobbyPanel;
        [Tooltip("Connecting text box")]
        [SerializeField]
        public GameObject connectingText;


        [Tooltip("The UI object to show rooms")]
        [SerializeField]
        public GameObject roomsPanel;
        #endregion

        [Tooltip("Region to connect to (eu, or us)")]
        [SerializeField]
        private string _region;

        #region Private Fields

        public const string MAP_PROP_KEY = "map";
        public const string ROUND_START_TIME = "StartTime";
        public const string RACE_NAME = "rn";


        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        string gameVersion = "3";
        /// <summary>
        /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
        /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
        /// Typically this is used for the OnConnectedToMaster() callback.
        /// </summary>
        bool isConnecting;


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
            //progressLabel.SetActive(false);
            loginPanel.SetActive(true);
            lobbyPanel.SetActive(false);
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
            Debug.Log("Launcher: Connecting...");

            lobbyPanel.SetActive(false);
            loginPanel.SetActive(false);
            connectingText.SetActive(true);

            // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinLobby();
                connectingText.SetActive(false);
                lobbyPanel.SetActive(true);
            }
            else
            {
                // #Critical, we must first and foremost connec"_Color"t to Photon Online Server.
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }



    public void CreateNewRoom(int mapNumber) {
        Debug.Log("Launcher: CreateRoom() called with map " + mapNumber);
        RoomOptions options = new RoomOptions();
        string[] CustomOptions = new string[2];
        CustomOptions[0] = MAP_PROP_KEY;
        CustomOptions[1] = RACE_NAME;
        string raceNumber = Random.Range(0f, 999f).ToString("000");
        string raceName = "";
        switch (mapNumber) {
            case 1:
                raceName = "Halloween" + raceNumber;
                break;
            case 2:
                raceName = "Practice" + raceNumber;
                break;
            case 3:
                raceName = "Xmas" + raceNumber;
                break;
            default:
                raceName = "ERROR" + raceNumber;
                break;
        }

        options.CustomRoomPropertiesForLobby = CustomOptions;
        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable {
          { MAP_PROP_KEY, mapNumber },
          { RACE_NAME, raceName}
        };
        options.MaxPlayers = maxPlayersPerRoom;
        options.PublishUserId = true;
        Debug.Log("Launcher: Creating room");
        PhotonNetwork.CreateRoom(null, options, null);
    }


    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    private void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        Debug.LogErrorFormat("Launcher: Room creation failed with error code {0} and error message {1}", codeAndMsg[0], codeAndMsg[1]);
    }

    public override void OnConnectedToMaster()
    {
      Debug.Log("Launcher: OnConnectedToMaster() was called by PUN");
      // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
      if (isConnecting)
      {
          Debug.Log("Launcher: Connecting to Lobby");
          PhotonNetwork.JoinLobby();
          //PhotonNetwork.JoinRandomRoom();
          isConnecting = false;
          connectingText.SetActive(false);
          lobbyPanel.SetActive(true);
      }
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
      Debug.LogWarningFormat("Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
      lobbyPanel.SetActive(false);
      loginPanel.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Launcher: OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room created");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        PhotonNetwork.LoadLevel("Map" + PhotonNetwork.CurrentRoom.CustomProperties[MAP_PROP_KEY]);
    }
    #endregion
    }
}
