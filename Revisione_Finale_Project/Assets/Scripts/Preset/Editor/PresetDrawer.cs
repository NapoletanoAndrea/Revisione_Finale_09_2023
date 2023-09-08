using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Preset<>))]
public class PresetDrawer : PropertyDrawer
{
	private int _editorIndex;
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		var sizeProperty = property.FindPropertyRelative("_size");
		EditorGUILayout.PropertyField(sizeProperty);
		
		var currentIndexProperty = property.FindPropertyRelative("_currentIndex");
		EditorGUILayout.PropertyField(currentIndexProperty);
		
		var currentTagProperty = property.FindPropertyRelative("_currentTag");
		EditorGUILayout.PropertyField(currentTagProperty);

		EditorGUILayout.BeginHorizontal();
		
		for (int i = 0; i < sizeProperty.intValue; i++)
		{
			GUIStyle style = new(GUI.skin.button);
			if (_editorIndex == i)
			{
				GUI.color = Color.yellow;
			}
			
			if (GUILayout.Button(i.ToString(), style))
			{
				_editorIndex = i;
			}
			
			if (_editorIndex == i)
			{
				GUI.color = Color.white;
			}
		}
		
		EditorGUILayout.EndHorizontal();
		
		var tagsProperty = property.FindPropertyRelative("_tags");
		tagsProperty.arraySize = sizeProperty.intValue;

		if (tagsProperty.arraySize > 0)
		{
			EditorGUILayout.PropertyField(tagsProperty.GetArrayElementAtIndex(_editorIndex), new GUIContent("Tag"));
		}

		var valuesProperty = property.FindPropertyRelative("values");
		valuesProperty.arraySize = sizeProperty.intValue;

		if (valuesProperty.arraySize > 0)
		{
			EditorGUILayout.PropertyField(valuesProperty.GetArrayElementAtIndex(_editorIndex));
		}

	}

}