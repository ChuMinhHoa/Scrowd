using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Partner : Leader
{

    public GameObject myLeader;
    public NavMeshAgent myAgent;
    public float minRange, maxRange;
    public float decisionTime, decisionTimeSetting;
    public Vector3 movement, offset;
    public GameObject mySpawn;

    public StageStatus status;

    public Animator myanim;
    

    private void Awake()
    {
        myAgent = GetComponent<NavMeshAgent>();
        myanim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!Manager.instance.StopNow)
        {
            if (gameObject.tag != "normal")
            {

                movement.x = myLeader.transform.position.x;
                movement.z = myLeader.transform.position.z;

                myAgent.speed = 6f;

                myAgent.destination = movement;
                myAgent.transform.rotation = myLeader.transform.rotation;
                SkinnedMeshRenderer skin = myLeader.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
                GetComponentInChildren<SkinnedMeshRenderer>().materials[0].color = skin.materials[0].color;

            }
            else
            {
                myAgent.speed = 1.5f;
                RandomMove();
            }
        }
        else myAgent.speed = 0f;
        
        
    }



    public void RandomMove() {
        
        if (decisionTime <= 0)
        {
            movement.x = RandomPosition(transform.position.x);
            movement.z = RandomPosition(transform.position.z);

            decisionTime = decisionTimeSetting;
        }
        else {
            decisionTime -= Time.deltaTime;
        }

        myAgent.destination = movement;

        SetAnim(movement);
        status = StageStatus.RandomMove;
       
    }

    public void SetAnim(Vector3 target) {
        float distance = Vector3.Distance(target, transform.position);
        if (distance > myAgent.stoppingDistance)
        {
            myanim.SetBool("Idle", false);
        }
        else { 
            myanim.SetBool("Idle", true);
            status = StageStatus.Idle;
        }
    }

    public float RandomPosition(float posi) {
        int decision = Random.Range(0, 2);

        if (decision == 0)
        {
            return posi + Random.Range(minRange, maxRange);
        }
        else return posi - Random.Range(minRange, maxRange);
    }

    public void RemoveMeFromSpawn() {
        if (mySpawn!=null)
        {
            if (mySpawn.GetComponent<Spawn>().normalESpawneds.IndexOf(gameObject) != -1)
            {
                mySpawn.GetComponent<Spawn>().normalESpawneds.Remove(gameObject);
            }
        }
    }


    public virtual void OnCollisionEnter(Collision collision)
    {
        //partner Enemy
        if (collision.gameObject.tag=="normal" && gameObject.tag=="Enemy")
        {
            ChangeToBeLeaderMind(collision.gameObject);
        }

        if (collision.gameObject.tag=="AI" && gameObject.tag=="Enemy")
        {

            int targetCount = collision.gameObject.GetComponent<Leader>().myPartners.Count; 
            if (targetCount < myLeader.GetComponent<Leader>().myPartners.Count)
            {
                ChangeToBeLeaderMind(collision.gameObject);
            }
        }

        if (collision.gameObject.tag == "Enemy" && gameObject.tag =="Enemy")
        {

            int targetCount = collision.gameObject.GetComponent<Partner>().myPartners.Count;
            if (targetCount < myPartners.Count)
            {
                Partner target = collision.gameObject.GetComponent<Partner>();
                if (target.myLeader!=null)
                {
                    target.myLeader.GetComponent<EnemyAI>().myPartners.Remove(collision.gameObject);
                    if (target.myLeader.GetComponent<EnemyAI>().onPartnerChangedCallback != null)
                        target.myLeader.GetComponent<EnemyAI>().onPartnerChangedCallback.Invoke();
                }
                
                ChangeToBeLeaderMind(collision.gameObject);
            }
        }

        if (collision.gameObject.tag == "PlayerBro" && gameObject.tag == "Enemy") {
            int targetCount = collision.gameObject.GetComponent<Partner>().myPartners.Count;
            if (targetCount < myPartners.Count)
            {
                Partner target = collision.gameObject.GetComponent<Partner>();
                if (target.myLeader != null)
                {
                    target.myLeader.GetComponent<PlayerLeader>().myPartners.Remove(collision.gameObject);
                    if (target.myLeader.GetComponent<PlayerLeader>().onPartnerChangedCallback != null)
                        target.myLeader.GetComponent<PlayerLeader>().onPartnerChangedCallback.Invoke();
                }

                ChangeToBeLeaderMind(collision.gameObject);
            }
        }

        //partnerPlayer
        if (collision.gameObject.tag=="Enemy" && gameObject.tag=="PlayerBro")
        {
            int targetCount = collision.gameObject.GetComponent<Partner>().myPartners.Count;
            if (targetCount < myPartners.Count)
            {
                Partner target = collision.gameObject.GetComponent<Partner>();
                if (target.myLeader != null)
                {
                    target.myLeader.GetComponent<EnemyAI>().myPartners.Remove(collision.gameObject);
                    if (target.myLeader.GetComponent<EnemyAI>().onPartnerChangedCallback != null)
                        target.myLeader.GetComponent<EnemyAI>().onPartnerChangedCallback.Invoke();
                }

                ChangeToBePlayerLeader(collision.gameObject);
            }
        }

        if (collision.gameObject.tag=="normal" && gameObject.tag=="PlayerBro")
        {
            int targetCount = collision.gameObject.GetComponent<Leader>().myPartners.Count;
            if (targetCount < myLeader.GetComponent<Leader>().myPartners.Count)
            {
                ChangeToBePlayerLeader(collision.gameObject);
            }
        }


        //EnemyLeader
        if (collision.gameObject.tag == "EnemyLeader" && gameObject.tag == "PlayerBro") {
            int targetCount = collision.gameObject.GetComponent<Leader>().myPartners.Count;
            if (targetCount == 0)
            {
                Destroy(collision.gameObject);
            }

        }
        if (collision.gameObject.tag == "EnemyLeader" && gameObject.tag == "Enemy") {
            int targetCount = collision.gameObject.GetComponent<Leader>().myPartners.Count;
            if (targetCount == 0)
            {
                Destroy(collision.gameObject);
            }

        }

        //PlayerLeader
        if (collision.gameObject.tag == "PlayerLeader" && gameObject.tag == "Enemy") {
            int targetCount = collision.gameObject.GetComponent<Leader>().myPartners.Count;
            
            if (targetCount == 0)
            {
                Manager.instance.PlayerDeath();
                Destroy(collision.gameObject);
            }
        }


    }


    void ChangeToBePlayerLeader(GameObject _partner) {
        List<GameObject> myLeaderPartner = myLeader.GetComponent<Leader>().myPartners;
        if (myLeaderPartner.IndexOf(_partner) == -1)
        {
            myLeader.GetComponent<Leader>().myPartners.Add(_partner);

            if (myLeader.GetComponent<PlayerLeader>().onPartnerChangedCallback != null)
                myLeader.GetComponent<PlayerLeader>().onPartnerChangedCallback.Invoke();

            _partner.tag = "PlayerBro";
            _partner.layer = 9;
            _partner.GetComponent<Partner>().myLeader = myLeader;
            _partner.GetComponent<Partner>().RemoveMeFromSpawn();
        }
    }


    void ChangeToBeLeaderMind(GameObject _partner) {
        List<GameObject> myLeaderPartner = myLeader.GetComponent<Leader>().myPartners;
        if (myLeaderPartner.IndexOf(_partner) == -1)
        {
            myLeader.GetComponent<Leader>().myPartners.Add(_partner);

            if(myLeader.GetComponent<EnemyAI>().onPartnerChangedCallback!=null)
                myLeader.GetComponent<EnemyAI>().onPartnerChangedCallback.Invoke();

            _partner.tag = "Enemy";
            _partner.layer = 7;
            _partner.GetComponent<Partner>().myLeader = myLeader;
            _partner.GetComponent<Partner>().RemoveMeFromSpawn();
        }
    }

   

    
}
public enum StageStatus { Idle, Gopartner, GoCombat, RandomMove }

