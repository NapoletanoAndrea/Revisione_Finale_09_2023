using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Preset<>))]
public class PresetDrawer : PropertyDrawer
{
	private bool _isOpen;
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		_isOpen = EditorPrefs.GetBool(nameof(_isOpen), false);
		_isOpen = EditorGUILayout.Foldout(_isOpen, property.name.SerializedName());
		EditorPrefs.SetBool(nameof(_isOpen), _isOpen);
		if (!_isOpen)
			return;

		EditorGUI.indentLevel++;
		var editorIndexProperty = property.FindPropertyRelative("_editorIndex");

		var sizeProperty = property.FindPropertyRelative("_size");
		
		var tagsProperty = property.FindPropertyRelative("_tags");
		tagsProperty.arraySize = sizeProperty.intValue;
		
		var valuesProperty = property.FindPropertyRelative("values");

		var currentIndexProperty = property.FindPropertyRelative("_currentIndex");
		if(sizeProperty.intValue > 0)
			EditorGUILayout.IntSlider(currentIndexProperty, 0, sizeProperty.intValue - 1);
		
		if (currentIndexProperty.intValue >= sizeProperty.intValue)
			currentIndexProperty.intValue = Mathf.Max(0, sizeProperty.intValue - 1);

		if (sizeProperty.intValue > 0)
		{
			var currentTagProperty = property.FindPropertyRelative("_currentTag");
			EditorGUILayout.PropertyField(currentTagProperty);
		}

		EditorGUILayout.BeginHorizontal();

		if (editorIndexProperty.intValue >= sizeProperty.intValue)
		{
			editorIndexProperty.intValue = 0;
		}
		
		if (GUILayout.Button("-"))
		{
			bool response = EditorUtility.DisplayDialog("Preset", $"Are you sure you want to delete Preset {editorIndexProperty.intValue}?", "Yes", "No");
			if (response)
			{
				tagsProperty.DeleteArrayElementAtIndex(editorIndexProperty.intValue);
				valuesProperty.DeleteArrayElementAtIndex(editorIndexProperty.intValue);
				sizeProperty.intValue--;
				editorIndexProperty.intValue = Mathf.Max(0, editorIndexProperty.intValue - 1);
				property.serializedObject.ApplyModifiedProperties();
			}
			GUIUtility.ExitGUI();
		}
		
		for (int i = 0; i < sizeProperty.intValue; i++)
		{
			GUIStyle style = new(GUI.skin.button);
			if (editorIndexProperty.intValue == i)
			{
				GUI.color = Color.yellow;
			}

			string tagString = i.ToString();
			string currentTag = tagsProperty.GetArrayElementAtIndex(i).stringValue;
			if (currentTag.Length > 0)
			{
				tagString += ": " + currentTag;
			}
			
			if (GUILayout.Button(tagString, style))
			{
				editorIndexProperty.intValue = i;
				GUI.FocusControl("");
			}
			
			if (editorIndexProperty.intValue == i)
			{
				GUI.color = Color.white;
			}
		}
		
		if (GUILayout.Button("+"))
		{
			sizeProperty.intValue++;
		}
		
		EditorGUILayout.EndHorizontal();
		
		tagsProperty.arraySize = sizeProperty.intValue;

		if (tagsProperty.arraySize > 0)
		{
			EditorGUILayout.PropertyField(tagsProperty.GetArrayElementAtIndex(editorIndexProperty.intValue), new GUIContent("Tag"));
		}
		
		var prevArraySize = valuesProperty.arraySize;
		valuesProperty.arraySize = sizeProperty.intValue;
		property.serializedObject.ApplyModifiedProperties();

		if (valuesProperty.arraySize > 0)
		{
			var valueProperty = valuesProperty.GetArrayElementAtIndex(editorIndexProperty.intValue);
			EditorGUILayout.PropertyField(valueProperty, new GUIContent(valueProperty.type.SerializedName()));
			
			Preset preset = (Preset) property.GetValue();
			var sizeDiff = valuesProperty.arraySize - prevArraySize;
			for (int i = 0; i < sizeDiff; i++)
			{
				preset.InitElement(i + prevArraySize);
			}
		}

	}

}