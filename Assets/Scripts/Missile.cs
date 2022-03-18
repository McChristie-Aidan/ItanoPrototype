using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Missile : MonoBehaviour
{
    [SerializeField]
    float speed = 11f;
    [SerializeField]
    float speedDampening = .1f;
    [SerializeField]
    float turningSpeed = 1f;
    [SerializeField]
    float separationDistance = 4f;

    [SerializeField]
    float maxForce;
    float angleModifier = .2f;

    [SerializeField]
    GameObject explosionPrefab;

    Vector3 direction;
    Vector3 avoidDir;
    Vector3 obstaclePos;
    Ray ray;
    RaycastHit hit;
    public LayerMask mask;
    public GameObject target;
    PlayerFlightControls pfc;

    Rigidbody rb;
    bool changeDir;

    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
            if (target == null)
            {
                Debug.Log("Missile has no target");
                Destroy(this);
            }
        }

        pfc = target.GetComponent<PlayerFlightControls>();
        rb = GetComponent<Rigidbody>();

        MissileManager.Instance.AddMissile(this);
    }

    // Update is called once per frame
    void Update()
    {
        DetectObstacle();
        if (changeDir)
        {
            if (obstaclePos != null)
            {
                var heading = obstaclePos - this.transform.position;
                float dot = Vector3.Dot(heading, this.transform.forward);
                if (dot <= 0)
                    changeDir = false;
                return;
            }
            return;
        }
        Debug.Log("H");
        direction = target.transform.position - this.transform.position;

        this.transform.rotation = Quaternion.Slerp(
            this.transform.rotation, 
            Quaternion.LookRotation(direction), 
            Time.deltaTime * turningSpeed);

        //this.transform.Translate(0, 0, speed * Time.deltaTime);
        if (pfc != null)
        {
            float dist = Vector3.Distance(target.transform.position, this.transform.position);
            speed = pfc.ActiveForwardSpeed + dist - speedDampening;
        }

        rb.velocity = this.transform.forward * (speed);

        //rb.AddForce(this.transform.forward * speed);
        //rb.AddForce(Separation());
    }

    private void LateUpdate()
    {
        //rb.velocity = Vector3.zero;
    }
    public Vector3 Separation()
    {
        Vector3 steer = Vector3.zero;
        int count = 0;

        foreach (Missile m in MissileManager.Instance.activeMissiles)
        {
            float dist = Vector3.Distance(m.transform.position, this.transform.position);

            if (dist > 0  && dist < separationDistance)
            {
                Vector3 diff = this.transform.position - m.transform.position;
                diff.Normalize();
                diff = diff / dist;
                steer += diff;
                count++;
            }
        }

        if (count > 0)
        {
            steer = steer / count;
        }

        if (steer.magnitude > 0)
        {
            steer.Normalize();
            steer *= speed;
            steer -= rb.velocity;
            //limit steer by max force
        }
        
        return steer;
    }

    private void OnCollisionEnter(Collision collision)
    {
        MissileManager.Instance.RemoveMissile(this);
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, this.transform.position, this.transform.rotation);
        }
        Destroy(this.gameObject);
    }

    

    void DetectObstacle()
    {
        if(Physics.Raycast(this.transform.position, transform.forward, out hit, 50f, mask))
        {
            RayCast();
            obstaclePos = hit.transform.position;
            changeDir = true;
            return;
        }
    }

    void AvoidManeuver()
    {

    }

    bool FindDirectionOfNoObstacle(Vector3 dir, Ray ray)
    {
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 50f, Color.red);
        if (!Physics.Raycast(ray, 50f))
        {
            rb.velocity = dir * speed;
            changeDir = false;
            return true;
        }
        return false;
    }

    void RayCast()
    {
        for (int i = 1; i < 20; i++)
        {
            avoidDir = (transform.forward + new Vector3(angleModifier * i, 0, 0)).normalized;
            if (FindDirectionOfNoObstacle(avoidDir, new Ray(transform.position, avoidDir)))
                return;

            avoidDir = (transform.forward - new Vector3(angleModifier * i, 0, 0)).normalized;
            if (FindDirectionOfNoObstacle(avoidDir, new Ray(transform.position, avoidDir)))
                return;

            avoidDir = (transform.forward - new Vector3(0, angleModifier * i, 0)).normalized;
            if (FindDirectionOfNoObstacle(avoidDir, new Ray(transform.position, avoidDir)))
                return;

            avoidDir = (transform.forward + new Vector3(0, angleModifier * i, 0)).normalized;
            if (FindDirectionOfNoObstacle(avoidDir, new Ray(transform.position, avoidDir)))
                return;


            avoidDir = (transform.forward + new Vector3(angleModifier * i, angleModifier * i, 0)).normalized;
            if (FindDirectionOfNoObstacle(avoidDir, new Ray(transform.position, avoidDir)))
                return;

            avoidDir = (transform.forward - new Vector3(angleModifier * i, angleModifier * i, 0)).normalized;
            if (FindDirectionOfNoObstacle(avoidDir, new Ray(transform.position, avoidDir)))
                return;

            avoidDir = (transform.forward + new Vector3(-angleModifier * i, angleModifier * i, 0)).normalized;
            if (FindDirectionOfNoObstacle(avoidDir, new Ray(transform.position, avoidDir)))
                return;

            avoidDir = (transform.forward - new Vector3(-angleModifier * i, angleModifier * i, 0)).normalized;
            if (FindDirectionOfNoObstacle(avoidDir, new Ray(transform.position, avoidDir)))
                return;
        }

    }
}
