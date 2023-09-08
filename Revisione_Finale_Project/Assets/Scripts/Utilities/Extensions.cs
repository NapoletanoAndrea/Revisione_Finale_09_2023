using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
		
	#region MonoBehaviour
		
	public static T GetOrAddComponent<T>(this MonoBehaviour mono) where T : Component
	{
		T component = mono.GetComponent<T>();
		return component != null ? component : mono.gameObject.AddComponent<T>();
	}

	#endregion
		
	#region LayerMask
		
	public static LayerMask ToLayerMask(this int layer)
	{
		return 1 << layer;
	}
    
	public static bool Contains(this LayerMask layerMask, int layer)
	{
		return layerMask == (layerMask | (1 << layer));
	}
		
	#endregion
		
	#region Vector2

	public static Vector3 InputToDirection(this Vector2 input)
	{
		return new Vector3(input.x, 0, input.y);
	}

	public static Vector3 InputToDirection(this Vector2 input, Transform camera)
	{
		var camForward = camera.forward;
		camForward.y = 0;

		var camRight = camera.right;
		
		
		return input.x * camRight.normalized + input.y * camForward.normalized;
	}
		
	#endregion

	#region Bool

	public static int ToInt(this bool value, int falseValue, int trueValue)
	{
		return value ? trueValue : falseValue;
	}

	public static int ToInt(this bool value)
	{
		return value.ToInt(0, 1);
	}

	public static QueryTriggerInteraction ToQueryTriggerInteraction(this bool value)
	{
		return value ? QueryTriggerInteraction.Collide : QueryTriggerInteraction.Ignore;
	}

	#endregion
	
	#region List

	public static bool AddUnique<T>(this List<T> list, T element)
	{
		if (!list.Contains(element))
		{
			list.Add(element);
			return true;
		}
		return false;
	}
	
	#endregion

}