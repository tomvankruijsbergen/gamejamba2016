using UnityEngine;
using System.Collections;

public class SlowTimeManager : MonoBehaviour {

    [SerializeField] private float slowTimeScale = .1f;
    [SerializeField] private float slowDownDuration = .2f;
    [SerializeField] private float speedUpDuration = .05f;
    private float normalTimeScale;
    private GameObject speedObject;
    
    void Awake(){
        normalTimeScale = Time.timeScale;
        Container.instance.AssignSlowTimeManager(this);
        Container.instance.OnDragStart += this.DragStart;
        Container.instance.OnDragEnd += this.DragRelease;
    }

    private void DragStart(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition) {
        SlowDown();
    }

    private void DragRelease(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition) {
        SpeedUp();
    }

    private void SlowDown() {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", normalTimeScale,
            "to", slowTimeScale,
            "onupdate", "TweenedTimeValue",
            "time", slowDownDuration
        ));
    }

    private void SpeedUp() {
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", slowTimeScale,
            "to", normalTimeScale,
            "onupdate", "TweenedTimeValue",
            "time", speedUpDuration
        ));
    }

    public void TweenedTimeValue(float value) {
        Time.timeScale = value;
    }
}