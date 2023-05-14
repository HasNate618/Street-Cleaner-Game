using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] prefabs;
    private List<Transform> items = new List<Transform>();

    [SerializeField] float positionRange, rotationRange, spawnPeriod, scrollSpeed, despawnPoint;

    [SerializeField] Sprite[] itemIcons, binIcons;

    public static Transform draggedItem = null;

    void Start()
    {
        InvokeRepeating("SpawnItem", spawnPeriod, spawnPeriod);
    }

    private void SpawnItem()
    {
        GameObject prefab = prefabs[UnityEngine.Random.Range(0, prefabs.Length)];
        Vector3 spawnPoint = new Vector3(UnityEngine.Random.Range(-positionRange, positionRange), 0, transform.position.z);
        Quaternion randomRotation = Quaternion.Euler(90, 0, UnityEngine.Random.Range(-rotationRange, rotationRange));
        GameObject item = Instantiate(prefab, spawnPoint, randomRotation);
        items.Add(item.transform);
    }

    private void Update()
    {
        Transform remove = null;

        foreach (Transform item in items) 
        {
            if (item != draggedItem)
            {
                item.position += new Vector3(0, 0, -scrollSpeed * Time.deltaTime);
                if (item.position.z < despawnPoint)
                {
                    remove = item;
                }
            }
        }

        if (remove != null)
        {
            items.Remove(remove);
            if (!remove.GetComponent<DraggableObject>().complete)
            {
                Destroy(remove.gameObject);
                GameManager.instance.UpdateScore("Missed");
            }
        }
    }

    public void ClearItems()
    {
        foreach (Transform item in items)
        {
            item.gameObject.SetActive(false);
        }
    }

    public string GetItemWasteType(string itemName)
    {
        foreach (GameObject item in prefabs) 
        {
            if (item.name == itemName)
            {
                return item.GetComponent<DraggableObject>().wasteType;
            }
        }
        return "Not Found";
    }

    public Sprite GetIconFromItem(string itemName)
    {
        foreach (GameObject item in prefabs)
        {
            if (item.name == itemName)
            {
                return itemIcons[Array.IndexOf(prefabs, item)];
            }
        }
        return null;
    }

    public Sprite GetIconFromWasteType(string wasteType)
    {
        switch (wasteType)
        {
            case "Recycling":
                return binIcons[0];
            case "Garbage":
                return binIcons[1];
            case "Organics":
                return binIcons[2];
        }
        return null;
    }
}
