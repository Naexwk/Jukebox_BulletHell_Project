using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Inicio");
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")){
            Debug.Log($"Detected player");
            playersIn.Add(other.gameObject);
            UpdateTarget();
            detected = true;
            alertLight.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")){
            Debug.Log($"Player Out");
            playersIn.Remove(other.gameObject);
            UpdateTarget();
        }
    }

    void UpdateTarget(){
        if (playersIn.Count > 0){
            Target = playersIn[playersIn.Count - 1];
        }
        else{
            Target = null;
            detected = false;
            alertLight.GetComponent<SpriteRenderer>().color = Color.black;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (detected){
            Direction = Target.transform.position - transform.position;
            canonGun.transform.up = Direction;
            timer += Time.deltaTime;
            if (timer > 0.5){
                timer = 0;
                shoot();
                Debug.Log($"Is Shooting");
            }
        }
    }

    void shoot(){
        GameObject newBullets = Instantiate(bullets, shootPoint.position, Quaternion.identity);
        newBullets.GetComponent<Rigidbody2D>().AddForce(Direction * force);
    }
}
