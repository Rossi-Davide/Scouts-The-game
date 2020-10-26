using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LIfeNascondino : MonoBehaviour
{
    public float life = 100f,spillo=5f,dinamite=10f,nCaramelleTrovate=0f;
    public Vector2 force;
    labirintoManager lb;
    Rigidbody2D rb;
    HealthBar bar;
    TextMeshProUGUI counterText;
    public void Start()
    {
        lb = GameObject.Find("/GameManager").GetComponent<labirintoManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        bar = GameObject.Find("/Canvas/healthbar").GetComponent<HealthBar>();
        counterText = GameObject.Find("/Canvas/caramCounter/counter").GetComponent<TextMeshProUGUI>();
        counterText.text = nCaramelleTrovate.ToString();
    }

    public void Spillo()
    {
        life -= spillo;
        rb.AddForce(force);
        bar.Health(life);
        if (life <= 0)
        {
            lb.StartCoroutine("Sconfitta");
        }
    }

    public void Dinamite()
    {
        life -= dinamite;
        bar.Health(life);
        if (life <= 0)
        {
            lb.StartCoroutine("Sconfitta");
        }
    }

    public void CaramelleCounter()
    {
        nCaramelleTrovate += 1;
        counterText.text = nCaramelleTrovate.ToString();
        if (nCaramelleTrovate >= 3)
        {
            lb.StartCoroutine("Vittoria");
        }
    }
}
