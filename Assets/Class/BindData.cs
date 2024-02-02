using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Class
{
    public class BindData
    {
        public string BindName;
        public KeyCode BindKeyCode;
        public TextMeshProUGUI BindLabel;
        public KeyCode BindkeyCodeDefault;

        public BindData(string name, TextMeshProUGUI label, KeyCode keycode, KeyCode orginalKeycode)
        {
            BindName = name;
            BindKeyCode = keycode;
            BindkeyCodeDefault = orginalKeycode;
            BindLabel = label;
            BindLabel.text = keycode.ToString();
        }

        public void ChangeBind(KeyCode keycode)
        {
            BindKeyCode = keycode;
            BindLabel.text = keycode.ToString();
            PlayerPrefs.SetString(BindName, keycode.ToString());
        }
    }

    public enum BindType
    {
        Forward,
        Backward,
        Left,
        Right,
        Jump,
        Crouch,
        Interaction,
        Flashlight
    }

    public class BindList
    {
        public static List<BindData> Binds = new();

        public BindList(BindData _bind)
        {
            Binds.Add(_bind);
        }

        static public BindData FindBind(string bindName) 
        {
            BindType bindType;
            if (Enum.TryParse(bindName, out bindType)) return Binds[(int)bindType];
            else return null;
        }
    }
}