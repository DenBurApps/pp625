using System;
using Bitsplash.DatePicker;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class AddPlantScreenView : MonoBehaviour
{
    [SerializeField] private Button _addPhotoButton;
    [SerializeField] private Button _addPhotoSmallButton;
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private TMP_InputField _descriptionInput;
    [SerializeField] private TMP_Text _dateText;
    [SerializeField] private Button _categoryButton;
    [SerializeField] private Button _addCareButton;
    [SerializeField] private Button _dateButton;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _backButton;

    [SerializeField] private DatePickerSettings _datePicker;

    private ScreenVisabilityHandler _screenVisabilityHandler;

    public event Action<string> DateInputed;
    public event Action<string> NameInputed;
    public event Action<string> DescriptionInputed;
    public event Action SaveButtonClicked;
    public event Action AddCareClicked;
    public event Action BackButtonClicked;
    public event Action CategoryClicked;
    public event Action AddPhotoClicked;

    private void Awake()
    {
        _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
    }

    private void OnEnable()
    {
        _datePicker.Content.OnSelectionChanged.AddListener(SetDate);
        _saveButton.onClick.AddListener(OnSaveClicked);
        _backButton.onClick.AddListener(OnBackClicked);

        if (_addCareButton != null)
            _addCareButton.onClick.AddListener(OnAddCareClicked);

        _categoryButton.onClick.AddListener(OnCategoryClicked);
        _addPhotoButton.onClick.AddListener(OnAddPhotoClicked);
        _addPhotoSmallButton.onClick.AddListener(OnAddPhotoClicked);

        _nameInput.onValueChanged.AddListener(OnNameInputed);
        _descriptionInput.onValueChanged.AddListener(OnDescriptionInputed);
        _dateButton.onClick.AddListener(OnDateButtonClicked);
    }

    private void OnDisable()
    {
        _datePicker.Content.OnSelectionChanged.RemoveListener(SetDate);
        _saveButton.onClick.RemoveListener(OnSaveClicked);
        _backButton.onClick.RemoveListener(OnBackClicked);

        if (_addCareButton != null)
            _addCareButton.onClick.RemoveListener(OnAddCareClicked);
        
        _categoryButton.onClick.RemoveListener(OnCategoryClicked);
        _addPhotoButton.onClick.RemoveListener(OnAddPhotoClicked);
        _addPhotoSmallButton.onClick.RemoveListener(OnAddPhotoClicked);

        _nameInput.onValueChanged.RemoveListener(OnNameInputed);
        _descriptionInput.onValueChanged.RemoveListener(OnDescriptionInputed);
        _dateButton.onClick.RemoveListener(OnDateButtonClicked);
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

    public void TogglePhotoButton(bool firstButton, bool secondButton)
    {
        _addPhotoButton.gameObject.SetActive(firstButton);
        _addPhotoSmallButton.gameObject.SetActive(secondButton);
    }

    public void SetSaveButtonInteractable(bool isInteractable)
    {
        _saveButton.interactable = isInteractable;
    }

    public void SetAddCareButtonInteractable(bool isInteractable)
    {
        _addCareButton.interactable = isInteractable;
    }

    public void SetName(string value)
    {
        _nameInput.text = value;
    }

    public void SetDescription(string value)
    {
        _descriptionInput.text = value;
    }

    public void SetDate(string date)
    {
        _dateText.text = date;
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
        DateInputed?.Invoke(text);
        CloseCalendar();
    }

    public void CloseCalendar()
    {
        _datePicker.gameObject.SetActive(false);
    }

    private void OnDateButtonClicked() => _datePicker.gameObject.SetActive(true);
    private void OnNameInputed(string name) => NameInputed?.Invoke(name);
    private void OnDescriptionInputed(string description) => DescriptionInputed?.Invoke(description);
    private void OnSaveClicked() => SaveButtonClicked?.Invoke();
    private void OnBackClicked() => BackButtonClicked?.Invoke();
    private void OnAddCareClicked() => AddCareClicked?.Invoke();
    private void OnCategoryClicked() => CategoryClicked?.Invoke();
    private void OnAddPhotoClicked() => AddPhotoClicked?.Invoke();
}