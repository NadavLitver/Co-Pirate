
using Photon.Realtime;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Photon.Pun.Demo.PunBasics
{
#pragma warning disable 649

    /// <summary>
    /// Game manager.
    /// Connects and watch Photon Status, Instantiate Player
    /// Deals with quiting the room and the game
    /// Deals with level loading (outside the in room synchronization)
    /// </summary>
    /// 

    public class LiveLobbyGameManager : MonoBehaviourPunCallbacks
    {

        #region Public Fields

        static public LiveLobbyGameManager instance;

        #endregion

        #region Private Fields

        [ReadOnly]
        public int readyCount;


        [Tooltip("The prefab to use for representing the player")]
        [SerializeField]
        private GameObject playerPrefab;

        [SerializeField]
        SpawnPoint[] team1SpawnPoints;
        [SerializeField]
        SpawnPoint[] team2SpawnPoints;

        public Button readyButton;
        public TextMeshProUGUI debugText;

        [ReadOnly]
        public bool isReady;

        [HideInInspector]
        public Player localPlayer;
        [HideInInspector]
        public GameObject localPlayerObject;
        #endregion
        #region MonoBehaviour CallBacks

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            InputManager.controls.Gameplay.Speak.performed += SetLocalReady;

            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
            }
        }
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
        public void SetLocalReady(InputAction.CallbackContext context)
        {

            isReady = !isReady;

            CallReadyRPC(isReady);
        }
        public void CallReadyRPC(bool isReady)
        {
            try
            {
                if (isReady)

                    photonView.RPC("AddReadyRPC", RpcTarget.All);

                else
                    photonView.RPC("RetractReadyRPC", RpcTarget.All);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

        }

        [PunRPC]
        public void AddReadyRPC()
        {
            readyCount++;

            debugText.text = "Players Ready: " + readyCount;
            JoinGameRoom();
        }
        [PunRPC]
        public void RetractReadyRPC()

        {
            readyCount--;
            debugText.text = "Players Ready: " + readyCount;
            JoinGameRoom();

        }
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
        #endregion
        public void JoinGameRoom()
        {
            if (PhotonNetwork.IsMasterClient && readyCount == LobbyInfromation._numOfPlayer)
            {
                PhotonNetwork.LoadLevel("Room For 4");
                Debug.Log("Trying to load game scene.");
            }

        }
        #region Private Methods
        #endregion

    }

}
