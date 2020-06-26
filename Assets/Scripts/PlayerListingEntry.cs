using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerListingEntry : MonoBehaviour
{
  
    [SerializeField]
    public Text _name;
    public Text _laps;
    //public Text _current;
    
    public Player PlayerInfo { get; private set; }
    
    public void SetPlayerInfo(Player playerInfo)
    {
        PlayerInfo = playerInfo;
        _name.text = playerInfo.NickName;
        //_laps.text = playerInfo.CustomProperties["laps"].ToString();
    }


}