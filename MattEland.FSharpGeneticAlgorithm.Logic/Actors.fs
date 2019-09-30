namespace MattEland.FSharpGeneticAlgorithm.Logic

open MattEland.FSharpGeneticAlgorithm.Logic.WorldPos

module Actors =

  [<AbstractClass>]
  type Actor(pos: WorldPos) =
    member this.Pos = pos
    abstract member Character: char

  type Squirrel(pos: WorldPos, hasAcorn: bool) =
    inherit Actor(pos)
    member this.HasAcorn = hasAcorn
    override this.Character = 'S'

  let createSquirrel pos = new Squirrel(pos, false)
  
  type Tree(pos: WorldPos) =
    inherit Actor(pos)
    override this.Character = 't'

  let createTree pos = new Tree(pos)

  type Acorn(pos: WorldPos) =
    inherit Actor(pos)
    override this.Character = 'a'

  let createAcorn pos = new Acorn(pos)

  type Rabbit(pos: WorldPos) =
    inherit Actor(pos)
    override this.Character = 'R'

  let createRabbit pos = new Rabbit(pos)

  type Doggo(pos: WorldPos) =
    inherit Actor(pos)
    override this.Character = 'D'

  let createDoggo pos = new Doggo(pos)
