using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script is the item behaviour which is sort of the same process as the other cards once the item is obtained
/// However the key difference being that the item only has to instaniate the item in order to provide a pickup event once awake
/// </summary>

public class itemBehaviour : MonoBehaviour
{
    [SerializeField] private itemStats item;
    public itemStats Item
    {
        get { return item; }
        set { item = value; }
    }

    public event EventHandler pickupEvent;

    public void CreateItem(itemStats newItem)
    {
        item = newItem;
        gameObject.name = item.itemName;
        if (item.itemEffects != null) 
        { 
            Instantiate(newItem.itemEffects, this.transform);
            PickUp();
        }
    }

    public void LoadItem(itemStats newItem)
    {
        item = newItem;
        gameObject.name = item.itemName;
        if (item.itemEffects != null)
        {
            Instantiate(newItem.itemEffects, this.transform);
        }
        pickupEvent = null;
    }

    public void PickUp()
    {
        pickupEvent?.Invoke(this, EventArgs.Empty);
    }

}
