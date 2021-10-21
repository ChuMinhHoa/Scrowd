using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankView : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI rankText;
    [SerializeField] TextMeshProUGUI myRank;
    RankManager rankManager;
    public void ChangeText(int index, Color color, int rankCout, string _name) {
        rankText.text =_name +" ("+ index.ToString()+")";
        rankText.color = color;
        myRank.text = "#" + (rankCout + 1).ToString();
        myRank.color = color;


    }

    
}
