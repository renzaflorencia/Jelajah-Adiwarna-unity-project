using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueprint 
{
    public string itemName;

    public string Reg1;
    public string Reg2;

    public int Reg1amount;
    public int Reg2amount;

    public int numOfRequirements;

    public int numberOfItems;

    public Blueprint(string name,int producedItems, int regNUM, string R1, int R1num, string R2, int R2num)
    {
        itemName = name;
        numOfRequirements = regNUM;
        numberOfItems = producedItems;


        Reg1 = R1;
        Reg2 = R2;

        Reg1amount = R1num;
        Reg2amount = R2num;
    }
}
