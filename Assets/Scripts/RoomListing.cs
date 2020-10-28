using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun;

public class RoomListing : MonoBehaviour
{

    [SerializeField]
    public Text _roomName;
    public Text _max;
    public Text _current;
    public Image _type;
    public Text _raceName;

    public RoomInfo RoomInfo { get; private set; }

    public Sprite champSprite;
    public Sprite freeSprite;

    public const string MAP_PROP_KEY = "map";
    public const string GAME_MODE_PROP_KEY = "gm";
    public const string ROUND_START_TIME = "StartTime";
    public const string RACE_NAME = "rn";

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        _raceName.text = roomInfo.CustomProperties[RACE_NAME].ToString();
        _max.text = roomInfo.MaxPlayers.ToString();
        _current.text = roomInfo.PlayerCount.ToString();
        _roomName.text = roomInfo.Name;
        /*if ((int)roomInfo.CustomProperties[GAME_MODE_PROP_KEY] == 1)
        {
            Debug.Log("RoomListing: SetRoomInfo - game type = free race" + freeSprite);
            _type.sprite = freeSprite;
        }
        else
        {
            Debug.Log("RoomListing: SetRoomInfo - game type = championship" + champSprite);
            _type.sprite = champSprite;
        }*/
    }

    public void JoinSelectedRoom(Text roomName)
    {
        Debug.Log("Joining Room: " + roomName.text);
        PhotonNetwork.JoinRoom(roomName.text);
    }

}
