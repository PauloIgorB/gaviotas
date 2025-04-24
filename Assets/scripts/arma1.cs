using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arma1 : MonoBehaviour
{
    private SpriteRenderer srArma1;
    public GameObject projetil;
    public AudioSource audioS;
    public AudioClip[] sounds;
    public float launchForce;
    public Transform shotPointV;
    private float currentRotation;
    public float minAngle = 90f;
    public float maxAngle = 270f;
    public float rotationSpeed = 200;
    public float ammoAtual = 8;
    public float ammoMax = 8;

    // Start is called before the first frame update
    void Start()
    {
        srArma1 = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("redMira");
        float rotationAmount = verticalInput * rotationSpeed * Time.deltaTime;

        transform.Rotate(0,0,rotationAmount);

        currentRotation += rotationAmount;

        currentRotation = Mathf.Clamp(currentRotation, minAngle, maxAngle);

        transform.eulerAngles = new Vector3(0,0, currentRotation);

        if (Input.GetButtonDown("flipa"))
        {
            srArma1.flipX = true;
            launchForce = 20;
            rotationSpeed = 200;
        }
        else if (Input.GetButtonDown("desflipa"))
        {
            srArma1.flipX = false;
            launchForce = -20;
            rotationSpeed = -200;
        }

        if(Input.GetMouseButtonDown(1))
        {
            Reload();
            audioS.clip = sounds[2];
            audioS.Play();
        }

        if (ammoAtual < 1 && Input.GetMouseButtonDown(0))
        {
            audioS.clip = sounds[1];
            audioS.Play();
            return;
        }

        if(Input.GetMouseButtonDown(0))
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
            GameObject newProj = Instantiate(projetil, shotPointV.position, shotPointV.rotation);
            newProj.GetComponent<Rigidbody2D>().velocity = transform.right * launchForce;
        }
    }
}
