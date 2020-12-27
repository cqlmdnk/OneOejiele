using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingController : CharacterController
{
   
    private float PathLenght;
    private Vector3 MoveVector;
    
    private bool FacingRight = true;
    private float StateChangeInterval , DirectionChangeInterval;
    // Start is called before the first frame update
     protected override void Awake()
    {
        
        base.Awake();
        
        characterState = Random.Range(0, 2) == 0 ? CharacterState.Idle : CharacterState.Run;
        PathLenght = Random.Range(2.0f, 4.0f);
        MoveVector = new Vector3(PathLenght, 0, 0);
        StateChangeInterval = Random.Range(0.0f, 5.0f);
        

    }

    // Update is called once per frame
    protected override void Update()
    {
        if(characterState == CharacterState.Run)
        {
            if (FacingRight)
                this.transform.position = this.transform.position + 2 * MoveVector.normalized * Time.deltaTime;
            else
                this.transform.position = this.transform.position + 2 * -MoveVector.normalized * Time.deltaTime;
            PathLenght -= 2 * MoveVector.x * Time.deltaTime;
            if (PathLenght < 0)
            {
                if (transform.position.x > 10.0f || transform.position.x < -10.0f)
                    FacingRight = !FacingRight;
                else
                {
                    if(DirectionChangeInterval > 3.0f)
                    {
                        FacingRight = Random.Range(0, 2) == 0 ? true : false;
                        DirectionChangeInterval = 0.0f;
                    }
                }
                    

                PathLenght = Random.Range(2.0f, 6.0f);
                MoveVector = new Vector3(PathLenght, 0, 0);
                

            }
            
        }
        StateChangeInterval -= Time.deltaTime;
        DirectionChangeInterval += Time.deltaTime;
        if (StateChangeInterval < 0)
        {
            StateChangeInterval = Random.Range(0.0f, 5.0f);
            characterState = Random.Range(0, 2) == 0 ? CharacterState.Idle : CharacterState.Run;

        }
        GetComponent<SpriteRenderer>().flipX = !FacingRight;
        
        
    }

    
    



}
