using Events;

namespace Monolith;

public class CopyPlayer : IPlayer
{
    private const string PlayerId = "The Copy Cat";
    private readonly Queue<Move> _previousMoves = new Queue<Move>();

    public PlayerMovedEvent MakeMove(GameStartedEvent e)
    {
        using var moveActivity = Program.ActivitySource.StartActivity();

        Move move = Move.Paper;
        if (_previousMoves.Count > 2)
        {
            move = _previousMoves.Dequeue();
        }

        var moveEvent = new PlayerMovedEvent
        {
            GameId = e.GameId,
            PlayerId = PlayerId,
            Move = move
        };
        
        Program.Logger1.Verbose("{0} made the following move: {1}", PlayerId, move );
        moveActivity.AddTag("Move.", move);
        moveActivity.AddTag("PlayerId", PlayerId);
        
        return moveEvent;
    }

    public void ReceiveResult(GameFinishedEvent e)
    {
        var otherMove = e.Moves.SingleOrDefault(m => m.Key != PlayerId).Value;
        _previousMoves.Enqueue(otherMove);
    }
}