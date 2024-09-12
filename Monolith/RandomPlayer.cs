using Events;

namespace Monolith;

public class RandomPlayer : IPlayer
{
    private const string PlayerId = "Mr. Random";

    public PlayerMovedEvent MakeMove(GameStartedEvent e)
    {   
        using var moveRndPly = Program.ActivitySource.StartActivity();
        var random = new Random();
        var next = random.Next(3);
        var move = next switch
        {
            0 => Move.Rock,
            1 => Move.Paper,
            _ => Move.Scissor
        };
        Program.Log.Verbose("{0} made the following move: {1}", PlayerId, move );
        moveRndPly.AddTag("Move.", move);
        moveRndPly.AddTag("PlayerId", PlayerId);
        return new PlayerMovedEvent
        {
            GameId = e.GameId,
            PlayerId = PlayerId,
            Move = move
        };
    }

    public void ReceiveResult(GameFinishedEvent e)
    { }
}

