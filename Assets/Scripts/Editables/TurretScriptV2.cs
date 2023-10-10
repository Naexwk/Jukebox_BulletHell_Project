using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TurretScriptV2 : MonoBehaviour
{
    public GameObject alertLight;
    public GameObject canonGun;
    public Transform shootPoint;

    private GameObject[] players;
    private GameObject target;

    public float distanceThreshold = 25f;
    public float fireRate;
    private float timeSinceLastFire;

    private GameObject bullethandler;
    private Vector2 Direction;
    public float force;

    public float bulletSpeed = 5f;
    public int bulletDamage = 5;


    void Start () {
        bullethandler = GameObject.FindWithTag("BulletHandler");
    }

    // Update is called once per frame
    void Update()
    {
        float closestDistance = Mathf.Infinity;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject player in players){
            
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance < closestDistance && distance < distanceThreshold){
                closestDistance = distance;
                //Debug.Log("Closest distance: " + closestDistance);
                target = player.gameObject;
            }
        }

        if (closestDistance == Mathf.Infinity) {
            target = null;
            alertLight.GetComponent<SpriteRenderer>().color = Color.black;
        }

        if (target != null) {
            alertLight.GetComponent<SpriteRenderer>().color = Color.red;
            Direction = target.transform.position - transform.position;
            Direction.Normalize();
            canonGun.transform.up = Direction;

            if ((Time.time - timeSinceLastFire) > (fireRate)) {
                timeSinceLastFire = Time.time;
                shoot();
            }
        }
        
    }

    void shoot(){
        GameObject clone;
        clone = Instantiate(bullethandler.GetComponent<BulletHandler>().prefabBullet, Direction, Quaternion.identity);
        clone.GetComponent<PlayerBullet>().bulletDamage = bulletDamage;
        clone.GetComponent<PlayerBullet>().bulletSpeed = bulletSpeed;
        clone.GetComponent<PlayerBullet>().bulletDirection = Direction;
        clone.GetComponent<Rigidbody2D>().velocity = (Direction) * (bulletSpeed);
        /*if (bullethandler.GetComponent<NetworkObject>().IsOwner) {
            bullethandler.GetComponent<BulletHandler>().spawnEnemyBulletServerRpc(force, Direction, shootPoint.position.x, shootPoint.position.y);
        }*/
    }
}
