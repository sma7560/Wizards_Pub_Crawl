using UnityEngine;
using UnityEngine.Networking;

public class OfflineHeroController : NetworkBehaviour
{
    public GameObject attackerUI;
    public Vector3 speed;
    private float moveSpeed;
    private Rigidbody heroRigidbody;

    private Camera mainCam;
    private Plane ground;
    private float rayLength;
    private Vector3 pointToLookAt;

    // For getting stats for the characters.
    private HeroModel heroModel;
    private BasicAttack basicAttackController;
    private TestAnimConrtoller animController;

    // Use this for initialization
    void Start()
    {
        if (!isLocalPlayer) return;
        moveSpeed = 5.0f;

        Debug.Log("Setting up rigid");
        heroRigidbody = GetComponent<Rigidbody>();

        Debug.Log("Setting up stats");
        basicAttackController = GetComponent<BasicAttack>();
        //heroManager = GetComponent<NetworkHeroManager>();
        animController = GetComponent<TestAnimConrtoller>();

        // Setting up heroManager (Network) This is temporary for the test.
        CmdSetupStats();
        // Setting up basic attack
        basicAttackController.CmdSetAttackParameters();
        animController.myHeroType = HeroType.magic; // For now hard coding this in.

        //StartUI();

        // Camera stuff
        Camera.main.GetComponent<OffHeroCam>().SetTarget(transform);
        mainCam = Camera.main.GetComponent<Camera>();
        ground = new Plane(Vector3.up, Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;
        CharacterMovement();
        SetRotation();
        if (Input.GetMouseButtonDown(0))
        {
            basicAttackController.PerformAttack();
            animController.PlayBasicAttack();
        }
        //Debug.Log(heroRigidbody.velocity);
        Debug.DrawLine(transform.position, transform.forward * 20 + transform.position, Color.red);
    }

    [Command]
    private void CmdSetupStats()
    {
        heroModel = GetComponent<HeroModel>(); // Setting it up on the server...
        int val = 10;
        heroModel.SetAtkSpeed(1);
        heroModel.SetHeroType(HeroType.magic);
        heroModel.SetMAttack(val);
        heroModel.SetMDefence(val);
        heroModel.SetPAttack(val);
        heroModel.SetPDefence(val);
    }

    private void SetRotation()
    {
        Ray cameraRay = mainCam.ScreenPointToRay(Input.mousePosition);
        if (ground.Raycast(cameraRay, out rayLength))
        {
            pointToLookAt = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLookAt, Color.blue);
            transform.LookAt(new Vector3(pointToLookAt.x, transform.position.y, pointToLookAt.z));
        }
    }

    //[Command]
    //private void CmdSetrota
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
