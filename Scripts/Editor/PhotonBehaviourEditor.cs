using UnityEditor;
using UnityEngine;

[CustomEditor( typeof( PhotonBehaviourScript ) )]
public class PhotonBehaviourEditor : Editor 
{
        private SerializedObject m_Object;
        private SerializedProperty m_stringValue;

       
        public void OnEnable ()
        {
                m_Object = new SerializedObject( target );
                m_stringValue = m_Object.FindProperty( "pbEnum" );
                //Debug.Log( m_stringValue.stringValue );
        }

        public override void OnInspectorGUI ()
        {
                DrawDefaultInspector();

                PhotonBehaviourScript myScript = ( PhotonBehaviourScript ) target;
                if ( GUILayout.Button( "Build Object" ) )
                {
                        myScript.BuildType();
                }
                if ( GUI.changed )
                {
                        Debug.Log( "Changed" );
                }
        }
}