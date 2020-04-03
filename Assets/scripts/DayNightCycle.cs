using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Color DayColor;
    public Color NightColor;
    public Color TwilightColor;
    public bool isDay;
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
        dayDuration = 30;
        time = 0f;
    
    }

    // Update is called once per frame
    void Update()
    {
        Light sun = GameObject.Find("sun").GetComponent<Light>();
        setLight();

        //define if it is either the day of the night
        if(curDayTime > 0.1 && curDayTime<0.85){
            isDay = true;
        }else{
            isDay = false;
        }
    }   


    //set the animations of the sun
    void setLight(){
        Light sun = GameObject.Find("sun").GetComponent<Light>();
        time += Time.deltaTime;
        curDayTime = time/dayDuration;
        
        if(curDayTime >= 1){
            sun.transform.rotation = Quaternion.Euler(-90,0,0);
            time = 0;
            curDayTime = 0;
        }
        
        float nightPercent = 0.02f;             //percentage time of night
        float twilightDawnPercent = 0.1f;      //percentage time for transition from night to day/day to night
        float midday = (1-nightPercent-twilightDawnPercent*2)/2+twilightDawnPercent; //the middle of the day

        //setting the sun's color animation according to day time
        if(curDayTime >0 && curDayTime < twilightDawnPercent){ 
            sun.color = Color.Lerp(NightColor, TwilightColor, curDayTime/ twilightDawnPercent);
        }else if(curDayTime > twilightDawnPercent && curDayTime < midday){
            float a = -1/Mathf.Pow(twilightDawnPercent-midday,2);
            float b = -2*a*midday;
            float c = 1+a*Mathf.Pow(midday, 2);
            sun.color = Color.Lerp(TwilightColor, DayColor, (a*curDayTime*curDayTime+b*curDayTime+c));
        }else if(curDayTime >midday && curDayTime < 1-twilightDawnPercent-nightPercent){
            float a = -1/Mathf.Pow(twilightDawnPercent-midday,2);
            float b = -2*a*midday;
            float c = 1+a*Mathf.Pow(midday, 2);
            sun.color = Color.Lerp(TwilightColor, DayColor, (a*curDayTime*curDayTime+b*curDayTime+c));
        }else if(curDayTime >1-twilightDawnPercent-nightPercent && curDayTime < 1-nightPercent){ 
            sun.color = Color.Lerp(TwilightColor, NightColor, (curDayTime-(1-twilightDawnPercent-nightPercent))/twilightDawnPercent);
        }

        //setting the sun's rotation animation according to day time
        if(curDayTime >0 && curDayTime < twilightDawnPercent){ 
            sun.transform.rotation = Quaternion.Euler(curDayTime*(90/twilightDawnPercent)-90,0,0);
        }else if(curDayTime > twilightDawnPercent && curDayTime < midday){
            float a = -1/Mathf.Pow(twilightDawnPercent-midday,2);
            float b = -2*a*midday;
            float c = 1+a*Mathf.Pow(midday, 2);
            sun.transform.rotation = Quaternion.Euler(90*(a*curDayTime*curDayTime+b*curDayTime+c),0,0);
        }else if(curDayTime >midday && curDayTime < 1-twilightDawnPercent-nightPercent){
            float a = -1/Mathf.Pow(twilightDawnPercent-midday,2);
            float b = -2*a*midday;
            float c = 1+a*Mathf.Pow(midday, 2);
            //Debug.Log("rotate: "+a*curDayTime*curDayTime+b*curDayTime+c);
            sun.transform.rotation = Quaternion.Euler(-90*(a*curDayTime*curDayTime+b*curDayTime+c)+180,0,0);
        }else if(curDayTime >1-twilightDawnPercent-nightPercent && curDayTime < 1-nightPercent){ 
            sun.transform.rotation = Quaternion.Euler(curDayTime*(90/twilightDawnPercent)+180-90/twilightDawnPercent*(1-twilightDawnPercent-nightPercent),0,0);
        }

    }
}
