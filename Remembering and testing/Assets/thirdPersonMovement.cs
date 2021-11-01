using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdPersonMovement : MonoBehaviour
{
    // Update is called once per frame
    Vector3 direction;
    public Rigidbody rb;
    public float moveSpeed;
    private void FixedUpdate() 
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        direction = new Vector3(horizontal, 0, vertical);
        //calculate desired movement speed and direction
        Vector3 targetVector = direction * moveSpeed;

        //calculate diff between current and desired. 
        Vector3 diff = targetVector - rb.velocity;

    }
}
