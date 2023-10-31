using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class FSM
    {
        List<StateBase> stateList = new List<StateBase>();
        private StateBase activeState = null;
        private int currentStateId = 0;

        /// <summary>
        /// 設定FSM的State
        /// </summary>
        /// <param name="stateID">要設定的StateID</param>
        public void SetState(int stateID)
        {
            //  如果當前State存在，退出當前State
            if (activeState != null)
            {
                activeState.OnStateExit();
                activeState = null;
                currentStateId = -1;
            }

            if (stateList[stateID] != null)
            {
                Debug.Log("[FSM] Transit to " + stateID);
                currentStateId = stateID;
                activeState = stateList[stateID];
                activeState.OnStateEnter();
            }
            else
            {
                Debug.LogError("[FSM] Transit is failed by " + stateID + "state");
            }
        }

        /// <summary>
        /// 動態新增新的State
        /// </summary>
        public void AddState(StateBase state)
        {
            stateList.Add(state);
        }

        /// <summary>
        /// 取得當前狀態
        /// </summary>
        /// <returns>當前狀態的 StateBase 物件</returns>
        private StateBase GetActiveState()
        {
            return activeState;
        }

        /// <summary>
        /// 取得當前狀態ID
        /// </summary>
        /// <returns>當前狀態ID</returns>
        public int GetcurrentStateId()
        {
            return currentStateId;
        }
    }
}