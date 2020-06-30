using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//using System.Net;

public class WebCalls : MonoBehaviour
{
  
    public string apiKey;
    public string apiBase;
    
    public IEnumerator UploadLap(string email, string name, float lap_time, int participents, string room_name)
    {
        Debug.Log("WebCalls: Uploading Lap Time");
        string hash = Md5Sum(email + name + apiKey);
        WWWForm form = new WWWForm();
        form.AddField("type", "lap");
        form.AddField("email", email);
        form.AddField("name", name);
        form.AddField("lap_time", lap_time.ToString());
        form.AddField("participents", participents.ToString());
        form.AddField("room_name", room_name);
        form.AddField("hash", hash);
        form.AddField("api", apiKey);
        using (UnityWebRequest www = UnityWebRequest.Post(apiBase, form))
        {
            yield return www.SendWebRequest();
            
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("WebCalls: " + www.error);
            }
            else 
            {
                Debug.Log("WebCalls: Lap uploaded");
            }
        }
        //WWW www = new WWW(apiBase, form);
    }
    
    public IEnumerator UploadRace(string email, string name, float fastest_time, int number_laps, int participents, string room_name, float race_length)
    {
        Debug.Log("Uploading Race Results");
        string hash = Md5Sum(email + name + apiKey);
        WWWForm form = new WWWForm();
        form.AddField("type", "race");
        form.AddField("email", email);
        form.AddField("name", name);
        form.AddField("fastest_time", fastest_time.ToString());
        form.AddField("number_laps", number_laps.ToString());
        form.AddField("participents", participents.ToString());
        form.AddField("room_name", room_name);      
        form.AddField("race_length", race_length.ToString());
        form.AddField("hash", hash);
        form.AddField("api", apiKey);
        using (UnityWebRequest www = UnityWebRequest.Post(apiBase, form))
        {
            yield return www.SendWebRequest();
            
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("WebCalls: " + www.error);
            }
            else 
            {
                Debug.Log("WebCalls: Race uploaded");
            }
        }
        //WWW www = new WWW(apiBase, form);
    }
    
    public string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);
        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);
        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";
        for (int i = 0; i < hashBytes.Length; i++)
        {
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        }
        return hashString.PadLeft(32, '0');
    }    
}
