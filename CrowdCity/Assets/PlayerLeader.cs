using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeader : Leader
{
    [SerializeField] Material myMaterial;


    public delegate void OnPartnerChange();
    public OnPartnerChange onPartnerChangedCallback;
    // Start is called before the first frame update
    void Start()
    {
        onPartnerChangedCallback += UpdatePartner;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdatePartner() {
        GetComponent<ViewCount>().ChangeText(myPartners.Count);
        for (int i = 0; i < myPartners.Count; i++)
        {
            Partner partner = myPartners[i].GetComponent<Partner>();
            partner.myPartners = myPartners;
            float distance = i/6;
            partner.myAgent.stoppingDistance = 0.2f * ( distance + 1 );
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag=="normal")
        {
            SkinnedMeshRenderer skin = collision.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
            skin.materials[0].color = myMaterial.color;
            collision.gameObject.tag = "PlayerBro";
            collision.gameObject.layer = 9;
            Partner partner = collision.gameObject.GetComponent<Partner>();
            partner.myLeader = gameObject;
            partner.RemoveMeFromSpawn();
            myPartners.Add(collision.gameObject);
            if (onPartnerChangedCallback != null)
                onPartnerChangedCallback.Invoke();
        }

        if (collision.gameObject.tag=="Enemy")
        {
            //Check count
            int targetCount = collision.gameObject.GetComponent<Partner>().myPartners.Count;
            if (targetCount < myPartners.Count)
            {
                Partner target = collision.gameObject.GetComponent<Partner>();
                if (target.myLeader != null)
                {
                    target.myLeader.GetComponent<EnemyAI>().myPartners.Remove(collision.gameObject);
                }

                ChangeToBePlayerLeader(collision.gameObject);
            }
        }

        //EnemyLeader
        if (collision.gameObject.tag == "EnemyLeader")
        {
            int targetCount = collision.gameObject.GetComponent<Leader>().myPartners.Count;
            if (targetCount == 0)
            {
                Destroy(collision.gameObject);
            }

        }
    }
    void ChangeToBePlayerLeader(GameObject _partner)
    {
        List<GameObject> myLeaderPartner = myPartners;
        if (myLeaderPartner.IndexOf(_partner) == -1)
        {
           myPartners.Add(_partner);

            if (onPartnerChangedCallback != null)
                onPartnerChangedCallback.Invoke();

            _partner.tag = "PlayerBro";
            _partner.layer = 9;
            _partner.GetComponent<Partner>().myLeader = gameObject;
            _partner.GetComponent<Partner>().RemoveMeFromSpawn();
        }
    }

}
