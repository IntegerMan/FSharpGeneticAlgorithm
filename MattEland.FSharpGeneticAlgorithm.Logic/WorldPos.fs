module MattEland.FSharpGeneticAlgorithm.Logic.WorldPos

type WorldPos = {X: int32; Y:int32}

let newPos x y = {X = x; Y = y}

let isAdjacentTo (posA: WorldPos) (posB: WorldPos): bool =
  let xDiff = abs (posA.X - posB.X)
  let yDiff = abs (posA.Y - posB.Y)
  let result = xDiff <= 1 && yDiff <= 1
  result

let getRandomPos(maxX:int32, maxY:int32, getRandom): WorldPos =
  let x = getRandom maxX
  let y = getRandom maxY
  newPos x y
