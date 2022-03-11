using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Missile : MonoBehaviour
{
    [SerializeField]
    float speed = 11f;
    [SerializeField]
    float turningSpeed = 1f;

    Vector3 direction;

    public GameObject target;

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

        rb = GetComponent<Rigidbody>();
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
        rb.velocity = this.transform.forward * (speed);

    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
