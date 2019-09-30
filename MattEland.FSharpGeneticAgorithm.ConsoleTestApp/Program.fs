open System
open MattEland.FSharpGeneticAlgorithm.Logic.World
open MattEland.FSharpGeneticAlgorithm.Logic.Simulator
open MattEland.FSharpGeneticAlgorithm.ConsoleTestApp.Display

let generateWorld randomizer =
  new World(13, 13, randomizer)
  
[<EntryPoint>]
let main argv =
  printfn "F# Console Application Tutorial by Matt Eland"
  
  let randomizer = new Random()

  let mutable simulating: bool = true
  let mutable world = generateWorld(randomizer)

  while simulating do
    displayWorld world

    let player = world.Squirrel
    let key = getUserInput()
    
    Console.Clear()

    match key.Key with
    | ConsoleKey.LeftArrow -> 
      world <- moveActor world player -1 0
    | ConsoleKey.RightArrow -> 
      world <- moveActor world player 1 0
    | ConsoleKey.UpArrow -> 
      world <- moveActor world player 0 -1
    | ConsoleKey.DownArrow -> 
      world <- moveActor world player 0 1
    | ConsoleKey.NumPad7 | ConsoleKey.Home  -> 
      world <- moveActor world player -1 -1
    | ConsoleKey.NumPad9 | ConsoleKey.PageUp -> 
      world <- moveActor world player 1 -1
    | ConsoleKey.NumPad1 | ConsoleKey.End -> 
      world <- moveActor world player -1 1
    | ConsoleKey.NumPad3 | ConsoleKey.PageDown -> 
      world <- moveActor world player 1 1
    | ConsoleKey.NumPad5 | ConsoleKey.Spacebar | ConsoleKey.Clear -> 
      printfn "Time Passes..."
    | ConsoleKey.X -> simulating <- false
    | ConsoleKey.R -> world <- generateWorld(randomizer)
    | _ -> printfn "Invalid input '%c'" key.KeyChar

  0 // return an integer exit code