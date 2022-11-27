using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torchScript : MonoBehaviour
{
    [SerializeField] AudioSource _torchAudio;
    // Start is called before the first frame update
    void Start()
    {
        _torchAudio.time = Random.Range(0f, 90f);
    }

}
