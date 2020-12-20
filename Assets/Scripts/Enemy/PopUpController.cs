using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpController : MonoBehaviour
{
    // Start is called before the first frame update
    public Renderer myRenderer;
    public float popUpDieTime = 1.0f;
    Color newColor;
    private int i = 0;
    void Start()
    {
        myRenderer.sortingLayerName = "Default";
        myRenderer.sortingOrder = 130;
        

    }
    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ((gameObject.name.Contains("(Clone)")))
        {
            popUpFadeOut();
            popUpDieTime -= Time.deltaTime;
            if (popUpDieTime < 0)
            {
                DestroyImmediate(this.gameObject);
            }
        }
    }

    void popUpFadeOut()
    {

        i++;

        newColor = new Color(myRenderer.material.color.r, myRenderer.material.color.g, myRenderer.material.color.b, myRenderer.material.color.a - Time.deltaTime / 2);
        transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime/10, transform.position.z);
        myRenderer.material.color = newColor;
            

        
    }
}
