using Assets.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private PlayerScript player;
    private int lastItemCount = 0;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerScript>();
        
    }

    void Update()
    {
        if (lastItemCount < player.items.Count)
        {
            foreach (Item i in player.items)
            {
                if (i.name.EndsWith("LoveLetter")) GameObject.Find("ItemLoveLetter").GetComponent<CanvasGroup>().alpha = 1;
                else if (i.name.EndsWith("Toolbox")) GameObject.Find("ItemToolbox").GetComponent<CanvasGroup>().alpha = 1;
            }
            lastItemCount = player.items.Count;
            return;
        }
    }


}
