using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporadicConfetti : MonoBehaviour
{
    public float confettiChance = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        if (Random.value < confettiChance) {
            gameObject.SetActive(true);
        } else {
            gameObject.SetActive(false);
        }
    }
}
