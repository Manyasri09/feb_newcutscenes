using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ezygamers.cmsv1
{
    
    public class LevelConfiggSO : ScriptableObject
    {
        public int LevelNumber;
       
        public int noOfSubLevel;

        //option 
        public List<QuestionBaseSO> question = new List<QuestionBaseSO>();
        

    }

    

}
