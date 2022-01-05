using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRegulator : MonoBehaviour
{

    public GameObject SpaceStation1;
    public GameObject Teleport;
    public GameObject TurboBoost;
    public GameObject SmokeTrail;
    public GameObject MissileParticleSystem;
    public GameObject LaserParticleSystem;
    public ParticleSystem[] MissileParticleSystems;
    public ParticleSystem[] LaserParticleSystems;
    public GameObject Bomb;
    public List<GameObject> bombs;

    public GameObject instancedMissiles;

    public static bool isTeleport;
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

    public int health;
    public static int speed;

    public static List<Rank> ranks;
  

    public GameObject FollowLights;
    //public static Vector3 Direction;
    public GameObject attackStar;
    public static Transform cTransform;
    public int counter;
    public enum LightPositions { One, Two, Three, Four, Five}
    public static LightPositions lightPosition;
    public enum States { AttackState, MoveState}

    public States state;

    public ArrayList DistanceToPositions;

    Ray camRay;
    LayerMask floor;
    Vector3 targetDestination;
    float distanceToDestination;
    float oldDistanceToDestination;

    public static int shoot;
    public static List<bool> active;

    enum MovingStates { Open, Aim, Move }
    MovingStates movingState;
    ParticleSystem missile1;
    float Timer1ForMissile1;

    public void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("SM_Ship_Cruiser_02"))
            Camera.main.GetComponent<CameraFollow>().mode = 1;
    }

    public void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.gameObject.name.Contains("SM_Ship_Cruiser_02"))
            Camera.main.GetComponent<CameraFollow>().mode = 1;
    }
    // public static 
    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        isTeleport = false;
        bombs = new List<GameObject>();
        ranks = new List<Rank>();

        ranks.Add(new Rank(5));

        speed = 160;

        //  var emission = WeaponParticle1.
        LaserParticleSystems = LaserParticleSystem.GetComponentsInChildren<ParticleSystem>();

        
         MissileParticleSystems = MissileParticleSystem.GetComponentsInChildren<ParticleSystem>();
        //  WeaponParticle1.GetComponent<ParticleSystem>().Stop();

        foreach (ParticleSystem ps in MissileParticleSystems)
            ps.Stop();
        TurboBoost.GetComponent<ParticleSystem>().Stop();


        // gameObject.GetComponent<ParticleSystem>
      //  missile1 = WeaponParticle1.GetComponent<ParticleSystem>();


        shoot = -1  ;
        DistanceToPositions = new ArrayList(5);
        //LightPositions
        lightPosition = LightPositions.One;
        counter = 0;
        active = new List<bool>();
        active.Add(false);
        active.Add(false);
        active.Add(false);
        active.Add(false);
        active.Add(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (health < 0)
            Destroy(this.gameObject);
      //  Debug.Log("Regulated");
        cTransform = transform;
        shoot = getClosestLight();

        if (Timer1ForMissile1 >= 0)
            Timer1ForMissile1 -= Time.deltaTime;
        else
        {
           // foreach (ParticleSystem ps in MissileParticleSystems)
             //   ps.Stop();

            if(instancedMissiles)
            foreach (ParticleSystem ps in instancedMissiles.GetComponentsInChildren<ParticleSystem>())
                ps.Stop();
            //WeaponParticle1.GetComponent<ParticleSystem>().Stop();
            //var emission = missile1.emission;
            //missile1.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        //Debug.Log(shoot);

       // Debug.Log("Working");
     //   Debug.Log(Input.GetKey(KeyCode.LeftShift));

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
           bombs[0].GetComponent<Bomb>().blowUp=true;


        }
        if(Input.GetKeyDown(KeyCode.CapsLock))
        {
            Transform[] MainStarTransforms = GetComponentsInChildren<Transform>();

            state = States.AttackState;
            // active[shoot] = true;
            LightAttack.attack = true;

            GameObject newBomb = Instantiate(Bomb, MainStarTransforms[1].position, MainStarTransforms[1].rotation);
            newBomb.GetComponent<Renderer>().material.color = MainStarTransforms[1].gameObject.GetComponent<Renderer>().material.color;
            newBomb.GetComponent<Light>().color = MainStarTransforms[1].gameObject.GetComponent<Renderer>().material.color;
           // MainStarTransforms[1].gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
            newBomb.GetComponent<Bomb>().setDirection(0);
            newBomb.tag = "Player1Bomb";
            bombs.Add(newBomb);


        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 40;
            Debug.Log(speed);

            TurboBoost.GetComponent<ParticleSystem>().Play();
            SmokeTrail.GetComponent<ParticleSystem>().Stop();

        }
        else
        {
            speed = 20;
            TurboBoost.GetComponent<ParticleSystem>().Stop();
            SmokeTrail.GetComponent<ParticleSystem>().Play();
        }
            if (Input.GetKeyDown(KeyCode.Tab))
        {

            Transform[] transforms = FollowLights.GetComponentsInChildren<Transform>();

            //int rank = transforms.Length / shoot;

            Transform[] MainStarTransforms = GetComponentsInChildren<Transform>();
            try
            {
                if (ranks[0].occupied[shoot])
                {
                    MainStarTransforms[1].gameObject.GetComponent<Renderer>().material.color = transforms[shoot + 6].gameObject.GetComponent<Renderer>().material.color;
                 //   MainStarTransforms[1].gameObject.GetComponent<Light>().color = transforms[shoot + 6].gameObject.GetComponent<Renderer>().material.color;

                    transforms[shoot + 6].gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1);

                    transforms[shoot + 6].gameObject.GetComponent<Renderer>().enabled = false;

                }
            }
            catch(System.IndexOutOfRangeException ioure)
            {
                Debug.Log(shoot);
                Debug.Log(transforms.Length);

            }

        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            isTeleport = true;
            /*
            Transform[] MainStarTransforms = GetComponentsInChildren<Transform>();

            state = States.AttackState;
            // active[shoot] = true;
            LightAttack.attack = true;

            GameObject newStar = Instantiate(attackStar, MainStarTransforms[1].position, MainStarTransforms[1].rotation);
            newStar.GetComponent<Renderer>().material.color = MainStarTransforms[1].gameObject.GetComponent<Renderer>().material.color;
            newStar.GetComponent<Light>().color = MainStarTransforms[1].gameObject.GetComponent<Renderer>().material.color;
            MainStarTransforms[1].gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
            newStar.GetComponent<ShootingStar>().setDirection(0);
            newStar.tag = "Player1Shooter";
            */
        }
        if(Input.GetKeyDown(KeyCode.E))
        {
            state = States.MoveState;

        }


       // fakeTransform = transform;
        //   if(movingState == MovingStates.Aim)
        {

            //     transform.LookAt(targetDestination);
            //      movingState = MovingStates.Move;
        }
        if (movingState == MovingStates.Move)
        {
            if (!atDestination())
            {
               
                transform.position += transform.forward * 6.0f * Time.deltaTime;

            }

        }


        if (Input.GetMouseButton(0))
        {

            camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit floorHit;
            if (Physics.Raycast(camRay, out floorHit, 10000, floor))
            {
                movingState = MovingStates.Move;

                
                    
                targetDestination = floorHit.point + new Vector3(0.0f, 2.0f, 0.0f);
                transform.LookAt(targetDestination);
                if (isTeleport)
                {

                    Instantiate(Teleport, transform.position, Quaternion.identity);
                    transform.position = targetDestination;
                    Instantiate(Teleport, transform.position, Quaternion.identity);
                    isTeleport = false;
                }
                

                    oldDistanceToDestination = Vector3.Distance(targetDestination, transform.position);
                distanceToDestination = oldDistanceToDestination;
               
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
         //   missile1.Play(true);
            Timer1ForMissile1 = 1;

            instancedMissiles =Instantiate(MissileParticleSystem, transform.position, transform.rotation);
            foreach (ParticleSystem ps in instancedMissiles.GetComponentsInChildren<ParticleSystem>())
                ps.Play();
          //  foreach (ParticleSystem ps in LaserParticleSystems)
           //     ps.Stop();

            camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit floorHit;

            if (Physics.Raycast(camRay, out floorHit, 10000, floor))
            {
                transform.LookAt(floorHit.point);

            
            }

        }
    }



   int getClosestLight()
    {


       // Debug.Log(counter++);

       // Debug.Log(LightMovement.Rotation.eulerAngles.y);
       // Debug.Log(RotateStar.Rotation.eulerAngles.y);

        Vector3 lmRot = new Vector3(LightMovement.Rotation.eulerAngles.x, Mathf.Round(LightMovement.Rotation.eulerAngles.y), LightMovement.Rotation.eulerAngles.z);
        Vector3 rsRot = new Vector3(RotateStar.Rotation.eulerAngles.x, Mathf.Round(RotateStar.Rotation.eulerAngles.y), RotateStar.Rotation.eulerAngles.z);
        float difference = Vector3.Distance((rsRot),(lmRot));

       // Debug.Log(difference);
      //  Debug.Log("Break");
        float totalDifference = (Mathf.Round(difference));

      //  Debug.Log(totalDifference);
        
        if (totalDifference > (36) && totalDifference <= (108))
            return 4;
        else if (totalDifference > (108) && totalDifference <= (180))
            return 3;
        else if (totalDifference > (180) && totalDifference <= (252))
            return 2;
        else if (totalDifference > (252) && totalDifference <= (324))
            return 1;
        else 
            return 0;


        
        // if(difference > )
       


        //Debug.Log(gameObject.name);
       // Debug.Log(gameObject.transform.eulerAngles);
        //Debug.Log(RotateStar.rotation.eulerAngles.ToString());
       

    }
    

    bool atDestination()
    {
        //        Vector3 oldDistanceTodestination;
        oldDistanceToDestination = distanceToDestination;
        distanceToDestination = Vector3.Distance(transform.position, targetDestination);

        if (Vector3.Distance(transform.position, targetDestination) < 1.0f)//|| oldDistanceToDestination < distanceToDestination)
        {
            movingState = MovingStates.Open;
            oldDistanceToDestination = 0;
            return true;
        }
        return false;
    }
}
