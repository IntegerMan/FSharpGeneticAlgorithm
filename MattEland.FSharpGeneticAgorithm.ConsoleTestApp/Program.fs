open System
open MattEland.FSharpGeneticAlgorithm.Logic.World
open MattEland.FSharpGeneticAlgorithm.Logic.Cells
    
[<EntryPoint>]
let main argv =
  printfn "F# Console Application Example by Matt Eland"
  printfn ""

  let randomizer = new Random()

  let mutable simulating: bool = true
  let mutable world = new World(8, 8, randomizer)

  while simulating do
    displayWorld world
  
    printfn "Press R to regenerate or X to exit"
    printfn ""

    let key = Console.ReadKey(true)
    Console.Clear()

    match key.Key with
    | ConsoleKey.X -> simulating <- false

    | ConsoleKey.R -> 
      printfn "Regenerating World"
      world <- new World(8, 8, randomizer)
      
    | _ -> printfn "Invalid input '%c'" key.KeyChar

  0 // return an integer exit code