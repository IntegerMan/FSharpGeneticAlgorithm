namespace MattEland.FSharpGeneticAlgorithm.Logic

module WorldPos =

  type WorldPos = {X: int32; Y:int32}

  let newPos x y = {X = x; Y = y}

  let isAdjacentTo (posA: WorldPos) (posB: WorldPos): bool =
    let xDiff = abs (posA.X - posB.X)
    let yDiff = abs (posA.Y - posB.Y)
    let result = xDiff <= 1 && yDiff <= 1
    result

  let isSamePos p1 p2 = p1.X = p2.X && p1.Y = p2.Y