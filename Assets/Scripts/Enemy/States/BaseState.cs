public abstract class BaseState
{

    public Enemy enemy;
    public StateMachine stateMachine;

    public abstract void Enter();
    public abstract void Perfrom();
    public abstract void Exit();
}
