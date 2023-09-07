
using UnityEngine;
using Unity.Netcode;
using System;
using System.Collections;

delegate void specialAbility();
public class PlayerController : NetworkBehaviour
{

    // Estadísticas de jugador
    public float playerSpeed;
    public float bulletSpeed;
    public float maxHealth;
    public int fireRate; // en disparos por segundo
    public int bulletDamage;

    // Variables de control
    public bool enableControl = false;
    public float currentHealth;
    private float timeSinceLastFire;
    public float abilityCooldown; // en segundos
    private float timeSinceLastAbility;
    public int abilityDamage;
    public bool isInvulnerable;
    public float invulnerabilityWindow;

    // Variables de personaje
    public string characterCode = "cheeseman";
    specialAbility specAb;

    // Objetos para movimiento
    public Rigidbody2D rig;

    // Objetos de cámara
    private Camera _mainCamera;
    public GameObject cameraTargetPrefab;

    // Objetos de Network
    public ulong playerNumber;
    private GameObject bullethandler;
    public GameObject prefabMenuManager;

    // Variables visuales
    private GameObject outline;
    
    // Spawn points
    // hardcodeado porque unity me odia
    private Vector3[] spawnPositions = { new Vector3(15f,4.5f,0f), new Vector3(16f,-9.23f,0f), new Vector3(-12.5f,-10f,0f), new Vector3(-18f,6.85f,0f) };

    // Función para colorear objetos según el número del jugador
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

    // Obtener su cámara, generador de balas, número de jugador y colorear su outline
    // También asigna la función de habilidad especial a specAb
    void Start()
    {
        _mainCamera = Camera.main;
        bullethandler = GameObject.FindWithTag("BulletHandler");
        playerNumber = gameObject.GetComponent<NetworkObject>().OwnerClientId;
        outline = gameObject.transform.GetChild(0).gameObject;

        //Instantiate(prefabMenuManager, new Vector3(0f,0f,0f), transform.rotation);
        if (IsOwner){
            spawnMenuManagerServerRpc(playerNumber);
        }
        
        
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
        // Si no es dueño de este script, ignorar
        if (!IsOwner) {
            return;
        }

        // Si no está habilitado el control, ignorar
        if (!enableControl){
            return;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Shoot();
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            castSpecialAbility();
        }
    }

    private void FixedUpdate(){

        // Si no es dueño de este script, ignorar
        if (!IsOwner) {
            return;
        }

        // Si no está habilitado el control, ignorar
        if (!enableControl){
            return;
        }

        Move();
        
    }

    // Maneja el movimiento del jugador
    private void Move(){
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");

        if (xInput == 0 || yInput == 0) {
            rig.velocity = new Vector2(xInput * playerSpeed, yInput * playerSpeed);
        } else {
            rig.velocity = new Vector2(xInput * playerSpeed * 0.707f, yInput * playerSpeed* 0.707f);
        }
    }

    // Dispara una bala si ya se cumplió el tiempo de espera del firerate.
    private void Shoot(){
        if ((Time.time - timeSinceLastFire) > (1f/fireRate)) {
            Vector3 worldMousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = worldMousePos - transform.position;
            direction.Normalize();
            bullethandler.GetComponent<BulletHandler>().spawnBulletServerRpc(bulletSpeed, bulletDamage, direction, playerNumber, transform.position.x, transform.position.y);
            timeSinceLastFire = Time.time;
        }
    }

    // Castea la habilidad especial, que depende del personaje
    private void castSpecialAbility(){
        if ((Time.time - timeSinceLastAbility) > (abilityCooldown)) {
            specAb();
        }
    }

    // Funciones de habilidades especiales

    // Cheeseman: Aparece una bola de queso que daña a los enemigos
    private void CheesemanSA () {
        
            Vector3 worldMousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = worldMousePos - transform.position;
            direction.Normalize();
            timeSinceLastAbility = Time.time;

            // DEV: Esto es para crear una bala client-side. Es muy poco confiable, entonces opté por
            // una bala server-side, aunque se vea con un poco de lag.
            /*if (!IsServer){
                GameObject clone;
                clone = Instantiate(prefabCheeseBullet, transform.position, transform.rotation);
                Physics2D.IgnoreCollision(clone.transform.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                clone.GetComponent<CheeseBullet>().bulletDamage = 3;
                clone.GetComponent<Rigidbody2D>().velocity = (direction) * (10f);
            }*/

            bullethandler.GetComponent<BulletHandler>().spawnCheeseBulletServerRpc(direction, playerNumber, abilityDamage, transform.position.x, transform.position.y);
    }

    // Función pública para hacer daño al jugador
    public void GetHit(){
        // Si es invulnerable, ignorar
        if (isInvulnerable) {
            return;
        }

        // Hacer daño y dar invulnerabilidad o morir
        currentHealth -= 1;
        if (currentHealth <= 0) {
            Die();
        } else {
            StartCoroutine(recordInvulnerabiltyFrames());
        }
    }

    // El jugador cae de lado, y se le quita el control
    public void Die(){
        transform.rotation = Quaternion.Euler(new Vector3(0,0,90));
        enableControl = false;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0f,0f,0f);
        gameObject.tag = "Dead Player";
        gameObject.GetComponent<Rigidbody2D>().simulated = false;
    }

    // Se miden unos segundos igual a invulnerabilityWindow,
    // durante este tiempo el jugador es transparente e invulnerable
    IEnumerator recordInvulnerabiltyFrames()
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        SpriteRenderer bSquareRenderer = this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        renderer.color = new Color(1f, 1f, 1f, 0.5f);
        bSquareRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityWindow);
        isInvulnerable = false;
        renderer.color = new Color(1f, 1f, 1f, 1f);
        bSquareRenderer.color = new Color(1f, 1f, 1f, 1f);
    }


    // Ignora las colisiones con otros jugadores
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Dead Player")
        {
            Physics2D.IgnoreCollision(collision.transform.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    // Función para respawnear al jugador
    // DEV: Añadir reset de estadísticas
    public void Respawn(){
        gameObject.tag = "Player";
        currentHealth = maxHealth;
        enableControl = true;
        StartCoroutine(recordInvulnerabiltyFrames());
        transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
        gameObject.transform.position = spawnPositions[Convert.ToInt32(playerNumber)];
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0f,0f,0f);
        gameObject.GetComponent<Rigidbody2D>().simulated = true;
        //spawnPlayerClientRpc();
    }

    // Función para Despawnear
    // Usada para las fases de compra/edicion
    public void Despawn(){
        gameObject.transform.position = new Vector3(0f,50f,0f);
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0f,0f,0f);
        gameObject.GetComponent<Rigidbody2D>().simulated = false;
        enableControl = false;

        // DEV: Move to the Shadow Realm
    }

    

    // Se ejecuta en todos los clientes
    // Spawnea a los jugadores y les da control
    /*
    [ClientRpc]
    public void spawnPlayerClientRpc(){
        gameObject.transform.position = spawnPositions[Convert.ToInt32(playerNumber)];
        enableControl = true;
    }*/


    // Se ejecuta en el servidor
    // Aparece un target que sigue al jugador
    [ServerRpc(RequireOwnership = false)]
    public void spawnCameraTargetServerRpc(ulong _playerNumber){
        //Debug.Log("Called from " + _playerNumber);
        GameObject spawnCam;
        spawnCam = Instantiate(cameraTargetPrefab, new Vector3(0f,0f,0f), transform.rotation);
        spawnCam.GetComponent<NetworkObject>().SpawnWithOwnership(_playerNumber);
        
    }

    // Se ejecuta en todos los clientes
    // Le dota su objetivo a la cámara
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

    
    [ServerRpc(RequireOwnership = false)]
    public void spawnMenuManagerServerRpc(ulong _playerNumber){
        //Debug.Log("Called from " + _playerNumber);
        GameObject spawnMM;
        spawnMM = Instantiate(prefabMenuManager, new Vector3(0f,0f,0f), transform.rotation);
        spawnMM.GetComponent<NetworkObject>().SpawnWithOwnership(_playerNumber);
        
    }



}
