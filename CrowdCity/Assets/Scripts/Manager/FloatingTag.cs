using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingTag : MonoBehaviour
{
    public Transform lookAt;
    [SerializeField] Vector3 offset,pos;
    Camera cam;


    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] Vector2 maxPos, minPos;

    public void ChangeText(int count)
    {
        textMesh.text = count.ToString();
    }


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        RectTransform myRect = GetComponent<RectTransform>();

        maxPos.x = Screen.width-(myRect.rect.width/2);
        maxPos.y = Screen.height - (myRect.rect.height / 2);
        minPos.x = 0 + (myRect.rect.width / 2);
        minPos.y = 0 + (myRect.rect.height / 2);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       
        if (lookAt != null)
        {
            pos = cam.WorldToScreenPoint(lookAt.position + offset);
            CheckOutSite();
            transform.position = pos;


        }
        else Destroy(gameObject);
        
    }

    void CheckOutSite() {
        
        
        if (pos.x > maxPos.x)
        {
            pos.x = maxPos.x;
        }

        if (pos.y > maxPos.y)
        {
            pos.y = maxPos.y;
        }

        ///
        if (pos.y < minPos.y)
        {
            pos.y = minPos.y;
        }

        if (pos.x < minPos.x)
        {
            pos.x = minPos.x;
        }
    }
}
