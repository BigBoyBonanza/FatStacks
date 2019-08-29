using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BoxInventoryDisplay : MonoBehaviour
{
    public Player player;
    public float spriteHeight;
    public LinkedList<GameObject> inventory = new LinkedList<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddBox(GameObject obj)
    {
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
