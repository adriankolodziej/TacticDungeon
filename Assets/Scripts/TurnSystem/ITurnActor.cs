using UnityEngine;

public interface ITurnActor
{
    protected int id { get; set; }
    protected string actorName { get; set; }
    protected int initiative { get; set; }
    protected SActor actorData { get; set; }

    protected abstract void TakeTurn();
}
