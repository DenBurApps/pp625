using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class ChooseCare : MonoBehaviour
{
    [SerializeField] private CareButton[] _careImages;
    [SerializeField] private Button _okButton;
    [SerializeField] private Button _cancelButton;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    private CareButton _currentButton;

    public event Action<CareType> CategoryChosen;
    public event Action Canceled;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _okButton.onClick.AddListener(OnOkButtonClicked);
        _cancelButton.onClick.AddListener(Cancel);

        foreach (var button in _careImages)
        {
            button.ButtonClicked += OnButtonClicked;
        }
        
        Validate();
    }

    private void OnDisable()
    {
        _okButton.onClick.RemoveListener(OnOkButtonClicked);
        _cancelButton.onClick.RemoveListener(Cancel);

        foreach (var button in _careImages)
        {
            button.ButtonClicked -= OnButtonClicked;
        }
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
        Validate();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void DisableButton(CareType careType)
    {
        foreach (var button in _careImages)
        {
            if (careType == button.Type)
            {
                button.Button.interactable = false;
            }
        }
    }

    public void EnableAllButtons()
    {
        foreach (var button in _careImages)
        {
            button.Button.interactable = true;
        }
    }
    
    private void OnOkButtonClicked()
    {
        if(_currentButton == null)
            return;
        
        CategoryChosen?.Invoke(_currentButton.Type);
        Disable();
    }

    private void Validate()
    {
        _okButton.interactable = _currentButton != null;
    }

    private void OnButtonClicked(CareButton careButton)
    {
        _currentButton = careButton;
        Validate();
    }

    private void Cancel()
    {
        _currentButton = null;
        Canceled?.Invoke();
        Disable();
    }
}
