using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Road : Terrain
{
    [SerializeField] private Car carPrefab;
    [SerializeField] private float minCarSpawnInterval;
    [SerializeField] private float maxCarSpawnInterval;

    private float timer;
    private Vector3 spawmPosition;
    private Quaternion carRotation;

    private void Start()
    {
        if (Random.value > 0.5f)
        {
            spawmPosition = new Vector3(horiszontalSize / 2 + 10, 0, this.transform.position.z);
            carRotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            spawmPosition = new Vector3(-(horiszontalSize / 2 + 10), 0, this.transform.position.z);
            carRotation = Quaternion.Euler(0, 90, 0);
        }
    }

    private void Update()
    {
        if (timer <= 0)
        {
            timer = Random.Range(minCarSpawnInterval, maxCarSpawnInterval);
            var car = Instantiate(carPrefab, spawmPosition, carRotation);
            car.SetupDistanceLimit(horiszontalSize + 30);
            return;
        }

        timer -= Time.deltaTime;
    }
}
