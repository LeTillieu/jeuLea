using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Color DayColor;
    public Color NightColor;
    public Color TwilightColor;

    public float intensity;
    public float dayDuration;           //day duration in seconds
    public float time;                  //time of the day in seconds
    public float curDayTime;            //time of the day between 0 et 1
    // Start is called before the first frame update
    void Start()
    {
        DayColor = new Color(1,1,1);
        NightColor = new Color(0,0,0);
        TwilightColor = new Color(210f/255f,85f/255f,0f/255f,255f/255f);
        intensity = 0.5f;
        dayDuration = 10;
        time = 0f;
    
    }

    // Update is called once per frame
    void Update()
    {Light sun = GameObject.Find("sun").GetComponent<Light>();
    setLight();
    }   

    void setLight(){
        Light sun = GameObject.Find("sun").GetComponent<Light>();
        time += Time.deltaTime;
        curDayTime = time/dayDuration;
        
        if(curDayTime >= 1){
            sun.transform.rotation = Quaternion.Euler(-90,0,0);
            time = 0;
            curDayTime = 0;
        }

        if(curDayTime >0 && curDayTime < 0.1){ 
            sun.color = Color.Lerp(NightColor, TwilightColor, curDayTime/0.1f);
        }else if(curDayTime >0.1 && curDayTime < 0.475){
            
            float a = -1;
            float b = 0.95f;
            float c = (0.5625f-0.9025f)/4;
            sun.color = Color.Lerp(TwilightColor, DayColor, 1/0.140625f*(a*curDayTime*curDayTime+b*curDayTime+c));
        }else if(curDayTime >0.475 && curDayTime < 0.85){
            
            float a = -1;
            float b = 0.95f;
            float c = (0.5625f-0.9025f)/4;
            sun.color = Color.Lerp(TwilightColor, DayColor, 1/0.140625f*(a*curDayTime*curDayTime+b*curDayTime+c));
        }else if(curDayTime >0.85 && curDayTime < 0.95){ 
            sun.color = Color.Lerp(TwilightColor, NightColor, (curDayTime-0.85f)/0.1f);
        }

        //Debug.Log(curDayTime);
        if(curDayTime >0 && curDayTime < 0.1){ 
            sun.transform.rotation = Quaternion.Euler(curDayTime*(90/0.1f)-90,0,0);
        }else if(curDayTime >0.1 && curDayTime < 0.475){
            
            float a = -1;
            float b = 0.95f;
            float c = (0.5625f-0.9025f)/4;
            //Debug.Log("rotate: "+a*curDayTime*curDayTime+b*curDayTime+c);
            sun.transform.rotation = Quaternion.Euler(640*(a*curDayTime*curDayTime+b*curDayTime+c),0,0);
        }else if(curDayTime >0.475 && curDayTime < 0.85){
            
            float a = -1;
            float b = 0.95f;
            float c = (0.5625f-0.9025f)/4;
            //Debug.Log("rotate: "+a*curDayTime*curDayTime+b*curDayTime+c);
            sun.transform.rotation = Quaternion.Euler(-640*(a*curDayTime*curDayTime+b*curDayTime+c)+180,0,0);
        }else if(curDayTime >0.85 && curDayTime < 0.95){ 
            sun.transform.rotation = Quaternion.Euler(curDayTime*(90/0.1f)-590,0,0);
        }

    }
}
