using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    /// <summary>
    /// Class for painting the UI text.
    /// </summary>
    public static class TextPainter
	{
		// COMMON
		public static readonly Color32 DefaultTextColor = new Color32(162, 170, 181, 255);

		public static void PaintRedYellowGrayGreen(
			TextMeshProUGUI textMesh,
			float evaluatingValue,
			float smallerOrEqualAreRed,
			float smallerOrEqualAreYellow,
			float largerOrEqualAreGreen)
		{
			textMesh.color = DefaultTextColor;

			if (evaluatingValue >= largerOrEqualAreGreen)
			{
				textMesh.color = Color.green;
			}

			// Gray zone here

			if (evaluatingValue <= smallerOrEqualAreYellow)
			{
				textMesh.color = Color.yellow;
			}

			if (evaluatingValue <= smallerOrEqualAreRed)
			{
				textMesh.color = Color.red;
			}
		}

		public static void PaintRedYellowGrayGreen2(
			TextMeshProUGUI textMesh,
			float evaluatingValue,
			float largerOrEqualAreRed,
			float largerOrEqualAreYellow,
			float smallerOrEqualAreGreen)
		{
			textMesh.color = DefaultTextColor;

			if (evaluatingValue <= smallerOrEqualAreGreen)
			{
				textMesh.color = Color.green;
			}

			// Gray zone here

			if (evaluatingValue >= largerOrEqualAreYellow)
			{
				textMesh.color = Color.yellow;
			}

			if (evaluatingValue >= largerOrEqualAreRed)
			{
				textMesh.color = Color.red;
			}
		}

		public static void PaintRedYellowGreen(
			TextMeshProUGUI textMesh,
			float evaluatingValue,
			float smallerOrEqualAreRed,
			float smallerOrEqualAreYellow)
		{
			textMesh.color = Color.green;

			if (evaluatingValue <= smallerOrEqualAreYellow)
			{
				textMesh.color = Color.yellow;
			}

			if (evaluatingValue <= smallerOrEqualAreRed)
			{
				textMesh.color = Color.red;
			}
		}


		public static void PaintGreenGrayYellowRed(
			TextMeshProUGUI textMesh,
			float evaluatingValue,
			float smallerOrEqualAreGreen,
			float largerOrEqualAreYellow,
			float largerOrEqualAreRed)
		{
			textMesh.color = DefaultTextColor;

			if (evaluatingValue <= smallerOrEqualAreGreen)
			{
				textMesh.color = Color.green;
			}

			if (evaluatingValue >= largerOrEqualAreYellow)
			{
				textMesh.color = Color.yellow;
			}

			if (evaluatingValue >= largerOrEqualAreRed)
			{
				textMesh.color = Color.red;
			}


		}

		public static void PaintChildDefaultColor(Transform child)
		{
			if (child != null && child.GetComponent<TextMeshProUGUI>() != null)
			{
				child.GetComponent<TextMeshProUGUI>().color = TextPainter.DefaultTextColor;
			}
		}
	}
}
