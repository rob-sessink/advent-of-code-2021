module HydroThermal

open System.IO
open System.Text.RegularExpressions

type Direction =
    | Horizontal
    | Vertical
    | Diagonal

type Coordinate =
    { X: int
      Y: int
      Marked: int }
    static member Create(x, y) = { X = x; Y = y; Marked = 0 }

    static member Create(points: int []) =
        match points with
        | c when c.Length = 2 ->
            { X = points.[0]
              Y = points.[1]
              Marked = 0 }
        | _ -> failwith $"Incorrect coordinate ${points}"

    member this.Mark() = { this with Marked = this.Marked + 1 }

    member private this.RouteFor(cc: Coordinate, ec: Coordinate, direction, velocity, acc) =
        match (cc, ec) with
        | s, e when s = e -> acc |> List.rev |> List.toArray
        | s, e ->
            let curr =
                match direction with
                | Horizontal -> { s with Y = s.Y + (fst velocity) }
                | Vertical -> { s with X = s.X + (snd velocity) }
                | Diagonal ->
                    { s with
                          X = s.X + (snd velocity)
                          Y = s.Y + (fst velocity) }

            this.RouteFor(curr, e, direction, velocity, (curr :: acc))

    member this.RouteFor(ec, direction, delta) =
        this.RouteFor(this, ec, direction, delta, [ this ])

type Line =
    { Start: Coordinate
      End: Coordinate }
    static member Create(coords: Coordinate []) =
        match coords with
        | c when c.Length = 2 -> { Start = coords.[0]; End = coords.[1] }
        | _ -> failwith $"Incorrect line ${coords}"

    member this.Direction() =
        match (this.Start, this.End) with
        | s, e when s.X = e.X && s.Y <> e.Y -> Horizontal
        | s, e when s.Y = e.Y && s.X <> e.X -> Vertical
        | s, e when s.X <> e.X && s.Y <> e.Y -> Diagonal
        | _ -> failwith "Invalid direction"

    member this.maxX() = max this.Start.X this.End.X

    member this.maxY() = max this.Start.Y this.End.Y

    member private this.Velocity() =
        (Operators.compare this.End.Y this.Start.Y), (Operators.compare this.End.X this.Start.X)

    member this.Draw() =
        this.Start.RouteFor(this.End, this.Direction(), this.Velocity())

    static member Dimension(lines: Line array) =
        let xd =
            lines
            |> Array.map (fun l -> l.maxX ())
            |> Array.max

        let yd =
            lines
            |> Array.map (fun l -> l.maxY ())
            |> Array.max

        (xd + 1, yd + 1)

type Map =
    { Map: Coordinate [,] }

    static member Create(lines: Line array) =
        let dim = Line.Dimension(lines)
        { Map = Array2D.init (fst dim) (snd dim) (fun x y -> Coordinate.Create(x, y)) }

    member this.DrawOn(lines: Line array) =
        let updateFor (map: Coordinate [,]) c =
            let upd = Array2D.get map c.X c.Y
            Array2D.set map c.X c.Y (upd.Mark())

        lines
        |> Array.collect (fun l -> l.Draw())
        |> Array.map (updateFor this.Map)
        |> ignore

    member this.Overlap(count) =
        this.Map
        |> Seq.cast<Coordinate>
        |> Seq.toArray
        |> Array.filter (fun p -> p.Marked >= count)
        |> Array.length

let parseLines lines =
    let parseCoordinate (coords: int array) =
        match coords with
        | c when c.Length = 2 -> (Coordinate.Create c)
        | _ -> failwith $"Incorrect coordinates ${coords}"

    let parseLine (line: string) =
        Regex.Split(line, " -> ")
        |> Array.map (fun cs -> cs.Split(',') |> Array.map int)
        |> Array.map parseCoordinate
        |> Line.Create

    lines |> Array.map parseLine

let all x = true
let nonDiagonal (x: Line) = x.Direction() <> Diagonal

let pathFinder filter file =
    let lines =
        File.ReadAllLines file
        |> parseLines
        |> Array.filter filter

    let map = lines |> Map.Create
    map.DrawOn(lines)
    map.Overlap(2)
