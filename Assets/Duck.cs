using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using DG.Tweening;
using UnityEditor.Timeline.Actions;
using UnityEngine.Events;

public class Duck : MonoBehaviour
{
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip car;
    [SerializeField] private AudioClip coincollect;
    [SerializeField] private AudioClip eagle;
    [SerializeField, Range(0, 1)] private float moveDuration;
    [SerializeField, Range(0, 1)] private float jumpHeight;
    [SerializeField] private int leftMoveLimit;
    [SerializeField] private int rightMoveLimit;
    [SerializeField] private int backMoveLimit;
    
    public UnityEvent<Vector3> OnJumpEnd;
    public UnityEvent<int> OnGetCoin;
    public UnityEvent OnCarCollision;
    public UnityEvent OnDie;
    private bool isMoveAble;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoveAble == false)
        {
            return;
        }
        if (DOTween.IsTweening(transform))
            return;
        
        Vector3 direction = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction += Vector3.forward;
        }else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction += Vector3.back;
        }else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction += Vector3.right;
        }else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction += Vector3.left;
        }

        if (direction == Vector3.zero)
        {
            return;
        }
        Move(direction);
    }

    public void UpdateMoveLimit(int horizontalSize, int backLimit)
    {
        leftMoveLimit = -horizontalSize / 2;
        rightMoveLimit = horizontalSize / 2;
        backMoveLimit = backLimit;
    }
    
    public void Move(Vector3 direction)
    {
        var targetPosition = transform.position + direction;
        if (targetPosition.x < leftMoveLimit || 
            targetPosition.x > rightMoveLimit || 
            targetPosition.z < backMoveLimit || 
            Tree.AllPositions.Contains(targetPosition))
        {
            targetPosition = transform.position;
            
        }
        transform.DOJump(targetPosition, jumpHeight, 1, moveDuration).onComplete
             = BroadcastPositionOnJumpEnd;
        transform.forward = direction;
        SoundManager.Instance.playSFX(jump);
    }

    public void SetMoveable(bool value)
    {
        isMoveAble = value;
    }

    private void BroadcastPositionOnJumpEnd()
    {
        OnJumpEnd.Invoke(transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            if (transform.localScale.y == 0.1f)
            {
                return;
            }

            transform.DOScale(new Vector3(2, 0.1f,2), 0.2f);
            isMoveAble = false;
            OnCarCollision.Invoke();
            Invoke("Die", 3);
            SoundManager.Instance.playSFX(car);
        }else if (other.CompareTag("Coin"))
        {
            var coin = other.GetComponent<Coin>();
            OnGetCoin.Invoke(coin.Value);
            coin.Colleted();
            SoundManager.Instance.playSFX(coincollect);
        }else if (other.CompareTag("Eagle"))
        {
            if (this.transform != other.transform)
            { 
                this.transform.SetParent(other.transform);
                Invoke("Die", 3);
                SoundManager.Instance.playSFX(eagle);
            }
           
        }
    }

    private void Die()
    {
        OnDie.Invoke();
    }
}

