using System.ComponentModel;
using UnityEngine;

public abstract class Interactable: MonoBehaviour
{
   public bool useEvents;
   public string _promptMessage;

   public void BaseInteract(GameObject interactor)
   {
      Interact(interactor);
   }

   protected virtual void Interact(GameObject interactor)
   {
      
   }
}
