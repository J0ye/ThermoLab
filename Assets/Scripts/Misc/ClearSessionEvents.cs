using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clears all registered actions to all of the events of the session. SHould be used to reset.
/// </summary>
public class ClearSessionEvents : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Session.Instance().ClearAllLoginEvents();
    }
}
