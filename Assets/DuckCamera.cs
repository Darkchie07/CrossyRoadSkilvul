using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DuckCamera : MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    [SerializeField, Range(0, 1)] private float moveDuration = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        offset = this.transform.position;
    }

    public void UpdatePosition(Vector3 targetPosition)
    {
        DOTween.Kill(this.transform);
        transform.DOMove(offset + targetPosition, moveDuration );
    }
}
