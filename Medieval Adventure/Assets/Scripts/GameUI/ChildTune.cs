using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ChildTune : MonoBehaviour
{
    [SerializeField] private Image parentImage;
    private Image image;
    [SerializeField] Transform parentTransfotm;
    void Start()
    {
         
        image = transform.GetComponent<Image>();

    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = parentTransfotm.localScale;
        image.color = parentImage.color;
    }
}
