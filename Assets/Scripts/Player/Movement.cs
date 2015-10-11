using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;

[RequireComponent(typeof(AudioSource))]
public class Movement : MonoBehaviour {
   
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    public AudioClip small_jump;
	public AudioClip spawn_sound;
    public float period = 0.13f;
	public Mesh[] runFrames = new Mesh[4];
	public Mesh idleFrame;
	public Mesh jumpFrame;
    public Color pipeSpawnColor;
	public string pipeTag;
	public bool growTween;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;
    private Vector3 prevMove;
    private MeshFilter meshFilter;
    private HFTInput hftInput;
    private int flipDirection;
    private AudioSource audioSource;
    private float nextFrameChange = 0.0f;
    private bool isStanding;
    private int currentFrame;

    public void Start()
    {
       Int32.TryParse(System.Text.RegularExpressions.Regex.Replace(this.GetComponent<MeshFilter>().mesh.name, @"[^\d]", ""), out currentFrame);
       currentFrame -= 1;

       controller = GetComponent<CharacterController>();
       meshFilter = GetComponent<MeshFilter>();
       audioSource = GetComponent<AudioSource>();
       hftInput = GetComponent<HFTInput>();

       meshFilter.mesh = jumpFrame;
     
	   GameObject spawner = GameObject.FindWithTag(pipeTag);
	   transform.position = spawner.transform.position + spawner.transform.up * 2.0F;
		moveDirection.y = jumpSpeed * spawner.transform.up.y;
       spawner.transform.GetChild(0).GetComponent<Renderer>().material.DOColor(pipeSpawnColor, 0.5f);
        // set pipe spawn color
       //spawners[spawnInd].transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", pipeSpawnColor);

       // Grab a free Sequence to use
       Sequence pipeSequence = DOTween.Sequence();
		if (growTween) 
		{
			pipeSequence.Append(spawner.transform.DOScale(0.12f, 0.15f).SetEase(Ease.InOutBounce).SetLoops(1));
			pipeSequence.Append(spawner.transform.DOScale(0.08597419f, 0.5f).SetEase(Ease.InOutElastic).SetLoops(1));
		} 
		else 
		{
			pipeSequence.Append(spawner.transform.DOScale(0.05f, 0.5f).SetEase(Ease.InOutBounce).SetLoops(1));
			pipeSequence.Append(spawner.transform.DOScale(0.08597419f, 0.3f).SetEase(Ease.InOutElastic).SetLoops(1));
		}

		audioSource.PlayOneShot (spawn_sound);


      // spawners[spawnInd].transform.DOScale(spawners[spawnInd].transform.localScale.x, 0.2f)
     
       transform.DOScale(0.4f, 0.5f).SetEase(Ease.OutBounce).SetLoops(1);
       
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") > 0 || hftInput.GetAxis("Horizontal") > 0)
        {
            meshFilter.mesh = runFrames[3];
            nextFrameChange = 0.0f;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            isStanding = false;
        }
        else if (Input.GetAxis("Horizontal") < 0 || hftInput.GetAxis("Horizontal") < 0)
        {
            meshFilter.mesh = runFrames[3];
            nextFrameChange = 0.0f;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            isStanding = false;
        }
        else 
        {
            isStanding = true;
        }

        if (controller.isGrounded)
        {
            // standing still frame is now displayed
            if(isStanding)
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
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        if (Time.time > nextFrameChange){
            nextFrameChange += period;
            if (isStanding == false &&
                controller.isGrounded == true)
                {
                    runAnimation();
                }
            }
        }

    private void runAnimation()
    {
        if (currentFrame <= 1)
            currentFrame += 1;
        else
            currentFrame = 0;

        this.GetComponent<MeshFilter>().mesh = runFrames[currentFrame];
    }
}
