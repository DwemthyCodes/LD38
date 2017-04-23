using UnityEngine;
using System.Collections;

public class CloudLauncher : MonoBehaviour {
    public CloudBall cloudPrefab;
    public Transform spawnPoint;
    public float launchForce = 10f;
    public float chargeRate = 0.1f;
    public float chargeMax = 2f;
    public LayerMask aimMask;
    public LayerMask shotMask;
    public LayerMask cloudMask;
    public Transform leftArm;
    public Transform rightArm;

    private Transform planet;
    private Vector3 spawnHome;
    private CloudBall currentShot;
    float dist = 100f;

    private void Start()
    {
        spawnHome = transform.TransformVector(spawnPoint.position);
        planet = GetComponent<OrbitalGravity>().planet;
    }

    private Vector3 CalcSpawnPosition()
    {
        return spawnPoint.position;
    }

    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            currentShot = Instantiate(cloudPrefab);
            currentShot.transform.position = CalcSpawnPosition();
            currentShot.gameObject.layer = 9;

            if (rightArm != null)
            {
                rightArm.Translate(0f, 0.5f, 0f);
                rightArm.Rotate(0f, 0f, -15f);
            }

            if(leftArm != null)
            {
                leftArm.Translate(0f, 0.5f, 0f);
                leftArm.Rotate(0f, 0f, 15f);
            }
        }
        else if (Input.GetMouseButtonUp(0) && currentShot != null)
        {
            Vector3 aim;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, dist, aimMask))
            {
                aim = hit.point;
            }
            else
            {
                aim = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10 * dist));
            }

            Vector3 launchV = aim - currentShot.transform.position;
            launchV.Normalize();
            launchV *= Mathf.Max(launchForce, launchForce * currentShot.transform.localScale.x);
            currentShot.SetVelocity(launchV);
            currentShot.planet = planet;
            currentShot.gameObject.layer = 8;
            currentShot = null;


            if (rightArm != null)
            {
                rightArm.Rotate(0f, 0f, 15f);
                rightArm.Translate(0f, -0.5f, 0f);
            }

            if (leftArm != null)
            {
                leftArm.Rotate(0f, 0f, -15f);
                leftArm.Translate(0f, -0.5f, 0f);
            }
        }
        else if (Input.GetMouseButton(0) && currentShot != null)
        {
            float currentScale = currentShot.transform.localScale.x;
            if(currentScale < chargeMax)
            {
                float newScale = currentScale + (chargeRate * Time.deltaTime);
                currentShot.transform.localScale = new Vector3(newScale, newScale, newScale);
            }

            currentShot.transform.position = CalcSpawnPosition();
        }

        if (Input.GetMouseButtonUp(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, dist, aimMask))
            {
                if(hit.transform != null)
                {
                    CloudBall ball = hit.transform.gameObject.GetComponent<CloudBall>();
                    if(ball != null)
                    {
                        Destroy(ball.gameObject);
                    }
                }
            }
        }
	}
}
