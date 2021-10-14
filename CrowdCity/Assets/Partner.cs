using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Partner : MonoBehaviour
{

    public GameObject myLeader;
    [SerializeField] NavMeshAgent myAgent;

    private void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (gameObject.tag=="AI")
        {
            myAgent.destination = myLeader.transform.position;

            float distance = Vector3.Distance(myLeader.transform.position, transform.position);
            if (distance >= myAgent.stoppingDistance)
            {
                GetComponent<Animator>().SetBool("Idle", false);
            }
            else GetComponent<Animator>().SetBool("Idle", true);
        }

    }
}
