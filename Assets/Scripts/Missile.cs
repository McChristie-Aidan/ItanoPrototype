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

    Vector3 direction;

    public GameObject target;
    PlayerFlightControls pfc;

    Rigidbody rb;

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
        Destroy(this.gameObject);
    }
}
