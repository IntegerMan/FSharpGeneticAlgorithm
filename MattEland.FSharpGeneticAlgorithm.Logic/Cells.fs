namespace MattEland.FSharpGeneticAlgorithm.Logic

module Cells =

  let printCell char isLastCell =
    if isLastCell then
      printfn "%c" char
    else
      printf "%c" char

