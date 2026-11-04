using UnityEngine;
using System.Collections.Generic;

public class AABBCollider : MonoBehaviour
{
    public float Length => _length;
    public float Width => _width;
    [SerializeField] private float _length;
    [SerializeField] private float _width;
    [SerializeField] private float _depth;

    public Vector3 max {get; private set;}
    public Vector3 min {get; private set;}

    private void Start()
    {
        if (CollisionManager.Instance != null)
        {
            CollisionManager.Instance.Register(this);
        }
    }

    private void OnDestroy()
    {
        if (CollisionManager.Instance != null)
        {
            CollisionManager.Instance.Unregister(this);
        }
    }

    public void FixedUpdate()
    {
        var posX = this.transform.position.x + _width * .5f;
        var posY = this.transform.position.y + _length * .5f;

        var negX = this.transform.position.x - _width * .5f;
        var negY = this.transform.position.y - _length * .5f;

        Item item = GetComponent<Item>();
        float currentZ = (item != null) ? item.itemPosition.z : this.transform.position.z;

        var posZ = currentZ + _depth * .5f;
        var negZ = currentZ - _depth * .5f;

        max = new Vector3(posX, posY, posZ);
        min = new Vector3(negX, negY, negZ);
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawLine(new Vector3(min.x, max.y, 0), new Vector3(max.x, max.y, 0), Color.azure); 
        Debug.DrawLine(new Vector3(max.x, max.y, 0), new Vector3(max.x, min.y, 0), Color.azure); 
        Debug.DrawLine(new Vector3(max.x, min.y, 0), new Vector3(min.x, min.y, 0), Color.azure); 
        Debug.DrawLine(new Vector3(min.x, min.y, 0), new Vector3(min.x, max.y, 0), Color.azure); 
    }

    public void OnCollision(AABBCollider other)
    {
        if (Player.Instance != null)
        {
            // if (other.gameObject.name == "Ground")
            // {
            //     if (this.gameObject == W8_Player.Instance.gameObject && W8_Player.Instance.verticalVelocity <= 0f)
            //     {
            //         W8_Player.Instance.ResetVerticalVelocity();
            //         W8_Player.Instance.isGrounded = true;
            //         W8_Player.Instance.transform.position = new Vector3(W8_Player.Instance.transform.position.x, other.max.y + _length * 0.5f, W8_Player.Instance.transform.position.z);
            //     }
            // }

            if (other.gameObject.name.StartsWith("Item"))
            {
                if (this.gameObject == Player.Instance.gameObject)
                {
                    Player.Instance.isCollidingWithItem = true;
                    Player.Instance.TakeDamage();
                    // bool isHorizontallyOver = this.max.x > other.min.x && this.min.x < other.max.x;
                    // bool isAboveObstacle = this.min.y >= other.max.y - 0.25f;

                    // if (Player.Instance.verticalVelocity <= 0f && isHorizontallyOver && isAboveObstacle)
                    // {
                    //     Player.Instance.ResetVerticalVelocity();
                    //     Player.Instance.isGrounded = true;
                    //     Player.Instance.transform.position = new Vector3(Player.Instance.transform.position.x, other.max.y + _length * 0.5f, Player.Instance.transform.position.z);
                    // }
                    // else if (!isAboveObstacle)
                    // { 
                    //     if (Player.Instance.transform.position.x < other.transform.position.x)
                    //     {
                    //         Player.Instance.transform.position = new Vector3(other.min.x - _width * 0.5f, Player.Instance.transform.position.y, Player.Instance.transform.position.z);
                    //     }
                    //     else if (Player.Instance.transform.position.x > other.transform.position.x)
                    //     {
                    //         Player.Instance.transform.position = new Vector3(other.max.x + _width * 0.5f, Player.Instance.transform.position.y, Player.Instance.transform.position.z);
                    //     }
                    // }  
                }
            }
        }
    }
}