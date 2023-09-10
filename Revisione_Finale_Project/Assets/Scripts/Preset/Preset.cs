using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Preset
{
	public abstract void InitElement(int index);
}

[Serializable]
public class Preset<T> : Preset where T : new()
{
	[SerializeField] private int _editorIndex;
	
	[SerializeField, Min(0)] private int _size = 1;
	public int Size => _size;
	[SerializeField] private int _currentIndex;
	public int CurrentIndex => _currentIndex;
	[SerializeField] private string _currentTag;
	public string CurrentTag => _currentTag;
		
	[SerializeField] private List<string> _tags = new();
	public List<T> values = new(1);

	public T Value => values.Count > 0 ? values[_currentIndex] : default;
	public T EditorValue => values.Count > 0 ? values[_editorIndex] : default;

	public override void InitElement(int index)
	{
		values[index] = new T();
	}

	public void Switch(int index)
	{
		if (index >= _size)
		{
			Debug.LogWarning("Preset index out of range!");
			return;
		}
			
		_currentIndex = index;
		_currentTag = _tags[index];
	}

	public void Switch(string tag)
	{
		var tagIndex = _tags.IndexOf(tag);
		if (tagIndex < 0)
		{
			Debug.LogWarning(tag + " not present in preset!");
			return;
		}

		_currentIndex = tagIndex;
		_currentTag = tag;
	}
}