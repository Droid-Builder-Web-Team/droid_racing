using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class WebCalls : MonoBehaviour
{
  
    public string apiKey;
    public string apiBase;
    
    // Start is called before the first frame update
    public void UploadLap(string email, string name, float lap_time, int participents, string room_name)
    {
        Debug.Log("Uploading Lap Time");
        string hash = Md5Sum(email + name + lap_time + apiKey);
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("name", name);
        form.AddField("lap_time", lap_time.ToString());
        form.AddField("participents", participents.ToString());
        form.AddField("room_name", room_name);
        form.AddField("hash", hash);
        form.AddField("api", apiKey);
        WWW www = new WWW(apiBase, form);
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
