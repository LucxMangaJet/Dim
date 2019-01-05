using Dim.Structures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dim.Enemies
{

    /////////////////////////////////////////////////
    /// StateMachine of a RhinoTrain, used by RhinoTrainController.
    /////////////////////////////////////////////////
    public class RhinoTrainStateMachine : StateMachine<RhinoTrainStateType, RhinoTrainState>
    {

        RhinoTrainController c;
        float timeStamp = 0;

        public RhinoTrainStateMachine(RhinoTrainController _controller) : base()
        {
            c = _controller;

            AddInBase();
            AddCharge();
            AddWait();
            AddWalkBack();

        }

        private void AddInBase()
        {
            fsm.Add(RhinoTrainStateType.InBase,
                new RhinoTrainState(RhinoTrainStateType.InBase,
                InBaseUpdate,
                new RhinoTrainTransition[]
                {
                    new RhinoTrainTransition(RhinoTrainStateType.Charge, InBaseToCharge)
                }
                ));

        }

        private void AddCharge()
        {
            fsm.Add(RhinoTrainStateType.Charge,
                new RhinoTrainState(RhinoTrainStateType.Charge,
                ChargeUpdate,
                new RhinoTrainTransition[]
                {
                    new RhinoTrainTransition(RhinoTrainStateType.Wait, ChargeToWait)
                }
                ));
        }

        private void AddWait()
        {
            fsm.Add(RhinoTrainStateType.Wait,
                new RhinoTrainState(RhinoTrainStateType.Wait,
                WaitUpdate,
                new RhinoTrainTransition[]
                {
                    new RhinoTrainTransition(RhinoTrainStateType.WalkBack, WaitToWalkBack)
                }
                ));
        }

        private void AddWalkBack()
        {
            fsm.Add(RhinoTrainStateType.WalkBack,
                new RhinoTrainState(RhinoTrainStateType.WalkBack,
                WalkBackUpdate,
                new RhinoTrainTransition[]
                {
                    new RhinoTrainTransition(RhinoTrainStateType.InBase, WalkBackToInBase)
                }
                ));
        }
       

        private void InBaseUpdate()
        {
            if (firstFrameOfStateChange)
                c.SetAnimatorWalkBack(false);
        }

        private bool InBaseToCharge()
        {
            return c.ShouldActivate();
        }

        private void ChargeUpdate()
        {
            if (firstFrameOfStateChange)
            {
                c.SetAnimatorWalk(true);
                c.source.Play();
            }
            MoveSideWaysInDirection(c.rhinoTrainBase.target.x > c.transform.position.x);
        }

        private bool ChargeToWait()
        {
            bool amRightOfTarget = c.transform.position.x - c.rhinoTrainBase.target.x > 0;
            if (c.MoveDirectionIsRight == amRightOfTarget)
            {
                return true;
            }

            if (!c.groundChecker.IsOnGround())
            {
                return true;
            }

            return false;
        }

        private void WaitUpdate()
        {
            if (firstFrameOfStateChange)
            {
                timeStamp = Time.time;
                c.rb.velocity = Vector3.zero;
                c.SetAnimatorWalk(false);
            }
        }

        private bool WaitToWalkBack()
        {
            if(Time.time-timeStamp > c.WaitTimeAfterPush)
            {
                return true;
            }
            return false;
        }
        
        private void WalkBackUpdate()
        {
            if (firstFrameOfStateChange)
            {
                c.SetAnimatorWalkBack(true);
            }
            MoveSideWaysInDirection(c.transform.position.x < c.rhinoTrainBase.transform.position.x);
        }

        private bool WalkBackToInBase()
        {
            bool amRightOfTarget = c.transform.position.x - c.rhinoTrainBase.transform.position.x > 0;
            if (c.MoveDirectionIsRight != amRightOfTarget)
            {
                c.rb.velocity = Vector3.zero;
                return true;
            }
            return false;
        }



        private void MoveSideWaysInDirection(bool right)
        {
            float dir = right ? 1 : -1;
            c.rb.AddForce(new Vector3(c.MovementForce * dir, 0, 0));

            if (c.rb.velocity.x > c.MovementCap)
            {
                c.rb.velocity = new Vector3(c.MovementCap, c.rb.velocity.y, 0);
            }
            else if (-c.rb.velocity.x > c.MovementCap)
            {
                c.rb.velocity = new Vector3(-c.MovementCap, c.rb.velocity.y, 0);
            }
        }




    }

    /////////////////////////////////////////////////
    /// Represents all the States of a RhinoTrainStateMachine.
    /////////////////////////////////////////////////
    public enum RhinoTrainStateType
    {
        InBase,
        Charge,
        Wait,
        WalkBack
    }

    /////////////////////////////////////////////////
    /// State class of the RhinoTrainStateMachine.
    /////////////////////////////////////////////////
    public class RhinoTrainState : State<RhinoTrainStateType>
    {
        public RhinoTrainState(RhinoTrainStateType _type, System.Action _Update, RhinoTrainTransition[] _Transitions) : base(_type, _Update, _Transitions) { }
    }

    /////////////////////////////////////////////////
    /// Transition class of the RhinoTrainStateMachine.
    /////////////////////////////////////////////////
    public class RhinoTrainTransition : Transition<RhinoTrainStateType>

    {
        public RhinoTrainTransition(RhinoTrainStateType _type, System.Func<bool> _transitionShouldOccur) : base(_type, _transitionShouldOccur) { }
    }

}
