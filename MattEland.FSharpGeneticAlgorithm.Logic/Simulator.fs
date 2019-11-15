module MattEland.FSharpGeneticAlgorithm.Logic.Simulator

open MattEland.FSharpGeneticAlgorithm.Logic.WorldPos
open MattEland.FSharpGeneticAlgorithm.Logic.World
open MattEland.FSharpGeneticAlgorithm.Logic.Actors
open MattEland.FSharpGeneticAlgorithm.Logic.States
open MattEland.FSharpGeneticAlgorithm.Logic.Fitness
open MattEland.FSharpGeneticAlgorithm.Logic.WorldGeneration
open MattEland.FSharpGeneticAlgorithm.Genetics.Genes

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

  let handleDogMove state otherActor =
    if otherActor.ActorKind = Rabbit then
      {
        state with World = {world with
        Rabbit = {world.Rabbit with IsActive = false}
        Doggo = {world.Doggo with Pos = pos}
      }}
    else
      {
        state with SimState = SimulationState.Lost; World = {world with
        Squirrel = {world.Squirrel with IsActive = false}
        Doggo = {world.Doggo with Pos = pos}
      }}

  let handleSquirrelMove otherActor hasAcorn =
    if not hasAcorn && otherActor.ActorKind = Acorn && otherActor.IsActive then
      // Moving to the acorn for the first time should give the squirrel the acorn
      {state with World =
        { 
          world with 
          Squirrel = {ActorKind = Squirrel true; Pos = pos; IsActive = true} 
          Acorn = {world.Acorn with IsActive = false}
        }
      }
    else if hasAcorn && otherActor.ActorKind = Tree then
      // Moving to the tree with the acorn - this should win the game
      {
        state with SimState = SimulationState.Won; World = { 
          world with Squirrel = {ActorKind = Squirrel true; Pos = pos; IsActive = true} 
        }
      }
    else
      performMove

  let target = tryGetActor(pos.X, pos.Y) world

  match target with
  | None -> performMove
  | Some otherActor ->
    if otherActor <> actor && canEnterActorCell actor.ActorKind otherActor.ActorKind then 
      match actor.ActorKind with 
      | Doggo -> handleDogMove state otherActor
      | Squirrel hasAcorn -> handleSquirrelMove otherActor hasAcorn
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

let simulateDoggo state =
  let doggo = state.World.Doggo
  let rabbit = state.World.Rabbit
  let squirrel = state.World.Squirrel

  // Eat any adjacent actor
  if rabbit.IsActive && isAdjacentTo doggo.Pos rabbit.Pos then
    moveActor state doggo rabbit.Pos
  else if squirrel.IsActive && isAdjacentTo doggo.Pos squirrel.Pos then
    moveActor state doggo squirrel.Pos
  else
    state

let decreaseTimer (state: GameState) =
  if state.SimState = SimulationState.Simulating then
    if state.TurnsLeft > 0 then
      {state with TurnsLeft = state.TurnsLeft - 1}
    else
      {state with TurnsLeft = 0; SimState = SimulationState.Lost}
  else
    state

let simulateActors (state: GameState, getRandomNumber) =
  moveRandomly state state.World.Rabbit getRandomNumber 
  |> simulateDoggo
  |> decreaseTimer

let handleChromosomeMove random chromosome state =
  if state.SimState = SimulationState.Simulating then
    let current = state.World.Squirrel.Pos
    let movedPos = getCandidates(current, state.World, true) 
                   |> Seq.sortBy(fun pos -> evaluateTile chromosome state.World pos random)
                   |> Seq.head
    let newState = moveActor state state.World.Squirrel movedPos
    simulateActors(newState, random.Next)
  else
    state

let buildStartingStateForWorld world =
  { World = world; SimState = SimulationState.Simulating; TurnsLeft = 80}

let buildStartingState (random: System.Random) = 
  makeWorld 15 15 random.Next |> buildStartingStateForWorld

let simulateIndividualGame random brain fitnessFunction world: IndividualWorldResult =
  let gameStates = ResizeArray<GameState>()
  gameStates.Add(world)
  let mutable currentState = world
  while currentState.SimState = SimulationState.Simulating do
    currentState <- handleChromosomeMove random brain currentState
    gameStates.Add(currentState)
  {
    score = evaluateFitness(gameStates.ToArray(), fitnessFunction)
    states = gameStates.ToArray();
  }

let simulateGame random brain fitnessFunction states =
  let results: IndividualWorldResult seq = Seq.map (fun world -> simulateIndividualGame random brain fitnessFunction world) states
  {
    totalScore = Seq.map (fun e -> e.score) results |> Seq.sum
    results = Seq.toArray results
    brain = {brain with age = brain.age + 1}
  }

let simulate brain worlds =
  let states = Seq.map (fun w -> buildStartingStateForWorld w) worlds
  let random = new System.Random(42)
  simulateGame random brain standardFitnessFunction states
