using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager instance;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else instance = this;
    }

    [SerializeField] List<Color> colors;

    public void AddColor(Color _color) {
        colors.Add(_color);
    }

    public void RemoveColor(Color _color) {
        colors.Remove(_color);
    }

    public bool CheckColor(Color _color) {

        if (colors.IndexOf(_color) != -1)
            return false;
        return true;
        
    }

}
