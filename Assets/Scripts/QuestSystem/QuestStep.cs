using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished;

    protected void  FinishQuestStep(QuestStep step) 
    {  
        if (!isFinished) 
        { 
            isFinished = true;

            //quest advancement

            Destroy(this.gameObject);
        } 
    }
}
