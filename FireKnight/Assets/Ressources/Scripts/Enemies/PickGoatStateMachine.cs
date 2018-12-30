using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dim.Structures;
using UnityEngine.AI;

namespace Dim.Enemies
{

    /////////////////////////////////////////////////
    /// StateMachine of a PickGoat, used by PickGoatController.
    /////////////////////////////////////////////////
    public class PickGoatStateMachine : StateMachine<PickGoatStateType, PickGoatState>
    {

        PickGoatController c;

        public PickGoatStateMachine(PickGoatController _controller) : base()
        {
            c = _controller;

            AddMine();
            AddWalk();
        }

        private void AddMine()
        {
            fsm.Add(PickGoatStateType.Mine,
                new PickGoatState(PickGoatStateType.Mine,
                MineUpdate,
                new PickGoatTransition[]
                {
                    new PickGoatTransition(PickGoatStateType.Walk, MineToWalk)
                }
                ));
        }

        private void AddWalk()
        {
            fsm.Add(PickGoatStateType.Walk,
                new PickGoatState(PickGoatStateType.Walk,
                WalkUpdate,
                new PickGoatTransition[]
                {
                    new PickGoatTransition(PickGoatStateType.Mine, WalkToMine)
                }
                ));
        }

        private void MineUpdate()
        {

        }

        private void WalkUpdate()
        {
            Transform t = GetStrongestDetectedEnergy();

            if (t != null)
            {
                bool isRight = t.position.x > c.transform.position.x;
                MoveSideWaysInDirection(isRight);
            }     
        }

        private bool MineToWalk()
        {
           if (GetStrongestDetectedEnergy() != null) 
            {
                return true;
            }
           
            return false;
        }

        private bool WalkToMine()
        {
            if (GetStrongestDetectedEnergy() == null)
            {
                return true;
            }
            return false;
        }
       

        private Transform GetStrongestDetectedEnergy()
        {
            Transform t = null;
            Transform tOut;

            if(EnergyHandler.GetStrongestEnergyInCone(c.DetectionOrigin1.position, c.DetectionOrigin1.forward, c.DetectionRange, c.DetectionRadius, out tOut))
            {
                t = tOut;
            }

            if(EnergyHandler.GetStrongestEnergyInCone(c.DetectionOrigin2.position, c.DetectionOrigin2.forward, c.DetectionRange, c.DetectionRadius, out tOut))
            {
                if (t == null)
                {
                    t = tOut;
                }
                else
                {
                    IEnergyObject i = t.GetComponent<IEnergyObject>();
                    IEnergyObject iOut = tOut.GetComponent<IEnergyObject>();

                    if (iOut.GetEnergyAmount() > i.GetEnergyAmount())
                    {
                        t = tOut;
                    }
                }
            }

            return t;
            
        }

        private void MoveSideWaysInDirection(bool right)
        {
            if (!c.groundChecker.IsOnGround())
            {
                return;
            }

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
    /// Represents all the States of the PickGoatStateMachine.
    /////////////////////////////////////////////////
    public enum PickGoatStateType
    {
        Mine,
        Walk
    }

    /////////////////////////////////////////////////
    /// State class of the PickGoatStateMachine.
    /////////////////////////////////////////////////
    public class PickGoatState : State<PickGoatStateType>
    {
        public PickGoatState(PickGoatStateType _type, System.Action _Update, PickGoatTransition[] _Transitions) : base(_type, _Update, _Transitions) { }
    }

    /////////////////////////////////////////////////
    /// Transition class of the PickGoatStateMachine.
    /////////////////////////////////////////////////
    public class PickGoatTransition : Transition<PickGoatStateType>
       
    {
        public PickGoatTransition(PickGoatStateType _type, System.Func<bool> _transitionShouldOccur) : base(_type, _transitionShouldOccur) { }
    }

}