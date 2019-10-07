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
    printf "  " // Indent slightly so it is a bit more readable

    for x in 1..world.MaxY do   
      // Determine which character should exist in this line
      let char = world |> getCharacterAtCell(x,y)

      // Let's set the overall color
      match char with
      | '.' -> Console.ForegroundColor <- ConsoleColor.DarkGreen
      | 't' -> Console.ForegroundColor <- ConsoleColor.Green
      | 'a' -> Console.ForegroundColor <- ConsoleColor.DarkYellow
      | 'S' -> Console.ForegroundColor <- ConsoleColor.Yellow
      | 'D' -> Console.ForegroundColor <- ConsoleColor.Red
      | 'R' -> Console.ForegroundColor <- ConsoleColor.Magenta
      | _ -> Console.ForegroundColor <- ConsoleColor.Gray 

      printCell char (x = world.MaxX)

  // Ensure we go back to standard format
  Console.ForegroundColor <- ConsoleColor.White

let getUserInput(world: World): ConsoleKeyInfo =
  displayWorld world
  printfn ""
  printfn "Press Arrow Keys to move, R to regenerate, or X to exit"

  let key = Console.ReadKey(true)
  Console.Clear()
  key
