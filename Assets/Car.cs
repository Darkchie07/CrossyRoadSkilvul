using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField, Range(0, 5)] private float speed = 1;
    private Vector3 initialPosition;
    private float distanceLimit;

    public void SetupDistanceLimit(float distance)
    {
        this.distanceLimit = distance;
    }
    private void Start()
    {
        initialPosition = this.transform.position;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if (Vector3.Distance(initialPosition, this.transform.position) > this.distanceLimit)
        {
            Destroy(this.gameObject);
        }
    }
}
