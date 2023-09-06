using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TurretScript : MonoBehaviour
{
    private GameObject Target;
    private List<GameObject> playersIn = new List<GameObject>();
    private bool detected = false;
    public GameObject alertLight;
    public GameObject canonGun;
    public GameObject bullets;
    public Transform shootPoint;
    public float force;
    private float timer;
    Vector2 Direction;
    public float fireRateTimer;

    private GameObject bullethandler;

    private GameObject nearestPlayer;
    private float distanceToNearestPlayer = -1;
    private float distanceToPlayer;

    void Start () {
        bullethandler = GameObject.FindWithTag("BulletHandler");
    }

    // Start is called before the first frame update

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")){
            //Debug.Log("Detected player, count " + playersIn.Count);
            /*if (!playersIn.Contains(other.gameObject))
            {
                playersIn.Add(other.gameObject);
            }*/
            playersIn.Add(other.gameObject);
            
            //UpdateTarget();
            //detected = true;
            //alertLight.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player") || other.CompareTag("Dead Player")){
            //Debug.Log("Player Out, count "+ playersIn.Count);
            playersIn.Remove(other.gameObject);
            //UpdateTarget();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check target
        if (playersIn.Count > 0){
            detected = true;
            alertLight.GetComponent<SpriteRenderer>().color = Color.red;
            distanceToNearestPlayer = -1f;

            foreach (GameObject player in playersIn){

                    distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
                    if ((distanceToPlayer < distanceToNearestPlayer) || distanceToNearestPlayer == -1f) {
                        distanceToNearestPlayer = distanceToPlayer;
                        nearestPlayer = player;
                    }

            }
            Target = nearestPlayer;
        }
        else{
            distanceToNearestPlayer = -1f;
            Target = null;
            detected = false;
            alertLight.GetComponent<SpriteRenderer>().color = Color.black;
        }


        if (detected){
            Direction = Target.transform.position - transform.position;
            Direction.Normalize();
            canonGun.transform.up = Direction;
            timer += Time.deltaTime;
            if (timer > fireRateTimer){
                timer = 0;
                shoot();
            }
        }
    }

    void shoot(){
        if (bullethandler.GetComponent<NetworkObject>().IsOwner) {
            bullethandler.GetComponent<BulletHandler>().spawnEnemyBulletServerRpc(force, Direction, shootPoint.position.x, shootPoint.position.y);
        }
    }
}
