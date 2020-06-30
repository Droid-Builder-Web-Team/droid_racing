using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using Photon.Pun;

public class ResultsListingEntry : MonoBehaviourPunCallbacks
{
  
    [SerializeField]
    public Text _name;
    public Text _laps;
    public Text _position;
    //public Text _current;
        
    public void SetResultsInfo(int laps, string name, int pos)
    {
        Debug.Log("ResultsListingEntry: SetResultsInfo()");
        _name.text = name;
        _laps.text = laps.ToString();
        _position.text = pos.ToString();
    }
}
