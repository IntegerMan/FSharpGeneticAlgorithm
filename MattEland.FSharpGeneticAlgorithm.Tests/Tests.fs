module MattEland.FSharpGeneticAlgorithm.Tests

open Xunit
open FsUnit
open MattEland.FSharpGeneticAlgorithm.Logic.WorldPos
open MattEland.FSharpGeneticAlgorithm.Logic.Actors
open MattEland.FSharpGeneticAlgorithm.Logic.World
open MattEland.FSharpGeneticAlgorithm.Logic.States
open MattEland.FSharpGeneticAlgorithm.Logic.WorldGeneration
open MattEland.FSharpGeneticAlgorithm.Logic.Simulator

[<Theory>]
[<InlineData(4, 2, 4, 1, true)>]
[<InlineData(4, 2, 4, 0, false)>]
let ``Point Adjaency Tests`` (x1, y1, x2, y2, expectedAdjacent) =
    // Arrange
    let pos1 = newPos x1 y1
    let pos2 = newPos x2 y2

    // Act & Assert
    isAdjacentTo pos1 pos2 |> should equal expectedAdjacent

let getRandomNumber (max: int): int = 42
let buildTestState = {World=(makeTestWorld false); SimState=SimulationState.Simulating; TurnsLeft = 30}

[<Fact>]
let ``Rabbit should move randomly`` () =
  // Arrange
  let state = buildTestState
  let originalPos = state.World.Rabbit.Pos

  // Act
  let newWorld = simulateActors(state, getRandomNumber)

  // Assert
  newWorld.World.Rabbit.Pos |> should not' (equal originalPos)
  
[<Fact>]
let ``Dog Should Eat Squirrel If Adjacent`` () =
  // Arrange
  let customSquirrel = {Pos=newPos 3 6; ActorKind = Squirrel false; IsActive = true}
  let testState = buildTestState
  let state: GameState = {testState with World = {testState.World with Squirrel = customSquirrel}}
    
  // Act
  let newState = simulateActors(state, getRandomNumber)
  
  // Assert
  newState.World.Doggo.Pos |> should equal newState.World.Squirrel.Pos
  newState.World.Squirrel.IsActive |> should equal false
  newState.SimState |> should equal SimulationState.Lost
  
[<Fact>]
let ``Simulating actors should decrease the turns left counter`` () =   
  // Arrange
  let initialState = buildTestState

  // Act
  let newState = simulateActors(initialState, getRandomNumber)

  // Assert
  newState.TurnsLeft |> should equal (initialState.TurnsLeft - 1)

[<Fact>]
let ``Running out of turns should lose the simulation`` () =
  // Arrange
  let state: GameState = {buildTestState with TurnsLeft = 0}
    
  // Act
  let newState = simulateActors(state, getRandomNumber)
  
  // Assert
  newState.SimState |> should equal SimulationState.Lost