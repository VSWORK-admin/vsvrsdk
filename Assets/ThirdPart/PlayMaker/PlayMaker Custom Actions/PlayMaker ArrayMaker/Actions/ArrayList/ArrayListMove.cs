// (c) Copyright HutongGames, LLC 2010-2018. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("ArrayMaker/ArrayList")]
    [Tooltip("Move an item from a specified index to another in a PlayMaker ArrayList Proxy component")]
    public class ArrayListMove : ArrayListActions
    {
        [ActionSection("Set up")]

        [RequiredField]
        [Tooltip("The gameObject with the PlayMaker ArrayList Proxy component")]
        [CheckForComponent(typeof(PlayMakerArrayListProxy))]
        public FsmOwnerDefault gameObject;

        [Tooltip("Author defined Reference of the PlayMaker ArrayList Proxy component ( necessary if several component coexists on the same GameObject")]
        public FsmString reference;

        [UIHint(UIHint.FsmInt)]
        [Tooltip("The index to be moved")]
        public FsmInt indexMoveFrom;

        [UIHint(UIHint.FsmInt)]
        [Tooltip("The index to move to. IF set to -1 it will move to the last index")]
        public FsmInt indexMoveTo;

        [ActionSection("Result")]

        [UIHint(UIHint.FsmEvent)]
        [Tooltip("The event to trigger if the removeAt throw errors")]
        public FsmEvent failureEvent;

        public override void Reset()
        {
            gameObject = null;
            failureEvent = null;
            reference = null;
            indexMoveFrom = null;
            indexMoveTo = -1;
        }

        public override void OnEnter()
        {
            if (SetUpArrayListProxyPointer(Fsm.GetOwnerDefaultTarget(gameObject), reference.Value))
                DoArrayListMove();

            Finish();
        }


        public void DoArrayListMove()
        {
            if (!isProxyValid())
                return;


            object indexValue = proxy.arrayList[indexMoveFrom.Value];
            if (indexMoveTo.Value == -1)
            {
                proxy.arrayList.RemoveAt(indexMoveFrom.Value);
                proxy.arrayList.Add(indexValue);
            }
            else
            {
                if (indexMoveFrom.Value < proxy._arrayList.Count && indexMoveTo.Value < proxy._arrayList.Count)

                    if (indexMoveFrom.Value < indexMoveTo.Value)
                    {
                        for (int i = indexMoveFrom.Value; i < indexMoveTo.Value; i++)
                        {
                            object element = null;
                            element = proxy.arrayList[i + 1];
                            proxy.arrayList[i] = element;
                        }
                        proxy.arrayList[indexMoveTo.Value] = indexValue;
                    }
                if (indexMoveFrom.Value > indexMoveTo.Value)
                {
                    for (int i = indexMoveFrom.Value; i > indexMoveTo.Value; i--)
                    {
                        object element = null;
                        element = proxy.arrayList[i - 1];
                        proxy.arrayList[i] = element;
                    }
                    proxy.arrayList[indexMoveTo.Value] = indexValue;
                }
            }
        }
    }
}