using System;
using UnityEngine;
using UnityEngine.UI;

public class EditPlantScreen : MonoBehaviour
{
    [SerializeField] private OpenPlantScreen _openScreen;
    [SerializeField] private AddPlantScreenView _view;
    [SerializeField] private PhotosController _photosController;
    [SerializeField] private ImagePlacer _imagePlacer;
    [SerializeField] private ChooseCategoryScreen _chooseCategoryScreen;
    [SerializeField] private CategoryImage _categoryImage;
    [SerializeField] private Button _saveButton;
    [SerializeField] private Button _deleteButton;

    private PlantCategory _category;
    private string _name;
    private string _description;
    private string _date;
    private byte[] _photo;
    private FilledPlane _filledPlane;

    public event Action BackButtonClicked;
    public event Action<FilledPlane> DataEdited;
    public event Action<FilledPlane> Deleted;

    private void Start()
    {
        _view.Disable();
        _chooseCategoryScreen.Disable();
        _view.CloseCalendar();
    }
    
    private void OnEnable()
    {
        _view.CategoryClicked += ChooseCategory;
        _view.DateInputed += DateInputed;
        _view.DescriptionInputed += DescriptionInputed;
        _view.NameInputed += NameInputed;
        _view.AddPhotoClicked += _photosController.OnSetImageButtonClick;
        _view.SaveButtonClicked += OnSaveClicked;
        _view.BackButtonClicked += OnBackButtonClicked;
        _deleteButton.onClick.AddListener(OnDeleteButtonClicked);

        _chooseCategoryScreen.CategoryChosen += SetCategory;
        _chooseCategoryScreen.Canceled += _view.Enable;

        _openScreen.EditClicked += OpenScreenWithExistingData;
    }

    private void OnDisable()
    {
        _view.CategoryClicked -= ChooseCategory;
        _view.DateInputed -= DateInputed;
        _view.DescriptionInputed -= DescriptionInputed;
        _view.NameInputed -= NameInputed;
        _view.AddPhotoClicked -= _photosController.OnSetImageButtonClick;
        _view.BackButtonClicked -= OnBackButtonClicked;
        _view.BackButtonClicked -= OnBackButtonClicked;
        _deleteButton.onClick.RemoveListener(OnDeleteButtonClicked);

        _chooseCategoryScreen.Canceled -= _view.Enable;
        _chooseCategoryScreen.CategoryChosen -= SetCategory;

        _openScreen.EditClicked -= OpenScreenWithExistingData;
    }

    private void OpenScreenWithExistingData(FilledPlane filledPlane)
    {
        _filledPlane = filledPlane;
        _view.Enable();
        LoadExistingData(filledPlane.PlantData);
        ValidateInput();
    }

    private void LoadExistingData(PlantData filledData)
    {
        _category = filledData.Category;
        _name = filledData.Name;
        _description = filledData.Description;
        _date = filledData.Date;
        _photo = filledData.ImagePath;

        _view.SetName(_name);
        _view.SetDescription(_description);
        _view.SetDate(_date);

        _categoryImage.SetImage(_category);
        SetCategory(_category);

        if (filledData.ImagePath != null)
        {
            _imagePlacer.SetImage(filledData.ImagePath);
            _view.TogglePhotoButton(false, true);
        }

        _view.TogglePhotoButton(true, false);
    }

    private void ChooseCategory()
    {
        _chooseCategoryScreen.Enable();
        _view.SetTransparent();
    }

    private void SetCategory(PlantCategory category)
    {
        _category = category;
        _categoryImage.SetImage(category);
        ValidateInput();
        _view.Enable();
    }

    private void NameInputed(string text)
    {
        _name = text;
        ValidateInput();
    }

    private void DateInputed(string date)
    {
        _date = date;
        ValidateInput();
    }

    private void DescriptionInputed(string text)
    {
        _description = text;
    }

    private void ValidateInput()
    {
        bool isValid = !string.IsNullOrEmpty(_name) && !string.IsNullOrEmpty(_date) && _category != PlantCategory.None;
        _view.SetSaveButtonInteractable(isValid);
    }

    private void OnBackButtonClicked()
    {
        _view.Disable();
        BackButtonClicked?.Invoke();
    }

    private void OnSaveClicked()
    {
        _filledPlane.PlantData.Name = _name;
        _filledPlane.PlantData.Category = _category;
        _filledPlane.PlantData.Date = _date;
        _filledPlane.PlantData.Description = _description;

        if (_photosController.GetPhoto() != null)
        {
            _filledPlane.PlantData.ImagePath = _photosController.GetPhoto();
        }
        
        DataEdited?.Invoke(_filledPlane);
        _filledPlane.UpdateUI();
        _view.Disable();
    }

    private void OnDeleteButtonClicked()
    {
        Deleted?.Invoke(_filledPlane);
        _view.Disable();
    }
}