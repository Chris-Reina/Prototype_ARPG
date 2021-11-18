using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckBossLife : MonoBehaviour
{
   public MiniBossModel model;

   public bool RedoLevel = false;
   private bool done = false;
   
   private void Update()
   {
      if (model.IsDead) RedoLevel = true;
   }

   private void OnCollisionEnter(Collision other)
   {
      if (done) return;
      
      if (other.gameObject.layer == LayersUtility.PlayerMaskIndex && RedoLevel)
      {
         done = true;
         SceneManager.LoadScene(0);
      }
   }
}
