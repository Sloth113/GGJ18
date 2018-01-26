using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGraph  {

    public PowerSource source;

    public List<Tower> towers;


    public void CalculatePowerGraph()
    {
        foreach (Tower tower in towers)
        {
            tower.visited = false;
            tower.inStack = false;
        }
        source.CalculateChildren();
    }

    public void SupplyPower(float power)
    {
        foreach (Tower tower in towers)
        {
            tower.ResetPower();
        }
        source.AddPower(power);
    }
}
