namespace MattEland.FSharpGeneticAlgorithm.Logic

open MattEland.FSharpGeneticAlgorithm.Logic.World

module Cells =

  let printCell char isLastCell =
    if isLastCell then
      printfn "%c" char
    else
      printf "%c" char

  let displayWorld (world: World) =
    printfn ""

    for y in 1..world.MaxX do
    for x in 1..world.MaxY do   
      // Determine which character should exist in this line
      let char = world.GetCharacterAtCell(x,y)
      printCell char (x = world.MaxX)
