using System.Collections.Generic;
using UnityEngine;
using Dim.Structures;

namespace Dim.Player
{

    /////////////////////////////////////////////////
    /// Used by PlayerStateMachines when changing Node.
    /////////////////////////////////////////////////
    public delegate void PlayerStateTransitionDelegate(StateType start, StateType end);


    /////////////////////////////////////////////////
    /// The Players StateMachine, tightly connected to the PlayerController, is responsible for the interactions/behavior.
    /////////////////////////////////////////////////
    public class PlayerStateMachine : StateMachine<StateType,PlayerState>
    {
   
        PlayerController pc;
        bool oldDirIsLeft = false;
        float timeStamp=0;
        bool used=false;

        public PlayerStateMachine(PlayerController _pc) :base()
        {
            pc = _pc;

            AddIdle();
            AddEmission();
            AddMoving();
            AddSprint();
            AddJump();
            AddCrouchIdle();
            AddCrouchMoving();
            AddAbsorption();
        }


        // setup methods ----------------------------------

        private void AddIdle()
        {
            fsm.Add(StateType.Idle,
                    new PlayerState
                    (
                        StateType.Idle,
                        IdleUpdate,
                        new PlayerStateTransition[]
                        {
                            new PlayerStateTransition(StateType.Emission,  IdleToEmission),
                            new PlayerStateTransition(StateType.Absorption, IdleToAbsorption),
                            new PlayerStateTransition(StateType.Moving, IdleToMoving),
                            new PlayerStateTransition(StateType.Jump, IdleToJump),
                            new PlayerStateTransition(StateType.Crouch, IdleToCrouch)
                        }
                    ));
        }

        private void AddEmission()
        {
            fsm.Add(StateType.Emission,
                    new PlayerState
                    (
                        StateType.Emission,
                        EmissionUpdate,
                        new PlayerStateTransition[]
                        {
                            new PlayerStateTransition(StateType.Idle, EmissionToIdle)
                        }
                    ));
        }

        private void AddAbsorption()
        {
            fsm.Add(StateType.Absorption,
                    new PlayerState
                    (
                        StateType.Absorption,
                        AbsorptionUpdate,
                        new PlayerStateTransition[]
                        {
                            new PlayerStateTransition(StateType.Idle, AbsorptionToIdle)
                        }
                    ));
        }

        private void AddMoving()
        {
            fsm.Add(StateType.Moving,
                    new PlayerState
                    (
                        StateType.Moving,
                        MovingUpdate,
                        new PlayerStateTransition[]
                        {
                            new PlayerStateTransition(StateType.Idle, MovingToIdle),
                            new PlayerStateTransition(StateType.Jump, MovingToJump),
                            new PlayerStateTransition(StateType.Sprint, MovingToSprint),
                            new PlayerStateTransition(StateType.CrouchMoving, MovingToCrouchMoving)
                        }
                    ));
        }

        private void AddSprint()
        {
            fsm.Add(StateType.Sprint,
                    new PlayerState
                    (
                        StateType.Sprint,
                        SprintUpdate,
                        new PlayerStateTransition[]
                        {
                            new PlayerStateTransition(StateType.Jump, SprintToJump),
                            new PlayerStateTransition(StateType.Moving, SprintToMoving)
                        }
                    ));
        }

        private void AddJump()
        {
            fsm.Add(StateType.Jump,
                    new PlayerState
                    (
                        StateType.Jump,
                        JumpUpdate,
                        new PlayerStateTransition[]
                        {
                            new PlayerStateTransition(StateType.Idle, JumpToIdle),
                        }
                    ));
        }

        private void AddCrouchIdle()
        {
            fsm.Add(StateType.Crouch,
                    new PlayerState
                    (
                        StateType.Crouch,
                        CrouchUpdate,
                        new PlayerStateTransition[]
                        {
                            new PlayerStateTransition(StateType.Idle, CrouchToIdle),
                            new PlayerStateTransition(StateType.CrouchMoving, CrouchToCrouchMoving),
                            new PlayerStateTransition(StateType.Emission, CrouchToEmission)
                        }
                    ));
        }

        private void AddCrouchMoving()
        {
            fsm.Add(StateType.CrouchMoving,
                    new PlayerState
                    (
                        StateType.CrouchMoving,
                        CrouchMovingUpdate,
                        new PlayerStateTransition[]
                        {
                            new PlayerStateTransition(StateType.Crouch, CrouchMovingToCrouch),
                            new PlayerStateTransition(StateType.Moving, CrouchMovingToMoving)
                        }
                    ));
        }

        // transition methods and dependencies ---------------

        private void IdleUpdate()
        {
            

            if (Mathf.Abs(pc.rb.velocity.x) > 0.1)
            {
                pc.rb.velocity = new Vector3(pc.SlowDownFactor * pc.rb.velocity.x, pc.rb.velocity.y, 0);
            }
        }

        private bool IdleToEmission()
        {
            if (InputController.GetReleaseEnergy(InputStateType.PRESSED) && pc.EnergyAmount > 0 && pc.currentStorage != null ) //last case is Temporary, Bait mechanic has beed removed until implemented
            {
                return true;
            }
                
            return false;
        }

        private bool IdleToAbsorption()
        {
            if (InputController.GetAbsorbEnergy(InputStateType.PRESSED)){

                if (pc.currentStorage != null)
                {
                    if (!pc.currentStorage.IsEmpty())
                    {
                        return true;
                    }
                }

                EnergyArea e = null;
                if (EnergyHandler.IsTransformInEnergyArea(pc.transform, out e))
                {
                    return true;
                }

            }

            return false;
        }

        private bool IdleToMoving()
        {
            if (InputController.GetHorizontalInput() != 0)
            {
                return true;
            }
            return false;
        }

        private bool IdleToJump()
        {
            return HasToJump();
        }

        private bool HasToJump()
        {
            if (InputController.GetJump(InputStateType.JUST_PRESSED) && pc.OnGround)
            {
                return true;
            }
            return false;
        }

        private bool IdleToCrouch()
        {
            return InputController.GetCrouch(InputStateType.PRESSED);
        }

        private void EmissionUpdate()
        {
            if (firstFrameOfStateChange)
            {
                timeStamp = Time.time;
                used = false;
                
            }
                
                if(Time.time-timeStamp >pc.emissionEmitTime && !used)
                {
                pc.PlayEmitSound();
                pc.SetEmissionSystem(true);

                if (pc.currentStorage == null)
                {
                    //CreateEnergy(); temporarelly disabled
                    used = true;
                    return;
                }
                else
                {
                    if (pc.currentStorage.HasSpace())
                    {
                        pc.currentStorage.AddEnergy();
                    }
                    else
                    {
                        used = true;
                        return;
                    }
                }

                pc.EnergyAmount -= 1;
                used = true;
            }
        }

        private void CreateEnergy()
        {
            EnergyArea ha;
            ha = EnergyHandler.CreateEnergyArea(pc.transform.position);
            ha.AddEnergy(true);
        }

        private bool EmissionToIdle()
        {
            if (Time.time-timeStamp>pc.emissionEndTime)
            {
                 pc.SetEmissionSystem(false);
                 return true;
            }
            return false;
        }

        private void AbsorptionUpdate()
        {

            if (firstFrameOfStateChange)
            {
                timeStamp = Time.time;
                used = false;
            }

            if (Time.time - timeStamp > pc.absorptionAbsorbTime && !used)
            {
                pc.PlayAbsorbSound();
                EnergyArea e = null;
                if (pc.currentStorage == null)
                {
                    EnergyHandler.IsTransformInEnergyArea(pc.transform, out e);
                    e.RemoveEnergy();
                }
                else
                {
                    pc.currentStorage.RemoveEnergy();
                }

                pc.EnergyAmount += 1;
                used = true;
            }
        }

        private bool AbsorptionToIdle()
        {
            if (Time.time - timeStamp > pc.absorbtionEndTime)
            {
                //Disable absortion PS
                return true;
            }
            return false;
        }

        private void MovingUpdate()
        {

            if (firstFrameOfStateChange)
            {
                pc.SetFeetLoudness(PlayerFootSoundEmission.StepType.Walk);
            }

            AddMovementForce();

            if (pc.rb.velocity.x > pc.MovementSpeedCap)
            {
                pc.rb.velocity = new Vector3(pc.MovementSpeedCap, pc.rb.velocity.y, 0);
            }else if(-pc.rb.velocity.x > pc.MovementSpeedCap)
            {
                pc.rb.velocity = new Vector3(-pc.MovementSpeedCap, pc.rb.velocity.y, 0);
            }


        }

        private void AddMovementForce()
        {
            float dir = InputController.GetHorizontalInput();
            pc.rb.AddForce(new Vector3(pc.MovementForce * dir, 0, 0));
        }

        private bool MovingToIdle()
        {
            if (InputController.GetHorizontalInput() == 0)
            {
                return true;
            }

            return false;
        }

        private bool MovingToJump()
        {
            return HasToJump();
        }

        private bool MovingToSprint()
        {
            if (InputController.GetSprint(InputStateType.PRESSED))
            {
                return true;
            }

            return false;
        }

        private bool MovingToCrouchMoving()
        {
            return IdleToCrouch();
        }

        private void SprintUpdate()
        {
            if (firstFrameOfStateChange)
            {
                pc.SetFeetLoudness(PlayerFootSoundEmission.StepType.Sprint);
            }

            SprintMoving();
        }

        private void SprintMoving()
        {
            float dir = InputController.GetHorizontalInput();
            AddMovementForce();
            if (pc.rb.velocity.x > pc.SprintSpeedCap)
            {
                pc.rb.velocity = new Vector3(pc.SprintSpeedCap, pc.rb.velocity.y, 0);
            }
            else if (-pc.rb.velocity.x > pc.SprintSpeedCap)
            {
                pc.rb.velocity = new Vector3(-pc.SprintSpeedCap, pc.rb.velocity.y, 0);
            }

            oldDirIsLeft = IsMovingLeft(dir);

        }

        private bool IsMovingLeft(float dir)
        {
            return dir > 0 ? false : true;
        }

        private bool SprintToMoving()
        {
            if (!InputController.GetSprint(InputStateType.PRESSED))
            {
                return true;
            }
            if (oldDirIsLeft!= IsMovingLeft(InputController.GetHorizontalInput()))
            {
                return true;
            }
            if(pc.rb.velocity.x == 0)
            {
                return true;
            }

            return false;
        }

        private bool SprintToJump()
        {
            return HasToJump();
        }

        private void JumpUpdate()
        {
            if (firstFrameOfStateChange)
            {
                Jump();
                pc.SetFeetLoudness(PlayerFootSoundEmission.StepType.Jump);

               pc.JumpGroundCollisionCheckCD = pc.JumpGroundCollisionCheckStartingCD;
            }

            if (pc.JumpGroundCollisionCheckCD > 0)
            {
                pc.JumpGroundCollisionCheckCD -= Time.deltaTime;
            }

            pc.OnGround = false;

            SprintMoving();

        }

        private void Jump()
        {
            pc.rb.AddForce(new Vector3(0, pc.JumpForce, 0));
        }

        private bool JumpToIdle()
        {
            return pc.OnGround && pc.JumpGroundCollisionCheckCD <= 0;
        }


        private void CrouchUpdate()
        {
            if (firstFrameOfStateChange)
            {
                pc.SetFeetLoudness(PlayerFootSoundEmission.StepType.Sneak);
            }

            IdleUpdate();
        }

        private bool CrouchToIdle()
        {
            return !InputController.GetCrouch(InputStateType.PRESSED);
        }
        private bool CrouchToCrouchMoving()
        {
            return IdleToMoving();
        }
        private bool CrouchToEmission()
        {
            return IdleToEmission();
        }

        private void CrouchMovingUpdate()
        {
            if (firstFrameOfStateChange)
            {
                pc.SetFeetLoudness(PlayerFootSoundEmission.StepType.Sneak);
            }

            AddMovementForce();

            if (pc.rb.velocity.x > pc.CrouchedMovementSpeedCap)
            {
                pc.rb.velocity = new Vector3(pc.CrouchedMovementSpeedCap, pc.rb.velocity.y, 0);
            }else if (-pc.rb.velocity.x > pc.CrouchedMovementSpeedCap)
            {
                pc.rb.velocity = new Vector3(-pc.CrouchedMovementSpeedCap, pc.rb.velocity.y, 0);
            }
        }

        private bool CrouchMovingToCrouch()
        {
            return MovingToIdle();
        }

        private bool CrouchMovingToMoving()
        {
            return !InputController.GetCrouch(InputStateType.PRESSED);
        }

    }

    /////////////////////////////////////////////////
    /// Enum containing all the possible states of the PlayerStateMachine.
    /////////////////////////////////////////////////
    public enum StateType
    {
        Idle,
        Moving,
        Crouch,
        CrouchMoving,
        Sprint,
        Jump,
        Emission,
        Absorption
    }


    /////////////////////////////////////////////////
    /// Used by the PlayerStateMachine, represents the Node.
    /////////////////////////////////////////////////
    public class PlayerState : State<StateType>
    {

        public PlayerState(StateType _type, System.Action _Update, PlayerStateTransition[] _Transitions) : base(_type, _Update, _Transitions) { }
        
    }

    /////////////////////////////////////////////////
    /// Used by the PlayerStateMachine, represents a Connection.
    /////////////////////////////////////////////////
    public class PlayerStateTransition : Transition<StateType>
    {
        public PlayerStateTransition(StateType _type, System.Func<bool> _transitionShouldOccur) : base(_type, _transitionShouldOccur) { }
    }
}


