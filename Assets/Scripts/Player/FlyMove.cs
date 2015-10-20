using UnityEngine;
using System.Collections;
using System;

public class FlyMove : MonoBehaviour {
	
	public float speed = 6.0F;
	public Mesh basicFrame;
	public Mesh specialFrame;
	public float leftBound, rightBound, upperBound, lowerBound;
	
	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;
	private Vector3 prevMove;
	private MeshFilter meshFilter;
	private HFTInput hftInput;
	
	public void Start()
	{	
		controller = GetComponent<CharacterController>();
		meshFilter = GetComponent<MeshFilter>();
		hftInput = GetComponent<HFTInput>();
		
		meshFilter.mesh = basicFrame;

		transform.position = new Vector3(leftBound + UnityEngine.Random.value * (rightBound - leftBound),
		                             lowerBound + UnityEngine.Random.value * (upperBound - lowerBound), 2);

	}
	
	void Update()
	{
		if (Input.GetAxis("Horizontal") > 0 || hftInput.GetAxis("Horizontal") > 0)
		{
			transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
		}
		else if (Input.GetAxis("Horizontal") < 0 || hftInput.GetAxis("Horizontal") < 0)
		{
			transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
		}

			moveDirection = new Vector3(hftInput.GetAxis("Horizontal") + Input.GetAxis("Horizontal"), 
			                            -hftInput.GetAxis("Vertical") - Input.GetAxis("Vertical"), 0);
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;

		controller.Move(moveDirection * Time.deltaTime);

		if (transform.position.x < leftBound) 
		{
			transform.position = new Vector3(leftBound, transform.position.y, 2);
		}
		else if(transform.position.x > rightBound)
		{
			transform.position = new Vector3(rightBound, transform.position.y, 2);
		}
		else if(transform.position.y < lowerBound)
		{
			transform.position = new Vector3(transform.position.x, lowerBound, 2);
		}
		else if(transform.position.y > upperBound)
		{
			transform.position = new Vector3(transform.position.x, upperBound, 2);
		}

	}
}
