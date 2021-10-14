using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeader : MonoBehaviour
{

    [SerializeField] GameObject[] parners;
    [SerializeField] Material myMaterial;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag=="normal")
        {
            SkinnedMeshRenderer skin = collision.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
            skin.materials[0].color = myMaterial.color;
            collision.gameObject.tag = "AI";
            collision.gameObject.GetComponent<Partner>().myLeader = gameObject;
        }

        if (collision.gameObject.tag=="Enemy")
        {
            //Check count
        }
    }


}
