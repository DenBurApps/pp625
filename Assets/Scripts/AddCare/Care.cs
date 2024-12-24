using System;
using System.Collections;
using System.Collections.Generic;
using Bitsplash.DatePicker;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Care : MonoBehaviour
{
    [SerializeField] private Button _deleteButton;
    
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _selectedColor;
    [SerializeField] private TimeSelector _timeSelector;
    [SerializeField] private TMP_Text _dateText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private Button[] _buttons;
    [SerializeField] private Button _dateButton;
    [SerializeField] private Button _timeButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _backButton;
    
    private DatePickerSettings _datePicker;
    private string _time;
    private string _date;
    private List<string> _days = new List<string>();
    private bool _dataInputed;
    private CareData _data;
    
    public event Action TimerOpened;
    public event Action SavedClicked;
    public event Action BackClicked;
    public event Action CalendarOpened;
    public event Action Enabled;

    public event Action DateDeleted; 

    public string Time => _time;
    public string Date => _date;
    public List<string> Days => _days;
    
    private void OnEnable()
    {
        _saveButton.onClick.AddListener(OnSaveClicked);
        _backButton.onClick.AddListener(OnBackButtonClicked);
        _timeSelector.OkClicked += SetTime;
        _timeSelector.CancelClicked += () => Enabled?.Invoke();
        _dateButton.onClick.AddListener(OpenCalendar);
        _timeButton.onClick.AddListener(OpenTimer);
        _deleteButton.onClick.AddListener(OnDataDeleted);
    }

    private void OnDisable()
    {
        foreach (var button in _buttons)
        {
            button.onClick.RemoveListener(() => OnButtonClicked(button));
            button.image.color = _defaultColor;
        }
        
        _saveButton.onClick.RemoveListener(OnSaveClicked);
        _backButton.onClick.RemoveListener(OnBackButtonClicked);
        _timeSelector.OkClicked -= SetTime;
        _timeSelector.CancelClicked -= () => Enabled?.Invoke();
        _datePicker.Content.OnSelectionChanged.RemoveListener(SetDate);
        _dateButton.onClick.RemoveListener(OpenCalendar);
        _timeButton.onClick.RemoveListener(OpenTimer);
        _deleteButton.onClick.RemoveListener(OnDataDeleted);
    }
    
    private void Start()
    {
        _deleteButton.gameObject.SetActive(false);
    }

    public void SetDatePicker(DatePickerSettings datePicker)
    {
        _datePicker = datePicker;
    }

    public void ReturnToDefault()
    {
        _dateText.text = "date";
        _timeText.text = "time";
        _days.Clear();
        _dataInputed = false;

        _date = string.Empty;
        _time = string.Empty;
        
        _deleteButton.gameObject.SetActive(false);
        
        foreach (var button in _buttons)
        {
            button.image.color = _defaultColor;
        }
    }

    public void SetData(CareData data)
    {
        _date = data.FilledDate;
        _time = data.FilledTime;

        _dateText.text = _date;
        _timeText.text = _time;
        
        _dataInputed = true;

        _data = data;
        
        _deleteButton.gameObject.SetActive(true);

        HighlightSelectedDays();
    }
    
    public void Enable()
    {
        gameObject.SetActive(true);
        _deleteButton.gameObject.SetActive(false);
        ValidateSaveButton();
        _datePicker.Content.OnSelectionChanged.AddListener(SetDate);
        
        foreach (var button in _buttons)
        {
            button.onClick.AddListener(() => OnButtonClicked(button));
        }
        
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    private void OnButtonClicked(Button button)
    {
        SetButton(button);
    }
    
    private void HighlightSelectedDays()
    {
        List<string> filledDays = null;
        
        if (_data is WateringData wateringData)
        {
            filledDays = wateringData.FilledDays;
        }
        else if (_data is ManuringData manuringData)
        {
            filledDays = manuringData.FilledDays;
        }
        else if (_data is PlantCareData plantCareData)
        {
            filledDays = plantCareData.FilledDays;
        }
    
        if (filledDays == null)
            return;

        foreach (var day in filledDays)
        {
            foreach (var dayImage in _buttons)
            {
                
                var dayText = dayImage.GetComponentInChildren<TMP_Text>().text;
                if (day.Equals(dayText, StringComparison.OrdinalIgnoreCase))
                {
                    SetButton(dayImage);
                    break;
                }
            }
        }
    }

    private void SetButton(Button button)
    {
        int buttonIndex = System.Array.IndexOf(_buttons, button);

        if (buttonIndex < 0) return;

        string selectedDay = buttonIndex switch
        {
            0 => "Mon",
            1 => "Tue",
            2 => "Wed",
            3 => "Thu",
            4 => "Fri",
            5 => "Sat",
            6 => "Sun",
            _ => null
        };

        if (selectedDay != null)
        {
            if (_days.Contains(selectedDay))
            {
                _days.Remove(selectedDay);
                button.image.color = _defaultColor;
            }
            else
            {
                _days.Add(selectedDay);
                button.image.color = _selectedColor;
            }
        }
        
        ValidateSaveButton();
    }

    private void OpenTimer()
    {
        _timeSelector.Enable();
        TimerOpened?.Invoke();
    }

    private void SetDate()
    {
        string text = "";
        var selection = _datePicker.Content.Selection;
        for (int i = 0; i < selection.Count; i++)
        {
            var date = selection.GetItem(i);
            text += date.ToString(format: "dd.MM.yyyy");
        }

        _dateText.text = text;
        _date = text;
        _datePicker.gameObject.SetActive(false);
        ValidateSaveButton();
        Enabled?.Invoke();
    }

    private void OpenCalendar()
    {
        _datePicker.gameObject.SetActive(true);
        CalendarOpened?.Invoke();
    }

    private void SetTime(string hr, string min)
    {
        _timeText.text = $"{hr}:{min}";
        _time = _timeText.text;
        ValidateSaveButton();
        Enabled?.Invoke();
    }

    private void ValidateSaveButton()
    {
        _saveButton.interactable = !string.IsNullOrEmpty(_date) && !string.IsNullOrEmpty(_time) && _days.Count > 0;
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

    private void OnDataDeleted()
    {
        DateDeleted?.Invoke();
    }
}