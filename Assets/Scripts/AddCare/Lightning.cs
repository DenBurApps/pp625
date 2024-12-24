using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lightning : MonoBehaviour
{
    [SerializeField] private Button _deleteButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _backButton;
    [SerializeField] private Button[] _buttons;

    private LightType _type = LightType.None;
    
    public event Action SavedClicked;
    public event Action BackClicked;
    public event Action Deleted;

    public LightType Type => _type;
    public bool IsActive { get; private set; }
    
    private void OnEnable()
    {
        _saveButton.onClick.AddListener(OnSaveClicked);
        _backButton.onClick.AddListener(OnBackButtonClicked);

        foreach (var button in _buttons)
        {
            button.onClick.AddListener(() => SetLightType(button));
        }
        
        _deleteButton.onClick.AddListener(OnDeleteButtonClicked);
    }

    private void OnDisable()
    {
        _saveButton.onClick.RemoveListener(OnSaveClicked);
        _backButton.onClick.RemoveListener(OnBackButtonClicked);

        foreach (var button in _buttons)
        {
            button.onClick.RemoveListener(() => SetLightType(button));
        }
        
        _deleteButton.onClick.RemoveListener(OnDeleteButtonClicked);
    }
    
    private void Start()
    {
        _deleteButton.gameObject.SetActive(false);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
        ValidateSaveButton();
        IsActive = true;
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        IsActive = false;
    }
    
    public void ReturnToDefault()
    {
        _type = LightType.None;
        ValidateSaveButton();
        _deleteButton.gameObject.SetActive(false);
    }
    
    public void SetData(LightningData data)
    {
        _type = data.Type;

        if (_type == LightType.Sunlight)
        {
            _buttons[0].Select();
        }
        else if (_type == LightType.Lamp)
        {
            _buttons[1].Select();
        }
        else
        {
            _buttons[2].Select();
        }
        
        _deleteButton.gameObject.SetActive(true);
    }
    
    private void OnSaveClicked()
    {
        SavedClicked?.Invoke();
    }

    private void OnBackButtonClicked()
    {
        ReturnToDefault();
        BackClicked?.Invoke();
    }

    private void SetLightType(Button button)
    {
        int buttonIndex = Array.IndexOf(_buttons, button);

        _type = buttonIndex switch
        {
            0 => LightType.Sunlight,
            1 => LightType.Lamp,
            2 => LightType.Ultraviolet,
            _ => LightType.None
        };

        ValidateSaveButton();
    }

    private void ValidateSaveButton()
    {
        _saveButton.interactable = _type != LightType.None;
    }

    private void OnDeleteButtonClicked()
    {
        Deleted?.Invoke();
    }
}
