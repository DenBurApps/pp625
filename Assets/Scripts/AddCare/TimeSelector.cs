using System;
using DanielLochner.Assets.SimpleScrollSnap;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class TimeSelector : MonoBehaviour
{
    [SerializeField] private Color _selectedColor;
    [SerializeField] private Color _unselectedColor;
    
    [SerializeField] private SimpleScrollSnap _hrScrollSnap;
    [SerializeField] private SimpleScrollSnap _minScrollSnap;
    [SerializeField] private TMP_Text[] _hrtext;
    [SerializeField] private TMP_Text[] _mintext;

    [SerializeField] private Button _okButton;
    [SerializeField] private Button _cancelButton;
    
    private string _hr;
    private string _min;
    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action<string,string> OkClicked;
    public event Action CancelClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _hrScrollSnap.OnPanelCentered.AddListener(SetHr);
        _minScrollSnap.OnPanelCentered.AddListener(SetMin);
        
        _okButton.onClick.AddListener(OnOkClicked);
        _cancelButton.onClick.AddListener(OnCancelClicked);

        InitializeTextFields();
    }

    private void OnDisable()
    {
        _hrScrollSnap.OnPanelCentered.RemoveListener(SetHr);
        _minScrollSnap.OnPanelCentered.RemoveListener(SetMin);
        
        _okButton.onClick.RemoveListener(OnOkClicked);
        _cancelButton.onClick.RemoveListener(OnCancelClicked);
    }

    private void Start()
    {
        Disable();
    }

    public void Enable()
    {
        _screenVisabilityHandler.EnableScreen();
        ValidateOkButton();
    }

    public void Disable()
    {
        _screenVisabilityHandler.DisableScreen();
    }

    private void SetHr(int start, int end)
    {
        _hr = _hrtext[start].text;
        SetColorForSelected(_hrtext, start);
        ValidateOkButton();
    }

    private void SetMin(int start, int end)
    {
        _min = _mintext[start].text;
        SetColorForSelected(_mintext, start);
        ValidateOkButton();
    }

    private void InitializeTextFields()
    {
        for (int i = 0; i < _hrtext.Length; i++)
        {
            _hrtext[i].text = i.ToString("00");
        }

        for (int i = 0; i < _mintext.Length; i++)
        {
            _mintext[i].text = i.ToString("00");
        }

        SetColorForSelected(_hrtext, 0);
        SetColorForSelected(_mintext, 0);
    }

    private void SetColorForSelected(TMP_Text[] texts, int selectedIndex)
    {
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].color = i == selectedIndex ? _selectedColor : _unselectedColor;
        }
    }

    private void ValidateOkButton()
    {
        _okButton.interactable = !string.IsNullOrEmpty(_hr) || !string.IsNullOrEmpty(_min);
    }

    private void OnOkClicked()
    {
        OkClicked?.Invoke(_hr, _min);
        Disable();
    }

    private void OnCancelClicked()
    {
        _hrScrollSnap.GoToPanel(0);
        _minScrollSnap.GoToPanel(0);
        
        _hr = string.Empty;
        _min = string.Empty;

        InitializeTextFields();
        ValidateOkButton();
        
        CancelClicked?.Invoke();
        Disable();
    }
}