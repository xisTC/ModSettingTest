using System.Collections.Generic;
using Duckov.Modding;
using UnityEngine;

namespace ModSettingTest {
    public class ModBehaviour : Duckov.Modding.ModBehaviour {
        private void OnEnable() {
            ModManager.OnModActivated += ModManager_OnModActivated;
            ModManager.OnModWillBeDeactivated += ModManager_OnModWillBeDeactivated;
            Saver.Load();
        }
        private void OnDisable() {
            ModManager.OnModActivated -= ModManager_OnModActivated;
            ModManager.OnModWillBeDeactivated -= ModManager_OnModWillBeDeactivated;
            Saver.Save();
            Setting.Clear();
        }

        private void Update() {
            if (Input.GetKeyDown(Setting.Keybinding1)) {
                Setting.SetSlider1(Mathf.Max(0, Setting.Slider1 - 2));
            }

            if (Input.GetKeyDown(Setting.Keybinding2)) {
                Setting.SetSlider1(Mathf.Min(100, Setting.Slider1 + 2));
                ModSettingAPI.GetValue<string>("I2", result => {
                    Debug.Log("输入框2的值为:" + result);
                });
                ModSettingAPI.GetValue<int>("S4", value => { Debug.Log("S4:" + value); });
            }
        }

        private void AddUI() {
            ModSettingAPI.AddDropdownList("D1", "下拉列表1",
                new List<string> { "选项1", "选项2", "选项3" }, 
                Setting.Dropdown1, Setting.SetDropdown1);
            ModSettingAPI.AddDropdownList("D2", "下拉列表2", 
                new List<string> { "选项7", "选项8", "选项9" },
                Setting.Dropdown2, Setting.SetDropdown2);
            ModSettingAPI.AddToggle("T1", "按钮1", Setting.Toggle1, Setting.SetToggle1);
            ModSettingAPI.AddToggle("T2", "按钮2", Setting.Toggle2, Setting.SetToggle2);
            ModSettingAPI.AddSlider("S1", "滑块1", Setting.Slider1, new Vector2(0, 100), Setting.SetSlider1);
            ModSettingAPI.AddSlider("S2", "滑块2", Setting.Slider2, new Vector2(0, 1000), Setting.SetSlider2,2);
            ModSettingAPI.AddSlider("S3", "滑块3", 60, new Vector2(0, 1000), null,3,8);
            ModSettingAPI.AddSlider("S4", "滑块4", 50,0,200,value=>{ Debug.Log("滑块4:"+value);});
            ModSettingAPI.AddInput("I1", "输入框1", Setting.Input1, 40, Setting.SetInput1);
            ModSettingAPI.AddInput("I2", "输入框2", Setting.Input2, 50, Setting.SetInput2);
            ModSettingAPI.AddKeybinding("K1", "按键绑定1", Setting.Keybinding1, Setting.SetKeybinding1);
            ModSettingAPI.AddKeybinding("K2", "按键绑定2", Setting.Keybinding2, Setting.SetKeybinding2);
            ModSettingAPI.AddKeybindingWithDefault("K3", "按键绑定3", KeyCode.Alpha0,KeyCode.Alpha0,value=>{
                Debug.Log(value);});
            ModSettingAPI.AddKeybindingWithDefault("K4", "按键绑定4", KeyCode.Alpha1,KeyCode.Alpha1);
            
            ModSettingAPI.AddButton("B1", "点击移除S2","按钮", () => {
                ModSettingAPI.RemoveUI("S2", result => { Debug.Log($"移除{(result?"成功":"失败")}");
                });
            });
            ModSettingAPI.AddButton("B2", "恢复所有默认值","重置",Reset);
            ModSettingAPI.AddButton("B3", "移除滑块组","移除", () => {
                ModSettingAPI.RemoveUI("G3");
            });

            ModSettingAPI.AddGroup("G1", "下拉列表组", new List<string>() { "D1", "D2" });
            ModSettingAPI.AddGroup("G2", "Toggle组", new List<string>() { "T1", "T2" }, 0.7f,true,true);
            ModSettingAPI.AddGroup("G3", "滑块组", new List<string>() { "S1", "S2","S3","S4" }, 0.7f,true);
            ModSettingAPI.AddGroup("G4", "输入组", new List<string>() { "I1", "I2"}, 0.7f,false,true);
            ModSettingAPI.AddGroup("G5", "绑定组", new List<string>() { "K1", "K2","K3","K4"});
            ModSettingAPI.AddGroup("G6", "按钮组", new List<string>() { "B1", "B2"});
            //注: 目前不支持group的嵌套，后续更新实现
            Setting.OnSlider1ValueChanged += Setting_OnSlider1ValueChanged;
            
        }
        private void Reset() {
            //注意：SetValue只是单方面通知UI设置值,也就是说UI的onValueChange不会被调用
            //如果需要同步，应该先设置此mod的值，再将此mod的值设置给ModSetting。如：Dropdown1这样，其余的都只改变了UI的值并没有改变此mod的值。
            Setting.SetDropdown1("选项1");
            ModSettingAPI.SetValue("D1",Setting.Dropdown1);
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

        private void ModManager_OnModWillBeDeactivated(ModInfo arg1, Duckov.Modding.ModBehaviour arg2) {
            if (arg1.name != ModSettingAPI.MOD_NAME || !ModSettingAPI.Init(info)) return;
            //禁用ModSetting的时候移除监听
            Setting.OnSlider1ValueChanged -= Setting_OnSlider1ValueChanged;
        }
        //下面两个函数需要实现，实现后的效果是：ModSetting和mod之间不需要启动顺序，两者无论谁先启动都能正常添加设置
        private void ModManager_OnModActivated(ModInfo arg1, Duckov.Modding.ModBehaviour arg2) {
            if (arg1.name != ModSettingAPI.MOD_NAME || !ModSettingAPI.Init(info)) return;
            //(触发时机:此mod在ModSetting之前启用)检查启用的mod是否是ModSetting,是进行初始化
            AddUI();
        }
        protected override void OnAfterSetup() {
            //(触发时机:此mod在ModSetting之后启用)此mod，Setup后,尝试进行初始化
            if (ModSettingAPI.Init(info)) AddUI();
        }
    }
}