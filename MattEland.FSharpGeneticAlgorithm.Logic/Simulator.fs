module MattEland.FSharpGeneticAlgorithm.Logic.Simulator

open MattEland.FSharpGeneticAlgorithm.Logic.WorldPos
open MattEland.FSharpGeneticAlgorithm.Logic.World
open MattEland.FSharpGeneticAlgorithm.Logic.Actors
open MattEland.FSharpGeneticAlgorithm.Logic.WorldGeneration
open MattEland.FSharpGeneticAlgorithm.Logic.Commands

type GameState = { World : World; Player : ActorKind }

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

let getCandidates (current: WorldPos, world: World, includeCenter: bool): WorldPos seq =
  let mutable candidates: WorldPos seq = Seq.empty
  for x in -1 .. 1 do
    for y in -1 .. 1 do
      if (includeCenter || x <> y || x <> 0) then
        // Make sure we're in the world boundaries
        let candidatePos = {X=current.X + x; Y=current.Y + y}
        if isValidPos candidatePos world then
          candidates <- Seq.append candidates [candidatePos]
  candidates

let simulateRabbit (world:World) getRandomNumber: World =
  let current = world.Rabbit.Pos
  let movedPos = getCandidates(current, world, false) 
                 |> Seq.sortBy(fun _ -> getRandomNumber 1000)
                 |> Seq.head
  { world with Rabbit = {world.Rabbit with Pos = movedPos}}

let simulateActors (state: GameState) getRandomNumber: World =
  simulateRabbit state.World getRandomNumber

let handlePlayerCommand state command =
  let world = state.World
  let player = state.World.Squirrel
  match command with 
  | MoveLeft -> { state with World = moveActor world player -1 0 }
  | MoveRight -> { state with World = moveActor world player 1 0 } 
  | MoveUp -> { state with World = moveActor world player 0 -1 } 
  | MoveDown -> { state with World = moveActor world player 0 1 }
  | MoveUpLeft  -> { state with World = moveActor world player -1 -1 }
  | MoveUpRight -> { state with World = moveActor world player 1 -1 }
  | MoveDownLeft -> { state with World = moveActor world player -1 1 } 
  | MoveDownRight -> { state with World = moveActor world player 1 1 }
  | _ -> state
    
let playTurn state getRandomNumber command =
  let world = state.World
  let newWorld = 
    match command with 
    | Restart -> makeWorld world.MaxX world.MaxY getRandomNumber
    | _ -> 
      let newState = handlePlayerCommand state command 
      simulateActors newState getRandomNumber

  { state with World = newWorld }
