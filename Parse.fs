namespace Tagger

open System.Text.RegularExpressions
open System.Reflection

open CommandLine;
open TagLib.Id3v1

module Parse =

    [<Verb("parse", HelpText = "ToDo parse")>]
    type Verb = {
        [<Option('f', "file", Required = true, HelpText = "Input files.")>] File : string;
        [<Option('r', "regex", Required = true, HelpText = "Input files.")>] Regex : string;
        [<Option('s', "source", Required = true, HelpText = "Input files.")>] SourceTag : string;
        [<Option('t', "target", Required = true, HelpText = "Input files.")>] TargetTag : seq<string>;
    }

    let (|Regex|_|) pattern input =
            let m = Regex.Match(input, pattern)
            if m.Success then Some(List.tail [ for g in m.Groups -> g.Value ])
            else None

//(.*)\/(.*)

    let GetTagByName tagFile tagName =
        let property = tagFile.GetType().GetProperty tagName
        property.GetValue tagFile

    let GetTagPropertyByName tag name =
        let property = tag.GetType().GetProperty name
        match property with
        | null -> None
        | x -> Some(x)

    let SetValue target ((value:string), (tag:Option<PropertyInfo>)) =
        match tag with
        | Some t ->
            match t.PropertyType with
            | x when x = typedefof<string> -> t.SetValue (target, value)
            | x when x = typedefof<string[]> -> t.SetValue (target, [|value|])
            | _ -> failwith "Unsupported type."
        | None -> failwith "Unsupported tag."

    let Action verb =
        let tagFile = TagLib.File.Create(verb.File)
        let source = GetTagByName tagFile.Tag verb.SourceTag :?> string
        let m = Regex.Match(source, verb.Regex)
        let r = if m.Success then Some(List.tail [ for g in m.Groups -> g.Value ]) else None
        match r with
        | Some l -> 
            l |> List.iter ( fun i -> printfn "%s" i )
            let setters = verb.TargetTag |> Seq.map (GetTagPropertyByName tagFile.Tag) |> Seq.toList
            List.zip l setters |> List.iter (SetValue tagFile.Tag)
        | _ -> 
            printfn "regex did not match"
        tagFile.Save ()
        ()