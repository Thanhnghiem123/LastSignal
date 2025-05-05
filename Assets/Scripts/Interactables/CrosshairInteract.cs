using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairInteract : MonoBehaviour
{
    public GameObject crosshair;
    private RectTransform restTransform;
    // Start is called before the first frame update
    void Start()
    {
        restTransform = crosshair.GetComponent<RectTransform>();
        
    }

    public void ScaleNormal()
    {
        restTransform.localScale = new Vector3(1f, 1f, 1f);
    }
    public void ScaleUp()
    {
        restTransform.localScale = new Vector3(7f, 7f, 7f);
    }

   
}
