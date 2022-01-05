using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Regulator : MonoBehaviour
{
    public class Rank
    {

        


        public List<bool> occupied;
        public int size;
        public Rank(int Size)
        {
            size = Size;
            occupied = new List<bool>();
            for (int i = 0; i < size; i++)
                occupied.Add(false);
        }

    }

    public GameObject CloneFighter1;
    public GameObject CloneFighter2;
    public GameObject attackStar;
    public GameObject FollowLights;
    public static List<Rank> ranks;
    public int shotsAvailable;
    public enum AIStates { Grow, Shoot, Aim, Follow, Multiply, Teleport, FlyOff}
    public bool switchToGrowState;
    public bool findNewState;
    public Vector3 growTarget;
    // Start is called before the first frame update
    public GameObject Player1;
    public AIStates aiState;

    public bool flyOffDirSet;
    public float flyOffDist;
    float speed;
    public int health;

    public int actionTimer = 5;
    Vector3 targetDestination;
    float distanceToDestination;
    float oldDistanceToDestination;

    bool isMoving;
    Transform fakeTransform;
    public static Quaternion Rotation;
    public static Transform cTransform;
    enum MovingStates { Open, Aim, Move}
    MovingStates movingState;
    void Start()
    {
        speed = 5;
        health = 100;
        ranks = new List<Rank>();
        shotsAvailable = 0;
        ranks.Add(new Rank(5));
        findNewState = false;
        switchToGrowState = false;
        growTarget = new Vector3();
        aiState = AIStates.Follow;
        movingState = MovingStates.Open;

     //   CloneFighter1 = Instantiate(CloneFighter1, transform.position + transform.right * (5.0f), Quaternion.identity);
      //  CloneFighter2 = Instantiate(CloneFighter1, transform.position - transform.right * (5.0f), Quaternion.identity);
      //  CloneFighter1.gameObject.SetActive(false);
      //  CloneFighter2.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (health <= 0)
            Destroy(this.gameObject);
        cTransform = transform;
        if (findNewState)
        {
            int randInt = Random.Range(0, 5);

            if (randInt == 0)
                aiState = AIStates.Follow;
            else if (randInt == 5)
            {
                aiState = AIStates.FlyOff;
                flyOffDirSet = false;
            }
            else if (randInt == 4)
                aiState = AIStates.Aim;
            else if (randInt == 1)
                aiState = AIStates.Multiply;
            else if (randInt == 2)
                aiState = AIStates.Teleport;
            findNewState = false;

            //    Debug.Log("FindNewState");
        }
        else if (switchToGrowState)
        {
            switchToGrowState = false;

            aiState = AIStates.Grow;

            movingState = MovingStates.Open;
            //   Debug.Log("STGS");

        }
        else if(aiState == AIStates.FlyOff)
        {
            if(!flyOffDirSet)
            { flyOffDirSet = true;

                flyOffDist = Random.value * 40;
                Vector2 xz = Random.insideUnitCircle;
                transform.forward = new Vector3(xz.x, 0.0f, xz.y);
                targetDestination = transform.position + transform.forward * flyOffDist;
                speed = Random.value * 15;
            }

            else
            {

                if (movingState == MovingStates.Move)
                {
                    if (!atDestination())
                    {
                        //   Debug.Log("Moving not at destination");
                        transform.position += transform.forward * speed * Time.deltaTime;

                    }
                    else
                        findNewState = true;

                }
               
            }
        }
        else if (aiState == AIStates.Teleport)
        {
            int upChange = Random.Range(10, 20);
            int sideChange = Random.Range(10, 20);

            int upMultx = Random.Range(-1, 1);
            int sideMultx = Random.Range(-1, 1);
             transform.position = Player1.transform.position + Player1.transform.forward * upMultx * upChange + Player1.transform.right * sideMultx * sideChange;
            findNewState = true;


        }
        else if (aiState == AIStates.Multiply)
        {


            Instantiate(CloneFighter1, transform.position + transform.right * (5.0f), Quaternion.identity);
            Instantiate(CloneFighter1, transform.position - transform.right * (5.0f), Quaternion.identity);
            CloneFighter1.transform.position = transform.position + transform.right * (5.0f);
            CloneFighter2.transform.position = transform.position - transform.right * (5.0f);

            CloneFighter1.gameObject.SetActive(true);
            CloneFighter2.gameObject.SetActive(true);
            findNewState = true;

        }
       else if(aiState == AIStates.Aim)
        {
            Transform[] MainStarTransforms = GetComponentsInChildren<Transform>();
           
          // Debug.Log("AIM");
            transform.LookAt(Player1.transform.position);
           // MainStarTransforms[1].gameObject.transform.rotation = Quaternion.LookRotation(Player1.transform.position - transform.position);

            int shootIndex = Random.Range(0, shotsAvailable);
            int aShot = 0;
            for(int i = 0; i<ranks[0].occupied.Count; i++)
                if(ranks[0].occupied[i])
                { aShot++;
                    if (aShot == shootIndex)
                        break;
                }
            {
                Transform[] transforms = FollowLights.GetComponentsInChildren<Transform>();

                //int rank = transforms.Length / shoot;

             //  Transform[] MainStarTransforms = GetComponentsInChildren<Transform>();
             
                    if (ranks[0].occupied[aShot])
                    {
                        MainStarTransforms[1].gameObject.GetComponent<Renderer>().material.color = transforms[aShot + 6].gameObject.GetComponent<Renderer>().material.color;
                      //  MainStarTransforms[1].gameObject.GetComponent<Light>().color = transforms[aShot + 6].gameObject.GetComponent<Renderer>().material.color;

                        transforms[aShot + 6].gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1);

                        transforms[aShot + 6].gameObject.GetComponent<Renderer>().enabled = false;

                    }


                }




            aiState = AIStates.Shoot;
        }
        else if(aiState == AIStates.Shoot)
        {
            Transform[] MainStarTransforms = GetComponentsInChildren<Transform>();

           // MainStarTransforms[1].gameObject.transform.rotation = Quaternion.LookRotation(Vector3.zero- MainStarTransforms[1].transform.position);
         //   transform.rotation = Quaternion.LookRotation(Vector3.zero - MainStarTransforms[1].transform.position);
            // active[shoot] = true;
            LightAttack.attack = true;

         /*   GameObject newStar = Instantiate(attackStar, MainStarTransforms[1].position, MainStarTransforms[1].rotation);
            newStar.GetComponent<Renderer>().material.color = MainStarTransforms[1].gameObject.GetComponent<Renderer>().material.color;
            newStar.GetComponent<Light>().color = MainStarTransforms[1].gameObject.GetComponent<Renderer>().material.color;
            MainStarTransforms[1].gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1);

            newStar.tag = "Player2Shooter";
            newStar.GetComponent<ShootingStar>().setDirection(1);*/
            findNewState = true;
        }
        else if (aiState == AIStates.Grow)
        {

          //  Debug.Log("AIG");
           

            if (movingState == MovingStates.Open)
            {
                movingState = MovingStates.Move;
                transform.LookAt(growTarget);
                oldDistanceToDestination = Vector3.Distance(growTarget, transform.position);
                distanceToDestination = oldDistanceToDestination;
            }
            if (movingState == MovingStates.Move)
            {
                if (!atGrowDestination())
                {
                    //   Debug.Log("Moving not at destination");
                    transform.position += transform.forward * 6.0f * Time.deltaTime;

                }

            }





        }
        else if (aiState == AIStates.Follow)
        {

            //  Debug.Log("AISF");

            if (Player1.GetComponent<LightRegulator>().health > 0)
                { 
            targetDestination = Player1.transform.position;

                if (movingState == MovingStates.Open)
                {

                    movingState = MovingStates.Move;
                    transform.LookAt(Player1.transform.position);
                    oldDistanceToDestination = Vector3.Distance(Player1.transform.position, transform.position);
                    distanceToDestination = oldDistanceToDestination;
                }
                if (movingState == MovingStates.Move)
                {
                    if (!atDestination())
                    {
                        //   Debug.Log("Moving not at destination");
                        transform.position += transform.forward * speed * Time.deltaTime;

                    }

                }
            }


        }

    }

    bool atGrowDestination()
    {
        //        Vector3 oldDistanceTodestination;
        oldDistanceToDestination = distanceToDestination;
        distanceToDestination = Vector3.Distance(transform.position, growTarget);
        //  if (Vector3.Distance(transform.position, targetDestination) < 1.0f)
        //   Debug.Log("Distancer");
        //if (oldDistanceToDestination < distanceToDestination + 0.10f)
        //    Debug.Log("Old");
        if (Vector3.Distance(transform.position, growTarget) < 1.0f)                 //|| oldDistanceToDestination < distanceToDestination)
        {

            movingState = MovingStates.Open;

            oldDistanceToDestination = 0;

            findNewState = true;
            return true;
        }
        return false;


    }
    bool atDestination()
    {
        //        Vector3 oldDistanceTodestination;
        oldDistanceToDestination = distanceToDestination;
        distanceToDestination = Vector3.Distance(transform.position, targetDestination);
        transform.LookAt(targetDestination);
        //  if (Vector3.Distance(transform.position, targetDestination) < 1.0f)
        //   Debug.Log("Distancer");
        //if (oldDistanceToDestination < distanceToDestination + 0.10f)
        //    Debug.Log("Old");
        if (Vector3.Distance(transform.position, targetDestination) < 5.0f)                 //|| oldDistanceToDestination < distanceToDestination)
        {
            findNewState = true;
           
            movingState = MovingStates.Open;
            oldDistanceToDestination = 0;
            return true;
        }
        return false;
    }
}
