
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


    public GameObject prefabCrap;
    public GameObject prefabBullet;

    void colorCodeToPlayer (GameObject go, ulong playerNumber) {
        if (playerNumber == 0) {
            go.GetComponent<Renderer>().material.color = Color.red;
        }
        if (playerNumber == 1) {
            go.GetComponent<Renderer>().material.color = Color.blue;
        }
        if (playerNumber == 2) {
            go.GetComponent<Renderer>().material.color = Color.yellow;
        }
        if (playerNumber == 3) {
            go.GetComponent<Renderer>().material.color = Color.green;
        }
    }

    void Start()
    {
        _mainCamera = Camera.main;
        playerNumber = gameObject.GetComponent<NetworkObject>().OwnerClientId;
        outline = gameObject.transform.GetChild(0).gameObject;


        // Change outline
        colorCodeToPlayer(outline, playerNumber);

    }

    void Update()
    {
        if (!IsOwner) {
            return;
        }

        if (Input.GetButton("Fire1"))
        {

            if ((Time.time - timeSinceLastFire) > (1f/fireRate)) {
                //Rigidbody2D clone;
                GameObject clone;
                Vector3 worldMousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = worldMousePos - transform.position;
                direction.Normalize();
                clone = Instantiate(prefabBullet, transform.position, transform.rotation);
                if (IsServer) {
                    clone.GetComponent<NetworkObject>().Spawn();
                }
                /*Debug.Log("1: " + clone.gameObject.GetComponent<NetworkObject>().IsSpawned);
                if (IsServer) {
                    clone.gameObject.GetComponent<NetworkObject>().Spawn();
                    Debug.Log("2: " + clone.gameObject.GetComponent<NetworkObject>().IsSpawned);
                }
                Debug.Log("3: " + clone.gameObject.GetComponent<NetworkObject>().IsSpawned);*/
                
                clone.GetComponent<PlayerBullet>().bulletDamage = 3;
                clone.GetComponent<PlayerBullet>().bulletSpeed = 3f;
                clone.GetComponent<Rigidbody2D>().velocity = (direction) * (bulletSpeed);
                
                colorCodeToPlayer(clone.gameObject, playerNumber);

                // Update timeSinceLastFire
                timeSinceLastFire = Time.time;
            }
            
        }

        if (Input.GetKeyDown("f")){
            if (IsServer) {
                GameObject crap;
                crap = Instantiate(prefabCrap, transform.position, transform.rotation);
                crap.GetComponent<Renderer>().material.color = Color.red;
                crap.GetComponent<NetworkObject>().SpawnWithOwnership(playerNumber);
            } else {
                Debug.Log("Called");
                spawnCrapServerRpc();
            }
            //spawnCrapClientRpc();
            
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

    [ServerRpc]
    void spawnCrapServerRpc() {
        Debug.Log("Called 2" );
        GameObject crap;
        crap = Instantiate(prefabCrap, transform.position, transform.rotation);
        crap.GetComponent<Renderer>().material.color = Color.blue;
        crap.GetComponent<NetworkObject>().SpawnWithOwnership(playerNumber);
    }
}
