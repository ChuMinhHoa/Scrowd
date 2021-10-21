using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    [SerializeField] float speed, rotageSpeed;
    Animator myAnim;
    Rigidbody myBody;

    Vector3 movement;

    public Joystick js;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody>();
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Manager.instance.StopNow)
        {
            myAnim.SetBool("Idle", false);
            JoyStickMove();

            movement = movement.normalized;
            if (movement != Vector3.zero)
            {
                Quaternion toRotate = Quaternion.LookRotation(movement, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, Time.deltaTime * rotageSpeed);
            }

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        

    }

    void JoyStickMove() {
        float x = js.Horizontal;
        float y = js.Vertical;
            movement.x = x;
            movement.z = y;
    }
}
