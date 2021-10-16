using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ViewCount : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh;
    [SerializeField] Transform cam;

    public void ChangeText(int count) {
        textMesh.text = count.ToString();
    }

    private void LateUpdate()
    {
        RectTransform rext = GetComponentInChildren<Image>().rectTransform;
        rext.LookAt(rext.localPosition + cam.forward);
    }
}
