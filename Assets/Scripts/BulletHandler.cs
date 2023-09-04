using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class BulletHandler : NetworkBehaviour
{

    public GameObject enemyBullet;

    [ServerRpc]
    public void spawnEnemyBulletServerRpc(float _force, Vector2 _direction) {
        Debug.Log("bruh");
        GameObject clone;
        clone = Instantiate(enemyBullet, transform.position, transform.rotation);
        clone.GetComponent<Rigidbody2D>().AddForce(_direction * _force);
        spawnEnemyBulletClientRPC(_force, _direction);
    }

    [ClientRpc]
    public void spawnEnemyBulletClientRPC(float _force, Vector2 _direction){
        if (!IsServer){
            GameObject clone;
            clone = Instantiate(enemyBullet, transform.position, transform.rotation);
            clone.GetComponent<Rigidbody2D>().AddForce(_direction * _force);
        }
    }
}
