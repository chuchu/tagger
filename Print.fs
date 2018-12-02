namespace Tagger

open System.IO;
open CommandLine;

module Print =

    [<Verb("print", HelpText = "ToDo parse")>]
    type Verb = {
        [<Option('f', "file", Required = true, HelpText = "Input files.")>] file : string;
    }

    let printTags (file:string) =
        if ( File.Exists file ) then 
            let tag = TagLib.File.Create(file)
            printfn "Title: %s" tag.Tag.Title
        else 
            sprintf "The file '%s' does not exist." file |> failwith

    let Action verb =
        printTags verb.file
        ()