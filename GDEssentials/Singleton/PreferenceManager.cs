using Godot;
using System;
using System.Collections.Generic;

using Chomp.Essentials;

public partial class PreferenceManager : Singleton<PreferenceManager>
{
    private new bool addToTree = false;
    private ConfigFile configFile;
    private string configFilepath = "user://preferences.cfg";

    public PreferenceManager() {
        configFile = new ConfigFile();
        if (System.IO.File.Exists(configFilepath)) {
            Error error = configFile.Load(configFilepath);
            if (error != Error.Ok)
                GD.PrintErr("Error loading preferences file: " + error.ToString());
        }
    }

    private void SavePreferences() {
        Error error = configFile.Save(configFilepath);
        if (error != Error.Ok)
            GD.PrintErr("Error saving preferences file: " + error.ToString());
    }

    public static void SetPreference(string key, Variant value) {
        Instance.configFile.SetValue("preferences", key, value);
        Instance.SavePreferences();
    }

    public static void DeletePreference(string key) {
        if (Instance.configFile.HasSectionKey("preferences", key))
            Instance.configFile.EraseSectionKey("preferences", key);
        Instance.SavePreferences();
    }

    public static Variant GetPreference(string key, Variant defaultValue = default) {
        return Instance.configFile.GetValue("preferences", key, defaultValue);
    }
}

