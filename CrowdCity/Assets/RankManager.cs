using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankManager : MonoBehaviour
{
    #region singleton
    public static RankManager instance;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else instance = this;
    }

    #endregion

    public List<GameObject> players;

    public List<int> playerCount;
    public List<string> playerName;
    public List<Color> playerColor;

    public List<RankView> rankViews;
    [SerializeField] GameObject rankParent;

    public delegate void OnRankChange();
    public OnRankChange onRankChangedCallback;

    // Start is called before the first frame update
    void Start()
    {
        foreach ( RankView rankItem in rankParent.GetComponentsInChildren<RankView>())
        {
            rankViews.Add(rankItem);
        }
        
        onRankChangedCallback += ChangeRank;
    }

    // Update is called once per frame
    void Update()
    {
        SortPlayer();
    }

    void SortPlayer() {
        
        for (int i = 1; i < players.Count; i++)
        {
            GameObject nullObj;
            if (players[i]!=null && players[i-1]!=null)
            {
                if (players[i].GetComponent<Leader>().myPartners.Count > players[i - 1].GetComponent<Leader>().myPartners.Count)
                {
                    nullObj = players[i];
                    players[i] = players[i - 1];
                    players[i - 1] = nullObj;
                }
                playerName[i] = players[i].name;
                playerName[i - 1] = players[i - 1].name;

                playerCount[i] = players[i].GetComponent<Leader>().myPartners.Count;
                playerCount[i - 1] = players[i - 1].GetComponent<Leader>().myPartners.Count;

                playerColor[i] = players[i].GetComponentInChildren<SkinnedMeshRenderer>().materials[0].color;
                playerColor[i - 1] = players[i - 1].GetComponentInChildren<SkinnedMeshRenderer>().materials[0].color;
            }

        }
        if (onRankChangedCallback != null)
            onRankChangedCallback.Invoke();
    }

    public void ChangeRank()
    {
        if (rankViews.Count!=0)
        {
            int indexPlayer = players.IndexOf(GameObject.FindGameObjectWithTag("PlayerLeader"));

            if (indexPlayer > 2)
            {
                rankViews[3].gameObject.SetActive(true);
                rankViews[3].ChangeText(
                    players[indexPlayer].GetComponent<Leader>().myPartners.Count,
                    players[indexPlayer].GetComponentInChildren<SkinnedMeshRenderer>().materials[0].color,
                    indexPlayer,
                    players[indexPlayer].name);

            }
            else rankViews[3].gameObject.SetActive(false);


            for (int i = 0; i < 3; i++)
                if (players[i] != null) rankViews[i].ChangeText(
                      players[i].GetComponent<Leader>().myPartners.Count,
                      players[i].GetComponentInChildren<SkinnedMeshRenderer>().materials[0].color,
                      i,
                      players[i].name);
        }
        

    }

}
