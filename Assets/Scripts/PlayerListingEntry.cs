using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerListingEntry : MonoBehaviourPunCallbacks
{

    [SerializeField]
    public Text _name;
    public Text _laps;
    public Image _color;
    //public Text _current;

    public Player PlayerInfo { get; private set; }

    public void SetPlayerInfo(Player playerInfo)
    {
        Debug.Log("PlayerListingEntry: SetPlayerInfo called");
        PlayerInfo = playerInfo;
        _name.text = playerInfo.NickName;
        Color color = new Color(
                (float)playerInfo.CustomProperties["rValue"],
                (float)playerInfo.CustomProperties["gValue"],
                (float)playerInfo.CustomProperties["bValue"]
            );
        _color.color = color;

    }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {
        Debug.Log("PlayerListingEntry: OnPlayerPropertiesUpdate called");
        if (PlayerInfo.NickName == target.NickName)
        {
            Debug.Log("PlayerListingEntry: Updating laps");
            this._laps.text = target.CustomProperties["laps"].ToString();
            Color color = new Color(
                    (float)target.CustomProperties["rValue"],
                    (float)target.CustomProperties["gValue"],
                    (float)target.CustomProperties["bValue"]
                );
            this._color.color = color;

        }
        Debug.Log("PlayerListingEntry: OnPlayerPropertiesUpdate finished");
    }


}
