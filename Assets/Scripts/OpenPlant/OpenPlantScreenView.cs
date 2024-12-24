using System;
using System.Collections;
using System.Collections.Generic;
using Bitsplash.DatePicker;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class OpenPlantScreenView : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _dateText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private Button _addCareButton;
    [SerializeField] private Button _editButton;
    [SerializeField] private Button _backButton;

    private ScreenVisabilityHandler _screenVisabilityHandler;
    
    public event Action EditButtonClicked;
    public event Action AddCareClicked;
    public event Action BackButtonClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _editButton.onClick.AddListener(OnEditClicked);
        _backButton.onClick.AddListener(OnBackClicked);
        _addCareButton.onClick.AddListener(OnAddCareClicked);
    }

    private void OnDisable()
    {
        _editButton.onClick.RemoveListener(OnEditClicked);
        _backButton.onClick.RemoveListener(OnBackClicked);
        _addCareButton.onClick.RemoveListener(OnAddCareClicked);
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    public void SetTransparent()
    {
        _screenVisabilityHandler.SetTransperent();
    }


    public void SetAddCareButtonInteractable(bool isInteractable)
    {
        _addCareButton.interactable = isInteractable;
    }

    public void SetName(string value)
    {
        _nameText.text = value;
    }

    public void SetDescription(string value)
    {
        _descriptionText.text = value;
    }

    public void SetDate(string date)
    {
        _dateText.text = date;
    }
    
    private void OnEditClicked() => EditButtonClicked?.Invoke();
    private void OnBackClicked() => BackButtonClicked?.Invoke();
    private void OnAddCareClicked() => AddCareClicked?.Invoke();

}
