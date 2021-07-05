
using Photon.Realtime;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Photon.Pun.Demo.PunBasics
{
#pragma warning disable 649

    /// <summary>
    /// Game manager.
    /// Connects and watch Photon Status, Instantiate Player
    /// Deals with quiting the room and the game
    /// Deals with level loading (outside the in room synchronization)
    /// </summary>
    public class GameManager : MonoBehaviourPunCallbacks
    {

        #region Public Fields

        static public GameManager instance;

        #endregion

        #region Private Fields

        public Camera blueCamera;
        public Camera redCamera;
        public event Action OnGameStart;

        [Tooltip("The prefab to use for representing the player")]
        [SerializeField]
        private GameObject playerPrefab;

        [SerializeField]
        SpawnPoint[] team1SpawnPoints;
        [SerializeField]
        SpawnPoint[] team2SpawnPoints;

        [HideInInspector]
        public GameObject localPlayerObject;

        [SerializeField]
        private float _startDelay = 10;
        #endregion

        #region State
        private bool _gameStarted = false;
        public bool GameStarted
        {
            get => _gameStarted;
            set
            {
                if (_gameStarted == value)
                    return;

                _gameStarted = value;

                if (_gameStarted)
                    OnGameStart?.Invoke();
            }
        }
        #endregion

        #region MonoBehaviour CallBacks

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
                Destroy(gameObject);

            if (PhotonNetwork.IsMasterClient)
                StartCoroutine(StartAfterDelayRoutine(_startDelay));
        }
        private IEnumerator StartAfterDelayRoutine(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);

            photonView.RPC("StartAfterDelayRPC", RpcTarget.All);
        }
        [PunRPC]
        private void StartAfterDelayRPC() => _gameStarted = true;
        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            Array.ForEach(PlayerInformation.players, (x) => x.spawnPoint = GetSpawnPoint(x.player));

            // in case we started this demo with the wrong scene being active, simply load the menu scene
            if (!PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene("PunBasics-Launcher");// fill in our ui scene

                return;
            }

            if (playerPrefab == null)
            { // #Tip Never assume public properties of Components are filled up properly, always check and inform the developer of it.

                Debug.LogError("<Color=Red><b>Missing</b></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {


                if (PlayerController.localPlayerCtrl == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    var spawnPoint = PlayerInformation.GetPlayerData(PhotonNetwork.LocalPlayer).spawnPoint;

                    if (spawnPoint != null)
                    {
                        localPlayerObject = PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPoint.transform.position, Quaternion.identity, 0);

                        //localPlayerObject.transform.SetParent(spawnPoint.spawnPoint.GetComponentInParent<ShipManager>().transform);

                        spawnPoint.taken = true;
                    }
                }
                else
                {

                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }


            }

        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity on every frame.
        /// </summary>
        void Update()
        {
            // "back" button of phone equals "Escape". quit app if that's pressed
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                QuitApplication();
            }
        }

        #endregion

        #region Photon Callbacks
        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("PunBasics-Launcher");
        }
        #endregion

        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void QuitApplication()
        {
            Application.Quit();
        }
        public SpawnPoint GetSpawnPoint(Player player)
        {
            if (player.GetPlayerTeam() == Team.Blue)
                return Array.Find(team1SpawnPoints, (x) => x.taken == false);
            else
                return Array.Find(team2SpawnPoints, (x) => x.taken == false);
        }

        public void LoadLobby()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("Lobby");
                Debug.Log("Trying to load Lobby.");
            }
        }
        public void LoadLauncher()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("Launcher");
                Debug.Log("Trying to load Launcher scene.");
            }
        }
        #endregion

        #region Private Methods
        #endregion
    }
    [Serializable]
    public class SpawnPoint
    {
        [HideLabel]
        public Transform transform;
        [HideInInspector]
        public bool taken = false;
    }


}