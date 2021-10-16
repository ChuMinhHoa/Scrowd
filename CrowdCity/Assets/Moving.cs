using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    [SerializeField] float speed;
    Animator myAnim;
    Rigidbody myBody;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody>();
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        Vector2 movement = Vector2.zero;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        float angle = transform.localRotation.y;
        if (movement!=Vector2.zero)
        {
            movement = movement.normalized;
            myBody.velocity = new Vector3(movement.x * speed * Time.deltaTime, 0, movement.y * speed * Time.deltaTime);
            myAnim.SetBool("Idle",false);
            
            angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;

            transform.eulerAngles = new Vector3(0, -(angle - 90), 0);

        }
        else myAnim.SetBool("Idle", true);

        
        
        
    }
}
