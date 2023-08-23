
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

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            // Instantiate the projectile at the position and rotation of this transform
            Rigidbody2D clone;
            Vector3 worldMousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = worldMousePos - transform.position;
            direction.Normalize();
            clone = Instantiate(playerBullet, transform.position, transform.rotation);

            // Give the cloned object an initial velocity along the current
            // object's Z axis
            

            clone.velocity = (direction) * (bulletSpeed);
        }

    }

    private void FixedUpdate(){
        //Vector2 velocityVector;
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        //velocityVector = new Vector2(xInput, yInput).normalized;
        //rig.velocity = new Vector2(xInput * playerSpeed, yInput * playerSpeed);
        
        if (xInput == 0 || yInput == 0) {
            rig.velocity = new Vector2(xInput * playerSpeed, yInput * playerSpeed);
        } else {
            rig.velocity = new Vector2(xInput * playerSpeed * 0.707f, yInput * playerSpeed* 0.707f);
        }
    }
}
