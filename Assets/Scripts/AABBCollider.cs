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
        float currentY = (item != null) ? item.itemPosition.y : this.transform.position.y;

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
            if (Player.Instance.isInvincible)
            {
                return;
            }
            else
            {
                if (other.TryGetComponent<Item>(out Item hitItem))
                {
                    if (this.gameObject == Player.Instance.gameObject)
                    {
                        Debug.Log($"Player Y: {transform.position.y} | Item Y: {other.transform.position.y}");
                        Player.Instance.isCollidingWithItem = true;
                        Player.Instance.TakeDamage(5);

                        hitItem.ReleaseItemToPool();
                    }
                }
            }
            
        }
    }
}