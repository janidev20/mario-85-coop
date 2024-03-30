using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class NetworkObjManage : NetworkBehaviour
{
    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
    }
}
