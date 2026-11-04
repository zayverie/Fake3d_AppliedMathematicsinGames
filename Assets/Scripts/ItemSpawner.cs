using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Runtime.CompilerServices;

public class ItemSpawner : MonoBehaviour
{
    public Item itemToSpawn;
    public float speed = 5f;
    public List<Item> items = new List<Item>();
    public Transform parent;
    private float[] lanes = {-1, 0, 1};

    private void Start()
    {
        InvokeRepeating(nameof(BeginSpawn), 1, .5f);
    }

    private void BeginSpawn()
    {
        var spawnedItem = Instantiate(itemToSpawn, parent);
        spawnedItem.itemPosition = GetRandomLocation();
        spawnedItem.ApplyPerspective();
        items.Add(spawnedItem);
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
            if (item == null)
            {
                continue;
            }
            ItemMover(item);
        }
        // transform.position = GetRandomLocation();
        List<Transform> children = new List<Transform>();
        foreach(Transform child in transform)
        {
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


}