using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using XJ.Unity3D.GUI.FieldGUIComponents;

namespace XJ.Unity3D.GUI
{
    public class FieldGUI
    {
        public enum FieldType
        {
            Int,
            Float,
            Bool,
            Vector2,
            Vector3,
            Vector4,
            Matrix,
            Enum,
            Unsupported
        }

        private class ActionSet
        {
            // 自分の親の ActionSet が入る。
            public ActionSet parentActionSet;
            // ネストされる ActionSet が含まれる。
            public List<ActionSet> actionSets;

            // 通常のアクションが入る。
            public Action action;
            // タブのアクションが入る。
            public List<Action> tabActions;

            public TabPanel tabPanel;
            public List<string> tabTitles;

            public void Controller()
            {
                if (this.tabPanel != null)
                {
                    this.tabPanel.Controller(this.tabActions, this.tabTitles.ToArray());
                }
                else
                {
                    if (this.action != null)
                    {
                        this.action();
                    }
                }
            }
        }

        #region Field

        private readonly List<FieldGUIComponent> fieldGUIs = new List<FieldGUIComponent>();
        private readonly List<ActionSet> actionSets;

        #endregion Field

        #region Constructor

        public FieldGUI(System.Object data)
        {
            this.actionSets = new List<ActionSet>();
            this.actionSets.Add(new ActionSet());
            GenerateGUIs(data);
        }

        #endregion Constructor

        #region Method

        private void GenerateGUIs(System.Object data)
        {
            FieldInfo[] fieldInfos = data.GetType().GetFields
                (BindingFlags.Public | BindingFlags.Instance);

            if (fieldInfos.Length == 0)
            {
                return;
            }

            for (var i = 0; i < fieldInfos.Length; i++)
            {
                FieldGUIComponent guiComponent = GenerateGUI(data, fieldInfos[i]);

                // --------------------------------------------------------------------------------
                // TabClear が指定されているとき、末尾の ActionSet を変更する。
                // --------------------------------------------------------------------------------

                #region Clear

                if (guiComponent.guiAttribute != null && guiComponent.guiAttribute.TabClear)
                {
                    ActionSet bottomActionSet = BottomSet(this.actionSets);

                    // TabPanel がないのに TabClear を呼ばれている場合。

                    if (bottomActionSet.tabPanel == null)
                    {
                        // Nothing to do.
                        // あるいは更に親の Panel を閉じに行く。
                    }

                    // TabPanel があるとき、新しく ActionSet を追加する。

                    else
                    {
                        if (bottomActionSet.parentActionSet == null)
                        {
                            this.actionSets.Add(new ActionSet());
                        }

                        bottomActionSet.parentActionSet.actionSets.Add(new ActionSet()
                        {
                            parentActionSet = bottomActionSet.parentActionSet
                        });
                    }
                }

                #endregion Clear

                // --------------------------------------------------------------------------------
                // TabTitle が指定されるとき。
                // --------------------------------------------------------------------------------

                #region Tab

                if (guiComponent.guiAttribute != null && guiComponent.guiAttribute.TabTitle != null)
                {
                    ActionSet bottomActionSet = BottomSet(this.actionSets);

                    // ----------------------------------------------------------------------------
                    // (1) 直前の ActionSet に TabPanel が設定されているとき。
                    // 既存の ActionSet のデータに追加する。
                    // ----------------------------------------------------------------------------

                    #region 

                    if (bottomActionSet.tabPanel != null)
                    {
                        bottomActionSet.tabTitles.Add(guiComponent.guiAttribute.TabTitle);
                        bottomActionSet.tabActions.Add(guiComponent.Controller);
                    }

                    #endregion

                    // ----------------------------------------------------------------------------
                    // (2) 直前の ActionSet に TabPanel が設定されないとき。
                    // 新しい ActionSet のデータを追加する。
                    // ----------------------------------------------------------------------------

                    #region

                    else
                    {
                        ActionSet newActionSet = new ActionSet()
                        {
                            parentActionSet = bottomActionSet.parentActionSet,

                            tabPanel = new TabPanel(),
                            tabTitles = new List<string>()
                            {
                                guiComponent.guiAttribute.TabTitle
                            },
                            tabActions = new List<Action>()
                            {
                                guiComponent.Controller
                            }
                        };

                        if (bottomActionSet.parentActionSet == null)
                        {
                            this.actionSets.Add(newActionSet);
                        }
                        else
                        {
                            bottomActionSet.parentActionSet.actionSets.Add(newActionSet);
                        }
                    }

                    #endregion
                }

                #endregion Tab
                
                // --------------------------------------------------------------------------------
                // TabTitle が指定されないとき
                // --------------------------------------------------------------------------------

                #region Default

                else
                {
                    ActionSet bottomActionSet = BottomSet(this.actionSets);

                    // 最後に追加された ActionSet に Tab があれば追加する。

                    if (bottomActionSet.tabPanel != null)
                    {
                        bottomActionSet.tabActions
                        [bottomActionSet.tabActions.Count - 1] += guiComponent.Controller;
                    }

                    // 最後に追加された Action に Tab はないので素直に追加する。

                    else
                    {
                        bottomActionSet.action += guiComponent.Controller;
                    }
                }

                #endregion Default
            }
        }

        public FieldGUIComponent GenerateGUI(System.Object data, FieldInfo fieldInfo)
        {
            FieldType fieldType = GetFieldType(fieldInfo);

            GUIAttribute guiAttribute = Attribute.GetCustomAttribute
                                       (fieldInfo, typeof(GUIAttribute)) as GUIAttribute;

            switch (fieldType)
            {
                case FieldType.Int:
                    return new FieldGUIComponents.IntGUI(data, fieldInfo, guiAttribute);
                case FieldType.Float:
                    return new FieldGUIComponents.FloatGUI(data, fieldInfo, guiAttribute);
                case FieldType.Vector2:
                    return new FieldGUIComponents.Vector2GUI(data, fieldInfo, guiAttribute);
                case FieldType.Vector3:
                    return new FieldGUIComponents.Vector3GUI(data, fieldInfo, guiAttribute);
                case FieldType.Vector4:
                    return new FieldGUIComponents.Vector4GUI(data, fieldInfo, guiAttribute);
                case FieldType.Bool:
                    return new FieldGUIComponents.BoolGUI(data, fieldInfo, guiAttribute);
                case FieldType.Enum:
                    return new FieldGUIComponents.Toolbar(data, fieldInfo, guiAttribute);
                case FieldType.Unsupported:
                    {
                        if (guiAttribute.IPv4)
                        {
                            return new FieldGUIComponents.IPv4GUI(data, fieldInfo);
                        }

                        return new FieldGUIComponents.UnsupportedGUI(data, fieldInfo, guiAttribute);
                    }
                default:
                    return new FieldGUIComponents.UnsupportedGUI(data, fieldInfo, guiAttribute);
            }
        }

        public FieldType GetFieldType(FieldInfo fieldInfo)
        {
            System.Type type = fieldInfo.FieldType;

            if (type.IsPrimitive)
            {
                if (type == typeof(int))
                {
                    return FieldType.Int;
                }

                if (type == typeof(float))
                {
                    return FieldType.Float;
                }

                if (type == typeof(bool))
                {
                    return FieldType.Bool;
                }

                return FieldType.Unsupported;
            }

            if (type.IsEnum)
            {
                return FieldType.Enum;
            }

            if (type.IsValueType)
            {
                if (type == typeof(Vector2))
                {
                    return FieldType.Vector2;
                }

                if (type == typeof(Vector3))
                {
                    return FieldType.Vector3;
                }

                if (type == typeof(Vector4))
                {
                    return FieldType.Vector4;
                }

                if (type == typeof(Matrix4x4))
                {
                    return FieldType.Matrix;
                }
            }

            return FieldType.Unsupported;
        }

        public void Controller()
        {
            for (int i = 0; i < this.actionSets.Count; i++)
            {
                this.actionSets[i].Controller();
            }
        }

        public void Save()
        {
            foreach (FieldGUIComponent fieldGUI in this.fieldGUIs)
            {
                fieldGUI.Save();
            }
        }

        public void Load(System.Object data)
        {
            foreach (FieldGUIComponent fieldGUI in this.fieldGUIs)
            {
                fieldGUI.Load();
            }
        }

        private ActionSet BottomSet(List<ActionSet> actionSets)
        {
            if (actionSets[actionSets.Count - 1].actionSets != null)
            {
                return BottomSet(actionSets[actionSets.Count - 1].actionSets);
            }
            else
            {
                return actionSets[actionSets.Count - 1];
            }
        }

        #endregion Method
    }
}