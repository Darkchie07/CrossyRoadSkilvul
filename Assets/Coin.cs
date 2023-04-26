using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value;
    [SerializeField, Range(0, 10)] private float rotationSpeed = 1;

    public int Value { get => value; }

    void Update()
    {
        transform.Rotate(0, 360 * rotationSpeed * Time.deltaTime, 0);
    }

    public void Colleted()
    {
        GetComponent<Collider>().enabled = false;
        rotationSpeed *= 10;
        this.transform.DOJump(this.transform.position, 1.5f, 1, 0.4f).onComplete = SelfDestruct;
    }

    private void SelfDestruct()
    {
        Destroy(this.gameObject);
    }
}
