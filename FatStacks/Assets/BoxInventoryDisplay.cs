using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BoxInventoryDisplay : MonoBehaviour
{
    public Player player;
    public float spriteHeight;
    public LinkedList<GameObject> inventory = new LinkedList<GameObject>();
    private static readonly string[] resourcePaths = new string[4]
        {
        "Character/ArsenalSystem/BoxInventory/Prefabs/BlueBoxIcon",
        "Character/ArsenalSystem/BoxInventory/Prefabs/RedBoxIcon",
        "Character/ArsenalSystem/BoxInventory/Prefabs/GreenBoxIcon",
        "Character/ArsenalSystem/BoxInventory/Prefabs/YellowBoxIcon",
        };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddBox(Box.GroupIdNames type)
    {
        GameObject obj = (GameObject)Resources.Load(resourcePaths[(int)type]);
        inventory.AddLast(Instantiate(obj, transform));
        ArrangeBoxes();
    }

    public void RemoveBox()
    {
        GameObject obj = inventory.Last.Value;
        inventory.RemoveLast();
        Destroy(obj);
        ArrangeBoxes();
    }

    void ArrangeBoxes()
    {
        int i = 0;
        float top = (inventory.Count * spriteHeight) / 2;
        foreach(GameObject obj in inventory)
        {
            RawImage img = obj.GetComponent<RawImage>();
            img.rectTransform.localPosition = new Vector3(0, top - spriteHeight * i);
            ++i;
        }
    }
}
