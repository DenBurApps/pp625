using System;
using System.Collections.Generic;
using UnityEngine;

public class OpenPlantScreen : MonoBehaviour
{
    [SerializeField] private CarePlane[] _carePlanes;
    [SerializeField] private TransplantationPlane _transplantationPlane;
    [SerializeField] private TemperaturePlane _temperaturePlane;
    [SerializeField] private LightPlane _lightPlane;
    [SerializeField] private OpenPlantScreenView _view;
    [SerializeField] private ImagePlacer _imagePlacer;
    [SerializeField] private CategoryImage _categoryHolder;
    [SerializeField] private MainScreen _mainScreen;
    [SerializeField] private AddCareScreen _addCareScreen;
    [SerializeField] private EditPlantScreen _editPlantScreen;
    
    private FilledPlane _filledPlane;
    private bool _careOpened = false;

    public event Action<FilledPlane> EditClicked;
    public event Action BackClicked;

    private void Start()
    {
        _view.Disable();
    }

    private void OnEnable()
    {
        _mainScreen.OpenedPlane += OpenScreen;
        _view.EditButtonClicked += OnEditClicked;
        _view.BackButtonClicked += OnBackClicked;
        _view.AddCareClicked += OnAddCareClicked;

        _carePlanes[0].Opened += OnAddCareClicked;
        _carePlanes[1].Opened += OnAddCareClicked;
        _carePlanes[2].Opened += OnAddCareClicked;
        _transplantationPlane.Opened += OnAddCareClicked;
        _temperaturePlane.Opened += OnAddCareClicked;
        _lightPlane.Opened += OnAddCareClicked;
       

        _editPlantScreen.BackButtonClicked += _view.Enable;
        _editPlantScreen.DataEdited += OpenScreen;

        _addCareScreen.NewDataSaved += OpenScreen;
        _addCareScreen.DataDeleted += () => OpenScreen(_filledPlane);
        _addCareScreen.BackButtonClicked += CheckedOpen;
    }

    private void OnDisable()
    {
        _mainScreen.OpenedPlane -= OpenScreen;
        _view.EditButtonClicked -= OnEditClicked;
        _view.BackButtonClicked -= OnBackClicked;
        _view.AddCareClicked -= OnAddCareClicked;
        
        _carePlanes[0].Opened -= OnAddCareClicked;
        _carePlanes[1].Opened -= OnAddCareClicked;
        _carePlanes[2].Opened -= OnAddCareClicked;
        _transplantationPlane.Opened -= OnAddCareClicked;
        _temperaturePlane.Opened -= OnAddCareClicked;
        _lightPlane.Opened -= OnAddCareClicked;
        
        _editPlantScreen.BackButtonClicked -= _view.Enable;
        _editPlantScreen.DataEdited -= OpenScreen;
        
        _addCareScreen.NewDataSaved -= OpenScreen;
        _addCareScreen.DataDeleted -= () => OpenScreen(_filledPlane);
        
        _addCareScreen.BackButtonClicked -= CheckedOpen;
    }
    
    private void OpenScreen(FilledPlane plane)
    {
        _view.Enable();
        
        if (plane == null)
            throw new ArgumentNullException(nameof(plane));

        _filledPlane = plane;
        
        _view.SetDate(_filledPlane.PlantData.Date);
        _view.SetName(_filledPlane.PlantData.Name);
        _view.SetDescription(_filledPlane.PlantData.Description);
        _imagePlacer.SetImage(_filledPlane.PlantData.ImagePath);
        _categoryHolder.SetImage(_filledPlane.PlantData.Category);
        
        ActivateCarePlanes(_filledPlane.PlantData.CareDatas);
    }
    
    private void ActivateCarePlanes(List<CareData> careDataList)
    {
        foreach (var plane in _carePlanes) plane.Disable();
        _transplantationPlane.Disable();
        _temperaturePlane.Disable();
        _lightPlane.Disable();
        
        foreach (var careData in careDataList)
        {
            if (careData is WateringData wateringData)
            {
                _carePlanes[0].Enable();
                _carePlanes[0].SetData(wateringData);
            }
            else if (careData is ManuringData manuringData)
            {
                _carePlanes[1].Enable();
                _carePlanes[1].SetData(manuringData);
            }
            else if (careData is PlantCareData plantCareData)
            {
                _carePlanes[2].Enable();
                _carePlanes[2].SetData(plantCareData);
            }
            else if (careData is TransplantationData transplantationData)
            {
                _transplantationPlane.Enable();
                _transplantationPlane.SetData(transplantationData);
            }
            else if (careData is TemperatureData temperatureData)
            {
                _temperaturePlane.Enable();
                _temperaturePlane.SetData(temperatureData);
            }
            else if (careData is LightningData lightningData)
            {
                _lightPlane.Enable();
                _lightPlane.SetData(lightningData);
            }
        }

        UpdateAddCareButtonInteractivity();
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

    private void OnEditClicked()
    {
        EditClicked?.Invoke(_filledPlane);
        _careOpened = false;
        _view.Disable();
    }

    private void OnBackClicked()
    {
        BackClicked?.Invoke();
        _careOpened = false;
        _view.Disable();
    }

    private void OnAddCareClicked(CareData careData)
    {
        _addCareScreen.OpenCareScreen(_filledPlane, careData);
        _careOpened = true;
        _view.Disable();
    }
    
    private void OnAddCareClicked()
    {
        _addCareScreen.OpenCareScreen(_filledPlane, null);
        _careOpened = true;
        _view.Disable();
    }
    
    private void CheckedOpen()
    {
        if (_addCareScreen)
        {
            _view.Enable();
        }
    }
}
