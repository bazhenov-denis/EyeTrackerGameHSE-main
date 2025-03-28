using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] Fruits;
    public GameObject[] Enemy;
    public GameObject Bomb;
    public List<GameObject> SpawnObject;
    private Vector3 whereToSpawn;
    public static float nextSpawn = 2.0f;
    private float x;
    private float y;
    public GameObject SpawnerScale;
    private GameObject e;
    public static float speed = 0.3f;
    public static int countBomb = 2;
    IEnumerator Spawn()
    {
        while (true)
        {
            x = Random.Range(-8, 8);
            y = 10;
            var objectIndex = Random.Range(0, SpawnObject.Count);
            whereToSpawn = new Vector3(x, y, 1);
            e = Instantiate(SpawnObject[objectIndex], whereToSpawn, Quaternion.identity, SpawnerScale.transform);
            e.GetComponent<Rigidbody2D>().gravityScale = 0.3f;

            // ƒобавл€ем компонент дл€ фиксации времени спавна, если его нет, и записываем врем€ спавна.
            CatchObjectItem item = e.GetComponent<CatchObjectItem>();
            if (item == null)
                item = e.AddComponent<CatchObjectItem>();
            item.spawnTime = Time.time;


            yield return new WaitForSeconds(nextSpawn);
        }
    }


    void Start()
    {
        for (int i = 0; i < Fruits.Length; i++)
        {
            SpawnObject.Add(Fruits[i]);
        }
        for (int i = 0; i < Enemy.Length; i++)
        {
            SpawnObject.Add(Enemy[i]);
        }

        for (int i = 0; i < countBomb; i++)
        {
            SpawnObject.Add(Bomb);
        }


        StartCoroutine(Spawn());
    }
}
