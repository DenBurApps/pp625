using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPlantScreen : MonoBehaviour
{
    [SerializeField] private MainScreen _mainScreen;
    [SerializeField] private AddPlantScreenView _view;
    [SerializeField] private PhotosController _photosController;
    [SerializeField] private CarePlane[] _carePlanes;
    [SerializeField] private TransplantationPlane _transplantationPlane;
    [SerializeField] private TemperaturePlane _temperaturePlane;
    [SerializeField] private LightPlane _lightPlane;
    [SerializeField] private ChooseCategoryScreen _chooseCategoryScreen;
    [SerializeField] private CategoryImage _categoryImage;
    [SerializeField] private AddCareScreen _addCareScreen;

    private PlantCategory _category;
    private string _name;
    private string _description;
    private string _date;
    private byte[] _photo;
    private bool _addCareOpened = false;
    private List<CareData> _careDatas = new List<CareData>();

    public event Action BackButtonClicked;
    public event Action<byte[], string> AddCare;
    public event Action<PlantData> PlantDataCreated; 

    private void OnEnable()
    {
        _view.CategoryClicked += ChooseCategory;
        _view.DateInputed += DateInputed;
        _view.DescriptionInputed += DescriptionInputed;
        _view.NameInputed += NameInputed;
        _view.AddPhotoClicked += _photosController.OnSetImageButtonClick;
        _view.BackButtonClicked += OnBackButtonClicked;
        _view.AddCareClicked += OnAddCareClicked;
        _view.SaveButtonClicked += OnSaveButtonClicked;
        
        _photosController.SetPhoto += PhotoAdded;

        _chooseCategoryScreen.CategoryChosen += SetCategory;
        _chooseCategoryScreen.Canceled += _view.Enable;

        _mainScreen.AddPlant += OpenScreen;

        _addCareScreen.SavedWatering += ActivateCarePlane;
        _addCareScreen.SavedManuring += ActivateCarePlane;
        _addCareScreen.PlantCareSaved += ActivateCarePlane;
        _addCareScreen.TemperatureSaved += ActivateCarePlane;
        _addCareScreen.TransplantationSaved += ActivateCarePlane;
        _addCareScreen.LightningSaved += ActivateCarePlane;
        _addCareScreen.BackButtonClicked += Open;
    }

    private void OnDisable()
    {
        _view.CategoryClicked -= ChooseCategory;
        _view.DateInputed -= DateInputed;
        _view.DescriptionInputed -= DescriptionInputed;
        _view.NameInputed -= NameInputed;
        _view.AddPhotoClicked -= _photosController.OnSetImageButtonClick;
        _view.BackButtonClicked -= OnBackButtonClicked;
        _view.AddCareClicked -= OnAddCareClicked;
        _view.SaveButtonClicked -= OnSaveButtonClicked;
        
        _photosController.SetPhoto -= PhotoAdded;

        _chooseCategoryScreen.Canceled -= _view.Enable;
        _chooseCategoryScreen.CategoryChosen -= SetCategory;
        _mainScreen.AddPlant -= OpenScreen;

        _addCareScreen.SavedWatering -= ActivateCarePlane;
        _addCareScreen.SavedManuring -= ActivateCarePlane;
        _addCareScreen.PlantCareSaved -= ActivateCarePlane;
        _addCareScreen.TemperatureSaved -= ActivateCarePlane;
        _addCareScreen.TransplantationSaved -= ActivateCarePlane;
        _addCareScreen.LightningSaved -= ActivateCarePlane;
        _addCareScreen.BackButtonClicked -= Open;
    }

    private void UpdateAddCareButtonInteractivity()
    {
        bool allPlanesActive = true;

        foreach (var plane in _carePlanes)
        {
            if (!plane.IsActive)
            {
                allPlanesActive = false;
                break;
            }
        }

        if (!_temperaturePlane.IsActive || !_transplantationPlane.IsActive || !_lightPlane.IsActive)
        {
            allPlanesActive = false;
        }

        _view.SetAddCareButtonInteractable(!allPlanesActive);
    }

    private void Start()
    {
        _view.Disable();
        _chooseCategoryScreen.Disable();
        _view.CloseCalendar();
    }

    private void ChooseCategory()
    {
        _chooseCategoryScreen.Enable();
        _view.SetTransparent();
    }

    private void OpenScreen()
    {
        _view.Enable();
        ResetData();
        ValidateInput();
    }

    private void Open()
    {
        if (_addCareOpened)
        {
            _view.Enable();
        }
    }

    private void PhotoAdded()
    {
        _view.TogglePhotoButton(false, true);
    }

    private void ResetData()
    {
        _category = PlantCategory.None;
        _view.CloseCalendar();
        _photosController.ResetPhotos();
        _view.SetDescription(string.Empty);
        _view.SetName(string.Empty);
        _name = string.Empty;
        _description = string.Empty;
        _date = string.Empty;
        _photo = null;
        DisableAllCarePlanes();
        _view.TogglePhotoButton(true, false);
        _categoryImage.SetImage(_category);
        _careDatas.Clear();
        _addCareOpened = false;
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

    private void DisableAllCarePlanes()
    {
        foreach (var plane in _carePlanes)
        {
            plane.Disable();
        }

        _temperaturePlane.Disable();
        _transplantationPlane.Disable();
        _lightPlane.Disable();
    }

    private void ValidateInput()
    {
        bool isValid = !string.IsNullOrEmpty(_name) && !string.IsNullOrEmpty(_date) && _category != PlantCategory.None;

        _view.SetSaveButtonInteractable(isValid);
        _view.SetAddCareButtonInteractable(isValid);
    }

    private void OnBackButtonClicked()
    {
        _view.Disable();
        ResetData();
        BackButtonClicked?.Invoke();
    }
    
    private void OnSaveButtonClicked()
    {
        PlantData newPlantData = new PlantData(_name, _category, _date)
        {
            Description = _description,
            ImagePath = _photosController.GetPhoto(),
            CareDatas = new List<CareData>(_careDatas)
        };
        
        PlantDataCreated?.Invoke(newPlantData);
        
        _view.Disable();
        ResetData();
    }

    private void OnAddCareClicked()
    {
        AddCare?.Invoke(_photosController.GetPhoto(), _name);
        _addCareOpened = true;
        _view.Disable();
    }

    private void ActivateCarePlane(CareData data)
    {
        _view.Enable();

        if (data is WateringData wateringData)
        {
            _carePlanes[0].Enable();
            _carePlanes[0].SetData(wateringData);
        }
        else if (data is ManuringData manuringData)
        {
            _carePlanes[1].Enable();
            _carePlanes[1].SetData(manuringData);
        }
        else if (data is PlantCareData plantCareData)
        {
            _carePlanes[2].Enable();
            _carePlanes[2].SetData(plantCareData);
        }
        else if (data is TransplantationData transplantationData)
        {
            _transplantationPlane.Enable();
            _transplantationPlane.SetData(transplantationData);
        }
        else if (data is TemperatureData temperatureData)
        {
            _temperaturePlane.Enable();
            _temperaturePlane.SetData(temperatureData);
        }
        else if (data is LightningData lightningData)
        {
            _lightPlane.Enable();
            _lightPlane.SetData(lightningData);
        }

        if (!_careDatas.Contains(data))
            _careDatas.Add(data);
        
        UpdateAddCareButtonInteractivity();
    }
}