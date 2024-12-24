using System;
using Bitsplash.DatePicker;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Transplantation : MonoBehaviour
{
    [SerializeField] private Button _deleteButton;
    [SerializeField] private TimeSelector _timeSelector;
    [SerializeField] private TMP_Text _dateText;
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private Button _dateButton;
    [SerializeField] private Button _timeButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _backButton;

    private DatePickerSettings _datePicker;
    private string _time;
    private string _date;
    
    public event Action TimerOpened;
    public event Action CalendarOpened;
    public event Action SavedClicked;
    public event Action BackClicked;
    public event Action Enabled;
    public event Action Deleted;

    public string Time => _time;
    public string Date => _date;
    
    private void OnEnable()
    {
        _saveButton.onClick.AddListener(OnSaveClicked);
        _backButton.onClick.AddListener(OnBackButtonClicked);
        _timeSelector.OkClicked += SetTime;
        _timeSelector.CancelClicked += () => Enabled?.Invoke();
        _dateButton.onClick.AddListener(OpenCalendar);
        _timeButton.onClick.AddListener(OpenTimer);
        _deleteButton.onClick.AddListener(OnDeleted);
    }

    private void OnDisable()
    {
        _saveButton.onClick.RemoveListener(OnSaveClicked);
        _backButton.onClick.RemoveListener(OnBackButtonClicked);
        _timeSelector.OkClicked -= SetTime;
        _timeSelector.CancelClicked -= () => Enabled?.Invoke();
        _datePicker.Content.OnSelectionChanged.RemoveListener(SetDate);
        _dateButton.onClick.RemoveListener(OpenCalendar);
        _timeButton.onClick.RemoveListener(OpenTimer);
        _deleteButton.onClick.RemoveListener(OnDeleted);
    }

    private void Start()
    {
        _deleteButton.gameObject.SetActive(false);
    }

    public void SetDatePicker(DatePickerSettings datePicker)
    {
        _datePicker = datePicker;
    }
    
    public void Enable()
    {
        gameObject.SetActive(true);
        ValidateSaveButton();
        _datePicker.Content.OnSelectionChanged.AddListener(SetDate);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
    
    public void SetData(TransplantationData data)
    {
        _date = data.Date;
        _time = data.Time;

        _dateText.text = _date;
        _timeText.text = _time;
        
        _deleteButton.gameObject.SetActive(true);
    }

    public void ReturnToDefault()
    {
        _dateText.text = "date";
        _timeText.text = "time";
        _date = string.Empty;
        _time = string.Empty;
        
        _deleteButton.gameObject.SetActive(false);
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
        _saveButton.interactable = !string.IsNullOrEmpty(_date) && !string.IsNullOrEmpty(_time);
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

    private void OnDeleted()
    {
        Deleted?.Invoke();
    }
}
