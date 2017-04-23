using UnityEngine;
using System.Collections;

public class CloudBall : MonoBehaviour {
    public Transform planet;
    public float gravityForce = 5f;
    public float rainRate = 1f;
    public GameObject rainSystem;

    private Rigidbody rigiBod;
    private Vector3 velocity;
    private bool raining = false;
    private bool settled = false;
    private bool spreadExhausted = false;
    private float rainElapsed = 0f;

    void Start () {
        rigiBod = GetComponent<Rigidbody>();
        rigiBod.velocity = velocity;
        rainSystem.SetActive(false);
	}
	
	void FixedUpdate () {
	    if(planet != null)
        {
            Vector3 gravityDirection = planet.transform.position - transform.position;
            gravityDirection.Normalize();

            if(rigiBod != null)
            {
                rigiBod.velocity += gravityDirection * gravityForce * Time.deltaTime;
            }

            transform.up = -gravityDirection;
        }

        if(raining)
        {
            if (spreadExhausted)
            {
                float scale = transform.localScale.x - rainRate * Time.deltaTime;
                if(scale > 0.1)
                {
                    transform.localScale = new Vector3(scale, scale, scale);
                }
                else
                {
                    Destroy(transform.gameObject);
                }
            }
            else
            {
                rainElapsed += Time.deltaTime;
                if (rainElapsed >= rainRate)
                {
                    rainElapsed = 0f;
                    bool spread = false;
                    Collider[] colliders = Physics.OverlapSphere(transform.position, transform.localScale.x * 0.5f);
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        CloudBall target = colliders[i].GetComponent<CloudBall>();
                        if (target != null && !target.isRaining())
                        {
                            spread = true;
                            target.Rain();
                            break;
                        }
                    }

                    spreadExhausted = !spread;
                }
            }
        }
	}

    public void Rain()
    {
        raining = true;
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        if(renderer != null)
        {
            renderer.material.color = Color.gray;
        }

        if (settled)
        {
            rainSystem.SetActive(true);
        }
        else
        {
            ParticleSystem cloudParticles = GetComponent<ParticleSystem>();
            if (cloudParticles != null)
            {
                cloudParticles.startColor = Color.blue;
            }
        }
    }

    public bool isRaining()
    {
        return raining;
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
            settled = true;
            ParticleSystem cloudParticles = GetComponent<ParticleSystem>();
            if (cloudParticles != null)
            {
                cloudParticles.Stop();
            }

            if (raining)
            {
                rainSystem.SetActive(true);
            }
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
