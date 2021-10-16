using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class EnemyAI : Partner
{

    Material material;
    public float viewFarCombat, viewFarPartner;
    public LayerMask whatIsCombat;
    public LayerMask whatIsPartner;

    public delegate void OnPartnerChange();
    public OnPartnerChange onPartnerChangedCallback;
    public bool changeColor;

    [SerializeField] Image countView;

    private void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();
        myanim = GetComponent<Animator>();
        onPartnerChangedCallback += UpdatePartner;
    }
    // Start is called before the first frame update
    void Start()
    {
        material = GetComponentInChildren<SkinnedMeshRenderer>().materials[0];
        ChangeMyColor();
    }

    void ChangeMyColor() {
        bool flag = false;
        
        ColorManager cManager = ColorManager.instance;
        
        if (cManager==null)
        {
            cManager = ColorManager.instance;
        }

        Color newColor = new Color(0, 0, 0);
        float r = Random.Range(0, 121) / 255f;
        float g = Random.Range(0, 121) / 255f;
        float b = Random.Range(0, 121) / 255f;
        newColor = new Color(r, g, b);

        flag = cManager.CheckColor(newColor);
        if (flag )
        {
            if (changeColor)
            {
                material.color = newColor;
                cManager.AddColor(newColor);
                countView.color = newColor;
            }
            
        }
        else {
            ChangeMyColor();
            return;
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void FixedUpdate()
    {
        GameObject playerInView = ViewCheck(whatIsCombat, viewFarCombat);
        GameObject partnerInView = ViewCheck(whatIsPartner, viewFarPartner);
        if (playerInView != null && status != StageStatus.Gopartner)
        {

            myLeader = playerInView;
            myAgent.stoppingDistance = 1f;

            myAgent.destination = myLeader.transform.position;

            SetAnim(myLeader.transform.position);
            status = StageStatus.GoCombat;
        }
        else
        if (partnerInView != null)
        {
            myAgent.stoppingDistance = 0.1f;
            
            myLeader = partnerInView;
            myAgent.destination = myLeader.transform.position;

            SetAnim(myLeader.transform.position);
            status = StageStatus.Gopartner;
        }
        else {
            RandomMove();
        }
    }

    void UpdatePartner()
    {
        GetComponentInChildren<ViewCount>().ChangeText(myPartners.Count);
        for (int i = 0; i < myPartners.Count; i++)
        {
            myPartners[i].GetComponent<Partner>().myPartners = myPartners;
        }
    }


    public GameObject ViewCheck(LayerMask whatIsFinding, float radius) {
        Collider[] hits = Physics.OverlapSphere(transform.position,radius, whatIsFinding);

        if (hits.Length>0)
        {
            float minDistance = Vector3.Distance(transform.position, hits[0].transform.position);
            myLeader = hits[0].gameObject;

            for (int i = 0; i < hits.Length; i++)
            {
                float distance = Vector3.Distance(transform.position, hits[i].transform.position);
                if (distance<minDistance)
                {
                    minDistance = distance;
                    myLeader = hits[i].gameObject;
                }
            }
            return myLeader;
        }

        return null;
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "normal")
        {
            ChangePartnerToMind(collision.gameObject); 
        }

        if (collision.gameObject.tag == "Enemy" && myPartners.IndexOf(collision.gameObject) == -1)
        {
            int targetCount = collision.gameObject.GetComponent<Partner>().myPartners.Count;
            if (targetCount < myPartners.Count)
            {
                Partner target = collision.gameObject.GetComponent<Partner>();
                if (target.myLeader != null)
                {
                    target.myLeader.GetComponent<EnemyAI>().myPartners.Remove(collision.gameObject);
                }

                ChangePartnerToMind(collision.gameObject);
            }
        }

        if (collision.gameObject.tag == "PlayerLeader")
        {
            int targetCount = collision.gameObject.GetComponent<Leader>().myPartners.Count;
            if (targetCount == 0)
            {
                Destroy(collision.gameObject);
            }
        }
    }

    public void ChangePartnerToMind(GameObject _partner) {
        if (myPartners.IndexOf(_partner) == -1)
        {
            myPartners.Add(_partner);

            if (onPartnerChangedCallback != null)
                onPartnerChangedCallback.Invoke();

            _partner.tag = "Enemy";
            _partner.layer = 7;
            _partner.GetComponent<Partner>().myLeader = gameObject;

            SkinnedMeshRenderer mySkin = GetComponentInChildren<SkinnedMeshRenderer>();
            _partner.GetComponentInChildren<SkinnedMeshRenderer>().materials[0] = mySkin.material;
            _partner.GetComponent<Partner>().RemoveMeFromSpawn();
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,viewFarCombat);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,viewFarPartner);
    }
}

