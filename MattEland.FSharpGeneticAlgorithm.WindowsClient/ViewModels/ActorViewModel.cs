using MattEland.FSharpGeneticAlgorithm.Logic;

namespace MattEland.FSharpGeneticAlgorithm.WindowsClient.ViewModels
{
    internal class ActorViewModel
    {
        private readonly Actors.Actor _actor;

        public ActorViewModel(Actors.Actor actor)
        {
            _actor = actor;
        }

        // Subtract 1 since my data's indexes start at 1 instead of 0
        public int PosX => (_actor.Pos.X - 1) * 10;
        public int PosY => (_actor.Pos.Y - 1) * 10;

        public string Text => Actors.getChar(_actor).ToString();

        public string ImagePath
        {
            get
            {
                if (_actor.ActorKind.Equals(Actors.ActorKind.Acorn))
                {
                    return "Images\\Acorn.png";
                }

                if (_actor.ActorKind.Equals(Actors.ActorKind.Doggo))
                {
                    return "Images\\Doggo.png";
                }

                if (_actor.ActorKind.Equals(Actors.ActorKind.Rabbit))
                {
                    return "Images\\Rabbit.png";
                }

                if (_actor.ActorKind.Equals(Actors.ActorKind.Tree))
                {
                    return "Images\\Tree.png";
                }

                return "Images\\SquirrelAcorn.png";
            }
        }
    }
}