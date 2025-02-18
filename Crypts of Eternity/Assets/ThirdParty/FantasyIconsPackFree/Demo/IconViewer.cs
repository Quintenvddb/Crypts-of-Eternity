﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.FantasyIconsPackFree.Demo
{
	/// <summary>
	/// Creates a grid view with icons.
	/// </summary>
	public class IconViewer : MonoBehaviour
	{
		public List<Object> Folders;
		public GameObject Icon;
		public Transform Grid;

		#if UNITY_EDITOR

		public void Start()
		{
			var sprites = new List<Sprite>();

			foreach (var folder in Folders)
			{
				var path = UnityEditor.AssetDatabase.GetAssetPath(folder);

				if (!Directory.Exists(path)) continue;

				foreach (var pattern in new[] { "*.png", "*.psd" })
				{
					sprites.AddRange(Directory.GetFiles(path, pattern, SearchOption.AllDirectories).Select(UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>));
				}
			}

			sprites.Shuffle();
           
			foreach (Transform child in Grid)
			{
				Destroy(child.gameObject);
			}

			foreach (var sprite in sprites)
			{
				var node = Instantiate(Icon, Grid);

				node.GetComponentsInChildren<Image>(true)[1].sprite = sprite;
				node.gameObject.SetActive(true);
			}
		}

		#endif

        public void Navigate(string url)
        {
			Application.OpenURL(url);
        }
	}

	public static class Extensions
	{
		public static List<T> Shuffle<T>(this List<T> source)
		{
			source.Sort((i, j) => Random.Range(0, 3) - 1);

			return source;
		}
	}
}