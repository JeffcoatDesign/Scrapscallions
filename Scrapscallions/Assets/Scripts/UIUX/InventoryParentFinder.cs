using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryParentFinder : MonoBehaviour
{
    public GameObject inventoryParent;
    public InventoryManager im;
    private InventoryReload[] irs;
    private void Awake()
    {
        im = GameObject.FindWithTag("InventoryParent").GetComponent<InventoryManager>();
        im.inventoryParent = inventoryParent;
        irs = FindObjectsByType<InventoryReload>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (InventoryReload ir in irs)
            ir.inventoryManager = im;
    }
}