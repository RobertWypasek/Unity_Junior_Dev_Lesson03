using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver = false;

    public float floatForce;
    private float playerAltitude;
    private float maxAltitude;
    private float gravityModifier = 1.5f;
    private float backgroundHeight;
    private float playerHeight;
    private Vector3 upperBorder;
    private Vector3 bottomBorder;

    private Rigidbody playerRb;
    public GameObject backgroundObject;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip bounceSound;



    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();

        playerRb = GetComponent<Rigidbody>();
        backgroundHeight = backgroundObject.GetComponent<BoxCollider>().size.y;
        playerHeight = this.GetComponent<BoxCollider>().size.y;
        maxAltitude = backgroundHeight - playerHeight * 3;
        upperBorder = new Vector3(0, maxAltitude, 0);
        bottomBorder = new Vector3(0, 0, 0);

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        playerAltitude = transform.position.y;
        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver && playerAltitude < maxAltitude)
        {
            playerRb.AddForce(Vector3.up * floatForce);
        }
        else if (playerAltitude > maxAltitude)
        {
            playerRb.position = upperBorder;
            playerRb.AddForce(Vector3.down, ForceMode.Impulse);
        }

        if (transform.position.y <= bottomBorder.y)
        {
            playerRb.AddForce(Vector3.up * 2, ForceMode.Impulse);
            playerAudio.PlayOneShot(bounceSound, 1.0f);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        }

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }

    }

}
