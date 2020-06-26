using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerListingMenu : MonoBehaviourPunCallbacks
{
  
    [SerializeField]
    private Transform _content;
    
    [SerializeField]
    private PlayerListingEntry _playerListing;
    
    private List<PlayerListingEntry> _listings = new List<PlayerListingEntry>();
    
    public override void OnPlayerEnteredRoom(Player player) 
    {
        Debug.Log("PlayerListingMenu: OnPlayerEnteredRoom()");
        PlayerListingEntry listing = Instantiate(_playerListing, _content);
        listing.SetPlayerInfo(player);
        _listings.Add(listing);
        Debug.Log("PlayerListingMenu: Number of entries in _listings: " + _listings.Count);
    }
    
    public override void OnPlayerLeftRoom(Player player)
    {
        Debug.Log("PlayerListingMenu: OnPlayerLeftRoom()");
        Debug.Log("PlayerListingMenu: _listing._name " + _listings[0]._name.ToString());
        int index = _listings.FindIndex(x => x._name.ToString() == player.NickName);              
        if (index != -1) 
        {
            Debug.Log("PlayerListingMenu: index for removal: " + index);
            Destroy(_listings[index].gameObject);
            _listings.RemoveAt(index);
        }
    }
    
    void Start() 
    {
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            PlayerListingEntry listing = Instantiate(_playerListing, _content);
            listing.SetPlayerInfo(PhotonNetwork.LocalPlayer);
            _listings.Add(listing);
        }
    }
    
    void Update() 
    {
        //Debug.Log("PlayerListingMenu: Update()");
    }
}
  