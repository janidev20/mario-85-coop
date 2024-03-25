using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenuUIManager : MonoBehaviour
{
    [SerializeField] List<GameObject> Opened;
    [SerializeField] List<GameObject> Closed;

    private bool closed = true;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            closed = !closed;
        }

        if (closed)
        {
            for (int i = 0; i < Closed.Count; i++)
            {
                Closed[i].SetActive(true);
            }
            for (int i = 0; i < Opened.Count; i++)
            {
                Opened[i].SetActive(false);
            }
        } else
        {
            for (int i = 0; i < Closed.Count; i++)
            {
                Closed[i].SetActive(false);
            }
            for (int i = 0; i < Opened.Count; i++)
            {
                Opened[i].SetActive(true);
            }
        }
    }
}
