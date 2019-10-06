module MattEland.FSharpGeneticAlgorithm.ConsoleTestApp.Display

open System
open MattEland.FSharpGeneticAlgorithm.Logic.World

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
    let char = world |> getCharacterAtCell(x,y)
    printCell char (x = world.MaxX)

let getUserInput(world: World): ConsoleKeyInfo =
  displayWorld world
  printfn ""
  printfn "Press Arrow Keys to move, R to regenerate, or X to exit"

  let key = Console.ReadKey(true)
  Console.Clear()
  key
