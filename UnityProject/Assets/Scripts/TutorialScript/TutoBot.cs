using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TutoBot : MonoBehaviour
{
    public Tutorial tuto;
    private void FixedUpdate()
    {
        ///Repeat message when tutorial bot are clicked
        if(CommonInput.GetMouseButtonDown(0))
        {
            if(CommonInput.GetKey(KeyCode.LeftControl) == false || CommonInput.GetKey(KeyCode.RightControl) == false)
            {
                if((MouseUtils.MouseToWorldPos().x > (this.transform.position.x - .5) && MouseUtils.MouseToWorldPos().x < (this.transform.position.x + .5)
                && MouseUtils.MouseToWorldPos().y > (this.transform.position.y - .5) && MouseUtils.MouseToWorldPos().y < (this.transform.position.y + .5))
                )
                {
                    tuto.Message(Tutorial.botGO);
                }
            }
        }
        
        if(CommonInput.GetMouseButtonDown(0) && CommonInput.GetKey(KeyCode.LeftControl) || CommonInput.GetMouseButtonDown(0) && CommonInput.GetKey(KeyCode.RightControl))
        {
            PlayerList.Instance.InGamePlayers[0].GameObject.GetComponent<UniversalObjectPhysics>().PullSet(this.GetComponent<UniversalObjectPhysics>(), true);
        }
    }
}
