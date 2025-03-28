using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnerEnemy : MonoBehaviour
{
    public GameObject[] Enemy;
    private Vector3 whereToSpawn;
    public static float nextSpawn = 2.0f;
    private float x;
    private float y;


    public GameObject Spawner;
    IEnumerator Spawn()
    {
        while (true)
        {
            while (true)
            {
                x = Random.Range(-15, 15);
                y = Random.Range(-15, 15);
                if ((x < -12 || x > 12)  || (y > 12 || y < -12))
                {
                    break; 
                }

            }
            var objectIndex = Random.Range(0, Enemy.Length);
            whereToSpawn = new Vector3(x, y, 1);
            Instantiate(Enemy[objectIndex], whereToSpawn, Quaternion.identity, Spawner.transform);

            yield return new WaitForSeconds(nextSpawn);
        }
    }


    void Start()
    {
        StartCoroutine(Spawn());
    }

}