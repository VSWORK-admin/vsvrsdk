using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FfmpegUnity.Sample
{
    public class SwingRotate : MonoBehaviour
    {
        const float ANGLE_SPEED = 15f;
        const float ANGLE_MAX = 30f;

        float angle_ = 0f;
        int angleTo_ = 1;

        void Update()
        {
            float angleMove = angleTo_ * ANGLE_SPEED * Time.deltaTime;
            float angleTemp = angle_ + angleMove;
            if (angleTemp > ANGLE_MAX || angleTemp < -ANGLE_MAX)
            {
                angleTemp = ANGLE_MAX * angleTo_ - (angleTemp - ANGLE_MAX * angleTo_);
                angleMove = angleTemp - angle_;
                angleTo_ *= -1;
            }
            angle_ += angleMove;
            transform.rotation *= Quaternion.Euler(0f, angleMove, 0f);
        }
    }
}
