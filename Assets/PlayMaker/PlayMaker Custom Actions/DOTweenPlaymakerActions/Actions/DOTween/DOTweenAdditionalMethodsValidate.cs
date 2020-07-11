// Copyright (c) 2015 - 2019 Doozy Entertainment / Marlink Trading SRL. All Rights Reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

using DG.Tweening;
using Doozy.PlayMaker.Actions;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("DOTween")]
    [Tooltip(" Checks all active tweens to find and remove eventually invalid ones (usually because their targets became NULL) and returns the total number of invalid tweens found and removed. Automatically called when loading a new scene if DG.Tweening.DOTween.useSafeMode is TRUE. BEWARE: this is a slightly expensive operation so use it with care")]
    [HelpUrl("http://dotween.demigiant.com/documentation.php")]
    public class DOTweenAdditionalMethodsValidate : FsmStateAction
    {
        [ActionSection("Debug Options")]
        public FsmBool debugThis;

        public override void Reset()
        {
            base.Reset();
            debugThis = new FsmBool {Value = false};
        }

        public override void OnEnter()
        {
            DOTween.Validate();
            if (debugThis.Value) State.Debug("DOTween Additional Methods Validate");
            Finish();
        }
    }
}