using UnityEngine;
public static class srPhotonUtility
{
    public static void SetEnable ( MonoBehaviour _mono , bool _enable )
    {
        if ( _mono == null )
        {
            LogError( "Mono" );
            return;
        }

        _mono.enabled = _enable;
    }

    public static void SetActive ( GameObject _go , bool _active )
    {
        if ( _go == null )
        {
            LogError( "GameObject" );
            return;
        }

        _go.SetActive( _active );
    }


    static void LogError ( string keyWord )
    {
        Debug.LogErrorFormat( "{0} is Null" , keyWord );
    }
}
