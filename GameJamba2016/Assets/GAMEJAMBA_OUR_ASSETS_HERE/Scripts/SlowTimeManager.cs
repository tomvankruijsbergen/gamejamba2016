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

        Container.instance.OnEnemyKilled += this.KillSlowDown;
    }

    private void DragStart(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition) {
        SlowDown();
    }

    private void DragRelease(Vector2 dragPosition, Vector2 playerPosition, Vector2 cameraPosition) {
        SpeedUp();
    }

    private void SlowDown() {
        iTween.Stop(gameObject);
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", normalTimeScale,
            "to", slowTimeScale,
            "onupdate", "TweenedTimeValue",
            "time", slowDownDuration
        ));
    }

    private void SpeedUp() {
        iTween.Stop(gameObject);
        iTween.ValueTo(gameObject, iTween.Hash(
            "from", slowTimeScale,
            "to", normalTimeScale,
            "onupdate", "TweenedTimeValue",
            "time", speedUpDuration
        ));
    }

    private void OnDestroy() {
        Container.instance.RemoveSlowTimeManager(this);
    }

    public void TweenedTimeValue(float value) {
        Time.timeScale = value;
    }

    private void KillSlowDown(){
        StartCoroutine(KillSlowDownRoutine());
    }

    private IEnumerator KillSlowDownRoutine(){
        SlowDown();
        yield return new WaitForSeconds(0.2f);
        SpeedUp();
    }
}