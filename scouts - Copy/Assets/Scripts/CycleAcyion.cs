using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleAcyion : MonoBehaviour
{
    private bool DoneCooking { get; set; }
    private bool DoneEating { get; set; }


   


    public bool CheckConditionsCycle(int? typeofAction)
    {
        bool canDo=false;

        switch (typeofAction)
        {
            //cucinare
            case 0:
                if (!DoneCooking)
                {
                    canDo = true;
                    DoneCooking = true;
                }
                break;

            case 1:
                if (DoneCooking && !DoneEating)
                {
                    canDo = true;
                    DoneEating = true;
                }
                break;

            case 2:
                if (DoneCooking && DoneEating)
                {
                    canDo = true;
                    DoneEating = false;
                    DoneCooking = false;

                }
                break;
        }
       
        

        return canDo;
    }
}
