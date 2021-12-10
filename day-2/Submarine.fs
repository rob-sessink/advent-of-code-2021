module Submarine

open System.IO

type Instruction =
    | Forward of int
    | Down of int
    | Up of int

    static member Create(ins, pos) =
        match ins, (int pos) with
        | "forward", p -> Forward p
        | "down", p -> Down p
        | "up", p -> Up p
        | _ -> failwith "Invalid instruction: {ins}"

type Position =
    { Horizontal: int
      Depth: int
      Aim: int }

    member this.ChangeFor(ins: Instruction) =
        match ins with
        | Forward p ->
            { this with
                  Horizontal = this.Horizontal + p
                  Depth = this.Depth + (this.Aim * p) }
        | Down p -> { this with Aim = this.Aim + p }
        | Up p -> { this with Aim = this.Aim - p }

    member this.Coord() = this.Horizontal * this.Depth

let execute (current: Position) (course: Instruction list) =
    course
    |> List.fold (fun (ip: Position) -> ip.ChangeFor) current

let asInstruction (instructionLine: string) =
    let ins = instructionLine.Split(" ")
    Instruction.Create(ins.[0], ins.[1])

let toCourse lines = lines |> List.map asInstruction

let toList file = File.ReadLines file |> Seq.toList

let navigate file =
    let startPosition = { Horizontal = 0; Depth = 0; Aim = 0 }

    let endPosition =
        toList file |> toCourse |> execute startPosition

    endPosition.Coord()
