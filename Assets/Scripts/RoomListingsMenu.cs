using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private RoomListing _roomListing;
    
    private List<RoomListing> _listings = new List<RoomListing>();
    
    public override void OnRoomListUpdate(List<RoomInfo> roomList) 
    {
      Debug.Log("Room list updated");
      foreach (RoomInfo info in roomList)  
      {
          if (info.RemovedFromList) 
          {
              Debug.Log("Room to be removed from List: " + info.Name);
              int index = _listings.FindIndex(x => x.RoomInfo.Name == info.Name);              
              if (index != -1) 
              {
                 Destroy(_listings[index].gameObject);
                 _listings.RemoveAt(index);
              } 
          }
          else 
          {
              RoomListing listing = Instantiate(_roomListing, _content);
                  if (listing != null)
                      listing.SetRoomInfo(info);
                      _listings.Add(listing);
          }
      }
      
    }
}
  