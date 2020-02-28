using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContainerBehaviour : MonoBehaviour
{
    public Slider slider;
    private ContainerController cc;

    void Start()
    {
        cc = GetComponent<ContainerController>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = cc.getLoad() * 1.0f / cc.containerSpace;
    }
}
