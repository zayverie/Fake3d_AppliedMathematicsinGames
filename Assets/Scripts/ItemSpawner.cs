using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.Pool;

public class ItemSpawner : MonoBehaviour
{
    public Item itemToSpawn;
    public float speed = 5f;
    public List<Item> items = new List<Item>();
    public Transform parent;

    [Header("Pool Settings")]
    [SerializeField] private bool collectionCheck = true;
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 100;

    private Dictionary<GameObject, IObjectPool<Item>> _pools = new Dictionary<GameObject, IObjectPool<Item>>();
    private float[] lanes = {-1, 0, 1};

    private void Start()
    {
        InvokeRepeating(nameof(BeginSpawn), 1, .5f);
    }

    private void BeginSpawn()
    {
        if (itemToSpawn == null)
        {
            return;
        }

        IObjectPool<Item> pool = GetPool(itemToSpawn.gameObject);
        Item spawnedItem = pool.Get();
    
        spawnedItem.itemPosition = GetRandomLocation();
        spawnedItem.ApplyPerspective();
        
        if (!items.Contains(spawnedItem))
        {
            items.Add(spawnedItem);
        }
    }

    private Vector3 GetRandomLocation()
    {
        float xRand = 0;
        if (lanes.Length > 0)
        {
            int randomIndex = Random.Range(0, lanes.Length);
            xRand = lanes[randomIndex];
        }
        
        float groundY = -3f;
        return new Vector3(xRand, groundY, 2000f);
    }

    private void Update()
    {
        foreach (var item in items)
        {
            if (item == null || !item.gameObject.activeInHierarchy)
            {
                continue;
            }
            ItemMover(item);
        }

        List<Transform> children = new List<Transform>();

        foreach(Transform child in transform)
        {
            if (!child.gameObject.activeInHierarchy)
            {
                continue;
            }
            children.Add(child);
        }       
        children.OrderBy(x => x.position.z);
        
        for(int i = 0 ; i < children.Count; i++)
        {
            children[i].SetSiblingIndex(i);
        }
    }

    private void ItemMover(Item item)
    {
        item.itemPosition.z -= speed;
    }

    #region Pooling
    private IObjectPool<Item> GetPool(GameObject prefab)
    {
        if (_pools.TryGetValue(prefab, out IObjectPool<Item> pool))
            return pool;

        pool = new ObjectPool<Item>(
            () => CreateItem(prefab),
            OnGetFromPool,
            OnReleaseToPool,
            OnDestroyPooledObject,
            collectionCheck,
            defaultCapacity,
            maxSize);

        _pools.Add(prefab, pool);
        return pool;
    }

    private Item CreateItem(GameObject prefab)
    {
        GameObject instance = Instantiate(prefab, parent != null ? parent : transform);
        instance.SetActive(false);

        Item item = instance.GetComponent<Item>();
        item.ObjPool = _pools[prefab];

        return item;
    }

    private void OnGetFromPool(Item item)
    {
        item.gameObject.SetActive(true);
    }

    private void OnReleaseToPool(Item item)
    {
        item.gameObject.SetActive(false);
    }

    private void OnDestroyPooledObject(Item item)
    {
        if (item != null)
            Destroy(item.gameObject);
    }
    #endregion


}