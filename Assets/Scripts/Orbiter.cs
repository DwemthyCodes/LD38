using UnityEngine;
using System.Collections;

public class Orbiter : MonoBehaviour {
    public Transform planet;
    public float radius = 35;
    public float speed = 3f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(planet != null)
        {
            //Move forward
            Vector3 forward = transform.forward;
            Vector3 destination = transform.position + (forward.normalized * speed * Time.deltaTime);

            //Readjust to maintain radius
            Vector3 planetDiff = destination - planet.transform.position;
            destination = (planetDiff.normalized * radius) + planet.transform.position;

            transform.position = destination;

            //Now readjust the rotation
            //transform.up = planetDiff.normalized;
            forward = AlignForwardToUp(forward, planetDiff.normalized);
            transform.rotation = Quaternion.LookRotation(forward, planetDiff.normalized);
        }
    }


    private Vector3 AlignForwardToUp(Vector3 forward, Vector3 up)
    {
        Vector3 right = Vector3.Cross(forward, up);
        return Vector3.Cross(up, right);
    }
}
