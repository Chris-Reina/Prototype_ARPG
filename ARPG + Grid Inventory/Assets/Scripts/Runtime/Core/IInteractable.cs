using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
   bool Interactable { get; }

   bool Interact();
}
