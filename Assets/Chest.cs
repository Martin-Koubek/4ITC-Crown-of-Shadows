using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Chest : MonoBehaviour
{
    public List<GameObject> lootList = new();
    bool open;
    public Transform dropSpot;
    public Collider hitBox;

    public void OpenChest()
    {
        if (open)
        {
            return;
        }
        else
        {
            open = true;
            GameObject loot = Instantiate(lootList[Random.Range(0, lootList.Count)], dropSpot);
            loot.transform.parent = null;
            loot.transform.position = dropSpot.position;
            hitBox.enabled = false;
        }
    }
}
