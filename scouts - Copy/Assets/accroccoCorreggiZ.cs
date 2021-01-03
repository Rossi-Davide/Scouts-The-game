using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class accroccoCorreggiZ : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Aggiustino),0.1f);
    }

    // Update is called once per frame
    public void Aggiustino()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
