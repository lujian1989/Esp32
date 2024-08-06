using System;
using System.ComponentModel;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.TerrainTools;
#endif
using UnityEngine;

namespace TimelineViewer
{
    // we have to use UDateTime instead of DateTime on our classes
    // we still typically need to either cast this to a DateTime or read the DateTime field directly
    [System.Serializable]
    public class UnityDateTime : ISerializationCallbackReceiver
    {
        [HideInInspector] public DateTime m_DateTime;

        // if you don't want to use the PropertyDrawer then remove HideInInspector here
        [HideInInspector][SerializeField] private string _dateTime;

        public static implicit operator DateTime(UnityDateTime udt)
        {
            return (udt.m_DateTime);
        }

        public static implicit operator UnityDateTime(DateTime dt)
        {
            return new UnityDateTime() { m_DateTime = dt };
        }

        public void OnAfterDeserialize()
        {
            DateTime.TryParse(_dateTime, out m_DateTime);
        }

        public void OnBeforeSerialize()
        {
            _dateTime = m_DateTime.ToString();
        }
    }

    // if we implement this PropertyDrawer then we keep the label next to the text field
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(UnityDateTime))]
    public class UnityDateTimeDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            Rect amountRect = new Rect(position.x, position.y, position.width, position.height);


            //GUI.enabled = false;
            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            //EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("_dateTime"), GUIContent.none);
            //EditorGUI.LabelField(amountRect, label.text, property.FindPropertyRelative("_dateTime").stringValue);
            //GUI.enabled = true;   
            var content = new GUIContent(property.FindPropertyRelative("_dateTime").stringValue);

            if (EditorGUI.DropdownButton(amountRect, content, FocusType.Keyboard))
            {
                if (UiCalendarWindow.IsDisplayed)
                {
                    //WarningPopup.Init("Only one instance of UICalendar allowed", "Ok");
                    Debug.Log("Only one instance of UICalendar allowed");
                    return;
                }

                UiCalendarWindow.Init();
                UiCalendarWindow.OnDateSelected.AddListener((selecteDate) =>
                {
                    property.FindPropertyRelative("_dateTime").stringValue = selecteDate.ToString();
                    property.serializedObject.ApplyModifiedProperties();
                    property.serializedObject.Update();
                });
            }

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

    }
#endif
}