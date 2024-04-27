using Godot;
using System;
using System.Collections.Generic;

using Lambchomp.Essentials;

public partial class PlayerPreferences : Singleton<PlayerPreferences>
{
    private new bool addToTree = false;
    private ConfigFile configFile;
    private string configFilepath = "user://preferences.cfg";

    public PlayerPreferences() {
        configFile = new ConfigFile();
        if (System.IO.File.Exists(configFilepath)) {
            Error error = configFile.Load(configFilepath);
            if (error != Error.Ok)
                GD.PrintErr("Error loading preferences file: " + error.ToString());
        }
    }

    public void SetPreference(string key, Variant value) {
        configFile.SetValue("preferences", key, value);
        SavePreferences();
    }

    public void DeletePreference(string key) {
        if (configFile.HasSectionKey("preferences", key))
            configFile.EraseSectionKey("preferences", key);
        SavePreferences();
    }

    public Variant GetPreference(string key, Variant defaultValue = default) {
        return configFile.GetValue("preferences", key, defaultValue);
    }

    private void SavePreferences() {
        Error error = configFile.Save(configFilepath);
        if (error != Error.Ok)
            GD.PrintErr("Error saving preferences file: " + error.ToString());
    }
}

