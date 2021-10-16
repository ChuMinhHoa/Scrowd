using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    [SerializeField] float speed;
    Animator myAnim;
    Rigidbody myBody;

    Vector2 movement;

    public Joystick js;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody>();
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        movement = Vector2.zero;
        MoveWithoutJS();
        JoyStickMove();
        Debug.Log(movement);
        float angle = transform.localRotation.y;
        if (movement != Vector2.zero)
        {
            movement = movement.normalized;
            myBody.velocity = new Vector3(movement.x * speed * Time.deltaTime, 0, movement.y * speed * Time.deltaTime);
            myAnim.SetBool("Idle", false);

            angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;

            transform.eulerAngles = new Vector3(0, -(angle - 90), 0);

        }
        else myAnim.SetBool("Idle", true);




    }

    void MoveWithoutJS() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        if (x != 0 || y != 0)
        {
            movement.x = x;
            movement.y = y;
        }
    }

    void JoyStickMove() {
        float x = js.Horizontal;
        float y = js.Vertical;
        if (x != 0 || y != 0)
        {
            movement.x = x;
            movement.y = y;
        }
    }
}
