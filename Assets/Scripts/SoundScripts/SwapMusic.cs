using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapMusic : MonoBehaviour
{
   public void Swap()
   {
        if (MusicManager.Instance) MusicManager.Instance.SwapTrack();
   }
}
