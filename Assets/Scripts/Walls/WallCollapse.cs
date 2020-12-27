using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class WallCollapse : MonoBehaviour
{
    // Start is called before the first frame update
    HealthController healthController;
    public GameObject collapseSprite;
    bool collapsing = false;
    void Awake()
    {
        healthController = GetComponent<HealthController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(healthController.GetHealth() <= 0f && !collapsing)
        {
            collapsing = true;
        
            collapseSprite.SetActive(true);
            collapseSprite.transform.DOScale(new Vector3(3, 2, 1), 5f);
            this.transform.DOShakePosition(5f, new Vector3(1, 0, 0), 1);
            this.transform.DOMoveY(-5f, 5f);
            collapseSprite.transform.DOMoveY(-5f, 5f);
            collapseSprite.GetComponent<SpriteRenderer>().DOFade(0.1f, 6f);
            StartCoroutine(DestroyWall());
        }
    }
    IEnumerator DestroyWall()
    {
        yield return new WaitForSeconds(4);
        
        Destroy(this.gameObject,1f);
        yield break;
    }
}
