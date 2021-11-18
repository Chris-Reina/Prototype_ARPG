using DoaT;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioCuePreview))]
public class AudioCuePreviewInspector : Editor
{
    private AudioCuePreview _target;

    private void OnEnable()
    {
        _target = (AudioCuePreview) target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        
        if(_target.cue == null) GUI.enabled = false; ////----////
        if (GUILayout.Button("Preview"))
        {
            if (_target.cue.clip != null)
            {
                _target.PreviewCue();
            }
        }
        GUI.enabled = true; ////----////

        if(!_target.IsPlaying) GUI.enabled = false; ////----////
        if (GUILayout.Button("StopPreview"))
        {
            _target.StopPreview();
        }
        GUI.enabled = true; ////----////
    }
}
