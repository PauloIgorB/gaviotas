using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private Animator playerAnim;
    private Rigidbody2D rbPlayer;
    private SpriteRenderer sr;
    public AudioSource audioS;
    public AudioClip[] sounds;
    public float speed = 3;
    public float pulo = 300;
    public bool inFloor = true;
    public bool doubleJump = true;

    private GameController gcPlayer;

    // Start is called before the first frame update
    void Start()
    {
        gcPlayer = GameController.gc;
        gcPlayer.azulVidas = 10;
        playerAnim  = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rbPlayer = GetComponent<Rigidbody2D>();

    }

    private void FixedUpdate()
    {
        movePlayer();
    }

    // Update is called once per frame
    void Update()
    {
        jump();

        if (gcPlayer.azulVidas < 1)
        {
            playerAnim.SetBool("dead", true);
            audioS.clip = sounds[1];
            audioS.Play();
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            GetComponent<player>().enabled = false;
            GetComponent<Animator>().SetBool("jump", false);
        }
        else
        {
            playerAnim.SetBool("dead", false);
        }
    }
    
    void movePlayer(){
        float horizontalMove = Input.GetAxisRaw("azulMove");
        //Debug.Log(horizontalMove);
        //transform.position += new Vector3(horizontalMove * Time.deltaTime * speed, 0, 0);
        rbPlayer.velocity = new Vector2(horizontalMove * speed, rbPlayer.velocity.y);

        if (horizontalMove > 0)
        {
            playerAnim.SetBool("walk", true);
            sr.flipX = false;
        }

        else if (horizontalMove < 0)
        {
            playerAnim.SetBool("walk", true);
            sr.flipX = true;
        }
        else
        {
            playerAnim.SetBool("walk", false);
        }
    }

    void jump ()
        {
            if (Input.GetButtonDown("Jump"))
            {
                if (inFloor)
                {
                    audioS.clip = sounds[2];
                    audioS.Play();
                    rbPlayer.velocity = Vector2.zero;
                    playerAnim.SetBool("jump", true);
                    rbPlayer.AddForce(new Vector2(0, pulo), ForceMode2D.Impulse);
                    inFloor = false;
                    doubleJump = true;
                }
                else if (inFloor == false && doubleJump == true)
                {
                    audioS.clip = sounds[2];
                    audioS.Play();
                    rbPlayer.velocity = Vector2.zero;
                    playerAnim.SetBool("jump", true);
                    rbPlayer.AddForce(new Vector2(0, pulo), ForceMode2D.Impulse);
                    inFloor = false;
                    doubleJump = false;
                }
            }
        }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "superficie")
        {
            playerAnim.SetBool("jump", false);
            inFloor = true;
            doubleJump = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "balaV")
        {
            Destroy(collision.gameObject);
            audioS.clip = sounds[0];
            audioS.Play();
            gcPlayer.azulVidas--;
            gcPlayer.gavALife.text = gcPlayer.azulVidas.ToString();
        }
    }

}
