using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class OrbitalGravity : MonoBehaviour {
    //TODO find ground, when on ground add the ground's forward to velocity
    public Transform planet;
    public Transform foot;
    public float speed = 5f;
    public float turnSpeed = 15f;
    public float jumpForce = 20f;
    public float gravityForce = 10f;
    
    private Rigidbody rigiBod;

    private Vector3 gravity;
    private bool jumping;
    
	void Start () {
        rigiBod = GetComponent<Rigidbody>();
        gravity = planet.transform.position - transform.position;
        gravity.Normalize();
    }
	
	void FixedUpdate () {
        Vector3 velocity = new Vector3();
        Orbiter orbiter = null;
        bool grounded = false;

        Collider[] colliders = Physics.OverlapSphere(foot.position, 0.01f);
        for(int i=0; i<colliders.Length; i++)
        {
            grounded = true;
            orbiter = colliders[i].transform.root.GetComponent<Orbiter>();
            if(orbiter != null)
            {
                break;
            }
        }

        Vector3 targetUp = transform.up;

        if(orbiter != null)
        {
            targetUp = orbiter.transform.up;
        }
        else
        {
            Vector3 toPlanet = planet.transform.position - transform.position;
            toPlanet.Normalize();
            targetUp = -toPlanet;
        }
        if (jumping)
        {
            Vector3 jumpDecay = -targetUp * Time.deltaTime * gravityForce * 2;
            gravity += jumpDecay;
            if (gravity.magnitude > speed){
                gravity = -targetUp * gravityForce;
                jumping = false;
            }
        }else
        {
            gravity = -targetUp * gravityForce;
        }

        if(grounded && Input.GetKeyDown(KeyCode.Space))
        {
            jumping = true;
            gravity = targetUp * jumpForce;
        }

        if (orbiter != null)
        {
            Vector3 orbiterVelocity = orbiter.transform.forward * orbiter.speed * 0.5f * Time.deltaTime;
            orbiterVelocity = transform.InverseTransformVector(orbiterVelocity);
            
            transform.Translate(orbiterVelocity);
        }

        Vector3 targetForward = HandleOrientationInput(targetUp);
        targetForward = AlignForwardToUp(targetForward, targetUp);
        transform.rotation = Quaternion.LookRotation(targetForward, targetUp);

        velocity += gravity;
        velocity += HandleVelocityInput();

        rigiBod.velocity = velocity;
    }

    private Vector3 HandleVelocityInput()
    {
        Vector3 InputVelocity = new Vector3();

        if (Input.GetKey(KeyCode.W))
        {
            InputVelocity += transform.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            InputVelocity -= transform.forward;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            InputVelocity -= transform.right;
        }

        if (Input.GetKey(KeyCode.E))
        {
            InputVelocity += transform.right;
        }

        return InputVelocity * speed;
    }

    private Vector3 HandleOrientationInput(Vector3 up)
    {
        Vector3 rotate = transform.forward;

        if (Input.GetKey(KeyCode.A))
        {
            rotate = Quaternion.AngleAxis(-turnSpeed * Time.deltaTime, up) * rotate;
        }

        if (Input.GetKey(KeyCode.D))
        {
            rotate = Quaternion.AngleAxis(turnSpeed * Time.deltaTime, up) * rotate;
        }

        return rotate;
    }

    private Vector3 AlignForwardToUp(Vector3 forward, Vector3 up)
    {
        Vector3 right = Vector3.Cross(forward, up);
        return Vector3.Cross(up, right);
    }
}
