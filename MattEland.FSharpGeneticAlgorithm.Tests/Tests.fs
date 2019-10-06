module MattEland.FSharpGeneticAlgorithm.Tests

open Xunit
open MattEland.FSharpGeneticAlgorithm.Logic.WorldPos

[<Theory>]
[<InlineData(4, 2, 4, 1, true)>]
[<InlineData(4, 2, 4, 0, false)>]
let ``Point Adjaency Tests`` (x1, y1, x2, y2, expectedAdjacent) =
    // Arrange
    let pos1 = newPos x1 y1
    let pos2 = newPos x2 y2

    // Act
    let isAdjacent = isAdjacentTo pos1 pos2

    // Assert
    Assert.Equal(expectedAdjacent, isAdjacent)
