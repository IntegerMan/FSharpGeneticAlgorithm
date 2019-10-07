module MattEland.FSharpGeneticAlgorithm.Logic.Simulator

open MattEland.FSharpGeneticAlgorithm.Logic.WorldPos
open MattEland.FSharpGeneticAlgorithm.Logic.World
open MattEland.FSharpGeneticAlgorithm.Logic.Actors
open MattEland.FSharpGeneticAlgorithm.Logic.WorldGeneration
open MattEland.FSharpGeneticAlgorithm.Logic.Commands

type GameState = { World : World; Player : Actor }

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

let simulateRabbit (world:World) getRandomNumber: World =
  let movedPos = newPos 1 1
  let newRabbit = {world.Rabbit with Pos = movedPos}
  { world with Rabbit = newRabbit }

let playTurn state player getRandomNumber command =
  let world = state.World
  match command with 
  | MoveLeft -> { state with World = moveActor world player -1 0 }
  | MoveRight -> { state with World = moveActor world player 1 0 } 
  | MoveUp -> { state with World = moveActor world player 0 -1 } 
  | MoveDown -> { state with World = moveActor world player 0 1 }
  | MoveUpLeft  -> { state with World = moveActor world player -1 -1 }
  | MoveUpRight -> { state with World = moveActor world player 1 -1 }
  | MoveDownLeft -> { state with World = moveActor world player -1 1 } 
  | MoveDownRight -> { state with World = moveActor world player 1 1 }
  | Wait ->
    printfn "Time Passes..."
    state
  | Restart ->
    let world = makeWorld 13 13 getRandomNumber
    { World = world; Player = world.Squirrel }