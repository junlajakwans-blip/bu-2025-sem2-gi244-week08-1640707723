using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 10f;
    public float gravityMultiplier = 1f;
    public bool gameOver = false;
    private Rigidbody rb;
    private InputAction jumpAction;
    // 5.8 add audio source variable to play crash sound
    private AudioSource audioSource;
    public AudioClip audioJump;
    public AudioClip audioCrash;


    public Animator animator;
    public ParticleSystem fxDirt;  
    public GameObject fxExplosionPrefab;
    private bool isOnGround = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        jumpAction = InputSystem.actions.FindAction("Jump");

        // 5.8 get audio source component, if not exist, add one
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Physics.gravity *= gravityMultiplier;
        animator.SetFloat("Speed_f", 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            return;
        }

        if (jumpAction.triggered && isOnGround)
        {
            rb.AddForce(jumpForce * Vector3.up, ForceMode.Impulse);
            isOnGround = false;
            animator.SetTrigger("Jump_trig");
             fxDirt.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
             audioSource.PlayOneShot(audioJump);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            fxDirt.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            gameOver = true;
            animator.SetBool("Death_b", true);
            animator.SetInteger("DeathType_int", 2);
             fxDirt.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            Instantiate(fxExplosionPrefab, transform.position, Quaternion.identity);
            
        }
        audioSource.PlayOneShot(audioCrash);
    }
}
