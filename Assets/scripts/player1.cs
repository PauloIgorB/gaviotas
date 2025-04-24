using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class player1 : MonoBehaviour
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

    private GameController gcPlayer1;

    // Start is called before the first frame update
    void Start()
    {
        gcPlayer1 = GameController.gc;
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

        if (gcPlayer1.redVidas < 1)
        {
            playerAnim.SetBool("V_dead", true);
            audioS.clip = sounds[1];
            audioS.Play();
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
            GetComponent<player1>().enabled = false;
            GetComponent<Animator>().SetBool("V_jump", false);
        }
        else
        {
            playerAnim.SetBool("V_dead", false);
        }
    }
    
    void movePlayer(){
        float horizontalMove = Input.GetAxisRaw("redMove");
        //Debug.Log(horizontalMove);
        //transform.position += new Vector3(horizontalMove * Time.deltaTime * speed, 0, 0);
        rbPlayer.velocity = new Vector2(horizontalMove * speed, rbPlayer.velocity.y);

        if (horizontalMove > 0)
        {
            playerAnim.SetBool("V_walk", true);
            sr.flipX = true;
        }

        else if (horizontalMove < 0)
        {
            playerAnim.SetBool("V_walk", true);
            sr.flipX = false;
        }
        else
        {
            playerAnim.SetBool("V_walk", false);
        }

    }

    void jump ()
        {
            if (Input.GetButtonDown("Legday"))
            {
                if (inFloor)
                {
                    audioS.clip = sounds[2];
                    audioS.Play();
                    rbPlayer.velocity = Vector2.zero;
                    playerAnim.SetBool("V_jump", true);
                    rbPlayer.AddForce(new Vector2(0, pulo), ForceMode2D.Impulse);
                    inFloor = false;
                    doubleJump = true;
                }
                else if (inFloor == false && doubleJump == true)
                {
                    audioS.clip = sounds[2];
                    audioS.Play();
                    rbPlayer.velocity = Vector2.zero;
                    playerAnim.SetBool("V_jump", true);
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
            playerAnim.SetBool("V_jump", false);
            inFloor = true;
            doubleJump = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "balaA")
        {
            Destroy(collision.gameObject);
            audioS.clip = sounds[0];
            audioS.Play();
            gcPlayer1.redVidas--;
            gcPlayer1.gavRLife.text = gcPlayer1.redVidas.ToString();
        }
    }

}
