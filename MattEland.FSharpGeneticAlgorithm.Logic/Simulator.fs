module MattEland.FSharpGeneticAlgorithm.Logic.Simulator

open MattEland.FSharpGeneticAlgorithm.Logic.WorldPos
open MattEland.FSharpGeneticAlgorithm.Logic.World
open MattEland.FSharpGeneticAlgorithm.Logic.Actors
open MattEland.FSharpGeneticAlgorithm.Logic.Commands
open MattEland.FSharpGeneticAlgorithm.Logic.WorldGeneration

type SimulationState = Simulating | Won | Lost

type GameState = { World : World; SimState: SimulationState }

let canEnterActorCell actor target =
  match target with
  | Rabbit | Squirrel _ -> actor = Doggo // Dog can eat the squirrel or rabbit
  | Doggo _ -> false // Nobody bugs the dog
  | Tree _ -> actor = Squirrel true // Only allow if squirrel has an acorn
  | Acorn _ -> actor = Squirrel false // Only allow if squirrel w/o acorn

let moveActor state actor pos = 
  let world = state.World
  let performMove =
    let actor = { actor with Pos = pos }
    match actor.ActorKind with
    | Squirrel _ -> { state with World={world with Squirrel = actor }}
    | Tree -> { state with World={world with Tree = actor }}
    | Acorn -> { state with World={world with Acorn = actor }}
    | Rabbit -> { state with World={world with Rabbit = actor }}
    | Doggo -> { state with World={world with Doggo = actor }}

  let target = tryGetActor(pos.X, pos.Y) world

  match target with
  | None -> performMove
  | Some otherActor ->
    if otherActor <> actor && canEnterActorCell actor.ActorKind otherActor.ActorKind then 

      match actor.ActorKind with 
      | Squirrel hasAcorn -> 
        if not hasAcorn && otherActor.ActorKind = Acorn then
          {state with World =
            { 
              world with 
              Squirrel = {ActorKind = Squirrel true; Pos = pos; IsActive = true} 
              Acorn = {world.Acorn with IsActive = false}
            }
          }
        else if hasAcorn && otherActor.ActorKind = Tree then
          {
            state with SimState = Won; World = { 
              world with Squirrel = {ActorKind = Squirrel true; Pos = pos; IsActive = true} 
            }
          }
        else
          performMove
      | _ -> performMove
    else
      state

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

let moveRandomly state actor getRandomNumber =
  let current = actor.Pos
  let movedPos = getCandidates(current, state.World, false) 
                 |> Seq.sortBy(fun _ -> getRandomNumber 1000)
                 |> Seq.head

  moveActor state actor movedPos

let simulateActors (state: GameState) getRandomNumber =
  let mutable endState = state

  endState <- moveRandomly endState endState.World.Rabbit getRandomNumber

  endState

let handlePlayerCommand state command =
  let player = state.World.Squirrel
  let xDelta =
    match command with
    | MoveLeft | MoveDownLeft | MoveUpLeft -> -1
    | MoveRight | MoveDownRight | MoveUpRight -> 1
    | _ -> 0
  let yDelta =
    match command with
    | MoveUpLeft | MoveUp | MoveUpRight -> -1
    | MoveDownLeft | MoveDown | MoveDownRight -> 1
    | _ -> 0

  let movedPos = {X=player.Pos.X + xDelta; Y=player.Pos.Y + yDelta}

  if isValidPos movedPos state.World then
    moveActor state player movedPos
  else
    state
    
let playTurn state getRandomNumber command =
  let world = state.World
  match command with 
  | Restart -> { World = makeWorld world.MaxX world.MaxY getRandomNumber; SimState = Simulating }
  | _ -> 
    match state.SimState with
    | Simulating -> 
      let newState = handlePlayerCommand state command 
      simulateActors newState getRandomNumber
    | _ -> state
