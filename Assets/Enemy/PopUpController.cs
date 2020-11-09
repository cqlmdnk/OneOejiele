using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpController : MonoBehaviour
{
    // Start is called before the first frame update
    public Renderer myRenderer;
    private List<Renderer> renderers;
    public float popUpDieTime;
    Color newColor;
    private int i = 0;
    void Start()
    {
        myRenderer.sortingLayerName = "Default";
        myRenderer.sortingOrder = 130;
        renderers = new List<Renderer>();
        renderers.Add(this.gameObject.GetComponent<Renderer>());
        Transform bubble = this.gameObject.transform.GetChild(0);

        renderers.Add(bubble.GetComponent<Renderer>());

        Transform white = this.gameObject.transform.GetChild(1);

        renderers.Add(white.GetComponent<Renderer>());

        Transform white_end = white.transform.GetChild(0);
        renderers.Add(white_end.GetComponent<Renderer>());

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
        Debug.Log("i");
        foreach (Renderer renderer in renderers)
        {
            Debug.Log(renderer.material.color.a + " ," + i);
            newColor = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, (renderer.material.color.a - Time.deltaTime/2));
            transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime/10, transform.position.z);
            
            renderer.material.color = newColor;
            

        }
    }
}
