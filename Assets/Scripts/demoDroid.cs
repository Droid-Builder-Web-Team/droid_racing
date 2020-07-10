using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class demoDroid : MonoBehaviour
{
  
    //public int speedOfRotation = 5;
    private float curveAmount = 0.0f;
    private float rotateSpeed = 100.0f;
    
    public Slider red;
    public Slider blue;
    public Slider green;
    
    private Material mat;
    
    // Start is called before the first frame update
    void Start()
    {
        mat = this.GetComponentInChildren<Renderer>().material;
        
        Color randomcolor = Random.ColorHSV();
        Color color = new Color(
            PlayerPrefs.GetFloat("rValue", randomcolor.r),
            PlayerPrefs.GetFloat("gValue", randomcolor.g),
            PlayerPrefs.GetFloat("bValue", randomcolor.b)
        );
        mat.SetColor("_Color", color);
        red.value = color.r;
        green.value = color.g;
        blue.value = color.b;
    }

    // Update is called once per frame
    void Update() {

        this.transform.Rotate(0,0.5f,0);

    }
    
    public void SetColorRed(float value)
    {
        Color currentColor = mat.GetColor("_Color");
        
        currentColor.r = value; 
        mat.SetColor("_Color", currentColor);
        SaveColor();
    }
    
    public void SetColorGreen(float value)
    {
        Color currentColor = mat.GetColor("_Color");
        
        currentColor.g = value; 
        mat.SetColor("_Color", currentColor);
        SaveColor();
    }
    
    public void SetColorBlue(float value)
    {
        Color currentColor = mat.GetColor("_Color");
        
        currentColor.b = value; 
        mat.SetColor("_Color", currentColor);
        SaveColor();
    }
    
    void SaveColor() 
    {
        Color color = mat.GetColor("_Color");
        PlayerPrefs.SetFloat("rValue", color.r);
        PlayerPrefs.SetFloat("gValue", color.g);
        PlayerPrefs.SetFloat("bValue", color.b);
    }
}
