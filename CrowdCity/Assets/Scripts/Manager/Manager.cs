using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Manager : MonoBehaviour
{
    [SerializeField] GameObject coutTag;
    [SerializeField] GameObject tagParents, stopPanel, allRankPanel, rankParent, rankObj;

    [SerializeField] float timeSetting, timeGame;
    [SerializeField] TextMeshProUGUI textTime, stopText;

    public bool StopNow, firstStop, played;

    #region singleton
    public static Manager instance;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else instance = this;
    }
    #endregion

    public FloatingTag CreateTag(Color _color, Transform _lookAtTrans) {
        coutTag.GetComponent<Image>().color = _color;
        FloatingTag myTag = coutTag.GetComponent<FloatingTag>();
        myTag.lookAt = _lookAtTrans;

        GameObject coutTagObj = Instantiate(coutTag, transform.position, Quaternion.identity, tagParents.transform);

        return coutTagObj.GetComponent<FloatingTag>();
    }


    private void Start()
    {
        timeSetting = timeGame;
        StopNow = true;
        played = false;
    }
    private void FixedUpdate()
    {
        if (played)
        {
            if (timeSetting > 0f)
            {
                timeSetting -= Time.deltaTime;
                StopNow = false;
                textTime.text = ((int)timeSetting / 60).ToString() + ":" + ((int)timeSetting % 60).ToString();
            }
            else
            {

                if (firstStop)
                    stopText.text = "Time Over!";
                if (StopNow == false) stopPanel.SetActive(true);
                StopNow = true;
            }
        }
        
    }

    public void SetTime() {
        if (firstStop)
        {
            List<string> playerName = RankManager.instance.playerName;
            List<int> playerCount = RankManager.instance.playerCount;
            List<Color> playerColor = RankManager.instance.playerColor;
            allRankPanel.SetActive(true);
            stopPanel.SetActive(false);
            for (int i = 0; i < playerName.Count; i++)
            {
                if (playerName[i] != null)
                {
                    rankObj.GetComponent<RankView>().ChangeText(
                    playerCount[i],
                    playerColor[i],
                    i,
                    playerName[i]);

                    Instantiate(rankObj, rankParent.transform.position,
                        Quaternion.identity, rankParent.transform);
                }

            }
        }
        else {
            timeSetting = timeGame;
            firstStop = true;
        }
    }
    public void PlayerDeath()
    {
        StopNow = true;
        firstStop = true;
        timeSetting = 0;
        SetTime();
    }

    public void Play() {
        StopNow = false;
        played = true;
    }
    public void Replay() {
        SceneManager.LoadScene(0);
    }

}
