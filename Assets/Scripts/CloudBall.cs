using UnityEngine;
using System.Collections;

public class CloudBall : MonoBehaviour {
    public Transform planet;
    public float gravityForce = 5f;

    private Rigidbody rigiBod;
    private Vector3 velocity;

    void Start () {
        rigiBod = GetComponent<Rigidbody>();
        rigiBod.velocity = velocity;
	}
	
	void FixedUpdate () {
	    if(rigiBod != null && planet != null)
        {
            Vector3 gravityDirection = planet.transform.position - transform.position;
            gravityDirection.Normalize();

            rigiBod.velocity += gravityDirection * gravityForce * Time.deltaTime;
        }
	}

    public void SetVelocity(Vector3 velocity)
    {
        this.velocity = velocity;
        if(rigiBod != null)
        {
            rigiBod.velocity = velocity;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.root.tag == "CloudSeed")
        {
            transform.parent = other.transform.root;
            Destroy(rigiBod);
            SphereCollider collider = GetComponent<SphereCollider>();
            if(collider != null)
            {
                collider.isTrigger = false;
                collider.radius = 0.5f;
            }
            rigiBod = null;
        }
        else if (other.transform.root.tag == "Planet")
        {
            Destroy(transform.gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        
    }
}
