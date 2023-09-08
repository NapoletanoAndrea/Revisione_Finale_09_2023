using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Well
{
	public int fuck;
	public string tag;
}

[Serializable]
public class Preset<T>
{
	[SerializeField, Min(0)] private int _size;
	[SerializeField] private int _currentIndex;
	public int CurrentIndex => _currentIndex;
	[SerializeField] private string _currentTag;
	public string CurrentTag => _currentTag;
		
	[SerializeField] private List<string> _tags = new();
		
	public T[] values;

	public T Value => values[_currentIndex];

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