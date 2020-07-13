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
    [Tooltip("Moves the target's localPosition to the given value, tweening only the X axis.")]
    [HelpUrl("http://dotween.demigiant.com/documentation.php")]
    public class DOTweenTransformLocalMoveX : FsmStateAction
    {
        [RequiredField]
        [CheckForComponent(typeof(Transform))]
        public FsmOwnerDefault gameObject;

        [ActionSection("")]
        [Tooltip("Select the target. Either a float or a GameObject's x position")]
        public Doozy.PlayMaker.Actions.Target target = Doozy.PlayMaker.Actions.Target.Value;
        [UIHint(UIHint.FsmFloat)]
        [Tooltip("The end value to reach")]
        public FsmFloat to;

        [UIHint(UIHint.FsmGameObject)]
        [Tooltip("The end value to reach")]
        public FsmGameObject toGameObject;

        [ActionSection("")]
        [UIHint(UIHint.FsmBool)]
        [Tooltip("If setRelative is TRUE sets the tween as relative (the endValue will be calculated as startValue + endValue instead of being used directly). In case of Sequences, sets all the nested tweens as relative. IMPORTANT: Has no effect on Reverse Options, since in that case you directly choose if the tween isRelative or not in the settings below")]
        public FsmBool setRelative;

        [UIHint(UIHint.FsmBool)]
        [Tooltip("If TRUE the tween will smoothly snap all values to integers.")]
        public FsmBool snapping;

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

        [ActionSection("Reverse Options")]
        [UIHint(UIHint.FsmBool)]
        [Tooltip("Play in reverse. Changes a TO tween into a FROM tween: sets the current target's startValue as the tween's endValue then immediately sends the target to the previously set endValue.")]
        public FsmBool playInReverse;

        [UIHint(UIHint.FsmBool)]
        [Tooltip("If TRUE the FROM value will be calculated as relative to the current one")]
        public FsmBool setReverseRelative;

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
            to = new FsmFloat { UseVariable = false };
            toGameObject = new FsmGameObject { UseVariable = false };
            duration = new FsmFloat { UseVariable = false };
            setSpeedBased = new FsmBool { UseVariable = false, Value = false };
            snapping = new FsmBool { UseVariable = false, Value = false };
            setRelative = new FsmBool { UseVariable = false, Value = false };
            playInReverse = new FsmBool { UseVariable = false, Value = false };
            setReverseRelative = new FsmBool { UseVariable = false, Value = false };
            startEvent = null;
            finishEvent = null;
            finishImmediately = new FsmBool { UseVariable = false, Value = false };
            stringAsId = new FsmString { UseVariable = false };
            tagAsId = new FsmString { UseVariable = false };
            startDelay = new FsmFloat { Value = 0 };
            selectedEase = Doozy.PlayMaker.Actions.SelectedEase.EaseType;
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
            switch (target)
            {
                case Doozy.PlayMaker.Actions.Target.Value:
                    tween = Fsm.GetOwnerDefaultTarget(gameObject).GetComponent<Transform>().DOLocalMoveX(to.Value, duration.Value, snapping.Value);
                    break;
                case Doozy.PlayMaker.Actions.Target.GameObject:
                    tween = Fsm.GetOwnerDefaultTarget(gameObject).GetComponent<Transform>().DOLocalMoveX(toGameObject.Value.transform.position.x, duration.Value, snapping.Value);
                    break;
            }
            if (setSpeedBased.Value) tween.SetSpeedBased();
            tween.SetRelative(setRelative.Value);
            tween.SetTweenId(tweenIdType, stringAsId, tagAsId, Fsm.GetOwnerDefaultTarget(gameObject));
            tween.SetDelay(startDelay.Value);
            tween.SetSelectedEase(selectedEase, easeType, animationCurve);
            tween.SetLoops(loops.Value, loopType);
            tween.SetAutoKill(autoKillOnCompletion.Value);
            tween.SetRecyclable(recyclable.Value);
            tween.SetUpdate(updateType, isIndependentUpdate.Value);
            if (playInReverse.Value) tween.From(setReverseRelative.Value);
            if (startEvent != null) tween.OnStart(() => { Fsm.Event(startEvent); });
            if (finishImmediately.Value == false) // This allows Action Sequences of this action.
            {
                if (finishEvent != null)
                    tween.OnComplete(() => { Fsm.Event(finishEvent); });
                else
                    tween.OnComplete(Finish);
            }
            tween.Play();
            if (debugThis.Value) State.Debug("DOTween Transform Local Move X");
            if (finishImmediately.Value) Finish();
        }
    }
}
