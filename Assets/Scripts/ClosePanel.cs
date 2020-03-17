using System.Collections;
using System.Collections.Generic;
using UnityEngine;


   

public class ClosePanel : MonoBehaviour
{


     public GameObject Panel;

     public void Dismiss(){
      Panel.SetActive(false);
       
    }
  
}
