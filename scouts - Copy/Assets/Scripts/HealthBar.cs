using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider sl;
    // Start is called before the first frame update
    public void Health(float health)
    {
        sl.value = health;
    }
}
