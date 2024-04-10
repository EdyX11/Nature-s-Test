using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint 
{

    [Header("Item Name")]
    [SerializeField] public string itemName;

    [Header("Requirements")]
    [SerializeField] public string Req1;
    [SerializeField] public string Req2;
    [SerializeField] public int Req1amount;
    [SerializeField] public int Req2amount;
    [SerializeField] public int numOfRequirements;
    [SerializeField] public int numberOfItemsToProduce;


    public Blueprint(string name, int producedItems, int reqNUM, string R1, int R1num, string R2, int R2num ) // constructor
    {
        itemName = name;

        numOfRequirements = reqNUM;
        numberOfItemsToProduce = producedItems;


        Req1 = R1;
        Req2 = R2;

        Req1amount = R1num;
        Req2amount = R2num;

    }



}
