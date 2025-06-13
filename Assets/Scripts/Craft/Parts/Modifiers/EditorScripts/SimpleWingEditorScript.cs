#if UNITY_EDITOR
namespace Assets.Scripts.Craft.Parts.Modifiers.EditorScripts
{
	using ModApi.Craft.Parts.Editor;

	/// <summary>
	/// An editor only class used to associated part modifiers with game objects when defining parts.
	/// </summary>
	public sealed class SimpleWingEditorScript : PartModifierEditorScript<SimpleWingData>
    {
    }
}
#endif