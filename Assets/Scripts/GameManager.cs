using System;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using PathCreation;
using PathCreation.Examples;

namespace uk.droidbuilders.droid_racing
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Public Fields
        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;
        public static GameManager Instance;
        public Text infoText;
        public Canvas infoBox;
        public GameObject playerList;
        public Canvas resultsBox;
        public Text timeLeftBox;
        public AudioClip start01;
        public AudioClip start02;

        [SerializeField]
        public ResultsListingEntry _resultListing;
        public Transform _resultsContent;


        public int waitBegin = 30; // How long to wait for more players if not minimum number of players
        public int minPlayers = 4; // Minimum number of players in room before automatically starting
        public int raceLength = 3; // Length of a game
        public int startDelay = 5; // Match countdown
        public int endDelay = 10;  // How long to show the results screen before returning to lobby.

        #endregion

        private Vector3[] spawnPoints = new [] {
          new Vector3(-5f,1f,10f),
          new Vector3(0f,1f,10f),
          new Vector3(-5f,1f,7f),
          new Vector3(0f,1f,7f),
          new Vector3(-5f,1f,4f),
          new Vector3(0f,1f,4f)
        };

        public float stateTime;
        private string gameState = "prerace";
        private bool resultsDrawn;
        private GameObject myPlayer;
        private WebCalls webCalls;
        private AudioSource finishLine;

        public const string MAP_PROP_KEY = "map";
        public const string GAME_MODE_PROP_KEY = "gm";
        public const string ROUND_START_TIME = "StartTime";
        public const string RACE_NAME = "rn";

        #region Photon Callbacks

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("GameManager: OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("GameManager: OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                //LoadArena();
            }
        }


        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("GameManager: OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("GameManager: OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


                //LoadArena();
            }
        }



        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            Debug.Log("GameManager: OnLeftRoom");
            SceneManager.LoadScene(0);
            PhotonNetwork.JoinLobby();
        }


        void Start()
        {
            Instance = this;
            webCalls = (WebCalls)GameObject.Find("WebCalls").GetComponent(typeof(WebCalls));
            finishLine = GameObject.FindWithTag("Finish").GetComponent<AudioSource>();
            GameObject gonk = PhotonNetwork.InstantiateSceneObject("gonk_animated", new Vector3(54f, 2f, 14f ), Quaternion.identity, 0);

            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
            }
            else
            {
                if (PlayerMove.LocalPlayerInstance == null )
                {
                    Debug.LogFormat("GameManager: We are Instantiating LocalPlayer from {0}", SceneManager.GetActiveScene().name);
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    int x = PhotonNetwork.PlayerList.Length;
                    myPlayer = (GameObject)PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPoints[x], Quaternion.identity, 0);
                    myPlayer.GetComponent<CameraWork>().enabled = true;
                    myPlayer.GetComponent<AudioListener>().enabled = true;
                }
                else
                {
                    Debug.LogFormat("GameManager: Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
            if (PhotonNetwork.IsMasterClient)
            {
                stateTime = (float)PhotonNetwork.Time;
                Debug.Log("GameManager: startTime set by MasterClient: " + stateTime);
                RoomOptions options = new RoomOptions();
                options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable {
                   { "StartTime", stateTime},
                   { "Map", PhotonNetwork.CurrentRoom.CustomProperties[MAP_PROP_KEY] }
                 };
                Debug.Log("GameManager: Starting Gonk!");
                PhotonNetwork.CurrentRoom.SetCustomProperties(options.CustomRoomProperties);
                gonk.GetComponent<PathFollower>().pathCreator = GameObject.Find("Gonk Path").GetComponent<PathCreator>();
            }
            else
            {
                stateTime = float.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
                Debug.Log("GameManager: startTime got from custom properties is: " + stateTime);
            }

            Debug.Log("GameManager: Freerace selected");
            gameState = "freerace";
            infoBox.gameObject.SetActive(false);
            myPlayer.GetComponent<CharacterController>().enabled = true;
            resultsBox.gameObject.SetActive(false);
            timeLeftBox.transform.parent.gameObject.SetActive(false);

        }

        void Update()
        {
            FreeRace();

        }

        void FreeRace()
        {
            //Debug.Log("GameManager: State - FreeRace");
        }

        void Quit()
        {
            Debug.Log("GameManager: Quit");
            LeaveRoom();
        }

        #endregion


        #region Public Methods


        public void LeaveRoom()
        {
            Debug.Log("GameManager: LeavingRoom called");
            PhotonNetwork.LeaveRoom();
            //Debug.Log("GameManager: Disconnecting from PhotonNetwork");
            //PhotonNetwork.Disconnect();
        }


        #endregion

        #region Private Methods
        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Map" + PhotonNetwork.CurrentRoom.CustomProperties[MAP_PROP_KEY]);
        }

        void CountdownBeep()
        {
            Debug.Log("Beep!");
            finishLine.Play();
        }


        #endregion
    }
}
