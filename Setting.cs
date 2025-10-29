using System;
using UnityEngine;

namespace ModSettingTest {
    public static class Setting {
        public static string Dropdown1 { get; private set; }
        public static string Dropdown2 { get; private set; }
        public static bool Toggle1 { get; private set; }
        public static bool Toggle2 { get; private set; }
        public static float Slider1 { get; private set; }
        public static float Slider2 { get; private set; }
        public static string Input1 { get; private set; }
        public static string Input2 { get; private set; }
        public static KeyCode Keybinding1 { get; private set; }
        public static KeyCode Keybinding2 { get; private set; }
        public static event Action<float> OnSlider1ValueChanged;
        public static void SetDropdown1(string value) => Dropdown1 = value;
        public static void SetDropdown2(string value) => Dropdown2 = value;
        public static void SetToggle1(bool value) => Toggle1 = value;
        public static void SetToggle2(bool value) => Toggle2 = value;
        public static void SetSlider1(float value) {
            Slider1 = value;
            OnSlider1ValueChanged?.Invoke(value);
        }

        public static void SetSlider2(float value) => Slider2 = value;
        public static void SetInput1(string value) => Input1 = value;
        public static void SetInput2(string value) => Input2 = value;
        public static void SetKeybinding1(KeyCode value) => Keybinding1 = value;
        public static void SetKeybinding2(KeyCode value) => Keybinding2 = value;
        
        public static SettingData GetData() {
            return new SettingData(
                Dropdown1,
                Dropdown2,
                Toggle1,
                Toggle2,
                Slider1,
                Slider2,
                Input1,
                Input2,
                Keybinding1.ToString(),
                Keybinding2.ToString());
        }

        public static void Clear() {
            OnSlider1ValueChanged = null;
        }
    }
    [Serializable]
    public struct SettingData {
        public string dropdown1;
        public string dropdown2;
        public bool toggle1;
        public bool toggle2;
        public float slider1;
        public float slider2;
        public string input1;
        public string input2;
        public string keybinding1;
        public string keybinding2;

        public SettingData(string dropdown1, string dropdown2, bool toggle1, bool toggle2, float slider1, float slider2, string input1, string input2, string keybinding1, string keybinding2) {
            this.dropdown1 = dropdown1;
            this.dropdown2 = dropdown2;
            this.toggle1 = toggle1;
            this.toggle2 = toggle2;
            this.slider1 = slider1;
            this.slider2 = slider2;
            this.input1 = input1;
            this.input2 = input2;
            this.keybinding1 = keybinding1;
            this.keybinding2 = keybinding2;
        }
    }
}