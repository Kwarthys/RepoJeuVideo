using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefineryBehaviour : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private GameObject smoke;

    private RessourceProcessor processor;

    void Start()
    {
        processor = transform.GetComponent<RessourceProcessor>();
    }

    // Update is called once per frame
    void Update()
    {
        float p = processor.getCompletion();

        toggleActive(p != 0);
        slider.value = p;
    }

    private void toggleActive(bool command)
    {
        canvas.SetActive(command);
        smoke.SetActive(command);
    }
}
