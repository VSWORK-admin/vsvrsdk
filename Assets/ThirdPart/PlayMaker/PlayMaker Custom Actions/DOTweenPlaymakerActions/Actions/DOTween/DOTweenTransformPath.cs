// Copyright (c) 2015 - 2019 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using DG.Tweening;
using Doozy.PlayMaker;
using Doozy.PlayMaker.Actions;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("DOTween")]
    [Tooltip("Tweens a Transform's position through the given path waypoints, using the chosen path algorithm. ")]
    [HelpUrl("http://dotween.demigiant.com/documentation.php")]
    public class DOTweenTransformPath : FsmStateAction
    {
        public enum LookAt { nothing, position, target, ahead }

        [RequiredField]
        [CheckForComponent(typeof(Transform))]
        public FsmOwnerDefault gameObject;

        [RequiredField]
        [Tooltip("The waypoints to go through")]
        public FsmGameObject[] path;

        [Tooltip("The type of path: Linear (straight path) or CatmullRom (curved CatmullRom path)")]
        public PathType pathType;

        [Tooltip("The path mode: 3D, side-scroller 2D, top-down 2D")]
        public PathMode pathMode;

        [UIHint(UIHint.FsmInt)]
        [Tooltip("The resolution of the path (useless in case of Linear paths): higher resolutions make for more detailed curved paths but are more expensive. Defaults to 10, but a value of 5 is usually enough if you don't have dramatic long curves between waypoints")]
        public FsmInt resolution;

        [UIHint(UIHint.FsmColor)]
        [Tooltip("The color of the path (shown when gizmos are active in the Play panel and the tween is running)")]
        public FsmColor gizmoColor;

        [RequiredField]
        [UIHint(UIHint.FsmFloat)]
        [Tooltip("The duration of the tween")]
        public FsmFloat duration;

        [UIHint(UIHint.FsmBool)]
        [Tooltip("If isSpeedBased is TRUE sets the tween as speed based (the duration will represent the number of units/degrees the tween moves x second). NOTE: if you want your speed to be constant, also set the ease to Ease.Linear.")]
        public FsmBool setSpeedBased;

        [UIHint(UIHint.FsmFloat)]
        [Tooltip("Set a delayed startup for the tween")]
        public FsmFloat startDelay;

        [ActionSection("Set Path Options")]

        [UIHint(UIHint.FsmBool)]
        [Tooltip("If TRUE the path will be automatically closed")]
        public FsmBool closePath;

        [Tooltip("The eventual movement axis to lock.")]
        public AxisConstraint lockPosition;

        [Tooltip("The eventual rotation axis to lock.")]
        public AxisConstraint lockRotation;

        [ActionSection("Set Look At Options")]

        [Tooltip("Select the look at target.")]
        public LookAt lookAt;

        [UIHint(UIHint.FsmVector3)]
        [Tooltip("The position to look at. Orients the target towards the given position")]
        public FsmVector3 lookAtPosition;

        [UIHint(UIHint.FsmGameObject)]
        [Tooltip("The target to look at. Orients the target towards the given transform of the GameObject")]
        public FsmGameObject lookAtTarget;

        [UIHint(UIHint.FsmFloat)]
        [Tooltip("The lookAhead percentage to use when orienting to the path (0 to 1). Orients the target to the path with the given lookAhead")]
        [HasFloatSlider(0, 1)]
        public FsmFloat lookAhead;

        [ActionSection("Set Custom Direction to consider as 'forward'")]

        [UIHint(UIHint.FsmVector3)]
        [Tooltip("The eventual direction to consider as 'forward'. Default: the regular forward side of the transform.")]
        public FsmVector3 forwardDirection;

        [ActionSection("Set Custom Up to consider which direction is 'up'")]

        [UIHint(UIHint.FsmVector3)]
        [Tooltip("The vector that defines in which direction up is. Default: Vector3.up")]
        public FsmVector3 up;

        [ActionSection("Events")]
        [UIHint(UIHint.FsmEvent)]
        public FsmEvent startEvent;
        [UIHint(UIHint.FsmEvent)]
        public FsmEvent finishEvent;
        [UIHint(UIHint.FsmBool)]
        [Tooltip("If TRUE this action will finish immediately, if FALSE it will finish when the tween is complete.")]
        public FsmBool finishImmediately;

        [ActionSection("Tween ID")]

        [UIHint(UIHint.Description)]
        public string tweenIdDescription = "Set an ID for the tween, which can then be used as a filter with DOTween's Control Methods";

        [Tooltip("Select the source for the tween ID")]
        public Doozy.PlayMaker.Actions.TweenId tweenIdType;

        [UIHint(UIHint.FsmString)]
        [Tooltip("Use a String as the tween ID")]
        public FsmString stringAsId;

        [UIHint(UIHint.Tag)]
        [Tooltip("Use a Tag as the tween ID")]
        public FsmString tagAsId;

        [ActionSection("Ease Settings")]

        public Doozy.PlayMaker.Actions.SelectedEase selectedEase;
        [Tooltip("Sets the ease of the tween. If applied to a Sequence instead of a Tweener, the ease will be applied to the whole Sequence as if it was a single animated timeline.Sequences always have Ease.Linear by default, independently of the global default ease settings.")]
        public Ease easeType;
        public FsmAnimationCurve animationCurve;

        [ActionSection("Loop Settings")]

        [UIHint(UIHint.Description)]
        public string loopsDescriptionArea = "Setting loops to -1 will make the tween loop infinitely.";
        [UIHint(UIHint.FsmInt)]
        [Tooltip("Setting loops to -1 will make the tween loop infinitely.")]
        public FsmInt loops;

        [Tooltip("Sets the looping options (Restart, Yoyo, Incremental) for the tween. LoopType.Restart: When a loop ends it will restart from the beginning. LoopType.Yoyo: When a loop ends it will play backwards until it completes another loop, then forward again, then backwards again, and so on and on and on. LoopType.Incremental: Each time a loop ends the difference between its endValue and its startValue will be added to the endValue, thus creating tweens that increase their values with each loop cycle. Has no effect if the tween has already started.Also, infinite loops will not be applied if the tween is inside a Sequence.")]
        public DG.Tweening.LoopType loopType = DG.Tweening.LoopType.Restart;

        [ActionSection("Special Settings")]

        [UIHint(UIHint.FsmBool)]
        [Tooltip("If autoKillOnCompletion is set to TRUE the tween will be killed as soon as it completes, otherwise it will stay in memory and you'll be able to reuse it.")]
        public FsmBool autoKillOnCompletion;

        [UIHint(UIHint.FsmBool)]
        [Tooltip("Sets the recycling behaviour for the tween. If you don't set it then the default value (set either via DOTween.Init or DOTween.defaultRecyclable) will be used.")]
        public FsmBool recyclable;

        [Tooltip("Sets the type of update (Normal, Late or Fixed) for the tween and eventually tells it to ignore Unity's timeScale. UpdateType.Normal: Updates every frame during Update calls. UpdateType.Late: Updates every frame during LateUpdate calls. UpdateType.Fixed: Updates using FixedUpdate calls. ")]
        public UpdateType updateType = UpdateType.Normal;

        [UIHint(UIHint.FsmBool)]
        [Tooltip(" If TRUE the tween will ignore Unity's Time.timeScale. NOTE: independentUpdate works also with UpdateType.Fixed but is not recommended in that case (because at timeScale 0 FixedUpdate won't run).")]
        public FsmBool isIndependentUpdate;

        [ActionSection("Debug Options")]
        [UIHint(UIHint.FsmBool)]
        public FsmBool debugThis;

        private Tweener tween;

        public override void Reset()
        {
            base.Reset();
            gameObject = null;
            duration = new FsmFloat { UseVariable = false };
            setSpeedBased = new FsmBool { UseVariable = false, Value = false };
            pathType = PathType.Linear;
            pathMode = PathMode.Full3D;
            resolution = new FsmInt { UseVariable = false, Value = 10 };
            gizmoColor = new FsmColor { UseVariable = false };
            closePath = new FsmBool { UseVariable = false, Value = false };
            lockPosition = AxisConstraint.None;
            lockRotation = AxisConstraint.None;
            lookAt = LookAt.nothing;
            lookAtPosition = new FsmVector3 { UseVariable = false, Value = Vector3.zero };
            lookAtTarget = new FsmGameObject { UseVariable = false, Value = null };
            lookAhead = new FsmFloat { UseVariable = false, Value = 0 };
            forwardDirection = new FsmVector3 { UseVariable = false, Value = Vector3.forward };
            up = new FsmVector3 { UseVariable = false, Value = Vector3.up };
            startEvent = null;
            finishEvent = null;
            finishImmediately = new FsmBool { UseVariable = false, Value = false };
            startDelay = new FsmFloat { Value = 0 };
            easeType = Ease.Linear;
            loops = new FsmInt { Value = 0 };
            loopType = DG.Tweening.LoopType.Restart;
            autoKillOnCompletion = new FsmBool { Value = true };
            recyclable = new FsmBool { Value = false };
            updateType = UpdateType.Normal;
            isIndependentUpdate = new FsmBool { Value = false };
            debugThis = new FsmBool { Value = false };
        }

        public override void OnEnter()
        {
            GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);

            if (go == null)
            {
                if (debugThis.Value) State.Debug("ERROR - DOTween Transform Local Path - The gameObject is null");
                return;
            }

            var trn = go.GetComponent<Transform>();

            if (trn == null)
            {
                if (debugThis.Value) State.Debug("ERROR - DOTween Transform Local Path - The GameObject [" + go.name + "] does not have a Transform Component");
                return;
            }

            var waypoints = new Vector3[path.Length];

            for (int i = 0; i < waypoints.Length; i++)
                waypoints[i] = path[i].Value.transform.position;


            switch (lookAt)
            {
                case LookAt.nothing:
                    tween = trn.DOPath(waypoints, duration.Value, pathType, pathMode, resolution.Value, gizmoColor.Value)
                    .SetOptions(closePath.Value, lockPosition, lockRotation);
                    break;

                case LookAt.position:
                    tween = trn.DOPath(waypoints, duration.Value, pathType, pathMode, resolution.Value, gizmoColor.Value)
                   .SetOptions(closePath.Value, lockPosition, lockRotation)
                   .SetLookAt(lookAtPosition.Value, forwardDirection.Value, up.Value);
                    break;

                case LookAt.target:
                    tween = trn.DOPath(waypoints, duration.Value, pathType, pathMode, resolution.Value, gizmoColor.Value)
                    .SetOptions(closePath.Value, lockPosition, lockRotation)
                    .SetLookAt(lookAtTarget.Value.transform, forwardDirection.Value, up.Value);
                    break;

                case LookAt.ahead:
                    tween = trn.DOPath(waypoints, duration.Value, pathType, pathMode, resolution.Value, gizmoColor.Value)
                    .SetOptions(closePath.Value, lockPosition, lockRotation)
                    .SetLookAt(lookAhead.Value, forwardDirection.Value, up.Value);
                    break;
            }
            if (setSpeedBased.Value) tween.SetSpeedBased();
            tween.SetTweenId(tweenIdType, stringAsId, tagAsId, Fsm.GetOwnerDefaultTarget(gameObject));
            tween.SetDelay(startDelay.Value);
            tween.SetSelectedEase(selectedEase, easeType, animationCurve);
            tween.SetLoops(loops.Value, loopType);
            tween.SetAutoKill(autoKillOnCompletion.Value);
            tween.SetRecyclable(recyclable.Value);
            tween.SetUpdate(updateType, isIndependentUpdate.Value);
            if (startEvent != null) tween.OnStart(() => { Fsm.Event(startEvent); });
            if (finishImmediately.Value == false) // This allows Action Sequences of this action.
            {
                if (finishEvent != null)
                    tween.OnComplete(() => { Fsm.Event(finishEvent); });
                else
                    tween.OnComplete(Finish);
            }
            tween.Play();
            if (debugThis.Value) State.Debug("DOTween Transform Path");
            if (finishImmediately.Value) Finish();
        }
    }
}
