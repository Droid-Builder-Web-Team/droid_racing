using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun;

public class RoomListing : MonoBehaviour
{
  
    [SerializeField]
    public Text _text;
    public Text _max;
    public Text _current;
    
    public RoomInfo RoomInfo { get; private set; }
    
    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        _text.text = roomInfo.Name;
        _max.text = roomInfo.MaxPlayers.ToString();
        _current.text = roomInfo.PlayerCount.ToString();
        //_text.text = "(" + roomInfo.MaxPlayers + ") " + roomInfo.Name;
    }

    public void JoinSelectedRoom(Text text) 
    {
        Debug.Log("Joining Room: " + text.text);
        PhotonNetwork.JoinRoom(text.text);
    }

}
