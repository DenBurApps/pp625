using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class ChooseCategoryScreen : MonoBehaviour
{
    [SerializeField] private CategoryHolder[] _categoryHolders;
    [SerializeField] private Button _okButton;
    [SerializeField] private Button _cancelButton;
    
    private ScreenVisabilityHandler _screenVisabilityHandler;

    private CategoryHolder _currentButton;

    public event Action<PlantCategory> CategoryChosen;
    public event Action Canceled;
    
    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _okButton.onClick.AddListener(OnOkButtonClicked);
        _cancelButton.onClick.AddListener(Cancel);

        foreach (var button in _categoryHolders)
        {
            button.ButtonClicked += OnButtonClicked;
        }
        
        Validate();
    }

    private void OnDisable()
    {
        _okButton.onClick.RemoveListener(OnOkButtonClicked);
        _cancelButton.onClick.RemoveListener(Cancel);

        foreach (var button in _categoryHolders)
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
    
    private void OnOkButtonClicked()
    {
        if(_currentButton == null)
            return;
        
        CategoryChosen?.Invoke(_currentButton.Category);
        Disable();
    }

    private void Validate()
    {
        _okButton.interactable = _currentButton != null;
    }

    private void OnButtonClicked(CategoryHolder categoryHolder)
    {
        _currentButton = categoryHolder;
        Validate();
    }

    private void Cancel()
    {
        _currentButton = null;
        Canceled?.Invoke();
        Disable();
    }
}


