using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class arma : MonoBehaviour
{
    private SpriteRenderer srArma;
    public GameObject projetil;
    public AudioSource audioS;
    public AudioClip[] sounds;
    public float launchForce;
    public Transform shotPoint;
    private float currentRotation;
    public float minAngle = 0f;
    public float maxAngle = 90f;
    public float rotationSpeed = 100;
    public float ammoAtual = 8;
    public float ammoMax = 8;

    // Start is called before the first frame update
    void Start()
    {
        srArma = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("azulMira");
        float rotationAmount = verticalInput * rotationSpeed * Time.deltaTime;

        transform.Rotate(0,0,rotationAmount);

        currentRotation += rotationAmount;

        currentRotation = Mathf.Clamp(currentRotation, minAngle, maxAngle);

        transform.eulerAngles = new Vector3(0,0, currentRotation);

        if (Input.GetButtonDown("vira"))
        {
            srArma.flipX = true;
            launchForce = -20;
            rotationSpeed = -200;
        }
        else if (Input.GetButtonDown("desvira"))
        {
            srArma.flipX = false;
            launchForce = 20;
            rotationSpeed = 200;
        }

        if(Input.GetButtonDown("azulReload"))
        {
            audioS.clip = sounds[2];
            audioS.Play();
            Reload();
        }

        if(ammoAtual < 1 && Input.GetButtonDown("azulTiro"))
        {
            audioS.clip = sounds[1];
            audioS.Play();
            return;
        }

        if (Input.GetButtonDown("azulTiro"))
        {
            Shoot();
            audioS.clip = sounds[0];
            audioS.Play();
            ammoAtual--;
        }

        void Reload()
        {
            ammoAtual = ammoMax - ammoAtual;
        }

        void Shoot()
        {
            GameObject newProj = Instantiate(projetil, shotPoint.position, shotPoint.rotation);
            newProj.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;
        }
    }
}
