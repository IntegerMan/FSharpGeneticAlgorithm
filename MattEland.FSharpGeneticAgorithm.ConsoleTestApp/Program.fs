open System
open MattEland.FSharpGeneticAlgorithm.Logic.Actors
open MattEland.FSharpGeneticAlgorithm.Logic.World
open MattEland.FSharpGeneticAlgorithm.Logic.Simulator
open MattEland.FSharpGeneticAlgorithm.ConsoleTestApp.Display
  
type GameCommand =
  | MoveLeft | MoveRight
  | MoveUp | MoveDown
  | MoveUpLeft | MoveUpRight
  | MoveDownLeft | MoveDownRight
  | Wait
  | Restart

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
    | ConsoleKey.NumPad1 | ConsoleKey.End -> Some (Action MoveDownRight)
    | ConsoleKey.NumPad3 | ConsoleKey.PageDown -> Some (Action MoveDownRight)
    | ConsoleKey.NumPad5 | ConsoleKey.Spacebar | ConsoleKey.Clear -> Some (Action Wait) 
    | ConsoleKey.X -> Some Exit
    | ConsoleKey.R -> Some (Action Restart)
    | _ -> None

type GameState = { World : World; Player : Actor }

[<EntryPoint>]
let main argv =
  printfn "F# Console Application Tutorial by Matt Eland"
  
  let getRandomNumber =
    let r = Random()
    fun max -> (r.Next max) + 1

  let endState =
    let world = makeWorld 13 13 getRandomNumber
    let player = world.Squirrel
    let state = { World = world; Player = world.Squirrel }

    let playTurn state command =
      match command with 
      | MoveLeft -> { state with World = moveActor world player -1 0 }
      | MoveRight -> { state with World = moveActor world player 1 0 } 
      | MoveUp -> { state with World = moveActor world player 0 -1 } 
      | MoveDown -> { state with World = moveActor world player 0 1 }
      | MoveUpLeft  -> { state with World = moveActor world player -1 -1 }
      | MoveUpRight -> { state with World = moveActor world player 1 -1 }
      | MoveDownLeft -> { state with World = moveActor world player -1 1 } 
      | MoveDownRight -> { state with World = moveActor world player 1 1 }
      | Wait ->
        printfn "Time Passes..."
        state
      | Restart ->
        let world = makeWorld 13 13 getRandomNumber
        { World = world; Player = world.Squirrel }

    Seq.initInfinite(fun _ -> getUserInput())
    |> Seq.choose tryParseInput
    |> Seq.takeWhile (function | Exit -> false | _ -> true)
    |> Seq.choose(function | Exit -> None | Action gameCommand -> Some gameCommand)
    |> Seq.fold playTurn state

  0 // return an integer exit code