using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class BulletHandler : NetworkBehaviour
{

    // Script para manejar las balas enemigas

    // Prefab de bala enemiga
    public GameObject enemyBullet;

    // Prefab de bala de jugador
    public GameObject prefabBullet;

    // Prefab de bala de jugador falsa
    public GameObject prefabFakeBullet;

    // Prefab de bala de queso
    public GameObject prefabCheeseBullet;

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

    // Llamada al server para crear una bala enemiga server-side
    [ServerRpc]
    public void spawnEnemyBulletServerRpc(float _force, Vector2 _direction, float _x, float _y) {
        GameObject clone;
        clone = Instantiate(enemyBullet, new Vector3 (_x, _y, 0f), transform.rotation);
        clone.GetComponent<Rigidbody2D>().AddForce(_direction * _force);
        spawnEnemyBulletClientRPC(_force, _direction, _x, _y);
    }

    // Llamada a todos los clientes para clonar la bala enemiga
    [ClientRpc]
    public void spawnEnemyBulletClientRPC(float _force, Vector2 _direction, float _x, float _y){
        if (!IsServer){
            GameObject clone;
            clone = Instantiate(enemyBullet, new Vector3 (_x, _y, 0f), transform.rotation);
            clone.GetComponent<Rigidbody2D>().AddForce(_direction * _force);
        }
    }

    // Llamada al server para crear una bala de jugador server-side
    [ServerRpc(RequireOwnership = false)]
    public void spawnBulletServerRpc(float _bulletSpeed, int _bulletDamage, Vector2 _direction, ulong playerNumber, float _x, float _y) {
        GameObject clone;
        clone = Instantiate(prefabBullet, new Vector3 (_x, _y, 0f), transform.rotation);
        clone.GetComponent<PlayerBullet>().bulletDamage = _bulletDamage;
        clone.GetComponent<PlayerBullet>().bulletSpeed = _bulletSpeed;
        clone.GetComponent<PlayerBullet>().bulletDirection = _direction;
        clone.GetComponent<Rigidbody2D>().velocity = (_direction) * (_bulletSpeed);
        colorCodeToPlayer(clone, playerNumber);
        spawnFakeBulletsClientRPC(_bulletSpeed, _direction, playerNumber, _x, _y);
    }

    // Llamada a todos los clientes para clonar la bala de jugador (falsa)
    [ClientRpc]
    void spawnFakeBulletsClientRPC(float _bulletSpeed, Vector2 _direction, ulong playerNumber, float _x, float _y){

        if (!IsServer){
            GameObject clone;
            clone = Instantiate(prefabFakeBullet, new Vector3 (_x, _y, 0f), transform.rotation);
            clone.GetComponent<FakePlayerBullet>().bulletSpeed = _bulletSpeed;
            clone.GetComponent<Rigidbody2D>().velocity = (_direction) * (_bulletSpeed);
            colorCodeToPlayer(clone, playerNumber);
        }
        
    }

    // Llamada al server para crear una bala de queso
    [ServerRpc(RequireOwnership = false)]
    public void spawnCheeseBulletServerRpc(Vector2 _direction, ulong playerNumber, int _abilityDamage, float _x, float _y) {
        GameObject clone;
        clone = Instantiate(prefabCheeseBullet, new Vector3 (_x, _y, 0f), transform.rotation);
        clone.GetComponent<CheeseBullet>().bulletDamage = _abilityDamage;
        clone.GetComponent<Rigidbody2D>().velocity = (_direction) * (10f);
        clone.GetComponent<NetworkObject>().Spawn();
        /*if (playerNumber != 0) {
            clone.GetComponent<NetworkObject>().NetworkHide(playerNumber);
        }*/
        
    }
}
