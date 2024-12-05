using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    public float dashDistance = 5f; 
    public float dashDuration = 0.2f; 
    public float dashCooldown = 1f; 

    private CharacterController characterController;
    private bool isDashing = false;
    private float lastDashTime;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + dashCooldown && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;

        float dashEndTime = Time.time + dashDuration;
        Vector3 dashDirection = transform.forward; 

        while (Time.time < dashEndTime)
        {
            characterController.Move(dashDirection * (dashDistance / dashDuration) * Time.deltaTime);
            yield return null;
        }

        isDashing = false;
    }
}
