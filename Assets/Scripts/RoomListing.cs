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
    public Image _type;
    
    public RoomInfo RoomInfo { get; private set; }
    
    public Sprite champSprite;
    public Sprite freeSprite;
    
    public const string MAP_PROP_KEY = "map";
    public const string GAME_MODE_PROP_KEY = "gm";   
    public const string ROUND_START_TIME = "StartTime";
    
    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        _text.text = roomInfo.Name;
        _max.text = roomInfo.MaxPlayers.ToString();
        _current.text = roomInfo.PlayerCount.ToString();
        if ((int)roomInfo.CustomProperties[GAME_MODE_PROP_KEY] == 1)
        {
            Debug.Log("RoomListing: SetRoomInfo - game type = free race" + freeSprite);
            _type.sprite = freeSprite;
        }
        else 
        {
            Debug.Log("RoomListing: SetRoomInfo - game type = championship" + champSprite);
            _type.sprite = champSprite;
        }
    }

    public void JoinSelectedRoom(Text text) 
    {
        Debug.Log("Joining Room: " + text.text);
        PhotonNetwork.JoinRoom(text.text);
    }

}
