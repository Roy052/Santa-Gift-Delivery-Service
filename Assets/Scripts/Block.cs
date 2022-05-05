using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private void Start()
    {
        //this.gameObject.SetActive(false);
    }
    public void ON()
    {
        this.gameObject.SetActive(true);
    }

    public void OFF()
    {
        this.gameObject.SetActive(false);
    }
}
