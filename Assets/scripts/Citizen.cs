using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Citizen : MonoBehaviour
{
    public float prodPerMinute;
    //public string job;
    public float happiness;
    public float tiredness;
    public string job;
    public bool doingActivity;
    public bool objectForActivityFound;
    private GameObject[] objectsForActivity;
    
    private float closestObjectForActivityDist;
    private GameObject closestObjectForActivity;
    public bool prevDay;

    public List<string> objectsTag;
    // Start is called before the first frame update
    void OnEnable()
    {

        objectsTag.Add("tree");
        objectsTag.Add("resting");
        //citizen shown data
        happiness = 0.5f;
        tiredness = 0.5f;
        doingActivity = false;
        objectsForActivity = new GameObject[0];
        if(objectsForActivity.Length != 0){
            List<GameObject> tmpList = new List<GameObject>(objectsForActivity);
            tmpList.Clear();
            objectsForActivity = tmpList.ToArray();

        }

        doingActivity = false;
        objectForActivityFound = false;
        closestObjectForActivityDist = float.MaxValue;
        closestObjectForActivity = null;  
        prevDay = GameObject.Find("sun").GetComponent<DayNightCycle>().isDay;

        foreach(string curTag in objectsTag){
            foreach(GameObject curObj in GameObject.FindGameObjectsWithTag(curTag)){
                curObj.GetComponent<resourceManager>().isOccupied = false;
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        bool isDay = GameObject.Find("sun").GetComponent<DayNightCycle>().isDay;
        //defining what to do according to the hour
        if(isDay){
            if(!objectForActivityFound){
                gameObject.GetComponent<NavMeshAgent>().ResetPath();
                goToWork(job);
            }
        } else{
           if(!objectForActivityFound){
                goToWork("rest");
            } 
        }
        


        resetAnimation(isDay);
        
        //reset behaviour when day time changes
        if(prevDay != isDay){
            gameObject.GetComponent<Citizen>().OnEnable();
        }
        prevDay = isDay;
        
    }




    //compute the distance between 2 points in the 3d space
    float distBetweenPos(Vector3 obj1, Vector3 obj2){
        float posX = Mathf.Pow(obj1.x-obj2.x, 2);
        float posY = Mathf.Pow(obj1.y-obj2.y, 2);
        float posZ = Mathf.Pow(obj1.z-obj2.z, 2);
        return Mathf.Sqrt(posX+posY+posZ);
    }


    //try to find an object to work with 
    private void goToWork(string activity){
        List<GameObject> objectsForActivityList = new List<GameObject>();
        //get all objects on the map according to the activity/job.
        //TODO To update when adding a job/activity
        if(activity == "lumberjack"){
            objectsForActivity = GameObject.FindGameObjectsWithTag("tree");
            objectsForActivityList = new List<GameObject>(objectsForActivity);
        }else if(activity == "rest"){
            objectsForActivity = GameObject.FindGameObjectsWithTag("resting");
            objectsForActivityList = new List<GameObject>(objectsForActivity);
        }

        while(objectsForActivityList.Count != 0 && !objectForActivityFound){
            //find the closest object from the character
            closestObjectForActivityDist = float.MaxValue;
            foreach(GameObject curObj in objectsForActivityList){
                float curDist = distBetweenPos(curObj.GetComponent<Collider>().bounds.center, gameObject.GetComponent<Collider>().bounds.center);
                if(curDist < closestObjectForActivityDist){
                    closestObjectForActivityDist = curDist;
                    closestObjectForActivity = curObj;
                }
            }

            //check if object is already used bysomeone, and if not move to the object
            if(closestObjectForActivity.GetComponent<resourceManager>().isOccupied){
                objectsForActivityList.Remove(closestObjectForActivity);
            }else{
                closestObjectForActivity.GetComponent<resourceManager>().isOccupied = true;
                objectForActivityFound = true;
                getPath(closestObjectForActivity);
            }
        }
        

    }
    
    //moving to an object
    void getPath(GameObject dest){
        Vector3 destCoord = new Vector3();
        NavMeshPath path = new NavMeshPath();

        //define a random destination point arround the destination object
        float corrector;
        corrector  = 0.5f;
        float min = dest.GetComponent<Collider>().bounds.center.x - dest.GetComponent<Collider>().bounds.size.x/2-corrector;
        float max = dest.GetComponent<Collider>().bounds.center.x + dest.GetComponent<Collider>().bounds.size.x/2+corrector;
        destCoord.x = Random.Range(min, max);
        destCoord.y= gameObject.transform.position.y;
        destCoord.z = Mathf.Sqrt(Mathf.Pow(dest.GetComponent<Collider>().bounds.size.x/2+corrector,2)-Mathf.Pow(destCoord.x-dest.GetComponent<Collider>().bounds.center.x,2))+dest.GetComponent<Collider>().bounds.center.z;

        //compute the path to go to the point calculated before
        NavMesh.CalculatePath(gameObject.transform.position,destCoord,NavMesh.AllAreas, path);
        for (int i = 0; i < path.corners.Length - 1; i++){
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        }
        //move to the point
        gameObject.GetComponent<NavMeshAgent>().SetDestination(destCoord);
    }


    //setting the right animation according to the current action
    private void resetAnimation(bool isDay){
        Animator animator = gameObject.GetComponent<Animator>();
        NavMeshAgent agent = gameObject.GetComponent<NavMeshAgent>();
        if(!objectForActivityFound){
            //idle when no object has been found to work with
            animator.SetBool("isWorking", false);
            animator.SetFloat("speed", 0);
        }else if(!doingActivity){
            //either walking if an object has been found but not reached yet, or working if it is day time
            gameObject.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(gameObject.transform.forward, closestObjectForActivity.transform.position - gameObject.transform.position, 20*Time.deltaTime, 0.0f));
            if(!gameObject.GetComponent<NavMeshAgent>().pathPending && gameObject.GetComponent<NavMeshAgent>().pathStatus == NavMeshPathStatus.PathComplete && gameObject.GetComponent<NavMeshAgent>().remainingDistance == 0){
                animator.SetBool("isWorking", isDay);
                doingActivity = true;
                animator.SetFloat("speed", 0);
            }else{
                animator.SetFloat("speed", agent.speed);
                animator.SetBool("isWorking", false);
            }
        }
    }
        
}
