using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBusHolder : MonoBehaviour
{
    public EventBus eventBus_ { get; private set; }

    private void Awake() => eventBus_ = new EventBus();
}
