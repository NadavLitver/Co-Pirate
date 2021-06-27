using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NameTextHandler : MonoBehaviour
{
    private Camera _mainCam;
    [SerializeField]
    private float _offsetY;
    [SerializeField]
    private GameObject _player;
    private void Start()
    {
        _mainCam = PlayerController.localPlayerCtrl.GetComponent<PlayerController>().PersonalCamera ??
            PlayerLobbyController.localPlayerCtrl.GetComponent<PlayerLobbyController>().personalCamera;

        GetComponentInParent<Canvas>().worldCamera = _mainCam;

        this.GetComponent<TextMeshProUGUI>().text = PhotonNetwork.NickName;

        if (_player != null)
            SetIconPosition();
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
