using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private void Awake()
    {

    }

    private void Start()
    {

    }

    void Update()
    {
        if (GameManager.Instance.State != GameState.Playing)
            return;

    }

}