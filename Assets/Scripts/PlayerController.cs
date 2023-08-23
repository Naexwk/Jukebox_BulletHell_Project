
using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    public float playerSpeed;
    public float bulletSpeed;
    public Rigidbody2D rig;
    public Rigidbody2D playerBullet;
    Vector2 rb;
    private Camera _mainCamera;
    private ulong playerNumber;
    private GameObject outline;
    private float timeSinceLastFire;
    public int fireRate;

    void Start()
    {
        _mainCamera = Camera.main;
        playerNumber = gameObject.GetComponent<NetworkObject>().NetworkObjectId;
        outline = gameObject.transform.GetChild(0).gameObject;


        // Change outline
        if (playerNumber == 1) {
            outline.GetComponent<Renderer>().material.color = Color.red;
        }
        if (playerNumber == 2) {
            outline.GetComponent<Renderer>().material.color = Color.blue;
        }
        if (playerNumber == 3) {
            outline.GetComponent<Renderer>().material.color = Color.yellow;
        }
        if (playerNumber == 4) {
            outline.GetComponent<Renderer>().material.color = Color.green;
        }

    }
    void Update()
    {
        if (!IsOwner) {
            return;
        }

        if (Input.GetButton("Fire1"))
        {
            if ((Time.time - timeSinceLastFire) > (1f/fireRate)) {
                Debug.Log((Time.time - timeSinceLastFire));
                Rigidbody2D clone;
                Vector3 worldMousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = worldMousePos - transform.position;
                direction.Normalize();
                clone = Instantiate(playerBullet, transform.position, transform.rotation);
                clone.GetComponent<PlayerBullet>().bulletDamage = 3;
                clone.GetComponent<PlayerBullet>().bulletSpeed = 30f;
                clone.velocity = (direction) * (bulletSpeed);
                // Change bullet color
                if (playerNumber == 1) {
                    clone.GetComponent<Renderer>().material.color = Color.red;
                }
                if (playerNumber == 2) {
                    clone.GetComponent<Renderer>().material.color = Color.blue;
                }
                if (playerNumber == 3) {
                    clone.GetComponent<Renderer>().material.color = Color.yellow;
                }
                if (playerNumber == 4) {
                    clone.GetComponent<Renderer>().material.color = Color.green;
                }

                // Update timeSinceLastFire
                timeSinceLastFire = Time.time;
            }
            
        }
    }


    private void FixedUpdate(){

        if (!IsOwner) {
            return;
        }
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        if (xInput == 0 || yInput == 0) {
            rig.velocity = new Vector2(xInput * playerSpeed, yInput * playerSpeed);
        } else {
            rig.velocity = new Vector2(xInput * playerSpeed * 0.707f, yInput * playerSpeed* 0.707f);
        }
    }
}
