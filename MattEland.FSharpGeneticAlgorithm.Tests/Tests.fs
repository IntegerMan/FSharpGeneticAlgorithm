module MattEland.FSharpGeneticAlgorithm.Tests

open System
open Xunit
open FsUnit
open MattEland.FSharpGeneticAlgorithm.Logic.WorldPos
open MattEland.FSharpGeneticAlgorithm.Logic.Actors
open MattEland.FSharpGeneticAlgorithm.Logic.World
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

[<Fact>]
let ``Rabbit should move randomly`` () =
  // Arrange
  let expectedPos = newPos 1 1
  let randomizer = new Random(42)
  let world: World = makeTestWorld false

  // Act
  let newWorld = simulateRabbit world randomizer

  // Assert
  newWorld.Rabbit.Pos |> should equal expectedPos



