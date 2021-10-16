using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHighLevel : EnemyAI
{

    [SerializeField] GameObject myCombaterChoice;
    [SerializeField] List<GameObject> combaters;

    [SerializeField] bool enemyOnview = false;
    public override void FixedUpdate()
    {
        
        GameObject partnerInView = ViewCheck(whatIsPartner, viewFarPartner);

        combaters.Clear();
        ViewCheckCombater();
        CheckCount();
        if (combaters.Count != 0 && myCombaterChoice != null)
        {
            GoTarget(myCombaterChoice, .1f, StageStatus.GoCombat);
        }
        else
        if (partnerInView != null && status != StageStatus.GoCombat)
            GoTarget(partnerInView, 0.1f, StageStatus.Gopartner);
        else
            RandomMove();
    }

    void CheckCount() {
        if (combaters.Count>0)
        {
            int min = combaters[0].GetComponent<Leader>().myPartners.Count;
            myCombaterChoice = combaters[0];
            for (int i = 1; i < combaters.Count; i++)
            {
                int ccount = combaters[i].GetComponent<Leader>().myPartners.Count;
                if (ccount < min)
                {
                    min = ccount;
                    myCombaterChoice = combaters[i];
                }
            }

            if (min >= myPartners.Count)
            {
                myCombaterChoice = null;
                status = StageStatus.Idle;
            }
        }
       
    } 

    void GoTarget(GameObject leader,float distance,StageStatus _status) {
        myLeader = leader;
        myAgent.stoppingDistance = distance;

        myAgent.destination = myLeader.transform.position;

        SetAnim(myLeader.transform.position);
        status = _status;
    }

    void ViewCheckCombater() {
        Collider[] hits = Physics.OverlapSphere(transform.position, viewFarCombat, whatIsCombat);
        for (int i = 0; i < hits.Length; i++)
        {
            if (myPartners.IndexOf(hits[i].gameObject) == -1 && hits[i].gameObject != gameObject)
                combaters.Add(hits[i].gameObject);
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }

}
