using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class NameTextHandler : MonoBehaviour
{
    private Camera _mainCam;
    [SerializeField]
    private float _offsetY;
    [SerializeField]
    private GameObject _player;
    [ReadOnly]
    public TextMeshProUGUI nameText;
    private void Start()
    {
        string name = PhotonNetwork.NickName;
        nameText = GetComponent<TextMeshProUGUI>();
        _mainCam = PlayerController.localPlayerCtrl.GetComponent<PlayerController>().PersonalCamera ??
            PlayerLobbyController.localPlayerCtrl.GetComponent<PlayerLobbyController>().personalCamera;

        GetComponentInParent<Canvas>().worldCamera = _mainCam;
      

        if (_player != null)
        {
            if (_player.GetComponent<PlayerController>() != null)
            {
                _player.GetComponent<PlayerController>().CallSetNameRPC();
            }
            else if (_player.GetComponent<PlayerLobbyController>() != null)
            {
                _player.GetComponent<PlayerLobbyController>().CallSetNameRPC();
            }
            SetIconPosition();
            
        }
       
    }
   
    private void LateUpdate()
    {
        SetIconPosition();
    }

    private void SetIconPosition()
    {
        if (_mainCam != null)
        {
            transform.position = _player.transform.position + Vector3.up * _offsetY;
            transform.LookAt(_mainCam.transform.position, Vector3.up);
        }
    }
}
