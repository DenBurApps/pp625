using System;
using System.Threading;
using TMPro;
using UnityEngine;

public class Calendar : MonoBehaviour
{
    [SerializeField] private MainScreenDateElement[] _dateElements;
    [SerializeField] private TMP_Text _currentDateText;

    private MainScreenDateElement _currentSelectedElement;

    private int _currentMonth;
    private int _currentYear;

    public event Action DateSelected; 
    
    public DateTime SelectedDate { get; private set; }

    private void Awake()
    {
        Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
    }

    private void OnEnable()
    {
        foreach (var element in _dateElements)
        {
            element.ElementClicked += SelectDate;
        }
    }

    private void OnDisable()
    {
        foreach (var element in _dateElements)
        {
            element.ElementClicked -= SelectDate;
        }
    }

    private void Start()
    {
        _currentMonth = DateTime.Now.Month;
        _currentYear = DateTime.Now.Year;
        _currentDateText.text = $"{_currentMonth}, {_currentYear}";
        SetMainText();
        DisableAllWindows();
        PopulateDays(_currentYear, _currentMonth);
    }

    public void NextMonth()
    {
        _currentMonth++;
        if (_currentMonth > 12)
        {
            _currentMonth = 1;
            _currentYear++;
        }

        PopulateDays(_currentYear, _currentMonth);
    }

    public void PreviousMonth()
    {
        _currentMonth--;
        if (_currentMonth < 1)
        {
            _currentMonth = 12;
            _currentYear--;
        }

        PopulateDays(_currentYear, _currentMonth);
    }

    private void PopulateDays(int year, int month)
    {
        DisableAllWindows();
        int daysInMonth = DateTime.DaysInMonth(year, month);

        for (int i = 0; i < _dateElements.Length; i++)
        {
            if (i < daysInMonth)
            {
                DateTime date = new DateTime(year, month, i + 1);
                _dateElements[i].SetDatesText(date.ToString("ddd"), (i + 1).ToString());
                _dateElements[i].gameObject.SetActive(true);
            }
            else
            {
                _dateElements[i].gameObject.SetActive(false);
            }
        }

        SetMainText();
    }

    private void SetMainText()
    {
        _currentDateText.text = $"{new DateTime(_currentYear, _currentMonth, 1):MMMM yyyy}";
    }

    private void DisableAllWindows()
    {
        foreach (var element in _dateElements)
        {
            element.gameObject.SetActive(false);
        }
    }

    private void SelectDate(MainScreenDateElement dateElement)
    {
        if (_currentSelectedElement != null)
        {
            _currentSelectedElement.Reset();
        }

        _currentSelectedElement = dateElement;

        ParseAndSetSelectedDate(dateElement);
    }

    private void ParseAndSetSelectedDate(MainScreenDateElement dateElement)
    {
        string dateText = dateElement.Day;

        int day = int.Parse(dateText);
        DateTime newDate = new DateTime(_currentYear, _currentMonth, day);
        SelectedDate = newDate;
        DateSelected?.Invoke();
    }
}