using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///Part of Untitled Digital Twin Authoring Tool
///This script should really be an interface for "swipe gestures generally"
///But for now we're being lazy and it just opens and closes stuff
///</summary>
public class Swipe : MonoBehaviour
{
    public GameObject moreInfoObject;
    public SwipeCollider leftCollider, rightCollider;
    public bool leftColliderTouchedRecently, rightColliderTouchedRecently;

    // Update is called once per frame
    void Update()
    {
      if(leftCollider.isTouched){
        if(rightColliderTouchedRecently){
          Dismiss();
        }
        else{
          leftColliderTouchedRecently = true;
          StartCoroutine(SetFalse(leftColliderTouchedRecently));
          StartCoroutine(SetFalse(leftCollider.isTouched));
        }
      }//end left collider
      if(rightCollider.isTouched){
        if(leftColliderTouchedRecently){
          Summon();
        }
        else{
          rightColliderTouchedRecently = true;
          StartCoroutine(SetFalse(rightColliderTouchedRecently));
          StartCoroutine(SetFalse(rightCollider.isTouched));
        }
      }//end right collider
    }//end update

    //we move these out to be their own function in case we want them to do something fancier later
    //these instances of setting isTouched back to false aren't strictly necessary,
    //but you don't trust the OnTriggerExit to always work correctly
    public void Dismiss(){
      moreInfoObject.SetActive(false);
      rightCollider.isTouched = false;
      leftCollider.isTouched = false;
    }
    public void Summon(){
      moreInfoObject.SetActive(true);
      rightCollider.isTouched = false;
      leftCollider.isTouched = false;
    }


    IEnumerator SetFalse(bool aBoolean)
    {
      // suspend execution for 5 seconds
      yield return new WaitForSeconds(1);
      aBoolean = false;
    }
}
