using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemySearch))]
public class EditorModif : Editor
{
    private void OnSceneGUI()
    {
        EnemySearch es = (EnemySearch)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(es.transform.position, Vector3.forward, Vector3.right, 360, es.viewRadius);
        Vector2 viewAngleA = es.DirFromAngle(-es.viewAngle / 2, false);
        Vector2 viewAngleB = es.DirFromAngle(es.viewAngle / 2, false);

        Handles.DrawLine(es.transform.position,(Vector2) es.transform.position + viewAngleA * es.viewRadius);
        Handles.DrawLine(es.transform.position, (Vector2)es.transform.position + viewAngleB * es.viewRadius);
        Handles.color = Color.red;
        /*foreach(Transform transform in es.visibleTarget)
        {
            Handles.DrawLine(es.transform.position, transform.position);
        }*/
    }

}
