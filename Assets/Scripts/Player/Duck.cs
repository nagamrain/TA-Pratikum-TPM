using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Duck : MonoBehaviour
{
    [SerializeField, Range(0,1)] float moveDuration = 0.18f;
    [SerializeField, Range(0,1)] float jumpHeight = 0.3f;
    [SerializeField] int  leftMoveLimit;
    [SerializeField] int  rightMoveLimit;
    [SerializeField] int  backMoveLimit;
    
    private Vector2 touchStartPosition;
    private Vector3 respawnPosition;
    public UnityEvent<Vector3> onJumpEnd;
    public UnityEvent<int> OnGetCoin;
    public UnityEvent OnDie;
    public UnityEvent OnRespawn;
    public UnityEvent OnCarCollision;

    private bool isMoveable = false;

    void Update()
    {
        if (isMoveable == false)
        {
            return;
        }
        if (DOTween.IsTweening(transform))
        {
            return;
        }

        Vector3 direction = Vector3.zero;
        
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction -= Vector3.back;
        }
        else if(Input.GetKeyDown(KeyCode.S)|| Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction -= Vector3.forward;
        }
        else if(Input.GetKeyDown(KeyCode.D)|| Input.GetKeyDown(KeyCode.RightArrow))
        { 
            direction -= Vector3.left;
        }
        else if(Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction -= Vector3.right;
        }

        foreach (Touch touch in Input.touches)
        {
            
        if (touch.phase == TouchPhase.Began)
        {
            touchStartPosition = touch.position;
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            Vector2 touchDelta = touch.position - touchStartPosition;
            float deltaX = Mathf.Abs(touchDelta.x);
            float deltaY = Mathf.Abs(touchDelta.y);

            if (deltaX > deltaY)
            {
                if (touchDelta.x > 0)
                {
                    direction -= Vector3.left;
                }
                else
                {
                    direction -= Vector3.right;
                }
            }
            else
            {
                if (touchDelta.y > 0)
                {
                    direction -= Vector3.back;
                }
                else
                {
                    direction -= Vector3.forward;
                }
            }

            // Reset touch start position
            touchStartPosition = touch.position;
        }
    }

        if (direction == Vector3.zero)
        {
            return;
        }
        Move(direction);
    }

    public void Move(Vector3 direction)
    {  
        var targetPosition = transform.position + direction;

        if (targetPosition.x < leftMoveLimit || targetPosition.x > rightMoveLimit || targetPosition.z < backMoveLimit || Obstacle.AllPositions.Contains(targetPosition))
        {
            targetPosition = transform.position;
        }

        transform.DOJump(
            targetPosition, 
            jumpHeight, 
            1, 
            moveDuration)
            .onComplete = BroadCastPositionOnJumpEnd;

        transform.forward = direction;
    }

    public void SetMoveable(bool value)
    {
        isMoveable = value;
    }

    public void UpdateMoveLimit(int horizontalSize, int backLimit )
    {
        leftMoveLimit = -4;
        rightMoveLimit = 4;
        backMoveLimit = backLimit;
    }

    private void BroadCastPositionOnJumpEnd()
    {
        onJumpEnd.Invoke(transform.position);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Car"))
        {
            if (transform.localScale.y == 0.1f)
                return;

            transform.DOScaleY(0.1f, 0.2f);

            isMoveable = false;
            OnCarCollision.Invoke();
            Invoke("Die", 3);
        }
        else if (other.CompareTag("Coin"))
        {
            var coin = other.GetComponent<Coin>();    
            OnGetCoin.Invoke(coin.Value);
            coin.Collected();      
        }
        else if(other.CompareTag("Eagle"))
        {
            if(this.transform != other.transform)
            {
                this.transform.SetParent(other.transform);
                Invoke("Die", 3);
            }  
        }
    }

    private void Die() {
        {
            respawnPosition = this.transform.position;
            OnDie.Invoke();
        }
    }

    public void Respawn()
    {
        transform.DOScaleY(1f, 0.1f);
        
        isMoveable = true;
        OnRespawn.Invoke();

        this.transform.position = respawnPosition;
    }
}
