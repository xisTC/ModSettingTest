using System.Collections.Generic;
using Duckov.Modding;
using SodaCraft.Localizations;
using UnityEngine;

namespace ModSettingTest {
    public class ModBehaviour : Duckov.Modding.ModBehaviour {
        private void OnEnable() {
            ModManager.OnModActivated += ModManager_OnModActivated;
            ModManager.OnModWillBeDeactivated += ModManager_OnModWillBeDeactivated;
            LocalizationManager.OnSetLanguage += LocalizationManager_OnSetLanguage;
            Saver.Load();
        }

        private void OnDisable() {
            ModManager.OnModActivated -= ModManager_OnModActivated;
            ModManager.OnModWillBeDeactivated -= ModManager_OnModWillBeDeactivated;
            LocalizationManager.OnSetLanguage -= LocalizationManager_OnSetLanguage;
            Saver.Save();
            Setting.Clear();
        }

        private void Update() {
            if (Input.GetKeyDown(Setting.Keybinding1)) {
                Setting.SetSlider1(Mathf.Max(0, Setting.Slider1 - 2));
            }

            if (Input.GetKeyDown(Setting.Keybinding2)) {
                Setting.SetSlider1(Mathf.Min(100, Setting.Slider1 + 2));
                ModSettingAPI.GetValue<string>("I2", result => { Debug.Log("输入框2的值为:" + result); });
                ModSettingAPI.GetValue<int>("S4", value => { Debug.Log("S4:" + value); });
            }
        }

        private void AddUI(SystemLanguage language = SystemLanguage.ChineseSimplified) {
            ModSettingAPI.AddDropdownList("D1", language + ":下拉列表1",
                new List<string> { language + ":选项1", language + ":选项2", language + ":选项3" },
                Setting.Dropdown1, Setting.SetDropdown1);
            ModSettingAPI.AddDropdownList("D2", language + ":下拉列表2",
                new List<string> { language + ":选项7", language + ":选项8", language + ":选项9" },
                Setting.Dropdown2, Setting.SetDropdown2);
            ModSettingAPI.AddToggle("T1", language + ":开关1", Setting.Toggle1, Setting.SetToggle1);
            ModSettingAPI.AddToggle("T2", language + ":开关2", Setting.Toggle2, Setting.SetToggle2);
            ModSettingAPI.AddSlider("S1", "滑块1", Setting.Slider1, new Vector2(0, 100), Setting.SetSlider1);
            ModSettingAPI.AddSlider("S2", "滑块2", Setting.Slider2, new Vector2(0, 1000), Setting.SetSlider2, 2);
            ModSettingAPI.AddSlider("S3", "滑块3", 60, new Vector2(0, 1000), null, 3, 8);
            ModSettingAPI.AddSlider("S4", "滑块4", 50, 0, 200, value => { Debug.Log("滑块4:" + value); });
            ModSettingAPI.AddInput("I1", language + ":输入框1", Setting.Input1, 40, Setting.SetInput1);
            ModSettingAPI.AddInput("I2", language + ":输入框2", Setting.Input2, 50, Setting.SetInput2);
            ModSettingAPI.AddKeybinding("K1", "按键绑定1", Setting.Keybinding1, Setting.SetKeybinding1);
            ModSettingAPI.AddKeybinding("K2", "按键绑定2", Setting.Keybinding2, Setting.SetKeybinding2);
            //设置按键绑定默认值
            ModSettingAPI.AddKeybinding("K3", "按键绑定3", KeyCode.Alpha0, KeyCode.Alpha0, value => { Debug.Log(value); });
            ModSettingAPI.AddKeybinding("K4", "按键绑定4", KeyCode.Alpha1, KeyCode.Alpha1);

            ModSettingAPI.AddButton("B1", "点击移除S2", "按钮",
                () => { ModSettingAPI.RemoveUI("S2", result => { Debug.Log($"移除{(result ? "成功" : "失败")}"); }); });
            ModSettingAPI.AddButton("B2", "恢复所有默认值", "重置", Reset);
            ModSettingAPI.AddButton("B3", "移除滑块组", "移除", () => { ModSettingAPI.RemoveUI("G3"); });
            //Group用于整理设置项过多的情况，一般来说不需要使用。
            ModSettingAPI.AddGroup("G1", language + ":下拉列表组", new List<string>() { "D1", "D2" }, 0.6f, true);
            ModSettingAPI.AddGroup("G2", language + ":开关组", new List<string>() { "T1", "T2" }, 0.7f, true, true);
            ModSettingAPI.AddGroup("G3", language + ":滑块组", new List<string>() { "S1", "S2", "S3", "S4" });
            // ModSettingAPI.AddGroup("G4", "输入组", new List<string>() { "I1", "I2"}, 0.7f,false,true);
            // ModSettingAPI.AddGroup("G5", "绑定组", new List<string>() { "K1", "K2","K3","K4"});
            // ModSettingAPI.AddGroup("G6", "按钮组", new List<string>() { "B1", "B2","B3"});
            //注: 目前不支持group的嵌套，后续更新实现
            Setting.OnSlider1ValueChanged += Setting_OnSlider1ValueChanged;
            //测试 issue1
            TestIssue1();
            //测试控件添加问题
            ModSettingAPI.AddToggle("testAdd", "添加控件", true, AddTest);
            AddTest(true);
        }

        private void AddTest(bool obj) {
            if (obj) {
                ModSettingAPI.AddButton("testadd1", "testadd1");
                ModSettingAPI.AddButton("testadd2", "testadd2");
                ModSettingAPI.AddButton("testadd3", "testadd3");
                ModSettingAPI.AddButton("testadd4", "testadd4");
                ModSettingAPI.AddButton("testadd5", "testadd5");
            } else {
                ModSettingAPI.RemoveUI("testadd1");
                ModSettingAPI.RemoveUI("testadd2");
                ModSettingAPI.RemoveUI("testadd3");
                ModSettingAPI.RemoveUI("testadd4");
                ModSettingAPI.RemoveUI("testadd5");
            }
        }

        private void TestIssue1() {
            bool enable = true;
            ModSettingAPI.AddToggle("quality", "是否启用品质视觉效果", enable, QualityUICallback);
            if (enable) AddTestUI();
        }

        private static void AddTestUI() {
            ModSettingAPI.AddButton("lv0color", "垃圾物品颜色");
            ModSettingAPI.AddButton("lv1color", "普通物品颜色");
            ModSettingAPI.AddButton("lv2color", "优良物品颜色");
            ModSettingAPI.AddButton("lv3color", "精良物品颜色");
            ModSettingAPI.AddButton("lv4color", "史诗物品颜色");
            ModSettingAPI.AddButton("lv5color", "传说物品颜色");
            ModSettingAPI.AddButton("lv6color", "神话物品颜色");
            ModSettingAPI.AddGroup("ColorGroup", "物品颜色设置",
                new List<string> { "lv0color", "lv1color", "lv2color", "lv3color", "lv4color", "lv5color", "lv6color" });
        }

        private void QualityUICallback(bool value) {
            if (value) {
               AddTestUI();
            } else {
                ModSettingAPI.RemoveUI("ColorGroup");
            }
        }

        private void Reset() {
            //注意：SetValue只是单方面通知UI设置值,也就是说UI的onValueChange不会被调用
            //如果需要同步，应该先设置此mod的值，再将此mod的值设置给ModSetting。如：Dropdown1这样，其余的都只改变了UI的值并没有改变此mod的值。
            Setting.SetDropdown1("选项1");
            ModSettingAPI.SetValue("D1", Setting.Dropdown1);
            ModSettingAPI.SetValue("D2", "选项7");
            ModSettingAPI.SetValue("T1", false);
            ModSettingAPI.SetValue("T2", false);
            ModSettingAPI.SetValue("S1", 0f);
            ModSettingAPI.SetValue("S2", 0f);
            ModSettingAPI.SetValue("S3", 0f);
            ModSettingAPI.SetValue("S4", 0);
            ModSettingAPI.SetValue("I1", "输入框1默认文本");
            ModSettingAPI.SetValue("I2", "输入框2默认文本");
            ModSettingAPI.SetValue("K1", KeyCode.I);
            ModSettingAPI.SetValue("K2", KeyCode.O);
            ModSettingAPI.SetValue("K3", KeyCode.P);
            ModSettingAPI.SetValue("K4", KeyCode.L);
        }

        private void Setting_OnSlider1ValueChanged(float value) {
            //此mod设置数据变化时，设置UI的数值，单方面通知UI更新
            ModSettingAPI.SetValue("S1", value);
        }

        // 语言切换执行：销毁原来的UI，添加新UI。mod标题会从mod设置的底部添加
        // 如果不想改变mod标题位置，可以使用RmoveUI(key)后，添加相应的UI
        // 如果觉得重新实例化会损失性能，后续更新UpdateUI(key)类似的API，或者使用对象池，或者RegisterLanguagePack(..)由ModSetting来更新UI
        private void LocalizationManager_OnSetLanguage(SystemLanguage obj) {
            ModSettingAPI.RemoveMod();
            AddUI(obj);
        }

        private void ModManager_OnModWillBeDeactivated(ModInfo arg1, Duckov.Modding.ModBehaviour arg2) {
            if (arg1.name != ModSettingAPI.MOD_NAME || !ModSettingAPI.Init(info)) return;
            //禁用ModSetting的时候移除监听
            Setting.OnSlider1ValueChanged -= Setting_OnSlider1ValueChanged;
        }

        //下面两个函数需要实现，实现后的效果是：ModSetting和mod之间不需要启动顺序，两者无论谁先启动都能正常添加设置
        private void ModManager_OnModActivated(ModInfo arg1, Duckov.Modding.ModBehaviour arg2) {
            if (arg1.name != ModSettingAPI.MOD_NAME || !ModSettingAPI.Init(info)) return;
            //(触发时机:此mod在ModSetting之前启用)检查启用的mod是否是ModSetting,是进行初始化
            AddUI(LocalizationManager.CurrentLanguage);
        }

        protected override void OnAfterSetup() {
            //(触发时机:此mod在ModSetting之后启用)此mod，Setup后,尝试进行初始化
            if (ModSettingAPI.Init(info)) AddUI(LocalizationManager.CurrentLanguage);
        }
    }
}