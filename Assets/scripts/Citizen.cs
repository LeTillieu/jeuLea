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
    public bool isWorking;
    public bool objectToWorkFound;

    private GameObject[] objectsToWork;
    
    private float closestObjectToWorkDist;
    public GameObject closestObjectToWork;

    private Vector3 prevCoord = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
    //citizen shown data
        happiness = 0.5f;
        tiredness = 0.5f;
        isWorking = false;

        closestObjectToWorkDist = float.MaxValue;
        objectToWorkFound = false;
        prevCoord = gameObject.transform.position;
        
    }



    // Update is called once per frame
    void Update()
    {
        Animator animator = gameObject.GetComponent<Animator>();
        NavMeshAgent agent = gameObject.GetComponent<NavMeshAgent>();
        NavMeshPath path = new NavMeshPath();
        if(!objectToWorkFound){
            if(job == "lumberjack"){
                //isWorking = true; // TODO: to move
                objectsToWork = GameObject.FindGameObjectsWithTag("tree");
                closestObjectToWorkDist = float.MaxValue;
            }
            int clostestId = -1;
            while(closestObjectToWorkDist == float.MaxValue && objectsToWork.Length != 0){
                foreach(GameObject curObj in objectsToWork){
                    float curDist = distBetweenPos(curObj.GetComponent<Collider>().bounds.center, gameObject.GetComponent<Collider>().bounds.center);
                    if(curDist < closestObjectToWorkDist && curObj.activeInHierarchy){
                        clostestId++;
                        closestObjectToWorkDist = curDist;
                        closestObjectToWork = curObj;
                    }
                }

                if(closestObjectToWork.GetComponent<resourceManager>().isOccupied){
                    closestObjectToWorkDist = float.MaxValue;
                    List<GameObject> tmpList = new List<GameObject>(objectsToWork);
                    tmpList.Remove(closestObjectToWork);
                    objectsToWork = tmpList.ToArray();
                }else{
                    closestObjectToWork.GetComponent<resourceManager>().isOccupied = true;
                    getPath(closestObjectToWork);
                    objectToWorkFound = true;
                }
            }           

        }

        if(closestObjectToWorkDist != float.MaxValue && !isWorking){
            gameObject.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(gameObject.transform.forward, closestObjectToWork.transform.position - gameObject.transform.position, 2*Time.deltaTime, 0.0f));
            if(!gameObject.GetComponent<NavMeshAgent>().pathPending && gameObject.GetComponent<NavMeshAgent>().pathStatus == NavMeshPathStatus.PathComplete && gameObject.GetComponent<NavMeshAgent>().remainingDistance == 0){
                animator.SetBool("isWorking", true);
                isWorking = true;
                animator.SetFloat("speed", 0);
            }else{
                animator.SetFloat("speed", agent.speed);
                animator.SetBool("isWorking", false);
            }
        }
        
        
        
        
             
    }

    float distBetweenPos(Vector3 obj1, Vector3 obj2){
        float posX = Mathf.Pow(obj1.x-obj2.x, 2);
        float posY = Mathf.Pow(obj1.y-obj2.y, 2);
        float posZ = Mathf.Pow(obj1.z-obj2.z, 2);
        return Mathf.Sqrt(posX+posY+posZ);
    }

    void getPath(GameObject dest){
        Vector3 destCoord = new Vector3();
        NavMeshPath path = new NavMeshPath();

        float corrector;
        corrector  = 0.5f;
        float min = closestObjectToWork.GetComponent<Collider>().bounds.center.x - closestObjectToWork.GetComponent<Collider>().bounds.size.x/2-corrector;
        float max = closestObjectToWork.GetComponent<Collider>().bounds.center.x + closestObjectToWork.GetComponent<Collider>().bounds.size.x/2+corrector;
        destCoord.x = Random.Range(min, max);
        destCoord.y= gameObject.transform.position.y;
        destCoord.z = Mathf.Sqrt(Mathf.Pow(closestObjectToWork.GetComponent<Collider>().bounds.size.x/2+corrector,2)-Mathf.Pow(destCoord.x-closestObjectToWork.GetComponent<Collider>().bounds.center.x,2))+closestObjectToWork.GetComponent<Collider>().bounds.center.z;

        NavMesh.CalculatePath(gameObject.transform.position,destCoord,NavMesh.AllAreas, path);
        for (int i = 0; i < path.corners.Length - 1; i++){
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        }
        gameObject.GetComponent<NavMeshAgent>().destination = destCoord;
    }

   

}
