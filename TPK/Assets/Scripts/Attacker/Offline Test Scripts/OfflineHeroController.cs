using UnityEngine;

public class OfflineHeroController : MonoBehaviour {
    public GameObject attackerUI;
    public Vector3 speed;
    private float moveSpeed;
    private Rigidbody heroRigidbody;

    private Camera mainCam;
    private Plane ground;
    private float rayLength;
    private Vector3 pointToLookAt;
    // Use this for initialization
    void Start () {
        moveSpeed = 10.0f;

        Debug.Log("Setting up rigid");
        heroRigidbody = GetComponent<Rigidbody>();
        //StartUI();

        // Camera stuff
        Camera.main.GetComponent<HeroCameraController>().setTarget(transform);
        mainCam = Camera.main.GetComponent<Camera>();
        ground = new Plane(Vector3.up, Vector3.zero);
    }
	
	// Update is called once per frame
	void Update () {
        CharacterMovement();
        SetRotation();
        //Debug.Log(heroRigidbody.velocity);
        Debug.DrawLine(transform.position, transform.forward * 20 + transform.position, Color.red);


    }
    private void SetRotation() {
        Ray cameraRay = mainCam.ScreenPointToRay(Input.mousePosition);
        if (ground.Raycast(cameraRay, out rayLength)) {
            pointToLookAt = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLookAt, Color.blue);
            transform.LookAt(new Vector3(pointToLookAt.x, transform.position.y, pointToLookAt.z));
        }
    }
    private void CharacterMovement()
    {
        //Debug.Log("Horizontal: " + Input.GetAxis("Horizontal"));
        //Debug.Log("Vertical: " + Input.GetAxis("Vertical"));
        //speed = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, 0, Input.GetAxis("Vertical") * moveSpeed);
        //heroRigidbody.velocity = speed;
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Debug.Log("Running Velocity");
            heroRigidbody.velocity = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, 0, Input.GetAxis("Vertical") * moveSpeed);
        }
        else
        {
            heroRigidbody.velocity = new Vector3(0, 0, 0);
        }
    }
}
