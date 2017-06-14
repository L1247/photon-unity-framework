using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ExitGames.Client.Photon;
using UnityEngine;

public class PhotonBehaviour : IPhotonEvent
{
        Dictionary<int , ccCallbackV1> callBackList = new Dictionary<int , ccCallbackV1>();
        RaiseEventOptions AllNoCache = new RaiseEventOptions () { CachingOption = EventCaching.DoNotCache , Receivers = ReceiverGroup.All , };
        RaiseEventOptions AllCache = new RaiseEventOptions () { CachingOption = EventCaching.AddToRoomCacheGlobal , Receivers = ReceiverGroup.All , };
        RaiseEventOptions OtherNoCache = new RaiseEventOptions () { CachingOption = EventCaching.DoNotCache , Receivers = ReceiverGroup.Others , };
        RaiseEventOptions OtherCache = new RaiseEventOptions () { CachingOption = EventCaching.AddToRoomCacheGlobal , Receivers = ReceiverGroup.Others , };
        RaiseEventOptions MasterClientNoCache = new RaiseEventOptions () { CachingOption = EventCaching.DoNotCache , Receivers = ReceiverGroup.MasterClient , };
        RaiseEventOptions MasterClientCache = new RaiseEventOptions () { CachingOption = EventCaching.AddToRoomCacheGlobal , Receivers = ReceiverGroup.MasterClient , };

        int index = 1;
        byte eventCode = 123;
        //MonoBehavior CallBack
        public virtual void Awake ()
        {
        }
        public virtual void AwakeIsMine ()
        {
        }


        public void AwakeIsNotMine ()
        {
        }

        public virtual void AwakeIsMasterClient ()
        {
        }

        public void AwakeIsNotMasterClient ()
        {
        }

        public virtual void Start ()
        {
        }
        public virtual void StartIsMine ()
        {
        }

        public void StartIsNotMine ()
        {
        }

        public virtual void StartIsMasterClient ()
        {
        }
        public void StartIsNotMasterClient ()
        {
        }

        public virtual void Update ()
        {
        }
        public virtual void UpdateIsMine ()
        {
        }

        public void UpdateIsNotMine ()
        {
        }

        public virtual void UpdateIsMasterClient ()
        {
        }
        public void UpdateIsNoMasterClient ()
        {
        }

        public virtual void OnTriggerEnter ( Collider other )
        {
        }
        public virtual void OnTriggerEnterIsMine ( Collider other )
        {
        }
        public void OnTriggerEnterIsNotMine ( Collider other )
        {
        }

        public virtual void OnTriggerEnterIsMasterClient ( Collider other )
        {
        }

        public void OnTriggerEnterIsNotMasterClient ( Collider other )
        {
        }

        public virtual void OnTriggerExit ( Collider other )
        {
        }
        public virtual void OnTriggerExitIsMine ( Collider other )
        {
        }
        public void OnTriggerExitIsNotMine ( Collider other )
        {
        }

        public virtual void OnTriggerExitIsMasterClient ( Collider other )
        {
        }

        public void OnTriggerExitIsNotMasterClient ( Collider other )
        {
        }

        //Photon CallBack
        public virtual void StreamReceive ( PhotonStream stream , PhotonMessageInfo info )
        {
        }
        public virtual void StreamSend ( PhotonStream stream , PhotonMessageInfo info )
        {
        }

        /// <summary>
        /// Called by PhotonNetwork.OnEventCall registration
        /// </summary>
        /// <param name="eventCode">Event code.</param>
        /// <param name="content">Content.</param>
        /// <param name="senderId">Sender identifier.</param>
        private void OnEvent ( byte Reviced_EventCode , object content , int senderId )
        {
                PhotonPlayer sender = PhotonPlayer.Find( senderId );
                if ( Reviced_EventCode == eventCode )
                {
                        Debug.Log( sender );
                        Hashtable hash = ( Hashtable ) content;
                        int callBackIndex = ( int ) hash[ "callBackIndex" ];
                        object obj = hash[ "eventContent" ];
                        ccCallbackV1 Callback = callBackList[ callBackIndex ];
                        Callback( obj );
                }
        }

        public void RPC_All ( ccCallbackV1 Callback , object obj = null )
        {
                RPC( AllNoCache , Callback , obj );
        }

        public void RPC_AllCache ( ccCallbackV1 Callback , object obj = null )
        {
                RPC( AllCache , Callback , obj );
        }

        public void RPC_Others ( ccCallbackV1 Callback , object obj = null )
        {
                RPC( OtherNoCache , Callback , obj );
        }

        public void RPC_OthersCache ( ccCallbackV1 Callback , object obj = null )
        {
                RPC( OtherCache , Callback , obj );
        }

        public void RPC_MasterClient ( ccCallbackV1 Callback , object obj = null )
        {
                RPC( MasterClientNoCache , Callback , obj );
        }

        public void RPC_MasterClientCache ( ccCallbackV1 Callback , object obj = null )
        {
                RPC( MasterClientCache , Callback , obj );
        }

        private void RPC ( RaiseEventOptions option , ccCallbackV1 Callback , object obj = null )
        {

                int callBackIndex = callBackList.FirstOrDefault( x => x.Value == Callback ).Key;

                Hashtable hash = new Hashtable();
                hash.Add( "callBackIndex" , callBackIndex );
                hash.Add( "eventContent" , obj );

                PhotonNetwork.RaiseEvent( eventCode , hash , true , option );
        }

        //Init
        public void Init ()
        {
                PhotonNetwork.OnEventCall = OnEvent;
               
                Type type = GetType();
                List<MethodInfo> entries = type.GetMethods( BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly ).ToList();

                foreach ( var callback in entries )
                {
                        Delegate test =
                      Delegate.CreateDelegate( typeof( ccCallbackV1 ) , this , callback , false );

                        callBackList.Add( index , ( ccCallbackV1 ) test );
                        index++;
                }
        }

        //Debug
        protected void Log ( object message )
        {
                Debug.Log( message );
        }

}
