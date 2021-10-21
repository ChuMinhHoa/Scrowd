using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerLeader : Leader
{
    [SerializeField] Material myMaterial;
    public FloatingTag myTag;

    public delegate void OnPartnerChange();
    public OnPartnerChange onPartnerChangedCallback;
    [SerializeField] CinemachineVirtualCamera myCam;
    [SerializeField] float camSize;
    float cameraSize;
    // Start is called before the first frame update
    void Start()
    {
        onPartnerChangedCallback += UpdatePartner;
        myTag = Manager.instance.CreateTag(myMaterial.color, transform);
        RankManager.instance.players.Add(gameObject);
        RankManager.instance.playerName.Add(gameObject.name);
        RankManager.instance.playerColor.Add(gameObject.GetComponentInChildren<SkinnedMeshRenderer>().materials[0].color);
        RankManager.instance.playerCount.Add(0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Manager.instance.StopNow)
        {
            if (myCam.m_Lens.OrthographicSize < cameraSize)
            {
                myCam.m_Lens.OrthographicSize += Time.deltaTime;
                CinemachineComponentBase componentBase = myCam.GetCinemachineComponent(CinemachineCore.Stage.Body);
                (componentBase as CinemachineFramingTransposer).m_CameraDistance = myCam.m_Lens.OrthographicSize * 2;
            }
        }

    }

    void UpdatePartner() {

        myTag.ChangeText(myPartners.Count);

        cameraSize = 0;
        for (int i = 0; i < myPartners.Count; i++)
        {
            Partner partner = myPartners[i].GetComponent<Partner>();
            partner.myPartners = myPartners;
            cameraSize += camSize;
        }

        cameraSize += 5f;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "normal")
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

        if (collision.gameObject.tag == "Enemy")
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
