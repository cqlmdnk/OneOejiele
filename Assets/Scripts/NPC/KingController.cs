using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingController : MonoBehaviour
{
    CharacterState KingState;
    private float PathLenght;
    private Vector3 m_MoveVector;
    private bool m_FacingRight = true;
    private float m_StateChangeInterval , m_DirectionChangeInterval;
    // Start is called before the first frame update
    void Start()
    {
        KingState = Random.Range(0, 2) == 0 ? CharacterState.Idle : CharacterState.Run;
        PathLenght = Random.Range(2.0f, 4.0f);
        m_MoveVector = new Vector3(PathLenght, 0, 0);
        m_StateChangeInterval = Random.Range(0.0f, 5.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(KingState == CharacterState.Run)
        {
            if (m_FacingRight)
                this.transform.position = this.transform.position + 2 * m_MoveVector.normalized * Time.deltaTime;
            else
                this.transform.position = this.transform.position + 2 * -m_MoveVector.normalized * Time.deltaTime;
            PathLenght -= 2 * m_MoveVector.x * Time.deltaTime;
            if (PathLenght < 0)
            {
                if (transform.position.x > 10.0f || transform.position.x < -10.0f)
                    m_FacingRight = !m_FacingRight;
                else
                {
                    if(m_DirectionChangeInterval > 3.0f)
                    {
                        m_FacingRight = Random.Range(0, 2) == 0 ? true : false;
                        m_DirectionChangeInterval = 0.0f;
                    }
                }
                    

                PathLenght = Random.Range(2.0f, 6.0f);
                m_MoveVector = new Vector3(PathLenght, 0, 0);
                

            }
            
        }
        m_StateChangeInterval -= Time.deltaTime;
        m_DirectionChangeInterval += Time.deltaTime;
        if (m_StateChangeInterval < 0)
        {
            m_StateChangeInterval = Random.Range(0.0f, 5.0f);
            KingState = Random.Range(0, 2) == 0 ? CharacterState.Idle : CharacterState.Run;

        }
        GetComponent<SpriteRenderer>().flipX = !m_FacingRight;
        
        
    }
    public CharacterState GetState()
    {
        return KingState;
    }



}
