using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnEnemies : MonoBehaviour
{

    public void onClickTransformToJohn()
    {
        GameObject.Find("Player").BroadcastMessage("onClickTransformToJohn");
    }

    public void onClickTransformToRob()
    {
        GameObject.Find("Player").BroadcastMessage("onClickTransformToRob");
    }

    public void onClickTransformToSoldier()
    {
        GameObject.Find("Player").BroadcastMessage("onClickTransformToSoldier");
    }

    public void onClickTransformToHerbert()
    {
        GameObject.Find("Player").BroadcastMessage("onClickTransformToHerbert");
    }
}
