using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	public CharacterController controller;

	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
	bool crouch = false;

	public Animator animator;
	void Update()
	{
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
		animator.SetBool("isJump", !controller.m_Grounded);
		animator.SetBool("isRun", !(Input.GetAxisRaw("Horizontal") == 0));

		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
		}
	}
	
	void FixedUpdate()
	{
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
		jump = false;
	}
}
