using System;
using System.Collections;
using System.Collections.Generic;
//using Unity.Mathematics;
using UnityEngine;
using static System.Math;


public class EnemyBehaviour : Sounds
{
    // Start is called before the first frame update
    public Vector3 startPosition;
    public Vector3 endPosition;
    public static float step = 0.005f;
    private float progress;
    public GameObject Enemy;
    public GameObject Explosion;
    private GameObject e;
    public static int counter = 0;
    public static float Scale = 1f;

    public float spawnTime; // время появления врага

    void Start()
    {
        startPosition = Enemy.transform.position;
        Enemy.transform.localScale = Enemy.transform.localScale * Scale;
        endPosition = new Vector3(0, 0, 1);
        spawnTime = Time.time; // фиксируем время спавна
    }
    
    void FixedUpdate()
    {
        Enemy.transform.position = Vector3.Lerp(startPosition, endPosition, progress);
        progress += step;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Вычисляем время реакции как разницу между текущим временем и временем появления врага
        float reactionTime = Time.time - spawnTime;
        // Регистрируем время реакции для анализа
        CosmonautProgressManager.RecordReactionTime(reactionTime);


        PlaySound(sounds[0]);
        e = Instantiate(Explosion, Enemy.transform.position, Enemy.transform.rotation);
        Destroy(Enemy);
        Destroy(e, 0.5f);
        counter += 1;

    }
    
}
