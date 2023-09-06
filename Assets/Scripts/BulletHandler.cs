using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class BulletHandler : NetworkBehaviour
{

    public GameObject enemyBullet;

    [ServerRpc]
    public void spawnEnemyBulletServerRpc(float _force, Vector2 _direction, float _x, float _y) {
        GameObject clone;
        clone = Instantiate(enemyBullet, new Vector3 (_x, _y, 0f), transform.rotation);
        clone.GetComponent<Rigidbody2D>().AddForce(_direction * _force);
        spawnEnemyBulletClientRPC(_force, _direction, _x, _y);
    }

    [ClientRpc]
    public void spawnEnemyBulletClientRPC(float _force, Vector2 _direction, float _x, float _y){
        if (!IsServer){
            GameObject clone;
            clone = Instantiate(enemyBullet, new Vector3 (_x, _y, 0f), transform.rotation);
            clone.GetComponent<Rigidbody2D>().AddForce(_direction * _force);
        }
    }
}
