using System.Collections.Generic;
using System.Linq;
using ModApi.Common.Extensions;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    /// <summary>
    /// Help class for manipulation with mesh vertices.
    /// </summary>
    public static class MeshHelper
	{
		private const float MinimalVertexDistance = 0.0001f;
        private const float MiddleYPoint = 0.5f;

        /// <summary>
        /// Swap X coordinates of airfoil vertices. Works only with double-convex airfoils.
        /// </summary>
        /// <param name="airfoilVertices">Vertices of airfoil mesh to swap.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1066:Mergeable \"if\" statements should be combined", Justification = "<Pending>")]
        public static Vector3[] InvertAirfoilVertices(Vector3[] airfoilVertices)
		{
			for (int i = 0; i < airfoilVertices.Length; i++)
			{
				// Root vertices
				if (airfoilVertices[i].y < MiddleYPoint && airfoilVertices[i].x > 0)
				{
					for (int j = 0; j < airfoilVertices.Length; j++)
					{
						if (airfoilVertices[j].y < MiddleYPoint && airfoilVertices[j].x < 0)
						{
							if (i != j)
							{
								if (Mathf.Abs(airfoilVertices[i].z - airfoilVertices[j].z) < MinimalVertexDistance)
								{
									float bufferX = airfoilVertices[i].x;
									airfoilVertices[i] = new Vector3(-airfoilVertices[j].x, airfoilVertices[i].y, airfoilVertices[i].z);
									airfoilVertices[j] = new Vector3(-bufferX, airfoilVertices[j].y, airfoilVertices[j].z);
								}
							}
						}
					}
				}

				// Tip vertices
				if (airfoilVertices[i].y > MiddleYPoint && airfoilVertices[i].x > 0)
				{
					for (int j = 0; j < airfoilVertices.Length; j++)
					{
						if (airfoilVertices[j].y > MiddleYPoint && airfoilVertices[j].x < 0)
						{
							if (i != j)
							{
								if (Mathf.Abs(airfoilVertices[i].z - airfoilVertices[j].z) < MinimalVertexDistance)
								{
									float bufferX = airfoilVertices[i].x;
									airfoilVertices[i] = new Vector3(-airfoilVertices[j].x, airfoilVertices[i].y, airfoilVertices[i].z);
									airfoilVertices[j] = new Vector3(-bufferX, airfoilVertices[j].y, airfoilVertices[j].z);
								}
							}
						}
					}
				}
			}

			return airfoilVertices;
		}

        /// <summary>
        /// Transform original airfoil tip vertices to target airfoil tip vertices.
        /// </summary>
        /// <param name="originalAirfoilVertices">All vertices from original airfoil (root airfoil).</param>
        /// <param name="targetAirfoilVertices">All vertices from target airfoil (tip airfoil).</param>
        /// <returns>Transformed vertices to target airfoil.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1066:Mergeable \"if\" statements should be combined", Justification = "<Pending>")]
        public static Vector3[] TransformTipVertices(Vector3[] originalAirfoilVertices, Vector3[] targetAirfoilVertices)
		{
			var originalTipPositive = originalAirfoilVertices
				.Select((vertex, index) => new { vertex, index })
				.Where(vertex => vertex.vertex.y > MiddleYPoint && vertex.vertex.x > MinimalVertexDistance)
				.OrderBy(vertex => vertex.vertex.z)
				.ToArray();
			var targetTipPositive = targetAirfoilVertices
				.Where(vertex => vertex.y > MiddleYPoint && vertex.x > MinimalVertexDistance)
				.OrderBy(vertex => vertex.z)
				.ToArray();

			var originalTipNegative = originalAirfoilVertices
				.Select((vertex, index) => new { vertex, index })
				.Where(vertex => vertex.vertex.y > MiddleYPoint && vertex.vertex.x < MinimalVertexDistance)
				.OrderBy(vertex => vertex.vertex.z)
				.ToArray();
			var targetTipNegative = targetAirfoilVertices
				.Where(vertex => vertex.y > MiddleYPoint && vertex.x < MinimalVertexDistance)
				.OrderBy(vertex => vertex.z)
				.ToArray();

			if ((originalTipPositive.Length != targetTipPositive.Length)
				|| (originalTipNegative.Length != targetTipNegative.Length))
			{
				Game.Instance.DevConsole.LogError(@$"{nameof(MeshHelper)}: {nameof(TransformTipVertices)} error.
					Root airfoil vertices count: {originalTipPositive.Length + originalTipNegative.Length},
					tip airfoil vertices count: {targetTipPositive.Length + targetTipNegative.Length}");
				return originalAirfoilVertices;
			}

			Dictionary<int, Vector3> newTipPositivePosition = new Dictionary<int, Vector3>();
			Dictionary<int, Vector3> newTipNegativePosition = new Dictionary<int, Vector3>();
			for (int i = 0; i < originalTipPositive.Length; i++)
			{
				newTipPositivePosition.Add(originalTipPositive[i].index, targetTipPositive[i]);
			}
			for (int i = 0; i < originalTipNegative.Length; i++)
			{
				newTipNegativePosition.Add(originalTipNegative[i].index, targetTipNegative[i]);
			}

			for (int i = 0; i < originalAirfoilVertices.Length; i++)
			{
				if (originalAirfoilVertices[i].y > MiddleYPoint && originalAirfoilVertices[i].x > MinimalVertexDistance)
				{
					if (newTipPositivePosition.TryGetValue(i, out Vector3 newPosition))
					{
						originalAirfoilVertices[i] = new Vector3(newPosition.x, newPosition.y, newPosition.z);
					}
				}
			}
			for (int i = 0; i < originalAirfoilVertices.Length; i++)
			{
				if (originalAirfoilVertices[i].y > MiddleYPoint && originalAirfoilVertices[i].x < MinimalVertexDistance)
				{
					if (newTipNegativePosition.TryGetValue(i, out Vector3 newPosition))
					{
						originalAirfoilVertices[i] = new Vector3(newPosition.x, newPosition.y, newPosition.z);
					}
				}
			}

			return originalAirfoilVertices;
		}

		/// <summary>
		/// Rotate tip vertices of the wing around pivot.
		/// </summary>
		/// <param name="wingVertices">Vertices of the wing.</param>
		/// <param name="rotationAngle">Angle of rotation.</param>
		/// <param name="pivot">Pivot point.</param>
		/// <returns>Rotated tip vertices.</returns>
		public static Vector3[] RotateTipVerticesAroundPivot(Vector3[] wingVertices, float rotationAngle, Vector3 pivot)
		{
			for (int i = 0; i < wingVertices.Length; i++)
			{
				if (wingVertices[i].y > MiddleYPoint)
				{
					wingVertices[i] = Quaternion.Euler(0f, rotationAngle, 0f) * (wingVertices[i] - pivot) + pivot;
				}
			}

			return wingVertices;
		}

		/// <summary>
		/// Rotates the point around pivot.
		/// </summary>
		/// <param name="point">Point.</param>
		/// <param name="rotationAngle">Rotation angle.</param>
		/// <param name="pivot">Pivot</param>
		/// <returns>Rotated point.</returns>
		public static Vector3 RotatePointAroundPivot(Vector3 point, float rotationAngle, Vector3 pivot)
		{
			return Quaternion.Euler(0f, rotationAngle, 0f) * (point - pivot) + pivot;
		}

		public static Vector3[] SliceWingFromControlSurface(
			Vector3[] wingVertices,
			float surfacePercentage,
			float middleWingYPoint,
			float rootScaleMultiplier,
			float rootScaleOffset,
			float tipScaleMultiplier,
			float tipScaleOffset,
			float tipOffset,
			bool isControlSurfaceBorderRounded)
		{
			const float SliceFromPoint = -0.5f;
			(float rootPositiveSlicePointX, float rootNegativeSlicePointX, float tipPositiveSlicePointX, float tipNegativeSlicePointX, float rootSlicePoint, float tipSlicePoint) =
				CalculateSlicePoint(wingVertices, SliceFromPoint, surfacePercentage, middleWingYPoint, rootScaleMultiplier, rootScaleOffset, tipScaleMultiplier, tipScaleOffset, tipOffset);

			float rootSliceThickness = rootPositiveSlicePointX - rootNegativeSlicePointX;
			float rootSliceHalfThickness = rootSliceThickness / 2f;
			float rootSliceMiddlePoint = rootPositiveSlicePointX - rootSliceHalfThickness;

			float tipSliceThickness = tipPositiveSlicePointX - tipNegativeSlicePointX;
			float tipSliceHalfThickness = tipSliceThickness / 2f;
			float tipSliceMiddlePoint = tipPositiveSlicePointX - tipSliceHalfThickness;

			List<KeyValuePair<float, int>> rootPositiveVertices = new List<KeyValuePair<float, int>>();
			List<KeyValuePair<float, int>> rootNegativeVertices = new List<KeyValuePair<float, int>>();
			List<KeyValuePair<float, int>> tipPositiveVertices = new List<KeyValuePair<float, int>>();
			List<KeyValuePair<float, int>> tipNegativeVertices = new List<KeyValuePair<float, int>>();

			for (int i = 0; i < wingVertices.Length; i++)
			{
				bool vertexBelongRoot = wingVertices[i].y < middleWingYPoint;
				bool vertexBehindRootSlicePoint = wingVertices[i].z < rootSlicePoint;
				bool vertexXPositive = wingVertices[i].x > 0f;
				bool vertexXNegative = wingVertices[i].x < 0f;

				if (vertexBelongRoot && vertexBehindRootSlicePoint && vertexXPositive)
				{
					if (isControlSurfaceBorderRounded)
					{
						rootPositiveVertices.Add(new KeyValuePair<float, int>(wingVertices[i].z, i));
					}

					wingVertices[i] = new Vector3(rootPositiveSlicePointX, wingVertices[i].y, rootSlicePoint);
				}

				if (vertexBelongRoot && vertexBehindRootSlicePoint && vertexXNegative)
				{
					if (isControlSurfaceBorderRounded)
					{
						rootNegativeVertices.Add(new KeyValuePair<float, int>(wingVertices[i].z, i));
					}

					wingVertices[i] = new Vector3(rootNegativeSlicePointX, wingVertices[i].y, rootSlicePoint);
				}
					
				if (vertexBelongRoot && vertexBehindRootSlicePoint && !vertexXPositive && !vertexXNegative)
				{
					wingVertices[i] = new Vector3(wingVertices[i].x, wingVertices[i].y, rootSlicePoint);

					if (isControlSurfaceBorderRounded)
					{
						wingVertices[i] = new Vector3(rootSliceMiddlePoint, wingVertices[i].y, rootSlicePoint + rootSliceHalfThickness);
					}
				}
					
				bool vertexBelongTip = wingVertices[i].y > middleWingYPoint;
				bool vertexBehindTipSlicePoint = wingVertices[i].z < tipSlicePoint;

				if (vertexBelongTip && vertexBehindTipSlicePoint && vertexXPositive)
				{
					if (isControlSurfaceBorderRounded)
					{
						tipPositiveVertices.Add(new KeyValuePair<float, int>(wingVertices[i].z, i));
					}

					wingVertices[i] = new Vector3(tipPositiveSlicePointX, wingVertices[i].y, tipSlicePoint);
				}
					
				if (vertexBelongTip && vertexBehindTipSlicePoint && vertexXNegative)
				{
					if (isControlSurfaceBorderRounded)
					{
						tipNegativeVertices.Add(new KeyValuePair<float, int>(wingVertices[i].z, i));
					}

					wingVertices[i] = new Vector3(tipNegativeSlicePointX, wingVertices[i].y, tipSlicePoint);
				}
					
				if (vertexBelongTip && vertexBehindTipSlicePoint && !vertexXPositive && !vertexXNegative)
				{
					wingVertices[i] = new Vector3(wingVertices[i].x, wingVertices[i].y, tipSlicePoint);

					if (isControlSurfaceBorderRounded)
					{
						wingVertices[i] = new Vector3(tipSliceMiddlePoint, wingVertices[i].y, tipSlicePoint + tipSliceHalfThickness);
					}
				}
					
			}

			rootPositiveVertices = rootPositiveVertices.OrderBy(x => x.Key).ToList();
			rootNegativeVertices = rootNegativeVertices.OrderBy(x => x.Key).ToList();
			tipPositiveVertices = tipPositiveVertices.OrderBy(x => x.Key).ToList();
			tipNegativeVertices = tipNegativeVertices.OrderBy(x => x.Key).ToList();

			if (isControlSurfaceBorderRounded)
			{
				int step = 2;

				float rootPositiveAngleStep = 90f / (rootPositiveVertices.Count / step);
				for (int i = 0; i < rootPositiveVertices.Count; i = i + step)
				{
					float z = Mathf.Cos(rootPositiveAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * rootSliceHalfThickness;
					float x = Mathf.Sin(rootPositiveAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * rootSliceHalfThickness;
					int index1 = rootPositiveVertices[i].Value;
					int index2 = rootPositiveVertices[i + 1].Value;
					wingVertices[index1] = new Vector3(rootSliceMiddlePoint + x, wingVertices[index1].y, rootSlicePoint + z);
					wingVertices[index2] = new Vector3(rootSliceMiddlePoint + x, wingVertices[index2].y, rootSlicePoint + z);
				}

				float rootNegativeAngleStep = 90f / (rootNegativeVertices.Count / step);
				for (int i = 0; i < rootNegativeVertices.Count; i = i + step)
				{
					float z = Mathf.Cos(rootNegativeAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * rootSliceHalfThickness;
					float x = Mathf.Sin(rootNegativeAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * rootSliceHalfThickness;
					int index1 = rootNegativeVertices[i].Value;
					int index2 = rootNegativeVertices[i + 1].Value;
					wingVertices[index1] = new Vector3(rootSliceMiddlePoint - x, wingVertices[index1].y, rootSlicePoint + z);
					wingVertices[index2] = new Vector3(rootSliceMiddlePoint - x, wingVertices[index2].y, rootSlicePoint + z);
				}

				float tipPositiveAngleStep = 90f / (tipPositiveVertices.Count / step);
				for (int i = 0; i < tipPositiveVertices.Count; i = i + step)
				{
					float z = Mathf.Cos(tipPositiveAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * tipSliceHalfThickness;
					float x = Mathf.Sin(tipPositiveAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * tipSliceHalfThickness;
					int index1 = tipPositiveVertices[i].Value;
					int index2 = tipPositiveVertices[i + 1].Value;
					wingVertices[index1] = new Vector3(tipSliceMiddlePoint + x, wingVertices[index1].y, tipSlicePoint + z);
					wingVertices[index2] = new Vector3(tipSliceMiddlePoint + x, wingVertices[index2].y, tipSlicePoint + z);
				}

				float tipNegativeAngleStep = 90f / (tipNegativeVertices.Count / step);
				for (int i = 0; i < tipNegativeVertices.Count; i = i + step)
				{
					float z = Mathf.Cos(tipNegativeAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * tipSliceHalfThickness;
					float x = Mathf.Sin(tipNegativeAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * tipSliceHalfThickness;
					int index1 = tipNegativeVertices[i].Value;
					int index2 = tipNegativeVertices[i + 1].Value;
					wingVertices[index1] = new Vector3(tipSliceMiddlePoint - x, wingVertices[index1].y, tipSlicePoint + z);
					wingVertices[index2] = new Vector3(tipSliceMiddlePoint - x, wingVertices[index2].y, tipSlicePoint + z);
				}
			}

			return wingVertices;
		}

		public static Vector3[] SliceControlSurfaceFromWing(
			Vector3[] surfaceVertices,
			Vector3[] unslicedWingVertices,
			float surfacePercentage,
			float middleWingYPoint,
			float rootScaleMultiplier,
			float rootScaleOffset,
			float tipScaleMultiplier,
			float tipScaleOffset,
			float tipOffset,
			bool isControlSurfaceBorderRounded)
		{
			const float SliceFromPoint = -0.5f;

			(float rootPositiveSlicePointX, float rootNegativeSlicePointX, float tipPositiveSlicePointX, float tipNegativeSlicePointX, float rootSlicePoint, float tipSlicePoint) =
				CalculateSlicePoint(unslicedWingVertices, SliceFromPoint, surfacePercentage, middleWingYPoint, rootScaleMultiplier, rootScaleOffset, tipScaleMultiplier, tipScaleOffset, tipOffset);

			float rootSliceThickness = rootPositiveSlicePointX - rootNegativeSlicePointX;
			float rootSliceHalfThickness = rootSliceThickness / 2f;
			float rootSliceMiddlePoint = rootPositiveSlicePointX - rootSliceHalfThickness;

			float tipSliceThickness = tipPositiveSlicePointX - tipNegativeSlicePointX;
			float tipSliceHalfThickness = tipSliceThickness / 2f;
			float tipSliceMiddlePoint = tipPositiveSlicePointX - tipSliceHalfThickness;

			List<KeyValuePair<float, int>> rootPositiveVertices = new List<KeyValuePair<float, int>>();
			List<KeyValuePair<float, int>> rootNegativeVertices = new List<KeyValuePair<float, int>>();
			List<KeyValuePair<float, int>> tipPositiveVertices = new List<KeyValuePair<float, int>>();
			List<KeyValuePair<float, int>> tipNegativeVertices = new List<KeyValuePair<float, int>>();

			for (int i = 0; i < surfaceVertices.Length; i++)
			{
				bool vertexBelongRoot = surfaceVertices[i].y < middleWingYPoint;
				bool vertexAheadRootSlicePoint = surfaceVertices[i].z > rootSlicePoint;
				bool vertexXPositive = surfaceVertices[i].x > 0f;
				bool vertexXNegative = surfaceVertices[i].x < 0f;

				if (vertexBelongRoot && vertexAheadRootSlicePoint && vertexXPositive)
				{
					if (isControlSurfaceBorderRounded)
					{
						rootPositiveVertices.Add(new KeyValuePair<float, int>(surfaceVertices[i].z, i));
					}

					surfaceVertices[i] = new Vector3(rootPositiveSlicePointX, surfaceVertices[i].y, rootSlicePoint);
				}

				if (vertexBelongRoot && vertexAheadRootSlicePoint && vertexXNegative)
				{
					if (isControlSurfaceBorderRounded)
					{
						rootNegativeVertices.Add(new KeyValuePair<float, int>(surfaceVertices[i].z, i));
					}

					surfaceVertices[i] = new Vector3(rootNegativeSlicePointX, surfaceVertices[i].y, rootSlicePoint);
				}

				if (vertexBelongRoot && vertexAheadRootSlicePoint && !vertexXPositive && !vertexXNegative)
				{
					surfaceVertices[i] = new Vector3(surfaceVertices[i].x, surfaceVertices[i].y, rootSlicePoint);

					if (isControlSurfaceBorderRounded)
					{
						surfaceVertices[i] = new Vector3(rootSliceMiddlePoint, surfaceVertices[i].y, rootSlicePoint + rootSliceHalfThickness);
					}
				}

				bool vertexBelongTip = surfaceVertices[i].y > middleWingYPoint;
				bool vertexAheadTipSlicePoint = surfaceVertices[i].z > tipSlicePoint;

				if (vertexBelongTip && vertexAheadTipSlicePoint && vertexXPositive)
				{
					if (isControlSurfaceBorderRounded)
					{
						tipPositiveVertices.Add(new KeyValuePair<float, int>(surfaceVertices[i].z, i));
					}

					surfaceVertices[i] = new Vector3(tipPositiveSlicePointX, surfaceVertices[i].y, tipSlicePoint);
				}

				if (vertexBelongTip && vertexAheadTipSlicePoint && vertexXNegative)
				{
					if (isControlSurfaceBorderRounded)
					{
						tipNegativeVertices.Add(new KeyValuePair<float, int>(surfaceVertices[i].z, i));
					}

					surfaceVertices[i] = new Vector3(tipNegativeSlicePointX, surfaceVertices[i].y, tipSlicePoint);
				}

				if (vertexBelongTip && vertexAheadTipSlicePoint && !vertexXPositive && !vertexXNegative)
				{
					surfaceVertices[i] = new Vector3(surfaceVertices[i].x, surfaceVertices[i].y, tipSlicePoint);

					if (isControlSurfaceBorderRounded)
					{
						surfaceVertices[i] = new Vector3(tipSliceMiddlePoint, surfaceVertices[i].y, tipSlicePoint + tipSliceHalfThickness);
					}	
				}
			}

			rootPositiveVertices = rootPositiveVertices.OrderByDescending(x => x.Key).ToList();
			rootNegativeVertices = rootNegativeVertices.OrderByDescending(x => x.Key).ToList();
			tipPositiveVertices = tipPositiveVertices.OrderByDescending(x => x.Key).ToList();
			tipNegativeVertices = tipNegativeVertices.OrderByDescending(x => x.Key).ToList();

			if (isControlSurfaceBorderRounded)
			{
				int step = 2;

				float rootPositiveAngleStep = 90f / (rootPositiveVertices.Count / step);
				for (int i = 0; i < rootPositiveVertices.Count; i = i + step)
				{
					float z = Mathf.Cos(rootPositiveAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * rootSliceHalfThickness;
					float x = Mathf.Sin(rootPositiveAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * rootSliceHalfThickness;
					int index1 = rootPositiveVertices[i].Value;
					int index2 = rootPositiveVertices[i + 1].Value;
					surfaceVertices[index1] = new Vector3(rootSliceMiddlePoint + x, surfaceVertices[index1].y, rootSlicePoint + z);
					surfaceVertices[index2] = new Vector3(rootSliceMiddlePoint + x, surfaceVertices[index2].y, rootSlicePoint + z);
				}

				float rootNegativeAngleStep = 90f / (rootNegativeVertices.Count / step);
				for (int i = 0; i < rootNegativeVertices.Count; i = i + step)
				{
					float z = Mathf.Cos(rootNegativeAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * rootSliceHalfThickness;
					float x = Mathf.Sin(rootNegativeAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * rootSliceHalfThickness;
					int index1 = rootNegativeVertices[i].Value;
					int index2 = rootNegativeVertices[i + 1].Value;
					surfaceVertices[index1] = new Vector3(rootSliceMiddlePoint - x, surfaceVertices[index1].y, rootSlicePoint + z);
					surfaceVertices[index2] = new Vector3(rootSliceMiddlePoint - x, surfaceVertices[index2].y, rootSlicePoint + z);
				}

				float tipPositiveAngleStep = 90f / (tipPositiveVertices.Count / step);
				for (int i = 0; i < tipPositiveVertices.Count; i = i + step)
				{
					float z = Mathf.Cos(tipPositiveAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * tipSliceHalfThickness;
					float x = Mathf.Sin(tipPositiveAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * tipSliceHalfThickness;
					int index1 = tipPositiveVertices[i].Value;
					int index2 = tipPositiveVertices[i + 1].Value;
					surfaceVertices[index1] = new Vector3(tipSliceMiddlePoint + x, surfaceVertices[index1].y, tipSlicePoint + z);
					surfaceVertices[index2] = new Vector3(tipSliceMiddlePoint + x, surfaceVertices[index2].y, tipSlicePoint + z);
				}

				float tipNegativeAngleStep = 90f / (tipNegativeVertices.Count / step);
				for (int i = 0; i < tipNegativeVertices.Count; i = i + step)
				{
					float z = Mathf.Cos(tipNegativeAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * tipSliceHalfThickness;
					float x = Mathf.Sin(tipNegativeAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * tipSliceHalfThickness;
					int index1 = tipNegativeVertices[i].Value;
					int index2 = tipNegativeVertices[i + 1].Value;
					surfaceVertices[index1] = new Vector3(tipSliceMiddlePoint - x, surfaceVertices[index1].y, tipSlicePoint + z);
					surfaceVertices[index2] = new Vector3(tipSliceMiddlePoint - x, surfaceVertices[index2].y, tipSlicePoint + z);
				}
			}

			return surfaceVertices;
		}

		public static Vector3[] SliceWingFromLeadingEdge(
			Vector3[] wingVertices,
			float leadingEdgePercentage,
			float middleWingYPoint,
			float rootScaleMultiplier,
			float rootScaleOffset,
			float tipScaleMultiplier,
			float tipScaleOffset,
			float tipOffset,
			bool isLeadingEdgeBorderRounded)
		{
			const float SliceFromPoint = 0.5f;
			(float rootPositiveSlicePointX, float rootNegativeSlicePointX, float tipPositiveSlicePointX, float tipNegativeSlicePointX, float rootSlicePoint, float tipSlicePoint) =
				CalculateSlicePoint(wingVertices, SliceFromPoint, leadingEdgePercentage, middleWingYPoint, rootScaleMultiplier, rootScaleOffset, tipScaleMultiplier, tipScaleOffset, tipOffset);

			float rootSliceThickness = rootPositiveSlicePointX - rootNegativeSlicePointX;
			float rootSliceHalfThickness = rootSliceThickness / 2f;
			float rootSliceMiddlePoint = rootPositiveSlicePointX - rootSliceHalfThickness;

			float tipSliceThickness = tipPositiveSlicePointX - tipNegativeSlicePointX;
			float tipSliceHalfThickness = tipSliceThickness / 2f;
			float tipSliceMiddlePoint = tipPositiveSlicePointX - tipSliceHalfThickness;

			List<KeyValuePair<float, int>> rootPositiveVertices = new List<KeyValuePair<float, int>>();
			List<KeyValuePair<float, int>> rootNegativeVertices = new List<KeyValuePair<float, int>>();
			List<KeyValuePair<float, int>> tipPositiveVertices = new List<KeyValuePair<float, int>>();
			List<KeyValuePair<float, int>> tipNegativeVertices = new List<KeyValuePair<float, int>>();

			for (int i = 0; i < wingVertices.Length; i++)
			{
				bool vertexBelongRoot = wingVertices[i].y < middleWingYPoint;
				bool vertexAheadRootSlicePoint = wingVertices[i].z > rootSlicePoint;
				bool vertexXPositive = wingVertices[i].x > 0f;
				bool vertexXNegative = wingVertices[i].x < 0f;

				if (vertexBelongRoot && vertexAheadRootSlicePoint && vertexXPositive)
				{
					if (isLeadingEdgeBorderRounded)
					{
						rootPositiveVertices.Add(new KeyValuePair<float, int>(wingVertices[i].z, i));
					}

					wingVertices[i] = new Vector3(rootPositiveSlicePointX, wingVertices[i].y, rootSlicePoint);
				}	

				if (vertexBelongRoot && vertexAheadRootSlicePoint && vertexXNegative)
				{
					if (isLeadingEdgeBorderRounded)
					{
						rootNegativeVertices.Add(new KeyValuePair<float, int>(wingVertices[i].z, i));
					}

					wingVertices[i] = new Vector3(rootNegativeSlicePointX, wingVertices[i].y, rootSlicePoint);
				}
					

				if (vertexBelongRoot && vertexAheadRootSlicePoint && !vertexXPositive && !vertexXNegative)
				{
					wingVertices[i] = new Vector3(wingVertices[i].x, wingVertices[i].y, rootSlicePoint);

					if (isLeadingEdgeBorderRounded)
					{
						wingVertices[i] = new Vector3(rootSliceMiddlePoint, wingVertices[i].y, rootSlicePoint + rootSliceHalfThickness);
					}
				}
					
				bool vertexBelongTip = wingVertices[i].y > middleWingYPoint;
				bool vertexAheadTipSlicePoint = wingVertices[i].z > tipSlicePoint;

				if (vertexBelongTip && vertexAheadTipSlicePoint && vertexXPositive)
				{
					if (isLeadingEdgeBorderRounded)
					{
						tipPositiveVertices.Add(new KeyValuePair<float, int>(wingVertices[i].z, i));
					}

					wingVertices[i] = new Vector3(tipPositiveSlicePointX, wingVertices[i].y, tipSlicePoint);
				}
					
				if (vertexBelongTip && vertexAheadTipSlicePoint && vertexXNegative)
				{
					if (isLeadingEdgeBorderRounded)
					{
						tipNegativeVertices.Add(new KeyValuePair<float, int>(wingVertices[i].z, i));
					}

					wingVertices[i] = new Vector3(tipNegativeSlicePointX, wingVertices[i].y, tipSlicePoint);
				}

				if (vertexBelongTip && vertexAheadTipSlicePoint && !vertexXPositive && !vertexXNegative)
				{
					wingVertices[i] = new Vector3(wingVertices[i].x, wingVertices[i].y, tipSlicePoint);

					if (isLeadingEdgeBorderRounded)
					{
						wingVertices[i] = new Vector3(tipSliceMiddlePoint, wingVertices[i].y, tipSlicePoint + tipSliceHalfThickness);
					}
				}	
			}

			rootPositiveVertices = rootPositiveVertices.OrderByDescending(x => x.Key).ToList();
			rootNegativeVertices = rootNegativeVertices.OrderByDescending(x => x.Key).ToList();
			tipPositiveVertices = tipPositiveVertices.OrderByDescending(x => x.Key).ToList();
			tipNegativeVertices = tipNegativeVertices.OrderByDescending(x => x.Key).ToList();

			if (isLeadingEdgeBorderRounded)
			{
				int step = 2;

				float rootPositiveAngleStep = 90f / (rootPositiveVertices.Count / step);
				for (int i = 0; i < rootPositiveVertices.Count; i = i + step)
				{
					float z = Mathf.Cos(rootPositiveAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * rootSliceHalfThickness;
					float x = Mathf.Sin(rootPositiveAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * rootSliceHalfThickness;
					int index1 = rootPositiveVertices[i].Value;
					int index2 = rootPositiveVertices[i + 1].Value;
					wingVertices[index1] = new Vector3(rootSliceMiddlePoint + x, wingVertices[index1].y, rootSlicePoint + z);
					wingVertices[index2] = new Vector3(rootSliceMiddlePoint + x, wingVertices[index2].y, rootSlicePoint + z);
				}

				float rootNegativeAngleStep = 90f / (rootNegativeVertices.Count / step);
				for (int i = 0; i < rootNegativeVertices.Count; i = i + step)
				{
					float z = Mathf.Cos(rootNegativeAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * rootSliceHalfThickness;
					float x = Mathf.Sin(rootNegativeAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * rootSliceHalfThickness;
					int index1 = rootNegativeVertices[i].Value;
					int index2 = rootNegativeVertices[i + 1].Value;
					wingVertices[index1] = new Vector3(rootSliceMiddlePoint - x, wingVertices[index1].y, rootSlicePoint + z);
					wingVertices[index2] = new Vector3(rootSliceMiddlePoint - x, wingVertices[index2].y, rootSlicePoint + z);
				}

				float tipPositiveAngleStep = 90f / (tipPositiveVertices.Count / step);
				for (int i = 0; i < tipPositiveVertices.Count; i = i + step)
				{
					float z = Mathf.Cos(tipPositiveAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * tipSliceHalfThickness;
					float x = Mathf.Sin(tipPositiveAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * tipSliceHalfThickness;
					int index1 = tipPositiveVertices[i].Value;
					int index2 = tipPositiveVertices[i + 1].Value;
					wingVertices[index1] = new Vector3(tipSliceMiddlePoint + x, wingVertices[index1].y, tipSlicePoint + z);
					wingVertices[index2] = new Vector3(tipSliceMiddlePoint + x, wingVertices[index2].y, tipSlicePoint + z);
				}

				float tipNegativeAngleStep = 90f / (tipNegativeVertices.Count / step);
				for (int i = 0; i < tipNegativeVertices.Count; i = i + step)
				{
					float z = Mathf.Cos(tipNegativeAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * tipSliceHalfThickness;
					float x = Mathf.Sin(tipNegativeAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * tipSliceHalfThickness;
					int index1 = tipNegativeVertices[i].Value;
					int index2 = tipNegativeVertices[i + 1].Value;
					wingVertices[index1] = new Vector3(tipSliceMiddlePoint - x, wingVertices[index1].y, tipSlicePoint + z);
					wingVertices[index2] = new Vector3(tipSliceMiddlePoint - x, wingVertices[index2].y, tipSlicePoint + z);
				}
			}

			return wingVertices;
		}

		public static Vector3[] SliceLeadingEdgeFromWing(
			Vector3[] leadingEdgeVertices,
			Vector3[] unslicedWingVertices,
			float leadingEdgePercentage,
			float middleWingYPoint,
			float rootScaleMultiplier,
			float rootScaleOffset,
			float tipScaleMultiplier,
			float tipScaleOffset,
			float tipOffset,
			bool isLeadingEdgeBorderRounded)
		{
			const float SliceFromPoint = 0.5f;

			(float rootPositiveSlicePointX, float rootNegativeSlicePointX, float tipPositiveSlicePointX, float tipNegativeSlicePointX, float rootSlicePoint, float tipSlicePoint) =
				CalculateSlicePoint(unslicedWingVertices, SliceFromPoint, leadingEdgePercentage, middleWingYPoint, rootScaleMultiplier, rootScaleOffset, tipScaleMultiplier, tipScaleOffset, tipOffset);

			float rootSliceThickness = rootPositiveSlicePointX - rootNegativeSlicePointX;
			float rootSliceHalfThickness = rootSliceThickness / 2f;
			float rootSliceMiddlePoint = rootPositiveSlicePointX - rootSliceHalfThickness;

			float tipSliceThickness = tipPositiveSlicePointX - tipNegativeSlicePointX;
			float tipSliceHalfThickness = tipSliceThickness / 2f;
			float tipSliceMiddlePoint = tipPositiveSlicePointX - tipSliceHalfThickness;

			List<KeyValuePair<float, int>> rootPositiveVertices = new List<KeyValuePair<float, int>>();
			List<KeyValuePair<float, int>> rootNegativeVertices = new List<KeyValuePair<float, int>>();
			List<KeyValuePair<float, int>> tipPositiveVertices = new List<KeyValuePair<float, int>>();
			List<KeyValuePair<float, int>> tipNegativeVertices = new List<KeyValuePair<float, int>>();

			for (int i = 0; i < leadingEdgeVertices.Length; i++)
			{
				bool vertexBelongRoot = leadingEdgeVertices[i].y < middleWingYPoint;
				bool vertexBehindRootSlicePoint = leadingEdgeVertices[i].z < rootSlicePoint;
				bool vertexXPositive = leadingEdgeVertices[i].x > 0f;
				bool vertexXNegative = leadingEdgeVertices[i].x < 0f;

				if (vertexBelongRoot && vertexBehindRootSlicePoint && vertexXPositive)
				{
					if (isLeadingEdgeBorderRounded)
					{
						rootPositiveVertices.Add(new KeyValuePair<float, int>(leadingEdgeVertices[i].z, i));
					}

					leadingEdgeVertices[i] = new Vector3(rootPositiveSlicePointX, leadingEdgeVertices[i].y, rootSlicePoint);
				}
					
				if (vertexBelongRoot && vertexBehindRootSlicePoint && vertexXNegative)
				{
					if (isLeadingEdgeBorderRounded)
					{
						rootNegativeVertices.Add(new KeyValuePair<float, int>(leadingEdgeVertices[i].z, i));
					}

					leadingEdgeVertices[i] = new Vector3(rootNegativeSlicePointX, leadingEdgeVertices[i].y, rootSlicePoint);
				}
					
				if (vertexBelongRoot && vertexBehindRootSlicePoint && !vertexXPositive && !vertexXNegative)
				{
					leadingEdgeVertices[i] = new Vector3(leadingEdgeVertices[i].x, leadingEdgeVertices[i].y, rootSlicePoint);

					if (isLeadingEdgeBorderRounded)
					{
						leadingEdgeVertices[i] = new Vector3(rootSliceMiddlePoint, leadingEdgeVertices[i].y, rootSlicePoint + rootSliceHalfThickness);
					}
				}

				bool vertexBelongTip = leadingEdgeVertices[i].y > middleWingYPoint;
				bool vertexBehindTipSlicePoint = leadingEdgeVertices[i].z < tipSlicePoint;

				if (vertexBelongTip && vertexBehindTipSlicePoint && vertexXPositive)
				{
					if (isLeadingEdgeBorderRounded)
					{
						tipPositiveVertices.Add(new KeyValuePair<float, int>(leadingEdgeVertices[i].z, i));
					}

					leadingEdgeVertices[i] = new Vector3(tipPositiveSlicePointX, leadingEdgeVertices[i].y, tipSlicePoint);
				}
					
				if (vertexBelongTip && vertexBehindTipSlicePoint && vertexXNegative)
				{
					if (isLeadingEdgeBorderRounded)
					{
						tipNegativeVertices.Add(new KeyValuePair<float, int>(leadingEdgeVertices[i].z, i));
					}

					leadingEdgeVertices[i] = new Vector3(tipNegativeSlicePointX, leadingEdgeVertices[i].y, tipSlicePoint);
				}
					
				if (vertexBelongTip && vertexBehindTipSlicePoint && !vertexXPositive && !vertexXNegative)
				{
					leadingEdgeVertices[i] = new Vector3(leadingEdgeVertices[i].x, leadingEdgeVertices[i].y, tipSlicePoint);

					if (isLeadingEdgeBorderRounded)
					{
						leadingEdgeVertices[i] = new Vector3(tipSliceMiddlePoint, leadingEdgeVertices[i].y, tipSlicePoint + tipSliceHalfThickness);
					}
				}
			}

			rootPositiveVertices = rootPositiveVertices.OrderBy(x => x.Key).ToList();
			rootNegativeVertices = rootNegativeVertices.OrderBy(x => x.Key).ToList();
			tipPositiveVertices = tipPositiveVertices.OrderBy(x => x.Key).ToList();
			tipNegativeVertices = tipNegativeVertices.OrderBy(x => x.Key).ToList();

			if (isLeadingEdgeBorderRounded)
			{
				int step = 2;

				float rootPositiveAngleStep = 90f / (rootPositiveVertices.Count / step);
				for (int i = 0; i < rootPositiveVertices.Count; i = i + step)
				{
					float z = Mathf.Cos(rootPositiveAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * rootSliceHalfThickness;
					float x = Mathf.Sin(rootPositiveAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * rootSliceHalfThickness;
					int index1 = rootPositiveVertices[i].Value;
					int index2 = rootPositiveVertices[i + 1].Value;
					leadingEdgeVertices[index1] = new Vector3(rootSliceMiddlePoint + x, leadingEdgeVertices[index1].y, rootSlicePoint + z);
					leadingEdgeVertices[index2] = new Vector3(rootSliceMiddlePoint + x, leadingEdgeVertices[index2].y, rootSlicePoint + z);
				}

				float rootNegativeAngleStep = 90f / (rootNegativeVertices.Count / step);
				for (int i = 0; i < rootNegativeVertices.Count; i = i + step)
				{
					float z = Mathf.Cos(rootNegativeAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * rootSliceHalfThickness;
					float x = Mathf.Sin(rootNegativeAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * rootSliceHalfThickness;
					int index1 = rootNegativeVertices[i].Value;
					int index2 = rootNegativeVertices[i + 1].Value;
					leadingEdgeVertices[index1] = new Vector3(rootSliceMiddlePoint - x, leadingEdgeVertices[index1].y, rootSlicePoint + z);
					leadingEdgeVertices[index2] = new Vector3(rootSliceMiddlePoint - x, leadingEdgeVertices[index2].y, rootSlicePoint + z);
				}

				float tipPositiveAngleStep = 90f / (tipPositiveVertices.Count / step);
				for (int i = 0; i < tipPositiveVertices.Count; i = i + step)
				{
					float z = Mathf.Cos(tipPositiveAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * tipSliceHalfThickness;
					float x = Mathf.Sin(tipPositiveAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * tipSliceHalfThickness;
					int index1 = tipPositiveVertices[i].Value;
					int index2 = tipPositiveVertices[i + 1].Value;
					leadingEdgeVertices[index1] = new Vector3(tipSliceMiddlePoint + x, leadingEdgeVertices[index1].y, tipSlicePoint + z);
					leadingEdgeVertices[index2] = new Vector3(tipSliceMiddlePoint + x, leadingEdgeVertices[index2].y, tipSlicePoint + z);
				}

				float tipNegativeAngleStep = 90f / (tipNegativeVertices.Count / step);
				for (int i = 0; i < tipNegativeVertices.Count; i = i + step)
				{
					float z = Mathf.Cos(tipNegativeAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * tipSliceHalfThickness;
					float x = Mathf.Sin(tipNegativeAngleStep * (i / step + 1f) * Mathf.Deg2Rad) * tipSliceHalfThickness;
					int index1 = tipNegativeVertices[i].Value;
					int index2 = tipNegativeVertices[i + 1].Value;
					leadingEdgeVertices[index1] = new Vector3(tipSliceMiddlePoint - x, leadingEdgeVertices[index1].y, tipSlicePoint + z);
					leadingEdgeVertices[index2] = new Vector3(tipSliceMiddlePoint - x, leadingEdgeVertices[index2].y, tipSlicePoint + z);
				}
			}

			return leadingEdgeVertices;
		}

		public static (float rootPositiveSlicePointX, float rootNegativeSlicePointX, float tipPositiveSlicePointX, float tipNegativeSlicePointX, float rootSlicePoint, float tipSlicePoint) CalculateSlicePoint(
			Vector3[] wingVertices,
			float sliceFromPoint,
			float surfacePercentage,
			float middleWingYPoint,
			float rootScaleMultiplier,
			float rootScaleOffset,
			float tipScaleMultiplier,
			float tipScaleOffset,
			float tipOffset)
		{
			surfacePercentage = surfacePercentage / 100f;
			float sliceLength = sliceFromPoint <= 0f ? sliceFromPoint + surfacePercentage : sliceFromPoint - surfacePercentage;
			float rootSlicePoint = sliceLength * rootScaleMultiplier + rootScaleOffset;
			float tipSlicePoint = sliceLength * tipScaleMultiplier + tipScaleOffset + tipOffset;

			float rootPositiveClosestBehindSlicePointZ = Mathf.NegativeInfinity;
			float rootPositiveClosestBehindSlicePointX = 0f;
			float rootPositiveClosestAheadSlicePointZ = Mathf.Infinity;
			float rootPositiveClosestAheadSlicePointX = 0f;
			float rootPositiveSlicePointX = 0f;

			float rootNegativeClosestBehindSlicePointZ = Mathf.NegativeInfinity;
			float rootNegativeClosestBehindSlicePointX = 0f;
			float rootNegativeClosestAheadSlicePointZ = Mathf.Infinity;
			float rootNegativeClosestAheadSlicePointX = 0f;
			float rootNegativeSlicePointX = 0f;

			float tipPositiveClosestBehindSlicePointZ = Mathf.NegativeInfinity;
			float tipPositiveClosestBehindSlicePointX = 0f;
			float tipPositiveClosestAheadSlicePointZ = Mathf.Infinity;
			float tipPositiveClosestAheadSlicePointX = 0f;
			float tipPositiveSlicePointX = 0f;

			float tipNegativeClosestBehindSlicePointZ = Mathf.NegativeInfinity;
			float tipNegativeClosestBehindSlicePointX = 0f;
			float tipNegativeClosestAheadSlicePointZ = Mathf.Infinity;
			float tipNegativeClosestAheadSlicePointX = 0f;
			float tipNegativeSlicePointX = 0f;

			for (int i = 0; i < wingVertices.Length; i++)
			{
				bool vertexBelongRoot = wingVertices[i].y < middleWingYPoint;
				bool vertexBelongTip = wingVertices[i].y > middleWingYPoint;
				bool vertexXPositive = wingVertices[i].x > 0f;
				bool vertexXNegative = wingVertices[i].x < 0f;

				//Root positive
				if (vertexBelongRoot && vertexXPositive)
				{
					if (wingVertices[i].z <= rootSlicePoint && wingVertices[i].z > rootPositiveClosestBehindSlicePointZ)
					{
						rootPositiveClosestBehindSlicePointZ = wingVertices[i].z;
						rootPositiveClosestBehindSlicePointX = wingVertices[i].x;
					}

					if (wingVertices[i].z > rootSlicePoint && wingVertices[i].z < rootPositiveClosestAheadSlicePointZ)
					{
						rootPositiveClosestAheadSlicePointZ = wingVertices[i].z;
						rootPositiveClosestAheadSlicePointX = wingVertices[i].x;
					}
				}

				//Root negative
				if (vertexBelongRoot && vertexXNegative)
				{
					if (wingVertices[i].z <= rootSlicePoint && wingVertices[i].z > rootNegativeClosestBehindSlicePointZ)
					{
						rootNegativeClosestBehindSlicePointZ = wingVertices[i].z;
						rootNegativeClosestBehindSlicePointX = wingVertices[i].x;
					}

					if (wingVertices[i].z > rootSlicePoint && wingVertices[i].z < rootNegativeClosestAheadSlicePointZ)
					{
						rootNegativeClosestAheadSlicePointZ = wingVertices[i].z;
						rootNegativeClosestAheadSlicePointX = wingVertices[i].x;
					}
				}

				//Tip positive
				if (vertexBelongTip && vertexXPositive)
				{
					if (wingVertices[i].z <= tipSlicePoint && wingVertices[i].z > tipPositiveClosestBehindSlicePointZ)
					{
						tipPositiveClosestBehindSlicePointZ = wingVertices[i].z;
						tipPositiveClosestBehindSlicePointX = wingVertices[i].x;
					}

					if (wingVertices[i].z > tipSlicePoint && wingVertices[i].z < tipPositiveClosestAheadSlicePointZ)
					{
						tipPositiveClosestAheadSlicePointZ = wingVertices[i].z;
						tipPositiveClosestAheadSlicePointX = wingVertices[i].x;
					}
				}

				//Tip negative
				if (vertexBelongTip && vertexXNegative)
				{
					if (wingVertices[i].z <= tipSlicePoint && wingVertices[i].z > tipNegativeClosestBehindSlicePointZ)
					{
						tipNegativeClosestBehindSlicePointZ = wingVertices[i].z;
						tipNegativeClosestBehindSlicePointX = wingVertices[i].x;
					}

					if (wingVertices[i].z > tipSlicePoint && wingVertices[i].z < tipNegativeClosestAheadSlicePointZ)
					{
						tipNegativeClosestAheadSlicePointZ = wingVertices[i].z;
						tipNegativeClosestAheadSlicePointX = wingVertices[i].x;
					}
				}
			}

			float rootInterpolantPositiveValue = Mathf.InverseLerp(rootPositiveClosestBehindSlicePointZ, rootPositiveClosestAheadSlicePointZ, rootSlicePoint);
			rootPositiveSlicePointX = Mathf.Lerp(rootPositiveClosestBehindSlicePointX, rootPositiveClosestAheadSlicePointX, rootInterpolantPositiveValue);

			float rootInterpolantNegativeValue = Mathf.InverseLerp(rootNegativeClosestBehindSlicePointZ, rootNegativeClosestAheadSlicePointZ, rootSlicePoint);
			rootNegativeSlicePointX = Mathf.Lerp(rootNegativeClosestBehindSlicePointX, rootNegativeClosestAheadSlicePointX, rootInterpolantNegativeValue);

			float tipInterpolantPositiveValue = Mathf.InverseLerp(tipPositiveClosestBehindSlicePointZ, tipPositiveClosestAheadSlicePointZ, tipSlicePoint);
			tipPositiveSlicePointX = Mathf.Lerp(tipPositiveClosestBehindSlicePointX, tipPositiveClosestAheadSlicePointX, tipInterpolantPositiveValue);

			float tipInterpolantNegativeValue = Mathf.InverseLerp(tipNegativeClosestBehindSlicePointZ, tipNegativeClosestAheadSlicePointZ, tipSlicePoint);
			tipNegativeSlicePointX = Mathf.Lerp(tipNegativeClosestBehindSlicePointX, tipNegativeClosestAheadSlicePointX, tipInterpolantNegativeValue);

			return (rootPositiveSlicePointX, rootNegativeSlicePointX, tipPositiveSlicePointX, tipNegativeSlicePointX, rootSlicePoint, tipSlicePoint);
		}
	}
}
