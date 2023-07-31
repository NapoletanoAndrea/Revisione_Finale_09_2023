using UnityEngine;

namespace CharacterController.Utilities
{

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
		
		#endregion

	}

}