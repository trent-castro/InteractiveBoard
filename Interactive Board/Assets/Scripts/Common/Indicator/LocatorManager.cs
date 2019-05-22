﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocatorManager : MonoBehaviour
{
    public List<Locatable> Locatables { get; private set; } = new List<Locatable>();

    public List<GameObject> IndicatorPrefabs { get; private set; } = new List<GameObject>();

    public List<Locator> Locators { get; private set; } = new List<Locator>();

    public static LocatorManager Instance { get; set; }

    private bool Started = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (Locator locator in Locators)
        {
            locator.SetupIndicators(Locatables, IndicatorPrefabs);
        }

        Started = true;
    }

    public void Register(Locatable locatable)
    {
        Locatables.Add(locatable);
        IndicatorPrefabs.Add(locatable.IndicatorPrefab);

        if (Started)
        {
            foreach (Locator locator in Locators)
            {
                locator.AddIndicator(locatable.IndicatorPrefab);
            }
        }
    }

    public void Register(Locator locator)
    {
        Locators.Add(locator);

        if (Started)
        {
            locator.SetupIndicators(Locatables, IndicatorPrefabs);
        }
    }
}
