using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handleanimations : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetWalking(bool isWalking)
    {
        animator.SetBool("IsWalking", isWalking);
    }

    public void SetJumping(bool isJumping)
    {
        animator.SetBool("IsJumping", isJumping);
    }
    public void SetFalling(bool isFalling)
    {
        animator.SetBool("IsFalling", isFalling);
    }
    public void SetCrouching(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
    }
}
