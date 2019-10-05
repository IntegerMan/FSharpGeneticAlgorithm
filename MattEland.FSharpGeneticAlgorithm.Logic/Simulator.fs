module MattEland.FSharpGeneticAlgorithm.Logic.Simulator

open MattEland.FSharpGeneticAlgorithm.Logic.WorldPos
open MattEland.FSharpGeneticAlgorithm.Logic.World
open MattEland.FSharpGeneticAlgorithm.Logic.Actors

let isValidPos pos (world: World): bool = 
  pos.X >= 1 && pos.Y >= 1 && pos.X <= world.MaxX && pos.Y <= world.MaxY

let hasObstacle pos (world: World) : bool =
  world.Actors
  |> Seq.exists(fun actor -> pos = actor.Pos)

let moveActor world actor xDiff yDiff = 
  let pos = newPos (actor.Pos.X + xDiff) (actor.Pos.Y + yDiff)

  if (isValidPos pos world) && not (hasObstacle pos world) then
    let actor = { actor with Pos = pos }
    match actor.ActorKind with
    | Squirrel _ -> { world with Squirrel = actor }
    | Tree -> { world with Tree = actor }
    | Acorn -> { world with Acorn = actor }
    | Rabbit -> { world with Rabbit = actor }
    | Doggo -> { world with Doggo = actor }
  else
    world

// TODO: I'll need a way of simulating an actor's turn