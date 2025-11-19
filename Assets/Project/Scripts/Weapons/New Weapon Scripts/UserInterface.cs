using TMPro;
using UnityEngine;

public class UserInterface : MonoBehaviour
{
   public static UserInterface singleton;
   
   [Header("Ammo")]
   public TextMeshProUGUI bulletCount_Text;

   void Awake()
   {
      {
         UserInterface.singleton = this;
      }
      
      
   }

   public void UpdateBulletCounter(int ammoCount, int maxAmmo)
   {
      bulletCount_Text.text = ammoCount + "/" + maxAmmo;
   }
}
