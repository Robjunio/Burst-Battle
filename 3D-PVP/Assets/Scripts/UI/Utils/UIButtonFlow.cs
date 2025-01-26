using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonFlow : MonoBehaviour
{
    
    [SerializeField] private Button starterButton;

    public void DestinationButton()
    {
        starterButton.Select();
    }

    private void OnEnable()
    {
        DestinationButton();
    }
}
