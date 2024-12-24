using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class MainScreen : MonoBehaviour
{
    private const string SaveFileName = "SavedData1.json";

    [SerializeField] private Calendar _calendar;
    [SerializeField] private MainScreenView _view;
    [SerializeField] private List<FilledPlane> _filledPlanes;
    [SerializeField] private AddPlantScreen _addPlantScreen;
    [SerializeField] private EditPlantScreen _editPlantScreen;
    [SerializeField] private OpenPlantScreen _openPlantScreen;
    [SerializeField] private FilterButton _filterButton;
    [SerializeField] private CategoryImage _careImage;
    [SerializeField] private Button _careImageDeletion;
    [SerializeField] private GameObject _carePlane;
    [SerializeField] private SettingsScreen _settingsScreen;

    private DateTime _selectedDate;

    private List<int> _availableIndexes = new List<int>();

    private string _saveFilePath => Path.Combine(Application.persistentDataPath, SaveFileName);

    public event Action<FilledPlane> OpenedPlane;
    public event Action AddPlant;
    public event Action SettingsOpen;

    private void Awake()
    {
        Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
    }

    private void OnEnable()
    {
        _view.AddPlantClicked += OnAddPlant;
        _addPlantScreen.BackButtonClicked += _view.Enable;
        _addPlantScreen.PlantDataCreated += EnablePlane;

        _openPlantScreen.BackClicked += _view.Enable;

        _editPlantScreen.Deleted += DeleteActivePlane;

        _filterButton.ChooseOpened += _view.SetTransparent;
        _filterButton.AllChosen += OnFilterUpdated;

        _careImageDeletion.onClick.AddListener(ResetCategory);

        _view.SearchInputed += FilterPlanes;

        _calendar.DateSelected += OnDateFilterButtonClicked;

        _view.SettingsClicked += OpenSettings;

        _settingsScreen.BackButtonClicked += _view.Enable;
    }

    private void OnDisable()
    {
        _view.AddPlantClicked -= OnAddPlant;
        _addPlantScreen.BackButtonClicked -= _view.Enable;
        _addPlantScreen.PlantDataCreated -= EnablePlane;

        _openPlantScreen.BackClicked -= _view.Enable;

        _editPlantScreen.Deleted -= DeleteActivePlane;

        _filterButton.ChooseOpened -= _view.SetTransparent;
        _filterButton.AllChosen -= OnFilterUpdated;
        _filterButton.ReturnedToDefault -= ShowAllPlanes;
        _filterButton.ReturnedToDefault -= ResetCategory;

        _careImageDeletion.onClick.RemoveListener(ResetCategory);

        _view.SearchInputed -= FilterPlanes;

        _calendar.DateSelected -= OnDateFilterButtonClicked;
        _view.SettingsClicked -= OpenSettings;
        _settingsScreen.BackButtonClicked -= _view.Enable;
    }

    private void Start()
    {
        _view.Enable();
        DisableAllWindows();
        _carePlane.gameObject.SetActive(false);
        LoadFilledWindowsData();
        _filterButton.ReturnedToDefault += ShowAllPlanes;
        _filterButton.ReturnedToDefault += ResetCategory;
    }

    private void EnablePlane(PlantData data)
    {
        _view.Enable();

        if (data == null)
            throw new ArgumentNullException(nameof(data));

        if (_availableIndexes.Count > 0)
        {
            int availableIndex = _availableIndexes[0];
            _availableIndexes.RemoveAt(0);

            var currentFilledItemPlane = _filledPlanes[availableIndex];

            if (currentFilledItemPlane != null)
            {
                currentFilledItemPlane.Enable();
                currentFilledItemPlane.SetData(data);
                currentFilledItemPlane.Opened += OnOpenPlane;
                currentFilledItemPlane.Updated += SaveFilledWindowsData;
            }
        }

        SaveFilledWindowsData();
        _view.ToggleEmptyPlane(_availableIndexes.Count >= _filledPlanes.Count);
    }

    private void DeleteActivePlane(FilledPlane plane)
    {
        if (plane == null)
            throw new ArgumentNullException(nameof(plane));

        _view.Enable();

        int index = _filledPlanes.IndexOf(plane);

        if (index >= 0 && !_availableIndexes.Contains(index))
        {
            _availableIndexes.Add(index);
        }

        plane.Opened -= OnOpenPlane;
        plane.Updated -= SaveFilledWindowsData;
        plane.Disable();

        _view.ToggleEmptyPlane(_availableIndexes.Count >= _filledPlanes.Count);

        SaveFilledWindowsData();
    }

    private void DisableAllWindows()
    {
        _availableIndexes.Clear();

        for (int i = 0; i < _filledPlanes.Count; i++)
        {
            _filledPlanes[i].Disable();
            _availableIndexes.Add(i);
        }
    }

    private void OnOpenPlane(FilledPlane plane)
    {
        OpenedPlane?.Invoke(plane);
        _view.Disable();
    }

    private void FilterPlanes()
    {
        _view.Enable();

        var chosenCategory = _filterButton.ChosenCategory;
        var chosenCareTypes = _filterButton.ChosenTypes;

        if (chosenCategory != null && chosenCategory != PlantCategory.None)
        {
            _carePlane.gameObject.SetActive(true);
            _careImage.SetImage(_filterButton.ChosenCategory);
        }

        if (_availableIndexes.Count >= _filledPlanes.Count)
            return;

        foreach (var plane in _filledPlanes)
        {
            var plantData = plane.PlantData;

            bool categoryMatches = chosenCategory == PlantCategory.None || plantData.Category == chosenCategory;
            bool careTypesMatch = chosenCareTypes.Count == 0 ||
                                  plantData.CareDatas.Exists(care => chosenCareTypes.Contains(care.FilledType));

            if ((chosenCategory != PlantCategory.None && categoryMatches) ||
                (chosenCareTypes.Count > 0 && careTypesMatch))
            {
                plane.Enable();
            }
            else
            {
                plane.Disable();
            }
        }
    }

    private void OnFilterUpdated()
    {
        FilterPlanes();
    }

    private void ShowAllPlanes()
    {
        _view.Enable();

        foreach (var plane in _filledPlanes)
        {
            if (plane.PlantData != null)
                plane.Enable();
        }
    }

    private void ResetCategory()
    {
        _filterButton.ResetCategory();
        _careImage.SetImage(PlantCategory.None);
        _carePlane.gameObject.SetActive(false);
    }

    private void OnAddPlant()
    {
        AddPlant?.Invoke();
        _view.Disable();
    }

    private void FilterPlanes(string searchText = "")
    {
        var chosenCategory = _filterButton.ChosenCategory;
        var chosenCareTypes = _filterButton.ChosenTypes;

        string lowerSearchText = searchText.ToLower();

        if (_availableIndexes.Count >= _filledPlanes.Count)
            return;

        foreach (var plane in _filledPlanes)
        {
            var plantData = plane.PlantData;

            bool categoryMatches = chosenCategory == PlantCategory.None || plantData.Category == chosenCategory;
            bool careTypesMatch = chosenCareTypes.Count == 0 ||
                                  plantData.CareDatas.Exists(care => chosenCareTypes.Contains(care.FilledType));
            bool nameMatches = string.IsNullOrEmpty(lowerSearchText) ||
                               plantData.Name.ToLower().Contains(lowerSearchText);

            if ((chosenCategory != PlantCategory.None && categoryMatches) ||
                (chosenCareTypes.Count > 0 && careTypesMatch) ||
                nameMatches)
            {
                plane.Enable();
            }
            else
            {
                plane.Disable();
            }
        }
    }

    private void OnDateFilterButtonClicked()
    {
        DateTime selectedDate = _calendar.SelectedDate;
        FilterPlanesByDate(selectedDate);
    }

    private void FilterPlanesByDate(DateTime selectedDate)
    {
        _selectedDate = selectedDate;

        foreach (var plane in _filledPlanes)
        {
            if (plane.PlantData != null)
            {
                DateTime nextCareDate = plane.NextCareDate;

                if (nextCareDate.Date == _selectedDate.Date)
                {
                    plane.Enable();
                }
                else
                {
                    plane.Disable();
                }
            }
        }
    }

    private void OpenSettings()
    {
        SettingsOpen?.Invoke();
        _view.Disable();
    }

    private void SaveFilledWindowsData()
    {
        List<PlantData> itemsToSave = new List<PlantData>();

        foreach (var window in _filledPlanes)
        {
            if (window.PlantData != null)
            {
                itemsToSave.Add(window.PlantData);
            }
        }

        var itemDataList = new ActivePlanesDataList(itemsToSave);
        string json = JsonConvert.SerializeObject(itemDataList, Formatting.Indented, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });

        try
        {
            File.WriteAllText(_saveFilePath, json);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save plant data: " + e.Message);
        }
    }

    private void LoadFilledWindowsData()
    {
        if (File.Exists(_saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(_saveFilePath);
                
                var loadedDataList = JsonConvert.DeserializeObject<ActivePlanesDataList>(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });

                int windowIndex = 0;
                foreach (PlantData data in loadedDataList.Data)
                {
                    if (windowIndex < _filledPlanes.Count)
                    {
                        if (_availableIndexes.Count > 0)
                        {
                            int availableIndex = _availableIndexes[0];
                            var currentFilledItemPlane = _filledPlanes[availableIndex];
                            _availableIndexes.RemoveAt(0);

                            currentFilledItemPlane.Enable();
                            currentFilledItemPlane.SetData(data);
                            currentFilledItemPlane.Opened += OnOpenPlane;
                            currentFilledItemPlane.Updated += SaveFilledWindowsData;
                        }
                    }
                }

                _view.ToggleEmptyPlane(_availableIndexes.Count >= _filledPlanes.Count);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load plant data: " + e.Message);
            }
        }
    }
}

[Serializable]
public class ActivePlanesDataList
{
    public List<PlantData> Data;

    public ActivePlanesDataList(List<PlantData> data)
    {
        Data = data;
    }
}