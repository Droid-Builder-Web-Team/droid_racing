using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Facebook.Unity;
using Photon.Pun;
using Photon.Realtime;


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
        
        public int waitBegin = 30; // How long to wait for more players if not minimum number of players
        public int minPlayers = 4; // Minimum number of players in room before automatically starting
        public int raceLength = 3; // Length of a game
        public int startDelay = 5; // Match countdown
        
        #endregion
        
        private Vector3[] spawnPoints = new [] {
          new Vector3(-5f,1f,10f),
          new Vector3(0f,1f,10f),
          new Vector3(-5f,1f,7f),
          new Vector3(0f,1f,7f),
          new Vector3(-5f,1f,4f),
          new Vector3(0f,1f,4f)
        };
        
        public float startTime;
        private float countdownStartTime;
        private float raceStartTime;
        private bool isRoomReady = false;
        private bool countdownStarted = false;
        private bool raceStarted = false;
        private GameObject myPlayer;
        
        private WebCalls webCalls;

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
        }


        void Start()
        {
            Instance = this;
            webCalls = (WebCalls)GameObject.Find("WebCalls").GetComponent(typeof(WebCalls));
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",this);
            }
            else
            {
                if (PlayerMove.LocalPlayerInstance == null )
                {
                    Debug.LogFormat("GameManager: We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    int x = PhotonNetwork.PlayerList.Length;
                    myPlayer = (GameObject)PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPoints[x], Quaternion.identity, 0);
                    myPlayer.GetComponent<CameraWork>().enabled = true;
                }
                else
                {
                    Debug.LogFormat("GameManager: Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
            if (PhotonNetwork.IsMasterClient)
            {
                startTime = (float)PhotonNetwork.Time;
                Debug.Log("GameManager: startTime set by MasterClient: " + startTime);
                RoomOptions options = new RoomOptions();
                options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable {  { "StartTime", startTime} };
                PhotonNetwork.CurrentRoom.SetCustomProperties(options.CustomRoomProperties);
            }
            else 
            {
                startTime = float.Parse(PhotonNetwork.CurrentRoom.CustomProperties["StartTime"].ToString());
                Debug.Log("GameManager: startTime got from custom properties is: " + startTime);
            }
        }
        
        
        void Update()
        {
            float currentTime = (float)PhotonNetwork.Time;
            float waitingTime = currentTime - startTime;
            if ( PhotonNetwork.PlayerList.Length < minPlayers && waitingTime < waitBegin && !isRoomReady && !raceStarted) 
            {
                infoText.text = "Waiting for " + (minPlayers - PhotonNetwork.PlayerList.Length).ToString("0") + " more players: " + (waitBegin - waitingTime).ToString("0");
                return;
            }
            else 
            {
                //startText.text = "Game starting in: " + countdown.ToString("n0"));
                isRoomReady = true;
                if (!countdownStarted && !raceStarted) 
                {
                    countdownStartTime = (float)PhotonNetwork.Time;
                    countdownStarted = true;
                }
                //myPlayer.GetComponent<CharacterController>().enabled = true;
            }
            if (isRoomReady && !raceStarted)
            {
                if (countdownStarted) {
                    float countdown = startDelay - ((float)PhotonNetwork.Time - countdownStartTime);
                    if (countdown > 0)
                    {
                        infoText.text = "Game starting in: " + countdown.ToString("n0");
                    } 
                    else 
                    {
                        infoText.text = String.Empty;
                        raceStarted = true;
                        countdownStarted = false;
                        raceStartTime = (float)PhotonNetwork.Time;
                        infoBox.enabled = false;
                        myPlayer.GetComponent<CharacterController>().enabled = true;
                    }
                }
                Debug.Log("Room is ready");
            }
            if (raceStarted) {
                float duration = (float)PhotonNetwork.Time - raceStartTime;
                if (duration >  raceLength)
                {
                    Debug.Log("Race Finished");
                    myPlayer.GetComponent<CharacterController>().enabled = false;
                    Debug.Log("GameManager: Player laps: " + myPlayer.GetComponent<PlayerMove>().laps);
                    infoText.text = "Race Over";
                    infoBox.enabled = true;
                    webCalls.UploadRace("email", PhotonNetwork.NickName, myPlayer.GetComponent<PlayerMove>().bestTime, 
                            myPlayer.GetComponent<PlayerMove>().laps, PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.Name, raceLength);
                    for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                    {
                        string name = PhotonNetwork.PlayerList[i].NickName;
                        //float best_lap = PhotonNetwork.PlayerList[i].GetComponent<PlayerMove>().bestTime;
                        //int number_laps = PhotonNetwork.PlayerList[i].GetComponent<PlayerMove>().laps;
                        
                    }
                    raceStarted = false;
                }
            
            }
            
        }

        #endregion


        #region Public Methods


        public void LeaveRoom()
        {
            Debug.Log("GameManager: LeavingRoom called");
            PhotonNetwork.LeaveRoom();
            Debug.Log("GameManager: Disconnecting from PhotonNetwork");
            PhotonNetwork.Disconnect();
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
            PhotonNetwork.LoadLevel("MainScene");
        } 
        
        #endregion
    }
}