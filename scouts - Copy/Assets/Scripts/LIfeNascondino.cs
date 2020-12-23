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
    public float playerSpeed;
    float lastX, lastY;
    Animator animator;
    bool isMoving;
    public void Start()
    {
        lb = GameObject.Find("/GameManager").GetComponent<labirintoManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        bar = GameObject.Find("/Canvas/healthbar").GetComponent<HealthBar>();
        counterText = GameObject.Find("/Canvas/caramCounter/counter").GetComponent<TextMeshProUGUI>();
        counterText.text = nCaramelleTrovate.ToString();
        animator = GetComponent<Animator>();
        bar.Health(life);
    }

    //copied from the 'real' player script
    void FixedUpdate()
    {
        Vector3 movement = Joystick.instance.direction;
        transform.position = Vector3.Lerp(transform.position, transform.position + movement, Time.deltaTime * playerSpeed);
        if (isMoving)
        {
            transform.Find("WaterParticle").GetComponent<ParticleSystem>().Play();
        }

        animator.SetFloat("Speed", movement.sqrMagnitude);
        if (movement.sqrMagnitude >= 0.01)
        {

            Invoke("StepSounds", 0.5f);
            isMoving = true;
            animator.SetFloat("XMovement", movement.x);
            animator.SetFloat("YMovement", movement.y);
            lastX = movement.x;
            lastY = movement.y;
        }
        else
        {
            GameObject.Find("AudioManager").GetComponent<AudioManager>().Stop("walking");

            isMoving = false;
            animator.SetFloat("XMovement", lastX);
            animator.SetFloat("YMovement", lastY);
        }
    }
    public void StepSounds()
    {
        GameObject.Find("AudioManager").GetComponent<AudioManager>().Play("walking");

    }
    //end of copied stuff

    public void Spillo(Transform spilPos)
    {
        life -= spillo; 
        Transform pPos = this.gameObject.GetComponent<Transform>();
        if ((spilPos.position.x - pPos.position.x) > 0)
        {
            force.x = -(force.x);
        }

        if ((spilPos.position.y - pPos.position.y) > 0)
        {
            force.y = -(force.y);
        }
        GameObject.Find("/Player").transform.Find("sangue").gameObject.SetActive(true);
        bar.Health(life);
        if (life <= 0)
        {
            lb.StartCoroutine("Sconfitta");
        }
    }

    public void Dinamite(Transform dinPos)
    {
        life -= dinamite;
        Transform pPos = this.gameObject.GetComponent<Transform>();
        if ((dinPos.position.x - pPos.position.x) > 0)
        {
            force.x = -(force.x);
        }

        if ((dinPos.position.y - pPos.position.y) > 0)
        {
            force.y = -(force.y);
        }
        rb.AddForce(force);
        GameObject.Find("/Player").transform.Find("sangue").gameObject.SetActive(true);
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
