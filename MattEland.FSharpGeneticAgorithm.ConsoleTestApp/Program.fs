open System
open MattEland.FSharpGeneticAlgorithm.Logic.Simulator
open MattEland.FSharpGeneticAlgorithm.Logic.WorldGeneration
open MattEland.FSharpGeneticAlgorithm.Logic.Commands
open MattEland.FSharpGeneticAlgorithm.ConsoleTestApp.Display
  
type Command =
  | Action of GameCommand
  | Exit

let tryParseInput (info:ConsoleKeyInfo) =
    match info.Key with
    | ConsoleKey.LeftArrow -> Some (Action MoveLeft)
    | ConsoleKey.RightArrow -> Some (Action MoveRight)
    | ConsoleKey.UpArrow -> Some (Action MoveUp)
    | ConsoleKey.DownArrow -> Some (Action MoveDown)
    | ConsoleKey.NumPad7 | ConsoleKey.Home  -> Some (Action MoveUpLeft)
    | ConsoleKey.NumPad9 | ConsoleKey.PageUp -> Some (Action MoveUpRight)
    | ConsoleKey.NumPad1 | ConsoleKey.End -> Some (Action MoveDownLeft)
    | ConsoleKey.NumPad3 | ConsoleKey.PageDown -> Some (Action MoveDownRight)
    | ConsoleKey.NumPad5 | ConsoleKey.Spacebar | ConsoleKey.Clear -> Some (Action Wait) 
    | ConsoleKey.X -> Some Exit
    | ConsoleKey.R -> Some (Action Restart)
    | _ -> None

[<EntryPoint>]
let main argv =
  printfn "F# Console Application Tutorial by Matt Eland"
  
  let getRandomNumber =
    let r = Random()
    fun max -> (r.Next max) + 1

  let world = makeTestWorld false

  let mutable state = { World = world; SimState = SimulationState.Simulating; TurnsLeft=30}
  let mutable simulating: bool = true

  while simulating do
    let userCommand = getUserInput(state) |> tryParseInput

    match userCommand with
    | Some command -> 
      match command with 
      | Exit -> simulating <- false
      | Action gameCommand -> state <- playTurn state getRandomNumber gameCommand
    | None -> printfn "Invalid input"
    
  0 // return an integer exit code