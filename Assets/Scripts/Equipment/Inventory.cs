using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /*
    public int inventorySize;
    public List<Item> items = new List<Item>();

    [HideInInspector] public Item equippedItem;
    [HideInInspector] public int equippedItemIndex;

    public ObjectRunBob itemRunBobManager;

    public float throwForce;
    public float throwTorqueForce;
    private void Start()
    {
        Cache.inventory = this;
        Cache.itemRunBobManager = itemRunBobManager;
        equippedItemIndex = -1;
        Cache.walkSpeedManager.AddValue("EquippedItem", 1f);

        foreach (Item item in items)
        {
            item.OnEquip();
        }
    }
    public void PickupItem(GameObject item)
    {
        if (items.Count < inventorySize)
        {
            GameObject pickup = (GameObject)Instantiate(item, transform.position, Quaternion.identity, transform);
            pickup.transform.localRotation = Quaternion.identity;
            Item pickupItem = pickup.GetComponent<Item>();
            items.Add(pickupItem);
            pickup.name += items.Count.ToString();
            EquipItem(items.Count - 1);
        }
    }
    public void EquipItem(int index)
    {
        if (index < items.Count && index != equippedItemIndex)
        {
            if (items[index] != null)
            {
                UnequipItem();
                equippedItemIndex = index;
                equippedItem = items[index];
                items[index].OnEquip();
            }
        }
    }
    public void ThrowItem(int index)
    {
        if (equippedItemIndex >= 0)
        {
            if (index < items.Count)
            {
                if (items[index] != null)
                {
                    if (index == equippedItemIndex) { UnequipItem(); }
                    GameObject thrownItem = (GameObject)Instantiate(items[index].thrownItem, Camera.main.transform.position, items[index].transform.rotation);
                    Rigidbody rb = thrownItem.GetComponent<Rigidbody>();
                    rb.AddTorque(Utilities.RandomRangeVector3(Utilities.V3All(-throwTorqueForce), Utilities.V3All(throwTorqueForce)));
                    float force = throwForce * Mathf.Clamp(FragMovementListener.hSpeedPercentage > 1f ? ((FragMovementListener.hSpeedPercentage - 1f) / 3.33f) + 1f : FragMovementListener.hSpeedPercentage, 0.75f, 99f);
                    rb.AddForce(Camera.main.transform.forward * force);
                    Destroy(items[index].gameObject);
                    items.RemoveAt(index);
                }
            }
        }
    }
    public void ClearHeldItem()
    {
        ClearItem(equippedItemIndex);
    }
    public void ClearItem(int index)
    {
        if (equippedItemIndex >= 0)
        {
            if (index < items.Count)
            {
                if (items[index] != null)
                {
                    if (index == equippedItemIndex) { UnequipItem(); }
                    Destroy(items[index].gameObject);
                    items.RemoveAt(index);
                }
            }
        }
    }
    public void ClearAllItems()
    {
        for (int i = 0; i < items.Count; i++)
        {
            ClearItem(i);
        }
    }
    public void UnequipItem()
    {
        if (equippedItem != null)
        {
            items[equippedItemIndex].OnUnequip();
            equippedItem = null;
            equippedItemIndex = -1;
            Cache.walkSpeedManager.UpdateValue("EquippedItem", 1f);
        }
    }
    private void Update()
    {
        int targetSlotIndex = equippedItemIndex;
        if (Input.GetKeyDown(KeyCode.Alpha1)) { targetSlotIndex = 0; }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) { targetSlotIndex = 1; }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) { targetSlotIndex = 2; }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) { targetSlotIndex = 3; }
        else if (Input.GetKeyDown(KeyCode.Alpha5)) { targetSlotIndex = 4; }
        else if (Input.GetKeyDown(KeyCode.Alpha6)) { targetSlotIndex = 5; }
        else if (Input.GetKeyDown(KeyCode.Alpha7)) { targetSlotIndex = 6; }
        else if (Input.GetKeyDown(KeyCode.Alpha8)) { targetSlotIndex = 7; }
        else if (Input.GetKeyDown(KeyCode.Alpha9)) { targetSlotIndex = 8; }
        if (items.Count > 0)
        {
            if (Input.mouseScrollDelta.y > 0) { targetSlotIndex++; if (targetSlotIndex > items.Count - 1) { targetSlotIndex = 0; } }
            if (Input.mouseScrollDelta.y < 0) { targetSlotIndex--; if (targetSlotIndex < 0) { targetSlotIndex = items.Count - 1; } }
        }
        if (targetSlotIndex <= items.Count) { EquipItem(targetSlotIndex); }

        if (Input.GetKeyDown(KeyCode.Q)) { UnequipItem(); }
        if (Input.GetKeyDown(KeyCode.G)) { ThrowItem(equippedItemIndex); }
    }
     * */

    // 0 - Primary Firearm
    // 1 - Secondary Firearm
    // 2 - Melee
    public Item[] items = new Item[3];

    public int equippedItemIndex { get; private set; } // -1 for none
    public Item equippedItem { get; private set; }

    void Start()
    {
        equippedItemIndex = -1;
    }

    public void Update()
    {
        GetInputs();
    }
    void GetInputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipItem(0);
            EquipItem(1);
            EquipItem(2);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UnequipCurrentItem();
        }
    }
    public void UnequipCurrentItem()
    {
        if (equippedItemIndex != -1)
        {
            equippedItem.OnUnequip();
            equippedItem = null;
            equippedItemIndex = -1;
        }
    }
    public void EquipItem(int index)
    {
        if (items[index] != null)
        {
            UnequipCurrentItem();
            equippedItem = items[index];
            equippedItemIndex = index;
            items[index].OnEquip();
        }

    }
}
