using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace ModSettingTest {
    public static class Saver {
        private static readonly JsonSerializer jsonSerializer = new JsonSerializer() {
            Formatting = Formatting.Indented
        };
        private const string CONFIG_FILE_NAME = "config.json";
        public static void Load() {
            string configPath = GetConfigPath();
            Debug.Log("加载配置文件:" + configPath);
            if (File.Exists(configPath)) {
                string json = File.ReadAllText(configPath);
                SettingData data = jsonSerializer.Deserialize<SettingData>(new JsonTextReader(new StringReader(json)));
                if (!string.IsNullOrEmpty(data.dropdown1)&&
                    !string.IsNullOrEmpty(data.dropdown1key)) {
                    Setting.SetDropdown1(data.dropdown1);
                    Setting.SetDropdown2(data.dropdown2);
                    Setting.SetToggle1(data.toggle1);
                    Setting.SetToggle2(data.toggle2);
                    Setting.SetInput1(data.input1);
                    Setting.SetInput2(data.input2);
                    Setting.SetSlider1(data.slider1);
                    Setting.SetSlider2(data.slider2);
                    Setting.SetKeybinding1(Enum.Parse<KeyCode>(data.keybinding1));
                    Setting.SetKeybinding2(Enum.Parse<KeyCode>(data.keybinding2));
                    return;
                }
            }
            // 默认设置
            Setting.SetDropdown1("选项2");
            Setting.Dropdown1Key = "D1_Option1";
            Setting.SetDropdown2("选项9");
            Setting.Dropdown2Key = "D2_Option2";
            Setting.SetToggle1(true);
            Setting.SetToggle2(false);
            Setting.SetKeybinding1(KeyCode.None);
            // 创建默认配置文件
            CreateConfigFile();
        }
        public static void Save() {
            CreateConfigFile();
        }
        private static void CreateConfigFile() {
            string configPath = GetConfigPath();
            Debug.Log("创建配置文件:" + configPath);
            string directory = Path.GetDirectoryName(configPath);
            if (directory == null) {
                Debug.LogError("directory不能为null");
                return;
            }
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            StringWriter stringWriter = new StringWriter();
            jsonSerializer.Serialize(stringWriter, Setting.GetData());
            string json = stringWriter.ToString();
            File.WriteAllText(configPath, json);
            Debug.Log("创建完成：" + json);
        }
        private static string GetConfigPath() {
            string assemblyLocation = typeof(ModBehaviour).Assembly.Location;
            string directory = Path.GetDirectoryName(assemblyLocation);
            if (string.IsNullOrEmpty(directory)) {
                directory = AppContext.BaseDirectory ?? ".";
            }
            return Path.Combine(directory, CONFIG_FILE_NAME);
        }
    }
}