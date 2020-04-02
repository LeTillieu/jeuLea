using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    private GameObject curHouse;
    private GameObject smoke;
    public float nbInhabitants;
    // Start is called before the first frame update
    void Start()
    {
        curHouse = gameObject;
        smoke = curHouse.transform.Find("smoke").gameObject;
        nbInhabitants = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        this.updateSmoke();//to Remove
    }
    //function to call on update of the amount of inhabitants in the house 
    void updateSmoke(){
        if(nbInhabitants > 0){
            smoke.SetActive(true);
        }else{
            smoke.SetActive(false);
        }
    }
}
