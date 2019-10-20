module MattEland.FSharpGeneticAlgorithm.Tests

open System
open Xunit
open FsUnit
open MattEland.FSharpGeneticAlgorithm.Logic.WorldPos
open MattEland.FSharpGeneticAlgorithm.Logic.Actors
open MattEland.FSharpGeneticAlgorithm.Logic.World
open MattEland.FSharpGeneticAlgorithm.Logic.WorldGeneration
open MattEland.FSharpGeneticAlgorithm.Logic.Simulator
open MattEland.FSharpGeneticAlgorithm.Logic.Commands

[<Theory>]
[<InlineData(4, 2, 4, 1, true)>]
[<InlineData(4, 2, 4, 0, false)>]
let ``Point Adjaency Tests`` (x1, y1, x2, y2, expectedAdjacent) =
    // Arrange
    let pos1 = newPos x1 y1
    let pos2 = newPos x2 y2

    // Act & Assert
    isAdjacentTo pos1 pos2 |> should equal expectedAdjacent

[<Fact>]
let ``Rabbit should move randomly`` () =
  // Arrange
  let randomizer = new Random(42)
  let world: World = makeTestWorld false
  let state: GameState = {World=world; SimState=Simulating}
  let originalPos = state.World.Rabbit.Pos

  // Act
  let newWorld = simulateActors state randomizer.Next

  // Assert
  newWorld.World.Rabbit.Pos |> should not' (equal originalPos)
  
[<Fact>]
let ``Squirrel Getting Acorn Should Change how it Displays`` () =
  // Arrange
  let customSquirrel = {Pos=newPos 6 7; ActorKind = Squirrel false; IsActive = true}
  let world: World = {(makeTestWorld false) with Squirrel = customSquirrel}
  let state: GameState = {World=world; SimState=Simulating}
  let command: GameCommand = MoveLeft
  
  // Act
  let newState = handlePlayerCommand state command

  // Assert
  newState.World.Squirrel.ActorKind |> should equal (Squirrel true)

[<Fact>]
let ``Squirrel Getting Acorn to Tree Should Win Game`` () =
  // Arrange
  let customSquirrel = {Pos=newPos 9 10; ActorKind = Squirrel true; IsActive = true}
  let world: World = {(makeTestWorld false) with Squirrel = customSquirrel}
  let state: GameState = {World=world; SimState=Simulating}
  let command: GameCommand = MoveLeft
  
  // Act
  let newState = handlePlayerCommand state command

  // Assert
  newState.World.Squirrel.Pos |> should equal world.Tree.Pos
  newState.SimState |> should equal Won
