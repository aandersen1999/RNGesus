using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroup : MonoBehaviour
{

    private void Start()
    {
        GameManager.Instance.SetPlayerGroup(gameObject);
    }
}
