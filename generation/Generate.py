from glob import glob
from os.path import splitext, split
from os import system




def FindToDos(FindPath, FileTypes):
    TODOS = [];
    for path in glob(FindPath):
        for filetype in FileTypes:
            if(splitext(path)[1][1:] == filetype):
                LineNumber=1;
                for line in open(path, "r").read().split("\n"):    
                    Status = line.find("TODO:");
                    if(Status != -1):
                        TODOS.append("  - {}: Line {}: {}\n".format(split(path)[1], LineNumber, line[Status+len("TODO:"):].strip()));
                    LineNumber += 1;

    return TODOS;


def ToDoList():
    TODOS = ["- SteamBot TODOs:"+"\n"]
    TODOS += FindToDos("..\src\*", ["cs"]);
    
    
    return '\n'.join(TODOS);

def ChangeLog():
    return open("changelog.md", "r").read();

def Main():
    print("Started!");
    ReplaceControllers = dict();
    ReplaceControllers["$TODO_LIST"] = ToDoList;
    ReplaceControllers["$CHANGE_LOG"] = ChangeLog;
    
    Template = open("template.md", "r").read();
    for replacestring in ReplaceControllers.keys():
        Status = Template.find(replacestring);
        if(Status != -1):
            Result = ReplaceControllers[replacestring]();
            Template = Template.replace(replacestring, Result);
    open("../readme.md", "w").write(Template);

    print("Done!");
    system("PAUSE");


Main();
