using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Movement : MonoBehaviour {
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    public AudioClip small_jump;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private Vector3 prevMove;
    private Mesh idleFrame;
    private Mesh jumpFrame;
    private MeshFilter meshFilter;
    private HFTInput hftInput;
    private int flipDirection;
    private AudioSource audioSource;

    public void Start()
    {
       
       idleFrame = Resources.Load("Models/Mario/mario_idle", typeof(Mesh)) as Mesh;
       jumpFrame = Resources.Load("Models/Mario/mario_jump", typeof(Mesh)) as Mesh;
       controller = GetComponent<CharacterController>();
       meshFilter = GetComponent<MeshFilter>();
       audioSource = GetComponent<AudioSource>();
       hftInput = GetComponent<HFTInput>();

    }
    void Update()
    {
        // if moving to the right
        if (Input.GetAxis("Horizontal") > 0 || hftInput.GetAxis("Horizontal") > 0)
        {
            //transform.localEulerAngles = new Vector3(0, 180, 0);
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // if moving to the left
        if (Input.GetAxis("Horizontal") < 0 || hftInput.GetAxis("Horizontal") < 0)
        {
            //transform.localEulerAngles = new Vector3(0, 0, 0);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        if (controller.isGrounded)
        {
            meshFilter.mesh = idleFrame;
            moveDirection = new Vector3(hftInput.GetAxis("Horizontal") + Input.GetAxis("Horizontal"), 0, 0);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump") || hftInput.GetButtonDown("Fire1"))
            {
                audioSource.PlayOneShot(small_jump);
                meshFilter.mesh = jumpFrame;
                moveDirection.y = jumpSpeed;
            }
        }
        else
        {
            moveDirection = new Vector3(hftInput.GetAxis("Horizontal") + Input.GetAxis("Horizontal"), moveDirection.y, 0);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection.x *= speed;
            //moveDirection.z *= speed;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
