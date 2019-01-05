using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dim.Structures;
using UnityEngine.AI;

namespace Dim.Enemies
{
    /////////////////////////////////////////////////
    /// StateMachine of the CoreDrainer, used by the CoreDrainerController.
    /////////////////////////////////////////////////
    public class CoreDrainerStateMachine : StateMachine<CoreDrainerStateType, CoreDrainerState>
    {

        CoreDrainerController controller;
        float cooldownTimeStamp;
        bool utilityFlag=false;

        public CoreDrainerStateMachine(CoreDrainerController _controller) : base()
        {
            controller = _controller;
            cooldownTimeStamp = 0;

            AddRecharging();
            AddWalkToSound();
            AddWalkToRecharging();
            AddPrepareToDrain();
            AddDrain();
            AddWalkOutOfRecharging();
            AddIdle();

        }

        private void AddRecharging()
        {
            fsm.Add(CoreDrainerStateType.Recharging, new CoreDrainerState(CoreDrainerStateType.Recharging,
                RechargingUpdate,
                new CoreDrainerTransition[]
                {
                    new CoreDrainerTransition(CoreDrainerStateType.WalkOutOfRecharging, RechargingToWalkOut)
                }
                ));
        }

        private void RechargingUpdate()
        {
            if (firstFrameOfStateChange)
            {
                if (controller.Energy > 0)
                {
                    controller.Energy = controller.Base.TryDepositQuantityOfEnergy(controller.Energy);
                }
                
                cooldownTimeStamp = Time.time;
                controller.transform.position = controller.Base.transform.position;
                controller.rb.velocity = Vector3.zero;
            }
        }


        private bool RechargingToWalkOut()
        {
            if(controller.LastSoundHeard != null && Time.time-cooldownTimeStamp >controller.RechargeMinTime)
            {
                return true;
            }

            return false;
        }

        private void AddWalkToSound()
        {
            fsm.Add(CoreDrainerStateType.WalkToSound, new CoreDrainerState(CoreDrainerStateType.WalkToSound,
                            WalkToSoundUpdate,
                            new CoreDrainerTransition[]
                            {
                    new CoreDrainerTransition(CoreDrainerStateType.PrepareToDrain, WalkToSoundToPrepareToDrain),
                    new CoreDrainerTransition(CoreDrainerStateType.Idle,WalkToSoundToIdle)
                            }
                            ));
        }

        private void WalkToSoundUpdate()
        {
            if (firstFrameOfStateChange)
            {
                controller.PlayWalkingSound();
            }

            if (controller.LastSoundHeard != null)
            {
                MoveSideWaysInDirection(controller.LastSoundHeard.Origin.x > controller.transform.position.x);
            }
            
        }

        private void MoveSideWaysInDirection(bool right)
        {
            float dir = right ? 1 : -1;
            controller.rb.AddForce(new Vector3(controller.MovementForce * dir, 0, 0));

            if (controller.rb.velocity.x > controller.MovementCap)
            {
                controller.rb.velocity = new Vector3(controller.MovementCap, controller.rb.velocity.y, 0);
            }
            else if (-controller.rb.velocity.x > controller.MovementCap)
            {
                controller.rb.velocity = new Vector3(-controller.MovementCap, controller.rb.velocity.y, 0);
            }
        }

        private void MovePerPendicularInDirection(bool outWards)
        {
            float dir = outWards ? 1 : -1;
            controller.rb.AddForce(new Vector3(0, 0, controller.MovementForce * dir));

            if (controller.rb.velocity.z > controller.MovementCap)
            {
                controller.rb.velocity = new Vector3(0, controller.rb.velocity.y, controller.MovementCap);
            }
            else if (-controller.rb.velocity.z > controller.MovementCap)
            {
                controller.rb.velocity = new Vector3(0, controller.rb.velocity.y, -controller.MovementCap);
            }
        }


        private bool WalkToSoundToPrepareToDrain()
        {
            if (controller.LastSoundHeard == null)
            {
                return false;
            }

            if(Mathf.Abs(controller.transform.position.x - controller.LastSoundHeard.Origin.x) < 1f)
            {
                return true;
            }

            return false;
        }


        private bool WalkToSoundToIdle()
        {
            if(controller.LastSoundHeard == null)
            {
                return true;
            }

            return false;
        }

        private void AddWalkToRecharging()
        {
            fsm.Add(CoreDrainerStateType.WalkToRecharging, new CoreDrainerState(CoreDrainerStateType.WalkToRecharging,
                           WalkToRechargeUpdate,
                           new CoreDrainerTransition[]
                           {
                    new CoreDrainerTransition(CoreDrainerStateType.Recharging, WalkToRechargeToRecharging)
                           }
                           ));
        }

        private void WalkToRechargeUpdate()
        {
            if (firstFrameOfStateChange)
            {
                utilityFlag = false;
                controller.PlayWalkingSound();
                
            }

            if (!utilityFlag)
            {
                MoveSideWaysInDirection(controller.Base.transform.position.x > controller.transform.position.x);

                if(Mathf.Abs(controller.Base.transform.position.x - controller.transform.position.x) < 0.1)
                {
                    utilityFlag = true;
                    controller.transform.position = new Vector3(controller.Base.transform.position.x, controller.transform.position.y, 0);
                    controller.rb.velocity = new Vector3(0, controller.rb.velocity.y, 0);
                }
            }
            else
            {
                MovePerPendicularInDirection(true);
            }
           
        }

        private bool WalkToRechargeToRecharging()
        {
            if(Vector3.Distance(controller.transform.position, controller.Base.transform.position) < 0.4)
            {
                return true;
            }
            return false;
        }

        private void AddPrepareToDrain()
        {
            fsm.Add(CoreDrainerStateType.PrepareToDrain, new CoreDrainerState(CoreDrainerStateType.PrepareToDrain,
                           PrepareToDrainUpdate,
                           new CoreDrainerTransition[]
                           {
                    new CoreDrainerTransition(CoreDrainerStateType.Drain, PrepareToDrainToDrain)
                           }
                           ));
        }

        private void PrepareToDrainUpdate()
        {
            if (firstFrameOfStateChange)
            {
                cooldownTimeStamp = Time.time;
            }


        }

        private bool PrepareToDrainToDrain()
        {
            if((Time.time-cooldownTimeStamp) > controller.DrainPreparationTimeInSec)
            {
                return true;
            }
            return false;
        }

        private void AddDrain()
        {
            fsm.Add(CoreDrainerStateType.Drain, new CoreDrainerState(CoreDrainerStateType.Drain,
                           DrainUpdate,
                           new CoreDrainerTransition[]
                           {
                    new CoreDrainerTransition(CoreDrainerStateType.Idle, DrainToIdle),

                           }
                           ));
        }

        private void DrainUpdate()
        {
            if (firstFrameOfStateChange)
            {
                cooldownTimeStamp = Time.time;
                controller.AbsorptionArea.SetActive(true);
                controller.DrainFlow.Active = true;
                controller.PlayAbsorbtionSound();
            }
            
        }

        private bool DrainToIdle()
        {
            if(Time.time-cooldownTimeStamp > controller.DrainDurationInSec)
            {
                return true;
            }
            return false;
        }



        private void AddWalkOutOfRecharging()
        {
            fsm.Add(CoreDrainerStateType.WalkOutOfRecharging, new CoreDrainerState(CoreDrainerStateType.WalkOutOfRecharging,
                           WalkOutOfRechargingUpdate,
                           new CoreDrainerTransition[]
                           {
                    new CoreDrainerTransition(CoreDrainerStateType.WalkToSound, WalkOutOfRechargingToWalkToSound),
                           }
                           ));
        }

        private void WalkOutOfRechargingUpdate()
        {
            if (firstFrameOfStateChange)
            {
                controller.PlayWalkingSound();
                controller.Energy = controller.Base.DrainAllEnergy();
            }

            MovePerPendicularInDirection(false);
            if(controller.transform.position.z < 0)
            {
                controller.rb.velocity = new Vector3(0, controller.rb.velocity.y, 0);
                controller.transform.position = new Vector3(controller.transform.position.x, controller.transform.position.y, 0);
            }
        }

        private bool WalkOutOfRechargingToWalkToSound()
        {
            if(controller.transform.position.z == 0)
            {
                return true;
            }
            return false;
        }

        private void AddIdle()
        {
            fsm.Add(CoreDrainerStateType.Idle, new CoreDrainerState(CoreDrainerStateType.Idle,
                           IdleUpdate,
                           new CoreDrainerTransition[]
                           {
                    new CoreDrainerTransition(CoreDrainerStateType.WalkToSound, IdleToWalkToSound),
                    new CoreDrainerTransition(CoreDrainerStateType.WalkToRecharging, IdleToWalkToRecharge)
                           }
                           ));
        }

        private void IdleUpdate()
        {
            if (firstFrameOfStateChange)
            {
                cooldownTimeStamp = Time.time;
                controller.AbsorptionArea.SetActive(false);
                controller.DrainFlow.Active = false;
                controller.StopSound();
            }


        }

        private bool IdleToWalkToSound()
        {
            if(controller.LastSoundHeard!= null)
            {
                return true;
            }

            return false;
        }

        private bool IdleToWalkToRecharge()
        {
            return Time.time - cooldownTimeStamp > controller.IdleToWalkBackTime;
        }

    }

    /////////////////////////////////////////////////
    /// Represents all the states of the CoreDrainerStateMachine.
    /////////////////////////////////////////////////
    public enum CoreDrainerStateType
    {
        Recharging,
        WalkToSound,
        WalkToRecharging,
        PrepareToDrain,
        Drain,
        WalkOutOfRecharging,
        Idle
    }

    /////////////////////////////////////////////////
    /// State class of the CoreDrainerStateMachine.
    /////////////////////////////////////////////////
    public class CoreDrainerState : State<CoreDrainerStateType>
    {
        public CoreDrainerState(CoreDrainerStateType _type, System.Action _Update, CoreDrainerTransition[] _Transitions) : base(_type, _Update, _Transitions) { }
    }

    /////////////////////////////////////////////////
    /// Transition class of the COreDrainerStateMachine.
    /////////////////////////////////////////////////
    public class CoreDrainerTransition : Transition<CoreDrainerStateType>

    {
        public CoreDrainerTransition(CoreDrainerStateType _type, System.Func<bool> _transitionShouldOccur) : base(_type, _transitionShouldOccur) { }
    }

}