
using UnityEngine;
using Unity.Netcode;
using System;
using System.Collections;

delegate void specialAbility();
public class PlayerController : NetworkBehaviour
{
    public float playerSpeed;
    public float bulletSpeed;
    public float maxHealth;
    public float currentHealth;
    public Rigidbody2D rig;
    public Rigidbody2D playerBullet;
    Vector2 rb;
    private Camera _mainCamera;
    public ulong playerNumber;
    private GameObject outline;
    private float timeSinceLastFire;
    // Fire rate (in bullets per second)
    public int fireRate;
    public int bulletDamage;


    public GameObject prefabCrap;
    public GameObject prefabBullet;
    public GameObject prefabFakeBullet;
    public GameObject prefabCheeseBullet;
    public string characterCode = "cheeseman";
    // Special ability cooldown (in seconds)
    public float abilityCooldown;
    private float timeSinceLastAbility;
    public int abilityDamage;

    public bool enableControl = false;

    public GameObject cameraTargetPrefab;

    specialAbility specAb;

    public float invulnerabilityWindow;

    public bool isInvulnerable;



    // hardcodeado porque unity me odia
    private Vector3[] spawnPositions = { new Vector3(15f,4.5f,0f), new Vector3(16f,-9.23f,0f), new Vector3(-12.5f,-10f,0f), new Vector3(-18f,6.85f,0f) };

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
        currentHealth = maxHealth;

        // Change outline
        colorCodeToPlayer(outline, playerNumber);
        if (characterCode == "cheeseman") {
            specAb = new specialAbility(CheesemanSA);
        }
        if (IsOwner) {
            spawnCameraTargetServerRpc(playerNumber);
        }
        

    }

    void Update()
    {
        if (!IsOwner) {
            return;
        }

        if (!enableControl){
            return;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {

            if ((Time.time - timeSinceLastFire) > (1f/fireRate)) {
                Vector3 worldMousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = worldMousePos - transform.position;
                direction.Normalize();
                spawnBulletServerRpc(bulletSpeed, bulletDamage, direction, playerNumber);
                timeSinceLastFire = Time.time;
            }
            
        }

        if (Input.GetKey(KeyCode.Mouse1)){
            if ((Time.time - timeSinceLastAbility) > (abilityCooldown)) {
                specAb();
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
        }

        
    }


    private void FixedUpdate(){

        if (!IsOwner) {
            return;
        }

        if (!enableControl){
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

    public void GetHit(){
        currentHealth -= 1;
        if (currentHealth <= 0) {
            Die();
        } else {
            StartCoroutine(recordInvulnerabiltyFrames());
        }
    }

    public void Die(){
        transform.rotation = Quaternion.Euler(new Vector3(0,0,90));
        enableControl = false;
    }

    IEnumerator recordInvulnerabiltyFrames()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityWindow);
        isInvulnerable = false;
    }

    [ServerRpc]
    void spawnCrapServerRpc() {
        GameObject crap;
        crap = Instantiate(prefabCrap, transform.position, transform.rotation);
        crap.GetComponent<Renderer>().material.color = Color.blue;
        crap.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc]
    void spawnBulletServerRpc(float _bulletSpeed, int _bulletDamage, Vector2 _direction, ulong playerNumber) {
        GameObject clone;
        clone = Instantiate(prefabBullet, transform.position, transform.rotation);
        clone.GetComponent<PlayerBullet>().bulletDamage = _bulletDamage;
        clone.GetComponent<PlayerBullet>().bulletSpeed = _bulletSpeed;
        clone.GetComponent<PlayerBullet>().bulletDirection = _direction;
        clone.GetComponent<Rigidbody2D>().velocity = (_direction) * (_bulletSpeed);
        colorCodeToPlayer(clone, playerNumber);
        spawnFakeBulletsClientRPC(_bulletSpeed, _direction);
    }
    [ClientRpc]
    void spawnFakeBulletsClientRPC(float _bulletSpeed, Vector2 _direction){

        if (!IsServer){
            GameObject clone;
            clone = Instantiate(prefabFakeBullet, transform.position, transform.rotation);
            clone.GetComponent<FakePlayerBullet>().bulletSpeed = _bulletSpeed;
            clone.GetComponent<Rigidbody2D>().velocity = (_direction) * (_bulletSpeed);
            colorCodeToPlayer(clone, playerNumber);
        }
        
    }

    

    private void CheesemanSA () {
        
            Vector3 worldMousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = worldMousePos - transform.position;
            direction.Normalize();
            timeSinceLastAbility = Time.time;
            if (!IsServer){
                GameObject clone;
                clone = Instantiate(prefabCheeseBullet, transform.position, transform.rotation);
                Physics2D.IgnoreCollision(clone.transform.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                clone.GetComponent<CheeseBullet>().cheeseDamage = 0;
                clone.GetComponent<Rigidbody2D>().velocity = (direction) * (10f);
            }
            spawnCheeseBulletServerRpc(direction, playerNumber);
    }

    [ServerRpc]
    void spawnCheeseBulletServerRpc(Vector2 _direction, ulong playerNumber) {
        GameObject clone;
        clone = Instantiate(prefabCheeseBullet, transform.position, transform.rotation);
        Physics2D.IgnoreCollision(clone.transform.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        clone.GetComponent<CheeseBullet>().cheeseDamage = abilityDamage;
        clone.GetComponent<Rigidbody2D>().velocity = (_direction) * (10f);
        clone.GetComponent<NetworkObject>().Spawn();
        if (playerNumber != 0) {
            clone.GetComponent<NetworkObject>().NetworkHide(playerNumber);
        }
        
    }


    [ClientRpc]
    public void spawnPlayerClientRpc(){
        gameObject.transform.position = spawnPositions[Convert.ToInt32(playerNumber)];
        enableControl = true;
        
    }

    [ServerRpc(RequireOwnership = false)]
    public void spawnCameraTargetServerRpc(ulong _playerNumber){
        Debug.Log("Called from " + _playerNumber);
        GameObject spawnCam;
        spawnCam = Instantiate(cameraTargetPrefab, new Vector3(0f,0f,0f), transform.rotation);
        spawnCam.GetComponent<NetworkObject>().SpawnWithOwnership(_playerNumber);
        
    }

    [ClientRpc]
    public void startCameraClientRpc(){
        GameObject mainCam;
        mainCam = GameObject.FindWithTag("MainCamera");
        
        GameObject[] cameraTargets = GameObject.FindGameObjectsWithTag("CameraTarget");
        foreach (GameObject cameraTarget in cameraTargets)
        {
            if(cameraTarget.GetComponent<NetworkObject>().IsOwner){
                mainCam.GetComponent<CameraMovement>().setCameraTarget(cameraTarget.transform);
                cameraTarget.GetComponent<CameraTarget>().StartCam();
            }
        }
        
    }



    

}
