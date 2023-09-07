using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ZombieSpawner : NetworkBehaviour
{
    // DEV: Variable de control, implementar
    public bool isInPlay;

    public GameObject zombiePrefab;
    public float timeToSpawn;

    // Empezar a instanciar zombies
    // DEV: Debería parar si no está en juego (!isInPlay)
    void Start()
    {
        StartCoroutine(spawnZombie());
    }


    // Aparecer un zombie cada timeToSpawn segundos
    IEnumerator spawnZombie() {
        yield return new WaitForSeconds(timeToSpawn);
        spawnZombieServerRpc();
        StartCoroutine(spawnZombie());
    }

    // Llamar al server para spawner al zombie en la red
    [ServerRpc]
    void spawnZombieServerRpc(){
        GameObject clone;
        clone = Instantiate(zombiePrefab, transform.position, transform.rotation);
        clone.GetComponent<NetworkObject>().Spawn();
    }
}
