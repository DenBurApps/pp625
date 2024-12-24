using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class ChoseCareMainScreen : MonoBehaviour
{
    [SerializeField] private Sprite _selectedSprite;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _selectedTextColor;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Color _defaultTextColor;
    
    [SerializeField] private CareButton[] _careImages;
    [SerializeField] private Button _okButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private Button _categoryButton;

    private ScreenVisabilityHandler _screenVisabilityHandler;
    private List<CareType> _chosenCares = new List<CareType>();

    public event Action<List<CareType>> CaresChosen;
    public event Action CategoryClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        foreach (var careButton in _careImages)
        {
            careButton.ButtonClicked += OnCareButtonClicked;
        }

        _okButton.onClick.AddListener(OnOkButtonClicked);
        _cancelButton.onClick.AddListener(OnCancelButtonClicked);
        _categoryButton.onClick.AddListener(OnCategoryClicked);
        Validate();
    }

    private void OnDisable()
    {
        foreach (var careButton in _careImages)
        {
            careButton.ButtonClicked += OnCareButtonClicked;
        }
        
        _okButton.onClick.RemoveListener(OnOkButtonClicked);
        _categoryButton.onClick.RemoveListener(OnCategoryClicked);
        _cancelButton.onClick.RemoveListener(OnCancelButtonClicked);
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
    
    private void OnCareButtonClicked(CareButton careButton)
    {
        if (_chosenCares.Contains(careButton.Type))
        {
            _chosenCares.Remove(careButton.Type);
            SetButtonColor(careButton, _defaultSprite, _defaultColor, _defaultTextColor);
        }
        else
        {
            _chosenCares.Add(careButton.Type);
            SetButtonColor(careButton, _selectedSprite, _selectedColor, _selectedTextColor);
        }

        Validate();
    }

    private void SetButtonColor(CareButton careButton, Sprite backgroundSprite,Color backgroundColor, Color textColor)
    {
        careButton.Button.image.sprite = backgroundSprite;
        careButton.Button.image.color = backgroundColor;
        careButton.GetComponentInChildren<TMP_Text>().color = textColor;
    }

    private void OnOkButtonClicked()
    {
        CaresChosen?.Invoke(_chosenCares);
        Disable();
        ResetCareButtonColors();
    }

    private void OnCancelButtonClicked()
    {
        _chosenCares.Clear();
        ResetCareButtonColors();
        Validate();
        Disable();
    }

    private void ResetCareButtonColors()
    {
        foreach (var careButton in _careImages)
        {
            SetButtonColor(careButton, _defaultSprite,_defaultColor, _defaultTextColor);
        }
    }

    private void Validate()
    {
        _okButton.interactable = _chosenCares.Count > 0;
    }

    private void OnCategoryClicked()
    {
        CategoryClicked?.Invoke();
        Disable();
    }
}
