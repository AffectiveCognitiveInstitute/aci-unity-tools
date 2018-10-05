using System;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Security.Cryptography;
using Aci.Unity.UI.Localization;
using Random = System.Random;

public class LocalizationTest
{
    ILocalizationManager locMan = new LocalizationManager();

    [Test]
    public void TestLocalizationLanguage([Random(10,100,3)]int totalStrings, [Values(5,10)] int numPicks)
    {
        Random rand = new Random();
        locMan.ClearData();
        string[] keys = new string[totalStrings];
        string[] values = new string[totalStrings];

        for (int i = 0; i < totalStrings; ++i)
        {
            keys[i] = Guid.NewGuid().ToString();
            values[i] = i + "_";
        }

        LocalizationData loc1 = LocalizationData.CreateInstance<LocalizationData>();
        loc1.languageIETF = "en_US";
        loc1.languageDescriptor = "ENG";
        for (int i = 0; i < totalStrings; ++i)
            loc1.stringData[keys[i]] = values[i] + "en";

        LocalizationData loc2 =  LocalizationData.CreateInstance<LocalizationData>();
        loc2.languageIETF = "de";
        loc2.languageDescriptor = "DEU";
        for (int i = 0; i < totalStrings; ++i)
            loc2.stringData[keys[i]] = values[i] + "de";

        locMan.AddLocalizationData(loc1);
        locMan.AddLocalizationData(loc2);

        for (int i = 0; i < numPicks; ++i)
        {
            int val = rand.Next(0, totalStrings);
            string key = keys[val];
            locMan.currentLocalization = "en_US";
            string value1 = locMan.GetLocalized(keys[val]);
            Assert.IsNotNull(value1);
            locMan.currentLocalization = "de";
            string value2 = locMan.GetLocalized(keys[val]);
            Assert.IsNotNull(value2);
            Assert.AreEqual(value1.Substring(0, value1.Length-3), value2.Substring(0, value2.Length - 3));
        }
    }

    [Test]
    public void TestLocalizationAdd([Random(10, 30, 3)] int numMatch, [Random(10, 30, 3)] int numCreate)
    {
        Random rand = new Random();
        locMan.ClearData();
        int totalStrings = 60;
        string[] keys = new string[totalStrings];
        string[] values = new string[totalStrings];

        for (int i = 0; i < totalStrings; ++i)
        {
            keys[i] = Guid.NewGuid().ToString();
            values[i] = i + "_";
        }

        LocalizationData loc1 = LocalizationData.CreateInstance<LocalizationData>();
        loc1.languageIETF = "en_US";
        loc1.languageDescriptor = "ENG";
        for (int i = 0; i < totalStrings; ++i)
            loc1.stringData[keys[i]] = values[i] + "en";

        int[] pickedValues = new int[numMatch];
        for (int i = 0; i < numMatch; ++i)
        {
            int n = rand.Next(0, totalStrings - 1);
            while (pickedValues.Contains(n))
            {
                n = rand.Next(0, totalStrings - 1);
            }
            pickedValues[i] = n;
        }

        string[] keysToMatch = new string[numMatch];
        string[] keysToCreate = new string[numCreate];

        LocalizationData loc2 = LocalizationData.CreateInstance<LocalizationData>();
        loc2.languageIETF = "en_US";
        loc2.languageDescriptor = "ENG";

        for (int i = 0; i < numMatch; ++i)
        {
            keysToMatch[i] = keys[pickedValues[i]];
            string key = keysToMatch[i];
            loc2.stringData[key] = loc1.stringData[key];
        }

        for (int i = 0; i < numCreate; ++i)
        {
            string key = Guid.NewGuid().ToString();
            keysToCreate[i] = key;
            Assert.IsFalse(loc1.stringData.ContainsKey(key));
            loc2.stringData[key] = i.ToString();
        }

        locMan.AddLocalizationData(loc1);
        locMan.AddLocalizationData(loc2);

        foreach (string key in keysToMatch)
            Assert.IsNotNull(locMan.GetLocalized(key));
        foreach (string key in keysToCreate)
            Assert.IsNull(locMan.GetLocalized(key));
    }

    [Test]
    public void TestLocalizationSubtract([Random(10, 30, 3)] int numSubtractDelete, [Random(10, 30, 3)] int numSubtractRetain)
    {
        Random rand = new Random();
        locMan.ClearData();
        int totalStrings = 60;
        string[] keys = new string[totalStrings];
        string[] values = new string[totalStrings];

        for (int i = 0; i < totalStrings; ++i)
        {
            keys[i] = Guid.NewGuid().ToString();
            values[i] = i + "_";
        }

        LocalizationData loc1 = LocalizationData.CreateInstance<LocalizationData>();
        loc1.languageIETF = "en_US";
        loc1.languageDescriptor = "ENG";
        for (int i = 0; i < totalStrings; ++i)
            loc1.stringData[keys[i]] = values[i] + "en";

        string[] keysToRetain = new string[numSubtractRetain];
        int[] pickedValues = new int[numSubtractRetain];
        for (int i = 0; i < numSubtractRetain; ++i)
        {
            int n = rand.Next(0, totalStrings - 1);
            while (pickedValues.Contains(n))
            {
                n = rand.Next(0, totalStrings - 1);
            }
            pickedValues[i] = n;
            keysToRetain[i] = keys[n];
        }

        LocalizationData loc2 = LocalizationData.CreateInstance<LocalizationData>();
        loc2.languageIETF = "en_US";
        loc2.languageDescriptor = "ENG";

        for (int i = 0; i < numSubtractRetain; ++i)
        {
            string key = keysToRetain[i];
            loc2.stringData[key] = loc1.stringData[key];
        }

        locMan.AddLocalizationData(loc1);
        locMan.AddLocalizationData(loc2);

        LocalizationData loc3 = LocalizationData.CreateInstance<LocalizationData>();
        loc3.languageIETF = "en_US";
        loc3.languageDescriptor = "ENG";

        string[] keysToDelete = new string[numSubtractDelete];
        int[] pickedValuesSub = new int[numSubtractDelete];
        for (int i = 0; i < numSubtractDelete; ++i)
        {
            int n = rand.Next(0, totalStrings - 1);
            while (pickedValuesSub.Contains(n) || pickedValues.Contains(n))
            {
                n = rand.Next(0, totalStrings - 1);
            }
            pickedValuesSub[i] = n;
            keysToDelete[i] = keys[n];
        }

        for (int i = 0; i < numSubtractRetain; ++i)
        {
            string key = keysToRetain[i];
            loc3.stringData[key] = loc1.stringData[key];
        }
        for (int i = 0; i < numSubtractDelete; ++i)
        {
            string key = keysToDelete[i];
            loc3.stringData[key] = loc1.stringData[key];
        }
        
        locMan.RemoveLocalizationData(loc3);
        locMan.currentLocalization = "en_US";

        LocalizationManager testMan = (LocalizationManager)locMan;
        //Assert.AreEqual(testMan.loadedData[0].stringData.Count, totalStrings - numSubtractDelete);

        foreach (string key in keysToRetain)
            Assert.IsNotNull(locMan.GetLocalized(key));
        foreach (string key in keysToDelete)
            Assert.IsNull(locMan.GetLocalized(key));
    }
}
