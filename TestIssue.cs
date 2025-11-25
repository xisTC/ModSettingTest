using System.Collections.Generic;
using UnityEngine;

namespace ModSettingTest {
    public static class TestIssue {

        public  static void Issue1() {
            bool enable = true;
            ModSettingAPI.AddToggle("quality", "是否启用品质视觉效果", enable, QualityUICallback);
            if (enable) AddTestUI();
        }
        private static void AddTestUI() {
            ModSettingAPI.AddSlider("lv0time", "垃圾物品搜索时间比例", 0.8f, new Vector2(0.1f, 3.0f));
            ModSettingAPI.AddSlider("lv1time", "普通物品搜索时间比例", 0.8f, new Vector2(0.1f, 3.0f));
            ModSettingAPI.AddSlider("lv2time", "优良物品搜索时间比例", 0.8f, new Vector2(0.1f, 3.0f));
            ModSettingAPI.AddSlider("lv3time", "精良物品搜索时间比例", 0.8f, new Vector2(0.1f, 3.0f));
            ModSettingAPI.AddSlider("lv4time", "史诗物品搜索时间比例", 0.8f, new Vector2(0.1f, 3.0f));
            ModSettingAPI.AddSlider("lv5time", "传说物品搜索时间比例", 0.8f, new Vector2(0.1f, 3.0f));
            ModSettingAPI.AddSlider("lv6time", "神话物品搜索时间比例", 0.8f, new Vector2(0.1f, 3.0f));
            ModSettingAPI.AddGroup("TimeGroup", "搜索时间设置",new List<string>{"lv0time","lv1time","lv2time","lv3time","lv4time","lv5time","lv6time"},0.7f,true);
        }
        private static void QualityUICallback(bool value) {
            if (value) {
                AddTestUI();
            } else {
                ModSettingAPI.RemoveUI("TimeGroup");
            }
        }

    }
}