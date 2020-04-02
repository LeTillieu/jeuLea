using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildingPlacement : MonoBehaviour
{
    public GameObject cam;
    private GameObject currentBuilding;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        Vector3 t = Input.mousePosition;
        Debug.DrawLine(Camera.main.ScreenPointToRay(t).origin, Camera.main.ScreenPointToRay(t).origin+1000*Camera.main.ScreenPointToRay(t).direction);
        RaycastHit hit = new RaycastHit();
        if(currentBuilding != null){
            if(Physics.Raycast(Camera.main.ScreenPointToRay(t).origin, Camera.main.ScreenPointToRay(t).direction,out hit, Mathf.Infinity, LayerMask.GetMask("ground"))){
                if(hit.point != currentBuilding.transform.position){
                    currentBuilding.transform.position = hit.point;
                }
            }
        }
        

                
            
    }

    public void SetItem(GameObject b)
    {
        currentBuilding = (GameObject)Instantiate(b);
    }

    
}
