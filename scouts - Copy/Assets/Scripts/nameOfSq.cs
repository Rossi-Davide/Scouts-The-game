using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class nameOfSq : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       

        gameObject.GetComponent<TextMeshProUGUI>().text = "Squadriglia: "+Player.instance.squadriglia.name;
    }

    
}
